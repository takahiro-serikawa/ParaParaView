// BitmapCache.cs - bitmap cache system for "para para Photo viewer"

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
        }

        //~BitmapCache() { }
        public void Dispose()
        {
            timer_min.Dispose();
            md5.Dispose();

            Finish();
        }

        // performance configuration
        float MAX_DISK_USAGE = 10*GB;
        float MIN_DISK_FREE = 10*GB;
        float MIN_MEM_FREE = 200*MB;

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
                if (e.bitmap != null)
                    bitmap = e.bitmap.Clone() as Bitmap;
                else if (e.filename != null)
                    try {
                        e.bitmap = Bitmap.FromFile(e.filename) as Bitmap;
                        bitmap = e.bitmap.Clone() as Bitmap;
                    } catch (Exception ex) {
                        Console.WriteLine("bitmap cache {0} broken: {1}", e.filename, ex.Message);
                        e.filename = null;
                    }

                if (bitmap != null) {
                    e.last_used = DateTime.Now;
                    e.used_cnt++;
                }
            }
            return bitmap;
        }

        /// <summary>
        /// キャッシュに画像を追加
        /// </summary>
        /// <param name="filename">追加する画像の読み込み元ファイル名</param>
        /// <param name="scale"></param>
        /// <param name="bitmap">追加する画像</param>
        public async void AddAsync(string filename, float scale, Bitmap bitmap)
        {
            string key = MakeKey(filename, scale);
            if (entries.ContainsKey(key))
                return; // cache duplicated

            var e = new CacheEntry(bitmap.Clone() as Bitmap);
            entries[key] = e;

            var sw = new Stopwatch();
            var b = bitmap.Clone() as Bitmap;
            var task = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);

                string cache_path = MakeFilename(key);
                //var b = bitmap.Clone() as Bitmap;
                //b.Save(cache_path, ImageFormat.Bmp);
                //b.Dispose();
                sw.Start();
                b.Save(cache_path, ImageFormat.Bmp);
                b.Dispose();
                return cache_path;
            });
            entries[key].filename = await task;
            Console.WriteLine("cache save: {0}msec", sw.ElapsedMilliseconds);

            var fi = new FileInfo(entries[key].filename);
            entries[key].size = fi.Length;
            cache_write += fi.Length;
            MemUsage += fi.Length;

            //free_mem_kb -= (ulong)(fi.Length/1024);
            //float mem_over = MIN_MEM_FREE - free_mem_kb*1024f;
            //if (mem_over > 0)
            //    CompactMem(mem_over);

            disk_usage_valid = false;
        }

        class CacheEntry
        {
            public Bitmap bitmap;       // メモリキャッシュ null ならメモリキャッシュなし
            public string filename;     // ファイル名 null ならキャッシュファイルなし
            public float size;          // ディスク専有バイト数、またはメモリ専有バイト数
            public DateTime added;      // キャッシュ登録日時
            public DateTime last_used;  // 最後にhitした日時
            public int used_cnt;        // hit回数

            public CacheEntry(Bitmap bitmap)
            {
                this.last_used = this.added = DateTime.Now;
                this.bitmap = bitmap;
                this.size = 0; // update after saved
                this.filename = null;
            }

            public CacheEntry(string cache_path, float bytes, DateTime used)
            {
                this.added = DateTime.Now;
                this.filename = cache_path;
                this.size = bytes;
                this.last_used = used;

                this.bitmap = null;
            }
        }

        Dictionary<string, CacheEntry> entries = new Dictionary<string, CacheEntry>();
        float disk_usage = 0;
        bool disk_usage_valid = false;
        System.Windows.Forms.Timer timer_min = new System.Windows.Forms.Timer();

        Thread thread;

        string cache_root;
        DriveInfo drive_info;

        /// <summary>
        /// 
        /// </summary>
        public string CachePath
        {
            get { return cache_root; }
            set
            {
                cache_root = value;
                if (!Directory.Exists(cache_root))
                    Directory.CreateDirectory(cache_root);

                drive_info = new DriveInfo(cache_root);

                var di = new DirectoryInfo(cache_root);
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
        public int FileCount { get { return entries.Count(e => e.Value.filename != null); } }

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
            return string.Format("{0}_{1:D5}", s, (int)(scale*1000));
        }

        static byte[] MakeHash(string filename)
        {
            byte[] data = Encoding.UTF8.GetBytes(filename);
            return md5.ComputeHash(data);
        }

        string MakeFilename(string key)
        {
            return cache_root + key + ".bmp";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="scale"></param>
        public void Remove(string filename, float scale)
        {
            this.Remove(MakeKey(filename, scale));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (entries.ContainsKey(key)) {
                var e = entries[key];
                if (e.bitmap != null) {
                    e.bitmap.Dispose();
                    MemUsage -= e.size;
                    MemFree += e.size;
                }
                entries.Remove(key);
            }

            string cache_path = MakeFilename(key);
            try {
                File.Delete(cache_path);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            disk_usage_valid = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="over"></param>
        public void Compact(float over)
        {
            if (over > 0) {
                var sw = Stopwatch.StartNew();
                // delete useless entries
                var list = new List<CacheEntry>(entries.Values);
                var o = list.OrderBy((x) => x.added);
                var p = o.Where((x) => { over -= x.size; return over > 0; });
                foreach (var a in p) {
                    string key = Path.GetFileNameWithoutExtension(a.filename);
                    this.Remove(key);
                }
                Console.WriteLine("Compact(); {0}msec", sw.ElapsedMilliseconds);
            }
        }

        public void Clear()
        {
            foreach (var key in new List<string>(entries.Keys))
                this.Remove(key);

            // delete all files in cache directory
            var di = new DirectoryInfo(cache_root);
            foreach (var f in di.GetFiles(/*"*.bmp"*/))
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
            foreach (var e in p) {
                e.bitmap.Dispose();
                e.bitmap = null;
                MemUsage -= e.size;
                MemFree += e.size;
            }

            GC.Collect();   //?
        }

        /// <summary>
        /// ディスクへの累積総書き込みバイト数(概算) SSD寿命評価用
        /// </summary>
        public float TotalCacheWrite { get { return total_cache_write + cache_write; } }

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

    }
}
