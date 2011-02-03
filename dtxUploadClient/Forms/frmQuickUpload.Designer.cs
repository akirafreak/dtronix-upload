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
			this._tlpUploadTable = new System.Windows.Forms.TableLayoutPanel();
			this._panConfirmUpload = new System.Windows.Forms.Panel();
			this._btnCancelConfirmUpload = new System.Windows.Forms.Button();
			this._chkHideClipConfirmation = new System.Windows.Forms.CheckBox();
			this._btnConfirmClipboardUpload = new System.Windows.Forms.Button();
			this._picClipboardType = new System.Windows.Forms.PictureBox();
			this._lblClipboardUpload = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this._tlpUploadTable.SuspendLayout();
			this._panConfirmUpload.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picClipboardType)).BeginInit();
			this.SuspendLayout();
			// 
			// _btnClearList
			// 
			this._btnClearList.Location = new System.Drawing.Point(3, 4);
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
			this.panel1.Location = new System.Drawing.Point(0, 379);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(270, 33);
			this.panel1.TabIndex = 2;
			// 
			// _btnUploadClipboard
			// 
			this._btnUploadClipboard.Location = new System.Drawing.Point(169, 4);
			this._btnUploadClipboard.Name = "_btnUploadClipboard";
			this._btnUploadClipboard.Size = new System.Drawing.Size(96, 23);
			this._btnUploadClipboard.TabIndex = 3;
			this._btnUploadClipboard.Text = "&Upload Clipboard";
			this._btnUploadClipboard.UseVisualStyleBackColor = true;
			this._btnUploadClipboard.Click += new System.EventHandler(this._btnUploadClipboard_Click);
			// 
			// _btnCancelAll
			// 
			this._btnCancelAll.Location = new System.Drawing.Point(67, 4);
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
			this._panFileItemContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panFileItemContainer.Location = new System.Drawing.Point(0, 0);
			this._panFileItemContainer.Margin = new System.Windows.Forms.Padding(0);
			this._panFileItemContainer.Name = "_panFileItemContainer";
			this._panFileItemContainer.Size = new System.Drawing.Size(270, 349);
			this._panFileItemContainer.TabIndex = 3;
			this._panFileItemContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this._panFileItemContainer_DragDrop);
			this._panFileItemContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this._panFileItemContainer_DragEnter);
			// 
			// _tlpUploadTable
			// 
			this._tlpUploadTable.ColumnCount = 1;
			this._tlpUploadTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tlpUploadTable.Controls.Add(this._panConfirmUpload, 0, 1);
			this._tlpUploadTable.Controls.Add(this._panFileItemContainer, 0, 0);
			this._tlpUploadTable.Controls.Add(this.panel1, 0, 2);
			this._tlpUploadTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tlpUploadTable.Location = new System.Drawing.Point(0, 0);
			this._tlpUploadTable.Name = "_tlpUploadTable";
			this._tlpUploadTable.RowCount = 3;
			this._tlpUploadTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tlpUploadTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this._tlpUploadTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
			this._tlpUploadTable.Size = new System.Drawing.Size(270, 412);
			this._tlpUploadTable.TabIndex = 4;
			// 
			// _panConfirmUpload
			// 
			this._panConfirmUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(227)))), ((int)(((byte)(145)))));
			this._panConfirmUpload.Controls.Add(this._btnConfirmClipboardUpload);
			this._panConfirmUpload.Controls.Add(this._btnCancelConfirmUpload);
			this._panConfirmUpload.Controls.Add(this._chkHideClipConfirmation);
			this._panConfirmUpload.Controls.Add(this._picClipboardType);
			this._panConfirmUpload.Controls.Add(this._lblClipboardUpload);
			this._panConfirmUpload.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panConfirmUpload.Location = new System.Drawing.Point(0, 349);
			this._panConfirmUpload.Margin = new System.Windows.Forms.Padding(0);
			this._panConfirmUpload.Name = "_panConfirmUpload";
			this._panConfirmUpload.Size = new System.Drawing.Size(270, 30);
			this._panConfirmUpload.TabIndex = 4;
			// 
			// _btnCancelConfirmUpload
			// 
			this._btnCancelConfirmUpload.Location = new System.Drawing.Point(212, 4);
			this._btnCancelConfirmUpload.Name = "_btnCancelConfirmUpload";
			this._btnCancelConfirmUpload.Size = new System.Drawing.Size(54, 23);
			this._btnCancelConfirmUpload.TabIndex = 6;
			this._btnCancelConfirmUpload.Text = "Cancel";
			this._btnCancelConfirmUpload.UseVisualStyleBackColor = true;
			this._btnCancelConfirmUpload.Click += new System.EventHandler(this._btnCancelConfirmUpload_Click);
			// 
			// _chkHideClipConfirmation
			// 
			this._chkHideClipConfirmation.AutoSize = true;
			this._chkHideClipConfirmation.Location = new System.Drawing.Point(82, 8);
			this._chkHideClipConfirmation.Name = "_chkHideClipConfirmation";
			this._chkHideClipConfirmation.Size = new System.Drawing.Size(48, 17);
			this._chkHideClipConfirmation.TabIndex = 5;
			this._chkHideClipConfirmation.Text = "Hide";
			this._chkHideClipConfirmation.UseVisualStyleBackColor = true;
			this._chkHideClipConfirmation.CheckedChanged += new System.EventHandler(this._chkHideClipConfirmation_CheckedChanged);
			// 
			// _btnConfirmClipboardUpload
			// 
			this._btnConfirmClipboardUpload.Location = new System.Drawing.Point(152, 4);
			this._btnConfirmClipboardUpload.Name = "_btnConfirmClipboardUpload";
			this._btnConfirmClipboardUpload.Size = new System.Drawing.Size(54, 23);
			this._btnConfirmClipboardUpload.TabIndex = 4;
			this._btnConfirmClipboardUpload.Text = "Confirm";
			this._btnConfirmClipboardUpload.UseVisualStyleBackColor = true;
			this._btnConfirmClipboardUpload.Click += new System.EventHandler(this._btnConfirmClipboardUpload_Click);
			// 
			// _picClipboardType
			// 
			this._picClipboardType.Image = global::dtxUpload.Properties.Resources.page_24_ns;
			this._picClipboardType.Location = new System.Drawing.Point(3, 3);
			this._picClipboardType.Name = "_picClipboardType";
			this._picClipboardType.Size = new System.Drawing.Size(24, 24);
			this._picClipboardType.TabIndex = 1;
			this._picClipboardType.TabStop = false;
			// 
			// _lblClipboardUpload
			// 
			this._lblClipboardUpload.AutoSize = true;
			this._lblClipboardUpload.Location = new System.Drawing.Point(29, 9);
			this._lblClipboardUpload.Name = "_lblClipboardUpload";
			this._lblClipboardUpload.Size = new System.Drawing.Size(47, 13);
			this._lblClipboardUpload.TabIndex = 0;
			this._lblClipboardUpload.Text = "Upload?";
			// 
			// frmQuickUpload
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(270, 412);
			this.Controls.Add(this._tlpUploadTable);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(286, 900);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(286, 276);
			this.Name = "frmQuickUpload";
			this.Text = "Upload Queue";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuickUpload_FormClosing);
			this.Load += new System.EventHandler(this.frmQuickUpload_Load);
			this.ResizeEnd += new System.EventHandler(this.frmQuickUpload_ResizeEnd);
			this.panel1.ResumeLayout(false);
			this._tlpUploadTable.ResumeLayout(false);
			this._panConfirmUpload.ResumeLayout(false);
			this._panConfirmUpload.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picClipboardType)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion


		private System.Windows.Forms.Button _btnClearList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel _panFileItemContainer;
		private System.Windows.Forms.Button _btnCancelAll;
		private System.Windows.Forms.Button _btnUploadClipboard;
		private System.Windows.Forms.TableLayoutPanel _tlpUploadTable;
		private System.Windows.Forms.Panel _panConfirmUpload;
		private System.Windows.Forms.Label _lblClipboardUpload;
		private System.Windows.Forms.PictureBox _picClipboardType;
		private System.Windows.Forms.Button _btnConfirmClipboardUpload;
		private System.Windows.Forms.CheckBox _chkHideClipConfirmation;
		private System.Windows.Forms.Button _btnCancelConfirmUpload;
	}
}