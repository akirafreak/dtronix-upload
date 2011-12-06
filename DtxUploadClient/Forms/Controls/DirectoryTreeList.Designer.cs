namespace DtxUpload {
	partial class DirectoryTreeList {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this._treDirectories = new System.Windows.Forms.TreeView();
			this._imlDirectories = new System.Windows.Forms.ImageList(this.components);
			this._layout = new System.Windows.Forms.TableLayoutPanel();
			this._txtSearchBox = new System.Windows.Forms.TextBox();
			this._contextMenuDirectory = new System.Windows.Forms.ContextMenu();
			this._cmiDirectoryCopyDirectoryUrl = new System.Windows.Forms.MenuItem();
			this._cmiDirectoryDelete = new System.Windows.Forms.MenuItem();
			this._cmiDirectoryRename = new System.Windows.Forms.MenuItem();
			this._cmiDirectoryBreak1 = new System.Windows.Forms.MenuItem();
			this._cmiDirectoryMakePrivate = new System.Windows.Forms.MenuItem();
			this._cmiDirectoryMakePublic = new System.Windows.Forms.MenuItem();
			this._cmiCreateDirectory = new System.Windows.Forms.MenuItem();
			this._layout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _treDirectories
			// 
			this._treDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
			this._treDirectories.HideSelection = false;
			this._treDirectories.ImageIndex = 0;
			this._treDirectories.ImageList = this._imlDirectories;
			this._treDirectories.LabelEdit = true;
			this._treDirectories.Location = new System.Drawing.Point(0, 0);
			this._treDirectories.Margin = new System.Windows.Forms.Padding(0);
			this._treDirectories.Name = "_treDirectories";
			this._treDirectories.SelectedImageIndex = 0;
			this._treDirectories.Size = new System.Drawing.Size(199, 257);
			this._treDirectories.TabIndex = 1;
			this._treDirectories.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._treDirectories_BeforeLabelEdit);
			this._treDirectories.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._treDirectories_AfterLabelEdit);
			this._treDirectories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treDirectories_AfterSelect);
			this._treDirectories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treDirectories_NodeMouseClick);
			this._treDirectories.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._treDirectories_KeyPress);
			// 
			// _imlDirectories
			// 
			this._imlDirectories.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this._imlDirectories.ImageSize = new System.Drawing.Size(16, 16);
			this._imlDirectories.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _layout
			// 
			this._layout.BackColor = System.Drawing.Color.Transparent;
			this._layout.ColumnCount = 1;
			this._layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._layout.Controls.Add(this._treDirectories, 0, 0);
			this._layout.Controls.Add(this._txtSearchBox, 0, 1);
			this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._layout.Location = new System.Drawing.Point(0, 0);
			this._layout.Name = "_layout";
			this._layout.RowCount = 2;
			this._layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this._layout.Size = new System.Drawing.Size(199, 282);
			this._layout.TabIndex = 2;
			// 
			// _txtSearchBox
			// 
			this._txtSearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._txtSearchBox.Location = new System.Drawing.Point(0, 260);
			this._txtSearchBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._txtSearchBox.Name = "_txtSearchBox";
			this._txtSearchBox.Size = new System.Drawing.Size(199, 20);
			this._txtSearchBox.TabIndex = 2;
			this._txtSearchBox.TextChanged += new System.EventHandler(this._txtSearchBox_TextChanged);
			this._txtSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtSearchBox_KeyDown);
			this._txtSearchBox.Leave += new System.EventHandler(this._txtSearchBox_Leave);
			// 
			// _contextMenuDirectory
			// 
			this._contextMenuDirectory.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._cmiDirectoryCopyDirectoryUrl,
            this._cmiDirectoryDelete,
            this._cmiDirectoryRename,
            this._cmiDirectoryBreak1,
            this._cmiDirectoryMakePrivate,
            this._cmiDirectoryMakePublic,
            this._cmiCreateDirectory});
			this._contextMenuDirectory.Popup += new System.EventHandler(this._contextMenuDirectory_Popup);
			// 
			// _cmiDirectoryCopyDirectoryUrl
			// 
			this._cmiDirectoryCopyDirectoryUrl.Index = 0;
			this._cmiDirectoryCopyDirectoryUrl.Text = "Copy Directory Url";
			this._cmiDirectoryCopyDirectoryUrl.Click += new System.EventHandler(this._cmiDirectoryCopyDirectoryUrl_Click);
			// 
			// _cmiDirectoryDelete
			// 
			this._cmiDirectoryDelete.Index = 1;
			this._cmiDirectoryDelete.Text = "Delete";
			this._cmiDirectoryDelete.Click += new System.EventHandler(this._cmiDirectoryDelete_Click);
			// 
			// _cmiDirectoryRename
			// 
			this._cmiDirectoryRename.Index = 2;
			this._cmiDirectoryRename.Text = "Rename";
			this._cmiDirectoryRename.Click += new System.EventHandler(this._cmiDirectoryRename_Click);
			// 
			// _cmiDirectoryBreak1
			// 
			this._cmiDirectoryBreak1.Index = 3;
			this._cmiDirectoryBreak1.Text = "-";
			// 
			// _cmiDirectoryMakePrivate
			// 
			this._cmiDirectoryMakePrivate.Index = 4;
			this._cmiDirectoryMakePrivate.Text = "Make Directory Private";
			this._cmiDirectoryMakePrivate.Click += new System.EventHandler(this._cmiDirectoryMakePrivate_Click);
			// 
			// _cmiDirectoryMakePublic
			// 
			this._cmiDirectoryMakePublic.Index = 5;
			this._cmiDirectoryMakePublic.Text = "Make Directory Public";
			this._cmiDirectoryMakePublic.Visible = false;
			this._cmiDirectoryMakePublic.Click += new System.EventHandler(this._cmiDirectoryMakePublic_Click);
			// 
			// _cmiCreateDirectory
			// 
			this._cmiCreateDirectory.Index = 6;
			this._cmiCreateDirectory.Text = "Create Directory";
			this._cmiCreateDirectory.Click += new System.EventHandler(this._cmiCreateDirectory_Click);
			// 
			// DirectoryTreeList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._layout);
			this.Name = "DirectoryTreeList";
			this.Size = new System.Drawing.Size(199, 282);
			this.Load += new System.EventHandler(this.DirectoryTreeList_Load);
			this._layout.ResumeLayout(false);
			this._layout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView _treDirectories;
		private System.Windows.Forms.TableLayoutPanel _layout;
		private System.Windows.Forms.TextBox _txtSearchBox;
		private System.Windows.Forms.ImageList _imlDirectories;
		private System.Windows.Forms.ContextMenu _contextMenuDirectory;
		private System.Windows.Forms.MenuItem _cmiDirectoryCopyDirectoryUrl;
		private System.Windows.Forms.MenuItem _cmiDirectoryRename;
		private System.Windows.Forms.MenuItem _cmiDirectoryDelete;
		private System.Windows.Forms.MenuItem _cmiDirectoryBreak1;
		private System.Windows.Forms.MenuItem _cmiDirectoryMakePrivate;
		private System.Windows.Forms.MenuItem _cmiDirectoryMakePublic;
		private System.Windows.Forms.MenuItem _cmiCreateDirectory;
	}
}
