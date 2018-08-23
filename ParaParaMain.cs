/* デジカメ用画像ビュアー */

// ver0.62 2018.8.23 Thumb scroll
// ver0.60 2018.8.20 github
// ver0.50 2018.8.2 主要機能実装完了
// ver0.00 2018.7.1 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
using System.IO;
using System.Reflection;
using System.Diagnostics;
//using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Collections.Concurrent;
using System.Threading;
//using System.Windows.Media.Imaging;
using RotateFlipExtensions;

namespace ParaParaView
{
    public partial class ParaParaMain: Form
    {
        const string CATALOG_EXT = ".ppPv";
        const string CATALOG_PPPV = "catalog.ppPv";
        Dictionary<string, ImageFormat> image_formats = new Dictionary<string, ImageFormat>() {
            { ".jpg", ImageFormat.Jpeg },
            { ".jpeg", ImageFormat.Jpeg },
            { ".png", ImageFormat.Png },
            { ".bmp", ImageFormat.Bmp }
        };

        RecentMenu recent;
        static AppLog log;
        BitmapCache cache;

        public ParaParaMain()
        {
            InitializeComponent();

            log = new AppLog(DebugLog);
            CheckAppVer();
            var sw0 = Stopwatch.StartNew();
            cache = new BitmapCache();
            DebugOut("cache; {1}entries, {2}msec\r\n{0}", cache.CachePath, cache.FileCount, sw0.ElapsedMilliseconds);
            new MovablePanel(ExifBox, ExifLabel);
            new MovablePanel(DebugBox, DebugLabel);
            new MovablePanel(ViewPort, ViewPort);
            new MovablePanel(ScalePanel, ScaleLabel);
            //new MovablePanel(mainMenuStrip, null);
            this.MouseWheel += Photo_MouseWheel;
            InitScaleBar();
            ScrollLeftItem.Tag = Keys.Left;
            ScrollUpItem.Tag = Keys.Up;
            ScrollRightItem.Tag = Keys.Right;
            ScrollDownItem.Tag = Keys.Down;

            var aa = Environment.GetCommandLineArgs();
            DebugOut("command line: {0}", string.Join(" ", aa.Skip(1)));
            if (Array.IndexOf(aa, "--nosett") < 0)
                RestoreAppSettings();
            else
                sett = new ParaSettings();

            string culture = "";
            var files = new List<string>();
            for (int i = 1; i < aa.Length; i++)
                if (aa[i][0] == '-') {
                    switch (aa[i]) {
                    case "-c":
                    case "--culture":
                        culture = _next_arg(aa, ref i, "en");
                        break;
                    case "--debug":
                    case "-d":
                        ViewDebugItem.Checked = true;
                        break;
                    case "--file":
                    case "-f":
                        files.Add(_next_arg(aa, ref i, ""));
                        break;
                    case "--nosett":
                        break;
                    default:
                        DebugOut(Color.Fuchsia, "unknown option {0}", aa[i]);
                        break;
                    }
                } else
                    files.Add(aa[i]);

            AppLocalize(culture);

            recent = new RecentMenu(RecentMenu, (path) => OpenSome(path), Localizer.Current["RecentClear"]);
            recent.AddRange(sett.recent);
#if DEBUG
            ViewDebugItem.Checked = true;
            Exif.BorderStyle = BorderStyle.FixedSingle;
            ViewRefreshItem.Visible = true;
            ViewClearShrinkItem.Visible = true;
            ClearCacheItem.Visible = true;
#endif
            //DebugBox.Visible = ViewDebugItem.Checked;
            ToolsVisibilityChanged();

            //AddFilesWorker.RunWorkerAsync(null);

            DebugOut(Color.White, "initialize complete");

            if (files.Count > 0)
                OpenSome(files.ToArray());                  // Recent 登録不要
            else if (sett.open_last_files) {
                if (catalog_filename != "")
                    SetRoot(catalog_filename);
                if (image_filename != "") {
                    if (shuffle.Count <= 0)
                        shuffle.AddRange(photo_list);
                    shuffle.Get(image_filename);

                    LoadImage(image_filename);
                }
            }
        }

        static string _next_arg(string[] args, ref int i, string def_value)
        {
            if (i+1 < args.Length && args[i+1][0] != '-')
                return args[++i];
            return def_value;
        }

        string app_caption;
        string app_ver;

        void CheckAppVer()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var ver = assembly.GetName().Version;
            var da = (AssemblyDescriptionAttribute[])assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            app_caption = string.Format("{0} ver{1}.{2:D2}: {3}", this.Text, ver.Major, ver.Minor, da[0].Description);
            app_ver = string.Format("ver{1}.{2:D2}", this.Text, ver.Major, ver.Minor, da[0].Description);
#if DEBUG
            app_caption += " DEBUG_BUILD";
#endif
            RefreshTitle();
        }

        void RefreshTitle()
        {
            if (catalog_filename != "") {
                string name = Path.GetFileName(catalog_filename);
                if (name == CATALOG_PPPV)
                    name = Path.GetFileName(Path.GetDirectoryName(catalog_filename));
                this.Text = name+" - "+app_caption;
            } else if (image_filename != null) {
                this.Text = Path.GetFileName(image_filename)+"\\ - "+app_caption;
            } else
                this.Text = app_caption;
        }

        /* massage log */
        static public void DebugOut(Color color, string fmt, params object[] args)
        {
            log.Out(color, fmt, args);
        }

        static public void DebugOut(string fmt, params object[] args)
        {
            log.Out(Color.Gray, fmt, args);
        }

        private void ParaParaMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (catalog_filename != "" && catalog_modified)
                SaveCatalogFile(catalog_filename);

            SaveAppSettings();
            log.Dispose();
            cache.Dispose();
        }

        private void ParaParaMain_Shown(object sender, EventArgs e)
        {
            Photo.Focus();

            if ((FormWindowState)sett.WinState == FormWindowState.Maximized)
                this.WindowState = (FormWindowState)sett.WinState;
        }

        /* application settings */

        public class ParaSettings
        {
            public int sett_revision;
            public int WinState { get; set; }
            public Rectangle RestoreBounds { get; set; }
            public bool open_last_files = true;
            public string catalog = "";
            public string filename = "";
            public List<string> recent = new List<string>();

            public int scale_mode;
            public float image_scale;
            public int slide_show_interval;

            public ParaSettings()
            {
                sett_revision = 1;
                WinState = (int)FormWindowState.Maximized;
            }

            public static ParaSettings Restore()
            {
                return SettManager<ParaSettings>.Restore();
            }

            public void Save()
            {
                SettManager<ParaSettings>.Save(this);
            }

            public void Upgrade() { }
        }

        ParaSettings sett;

        /* application settings */
        void SaveAppSettings()
        {
            DebugOut("save to {0}", SettManager<ParaSettings>.GetDefaultFilename());
            try {
                //var sett = Properties.Settings.Default;
                sett.WinState = (int)this.WindowState;
                if (this.WindowState == FormWindowState.Normal)
                    sett.RestoreBounds = this.Bounds;
                else
                    sett.RestoreBounds = this.RestoreBounds;

                sett.catalog = catalog_filename;
                sett.filename = image_filename;

                sett.recent = new List<string>();
                for (int i = 0; i < 10 && i < recent.Count; i++)
                    sett.recent.Add(recent[i]);

                sett.scale_mode = (int)ScaleMode;
                sett.image_scale = ImageScale;

                sett.slide_show_interval = SlideShowTimer.Interval;

                sett.Save();

                using (var reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PERFORMANCE_KEY, true)) {
                    reg.SetValue(FIRST_DATE, first_date);
                    //reg.SetValue(PHOTO_COUNT, photo_count);
                    reg.GetValue(TOTAL_PHOTO_COUNT, total_photo_count);
                }

            } catch (Exception ex) {
                DebugOut(Color.Fuchsia, "SaveAppSettings: "+ex.Message);
            }
        }

        const string PERFORMANCE_KEY = @"Software\ParaParaView\Performance";
        const string FIRST_DATE = @"first_date";
        //const string PHOTO_COUNT = @"photo_count";
        const string TOTAL_PHOTO_COUNT = @"total_photo_count";

        string first_date;
        //int photo_count;
        int total_photo_count;

        void RestoreAppSettings()
        {
            //var sett = Properties.Settings.Default;
            sett = SettManager<ParaSettings>.Restore();
            DebugOut("restore from {0}", SettManager<ParaSettings>.GetDefaultFilename());

            //DebugOut("RestoureBoudns={0}", sett.RestoreBounds);
            this.WindowState = FormWindowState.Normal;
            if (sett.RestoreBounds.Width > 0)
                this.Bounds = sett.RestoreBounds;
            //Console.WriteLine("RestoreBounds={0}", this.Bounds);
            catalog_filename = sett.catalog;
            image_filename = sett.filename;

            ScaleMode = (ImageScaleMode)sett.scale_mode;
            //ImageScale = sett.image_scale;
            ImageScale = 1.0f;
            scroll.X = scroll.Y = 0;

            if (sett.slide_show_interval > 0)
                SlideShowTimer.Interval = sett.slide_show_interval;

            using (var reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PERFORMANCE_KEY)) {
                first_date = (string)reg.GetValue(FIRST_DATE, "");
                if (first_date == "")
                    first_date = DateTime.Now.ToShortDateString();
                //photo_count = (int)reg.GetValue(PHOTO_COUNT, 0);
                total_photo_count = (int)reg.GetValue(TOTAL_PHOTO_COUNT, 0);
            }
        }

        /* localize */
        void AppLocalize(string culture)
        {
            if (culture == "")
                culture = Application.CurrentCulture.Name;
            else
                try {
                    Application.CurrentCulture = new CultureInfo(culture);
                } catch (Exception ex) {
                    DebugOut(Color.Fuchsia, "set culture: ", ex.Message);
                }

            var lang = new Localizer(this.GetType().Namespace+".lang."+culture+".txt");

            if (File.Exists(culture))
                lang.AddFromFile(culture);

            Localizer.Current.Apply(toolTip1, this, mainMenuStrip, openFileDialog1, saveFileDialog1);
            // Localizer.Current == lang
        }

        /* file menu handlers */
        private void FileNewItem_Click(object sender, EventArgs e)
        {
            // not supported
        }

        private void OpenFolderItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = CurrentPath;
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.FileName = CATALOG_PPPV;

            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                OpenSome(openFileDialog1.FileNames);
                recent.AddRange(openFileDialog1.FileNames);
            }
        }

        private void OpenImageItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = CurrentPath;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                OpenSome(openFileDialog1.FileNames);
                recent.AddRange(openFileDialog1.FileNames);
            }
        }

        private void FileaSaveItem_Click(object sender, EventArgs e)
        {
            if (bitmap == null)
                return;

            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(image_filename);
            saveFileDialog1.FileName = Path.GetFileName(image_filename);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                string filename = saveFileDialog1.FileName;
                string ext = Path.GetExtension(filename).ToLower();
                ImageFormat format = image_formats.ContainsKey(ext) ? image_formats[ext] : ImageFormat.Png;

                var sw = Stopwatch.StartNew();
                using (var b = bitmap.Clone() as Bitmap) {
                    b.Save(filename, format);
                }
                DebugOut(Color.White, "save {0}: {1}msec", saveFileDialog1.FileName, sw.ElapsedMilliseconds);
            }
        }

        private void EjectItem_Click(object sender, EventArgs e)
        {
            if (CurrentPath != null) {
                var path = CurrentPath;
                CloseCatalog();

                Ejector.EjectMedia(path[0]);
            }
        }

        private void FileCloseItem_Click(object sender, EventArgs e)
        {
            CloseCatalog();
        }

        private void AppExitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        AboutForm about_box = new AboutForm();

        private void HelpAboutItem_Click(object sender, EventArgs e)
        {
            about_box.Params["app_ver"] = app_ver;
            about_box.Params["first_date"] = first_date;
            float[] w = { cache.TotalCacheWrite };
            string u = _media_space_unit(w);
            about_box.Params["cache_total_write"] = string.Format("{0} {1}", w[0], u);
            about_box.Params["total_photo_count"] = total_photo_count.ToString();
            //about_box.Params["photo_count"] = ;
            about_box.FadeIn(this.Bounds);
        }

        private void ParaParaMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
                //var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                //DebugOut("{0}", filenames);
            } else
                e.Effect = DragDropEffects.None;
        }

        private void ParaParaMain_DragDrop(object sender, DragEventArgs e)
        {
            var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            OpenSome(filenames);
            recent.AddRange(filenames);

            this.Activate();
        }

        // "Edit" menu handlers

        private void ExplorerItem_Click(object sender, EventArgs e)
        {
            WinUtils.Expolorer(image_filename);
        }

        private void FileDeleteItem_Click(object sender, EventArgs e)
        {
            if (image_filename != null
             && MessageBox.Show(Localizer.Format("Delete image file", image_filename),
                  app_caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                string filename = image_filename;
                CloseImage();
                cache.Remove(filename, 1f);
                
                File.Delete(filename);

                LoadImage(photo_list.RemoveCurrent());
            }
        }

        private void FileTrashItem_Click(object sender, EventArgs e)
        {
            if (image_filename != null
             && MessageBox.Show(Localizer.Format("Move to Trash this image file", image_filename),
                  app_caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                string filename = image_filename;
                CloseImage();
                cache.Remove(filename, 1f);

                // 参照 Microsoft.VisualBasic.dll
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(filename, 
                   Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                   Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                
                LoadImage(photo_list.RemoveCurrent());
            }
        }

        private void EditCopyItem_Click(object sender, EventArgs e)
        {
            if (bitmap != null) {
                Clipboard.SetImage(bitmap);
                DebugOut(Color.White, "copy to clipboard");
            }
        }

        private void EditPasteItem_Click(object sender, EventArgs e)
        {
            throw new Exception("YET");
            //DebugOut(Color.White, "paste from clipboard");
        }

        // "View" menu handlers
        private void FitToWindowItem_Click(object sender, EventArgs e)
        {
            if (FitToWindowItem.Checked)
                ScaleMode = ImageScaleMode.FitToWindow;
            else
                ScaleMode = ImageScaleMode.FixedScale;
        }

        private void FullSizeItem_Click(object sender, EventArgs e)
        {
            if (FullSizeItem.Checked)
                ScaleMode = ImageScaleMode.FullSize;
            else
                ScaleMode = ImageScaleMode.FixedScale;
        }

        private void FullScreenItem_Click(object sender, EventArgs e)
        {
            DebugOut(Color.White, "toggle fullscreen");
            FullScreen = !FullScreenItem.Checked;
        }

        private void OnlyPhotoItem_Click(object sender, EventArgs e)
        {
            DebugOut(Color.White, "toggle only photo");
            ToolsVisibilityChanged();
        }

        int cursor_hide = 0;

        void ToolsVisibilityChanged()
        {
            ExifBox.Visible = ViewExifItem.Checked && !OnlyPhotoItem.Checked;
            ViewPort.Visible = ViewPortItem.Checked && !OnlyPhotoItem.Checked;
            DebugBox.Visible = ViewDebugItem.Checked && !OnlyPhotoItem.Checked;
            ScalePanel.Visible = ScaleBarItem.Checked && !OnlyPhotoItem.Checked;

            _hide_menu();

            if (OnlyPhotoItem.Checked)
                for (; cursor_hide <= 0; cursor_hide++)
                    Cursor.Hide();
            else
                for (; cursor_hide > 0; cursor_hide--)
                    Cursor.Show();
        }

        private void ExifItem_Click(object sender, EventArgs e)
        {
            if (ViewExifItem.Checked)
                OnlyPhotoItem.Checked = false;
            ToolsVisibilityChanged();
        }

        private void ViewPortItem_Click(object sender, EventArgs e)
        {
            if (ViewPortItem.Checked)
                OnlyPhotoItem.Checked = false;
            ToolsVisibilityChanged();
        }

        private void ScaleBarItem_Click(object sender, EventArgs e)
        {
            if (ScaleBarItem.Checked)
                OnlyPhotoItem.Checked = false;
            ToolsVisibilityChanged();
        }

        private void DebugItem_Click(object sender, EventArgs e)
        {
            if (ViewDebugItem.Checked)
                OnlyPhotoItem.Checked = false;
            ToolsVisibilityChanged();
        }

        // "Photo" menu handlers
        enum SlideMode { Straight, Shuffle };
        SlideMode slide_mode = SlideMode.Straight;

        private void PhotoPrevItem_Click(object sender, EventArgs e)
        {
            string name = photo_list.Prev();
            if (name != null) {
                GoHaste(1, HASTE_MSEC);
                LoadImage(name);
            }
            slide_mode = SlideMode.Straight;
        }

        private void PhotoNextItem_Click(object sender, EventArgs e)
        {
            string name = photo_list.Next();
            if (name != null) {
                GoHaste(1, HASTE_MSEC);
                LoadImage(name);
            }
            slide_mode = SlideMode.Straight;
        }

        private void PhotoHomeItem_Click(object sender, EventArgs e)
        {
            string filename = photo_list.First();
            if (shuffle.Count <= 0)
                shuffle.AddRange(photo_list);
            shuffle.Get(filename);
            LoadImage(filename);
        }

        private void PhotoEndItem_Click(object sender, EventArgs e)
        {
            string filename = photo_list.End();
            if (shuffle.Count <= 0)
                shuffle.AddRange(photo_list);
            shuffle.Get(filename);
            LoadImage(filename);
        }

        ShuffleList shuffle = new ShuffleList(-1);

        private void ShuffleNextItem_Click(object sender, EventArgs e)
        {
            if (photo_list.Count > 0) {
                slide_mode = SlideMode.Shuffle;

                GoHaste(1, HASTE_MSEC);
                if (shuffle.Count <= 0)
                    shuffle.AddRange(photo_list);

                for ( ; shuffle.Count > 0; ) {
                    string filename = shuffle.Get();
                    if (filename == null)
                        break;
                    if (LoadImage(filename)) {
                        break;
                    }
                }
            } else
                DebugOut(Color.Fuchsia, "no photo to shuffle");
        }

        private void PhotoBackItem_Click(object sender, EventArgs e)
        {
            while (true) {
                string filename = shuffle.Back();
                if (filename == null)
                    break;
                if (LoadImage(filename))
                    break;
            }
        }

        private void SlideShowItem_Click(object sender, EventArgs e)
        {
            if (SlideShowItem.Checked)
                StartSlideShow();
            else
                StopSlideShow("Stop Slide Show");
        }

        private void SlideShowSettItem_Click(object sender, EventArgs e)
        {
            var dlg = new SlideShowDialog() {
                Interval = SlideShowTimer.Interval,
                Shuffle = slide_mode == SlideMode.Shuffle
            };
            if (dlg.ShowDialog() == DialogResult.OK) {
                if (dlg.Interval > 0)
                    SlideShowTimer.Interval = dlg.Interval;
                if (dlg.Shuffle)
                    slide_mode = SlideMode.Shuffle;
                else
                    slide_mode = SlideMode.Straight;
                StartSlideShow();
            } else
                StopSlideShow("");
        }

        void StartSlideShow()
        {
            SlideShowItem.Checked = SlideShowTimer.Enabled = true;
            StillUp.Active = true;
            DebugOut(Color.White, "Start Slide Show");
        }

        void StopSlideShow(string msg)
        {
            SlideShowItem.Checked = false;
            SlideShowTimer.Enabled = false;
            StillUp.Active = false;
            DebugOut(Color.Red, msg);
        }

        private void SlideShowTimer_Tick(object sender, EventArgs e)
        {
            if (slide_mode == SlideMode.Shuffle)
                ShuffleNextItem_Click(null, null);
            else
                PhotoNextItem_Click(null, null);
        }

        private void ParaParaMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }


        private void ParaParaMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (SlideShowItem.Checked)
                StopSlideShow("slide show stopped by key");

            if (this.ActiveControl != null && this.ActiveControl.CanSelect)
                return;
            
            switch (e.KeyCode) {
            case Keys.PageUp:
                PhotoPrevItem_Click(null, null);
                break;
            case Keys.PageDown:
                PhotoNextItem_Click(null, null);
                break;
            case Keys.Home:
                PhotoHomeItem_Click(null, null);
                break;
            case Keys.End:
                ShuffleNextItem_Click(null, null);
                break;
            case Keys.Back:
                PhotoBackItem_Click(null, null);
                break;

            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                KeyScrollAccel(e.KeyCode);
                break;

            case Keys.Space:
            case Keys.Delete:
            case Keys.NumPad0:
            case Keys.NumPad1:
            case Keys.NumPad2:
            case Keys.NumPad3:
            case Keys.NumPad4:
            case Keys.NumPad5:
            case Keys.NumPad6:
            case Keys.NumPad7:
            case Keys.NumPad8:
            case Keys.NumPad9:
                DebugOut("{0}", e.KeyData);
                return;

            default:
                return;
            }
            e.Handled = true;
        }

        private void ParaParaMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.ActiveControl != null && this.ActiveControl.CanSelect)
                return;

            switch (e.KeyCode) {
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                KeyScrollOff(e.KeyCode);
                break;

            case Keys.Escape:
                //FullScreen = false;
                break;

            case Keys.C:
                if ((e.Modifiers & Keys.Control) != 0)
                    EditCopyItem_Click(null, null);
                break;

            case Keys.P:
                if ((e.Modifiers & Keys.Control) != 0)
                    EditPasteItem_Click(null, null);
                break;

            default:
                return;
            }
            e.Handled = true;
        }

        enum CursorKey
        {
            Left = 1, Up = 2, Right = 4, Down = 8
        }

        CursorKey keys = 0, last_keys = 0;
        int key_accel = 0;

        void KeyScrollAccel(Keys key)
        {
            switch (key) {
            case Keys.Left:     // 37
                keys |= CursorKey.Left;
                break;
            case Keys.Up:       // 38
                keys |= CursorKey.Up;
                break;
            case Keys.Right:    // 39
                keys |= CursorKey.Right;
                break;
            case Keys.Down:     // 40
                keys |= CursorKey.Down;
                break;
            default:
                return;
            }

            if (keys != 0 && keys == last_keys) {
                key_accel++;
                if (keys.HasFlag(CursorKey.Left))
                    scroll.X -= key_accel;
                if (keys.HasFlag(CursorKey.Right))
                    scroll.X += key_accel;
                if (keys.HasFlag(CursorKey.Up))
                    scroll.Y -= key_accel;
                if (keys.HasFlag(CursorKey.Down))
                    scroll.Y += key_accel;
                ScrollImageLimit();
            } else
                key_accel = 0;
            last_keys = keys;
        }

        void KeyScrollOff(Keys key)
        {
            switch (key) {
            case Keys.Left:     // 37
                keys &= ~CursorKey.Left;
                break;
            case Keys.Up:       // 38
                keys &= ~CursorKey.Up;
                break;
            case Keys.Right:    // 39
                keys &= ~CursorKey.Right;
                break;
            case Keys.Down:     // 40
                keys &= ~CursorKey.Down;
                break;
            }
        }

        void ScrollImageLimit()
        {
            if (bitmap == null)
                return;

            float scale = GetActualScale();
            int w = (int)(bitmap.Width*scale + 0.5);
            int h = (int)(bitmap.Height*scale + 0.5);

            int m = 3;
            if (scroll.X > w/2)
                scroll.X = w/2;
            else if (scroll.X < -w/2)
                scroll.X = -w/2;
            else
                m ^= 1;

            if (scroll.Y > h/2)
                scroll.Y = h/2;
            else if (scroll.Y < -h/2)
                scroll.Y = -h/2;
            else
                m ^= 2;

            // cancel thumb scroll
            if (m != 0) {
                Cursor.Current = Cursors.Default;
                thumb_scroll_flag = false;
            }

            Photo.Invalidate();
            Thumb.Invalidate();
        }

        Size ViewSize
        {
            get
            {
                float scale = GetActualScale();
                int w = (int)(bitmap.Width*scale + 0.5);
                int h = (int)(bitmap.Height*scale + 0.5);
                return new Size(w, h);
            }
        }

        void Photo_MouseWheel(object sender, MouseEventArgs e)
        {
            //DebugOut("MouseWheel(button:{0}, clickes:{1}, delta:{2})", e.Button, e.Clicks, e.Delta);
            //? e.Button, e.Clicks 多分データはいってない

            if (this.ActiveControl != null && this.ActiveControl.CanSelect)
                return;

            int delta = e.Delta / SystemInformation.MouseWheelScrollDelta;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
                GoHaste(2, HASTE_MSEC);
                ScaleIndex += delta;
            }// else
            //    ;   // not assigned
        }

        // catalog file
        string catalog_filename = "";
        bool catalog_modified = false;
        //string root_path = "";

        class FileList: List<string>
        {
            public FileList() { }

            public string Prev()
            {
                if (0 < index && index < this.Count)
                    return this[--index];
                return null;
            }

            public string Next()
            {
                if (0 <= index && index+1 < this.Count)
                    return this[++index];
                return null;
            }

            public string First()
            {
                if (this.Count > 0) {
                    index = 0;
                    return this[index];
                }
                return null;
            }

            public string End()
            {
                if (this.Count > 0) {
                    index = this.Count-1;
                    return this[index];
                }
                return null;
            }
            
            public string RemoveCurrent()
            {
                if (0 <= index && index < this.Count) {
                    this.RemoveAt(index);
                    if (index < this.Count)
                        return this[index];
                }
                return null;
            }

            int index = 0;

            public string Current
            {
                get {
                    if (index < 0 || index >= this.Count)
                        return null;
                    return this[index];
                }
            }

            public int Index
            {
                get
                {
                    return index;
                }
            }

            public void SelectName(string filename)
            {
                int i = this.IndexOf(filename);
                if (i >= 0)
                    index = i;
            }
        }

        FileList photo_list = new FileList();

        void OpenSome(params string[] paths)
        {
            Cursor.Current = Cursors.WaitCursor;
            last_add_index = -1;

            int tc0 = Environment.TickCount;
            foreach (string path in paths) {
                if (Directory.Exists(path) || Path.GetExtension(path) == CATALOG_EXT)
                    SetRoot(path);
                else if (File.Exists(path))
                    AddFilename(path);
                else
                    DebugOut(Color.Fuchsia, "not found {0}", path);
            }
            DebugOut("{0} files; {1}msec", photo_list.Count, Environment.TickCount-tc0);
            Cursor.Current = Cursors.Default;

            if (last_add_index >= 0) {
                if (shuffle.Count <= 0)
                    shuffle.AddRange(photo_list);
                string filename = photo_list[last_add_index];
                shuffle.Get(filename);
                LoadImage(filename);
            }
        }

        void SetRoot(string path)
        {
            photo_list.Clear();
            if (path != "") {
                if (Directory.Exists(path))
                    catalog_filename = Path.Combine(path, CATALOG_PPPV);
                else
                    catalog_filename = path;
                string root_path = Path.GetDirectoryName(catalog_filename);

                try {
                    FSWatcher.EnableRaisingEvents = false;
                    FSWatcher.Path = root_path;
                    FSWatcher.EnableRaisingEvents = true;
                } catch (Exception ex) {
                    DebugOut(Color.Fuchsia, "FSWatcher: " + ex.Message);
                }

                DebugOut(Color.White, "root {0} / {1}", root_path, Path.GetFileName(catalog_filename));
                AddFilenames(root_path);
                //AddReq(root_path);
            } else {
                catalog_filename = "";
                //root_path = "";
            }
            RefreshTitle();
        }

        string CurrentPath
        {
            get
            {
                if (catalog_filename != null && catalog_filename != "")
                    return Path.GetDirectoryName(catalog_filename);
                if (image_filename != null && image_filename != "")
                    return Path.GetDirectoryName(image_filename);
                //throw new Exception("no directory");
                return null;
            }
        }

        int last_add_index = -1;
        bool opt_dir_sort = true;   // フォルダ一括追加時に名前でソート

        void AddFilenames(params string[] names)
        {
            foreach (var name in names)
                if (Directory.Exists(name)) {
                    string[] nn = Directory.GetFileSystemEntries(name);
                    if (opt_dir_sort)
                        Array.Sort(nn);
                    AddFilenames(nn);
                } else if (File.Exists(name))
                    AddFilename(name);
                else
                    DebugOut("no file {0}", name);
        }

        void AddFilename(string name)
        {
            string ext = Path.GetExtension(name).ToLower();
            if (!image_formats.ContainsKey(ext))
                return;

            string path = Path.GetFullPath(name);
            var fi = new FileInfo(path);
            if (fi.Attributes.HasFlag(FileAttributes.Hidden))
                return;

            int index = photo_list.IndexOf(path);
            if (index < 0) {
                photo_list.Add(path);
                index = photo_list.Count-1;
                if (last_add_index < 0)
                    last_add_index = index;

                shuffle.Add(path);
            }
        }

        Stopwatch addr_sw;
        ManualResetEvent found_first_event = new ManualResetEvent(false);
        string found_first = null;
        Task task = null;
        int dbg_scan_count = 0;

        string AddFiles4(string path)
        {
            addr_sw = Stopwatch.StartNew();
            var di = new DirectoryInfo(path);

            task = Task.Run(() => AddFiles4Sub(di));

            found_first_event.WaitOne();
            return found_first;
        }

        void AddFiles4Sub(DirectoryInfo di)
        {
            try {
                var ff = di.GetFiles(/*"*.jpg"*/);
                var ss = new List<string>();
                foreach (var f in ff) {
                    dbg_scan_count++;
                    if (image_formats.ContainsKey(f.Extension.ToLower())
                     && !f.Attributes.HasFlag(FileAttributes.Hidden)) {
                        if (found_first == null) {
                            found_first = f.FullName;
                            found_first_event.Set();
                        }
                        ss.Add(f.FullName);
                    }
                }

                ScanAsyncLabel.Invoke((MethodInvoker)(() =>
                {
                    ScanAsyncLabel.Text = string.Format("count {0}/{1}", photo_list.Count, dbg_scan_count);
                    //listBox1.Items.AddRange(ss.ToArray());
                    photo_list.AddRange(ss);
                }));
            } catch (System.UnauthorizedAccessException ex) {
                Console.WriteLine(ex.Message);
            }

            try {
                var dd = di.GetDirectories();
                foreach (var d in dd)
                    if (!d.Attributes.HasFlag(FileAttributes.Hidden))
                        AddFiles4Sub(d);
            } catch (System.UnauthorizedAccessException ex) {
                Console.WriteLine(ex.Message);
            }
        }

        void SaveCatalogFile(string filename)
        {
            throw new Exception("YET");
        }

        void LoadCatalogFile(string filename)
        {
            throw new Exception("YET");
        }

        void CloseCatalog()
        {
            DebugOut("close");

            CloseImage();
            
            FSWatcher.EnableRaisingEvents = false;

            if (catalog_modified) {
                // TODO: save ...

                catalog_modified = false;
            }
            shuffle = new ShuffleList(-1);
            //Init();
            photo_list.Clear();

            SetRoot("");
        }

        // full screen toggle gimmick
        bool menu_activated = false;
        int fade_menu_tc;

        void _show_menu()
        {
            if (!FullScreen)
                return;
            mainMenuStrip.BringToFront();
            FullScreenLabel.Visible = FullScreen;
            menu_activated = true;
            fade_menu_tc = Environment.TickCount;
        }

        void _hide_menu()
        {
            mainMenuStrip.SendToBack();
            FullScreenLabel.Visible = false;
        }

        bool FullScreen
        {
            get { return this.WindowState == FormWindowState.Maximized; }
            set
            {
                if (value) {
                    if (this.WindowState == FormWindowState.Maximized && this.FormBorderStyle != FormBorderStyle.None) {
                        this.WindowState = FormWindowState.Normal;
                    }
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;

                    MainMenuStrip.Dock = DockStyle.None;
                    FullScreenItem.Checked = true;
                } else {
                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.Sizable;

                    mainMenuStrip.Dock = DockStyle.Top;
                    mainMenuStrip.SendToBack();
                    FullScreenItem.Checked = false;
                    fade_menu_tc = Environment.TickCount;
                }
            }
        }

        private void ParaParaMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                FullScreen = true;

            //Console.WriteLine("Form_Resize(): RestoreBounds={0}, Bound={1}, win_state={2}", this.RestoreBounds, this.Bounds, this.WindowState);
        }

        private void MainMenuStrip_MenuActivate(object sender, EventArgs e)
        {
            _show_menu();
            //FullScreenLabel.Visible = fullscreen;
            //mainMenuStrip.BringToFront();
            //menu_activated = true;
        }

        private void MainMenuStrip_MenuDeactivate(object sender, EventArgs e)
        {
            menu_activated = false;
            fade_menu_tc = Environment.TickCount;
        }

        private void Timer10_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount;
            if (FullScreen && menu_activated && tc - fade_menu_tc >= 3000) {
                mainMenuStrip.SendToBack();
                FullScreenLabel.Visible = false;
            }

            var p = this.PointToClient(Cursor.Position);
            if (0 <= p.Y && p.Y < 32 && 0 <= p.X && p.Y < this.Width)
                _show_menu();

            CursorLabel.Text = string.Format("cursor: ({0}, {1})", p.X, p.Y);

            Thumb.Invalidate(); // dash pattern animation

            if (in_haste && tc-haste_tc >= 0)
                OnHasteOff();
        }

        bool opt_redraw_later = true;

        void OnHasteOff()
        {
            if (in_haste) {
                in_haste = false;

                // redraw Photo high quality
                if (opt_redraw_later && !dbg_draw_quality.Contains('H'))
                    Redraw();

                if (thumb_bitmap == null)
                    MakeThumb(bitmap);
            }
        }

        // image scaling
        enum ImageScaleMode { FitToWindow, _ShrinkToFit, _ExtendToFit, FixedScale, FullSize };

        ImageScaleMode _scale_mode = ImageScaleMode.FitToWindow;

        ImageScaleMode ScaleMode
        {
            get { return _scale_mode; }
            set
            {
                if (_scale_mode != value) {
                    _scale_mode = value;

                    FitToWindowItem.CheckState = (_scale_mode == ImageScaleMode.FitToWindow) ? CheckState.Indeterminate : CheckState.Unchecked;
                    FullSizeItem.CheckState = (_scale_mode == ImageScaleMode.FullSize) ? CheckState.Indeterminate : CheckState.Unchecked;
                    //if (_scale_mode == ImageScaleMode.FullSize)
                    //    ScaleBar.Value = 0;
                    if (_scale_mode == ImageScaleMode.FitToWindow)
                        scroll.X = scroll.Y = 0;

                    Redraw();
                }
            }
        }

        float _image_scale = 1f;

        float ImageScale
        {
            get { return _image_scale; }
            set
            {
                if (_image_scale != value) {
                    _image_scale = value;
                    Photo.Invalidate();
                }
            }
        }

        float GetActualScale()
        {
            switch (_scale_mode) {
            case ImageScaleMode.FullSize:
                return 1.0f;
            case ImageScaleMode.FixedScale:
                return _image_scale;
            case ImageScaleMode.FitToWindow:
            default:
                if (bitmap != null) {
                    float sx = (float)Photo.Width / bitmap.Width;
                    float sy = (float)Photo.Height / bitmap.Height;
                    return (float)Math.Min(sx, sy);
                } else
                    return 1f;
            }
        }

        void RefreshActualScale()
        {
            float scale = GetActualScale();
            if (scale < 1f)
                ReciprocalLabel.Text = Localizer.Format("(1/{0:F})", 1f/scale);
            else
                ReciprocalLabel.Text = "";

            ScaleEdit.Text = (scale*100).ToString("F1");
            if (scale == 1f)
                ScaleEdit.BackColor = Color.Orange;
            else
                ScaleEdit.BackColor = Color.White;

            int v = (int)Math.Round(60*Math.Log(scale)/Math.Log(2.0));
            if (v > ScaleBar.Maximum)
                ScaleBar.Value = ScaleBar.Maximum;
            else if (v < ScaleBar.Minimum)
                ScaleBar.Value = ScaleBar.Minimum;
            else
                ScaleBar.Value = v;
        }

        int ScaleIndex
        {
            get { return ScaleBar.Value; }
            set {
                if (value > ScaleBar.Maximum)
                    ScaleBar.Value = ScaleBar.Maximum;
                else if (value < ScaleBar.Minimum)
                    ScaleBar.Value = ScaleBar.Minimum;
                else
                    ScaleBar.Value = value;
                ScaleMode = ImageScaleMode.FixedScale;
                ImageScale = (float)Math.Pow(2.0, ScaleBar.Value/60.0);
            }
        }

        void InitScaleBar()
        {
            ScaleBar.TickFrequency = 60;
            ScaleBar.Maximum = +4*60;
            ScaleBar.Minimum = -4*60;
        }

        private void ScaleBar_Scroll(object sender, EventArgs e)
        {
            ScaleIndex = ScaleBar.Value;
        }

        private void ScaleUpItem_Click(object sender, EventArgs e)
        {
            GoHaste(2, HASTE_MSEC);
            ScaleIndex += 5;
        }

        private void ScaleDownItem_Click(object sender, EventArgs e)
        {
            GoHaste(2, HASTE_MSEC);
            ScaleIndex -= 5;
        }

        private void ScaleEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') {
                float value;
                if (float.TryParse(ScaleEdit.Text, out value)) {
                    ScaleMode = ImageScaleMode.FixedScale;
                    ImageScale = value/100f;
                    e.Handled = true;
                    ScaleEdit.ForeColor = Color.Black;
                } else
                    ScaleEdit.ForeColor = Color.Red;
            }
        }

        // image rotation and flip
        private void ViewOrientItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripItem;
            var op = (RotateFlipType)int.Parse((string)menu.Tag);
            var sw2 = Stopwatch.StartNew();
            bitmap.RotateFlip(op);
            image_orientation = RotateFlipOperation.Op(image_orientation, op);
            Photo.Invalidate();

            ClearShrink();
            cache.Remove(image_filename, GetActualScale());
            Photo.Invalidate();
            DebugOut(Color.White, "rotate flip{0}; {1}msec", op, sw2.ElapsedMilliseconds);

            //MakeThumb(bitmap);
            thumb_bitmap.RotateFlip(op);
            FitThumb();
        }

        // image scrolling
        Point scroll = new Point(0, 0);
        bool mouse_down_flag = false;
        Point last_loc;

        private void Photo_MouseDown(object sender, MouseEventArgs e)
        {
            Photo.Focus();

            if (e.Button == MouseButtons.Left) {
                if (Control.ModifierKeys == 0) {
                    last_loc = e.Location;
                    mouse_down_flag = true;
                    Cursor.Current = Cursors.SizeAll;
                }

                // 暫定 Shift でエクスプローラへのドロップ
                if ((Control.ModifierKeys & Keys.Shift) != 0) {
                    string[] files = { image_filename };
                    var obj = new DataObject(DataFormats.FileDrop, files);
                    DragDropEffects dde = Photo.DoDragDrop(obj, DragDropEffects.Copy);
                }
            }
        }

        private void Photo_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down_flag) {
                //DebugOut("button={0}, dragsize={1}", e.Button, SystemInformation.DragSize);
                scroll.X += e.Location.X - last_loc.X;
                scroll.Y += e.Location.Y - last_loc.Y;
                ScrollImageLimit();
                last_loc = e.Location;
                GoHaste(1, HASTE_MSEC);
                Photo.Invalidate();
                Thumb.Invalidate();
            }

            //for (; cursor_hide > 0; cursor_hide--)
            //    Cursor.Show();
        }

        private void Photo_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouse_down_flag) {
                mouse_down_flag = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void Scroll_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripItem;
            switch ((Keys)menu.Tag) {
            case Keys.Right:
                scroll.X += 1;
                break;
            case Keys.Up:
                scroll.Y -= 1;
                break;
            case Keys.Left:
                scroll.X -= 1;
                break;
            case Keys.Down:
                scroll.Y += 1;
                break;
            }
            ScrollImageLimit();
        }

        private void ScrollCenterItem_Click(object sender, EventArgs e)
        {
            scroll.X = scroll.Y = 0;
            Photo.Invalidate();
        }

        private void DumyFocus_Enter(object sender, EventArgs e)
        {
            Photo.Focus();
        }

        InterpolationMode fast_draw_mode = InterpolationMode.NearestNeighbor;   // Lowよりかなり早いが、拡大したときはモザイク
        //InterpolationMode fast_draw_mode = InterpolationMode.Low;
        InterpolationMode high_quality_shrink_mode = InterpolationMode.HighQualityBicubic;
        InterpolationMode high_quality_expand_mode = InterpolationMode.HighQualityBicubic;
        int opt_shrink = 2; // 0: none, 1: < 1.0, 2: 0.5x0.5 quatro
        float shrink_scale = -1f;
        string shrink_name = "";
        Bitmap shrink_bitmap = null;

        void ClearShrink()
        {
            if (shrink_bitmap != null) {
                shrink_bitmap.Dispose();
                shrink_bitmap = null;
            }
        }

        void Redraw()
        {
            ClearShrink();
            Photo.Invalidate();
        }

        private void ViewRefreshItem_Click(object sender, EventArgs e)
        {
            Photo.Refresh();
        }

        private void ViewClearShrinkItem_Click(object sender, EventArgs e)
        {
            ClearShrink();
        }

        long dbg_time_shrink, dbg_time_draw;
        long dbg_time_load, dbg_time_thumb;

        bool opt_exif_orientation = true;
        //Stopwatch sw = new Stopwatch();
        bool opt_cache_enabled = true;

        private void Photo_Paint(object sender, PaintEventArgs e)
        {
            try {
                dbg_time_shrink = -1;

                var g = e.Graphics;
                var sw = Stopwatch.StartNew();
                g.Clear(Color.Black);

                if (bitmap != null) {
                    float scale = GetActualScale();
                    RefreshActualScale();

                    int w = (int)(bitmap.Width*scale + 0.5);
                    int h = (int)(bitmap.Height*scale + 0.5);
                    int x = (Photo.Width-w)/2 + scroll.X;
                    int y = (Photo.Height-h)/2 + scroll.Y;

                    if (InHaste && scale < shrink_scale
                     || shrink_name != image_filename)
                        ClearShrink();

                    if (opt_shrink > 0 && shrink_bitmap == null && scale < 1.0f) {
                        if (opt_cache_enabled)
                            shrink_bitmap = cache.Get(image_filename, scale);

                        if (shrink_bitmap != null) {
                            if (dbg_verbose)
                                DebugOut(Color.Yellow, "cache hit SHRINK, fmt={0}, {1}x{2}", shrink_bitmap.PixelFormat, shrink_bitmap.Width, shrink_bitmap.Height);

                            if (image_orientation != RotateFlipType.RotateNoneFlipNone)
                                shrink_bitmap.RotateFlip(image_orientation);
                            dbg_draw_quality = "Hc";
                        } else {
                            if (opt_shrink == 2 && 0.5f <= scale && scale < 1f) {
                                shrink_bitmap = new Bitmap(bitmap.Width/2, bitmap.Height/2, bitmap.PixelFormat);
                                shrink_scale = 0.5f;
                                dbg_draw_quality = "q";
                            } else {
                                shrink_bitmap = new Bitmap(w, h, bitmap.PixelFormat);
                                shrink_scale = scale;
                                dbg_draw_quality = "s";
                            }

                            using (var sg = Graphics.FromImage(shrink_bitmap)) {
                                if (InHaste) {
                                    sg.InterpolationMode = fast_draw_mode;
                                    sg.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                                    dbg_draw_quality = "L"+dbg_draw_quality;
                                } else {
                                    sg.InterpolationMode = high_quality_shrink_mode;
                                    sg.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    dbg_draw_quality = "H"+dbg_draw_quality;
                                }
                                sg.DrawImage(bitmap, 0, 0, shrink_bitmap.Width, shrink_bitmap.Height);

                                if (!InHaste && opt_cache_enabled)
                                    cache.AddAsync(image_filename, shrink_scale, shrink_bitmap);
                            }
                            shrink_name = image_filename;

                            dbg_time_shrink = sw.ElapsedMilliseconds;

                            if (dbg_verbose)
                                DebugOut(Color.Yellow, "non cache SHRINK, fmt={0}, {1}x{2}", shrink_bitmap.PixelFormat, shrink_bitmap.Width, shrink_bitmap.Height);
                        }
                    }

                    if (shrink_bitmap != null) {
                        g.DrawImage(shrink_bitmap, x, y, w, h);
                    } else {
                        if (InHaste) {
                            g.InterpolationMode = fast_draw_mode;
                            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                            dbg_draw_quality = "L";
                        } else {
                            g.InterpolationMode = (scale < 1.0f) ? high_quality_shrink_mode : high_quality_expand_mode;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            dbg_draw_quality = "H";
                        }

                        if (scale == 1.0f) {
                            g.DrawImage(bitmap, x, y, bitmap.Width, bitmap.Height);
                            dbg_draw_quality = "Ho";
                        } else {
                            g.DrawImage(bitmap, x, y, w, h);    // stretch
                            //dbg_draw_quality = "HO";
                        }
                    }

                    dbg_time_draw = sw.ElapsedMilliseconds;

                    _refresh_benchi();
#if DEBUG
                    using (var pen = new Pen(Color.Blue, 1)) {
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, x, y, w-1, h-1);
                    }
#endif
                } else {
                    g.DrawString(Localizer.Current["NO PHOTO"],
                       FullScreenLabel.Font, Brushes.Blue, Photo.Bounds,
                       new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }

                DebugOut(Color.Lime, "Photo_Paint(); {0}msec", sw.ElapsedMilliseconds);
#if DEBUG
                using (var pen = new Pen(Color.Red, 1)) {
                    pen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(pen, 0, 0, Photo.Width-1, Photo.Height-1);
                }
#endif
            } catch (Exception ex) {
                Console.Write("Photo_Paint(): {0}", ex.Message);
            }
        }

        void CloseImage()
        {
            if (bitmap != null) { bitmap.Dispose(); bitmap = null; }
            ClearShrink();
            ClearThumb();

            Exif.Text = "";
            //orientation = 0;
            image_filename = null;

            Photo.Invalidate();
        }

        // image viewing
        Bitmap bitmap = null;
        string image_filename = null;
        RotateFlipType image_orientation;

        //const int MAX_IMAGE_HISTORY = 100;

        bool dbg_verbose = false;

        bool LoadImage(string filename)
        {
            CloseImage();
            if (filename == null || !File.Exists(filename))
                return false;

            var swx = Stopwatch.StartNew();
            try {
                if (opt_cache_enabled)
                    bitmap = cache.Get(filename, 1f);

                if (bitmap != null) {
                    if (dbg_verbose)
                        DebugOut(Color.Yellow, "cache hit FULL, fmt={0}, {1}x{2}", bitmap.PixelFormat, bitmap.Width, bitmap.Height);

                    // cached bitmap has no EXIF, get EXIF from original photo.
                    using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    using (var b = Bitmap.FromStream(stream, false, false)) {
                        ExifLabel.Text = string.Format("{0}x{1} {2:N0}bytes", bitmap.Width, bitmap.Height, stream.Length);
                        Exif.Text = ExifInfo.MakeExifStr(b);
                    }
                    dbg_time_load = swx.ElapsedMilliseconds;
                } else {
                    //var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                    bitmap = Bitmap.FromFile(filename) as Bitmap;
                    dbg_time_load = swx.ElapsedMilliseconds;

                    if (dbg_verbose)
                        DebugOut(Color.Yellow, "non-cache FULL, fmt={0}, {1}x{2}", bitmap.PixelFormat, bitmap.Width, bitmap.Height);

                    var fi = new FileInfo(filename);
                    ExifLabel.Text = string.Format("{0}x{1} {2:N0}bytes", bitmap.Width, bitmap.Height, fi.Length);
                    Exif.Text = ExifInfo.MakeExifStr(bitmap);

                    var orientation = ExifInfo.GetOrientation(bitmap);
                    if (opt_exif_orientation && orientation > 1) {
                        image_orientation = image_orientation.FromExif(orientation);
                        //bitmap.RotateFlip(RotateFlipOperation.FromExif(orientation));
                        bitmap.RotateFlip(image_orientation);
                    }

                    if (!bitmap.RawFormat.Equals(ImageFormat.Bmp) && opt_cache_enabled) {
                        if (dbg_verbose)
                            DebugOut(Color.Yellow, "add cache FULL");

                        cache.AddAsync(filename, 1f, bitmap);
                    }
                }
                Photo.Invalidate();

                image_filename = filename;
                image_orientation = RotateFlipType.RotateNoneFlipNone;

                photo_list.SelectName(filename);
                IndexLabel.Text = string.Format("index {0}/{1}", photo_list.Index, photo_list.Count);

                DebugOut(Color.Aqua, "LoadImage({0}); {1}msec", Path.GetFileName(filename), swx.ElapsedMilliseconds);
                total_photo_count++;

                if (!InHaste)
                    MakeThumb(bitmap);
                return true;
            } catch (Exception ex) {
                DebugOut(Color.Fuchsia, "LoadImage: "+ex.Message);
            }

            return false;
        }

        private void Photo_Resize(object sender, EventArgs e)
        {
            GoHaste(1, HASTE_MSEC);
            Photo.Invalidate();
        }

        // view port
        Bitmap thumb_bitmap = null;
        float thumb_scale = 1f;
        const float THUMB_SIZE = 200f;

        void MakeThumb(Bitmap bitmap)
        {
            ClearThumb();

            if (bitmap != null) {
                var sw = Stopwatch.StartNew();

                if (bitmap.Width >= bitmap.Height)
                    thumb_scale = THUMB_SIZE/bitmap.Width;
                else
                    thumb_scale = THUMB_SIZE/bitmap.Height;

                if (opt_cache_enabled)
                    thumb_bitmap = cache.Get(image_filename, 0);

                if (thumb_bitmap != null) {
                    thumb_bitmap.RotateFlip(image_orientation);
                } else { 
                    int tw = (int)(bitmap.Width*thumb_scale + 0.5);
                    int th = (int)(bitmap.Height*thumb_scale + 0.5);
                    thumb_bitmap = new Bitmap(tw, th);
                    using (var g = Graphics.FromImage(thumb_bitmap)) {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        g.DrawImage(bitmap, 0, 0, tw, th);
                    }

                    cache.AddAsync(image_filename, 0, thumb_bitmap);
                }

                FitThumb();
                dbg_time_thumb = sw.ElapsedMilliseconds;
            }
        }

        void FitThumb()
        {
            Thumb.Height = thumb_bitmap.Height;
            Thumb.Width = thumb_bitmap.Width;
            ViewPort.Width = thumb_bitmap.Width;
            ViewPort.Height = thumb_bitmap.Height + 0;
            Thumb.Invalidate();
        }

        void ClearThumb()
        {
            if (thumb_bitmap != null) {
                thumb_bitmap.Dispose();
                thumb_bitmap = null;
            }
        }

        Pen thumb_pen = new Pen(Color.Black);

        Rectangle GetViewPortRect()
        {
            float scale = GetActualScale();
            float tx = -scroll.X * thumb_scale / scale; // THUMB_SIZE / Photo.Width
            float ty = -scroll.Y * thumb_scale / scale;
            float tw = Photo.Width * Thumb.Width / (bitmap.Width*scale);
            float th = Photo.Height * Thumb.Height / (bitmap.Height*scale);
            float x = (Thumb.Width-tw)/2 + tx;
            float y = (Thumb.Height-th)/2 + ty;
            if (tw < 5)
                tw = 5f;
            if (th < 5)
                th = 5f;
            return new Rectangle((int)x, (int)y, (int)tw, (int)th);
        }

        Rectangle thumb_rect = new Rectangle();

        private void Thumb_Paint(object sender, PaintEventArgs e)
        {
            const int DASH = 6;
            thumb_pen.DashPattern = new float[] { 3, DASH-3 };
            thumb_pen.DashOffset = -Environment.TickCount % (DASH*100) / 100;
            //thumb_pen.Color = ColorCycle.Degree(Environment.TickCount / 10 % 360);

            var g = e.Graphics;
            if (thumb_bitmap != null) {
                g.DrawImage(thumb_bitmap, 0, 0);

                thumb_rect = GetViewPortRect();
                g.DrawRectangle(Pens.White, thumb_rect);
                g.DrawRectangle(thumb_pen, thumb_rect);
            }
        }

        bool thumb_scroll_flag = false;

        private void Thumb_MouseDown(object sender, MouseEventArgs e)
        {
            Thumb.Focus();

            if (e.Button == MouseButtons.Left && bitmap != null && thumb_rect.Contains(e.Location)) {
                thumb_scroll_flag = true;
                last_loc = e.Location;
            }
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (thumb_rect.Contains(e.Location)) {
                Cursor.Current = Cursors.Hand;
            } else {
                Cursor.Current = Cursors.Arrow;
            }

            if (thumb_scroll_flag) {
                float scale = GetActualScale();
                scroll.X -= (int)((e.Location.X - last_loc.X) * scale / thumb_scale);
                scroll.Y -= (int)((e.Location.Y - last_loc.Y) * scale / thumb_scale);
                last_loc = e.Location;
                ScrollImageLimit();
            }
        }

        private void Thumb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                thumb_scroll_flag = false;
            }
        }

        // file system watcher
        private void FSWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            DebugOut("FSWatcher_Changed: type={0}, name={1}", e.ChangeType, e.Name);

            AddFilename(e.FullPath);

            // remove from cache?
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            DebugOut("FSWatcher_Created: type={0}, name={1}", e.ChangeType, e.Name);

            AddFilename(e.FullPath);
        }

        private void FSWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            DebugOut("FSWatcher_Deleted: type={0}, name={1}", e.ChangeType, e.Name);

            int index = photo_list.IndexOf(e.FullPath);
            if (index >= 0)
                photo_list.RemoveAt(index);
        }

        private void FSWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            DebugOut("FSWatcher_Renamed: type={0}, name={1} to {2}", e.ChangeType, e.OldName, e.FullPath);

            int index = photo_list.IndexOf(e.OldFullPath);
            if (index >= 0)
                photo_list[index] = e.FullPath;
            else
                AddFilename(e.FullPath);
        }

        float[] media_bytes = { 0, 0 };

        // general purpose timer; interval 1sec
        private void SecTimer_Tick(object sender, EventArgs e)
        {
            // disk usage
            DriveInfo drive = null;
            if (CurrentPath != null && !CurrentPath.StartsWith(@"\\"))
                try {
                    drive = new DriveInfo(CurrentPath);
                } catch (Exception ex) {
                    Console.WriteLine("SecTimer: "+ex.Message);
                }
            if (drive != null && drive.IsReady) {
                media_bytes[0] = drive.TotalSize;
                media_bytes[1] = drive.AvailableFreeSpace;
                string byte_unit = _media_space_unit(media_bytes);
                MediaSpaceLabel.Text = string.Format("free {0:F1}/{1:F1} {2} {3}", media_bytes[1], media_bytes[0], byte_unit, drive.VolumeLabel);
                EjectItem.Text = Localizer.Format("eject {0}", drive.Name);
                EjectItem.Enabled = drive.DriveType == DriveType.Removable || drive.DriveType == DriveType.CDRom;
            } else {
                MediaSpaceLabel.Text = "---";
                EjectItem.Enabled = false;
            }

            MediaSpace.Invalidate();

            // focus control
            ControlNameLabel.Text = "active: " + this.ActiveControl.Name;

            // memory information
            float[] ws = { cache.MemFree, Environment.WorkingSet };
            string unit = _media_space_unit(ws);
            dbgMemoryLabel.Text = string.Format("free {0:N1}, WS {1:N1} {2}", ws[0], ws[1], unit);
        }

        static string _media_space_unit(float[] bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            for (; i < units.Length-1 && bytes[0] > 1024; i++)
                for (int j = 0; j < bytes.Length; j++)
                    bytes[j] /= 1024;
            return units[i];
        }

        private void MediaSpace_Paint(object sender, PaintEventArgs e)
        {
            // cache usage
            float[] bytes = { cache.DiskSize, cache.DiskUsage, cache.MemUsage };
            string byte_unit = _media_space_unit(bytes);
            CacheLabel.Text = string.Format("cache {2:F1}, {1:F1}/{0:F1} {3}", bytes[0], bytes[1], bytes[2], byte_unit);

            var g = e.Graphics;
            g.Clear(Color.Gray);
            //if (cache.CachePath)
            //g.FillRectangle(Brushes.Black, 1, 1, 100, 8);
            g.FillRectangle(Brushes.Red, 1, 1, 100*cache.DiskUsage/cache.DiskSize, 8);
            g.FillRectangle(Brushes.Aqua, 1, 11, 100*(media_bytes[0]-media_bytes[1])/media_bytes[0], 8);
        }

        string dbg_draw_quality = "?";  // H: high, L: fast, s: shrinked

        void _refresh_benchi()
        {
            DrawBenchLabel.Text = string.Format("{0} load{1}+th{2} sh{3}/dr{4}",
               dbg_draw_quality, dbg_time_load, dbg_time_thumb,
               dbg_time_shrink, dbg_time_draw);
        }

        const int HASTE_MSEC = 1000;
        int haste_tc;
        bool in_haste = false;

        void GoHaste(int level, int msec)
        {
            if (msec > 0) {
                in_haste = true;
                haste_tc = Environment.TickCount + msec;
            } else
                OnHasteOff();
        }

        bool InHaste { get { return in_haste; } }

        //

        public delegate void ActionProc();
        public delegate void ActionProc2(object sender, EventArgs e);

        public class ActionTable: Dictionary<string, ActionProc>
        {
        }

        ActionTable actions = new ActionTable() {
            {"OPEN_TRASH", WinUtils.OpenTrash },
        };

        private void ClearCacheItem_Click(object sender, EventArgs e)
        {
            cache.Clear();
        }

        public void ActionHandler(object sender, EventArgs e)
        {
            var control = sender as Control;
            var item = sender as ToolStripItem;
            string tag, name;
            if (control != null) {
                name = control.Name;
                tag = (string)control.Tag;
            } else if (item != null) {
                name = item.Name;
                tag = (string)item.Tag;
            } else {
                DebugOut(Color.Fuchsia, "action: no support {0}", sender);
                return;
            }

            if (actions.ContainsKey(tag)) {
                var proc = actions[tag];
                proc(/*sender, e*/);
            } else
                DebugOut(Color.Fuchsia, "action: unknwon key {0}.Tag={1}", name, tag);
        }




    }
}
