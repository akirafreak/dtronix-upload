namespace dtxUpload {
	partial class frmQuickUpload {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuickUpload));
			this._btnClearList = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this._btnUploadClipboard = new System.Windows.Forms.Button();
			this._btnCancelAll = new System.Windows.Forms.Button();
			this._panFileItemContainer = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _btnClearList
			// 
			this._btnClearList.Location = new System.Drawing.Point(5, 4);
			this._btnClearList.Name = "_btnClearList";
			this._btnClearList.Size = new System.Drawing.Size(58, 23);
			this._btnClearList.TabIndex = 1;
			this._btnClearList.Text = "&Clear List";
			this._btnClearList.UseVisualStyleBackColor = true;
			this._btnClearList.Click += new System.EventHandler(this._btnClearList_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this._btnUploadClipboard);
			this.panel1.Controls.Add(this._btnCancelAll);
			this.panel1.Controls.Add(this._btnClearList);
			this.panel1.Location = new System.Drawing.Point(-1, 355);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(272, 38);
			this.panel1.TabIndex = 2;
			// 
			// _btnUploadClipboard
			// 
			this._btnUploadClipboard.Location = new System.Drawing.Point(168, 4);
			this._btnUploadClipboard.Name = "_btnUploadClipboard";
			this._btnUploadClipboard.Size = new System.Drawing.Size(96, 23);
			this._btnUploadClipboard.TabIndex = 3;
			this._btnUploadClipboard.Text = "&Upload Clipboard";
			this._btnUploadClipboard.UseVisualStyleBackColor = true;
			this._btnUploadClipboard.Click += new System.EventHandler(this._btnUploadClipboard_Click);
			// 
			// _btnCancelAll
			// 
			this._btnCancelAll.Location = new System.Drawing.Point(69, 4);
			this._btnCancelAll.Name = "_btnCancelAll";
			this._btnCancelAll.Size = new System.Drawing.Size(62, 23);
			this._btnCancelAll.TabIndex = 2;
			this._btnCancelAll.Text = "Cancel &All";
			this._btnCancelAll.UseVisualStyleBackColor = true;
			this._btnCancelAll.Click += new System.EventHandler(this._btnCancelAll_Click);
			// 
			// _panFileItemContainer
			// 
			this._panFileItemContainer.AllowDrop = true;
			this._panFileItemContainer.AutoScroll = true;
			this._panFileItemContainer.BackColor = System.Drawing.Color.White;
			this._panFileItemContainer.Dock = System.Windows.Forms.DockStyle.Top;
			this._panFileItemContainer.Location = new System.Drawing.Point(0, 0);
			this._panFileItemContainer.Name = "_panFileItemContainer";
			this._panFileItemContainer.Size = new System.Drawing.Size(270, 355);
			this._panFileItemContainer.TabIndex = 3;
			this._panFileItemContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this._panFileItemContainer_DragDrop);
			this._panFileItemContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this._panFileItemContainer_DragEnter);
			// 
			// frmQuickUpload
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(270, 388);
			this.Controls.Add(this._panFileItemContainer);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmQuickUpload";
			this.Text = "Upload Queue";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuickUpload_FormClosing);
			this.Load += new System.EventHandler(this.frmQuickUpload_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion


		private System.Windows.Forms.Button _btnClearList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel _panFileItemContainer;
		private System.Windows.Forms.Button _btnCancelAll;
		private System.Windows.Forms.Button _btnUploadClipboard;
	}
}