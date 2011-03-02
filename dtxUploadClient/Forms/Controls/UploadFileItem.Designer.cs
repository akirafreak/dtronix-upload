namespace dtxUpload {
	partial class UploadFileItem {
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
			this._barProgress = new System.Windows.Forms.ProgressBar();
			this._lblFileName = new System.Windows.Forms.Label();
			this._btnCancel = new System.Windows.Forms.Button();
			this._lblStatus = new System.Windows.Forms.Label();
			this._btnCopyUrl = new System.Windows.Forms.Button();
			this._picPreview = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _barProgress
			// 
			this._barProgress.Location = new System.Drawing.Point(3, 35);
			this._barProgress.Name = "_barProgress";
			this._barProgress.Size = new System.Drawing.Size(183, 12);
			this._barProgress.TabIndex = 2;
			// 
			// _lblFileName
			// 
			this._lblFileName.AutoEllipsis = true;
			this._lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblFileName.Location = new System.Drawing.Point(33, 2);
			this._lblFileName.Name = "_lblFileName";
			this._lblFileName.Size = new System.Drawing.Size(214, 16);
			this._lblFileName.TabIndex = 0;
			this._lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _btnCancel
			// 
			this._btnCancel.BackColor = System.Drawing.Color.Transparent;
			this._btnCancel.Location = new System.Drawing.Point(70, 24);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(55, 23);
			this._btnCancel.TabIndex = 4;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.UseVisualStyleBackColor = false;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _lblStatus
			// 
			this._lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblStatus.Location = new System.Drawing.Point(36, 18);
			this._lblStatus.Name = "_lblStatus";
			this._lblStatus.Size = new System.Drawing.Size(150, 14);
			this._lblStatus.TabIndex = 1;
			this._lblStatus.Text = "Pending";
			this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _btnCopyUrl
			// 
			this._btnCopyUrl.BackColor = System.Drawing.Color.Transparent;
			this._btnCopyUrl.Location = new System.Drawing.Point(3, 24);
			this._btnCopyUrl.Name = "_btnCopyUrl";
			this._btnCopyUrl.Size = new System.Drawing.Size(64, 23);
			this._btnCopyUrl.TabIndex = 5;
			this._btnCopyUrl.Text = "Copy URL";
			this._btnCopyUrl.UseVisualStyleBackColor = false;
			this._btnCopyUrl.Visible = false;
			this._btnCopyUrl.Click += new System.EventHandler(this._btnCopyUrl_Click);
			// 
			// _picPreview
			// 
			this._picPreview.Image = global::dtxUpload.Properties.Resources.icon_24_control_stop;
			this._picPreview.Location = new System.Drawing.Point(3, 2);
			this._picPreview.Name = "_picPreview";
			this._picPreview.Size = new System.Drawing.Size(24, 24);
			this._picPreview.TabIndex = 0;
			this._picPreview.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._btnCancel);
			this.panel1.Controls.Add(this._btnCopyUrl);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(120, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(130, 50);
			this.panel1.TabIndex = 6;
			// 
			// UploadFileItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._lblStatus);
			this.Controls.Add(this._barProgress);
			this.Controls.Add(this._lblFileName);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._picPreview);
			this.MaximumSize = new System.Drawing.Size(500, 50);
			this.MinimumSize = new System.Drawing.Size(250, 36);
			this.Name = "UploadFileItem";
			this.Size = new System.Drawing.Size(250, 50);
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _btnCancel;
		public System.Windows.Forms.PictureBox _picPreview;
		public System.Windows.Forms.ProgressBar _barProgress;
		public System.Windows.Forms.Label _lblFileName;
		public System.Windows.Forms.Label _lblStatus;
		private System.Windows.Forms.Button _btnCopyUrl;
		private System.Windows.Forms.Panel panel1;
	}
}
