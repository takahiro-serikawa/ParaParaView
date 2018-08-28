﻿// BitmapCache.cs - bitmap cache system for "para para Photo viewer"

/*
 マネージメントメモリ上の Bitmap オブジェクト
 ファイルシステム C:\Users\ユーザー\AppData\Local\Temp\ParaParaView_cache\*.bmp ファイル
 のいずれか

    cache = new BitmapCache()
    void Dispose()

    void Add(string filename, float scale, Bitmap bitmap)
 
    Bitmap Get(string filename, float scale)

    void Remove(string key)
 
    void Compact(float over)
    void CompactMem(float over)

 実メモリとファイルシステムぞえぞれの使用量/空き監視
    int EntryCount, MemoryCount, FileCount
    float DiskSize, DiskFree, MemFree, DiskUsage, MemUsage

 累積総書き込みバイト数　(レジストリに保存)
    float TotalCacheWrite
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;

namespace ParaParaView
{
    /// <summary>
    /// 
    /// </summary>
    class BitmapCache: IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BitmapCache()
        {
            md5 = System.Security.Cryptography.MD5.Create();

            CachePath = Path.GetTempPath() + @"ParaParaView_cache\";

            timer_min.Interval = 60*1000;
            timer_min.Tick += _timer_Tick;
            timer_min.Enabled = true;
            _timer_Tick(null, null);

            RestorePerformance();
            writing = new BackgroundCacheWriter("writer", this);
            reading = new BackgroundCacheReader("reader", this);
        }

        BackgroundCacheWriter writing;
        BackgroundCacheReader reading;

        public void Dispose()
        {
            Finish();

            timer_min.Dispose();
            md5.Dispose();
            writing.Stop();
            reading.StopAndWait(5000);
            writing.StopAndWait(5000);
            reading.Dispose();
            writing.Dispose();
        }

        public static Bitmap BitmapFromFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return Bitmap.FromStream(stream) as Bitmap;
        }

        // performance configuration
        public float MAX_DISK_USAGE = 10*GB;
        public float MIN_DISK_FREE = 10*GB;
        public float MIN_MEM_FREE = 200*MB;

        const float GB = 1024*1024*1024;
        const float MB = 1024*1024;

        /// <summary>
        /// キャッシュから(もしあれば)画像を取得
        /// </summary>
        /// <param name="filename">キャッシュに問い合わせる画像ファイル名</param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Bitmap Get(string filename, float scale)
        {
            string key = MakeKey(filename, scale);
            Bitmap bitmap = null;
            if (entries.ContainsKey(key)) {
                var e = entries[key];
                lock (e) {
                    if (e.bitmap != null)
                        bitmap = e.bitmap.Clone() as Bitmap;
                    else if (e.cachename != null)
                        try {
                            //e.bitmap = Bitmap.FromFile(e.cachename) as Bitmap;
                            e.bitmap = BitmapCache.BitmapFromFile(e.cachename);
                            bitmap = e.bitmap.Clone() as Bitmap;
                        } catch (Exception ex) {
                            Console.WriteLine("bitmap cache {0} broken: {1}", e.cachename, ex.Message);
                            e.cachename = null;
                        }
                    if (bitmap != null) {
                        e.last_used = DateTime.Now;
                        e.used_cnt++;
                    }
                }
            }
            return bitmap;
        }

        public Bitmap GetM(string filename, float scale)
        {
            string key = MakeKey(filename, scale);
            Bitmap result = null;
            if (entries.ContainsKey(key)) {
                var e = entries[key];
                if (e.bitmap != null) {
                    result = e.bitmap.Clone() as Bitmap;
                    e.last_used = DateTime.Now;
                    e.used_cnt++;
                }
            }
            return result;
        }

        /// <summary>
        /// キャッシュに画像を追加
        /// </summary>
        /// <param name="filename">original full path filename of image</param>
        /// <param name="scale">image scale, or 0 for thumb</param>
        /// <param name="bitmap">image to add</param>
        public void Add2(string filename, float scale, Bitmap bitmap)
        {
            CacheEntry e = GetOrNew(filename, scale);
            e.bitmap = bitmap.Clone() as Bitmap;

            lock (usage_obj) {
            }

            writing.AddReq(e);  // ??
        }

        public void AddM(string filename, float scale, Bitmap bitmap)
        {
            CacheEntry e = GetOrNew(filename, scale);
            e.bitmap = bitmap.Clone() as Bitmap;

            lock (usage_obj) {
            }
        }

        CacheEntry GetOrNew(string filename, float scale)
        {
            string key = MakeKey(filename, scale);
            CacheEntry e;
            if (entries.ContainsKey(key))
                e = entries[key];
            else
                entries[key] = e = new CacheEntry();
            e.image_name = filename;
            e.scale = scale;
            return e;
        }

        public void PreLoad(IEnumerable<string> filenames, float scale)
        {
            reading.CancelAll();
            foreach (string filename in filenames) {
                CacheEntry e1 = GetOrNew(filename, 1f);
                reading.AddReq(e1);
                if (scale != 1f) {
                    CacheEntry e2 = GetOrNew(filename, scale);
                    reading.AddReq(e2);
                }
            }
        }

        public void PreLoadCancel()
        {
            reading.CancelAll();
        }

        class CacheEntry
        {
            // cache file rule
            // file is Windows bitmap
            // KEY equals FILENAME (with .bmp, but no directory)
            // KEY: MD5 hash of orginal photo filename + _ + scale + .bmp; hash is uppercase, .bmp is lowercase
            // ${CACHE_PATH}/
            //   7EE5D17FB6951C8B5B7C3685FA1B7598_01000.bmp
            //   7EE5D17FB6951C8B5B7C3685FA1B7598_00500.bmp
            //   7EE5D17FB6951C8B5B7C3685FA1B7598_00000.bmp thumbに限りscale=00000

            public Bitmap bitmap;       // メモリキャッシュ null ならメモリキャッシュなし
            public string cachename;    // fullpath of cached bitmap, or null if not exists
            public float size;          // ディスク専有バイト数、またはメモリ専有バイト数

            public string image_name;
            public float scale;

            public DateTime added;      // キャッシュ登録日時
            public DateTime last_used;  // 最後にhitした日時
            public int used_cnt;        // hit回数

            public CacheEntry()
            {
                this.last_used = this.added = DateTime.Now;
            }

            public CacheEntry(Bitmap bitmap)
            {
                this.last_used = this.added = DateTime.Now;
                this.bitmap = bitmap;
                this.size = 0; // update after saved
                this.cachename = null;
            }

            public CacheEntry(string cachename, float bytes, DateTime used)
            {
                this.added = DateTime.Now;
                this.cachename = cachename;
                this.size = bytes;
                this.last_used = used;

                this.bitmap = null;
            }

            public override string ToString()   // debug用
            {
                string b = this.bitmap != null ? string.Format("{0}x{1}", bitmap.Width, bitmap.Height) : "null";
                return string.Format("{0}:{3} {2:P1} {1}",
                    Path.GetFileNameWithoutExtension(this.cachename),
                    this.image_name, this.scale, b);
            }
        }

        ConcurrentDictionary<string, CacheEntry> entries = new ConcurrentDictionary<string, CacheEntry>();
        object usage_obj = new object();
        float disk_usage = 0;
        bool disk_usage_valid = false;
        System.Windows.Forms.Timer timer_min = new System.Windows.Forms.Timer();

        string cache_path;
        DriveInfo drive_info;

        /// <summary>
        /// switch cache directory, restore heritage cache
        /// </summary>
        public string CachePath
        {
            get { return cache_path; }
            set
            {
                cache_path = value;
                if (!Directory.Exists(cache_path))
                    Directory.CreateDirectory(cache_path);

                drive_info = new DriveInfo(cache_path);

                var di = new DirectoryInfo(cache_path);
                foreach (var fi in di.GetFiles("*.bmp"))
                    entries[fi.Name] = new CacheEntry(fi.FullName, fi.Length, fi.LastAccessTime);
            }
        }

        /// <summary>
        /// キャッシュエントリ数。無効なエントリも含む。性能評価用
        /// </summary>
        public int EntryCount { get { return entries.Count; } }

        /// <summary>
        /// メモリキャッシュエントリ数。性能評価用
        /// </summary>
        public int MemoryCount { get { return entries.Count(e => e.Value.bitmap != null); } }

        /// <summary>
        /// ファイルキャッシュエントリ数。性能評価用
        /// </summary>
        public int FileCount { get { return entries.Count(e => e.Value.cachename != null); } }

        /// <summary>
        /// キャッシュとして使用しているドライブの総バイト数。性能評価用
        /// </summary>
        public float DiskSize { get { return drive_info.TotalSize; } }

        /// <summary>
        /// キャッシュとして使用しているドライブの空きバイト数。性能評価用
        /// </summary>
        public float DiskFree { get { return drive_info.TotalFreeSpace; } }

        /// <summary>
        /// ファイルキャッシュの使用バイト数。性能評価用
        /// </summary>
        public float DiskUsage
        {
            get
            {
                if (!disk_usage_valid) {
                    disk_usage_valid = false;
                    disk_usage = entries.Sum(x => x.Value.size);
                }
                return disk_usage;
            }
        }

        /// <summary>
        /// メモリキャッシュの使用バイト数。性能評価用
        /// </summary>
        public float MemUsage { get; private set; }

        static System.Security.Cryptography.MD5 md5 = null;

        static string MakeKey(string filename, float scale)
        {
            var hash = MakeHash(filename);
            string s = BitConverter.ToString(hash).Replace("-", "");
            return string.Format("{0}_{1:D5}.bmp", s, (int)(scale*1000));
        }

        static byte[] MakeHash(string filename)
        {
            byte[] data = Encoding.UTF8.GetBytes(filename);
            byte[] hash;
            lock (md5)
                hash = md5.ComputeHash(data);
            return hash;
        }

        public string MakeCacheName(string key)
        {
            return cache_path + key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="scale"></param>
        public void Remove0(string filename, float scale)
        {
            this.Remove0(MakeKey(filename, scale));
        }

        void _free_bitmap(CacheEntry e)
        {
            lock (e)
                if (e.bitmap != null) {
                    e.bitmap.Dispose();
                    e.bitmap = null;
                }
        }

        void _free_file(CacheEntry e)
        {
            lock (e)
                if (e.cachename != null) {
                    try {
                        File.Delete(e.cachename);
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                    e.cachename = null;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove0(string key)
        {
            if (entries.TryRemove(key, out CacheEntry e)) {
                _free_bitmap(e);
                if (e.bitmap != null)
                    Console.WriteLine("Remove({0})", key);
            }

            try {
                File.Delete(MakeCacheName(key));
                disk_usage_valid = false;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="over"></param>
        public void Compact(float over)
        {
            if (over > 0) {
                var sw = Stopwatch.StartNew();
                // delete useless files
                var list = new List<CacheEntry>(entries.Values);
                var o = list.OrderBy((x) => x.added);
                var p = o.Where((x) => { over -= x.size; return over > 0; });
                foreach (var a in p)
                    _free_file(a);
                Console.WriteLine("Compact(); {0}msec", sw.ElapsedMilliseconds);
            }
        }

        public void Clear()
        {
            foreach (var e in entries.Values)
                _free_bitmap(e);

            entries.Clear();

            // delete all files in cache directory
            var di = new DirectoryInfo(cache_path);
            foreach (var f in di.GetFiles())
                try {
                    File.Delete(f.FullName);
                } catch (Exception ex) {
                    Console.WriteLine("cache clear: "+ex.Message);
                }
            disk_usage_valid = false;
        }

        /* free RAM management */
        public float MemFree { get; private set; }
        public float MemSize { get; private set; }
        ulong free_phys_kb = 0;

        void _timer_Tick(object sender, object e)
        {
            var sw = Stopwatch.StartNew();
            using (var mc = new System.Management.ManagementClass("Win32_OperatingSystem"))
            using (var moc = mc.GetInstances())
                foreach (var mo in moc) {
                    //合計物理メモリ
                    if (MemSize <= 0) {
                        MemSize = (ulong)mo["TotalVisibleMemorySize"] * 1024f;
                        Console.WriteLine("合計物理メモリ:{0}KB", mo["TotalVisibleMemorySize"]);
                    }
                    //利用可能な物理メモリ
                    //Console.WriteLine("利用可能物理メモリ:{0}KB", mo["FreePhysicalMemory"]);
                    free_phys_kb = (ulong)mo["FreePhysicalMemory"];
                    MemFree = free_phys_kb*1024;
                    //合計仮想メモリ
                    //Console.WriteLine("合計仮想メモリ:{0}KB", mo["TotalVirtualMemorySize"]);
                    //利用可能な仮想メモリ
                    //Console.WriteLine("利用可能仮想メモリ:{0}KB", mo["FreeVirtualMemory"]);

                    //他のページをスワップアウトせずにページングファイルにマップできるサイズ
                    //Console.WriteLine("FreeSpaceInPagingFiles:{0}KB", mo["FreeSpaceInPagingFiles"]);
                    //ページングファイルに保存できる合計サイズ
                    //Console.WriteLine("SizeStoredInPagingFiles:{0}KB", mo["SizeStoredInPagingFiles"]);
                    //スワップスペースの合計サイズ
                    //スワップスペースとページングファイルが区別されていなければ、NULL
                    //Console.WriteLine("TotalSwapSpaceSize:{0}KB", mo["TotalSwapSpaceSize"]);
                    mo.Dispose();
                }

            if (MemFree < MIN_MEM_FREE)
                CompactMem(MIN_MEM_FREE - MemFree);
            if (DiskUsage > MAX_DISK_USAGE)
                Compact(DiskUsage - MAX_DISK_USAGE);
            if (DiskFree < MIN_DISK_FREE)
                Compact(MIN_DISK_FREE - DiskFree);
        }

        /// <summary>
        /// 空きメモリが圧迫されているときにメモリ開放を試みる・。
        /// </summary>
        /// <param name="over"></param>
        public void CompactMem(float over)
        {
            var list = new List<CacheEntry>(entries.Values);
            var o = list.OrderBy((x) => x.last_used);
            var p = o.Where((x) =>
            {
                if (x.bitmap == null)
                    return false;
                over -= x.size;
                return over > 0;
            });
            foreach (var e in p)
                _free_bitmap(e);

            GC.Collect();   //?
        }

        /// <summary>
        /// ディスクへの累積総書き込みバイト数(概算) SSD寿命評価用
        /// </summary>
        public float TotalCacheWrite
        {
            get
            {
                float result;
                lock (usage_obj) {
                    result = total_cache_write + cache_write;
                }
                return result;
            }
        }

        const string PERFORMANCE_KEY = @"Software\ParaParaView\Performance";
        const string TOTAL_CACHE_WRITE = @"total_cache_write";
        float total_cache_write = 0;
        float cache_write = 0;

        void RestorePerformance()
        {
            using (var reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PERFORMANCE_KEY)) {
                var s = (string)reg.GetValue(TOTAL_CACHE_WRITE, "0");
                float.TryParse(s, out total_cache_write);
                cache_write = 0;
            }
        }

        public void Finish()
        {
            // TODO 複数アプリから使う場合、要排他ロック

            using (var reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PERFORMANCE_KEY)) {
                var s = (string)reg.GetValue(TOTAL_CACHE_WRITE, "0");
                float.TryParse(s, out total_cache_write);
                total_cache_write += cache_write;
                // save as string, it will be too large number
                reg.SetValue(TOTAL_CACHE_WRITE, total_cache_write.ToString());
                reg.SetValue("last_update_time", DateTime.Now.ToShortDateString());
            }
        }

        abstract class Backgrounder: IDisposable
        {
            protected BitmapCache cache;
            public string Name { get; private set; }

            public Backgrounder(string name, BitmapCache cache)
            {
                this.Name = name;
                this.cache = cache;
                thread = new Thread(ThreadProc);
                thread.Priority = ThreadPriority.Lowest;
                thread.IsBackground = true;
                thread.Start();
            }

            public void Dispose()
            {
                ev.Dispose();
            }

            Thread thread;
            volatile bool stop_flag = false;
            AutoResetEvent ev = new AutoResetEvent(false);
            ConcurrentQueue<CacheEntry> queue = new ConcurrentQueue<CacheEntry>();
            public int ProcDelay { get; set; }
            protected abstract void Proc(CacheEntry e);

            void ThreadProc()
            {
                for (; !stop_flag;) {
                    if (ProcDelay > 0)
                        Thread.Sleep(ProcDelay);

                    CacheEntry e;
                    while (queue.TryDequeue(out e))
                        try {
                            Proc(e);
                        } catch (Exception ex) {
                            Console.WriteLine(Name + ":" + ex.Message);
                        }

                    ev.WaitOne();
                }
                Console.WriteLine(Name + " quit normally");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            public void AddReq(CacheEntry e)
            {
                //lock (queue)
                queue.Enqueue(e);
                ev.Set();
            }

            /// <summary>
            /// Stop background thread.
            /// </summary>
            public void Stop()
            {
                stop_flag = true;
                ev.Set();
            }

            public bool StopAndWait(int msec)
            {
                CancelAll();
                stop_flag = true;
                ev.Set();
                return thread.Join(msec);
            }

            public void CancelAll()
            {
                //queue.Clear();
                CacheEntry e;
                while (queue.TryDequeue(out e))
                    ;
            }
        }

        class BackgroundCacheWriter: Backgrounder
        {
            public BackgroundCacheWriter(string name, BitmapCache cache) : base(name, cache)
            {
                ProcDelay = 3000;
            }

            protected override void Proc(CacheEntry e)
            {
                if (e.bitmap == null)
                    return;

                var key = MakeKey(e.image_name, e.scale);
                string cachename = cache.MakeCacheName(key);
                Bitmap b;
                lock (e)
                    b = e.bitmap.Clone() as Bitmap;
                b.Save(cachename, ImageFormat.Bmp);
                b.Dispose();
                //using (var b = e.bitmap.Clone() as Bitmap)
                //    b.Save(cachename, ImageFormat.Bmp);
                e.cachename = cachename;

                lock (cache.usage_obj) {
                    var fi = new FileInfo(e.cachename);
                    cache.cache_write += fi.Length;
                    cache.disk_usage_valid = false;
                    cache.MemUsage += fi.Length;
                }
            }
        }

        public static long EstimateBitmapSize(Bitmap bmp)
        {
            return bmp.Width * bmp.Height * 3;
        }

        class BackgroundCacheReader: Backgrounder
        {
            public BackgroundCacheReader(string name, BitmapCache cache) : base(name, cache)
            {
                //ProcDelay = 3000;
            }

            protected override void Proc(CacheEntry e)
            {
                Console.WriteLine("preload: {0}", e.ToString());

                if (e.bitmap != null) {
                    Console.WriteLine("already");
                    return;
                }

                long size = 0;
                lock (e) {
                    if (e.cachename != null)
                        try {
                            var sw0 = Stopwatch.StartNew();
                            //e.bitmap = Bitmap.FromFile(e.cachename) as Bitmap;
                            e.bitmap = BitmapCache.BitmapFromFile(e.cachename);
                            Console.WriteLine("load from cache {0}; {1}msec", Path.GetFileName(e.cachename), sw0.ElapsedMilliseconds);
                            return;
                        } catch (Exception ex) {
                            e.cachename = null;
                            Console.WriteLine(ex.Message);
                        }

                    var full = cache.GetM(e.image_name, 1f);
                    if (full != null)
                        Console.WriteLine("full image on mem");

                    if (full == null) {
                        var sw1 = Stopwatch.StartNew();
                        //full = Bitmap.FromFile(e.image_name) as Bitmap;
                        full = BitmapCache.BitmapFromFile(e.image_name);
                        Console.WriteLine("original image {0}", e.image_name, sw1.ElapsedMilliseconds);
                    }

                    if (e.scale <= 0)
                        throw new Exception("illegal preload scale");

                    if (e.scale < 1f) {
                        // make shrinked cache
                        var sw2 = Stopwatch.StartNew();
                        int w = (int)(full.Width*e.scale + 0.5);
                        int h = (int)(full.Height*e.scale + 0.5);
                        var shrink = new Bitmap(w, h);
                        using (var sg = Graphics.FromImage(shrink)) {
                            sg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                            sg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                            sg.DrawImage(full, 0, 0, w, h);
                        }
                        Console.WriteLine("shrink {0:P1}; {1}msec", e.scale, sw2.ElapsedMilliseconds);

                        e.bitmap = shrink;
                        //cache.Add2(e.image_name, 1f, bitmap);
                    } else
                        e.bitmap = full;

                    size = EstimateBitmapSize(e.bitmap);
                }

                lock (cache.usage_obj) {
                    //var fi = new FileInfo(e.filename);
                    cache.cache_write += size;
                    cache.disk_usage_valid = false;
                    cache.MemUsage += size;
                    if (cache.MemFree < cache.MIN_MEM_FREE)
                        CancelAll();
                }
            }
        }
    }
}