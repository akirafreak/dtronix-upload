namespace DtxUpload {
	partial class frmManage {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManage));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this._lblStatusText = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this._lblTotalFiles = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this._dtlDirectories = new DtxUpload.DirectoryTreeList();
			this._lstFiles = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._imlFiles = new System.Windows.Forms.ImageList(this.components);
			this._imlDirectories = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this._miNewDirectory = new System.Windows.Forms.MenuItem();
			this._miRefreshDirectories = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this._miMainMenuClose = new System.Windows.Forms.MenuItem();
			this._cmiOpenDirectory = new System.Windows.Forms.MenuItem();
			this._contextMenuFiles = new System.Windows.Forms.ContextMenu();
			this._cmiFilesOpenLinks = new System.Windows.Forms.MenuItem();
			this._cmiFilesCopyLinks = new System.Windows.Forms.MenuItem();
			this._cmiDeleteFiles = new System.Windows.Forms.MenuItem();
			this.vistaMenu = new dtxCore.Controls.VistaMenu(this.components);
			this.statusStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblStatusText,
            this.toolStripStatusLabel1,
            this._lblTotalFiles});
			this.statusStrip1.Location = new System.Drawing.Point(0, 356);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(766, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// _lblStatusText
			// 
			this._lblStatusText.Name = "_lblStatusText";
			this._lblStatusText.Size = new System.Drawing.Size(50, 17);
			this._lblStatusText.Text = "Loading";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
			this.toolStripStatusLabel1.Text = "  |  ";
			// 
			// _lblTotalFiles
			// 
			this._lblTotalFiles.Name = "_lblTotalFiles";
			this._lblTotalFiles.Size = new System.Drawing.Size(0, 17);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this._dtlDirectories);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this._lstFiles);
			this.splitContainer1.Size = new System.Drawing.Size(766, 356);
			this.splitContainer1.SplitterDistance = 198;
			this.splitContainer1.TabIndex = 2;
			// 
			// _dtlDirectories
			// 
			this._dtlDirectories.AllowDrop = true;
			this._dtlDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
			this._dtlDirectories.Location = new System.Drawing.Point(0, 0);
			this._dtlDirectories.Name = "_dtlDirectories";
			this._dtlDirectories.Size = new System.Drawing.Size(198, 356);
			this._dtlDirectories.TabIndex = 0;
			this._dtlDirectories.DragDrop += new System.Windows.Forms.DragEventHandler(this._dtlDirectories_DragDrop);
			this._dtlDirectories.DragEnter += new System.Windows.Forms.DragEventHandler(this._dtlDirectories_DragEnter);
			// 
			// _lstFiles
			// 
			this._lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this._lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lstFiles.Location = new System.Drawing.Point(0, 0);
			this._lstFiles.Name = "_lstFiles";
			this._lstFiles.Size = new System.Drawing.Size(564, 356);
			this._lstFiles.SmallImageList = this._imlFiles;
			this._lstFiles.TabIndex = 0;
			this._lstFiles.UseCompatibleStateImageBehavior = false;
			this._lstFiles.View = System.Windows.Forms.View.Details;
			this._lstFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._lstFiles_ItemDrag);
			this._lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this._lstFiles_DragDrop);
			this._lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this._lstFiles_DragEnter);
			this._lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lstFiles_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 219;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Size";
			this.columnHeader2.Width = 78;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Type";
			this.columnHeader3.Width = 87;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "DateUploaded";
			this.columnHeader4.Width = 109;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Views";
			this.columnHeader5.Width = 47;
			// 
			// _imlFiles
			// 
			this._imlFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this._imlFiles.ImageSize = new System.Drawing.Size(16, 16);
			this._imlFiles.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _imlDirectories
			// 
			this._imlDirectories.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this._imlDirectories.ImageSize = new System.Drawing.Size(16, 16);
			this._imlDirectories.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._miNewDirectory,
            this._miRefreshDirectories,
            this.menuItem9,
            this._miMainMenuClose});
			this.menuItem1.Text = "File";
			// 
			// _miNewDirectory
			// 
			this._miNewDirectory.Index = 0;
			this._miNewDirectory.Text = "";
			// 
			// _miRefreshDirectories
			// 
			this._miRefreshDirectories.Index = 1;
			this._miRefreshDirectories.Text = "";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "-";
			// 
			// _miMainMenuClose
			// 
			this._miMainMenuClose.Index = 3;
			this._miMainMenuClose.Text = "Close";
			this._miMainMenuClose.Click += new System.EventHandler(this._miMainMenuClose_Click);
			// 
			// _cmiOpenDirectory
			// 
			this._cmiOpenDirectory.Index = -1;
			this._cmiOpenDirectory.Text = "";
			// 
			// _contextMenuFiles
			// 
			this._contextMenuFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._cmiFilesOpenLinks,
            this._cmiFilesCopyLinks,
            this._cmiDeleteFiles});
			// 
			// _cmiFilesOpenLinks
			// 
			this._cmiFilesOpenLinks.Index = 0;
			this._cmiFilesOpenLinks.Text = "Open Link(s)";
			this._cmiFilesOpenLinks.Click += new System.EventHandler(this._cmiFilesOpenLinks_Click);
			// 
			// _cmiFilesCopyLinks
			// 
			this._cmiFilesCopyLinks.Index = 1;
			this._cmiFilesCopyLinks.Text = "Copy Link(s)";
			this._cmiFilesCopyLinks.Click += new System.EventHandler(this._cmiFilesCopyLinks_Click);
			// 
			// _cmiDeleteFiles
			// 
			this._cmiDeleteFiles.Index = 2;
			this._cmiDeleteFiles.Text = "Delete";
			this._cmiDeleteFiles.Click += new System.EventHandler(this._cmiDeleteFiles_Click);
			// 
			// vistaMenu
			// 
			this.vistaMenu.ContainerControl = this;
			// 
			// frmManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(766, 378);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "frmManage";
			this.Text = "Manage Uploaded Files";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmManage_FormClosed);
			this.Load += new System.EventHandler(this.frmManage_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView _lstFiles;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem _miMainMenuClose;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ImageList _imlDirectories;
		private System.Windows.Forms.ImageList _imlFiles;
		private dtxCore.Controls.VistaMenu vistaMenu;
		private System.Windows.Forms.ToolStripStatusLabel _lblStatusText;
		private System.Windows.Forms.ContextMenu _contextMenuFiles;
		private System.Windows.Forms.MenuItem _cmiFilesDelete;
		private System.Windows.Forms.MenuItem _cmiFilesCopyLinks;
		private System.Windows.Forms.MenuItem _cmiFilesOpenLinks;
		private System.Windows.Forms.MenuItem _miNewDirectory;
		private System.Windows.Forms.MenuItem _cmiDeleteFiles;
		private System.Windows.Forms.MenuItem _miRefreshDirectories;
		private System.Windows.Forms.MenuItem _cmiOpenDirectory;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel _lblTotalFiles;
		private DirectoryTreeList _dtlDirectories;

	}
}