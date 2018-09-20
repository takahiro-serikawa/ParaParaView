// PictureBox improvement for "para para Photo viewer"
// scaling, scrolling, lazy fast drawing

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace ParaParaView
{
    /// <summary>
    /// 
    /// </summary>
    public enum ImageScaleMode { FitToWindow, _ShrinkToFit, _ExtendToFit, FixedScale, FullSize };

    /// <summary>
    /// 
    /// </summary>
    public partial class ParaParaImage: PictureBox
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ParaParaImage()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserMouse, true);
            Cursor = Cursors.Cross;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            InHaste = 1;
            Invalidate();
            _refresh_visible_rect();

            if (_scale_mode == ImageScaleMode.FitToWindow
             && ImageScaleChanged != null)
                ImageScaleChanged(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Refresh()
        {
            ClearShrink();
            Invalidate();
        }

        /// <summary>
        /// Image that has been casted to bitmap.
        /// </summary>
        public Bitmap Bitmap {
            get { return (Bitmap)Image; }
            //private set { Image = value; }
        }

        /// <summary>
        /// Draw string if no Image.
        /// </summary>
        public string NoPhoto { get; set; } = "NO PHOTO";

        InterpolationMode high_quality_shrink_mode = InterpolationMode.HighQualityBicubic;
        InterpolationMode high_quality_expand_mode = InterpolationMode.HighQualityBicubic;

        protected override void OnPaint(PaintEventArgs pe)
        {
            //base.OnPaint(pe);

            var g = pe.Graphics;
            g.Clear(Color.Black);
            dbg_time_shrink = -1;
            var sw = Stopwatch.StartNew();

            if (Bitmap != null) {
                try {
                    float scale = ActualScale;

                    int w = (int)(Bitmap.Width*scale + 0.5);
                    int h = (int)(Bitmap.Height*scale + 0.5);
                    int x = (Width-w)/2 + offset.X;
                    int y = (Height-h)/2 + offset.Y;

                    if (scale < 1f) {
                        if (scale < 0.5f && scale != shrink_scale)
                            ClearShrink();
                        if (shrink_bitmap == null) {
                            if (scale < 0.5f)
                                make_shrink(Bitmap, scale, w, h);
                            else
                                make_shrink(Bitmap, 0.5f, Bitmap.Width/2, Bitmap.Height/2);
                        }
                    }
                    //    if (InHaste && scale < shrink_scale
                    //     || shrink_name != image_filename)
                    //        ClearShrink();

                    //    if (opt_shrink > 0 && shrink_bitmap == null && scale < 1.0f) {
                    //        shrink_bitmap = cache[image_filename, scale];

                    //        if (shrink_bitmap != null) {
                    //            if (image_orientation != RotateFlipType.RotateNoneFlipNone)
                    //                shrink_bitmap.RotateFlip(image_orientation);
                    //dbg_draw_quality = "Hc";
                    //        } else {
                    //            make_shrink(bitmap, scale, w, h);
                    //dbg_time_shrink = sw.ElapsedMilliseconds;
                    //        }
                    //    }

                    if (InHaste > 0) {
                        g.InterpolationMode = fast_draw_mode;
                        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        dbg_draw_quality = "L";
                    } else {
                        g.InterpolationMode = (scale < 1.0f) ? high_quality_shrink_mode : high_quality_expand_mode;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        dbg_draw_quality = "H";
                    }

                    //if (shrink_bitmap != null) {
                    if (scale < 1f) {
                        g.DrawImage(shrink_bitmap, x, y, w, h);
                    } else if (scale == 1.0f) {
                        g.DrawImage(Bitmap, x, y, Bitmap.Width, Bitmap.Height);
                        dbg_draw_quality += "O";
                    } else {
                        g.DrawImage(Bitmap, x, y, w, h);    // stretch
                        dbg_draw_quality += "o";
                    }

                    dbg_time_draw = sw.ElapsedMilliseconds;
                    //_refresh_benchi();
#if DEBUG
                    using (var pen = new Pen(Color.Blue, 1)) {
                        pen.DashStyle = DashStyle.Dash;

                        g.DrawRectangle(pen, x, y, w-1, h-1);
                    }
#endif
                } catch (Exception ex) {
                    Console.Write("Photo_Paint(): {0}", ex.Message);
                }
            } else {
                g.DrawString(NoPhoto, this.Font, Brushes.Blue, this.Bounds,
                   new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

#if DEBUG
            using (var pen = new Pen(Color.Red, 1)) {
                pen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(pen, 0, 0, Width-1, Height-1);
            }
#endif
        }

        InterpolationMode fast_draw_mode = InterpolationMode.NearestNeighbor;   // Lowよりかなり早いが、拡大したときはモザイク
        //InterpolationMode fast_draw_mode = InterpolationMode.Low;
        //InterpolationMode high_quality_shrink_mode = InterpolationMode.HighQualityBicubic;
        //InterpolationMode high_quality_expand_mode = InterpolationMode.HighQualityBicubic;
        int opt_shrink = 2; // 0: none, 1: < 1.0, 2: 0.5x0.5 quatro
        float shrink_scale = -1f;
        string shrink_name = "";
        Bitmap shrink_bitmap = null;

        public void ClearShrink()
        {
            if (shrink_bitmap != null) {
                //shrink_bitmap.Dispose();
                shrink_bitmap = null;
            }
        }

        void make_shrink(Bitmap bitmap, float scale, int w, int h)
        {
            try {
                if (opt_shrink == 2 && 0.5f <= scale && scale< 1f) {
                    shrink_bitmap = new Bitmap(bitmap.Width/2, bitmap.Height/2, bitmap.PixelFormat);
                    shrink_scale = 0.5f;
                    dbg_draw_quality = "q";
                } else {
                    shrink_bitmap = new Bitmap(w, h, bitmap.PixelFormat);
                    shrink_scale = scale;
                    dbg_draw_quality = "s";
                }

                using (var sg = Graphics.FromImage(shrink_bitmap)) {
                    //if (InHaste) {
                    //    sg.InterpolationMode = fast_draw_mode;
                    //    sg.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    //    dbg_draw_quality = "L"+dbg_draw_quality;
                    //} else {
                    sg.InterpolationMode = high_quality_shrink_mode;
                    sg.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    dbg_draw_quality = "H"+dbg_draw_quality;
                    //}
                    sg.DrawImage(bitmap, 0, 0, shrink_bitmap.Width, shrink_bitmap.Height);

                    //cache[image_filename, shrink_scale] = shrink_bitmap;
                }
                //shrink_name = image_filename;
            } catch (Exception ex) {
                // {"インデックス付きのピクセル形式をもつイメージからグラフィックス オブジェクトを作成することはできません。"}
                shrink_name = "";
                shrink_bitmap = null;
                Console.WriteLine(ex.Message);
            }
        }

        // Image Scaling
        ImageScaleMode _scale_mode = ImageScaleMode.FitToWindow;

        /// <summary>
        /// 
        /// </summary>
        public ImageScaleMode ImageScaleMode {
            get { return _scale_mode; }
            set {
                if (_scale_mode != value) {
                    _scale_mode = value;

                    if (_scale_mode == ImageScaleMode.FitToWindow)
                        offset = new Point(0, 0);

                    Invalidate();
                    _refresh_visible_rect();

                    if (ImageScaleChanged != null)
                        ImageScaleChanged(this, new EventArgs());
                }
            }
        }

        float scale = 1f;

        /// <summary>
        /// 
        /// </summary>
        public float ImageFixedScale {
            get { return scale; }
            set {
                if (scale != value) {
                    scale = value;
                    ImageScaleMode = ImageScaleMode.FixedScale;
                    Invalidate();
                    _refresh_visible_rect();

                    if (ImageScaleChanged != null)
                        ImageScaleChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ImageScaleChanged = null;

        /// <summary>
        /// 
        /// </summary>
        public int ScaleIndex {
            get
            {
                return (int)Math.Round(60*Math.Log(ActualScale)/Math.Log(2.0));
            }
            set
            {
                dbg_scale_index = value;
                ImageFixedScale = (float)Math.Pow(2.0, value/60.0);
                _refresh_visible_rect();
                if (ImageScaleChanged != null)
                    ImageScaleChanged(this, new EventArgs());
            }
        }

        int dbg_scale_index = 0;

        /// <summary>
        /// 
        /// </summary>
        public float ActualScale {
            get {
                switch (_scale_mode) {
                case ImageScaleMode.FullSize:
                    return 1.0f;
                case ImageScaleMode.FixedScale:
                    return scale;
                case ImageScaleMode.FitToWindow:
                default:
                    if (Image == null)
                        return 1f;  // ???
                    float sx = (float)Width / Image.Width;
                    float sy = (float)Height / Image.Height;
                    return (float)Math.Min(sx, sy);
                }
            }
        }

        public Size GetActualSize()
        {
            float scale = ActualScale;
            int w = (int)(Bitmap.Width*scale + 0.5);
            int h = (int)(Bitmap.Height*scale + 0.5);
            return new Size(w, h);
        }

        /// <summary>
        /// Allow scaling by mouse wheel.
        /// </summary>
        public bool ImageScaleByWheelEnabled { get; set; } = true;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (ImageScaleByWheelEnabled) {
                int delta = e.Delta / SystemInformation.MouseWheelScrollDelta;
                if ((Control.ModifierKeys & Keys.Control) != 0) {
                    InHaste = 2;
                    if ((Control.ModifierKeys & Keys.Shift) != 0)
                        ScaleIndex += delta;    // scale up/down slowly
                    else
                        ScaleIndex += 5*delta;  // scale up/down quickly
                }
            }
        }

        // scrolling
        Point offset = new Point(0, 0);
        public Point ImageScroll {
            get { return offset; }
            set {
                offset = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ImageScrolled = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void ScrollRelative(int dx, int dy)
        {
            offset.X += dx;
            offset.Y += dy;

            if (Bitmap == null)
                return;

            float scale = ActualScale;
            int w = (int)(Bitmap.Width*scale + 0.5);
            int h = (int)(Bitmap.Height*scale + 0.5);

            int m = 3;
            if (offset.X > w/2)
                offset = new Point(w/2, 0);
            else if (offset.X < -w/2)
                offset = new Point(-w/2, 0);
            else
                m ^= 1;

            if (offset.Y > h/2)
                offset = new Point(0, h/2);
            else if (offset.Y < -h/2)
                offset = new Point(0, -h/2);
            else
                m ^= 2;

            // cancel thumb scroll
            if (m != 0) {
                //Cursor.Current = Cursors.Default;
                Cursor = Cursors.Cross;
                //thumb_scroll_flag = false;
            }

            Invalidate();
            _refresh_visible_rect();

            if (ImageScrolled != null)
                ImageScrolled(this, new EventArgs());
        }

        public bool ImageScrollByMouseEnabled { get; set; } = true;
        bool mouse_down_flag = false;
        Point last_loc;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (ImageScrollByMouseEnabled
             && e.Button == MouseButtons.Left && Control.ModifierKeys == 0) {
                last_loc = e.Location;
                mouse_down_flag = true;
                Cursor.Current = Cursors.SizeAll;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouse_down_flag) {
                ScrollRelative(e.Location.X - last_loc.X, e.Location.Y - last_loc.Y);
                InHaste = 1;
            }

            if (last_loc != e.Location)
                Cursor = Cursors.Cross;

            last_loc = e.Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (mouse_down_flag) {
                mouse_down_flag = false;
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ImageScrollByKeyEnabled { get; set; } = true;

        protected override bool IsInputKey(Keys keyData)
        {
            if (ImageScrollByKeyEnabled
             && (keyData == Keys.Right || keyData == Keys.Left ||
                 keyData == Keys.Up || keyData == Keys.Down)) {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode) {
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                KeyScrollAccel(e.KeyCode);
                break;
            default:
                return;
            }
            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch (e.KeyCode) {
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
                    ScrollRelative(-key_accel, 0);
                if (keys.HasFlag(CursorKey.Right))
                    ScrollRelative(+key_accel, 0);
                if (keys.HasFlag(CursorKey.Up))
                    ScrollRelative(0, -key_accel);
                if (keys.HasFlag(CursorKey.Down))
                    ScrollRelative(0, +key_accel);
            } else
                key_accel = 0;
            last_keys = keys;
        }

        public RectangleF VisibleRect { get; private set; }

        void _refresh_visible_rect()
        {
            if (Image != null) {
                float scale = ActualScale;
                float x = (Image.Width/2 - offset.X/scale) * 1;
                float y = (Image.Height/2 - offset.Y/scale) * 1;
                float tw = Width/scale * 1;
                float th = Height/scale * 1;
                VisibleRect = new RectangleF(x-tw/2, y-th/2, tw, th);
            }
        }

        public RectangleF GetVisibleRect(float thumb_scale)
        {
            float x = VisibleRect.X*thumb_scale;
            float y = VisibleRect.Y*thumb_scale;
            float w = VisibleRect.Width*thumb_scale;
            float h = VisibleRect.Height*thumb_scale;
            if (w < 4)
                w = 4f;
            if (h < 4)
                h = 4f;
            return new RectangleF(x, y, w, h);
        }

        // lazy draw
        int haste = 0;
        Timer haste_timer = new Timer();

        public int InHaste {
            get { return haste; }
            set {
                haste = value;
                if (value > 0) {
                    haste_timer.Stop();
                    haste_timer.Interval = 333;
                    haste_timer.Tick += (sender, args) => {
                        haste_timer.Stop();
                        haste = 0;

                        if (!dbg_draw_quality.Contains('H'))
                            Invalidate();

                        if (HasteTimeouted != null)
                            HasteTimeouted(this, new EventArgs());
                    };
                    haste_timer.Start();
                }
            }
        }

        public event EventHandler HasteTimeouted = null;

        // test, debug, benchimark
        public string dbg_draw_quality { get; private set; } = "";
        long dbg_time_shrink, dbg_time_draw;

        /// <summary>
        /// 
        /// </summary>
        public string BentiStr {
            get {
                return string.Format("{0} sh{1}/dr{2}", dbg_draw_quality, dbg_time_shrink, dbg_time_draw);
            }
        }
    }
}


