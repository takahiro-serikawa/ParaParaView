namespace ParaParaView
{
    partial class ParaParaMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (thumb_bitmap != null)
                thumb_bitmap.Dispose();
            //if (shrink_bitmap != null)
            //    shrink_bitmap.Dispose();
            cache.Dispose();
            thumb_pen.Dispose();

            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timer10 = new System.Windows.Forms.Timer(this.components);
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RecentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.FilePrintItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileaSaveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileCloseItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EjectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.AppSettingsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearCacheItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.FileDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileTrashItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenTrashItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExplorerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyFullPathItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditCutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditCopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditPasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScaleUpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScaleDownItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.RotateRightItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RotateLeftItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipHorizItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipVertItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.ScrollMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollRightItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollLeftItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollUpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollDownItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollCenterItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewRefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FitToWindowItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FullSizeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FullScreenItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.OnlyPhotoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewExifItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewPortItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScaleBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewDebugItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrowseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PhotoPrevItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PhotoNextItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PhotoHomeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PhotoEndItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShuffleNextItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PhotoBackItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.SlideShowItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SlideShowSettItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpAboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FullScreenLabel = new System.Windows.Forms.Label();
            this.DebugBox = new System.Windows.Forms.GroupBox();
            this.CacheLabel = new System.Windows.Forms.Label();
            this.ScanAsyncLabel = new System.Windows.Forms.Label();
            this.dbgMemoryLabel = new System.Windows.Forms.Label();
            this.ControlNameLabel = new System.Windows.Forms.Label();
            this.DrawBenchLabel = new System.Windows.Forms.Label();
            this.IndexLabel = new System.Windows.Forms.Label();
            this.MediaSpaceLabel = new System.Windows.Forms.Label();
            this.MediaSpace = new System.Windows.Forms.PictureBox();
            this.CursorLabel = new System.Windows.Forms.Label();
            this.DebugLabel = new System.Windows.Forms.Label();
            this.DebugLog = new System.Windows.Forms.RichTextBox();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FSWatcher = new System.IO.FileSystemWatcher();
            this.SecTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Thumb = new System.Windows.Forms.PictureBox();
            this.SlideShowTimer = new System.Windows.Forms.Timer(this.components);
            this.ScaleBar = new System.Windows.Forms.TrackBar();
            this.ExifBox = new System.Windows.Forms.Panel();
            this.Filename = new System.Windows.Forms.TextBox();
            this.Exif = new System.Windows.Forms.TextBox();
            this.ExifLabel = new System.Windows.Forms.Label();
            this.ScalePanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ScaleEdit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReciprocalLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ViewPort = new System.Windows.Forms.Panel();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.PageUpDownTimer = new System.Windows.Forms.Timer(this.components);
            this.Photo = new ParaParaView.ParaParaImage();
            this.mainMenuStrip.SuspendLayout();
            this.DebugBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediaSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Thumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBar)).BeginInit();
            this.ExifBox.SuspendLayout();
            this.ScalePanel.SuspendLayout();
            this.ViewPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Photo)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer10
            // 
            this.Timer10.Enabled = true;
            this.Timer10.Interval = 10;
            this.Timer10.Tick += new System.EventHandler(this.Timer10_Tick);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.ViewMenu,
            this.WindowMenu,
            this.BrowseMenu,
            this.HelpMenu});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(640, 26);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            this.mainMenuStrip.MenuActivate += new System.EventHandler(this.MainMenuStrip_MenuActivate);
            this.mainMenuStrip.MenuDeactivate += new System.EventHandler(this.MainMenuStrip_MenuDeactivate);
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileNewItem,
            this.OpenFolderItem,
            this.OpenFileItem,
            this.RecentMenu,
            this.toolStripMenuItem9,
            this.FilePrintItem,
            this.FileaSaveItem,
            this.FileCloseItem,
            this.EjectItem,
            this.toolStripMenuItem3,
            this.AppSettingsItem,
            this.ClearCacheItem,
            this.AppExitItem});
            this.FileMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(40, 22);
            this.FileMenu.Text = "&File";
            // 
            // FileNewItem
            // 
            this.FileNewItem.Name = "FileNewItem";
            this.FileNewItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.FileNewItem.Size = new System.Drawing.Size(210, 22);
            this.FileNewItem.Text = "&New ...";
            this.FileNewItem.Click += new System.EventHandler(this.FileNewItem_Click);
            // 
            // OpenFolderItem
            // 
            this.OpenFolderItem.Name = "OpenFolderItem";
            this.OpenFolderItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenFolderItem.Size = new System.Drawing.Size(210, 22);
            this.OpenFolderItem.Text = "&Open Folder ...";
            this.OpenFolderItem.Click += new System.EventHandler(this.OpenFolderItem_Click);
            // 
            // OpenFileItem
            // 
            this.OpenFileItem.Name = "OpenFileItem";
            this.OpenFileItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.OpenFileItem.Size = new System.Drawing.Size(210, 22);
            this.OpenFileItem.Text = "Open &Image ...";
            this.OpenFileItem.ToolTipText = "画像ファイル、または画像カタログを開きます。";
            this.OpenFileItem.Click += new System.EventHandler(this.OpenImageItem_Click);
            // 
            // RecentMenu
            // 
            this.RecentMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8});
            this.RecentMenu.Name = "RecentMenu";
            this.RecentMenu.Size = new System.Drawing.Size(210, 22);
            this.RecentMenu.Text = "&Recent";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(57, 6);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(207, 6);
            // 
            // FilePrintItem
            // 
            this.FilePrintItem.Name = "FilePrintItem";
            this.FilePrintItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.FilePrintItem.Size = new System.Drawing.Size(210, 22);
            this.FilePrintItem.Text = "&Print ...";
            this.FilePrintItem.Click += new System.EventHandler(this.FilePrintItem_Click);
            // 
            // FileaSaveItem
            // 
            this.FileaSaveItem.Name = "FileaSaveItem";
            this.FileaSaveItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.FileaSaveItem.Size = new System.Drawing.Size(210, 22);
            this.FileaSaveItem.Text = "&Save ...";
            this.FileaSaveItem.Click += new System.EventHandler(this.FileaSaveItem_Click);
            // 
            // FileCloseItem
            // 
            this.FileCloseItem.Name = "FileCloseItem";
            this.FileCloseItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.FileCloseItem.Size = new System.Drawing.Size(210, 22);
            this.FileCloseItem.Text = "&Close";
            this.FileCloseItem.Click += new System.EventHandler(this.FileCloseItem_Click);
            // 
            // EjectItem
            // 
            this.EjectItem.Name = "EjectItem";
            this.EjectItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.EjectItem.Size = new System.Drawing.Size(210, 22);
            this.EjectItem.Text = "e&Ject ...";
            this.EjectItem.Click += new System.EventHandler(this.EjectItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(207, 6);
            // 
            // AppSettingsItem
            // 
            this.AppSettingsItem.Name = "AppSettingsItem";
            this.AppSettingsItem.Size = new System.Drawing.Size(210, 22);
            this.AppSettingsItem.Text = "Preferences ...";
            // 
            // ClearCacheItem
            // 
            this.ClearCacheItem.Name = "ClearCacheItem";
            this.ClearCacheItem.Size = new System.Drawing.Size(210, 22);
            this.ClearCacheItem.Text = "dbg:Clear Cache";
            this.ClearCacheItem.Visible = false;
            this.ClearCacheItem.Click += new System.EventHandler(this.ClearCacheItem_Click);
            // 
            // AppExitItem
            // 
            this.AppExitItem.Name = "AppExitItem";
            this.AppExitItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.AppExitItem.Size = new System.Drawing.Size(210, 22);
            this.AppExitItem.Text = "e&Xit";
            this.AppExitItem.Click += new System.EventHandler(this.AppExitItem_Click);
            // 
            // EditMenu
            // 
            this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.FileDeleteItem,
            this.FileTrashItem,
            this.OpenTrashItem,
            this.ExplorerItem,
            this.CopyFullPathItem,
            this.toolStripMenuItem1,
            this.EditCutItem,
            this.EditCopyItem,
            this.EditPasteItem});
            this.EditMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.EditMenu.Name = "EditMenu";
            this.EditMenu.Size = new System.Drawing.Size(42, 22);
            this.EditMenu.Text = "&Edit";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 6);
            // 
            // FileDeleteItem
            // 
            this.FileDeleteItem.Name = "FileDeleteItem";
            this.FileDeleteItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.FileDeleteItem.Size = new System.Drawing.Size(188, 22);
            this.FileDeleteItem.Text = "&Delete ...";
            this.FileDeleteItem.Click += new System.EventHandler(this.FileDeleteItem_Click);
            // 
            // FileTrashItem
            // 
            this.FileTrashItem.Name = "FileTrashItem";
            this.FileTrashItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.FileTrashItem.Size = new System.Drawing.Size(188, 22);
            this.FileTrashItem.Text = "&Trash ...";
            this.FileTrashItem.Click += new System.EventHandler(this.FileTrashItem_Click);
            // 
            // OpenTrashItem
            // 
            this.OpenTrashItem.Name = "OpenTrashItem";
            this.OpenTrashItem.Size = new System.Drawing.Size(188, 22);
            this.OpenTrashItem.Tag = "OPEN_TRASH";
            this.OpenTrashItem.Text = "Open Trash ...";
            this.OpenTrashItem.Click += new System.EventHandler(this.ActionHandler);
            // 
            // ExplorerItem
            // 
            this.ExplorerItem.Name = "ExplorerItem";
            this.ExplorerItem.Size = new System.Drawing.Size(188, 22);
            this.ExplorerItem.Text = "Explorer ...";
            this.ExplorerItem.Click += new System.EventHandler(this.ExplorerItem_Click);
            // 
            // CopyFullPathItem
            // 
            this.CopyFullPathItem.Name = "CopyFullPathItem";
            this.CopyFullPathItem.Size = new System.Drawing.Size(188, 22);
            this.CopyFullPathItem.Text = "Copy &FullPath";
            this.CopyFullPathItem.Click += new System.EventHandler(this.CopyFullPathItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 6);
            // 
            // EditCutItem
            // 
            this.EditCutItem.Enabled = false;
            this.EditCutItem.Name = "EditCutItem";
            this.EditCutItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.EditCutItem.Size = new System.Drawing.Size(188, 22);
            this.EditCutItem.Text = "Cu&T";
            // 
            // EditCopyItem
            // 
            this.EditCopyItem.Name = "EditCopyItem";
            this.EditCopyItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.EditCopyItem.Size = new System.Drawing.Size(188, 22);
            this.EditCopyItem.Text = "&Copy";
            this.EditCopyItem.Click += new System.EventHandler(this.EditCopyItem_Click);
            // 
            // EditPasteItem
            // 
            this.EditPasteItem.Enabled = false;
            this.EditPasteItem.Name = "EditPasteItem";
            this.EditPasteItem.ShortcutKeyDisplayString = "Ctrl+P";
            this.EditPasteItem.Size = new System.Drawing.Size(188, 22);
            this.EditPasteItem.Text = "&Paste";
            this.EditPasteItem.Click += new System.EventHandler(this.EditPasteItem_Click);
            // 
            // ViewMenu
            // 
            this.ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScaleUpItem,
            this.ScaleDownItem,
            this.toolStripMenuItem5,
            this.RotateRightItem,
            this.RotateLeftItem,
            this.FlipHorizItem,
            this.FlipVertItem,
            this.toolStripMenuItem7,
            this.ScrollMenu,
            this.ScrollCenterItem,
            this.ViewRefreshItem});
            this.ViewMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ViewMenu.Name = "ViewMenu";
            this.ViewMenu.Size = new System.Drawing.Size(48, 22);
            this.ViewMenu.Text = "&View";
            // 
            // ScaleUpItem
            // 
            this.ScaleUpItem.Name = "ScaleUpItem";
            this.ScaleUpItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F9)));
            this.ScaleUpItem.Size = new System.Drawing.Size(233, 22);
            this.ScaleUpItem.Tag = "5";
            this.ScaleUpItem.Text = "Scale &Up";
            this.ScaleUpItem.Click += new System.EventHandler(this.ScaleUpItem_Click);
            // 
            // ScaleDownItem
            // 
            this.ScaleDownItem.Name = "ScaleDownItem";
            this.ScaleDownItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9)));
            this.ScaleDownItem.Size = new System.Drawing.Size(233, 22);
            this.ScaleDownItem.Tag = "-5";
            this.ScaleDownItem.Text = "Scale &Down";
            this.ScaleDownItem.Click += new System.EventHandler(this.ScaleDownItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(230, 6);
            // 
            // RotateRightItem
            // 
            this.RotateRightItem.Name = "RotateRightItem";
            this.RotateRightItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.RotateRightItem.Size = new System.Drawing.Size(233, 22);
            this.RotateRightItem.Tag = "1";
            this.RotateRightItem.Text = "Rotate &Right";
            this.RotateRightItem.Click += new System.EventHandler(this.ViewOrientItem_Click);
            // 
            // RotateLeftItem
            // 
            this.RotateLeftItem.Name = "RotateLeftItem";
            this.RotateLeftItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F2)));
            this.RotateLeftItem.Size = new System.Drawing.Size(233, 22);
            this.RotateLeftItem.Tag = "3";
            this.RotateLeftItem.Text = "Rotate &Left";
            this.RotateLeftItem.Click += new System.EventHandler(this.ViewOrientItem_Click);
            // 
            // FlipHorizItem
            // 
            this.FlipHorizItem.Name = "FlipHorizItem";
            this.FlipHorizItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
            this.FlipHorizItem.Size = new System.Drawing.Size(233, 22);
            this.FlipHorizItem.Tag = "4";
            this.FlipHorizItem.Text = "Flip &Horizontal";
            this.FlipHorizItem.Click += new System.EventHandler(this.ViewOrientItem_Click);
            // 
            // FlipVertItem
            // 
            this.FlipVertItem.Name = "FlipVertItem";
            this.FlipVertItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F2)));
            this.FlipVertItem.Size = new System.Drawing.Size(233, 22);
            this.FlipVertItem.Tag = "6";
            this.FlipVertItem.Text = "Flip &Vertical";
            this.FlipVertItem.Click += new System.EventHandler(this.ViewOrientItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(230, 6);
            // 
            // ScrollMenu
            // 
            this.ScrollMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScrollRightItem,
            this.ScrollLeftItem,
            this.ScrollUpItem,
            this.ScrollDownItem});
            this.ScrollMenu.Name = "ScrollMenu";
            this.ScrollMenu.Size = new System.Drawing.Size(233, 22);
            this.ScrollMenu.Text = "&Scroll";
            // 
            // ScrollRightItem
            // 
            this.ScrollRightItem.Name = "ScrollRightItem";
            this.ScrollRightItem.ShortcutKeyDisplayString = "←";
            this.ScrollRightItem.Size = new System.Drawing.Size(129, 22);
            this.ScrollRightItem.Text = "&Right";
            this.ScrollRightItem.Click += new System.EventHandler(this.Scroll_Click);
            // 
            // ScrollLeftItem
            // 
            this.ScrollLeftItem.Name = "ScrollLeftItem";
            this.ScrollLeftItem.ShortcutKeyDisplayString = "→";
            this.ScrollLeftItem.Size = new System.Drawing.Size(129, 22);
            this.ScrollLeftItem.Text = "Left";
            this.ScrollLeftItem.Click += new System.EventHandler(this.Scroll_Click);
            // 
            // ScrollUpItem
            // 
            this.ScrollUpItem.Name = "ScrollUpItem";
            this.ScrollUpItem.ShortcutKeyDisplayString = "↑";
            this.ScrollUpItem.Size = new System.Drawing.Size(129, 22);
            this.ScrollUpItem.Text = "Up";
            this.ScrollUpItem.Click += new System.EventHandler(this.Scroll_Click);
            // 
            // ScrollDownItem
            // 
            this.ScrollDownItem.Name = "ScrollDownItem";
            this.ScrollDownItem.ShortcutKeyDisplayString = "↓";
            this.ScrollDownItem.Size = new System.Drawing.Size(129, 22);
            this.ScrollDownItem.Text = "Down";
            this.ScrollDownItem.Click += new System.EventHandler(this.Scroll_Click);
            // 
            // ScrollCenterItem
            // 
            this.ScrollCenterItem.Name = "ScrollCenterItem";
            this.ScrollCenterItem.Size = new System.Drawing.Size(233, 22);
            this.ScrollCenterItem.Text = "&Center";
            this.ScrollCenterItem.Click += new System.EventHandler(this.ScrollCenterItem_Click);
            // 
            // ViewRefreshItem
            // 
            this.ViewRefreshItem.Name = "ViewRefreshItem";
            this.ViewRefreshItem.Size = new System.Drawing.Size(233, 22);
            this.ViewRefreshItem.Text = "dbg:Refresh";
            this.ViewRefreshItem.Visible = false;
            this.ViewRefreshItem.Click += new System.EventHandler(this.ViewRefreshItem_Click);
            // 
            // WindowMenu
            // 
            this.WindowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FitToWindowItem,
            this.FullSizeItem,
            this.FullScreenItem,
            this.toolStripMenuItem6,
            this.OnlyPhotoItem,
            this.ViewExifItem,
            this.ViewPortItem,
            this.ScaleBarItem,
            this.ViewDebugItem});
            this.WindowMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.WindowMenu.Name = "WindowMenu";
            this.WindowMenu.Size = new System.Drawing.Size(66, 22);
            this.WindowMenu.Text = "&Window";
            // 
            // FitToWindowItem
            // 
            this.FitToWindowItem.Checked = true;
            this.FitToWindowItem.CheckOnClick = true;
            this.FitToWindowItem.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.FitToWindowItem.Name = "FitToWindowItem";
            this.FitToWindowItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.FitToWindowItem.Size = new System.Drawing.Size(179, 22);
            this.FitToWindowItem.Tag = "";
            this.FitToWindowItem.Text = "Fit to &Window";
            this.FitToWindowItem.Click += new System.EventHandler(this.FitToWindowItem_Click);
            // 
            // FullSizeItem
            // 
            this.FullSizeItem.CheckOnClick = true;
            this.FullSizeItem.Name = "FullSizeItem";
            this.FullSizeItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.FullSizeItem.Size = new System.Drawing.Size(179, 22);
            this.FullSizeItem.Text = "Full &Size";
            this.FullSizeItem.Click += new System.EventHandler(this.FullSizeItem_Click);
            // 
            // FullScreenItem
            // 
            this.FullScreenItem.Name = "FullScreenItem";
            this.FullScreenItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.FullScreenItem.Size = new System.Drawing.Size(179, 22);
            this.FullScreenItem.Text = "&FullScreen";
            this.FullScreenItem.Click += new System.EventHandler(this.FullScreenItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(176, 6);
            // 
            // OnlyPhotoItem
            // 
            this.OnlyPhotoItem.CheckOnClick = true;
            this.OnlyPhotoItem.Name = "OnlyPhotoItem";
            this.OnlyPhotoItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.OnlyPhotoItem.Size = new System.Drawing.Size(179, 22);
            this.OnlyPhotoItem.Tag = "TOOLS_VISIBILITY";
            this.OnlyPhotoItem.Text = "&Only Photo";
            this.OnlyPhotoItem.Click += new System.EventHandler(this.OnlyPhotoItem_Click);
            // 
            // ViewExifItem
            // 
            this.ViewExifItem.Checked = true;
            this.ViewExifItem.CheckOnClick = true;
            this.ViewExifItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewExifItem.Name = "ViewExifItem";
            this.ViewExifItem.Size = new System.Drawing.Size(179, 22);
            this.ViewExifItem.Text = "EXIF";
            this.ViewExifItem.Click += new System.EventHandler(this.ExifItem_Click);
            // 
            // ViewPortItem
            // 
            this.ViewPortItem.Checked = true;
            this.ViewPortItem.CheckOnClick = true;
            this.ViewPortItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewPortItem.Name = "ViewPortItem";
            this.ViewPortItem.Size = new System.Drawing.Size(179, 22);
            this.ViewPortItem.Text = "ViewPort";
            this.ViewPortItem.Click += new System.EventHandler(this.ViewPortItem_Click);
            // 
            // ScaleBarItem
            // 
            this.ScaleBarItem.Checked = true;
            this.ScaleBarItem.CheckOnClick = true;
            this.ScaleBarItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ScaleBarItem.Name = "ScaleBarItem";
            this.ScaleBarItem.Size = new System.Drawing.Size(179, 22);
            this.ScaleBarItem.Text = "ScaleBar";
            this.ScaleBarItem.Click += new System.EventHandler(this.ScaleBarItem_Click);
            // 
            // ViewDebugItem
            // 
            this.ViewDebugItem.CheckOnClick = true;
            this.ViewDebugItem.Name = "ViewDebugItem";
            this.ViewDebugItem.Size = new System.Drawing.Size(179, 22);
            this.ViewDebugItem.Text = "debug";
            this.ViewDebugItem.Click += new System.EventHandler(this.DebugItem_Click);
            // 
            // BrowseMenu
            // 
            this.BrowseMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PhotoPrevItem,
            this.PhotoNextItem,
            this.PhotoHomeItem,
            this.PhotoEndItem,
            this.ShuffleNextItem,
            this.PhotoBackItem,
            this.toolStripMenuItem4,
            this.SlideShowItem,
            this.SlideShowSettItem});
            this.BrowseMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BrowseMenu.Name = "BrowseMenu";
            this.BrowseMenu.Size = new System.Drawing.Size(63, 22);
            this.BrowseMenu.Text = "&Browse";
            // 
            // PhotoPrevItem
            // 
            this.PhotoPrevItem.Name = "PhotoPrevItem";
            this.PhotoPrevItem.ShortcutKeyDisplayString = "PageUp";
            this.PhotoPrevItem.Size = new System.Drawing.Size(201, 22);
            this.PhotoPrevItem.Text = "&Previous";
            this.PhotoPrevItem.Click += new System.EventHandler(this.PhotoPrevItem_Click);
            // 
            // PhotoNextItem
            // 
            this.PhotoNextItem.Name = "PhotoNextItem";
            this.PhotoNextItem.ShortcutKeyDisplayString = "PageDown";
            this.PhotoNextItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.PhotoNextItem.Size = new System.Drawing.Size(201, 22);
            this.PhotoNextItem.Text = "&Next";
            this.PhotoNextItem.Click += new System.EventHandler(this.PhotoNextItem_Click);
            // 
            // PhotoHomeItem
            // 
            this.PhotoHomeItem.Name = "PhotoHomeItem";
            this.PhotoHomeItem.ShortcutKeyDisplayString = "Home";
            this.PhotoHomeItem.Size = new System.Drawing.Size(201, 22);
            this.PhotoHomeItem.Text = "&Home";
            this.PhotoHomeItem.Click += new System.EventHandler(this.PhotoHomeItem_Click);
            // 
            // PhotoEndItem
            // 
            this.PhotoEndItem.Name = "PhotoEndItem";
            this.PhotoEndItem.Size = new System.Drawing.Size(201, 22);
            this.PhotoEndItem.Text = "end";
            this.PhotoEndItem.Visible = false;
            this.PhotoEndItem.Click += new System.EventHandler(this.PhotoEndItem_Click);
            // 
            // ShuffleNextItem
            // 
            this.ShuffleNextItem.Name = "ShuffleNextItem";
            this.ShuffleNextItem.ShortcutKeyDisplayString = "End";
            this.ShuffleNextItem.Size = new System.Drawing.Size(201, 22);
            this.ShuffleNextItem.Text = "&Shuffle";
            this.ShuffleNextItem.Click += new System.EventHandler(this.ShuffleNextItem_Click);
            // 
            // PhotoBackItem
            // 
            this.PhotoBackItem.Name = "PhotoBackItem";
            this.PhotoBackItem.ShortcutKeyDisplayString = "Back Space";
            this.PhotoBackItem.Size = new System.Drawing.Size(201, 22);
            this.PhotoBackItem.Text = "&Back";
            this.PhotoBackItem.Click += new System.EventHandler(this.PhotoBackItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(198, 6);
            // 
            // SlideShowItem
            // 
            this.SlideShowItem.CheckOnClick = true;
            this.SlideShowItem.Name = "SlideShowItem";
            this.SlideShowItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.SlideShowItem.Size = new System.Drawing.Size(201, 22);
            this.SlideShowItem.Text = "Slide Show";
            this.SlideShowItem.Click += new System.EventHandler(this.SlideShowItem_Click);
            // 
            // SlideShowSettItem
            // 
            this.SlideShowSettItem.Name = "SlideShowSettItem";
            this.SlideShowSettItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.SlideShowSettItem.Size = new System.Drawing.Size(201, 22);
            this.SlideShowSettItem.Text = " settings ...";
            this.SlideShowSettItem.Click += new System.EventHandler(this.SlideShowSettItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpAboutItem});
            this.HelpMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(46, 22);
            this.HelpMenu.Text = "&Help";
            // 
            // HelpAboutItem
            // 
            this.HelpAboutItem.Name = "HelpAboutItem";
            this.HelpAboutItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.HelpAboutItem.Size = new System.Drawing.Size(238, 22);
            this.HelpAboutItem.Text = "&About \'ParaParaView\' ...";
            this.HelpAboutItem.Click += new System.EventHandler(this.HelpAboutItem_Click);
            // 
            // FullScreenLabel
            // 
            this.FullScreenLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FullScreenLabel.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FullScreenLabel.Location = new System.Drawing.Point(206, 26);
            this.FullScreenLabel.Name = "FullScreenLabel";
            this.FullScreenLabel.Size = new System.Drawing.Size(400, 24);
            this.FullScreenLabel.TabIndex = 3;
            this.FullScreenLabel.Text = "Press F11 to Exit Full Screen";
            this.FullScreenLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.FullScreenLabel.Visible = false;
            this.FullScreenLabel.Click += new System.EventHandler(this.FullScreenItem_Click);
            // 
            // DebugBox
            // 
            this.DebugBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugBox.BackColor = System.Drawing.Color.Black;
            this.DebugBox.Controls.Add(this.CacheLabel);
            this.DebugBox.Controls.Add(this.ScanAsyncLabel);
            this.DebugBox.Controls.Add(this.dbgMemoryLabel);
            this.DebugBox.Controls.Add(this.ControlNameLabel);
            this.DebugBox.Controls.Add(this.DrawBenchLabel);
            this.DebugBox.Controls.Add(this.IndexLabel);
            this.DebugBox.Controls.Add(this.MediaSpaceLabel);
            this.DebugBox.Controls.Add(this.MediaSpace);
            this.DebugBox.Controls.Add(this.CursorLabel);
            this.DebugBox.Controls.Add(this.DebugLabel);
            this.DebugBox.Controls.Add(this.DebugLog);
            this.DebugBox.Location = new System.Drawing.Point(100, 247);
            this.DebugBox.Name = "DebugBox";
            this.DebugBox.Size = new System.Drawing.Size(500, 181);
            this.DebugBox.TabIndex = 4;
            this.DebugBox.TabStop = false;
            this.DebugBox.Text = "debug";
            this.DebugBox.Visible = false;
            // 
            // CacheLabel
            // 
            this.CacheLabel.AutoSize = true;
            this.CacheLabel.Location = new System.Drawing.Point(117, 19);
            this.CacheLabel.Name = "CacheLabel";
            this.CacheLabel.Size = new System.Drawing.Size(40, 15);
            this.CacheLabel.TabIndex = 23;
            this.CacheLabel.Text = "cache";
            // 
            // ScanAsyncLabel
            // 
            this.ScanAsyncLabel.AutoSize = true;
            this.ScanAsyncLabel.Location = new System.Drawing.Point(325, 120);
            this.ScanAsyncLabel.Name = "ScanAsyncLabel";
            this.ScanAsyncLabel.Size = new System.Drawing.Size(33, 15);
            this.ScanAsyncLabel.TabIndex = 20;
            this.ScanAsyncLabel.Text = "scan";
            // 
            // dbgMemoryLabel
            // 
            this.dbgMemoryLabel.AutoSize = true;
            this.dbgMemoryLabel.Location = new System.Drawing.Point(117, 37);
            this.dbgMemoryLabel.Name = "dbgMemoryLabel";
            this.dbgMemoryLabel.Size = new System.Drawing.Size(38, 15);
            this.dbgMemoryLabel.TabIndex = 18;
            this.dbgMemoryLabel.Text = "mem";
            // 
            // ControlNameLabel
            // 
            this.ControlNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ControlNameLabel.AutoSize = true;
            this.ControlNameLabel.Location = new System.Drawing.Point(212, 0);
            this.ControlNameLabel.Name = "ControlNameLabel";
            this.ControlNameLabel.Size = new System.Drawing.Size(47, 15);
            this.ControlNameLabel.TabIndex = 16;
            this.ControlNameLabel.Text = "control";
            // 
            // DrawBenchLabel
            // 
            this.DrawBenchLabel.AutoSize = true;
            this.DrawBenchLabel.Location = new System.Drawing.Point(236, 139);
            this.DrawBenchLabel.Name = "DrawBenchLabel";
            this.DrawBenchLabel.Size = new System.Drawing.Size(104, 15);
            this.DrawBenchLabel.TabIndex = 15;
            this.DrawBenchLabel.Text = "DrawBenchLabel";
            // 
            // IndexLabel
            // 
            this.IndexLabel.AutoSize = true;
            this.IndexLabel.Location = new System.Drawing.Point(3, 19);
            this.IndexLabel.Name = "IndexLabel";
            this.IndexLabel.Size = new System.Drawing.Size(43, 15);
            this.IndexLabel.TabIndex = 13;
            this.IndexLabel.Text = "index:";
            // 
            // MediaSpaceLabel
            // 
            this.MediaSpaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MediaSpaceLabel.AutoSize = true;
            this.MediaSpaceLabel.Location = new System.Drawing.Point(113, 158);
            this.MediaSpaceLabel.Name = "MediaSpaceLabel";
            this.MediaSpaceLabel.Size = new System.Drawing.Size(43, 15);
            this.MediaSpaceLabel.TabIndex = 11;
            this.MediaSpaceLabel.Text = "media";
            // 
            // MediaSpace
            // 
            this.MediaSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MediaSpace.Location = new System.Drawing.Point(9, 155);
            this.MediaSpace.Name = "MediaSpace";
            this.MediaSpace.Size = new System.Drawing.Size(102, 20);
            this.MediaSpace.TabIndex = 10;
            this.MediaSpace.TabStop = false;
            this.MediaSpace.Paint += new System.Windows.Forms.PaintEventHandler(this.MediaSpace_Paint);
            // 
            // CursorLabel
            // 
            this.CursorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CursorLabel.AutoSize = true;
            this.CursorLabel.Location = new System.Drawing.Point(88, 0);
            this.CursorLabel.Name = "CursorLabel";
            this.CursorLabel.Size = new System.Drawing.Size(48, 15);
            this.CursorLabel.TabIndex = 7;
            this.CursorLabel.Text = "cursor:";
            // 
            // DebugLabel
            // 
            this.DebugLabel.AutoSize = true;
            this.DebugLabel.Location = new System.Drawing.Point(6, 0);
            this.DebugLabel.Name = "DebugLabel";
            this.DebugLabel.Size = new System.Drawing.Size(42, 15);
            this.DebugLabel.TabIndex = 6;
            this.DebugLabel.Text = "debug";
            // 
            // DebugLog
            // 
            this.DebugLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugLog.BackColor = System.Drawing.Color.Black;
            this.DebugLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DebugLog.ForeColor = System.Drawing.Color.White;
            this.DebugLog.Location = new System.Drawing.Point(6, 55);
            this.DebugLog.Name = "DebugLog";
            this.DebugLog.ReadOnly = true;
            this.DebugLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.DebugLog.Size = new System.Drawing.Size(488, 97);
            this.DebugLog.TabIndex = 8;
            this.DebugLog.Text = "log";
            this.DebugLog.WordWrap = false;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.BackColor = System.Drawing.Color.Transparent;
            this.ScaleLabel.Location = new System.Drawing.Point(0, 6);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(50, 15);
            this.ScaleLabel.TabIndex = 12;
            this.ScaleLabel.Text = "scale x";
            this.ScaleLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.jpg";
            this.openFileDialog1.Filter = "all files|*.*|JPEG image|*.jpg;*.jpeg;*.jfif|PNG image|*.png|Windows bitmap|*.bmp" +
    ";*.rle;*.dib|TIFF|*.tiff;*.tif";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.Title = "Open Image or Catalog";
            // 
            // FSWatcher
            // 
            this.FSWatcher.EnableRaisingEvents = true;
            this.FSWatcher.IncludeSubdirectories = true;
            this.FSWatcher.SynchronizingObject = this;
            this.FSWatcher.Changed += new System.IO.FileSystemEventHandler(this.FSWatcher_Changed);
            this.FSWatcher.Created += new System.IO.FileSystemEventHandler(this.FSWatcher_Created);
            this.FSWatcher.Deleted += new System.IO.FileSystemEventHandler(this.FSWatcher_Deleted);
            this.FSWatcher.Renamed += new System.IO.RenamedEventHandler(this.FSWatcher_Renamed);
            // 
            // SecTimer
            // 
            this.SecTimer.Enabled = true;
            this.SecTimer.Interval = 1000;
            this.SecTimer.Tick += new System.EventHandler(this.SecTimer_Tick);
            // 
            // Thumb
            // 
            this.Thumb.Location = new System.Drawing.Point(0, 2);
            this.Thumb.Name = "Thumb";
            this.Thumb.Size = new System.Drawing.Size(160, 120);
            this.Thumb.TabIndex = 8;
            this.Thumb.TabStop = false;
            this.toolTip1.SetToolTip(this.Thumb, "表示範囲をスクロール");
            this.Thumb.Paint += new System.Windows.Forms.PaintEventHandler(this.Thumb_Paint);
            this.Thumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseDown);
            this.Thumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseMove);
            this.Thumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseUp);
            // 
            // SlideShowTimer
            // 
            this.SlideShowTimer.Interval = 2500;
            this.SlideShowTimer.Tick += new System.EventHandler(this.SlideShowTimer_Tick);
            // 
            // ScaleBar
            // 
            this.ScaleBar.AutoSize = false;
            this.ScaleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScaleBar.LargeChange = 60;
            this.ScaleBar.Location = new System.Drawing.Point(10, 24);
            this.ScaleBar.Maximum = 240;
            this.ScaleBar.Minimum = -240;
            this.ScaleBar.Name = "ScaleBar";
            this.ScaleBar.Size = new System.Drawing.Size(154, 43);
            this.ScaleBar.TabIndex = 5;
            this.ScaleBar.Tag = "0";
            this.ScaleBar.TickFrequency = 60;
            this.ScaleBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ScaleBar.Scroll += new System.EventHandler(this.ScaleBar_Scroll);
            // 
            // ExifBox
            // 
            this.ExifBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExifBox.Controls.Add(this.Filename);
            this.ExifBox.Controls.Add(this.Exif);
            this.ExifBox.Controls.Add(this.ExifLabel);
            this.ExifBox.Location = new System.Drawing.Point(12, 331);
            this.ExifBox.Name = "ExifBox";
            this.ExifBox.Size = new System.Drawing.Size(180, 148);
            this.ExifBox.TabIndex = 14;
            // 
            // Filename
            // 
            this.Filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Filename.BackColor = System.Drawing.Color.Black;
            this.Filename.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Filename.ForeColor = System.Drawing.Color.White;
            this.Filename.Location = new System.Drawing.Point(2, 130);
            this.Filename.Name = "Filename";
            this.Filename.ReadOnly = true;
            this.Filename.Size = new System.Drawing.Size(176, 16);
            this.Filename.TabIndex = 18;
            this.Filename.Text = "filename";
            // 
            // Exif
            // 
            this.Exif.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Exif.BackColor = System.Drawing.Color.Black;
            this.Exif.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Exif.ForeColor = System.Drawing.Color.White;
            this.Exif.Location = new System.Drawing.Point(1, 19);
            this.Exif.Multiline = true;
            this.Exif.Name = "Exif";
            this.Exif.ReadOnly = true;
            this.Exif.Size = new System.Drawing.Size(178, 111);
            this.Exif.TabIndex = 9;
            this.Exif.Text = "EXIF";
            // 
            // ExifLabel
            // 
            this.ExifLabel.AutoSize = true;
            this.ExifLabel.Location = new System.Drawing.Point(3, 0);
            this.ExifLabel.Name = "ExifLabel";
            this.ExifLabel.Size = new System.Drawing.Size(74, 15);
            this.ExifLabel.TabIndex = 8;
            this.ExifLabel.Text = "information";
            // 
            // ScalePanel
            // 
            this.ScalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScalePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScalePanel.Controls.Add(this.label4);
            this.ScalePanel.Controls.Add(this.ScaleLabel);
            this.ScalePanel.Controls.Add(this.label3);
            this.ScalePanel.Controls.Add(this.ScaleEdit);
            this.ScalePanel.Controls.Add(this.label1);
            this.ScalePanel.Controls.Add(this.ReciprocalLabel);
            this.ScalePanel.Controls.Add(this.ScaleBar);
            this.ScalePanel.Controls.Add(this.label2);
            this.ScalePanel.Location = new System.Drawing.Point(454, 436);
            this.ScalePanel.Name = "ScalePanel";
            this.ScalePanel.Size = new System.Drawing.Size(173, 53);
            this.ScalePanel.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(147, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(17, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 14);
            this.label3.TabIndex = 17;
            this.label3.Text = "[";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ScaleEdit
            // 
            this.ScaleEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ScaleEdit.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleEdit.Location = new System.Drawing.Point(50, 5);
            this.ScaleEdit.Name = "ScaleEdit";
            this.ScaleEdit.Size = new System.Drawing.Size(45, 16);
            this.ScaleEdit.TabIndex = 13;
            this.ScaleEdit.Text = "100.0";
            this.ScaleEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ScaleEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleEdit_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(80, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "o";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ReciprocalLabel
            // 
            this.ReciprocalLabel.AutoSize = true;
            this.ReciprocalLabel.BackColor = System.Drawing.Color.Transparent;
            this.ReciprocalLabel.Location = new System.Drawing.Point(117, 6);
            this.ReciprocalLabel.Name = "ReciprocalLabel";
            this.ReciprocalLabel.Size = new System.Drawing.Size(36, 15);
            this.ReciprocalLabel.TabIndex = 14;
            this.ReciprocalLabel.Text = "(1/1)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(97, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "%";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "*.jpg";
            this.saveFileDialog1.Filter = "all files|*.*|JPEG image|*.jpg;*.jpeg;*.jfif|PNG image|*.png|Windows bitmap|*.bmp" +
    ";*.rle;*.dib|TIFF|*.tiff;*.tif";
            this.saveFileDialog1.Title = "Save Image File";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // ViewPort
            // 
            this.ViewPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ViewPort.Controls.Add(this.Thumb);
            this.ViewPort.Location = new System.Drawing.Point(400, 80);
            this.ViewPort.Name = "ViewPort";
            this.ViewPort.Size = new System.Drawing.Size(160, 135);
            this.ViewPort.TabIndex = 16;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // PageUpDownTimer
            // 
            this.PageUpDownTimer.Tick += new System.EventHandler(this.PageUpDownTimer_Tick);
            // 
            // Photo
            // 
            this.Photo.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Photo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Photo.ImageFixedScale = 1F;
            this.Photo.ImageScaleByWheelEnabled = true;
            this.Photo.ImageScaleMode = ParaParaView.ImageScaleMode.FitToWindow;
            this.Photo.ImageScroll = new System.Drawing.Point(0, 0);
            this.Photo.ImageScrollByKeyEnabled = true;
            this.Photo.ImageScrollByMouseEnabled = true;
            this.Photo.InHaste = 0;
            this.Photo.Location = new System.Drawing.Point(0, 26);
            this.Photo.Name = "Photo";
            this.Photo.NoPhoto = "NO PHOTO";
            this.Photo.ScaleIndex = 0;
            this.Photo.Size = new System.Drawing.Size(640, 480);
            this.Photo.TabIndex = 17;
            this.Photo.TabStop = false;
            this.Photo.ImageScaleChanged += new System.EventHandler(this.Photo_ImageScrolled);
            this.Photo.ImageScrolled += new System.EventHandler(this.Photo_ImageScaleChanged);
            this.Photo.HasteTimeouted += new System.EventHandler(this.Photo_HasteTimeouted);
            // 
            // ParaParaMain
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(640, 506);
            this.Controls.Add(this.ExifBox);
            this.Controls.Add(this.ViewPort);
            this.Controls.Add(this.ScalePanel);
            this.Controls.Add(this.DebugBox);
            this.Controls.Add(this.FullScreenLabel);
            this.Controls.Add(this.Photo);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(336, 278);
            this.Name = "ParaParaMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "para para Photo viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ParaParaMain_FormClosed);
            this.Shown += new System.EventHandler(this.ParaParaMain_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ParaParaMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ParaParaMain_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParaParaMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ParaParaMain_KeyUp);
            this.Resize += new System.EventHandler(this.ParaParaMain_Resize);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.DebugBox.ResumeLayout(false);
            this.DebugBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediaSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Thumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBar)).EndInit();
            this.ExifBox.ResumeLayout(false);
            this.ExifBox.PerformLayout();
            this.ScalePanel.ResumeLayout(false);
            this.ScalePanel.PerformLayout();
            this.ViewPort.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Photo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer10;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileNewItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFolderItem;
        private System.Windows.Forms.ToolStripMenuItem AppExitItem;
        private System.Windows.Forms.ToolStripMenuItem EditMenu;
        private System.Windows.Forms.ToolStripMenuItem ViewMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutItem;
        private System.Windows.Forms.Label FullScreenLabel;
        private System.Windows.Forms.GroupBox DebugBox;
        private System.Windows.Forms.Label DebugLabel;
        private System.Windows.Forms.Label CursorLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem FileDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem FileTrashItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem EjectItem;
        private System.Windows.Forms.ToolStripMenuItem OpenTrashItem;
        private System.Windows.Forms.ToolStripMenuItem RecentMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenFileItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox DebugLog;
        private System.IO.FileSystemWatcher FSWatcher;
        private System.Windows.Forms.ToolStripMenuItem FileCloseItem;
        private System.Windows.Forms.Label MediaSpaceLabel;
        private System.Windows.Forms.PictureBox MediaSpace;
        private System.Windows.Forms.Timer SecTimer;
        private System.Windows.Forms.ToolStripMenuItem PhotoPrevItem;
        private System.Windows.Forms.ToolStripMenuItem PhotoNextItem;
        private System.Windows.Forms.ToolStripMenuItem PhotoHomeItem;
        private System.Windows.Forms.ToolStripMenuItem ShuffleNextItem;
        private System.Windows.Forms.ToolStripMenuItem SlideShowSettItem;
        private System.Windows.Forms.ToolStripMenuItem BrowseMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem PhotoBackItem;
        private System.Windows.Forms.Label IndexLabel;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.Label DrawBenchLabel;
        private System.Windows.Forms.ToolStripMenuItem FileaSaveItem;
        private System.Windows.Forms.ToolStripMenuItem AppSettingsItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer SlideShowTimer;
        private System.Windows.Forms.TrackBar ScaleBar;
        private System.Windows.Forms.Panel ExifBox;
        private System.Windows.Forms.TextBox Exif;
        private System.Windows.Forms.Label ExifLabel;
        private System.Windows.Forms.PictureBox Thumb;
        private System.Windows.Forms.Label ControlNameLabel;
        private System.Windows.Forms.ToolStripMenuItem RotateRightItem;
        private System.Windows.Forms.ToolStripMenuItem RotateLeftItem;
        private System.Windows.Forms.ToolStripMenuItem FlipVertItem;
        private System.Windows.Forms.ToolStripMenuItem FlipHorizItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem ScaleUpItem;
        private System.Windows.Forms.ToolStripMenuItem ScaleDownItem;
        private System.Windows.Forms.ToolStripMenuItem SlideShowItem;
        private System.Windows.Forms.Panel ScalePanel;
        private System.Windows.Forms.ToolStripMenuItem PhotoEndItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem ExplorerItem;
        private System.Windows.Forms.TextBox ScaleEdit;
        private System.Windows.Forms.Label ReciprocalLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label dbgMemoryLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem EditCopyItem;
        private System.Windows.Forms.ToolStripMenuItem EditPasteItem;
        private System.Windows.Forms.ToolStripMenuItem EditCutItem;
        private System.Windows.Forms.Label CacheLabel;
        private System.Windows.Forms.ToolStripMenuItem WindowMenu;
        private System.Windows.Forms.ToolStripMenuItem FitToWindowItem;
        private System.Windows.Forms.ToolStripMenuItem FullSizeItem;
        private System.Windows.Forms.ToolStripMenuItem FullScreenItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem OnlyPhotoItem;
        private System.Windows.Forms.ToolStripMenuItem ViewExifItem;
        private System.Windows.Forms.ToolStripMenuItem ViewPortItem;
        private System.Windows.Forms.ToolStripMenuItem ScaleBarItem;
        private System.Windows.Forms.ToolStripMenuItem ViewDebugItem;
        private System.Windows.Forms.Label ScanAsyncLabel;
        private System.Windows.Forms.Panel ViewPort;
        private System.Windows.Forms.ToolStripMenuItem ViewRefreshItem;
        private System.Windows.Forms.ToolStripMenuItem ClearCacheItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem ScrollCenterItem;
        private System.Windows.Forms.ToolStripMenuItem ScrollMenu;
        private System.Windows.Forms.ToolStripMenuItem ScrollRightItem;
        private System.Windows.Forms.ToolStripMenuItem ScrollLeftItem;
        private System.Windows.Forms.ToolStripMenuItem ScrollUpItem;
        private System.Windows.Forms.ToolStripMenuItem ScrollDownItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.TextBox Filename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem CopyFullPathItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem FilePrintItem;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Timer PageUpDownTimer;
        //private ParaParaImage Photo;
        private ParaParaImage Photo;
    }
}

