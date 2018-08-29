/*
RecentMenu
AppLog
Localizer
MovablePanel
SettManager

StillUp
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;   // DllImport
//using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ParaParaView
{
    /// <summary>
    /// 
    /// </summary>
    static class BitmapFrom
    {
        public static Bitmap Image(Bitmap source)
        {
            BitmapData data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
            Bitmap result = new Bitmap(source.Width, source.Height, data.Stride, data.PixelFormat, data.Scan0);
            source.UnlockBits(data);
            return result;
        }

        public static Bitmap File(string filename)
        {
            Bitmap source;
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                source = (Bitmap)Bitmap.FromStream(stream);
            //source.Dispose(); ??do not Dispose()

            return BitmapFrom.Image(source);
        }
    }

    // RecentMenu - 
    // initilaize
    // var recent = new RecentMenu(menu, void callback(string filename))

    // recent.Add(string filename)
    // int recent.Count()
    // string recent[int index]
    // recent.Clear()

    //  for (int i = 0; i< 10; i++)
    //      Properties.Settings.Default["recent"+i.ToString()] = (i<recent.Count) ? recent[i] : "";

    //  for (int i = 0; i< 10; i++) {
    //      string path = (string)Properties.Settings.Default["recent"+i.ToString()];
    //      recent.Add(path);
    //  }

    /// <summary>
    /// 指定したmenuに「最近使った項目」サブメニューを追加します。
    /// add sub menu for recent open items.
    /// </summary>
    class RecentMenu
    {
        public delegate void RecentCallback(string path);

        ToolStripMenuItem menu = null;
        RecentCallback callback = null;
        List<string> items = new List<string>();
        string clear_text = null;

        /// <summary>
        /// menuの下に「最近使ったアイテム」サブメニューを追加します。
        /// そのサブメニューを選択したときに呼ぶcallbackを登録します。
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="callback"></param>
        public RecentMenu(ToolStripMenuItem menu, RecentCallback callback, string clear_text = null)
        {
            this.menu = menu;
            menu.DropDownOpening += _menu_DropDownOpening;

            this.callback = callback;
            this.clear_text = clear_text;
        }

        void _menu_DropDownOpening(object sender, EventArgs e)
        {
            menu.DropDownItems.Clear();
            for (int i = items.Count-1, n = 0; i >= 0; --i, n++) {
                string caption = items[i];
                if (n < 10)
                    caption = string.Format("&{0} {1}", n, caption);
                var item = new ToolStripMenuItem(caption, null, _item_Click);
                item.Tag = items[i];
                menu.DropDownItems.Add(item);
            }

            if (clear_text != null && clear_text != "") {
                menu.DropDownItems.Add(new ToolStripSeparator());
                menu.DropDownItems.Add(clear_text, null, _clear_Click);
            }
        }

        void _item_Click(object sender, EventArgs e)
        {
            string path = (string)(sender as ToolStripMenuItem).Tag;

            items.Remove(path);
            items.Add(path);

            //callback?.Invoke(path);
            if (callback != null)
                callback(path);
        }

        void _clear_Click(object sender, EventArgs e)
        {
            Clear();
            menu.HideDropDown();
        }

        /// <summary>
        /// 追加した「最近使った項目」サブメニューの数
        /// </summary>
        public int Count { get { return items.Count; } }

        /// <summary>
        /// 「最近使った項目」をインデックスを指定して取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>ファイル名(フルパス)</returns>
        public string this[int index] { get { return items[index]; } }

        /// <summary>
        /// 「最近使った項目」を追加します。
        /// </summary>
        /// <param name="name"></param>
        public void Add(string name)
        {
            if (name == null || name == "")
                return;
            string fullpath = Path.GetFullPath(name);
            items.Remove(fullpath);
            items.Add(fullpath);
            if (items.Count > 10)
                items.RemoveAt(0);
        }

        /// <summary>
        /// 「最近使った項目」を複数一括追加します。
        /// </summary>
        /// <param name="name"></param>
        public void AddRange(IEnumerable<string> names)
        {
            foreach (var n in names)
                Add(n);
        }

        /// <summary>
        /// 全項目を削除します。
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

    } // RecentMenu

    // AppLog - 
    // initilaize
    // var log = new AppLog(RichTextBox rich)
    // log.Out(color, format, ...)
    // log.Out(format, ...)

    class AppLog : IDisposable
    {
        RichTextBox rich1 = null, rich2 = null;
        Timer timer;
        StreamWriter logfile = null;
        const int LOG_LINES = 100;  // 15
        string log_time_fmt = "HH:mm:ss.fff ";
        int log_index = 0;

        public AppLog(RichTextBox rich)
        {
            rich1 = rich;

            rich2 = new RichTextBox();
            rich2.Font = rich1.Font;
            rich2.ForeColor = rich1.ForeColor;

            timer = new Timer();
            timer.Interval = 1;
            timer.Enabled = true;
            timer.Tick += _timer_Tick;
#if DEBUG
            string logname = Path.ChangeExtension(Application.ExecutablePath, ".log");
            logfile = new StreamWriter(logname, true);
#endif
        }

        public void Dispose()
        {
            if (logfile != null) {
                //logfile.Close();
                logfile.Dispose();
                logfile = null;
            }

            rich1 = null;
            rich2.Dispose();

            timer.Dispose();
        }

        public void Out(string fmt, params object[] args) { Out(Color.White, fmt, args); }

        public void Out(Color color, string fmt, params object[] args)
        {
            if (rich1.InvokeRequired) {
                rich1.BeginInvoke((MethodInvoker)(() => Out(color, fmt, args)));
            } else {
                string time = DateTime.Now.ToString(log_time_fmt);
                string text = string.Format(fmt, args);

                if (logfile != null) {
                    logfile.WriteLine(text);
                }

                // delete old lines
                if (rich2.Lines.Length >= LOG_LINES) {
                    int n = rich2.Lines.Length - LOG_LINES + 1;
                    int index = 0;
                    for (int i = 0; i < n; i++)
                        index = rich2.Text.IndexOf('\n', index + 1);
                    rich2.Select(0, index + 1);
                    rich2.SelectedText = "";
                }

                // change inner RichTextBox
                rich2.Select(rich2.Text.Length, 0);
                rich2.SelectionFont = rich2.Font;
                rich2.SelectedText = time;
                rich2.SelectionColor = color;
                rich2.SelectedText = text;
                rich2.SelectionColor = Color.White;
                rich2.SelectedText = "\n";

                // copy inner to visible RichTextBox 
                rich1.Rtf = rich2.Rtf;
                rich1.ClearUndo();

                // scroll to new line
                log_index = rich1.Text.Length;
                rich1.Select(log_index, 0);
                rich1.ScrollToCaret();
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (rich1 != null && !rich1.Focused) {
                rich1.Select(log_index, 100);
                rich1.SelectionColor = Color.White;
                rich1.SelectedText = DateTime.Now.ToString(log_time_fmt);
            }
        }

    }

    /* LocalizeDictionary */
    // 言語コード ISO-639 抜粋
    // en: english en-US, en-GB
    // ja: japanese ja-JP
    // de: deutch
    // zh: chinese zh-CN, zh-TW
    // ko: korean

    /// <summary>
    /// Localize: internationalization
    /// フォームのコントロールやメニューなどのプロパティをローカライズ
    /// </summary>
    public class Localizer
    {
        Dictionary<string, Dictionary<string, string>> dict;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Localizer()
        {
        }

        /*~Localizer()
        {
            if (current == this)
                current = null;
        }*/

        /// <summary>
        /// Constructor with resource name.
        /// e.g. var dict = new Localizer("NAMESPACE.lang.ja-JP.txt")
        /// </summary>
        /// <param name="resource_name">manifest reource name.</param>
        public Localizer(string resource_name)
        {
            dict = new Dictionary<string, Dictionary<string, string>>();
            LoadManifestResources(resource_name);
        }

        /// <summary>
        /// Load language definitions from resources.
        /// </summary>
        /// <param name="resource_name">manifest reource name.</param>
        public void LoadManifestResources(string resource_name)
        {
            List<string> resources = new List<string>();
            string path = Path.GetFileNameWithoutExtension(resource_name);  // APP.lang.ja-JP
            path = Path.GetFileNameWithoutExtension(path);                  // APP.lang
            resources.Add(path+".en.txt");                                  // APP.lang.en.txt; default language is english
            string[] nn = resource_name.Split('-');
            if (nn.Length >= 2)
                resources.Add(nn[0]+".txt");                                // APP.lang.ja.txt
            resources.Add(resource_name);                                   // APP.lang.ja-JP.txt

            dict.Clear();
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            //Console.WriteLine(" *Manifest Resources\r\n{0}", string.Join("\r\n", asm.GetManifestResourceNames()));
            foreach (string r in resources)
                using (var stream = asm.GetManifestResourceStream(r))
                    if (stream != null)
                        AddFromStream(stream);
            //else
            //    Console.WriteLine("no resource {0}", r);

            current = this;
        }

        public void AddFromStream(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8))
                for (; !sr.EndOfStream;) {
                    string line = sr.ReadLine();
                    if (line.Length <= 0 || line[0] == '#')   // skip empty line and comments
                        continue;

                    int i = line.IndexOf('=');
                    string name = (i > 0) ? line.Substring(0, i) : line;
                    string property = "Text";
                    int j = name.IndexOf('.');
                    if (j > 0) {
                        property = name.Substring(j+1);
                        if (property == "Hint")    // compatibility for Turbo C++ version
                            property = "ToolTipText";

                        name = name.Substring(0, j);
                    }

                    string text = (i > 0) ? line.Substring(i+1) : name;
                    text = text.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace(@"\t", "\t");

                    this[name, property] = text;
                }
        }

        /// <summary>
        /// Add language definitions from file.
        /// </summary>
        /// <param name="filename">filename</param>
        public void AddFromFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
                AddFromStream(stream);

            current = this;
        }

        /// <summary>
        /// Delete all definitions.
        /// </summary>
        public void Clear()
        {
            dict.Clear();
        }

        /// <summary>
        /// 
        /// e.g. string lang["button1", "Hint"]
        /// </summary>
        /// <param name="name">Component name</param>
        /// <param name="property">propertyt name</param>
        /// <returns></returns>
        public string this[string name, string property]
        {
            get
            {
                string key = name.ToLower();
                if (!dict.ContainsKey(key) || !dict[key].ContainsKey(property))
                    //return null;
                    return name;
                return dict[key][property];
            }
            set
            {
                string key = name.ToLower();
                if (!dict.ContainsKey(key))
                    dict[key] = new Dictionary<string, string>();
                dict[key][property] = value;
            }
        }

        /// <summary>
        /// 
        /// e.g. string lang["confirm msg"]
        /// </summary>
        /// <param name="str">String to translate.</param>
        /// <returns>Translated text</returns>
        public string this[string str]
        {
            get { return this[str, "Text"]; }
            set { this[str, "Text"] = value; }
        }

        /// <summary>
        /// Translate one component.
        /// </summary>
        /// <param name="tooltip"></param>
        /// <param name="component"></param>
        public void ApplyComponent(ToolTip tooltip, object component)
        {
            var type = component.GetType();
            var n = type.GetProperty("Name");
            if (n == null)
                return;                 // obj has no "Name", maybe it is not form, contorl, menu item
            string name = (string)n.GetValue(component, null);
            string key = name.ToLower();

            if (dict.ContainsKey(key)) {
                foreach (var prop in dict[key]) {
                    var p = type.GetProperty(prop.Key);
                    if (p != null)
                        p.SetValue(component, prop.Value, null);
                    else if (prop.Key == "ToolTipText" && tooltip != null)
                        tooltip.SetToolTip(component as Control, prop.Value);
                    //else
                    //    Console.WriteLine("{0} has no property .{1}", name, prop.Key);
                }
            }// else
            //    Console.WriteLine("{0} is not in dictionary", name);
        }

        //public void Apply(ToolTip tooltip, IEnumerable<object> components)

        // e.g. lang.Apply(toolTip1, this, ...)
        public void Apply(ToolTip tooltip, params object[] components)
        {
            foreach (var component in components) {
                ApplyComponent(tooltip, component);

                if (component is DataGridView) {
                    var grid = component as DataGridView;
                    foreach (var col in grid.Columns)
                        Apply(tooltip, col);
                } else if (component is ContextMenuStrip) {
                    var menu = component as ContextMenuStrip;
                    foreach (Component m in menu.Items)
                        Apply(tooltip, m);
                } else if (component is MenuStrip) {
                    var menu = component as MenuStrip;
                    foreach (Component m in menu.Items)
                        Apply(tooltip, m);
                } else if (component is ToolStripMenuItem) {
                    var menu = component as ToolStripMenuItem;
                    foreach (Component m in menu.DropDownItems)
                        Apply(tooltip, m);
                } else if (component is Control) {
                    var form = component as Control;
                    foreach (Control c in form.Controls)
                        Apply(tooltip, c);
                }
            }
        }

        /* static method version */
        static Localizer current = null;

        /// <summary>
        /// Last loaded language instance.
        /// can use as static Class.
        /// e.g. Localizer.Current["name", "property"] 
        ///      Localizer.Current["messege text"]
        /// </summary>
        public static Localizer Current { get { return current; } }

        /// <summary>
        /// Translate str, and format using string.Format()
        /// .e.g. Localizer.Format("{0:F3} sec", sec)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(string str, params object[] args)
        {
            string fmt = current[str];
            try {
                return string.Format(fmt, args);
            } catch (System.FormatException) {
                return fmt;
            }
        }

        //void Export()
    }

    /// <summary>
    /// 
    /// </summary>
    public class MovablePanel
    {
        /// <summary>
        /// Constructor.
        /// 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="title"></param>
        public MovablePanel(Control panel, Control title)
        {
            this.panel = panel;

            if (title == null)
                title = panel;

            title.MouseDown += _title_MouseDown;
            title.MouseUp += _title_MouseUp;
            title.MouseMove += _title_MouseMove;
        }

        Control panel;
        Point last_loc;
        bool mouse_down_flag = false;

        void _title_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                // begin panel move
                last_loc = e.Location;
                mouse_down_flag = true;
                panel.BringToFront();
                Cursor.Current = Cursors.Hand;
            }
        }

        void _title_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down_flag) {
                // panel moving
                panel.Left += e.Location.X - last_loc.X;
                panel.Top += e.Location.Y - last_loc.Y;
            }
        }

        void _title_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouse_down_flag) {
                // panel move end
                mouse_down_flag = false;

                // update bounds anchor
                var parent = panel.Parent;
                var anchor = panel.Anchor;
                if (panel.Left < parent.Width/3)
                    anchor |= AnchorStyles.Left;
                else
                    anchor &= ~AnchorStyles.Left;
                if (panel.Top < parent.Height/3)
                    anchor |= AnchorStyles.Top;
                else
                    anchor &= ~AnchorStyles.Top;
                if (panel.Right > 2*parent.Width/3)
                    anchor |= AnchorStyles.Right;
                else
                    anchor &= ~AnchorStyles.Right;
                if (panel.Bottom > 2*parent.Height/3)
                    anchor |= AnchorStyles.Bottom;
                else
                    anchor &= ~AnchorStyles.Bottom;
                if (panel.Anchor != anchor) {
                    panel.Anchor = anchor;
                    System.Diagnostics.Debug.WriteLine("{0}.Anchor changed to {1}", panel.Name, anchor);
                }
            }
        }

    }

    static class RotateFlipOperation
    {
        public static RotateFlipType Default { get { return RotateFlipType.RotateNoneFlipNone; } }

        public static RotateFlipType FromExif(int orientation)
        {
            switch (orientation) {
            case 2: // 上下反転(上下鏡像 ?)
                return RotateFlipType.RotateNoneFlipY;
            case 3: // 180度回転
                return RotateFlipType.Rotate180FlipNone;
            case 4: // 左右反転
                return RotateFlipType.RotateNoneFlipX;
            case 5: // 上下反転、時計周りに270度回転
                return RotateFlipType.Rotate270FlipY;
            case 6: // 時計周りに90度回転
                return RotateFlipType.Rotate90FlipNone;
            case 7: // 上下反転、時計周りに90度回転
                return RotateFlipType.Rotate90FlipY;
            case 8: // 時計周りに270度回転
                return RotateFlipType.Rotate270FlipNone;
            default:    // そのまま
                return RotateFlipType.RotateNoneFlipNone;
            }
        }

        public static RotateFlipType Op(RotateFlipType type, int flip4, int step90)
        {
            int n = (int)type;
            int r = (n+step90) & 3;
            int f = (n^flip4) & 4;
            return (RotateFlipType)(f|r);
        }

        public static RotateFlipType Op(RotateFlipType t1, RotateFlipType t2)
        {
            int a = (int)t1;
            int b = (int)t2;
            int f = (a^b) & 4;
            int r = (f == 0) ? a+b : a-b;
            return (RotateFlipType)(r&3 | f);
        }

        public static RotateFlipType RotateRight(RotateFlipType type) { return Op(type, 0, 1); }
        public static RotateFlipType RotateLeft(RotateFlipType type) { return Op(type, 0, 3); }
        public static RotateFlipType FlipVertical(RotateFlipType type) { return Op(type, 4, 2); }
        public static RotateFlipType FlipHorizontal(RotateFlipType type) { return Op(type, 4, 0); }
    }

    public class ImageOrientation
    {
        RotateFlipType rf;

        RotateFlipType[] rf_table = {
            RotateFlipType.RotateNoneFlipNone,
            RotateFlipType.RotateNoneFlipNone,
            RotateFlipType.RotateNoneFlipY,     // 2: 上下反転(上下鏡像 ?)
            RotateFlipType.Rotate180FlipNone,   // 3: 180度回転
            RotateFlipType.RotateNoneFlipX,     // 4: 左右反転
            RotateFlipType.Rotate270FlipY,      // 5: 上下反転、時計周りに270度回転
            RotateFlipType.Rotate90FlipNone,    // 6: 時計周りに90度回転
            RotateFlipType.Rotate90FlipY,       // 7: 上下反転、時計周りに90度回転
            RotateFlipType.Rotate270FlipNone    // 8: 時計周りに270度回転
        };

        public ImageOrientation(int orientation)
        {
            if (0 <= orientation && orientation < rf_table.Length)
                rf = rf_table[orientation];
            else
                throw new Exception();
        }
    }

    /// <summary>
    /// アプリケーション設定の保存/復元
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public class SettManager<T> where T: new()
    {
        /// <summary>
        /// 保存先ファイル名を得る。
        /// ユーザー名\AppData\Local\アプリ.sett.xml
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultFilename()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string app = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string dirname = path + @"\" + app;
            Directory.CreateDirectory(dirname);
            return dirname + @"\settings.xml";
        }

        /// <summary>
        /// 設定をファイルに保存
        /// </summary>
        /// <param name="sett">設定データクラス T のインスタンス</param>
        static public void Save(T sett) { SaveXML(GetDefaultFilename(), sett); }

        static public void SaveXML(string filename, T sett)
        {
            var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var w = new StreamWriter(filename, false)) {
                ser.Serialize(w, sett);
                //w.Close();
            }
        }

        /// <summary>
        /// 設定データの読み込み
        /// </summary>
        /// <returns>読み込んだ設定データクラス T のインスタンスを返す。初回起動時など、設定ファイルがまだないとき T のデフォルトコンストラクタ</returns>
        public static T Restore() { return RestoreXML(GetDefaultFilename()); }

        public static T RestoreXML(string filename)
        {
            if (File.Exists(filename)) {
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (var r = new StreamReader(filename)) {
                    return (T)ser.Deserialize(r);
                }
            } else
                return new T();
        }
    }

    /// <summary>
    /// Screen saver suppression
    /// </summary>
    public static class StillUp
    {
        // https://variedtastefinder.jp/blog/?p=1567より
        [FlagsAttribute]
        public enum ExecutionState: uint
        {
            // 関数が失敗した時の戻り値
            Null = 0,
            // スタンバイを抑止(Vista以降は効かない？)
            SystemRequired = 1,
            // 画面OFFを抑止
            DisplayRequired = 2,
            // 効果を永続させる。ほかオプションと併用する。
            Continuous = 0x80000000,
        }

        [DllImport("user32.dll")]
        extern static uint SendInput(
            uint nInputs,   // INPUT 構造体の数(イベント数)
            INPUT[] pInputs,   // INPUT 構造体
            int cbSize     // INPUT 構造体のサイズ
            );

        [StructLayout(LayoutKind.Sequential)]  // アンマネージ DLL 対応用 struct 記述宣言
        struct INPUT
        {
            public int type;  // 0 = INPUT_MOUSE(デフォルト), 1 = INPUT_KEYBOARD
            public MOUSEINPUT mi;
            // Note: struct の場合、デフォルト(パラメータなしの)コンストラクタは、
            //       言語側で定義済みで、フィールドを 0 に初期化する。
        }

        [StructLayout(LayoutKind.Sequential)]  // アンマネージ DLL 対応用 struct 記述宣言
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;  // amount of wheel movement
            public int dwFlags;
            public int time;  // time stamp for the event
            public IntPtr dwExtraInfo;
            // Note: struct の場合、デフォルト(パラメータなしの)コンストラクタは、
            //       言語側で定義済みで、フィールドを 0 に初期化する。
        }

        // dwFlags
        const int MOUSEEVENTF_MOVED = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;  // 左ボタン Down
        const int MOUSEEVENTF_LEFTUP = 0x0004;  // 左ボタン Up
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;  // 右ボタン Down
        const int MOUSEEVENTF_RIGHTUP = 0x0010;  // 右ボタン Up
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  // 中ボタン Down
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;  // 中ボタン Up
        const int MOUSEEVENTF_WHEEL = 0x0080;
        const int MOUSEEVENTF_XDOWN = 0x0100;
        const int MOUSEEVENTF_XUP = 0x0200;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        const int screen_length = 0x10000;  // for MOUSEEVENTF_ABSOLUTE (この値は固定)

        [DllImport("kernel32.dll")]
        extern static ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        static public void Once()
        {
            //画面暗転阻止
            SetThreadExecutionState(ExecutionState.DisplayRequired);

            // ドラッグ操作の準備 (struct 配列の宣言)
            INPUT[] input = new INPUT[1];  // イベントを格納

            // ドラッグ操作の準備 (イベントの定義 = 相対座標へ移動)
            input[0].mi.dx = 0;  // 相対座標で0　つまり動かさない
            input[0].mi.dy = 0;  // 相対座標で0 つまり動かさない
            input[0].mi.dwFlags = MOUSEEVENTF_MOVED;

            // ドラッグ操作の実行 (イベントの生成)
            SendInput(1, input, Marshal.SizeOf(input[0]));
        }

        static Timer timer;
        static bool active = false;

        static public bool Active
        {
            get { return active; }
            set
            {
                if (active != value) {
                    active = value;
                    if (active) {
                        timer = new Timer();
                        timer.Interval = 50*1000;
                        timer.Tick += (sender, e) => StillUp.Once();
                        timer.Enabled = true;
                    } else {
                        timer.Dispose();
                        timer = null;
                    }
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class RemovalDiskWatcher
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public static class WinUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void Expolorer(string path)
        {
            if (File.Exists(path)) {
                System.Diagnostics.Process.Start("EXPLORER.EXE", "/select,\""+path+"\"");
            } else {
                string dir = Path.GetDirectoryName(path);
                System.Diagnostics.Process.Start("EXPLORER.EXE", "\""+dir+"\"");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OpenTrash()
        {
            System.Diagnostics.Process.Start(@"shell:::{645FF040-5081-101B-9F08-00AA002F954E}");
        }

    }

    public static class ColorCycle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree">0 .. 359</param>
        /// <returns></returns>
        public static Color Degree(int degree)
        {
            int c = degree / 60;
            int d = degree % 60;
            switch (c) {
            default:
                return Color.FromArgb(255, 255*d/59, 0);           //   0 ..  59: r -> Y
            case 1:
                return Color.FromArgb(255-255*d/59, 255, 0);           //  60 .. 119: Y -> g
            case 2:
                return Color.FromArgb(0, 255, 255*d/59);    // 120 .. 179: g -> C
            case 3:
                return Color.FromArgb(0, 255-255*d/59, 255);    // 180 .. 239: C -> b
            case 4:
                return Color.FromArgb(255*d/59, 0, 255);               // 240 .. 299: b -> M
            case 5:
                return Color.FromArgb(255, 0, 255-255*d/59);           // 300 .. 359: M -> r
            }
        }
    }
}

namespace RotateFlipExtensions
{
    public static class _rotate_flip_extensions
    {
        public static RotateFlipType FromExif(this RotateFlipType rf, int orientation)
        {
            switch (orientation) {
            case 2: // 上下反転(上下鏡像 ?)
                return RotateFlipType.RotateNoneFlipY;
            case 3: // 180度回転
                return RotateFlipType.Rotate180FlipNone;
            case 4: // 左右反転
                return RotateFlipType.RotateNoneFlipX;
            case 5: // 上下反転、時計周りに270度回転
                return RotateFlipType.Rotate270FlipY;
            case 6: // 時計周りに90度回転
                return RotateFlipType.Rotate90FlipNone;
            case 7: // 上下反転、時計周りに90度回転
                return RotateFlipType.Rotate90FlipY;
            case 8: // 時計周りに270度回転
                return RotateFlipType.Rotate270FlipNone;
            default:    // そのまま
                return RotateFlipType.RotateNoneFlipNone;
            }
        }

        public static RotateFlipType Add(this RotateFlipType t1, RotateFlipType t2)
        {
            int a = (int)t1;
            int b = (int)t2;
            int f = (a^b) & 4;
            int r = (f == 0) ? a+b : a-b;
            return (RotateFlipType)(r&3 | f);
        }

        /*        public static RotateFlipType Op(RotateFlipType t1, RotateFlipType t2)
                {
                    int a = (int)t1;
                    int b = (int)t2;
                    int f = (a^b) & 4;
                    int r = (f == 0) ? a+b : a-b;
                    return (RotateFlipType)(r&3 | f);
                }
                */
    }

}