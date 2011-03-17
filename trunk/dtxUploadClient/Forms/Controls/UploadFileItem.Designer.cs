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
			this._lblFileName = new System.Windows.Forms.Label();
			this._lblStatus = new System.Windows.Forms.Label();
			this._picPreview = new System.Windows.Forms.PictureBox();
			this._barProgress = new System.Windows.Forms.ProgressBar();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lblFileName
			// 
			this._lblFileName.AutoEllipsis = true;
			this._lblFileName.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._lblFileName, 2);
			this._lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblFileName.Location = new System.Drawing.Point(38, 2);
			this._lblFileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
			this._lblFileName.Name = "_lblFileName";
			this._lblFileName.Size = new System.Drawing.Size(10, 12);
			this._lblFileName.TabIndex = 0;
			this._lblFileName.Text = "-";
			this._lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._lblFileName.MouseDown += new System.Windows.Forms.MouseEventHandler(this._lblFileName_MouseDown);
			// 
			// _lblStatus
			// 
			this._lblStatus.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._lblStatus, 2);
			this._lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblStatus.Location = new System.Drawing.Point(38, 30);
			this._lblStatus.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
			this._lblStatus.Name = "_lblStatus";
			this._lblStatus.Size = new System.Drawing.Size(46, 13);
			this._lblStatus.TabIndex = 1;
			this._lblStatus.Text = "Pending";
			this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._lblStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this._lblStatus_MouseDown);
			// 
			// _picPreview
			// 
			this._picPreview.BackgroundImage = global::dtxUpload.Properties.Resources.icon_24_control_stop;
			this._picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this._picPreview.Location = new System.Drawing.Point(2, 8);
			this._picPreview.Margin = new System.Windows.Forms.Padding(2, 8, 0, 0);
			this._picPreview.Name = "_picPreview";
			this.tableLayoutPanel1.SetRowSpan(this._picPreview, 3);
			this._picPreview.Size = new System.Drawing.Size(32, 32);
			this._picPreview.TabIndex = 0;
			this._picPreview.TabStop = false;
			this._picPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this._picPreview_MouseDown);
			// 
			// _barProgress
			// 
			this.tableLayoutPanel1.SetColumnSpan(this._barProgress, 2);
			this._barProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this._barProgress.Location = new System.Drawing.Point(35, 14);
			this._barProgress.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this._barProgress.Name = "_barProgress";
			this._barProgress.Size = new System.Drawing.Size(149, 15);
			this._barProgress.TabIndex = 2;
			this._barProgress.MouseDown += new System.Windows.Forms.MouseEventHandler(this._barProgress_MouseDown);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
			this.tableLayoutPanel1.Controls.Add(this._lblFileName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._lblStatus, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this._barProgress, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._picPreview, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(188, 47);
			this.tableLayoutPanel1.TabIndex = 3;
			this.tableLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseDown);
			// 
			// UploadFileItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UploadFileItem";
			this.Size = new System.Drawing.Size(188, 47);
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.PictureBox _picPreview;
		public System.Windows.Forms.Label _lblFileName;
		public System.Windows.Forms.Label _lblStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.ProgressBar _barProgress;
	}
}
