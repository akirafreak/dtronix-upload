namespace DtxUpload {
	partial class frmUploadZip {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploadZip));
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._lstFiles = new System.Windows.Forms.ListView();
			this._imlFiles = new System.Windows.Forms.ImageList(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._txtZipName = new System.Windows.Forms.TextBox();
			this._txtPassword = new System.Windows.Forms.TextBox();
			this._btnRandomPass = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this._btnUpload = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this._lblStatus = new System.Windows.Forms.Label();
			this._picLoader = new System.Windows.Forms.PictureBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picLoader)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Zip Name:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._lstFiles);
			this.groupBox1.Location = new System.Drawing.Point(12, 65);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(450, 137);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Files To Compress";
			// 
			// _lstFiles
			// 
			this._lstFiles.AllowDrop = true;
			this._lstFiles.Location = new System.Drawing.Point(6, 19);
			this._lstFiles.Name = "_lstFiles";
			this._lstFiles.Size = new System.Drawing.Size(438, 112);
			this._lstFiles.SmallImageList = this._imlFiles;
			this._lstFiles.TabIndex = 0;
			this.toolTip.SetToolTip(this._lstFiles, "Displays the current files to be uploaded.");
			this._lstFiles.UseCompatibleStateImageBehavior = false;
			this._lstFiles.View = System.Windows.Forms.View.SmallIcon;
			this._lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this._lstFiles_DragDrop);
			this._lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this._lstFiles_DragEnter);
			this._lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lstFiles_KeyDown);
			// 
			// _imlFiles
			// 
			this._imlFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this._imlFiles.ImageSize = new System.Drawing.Size(16, 16);
			this._imlFiles.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _txtZipName
			// 
			this._txtZipName.Location = new System.Drawing.Point(92, 12);
			this._txtZipName.MaxLength = 128;
			this._txtZipName.Name = "_txtZipName";
			this._txtZipName.Size = new System.Drawing.Size(370, 20);
			this._txtZipName.TabIndex = 4;
			this.toolTip.SetToolTip(this._txtZipName, "Name for the new folder to have.");
			// 
			// _txtPassword
			// 
			this._txtPassword.Location = new System.Drawing.Point(92, 38);
			this._txtPassword.MaxLength = 128;
			this._txtPassword.Name = "_txtPassword";
			this._txtPassword.Size = new System.Drawing.Size(303, 20);
			this._txtPassword.TabIndex = 8;
			this.toolTip.SetToolTip(this._txtPassword, "When a password is set, the entire file is encrypted.");
			// 
			// _btnRandomPass
			// 
			this._btnRandomPass.Location = new System.Drawing.Point(407, 36);
			this._btnRandomPass.Name = "_btnRandomPass";
			this._btnRandomPass.Size = new System.Drawing.Size(55, 23);
			this._btnRandomPass.TabIndex = 9;
			this._btnRandomPass.Text = "&Random";
			this.toolTip.SetToolTip(this._btnRandomPass, "Click to generate a random password");
			this._btnRandomPass.UseVisualStyleBackColor = true;
			this._btnRandomPass.Click += new System.EventHandler(this._btnRandomPass_Click);
			// 
			// _btnCancel
			// 
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Location = new System.Drawing.Point(387, 208);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(75, 23);
			this._btnCancel.TabIndex = 5;
			this._btnCancel.Text = "&Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _btnUpload
			// 
			this._btnUpload.Location = new System.Drawing.Point(306, 208);
			this._btnUpload.Name = "_btnUpload";
			this._btnUpload.Size = new System.Drawing.Size(75, 23);
			this._btnUpload.TabIndex = 6;
			this._btnUpload.Text = "&Upload";
			this._btnUpload.UseVisualStyleBackColor = true;
			this._btnUpload.Click += new System.EventHandler(this._btnUpload_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Zip Password:";
			// 
			// _lblStatus
			// 
			this._lblStatus.Location = new System.Drawing.Point(30, 213);
			this._lblStatus.Name = "_lblStatus";
			this._lblStatus.Size = new System.Drawing.Size(270, 13);
			this._lblStatus.TabIndex = 10;
			// 
			// _picLoader
			// 
			this._picLoader.Image = global::DtxUpload.Properties.Resources.ajax_loader;
			this._picLoader.Location = new System.Drawing.Point(9, 212);
			this._picLoader.Name = "_picLoader";
			this._picLoader.Size = new System.Drawing.Size(16, 16);
			this._picLoader.TabIndex = 11;
			this._picLoader.TabStop = false;
			this._picLoader.Visible = false;
			// 
			// frmUploadZip
			// 
			this.AcceptButton = this._btnUpload;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(474, 240);
			this.Controls.Add(this._picLoader);
			this.Controls.Add(this._lblStatus);
			this.Controls.Add(this._btnRandomPass);
			this.Controls.Add(this._txtPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this._btnUpload);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._txtZipName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmUploadZip";
			this.Text = "Compress and Upload Files";
			this.Load += new System.EventHandler(this.frmUploadZip_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._picLoader)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView _lstFiles;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TextBox _txtZipName;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.Button _btnUpload;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox _txtPassword;
		private System.Windows.Forms.Button _btnRandomPass;
		private System.Windows.Forms.ImageList _imlFiles;
		private System.Windows.Forms.Label _lblStatus;
		private System.Windows.Forms.PictureBox _picLoader;
	}
}