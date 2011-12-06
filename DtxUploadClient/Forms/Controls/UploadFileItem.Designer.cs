﻿namespace DtxUpload {
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
			this._tabLayout = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this._panOverlay = new DtxUpload.TransparentPanel();
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).BeginInit();
			this._tabLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lblFileName
			// 
			this._lblFileName.AutoEllipsis = true;
			this._lblFileName.AutoSize = true;
			this._tabLayout.SetColumnSpan(this._lblFileName, 2);
			this._lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblFileName.Location = new System.Drawing.Point(38, 2);
			this._lblFileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
			this._lblFileName.Name = "_lblFileName";
			this._lblFileName.Size = new System.Drawing.Size(10, 13);
			this._lblFileName.TabIndex = 0;
			this._lblFileName.Text = "-";
			this._lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblStatus
			// 
			this._lblStatus.AutoSize = true;
			this._tabLayout.SetColumnSpan(this._lblStatus, 2);
			this._lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblStatus.Location = new System.Drawing.Point(38, 29);
			this._lblStatus.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
			this._lblStatus.Name = "_lblStatus";
			this._lblStatus.Size = new System.Drawing.Size(46, 13);
			this._lblStatus.TabIndex = 1;
			this._lblStatus.Text = "Pending";
			this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _picPreview
			// 
			this._picPreview.BackgroundImage = global::DtxUpload.Properties.Resources.icon_24_control_stop;
			this._picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this._picPreview.Location = new System.Drawing.Point(2, 8);
			this._picPreview.Margin = new System.Windows.Forms.Padding(2, 8, 0, 0);
			this._picPreview.Name = "_picPreview";
			this._tabLayout.SetRowSpan(this._picPreview, 3);
			this._picPreview.Size = new System.Drawing.Size(32, 32);
			this._picPreview.TabIndex = 0;
			this._picPreview.TabStop = false;
			// 
			// _barProgress
			// 
			this._tabLayout.SetColumnSpan(this._barProgress, 2);
			this._barProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this._barProgress.Location = new System.Drawing.Point(35, 16);
			this._barProgress.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this._barProgress.Name = "_barProgress";
			this._barProgress.Size = new System.Drawing.Size(149, 12);
			this._barProgress.TabIndex = 2;
			// 
			// _tabLayout
			// 
			this._tabLayout.BackColor = System.Drawing.Color.Transparent;
			this._tabLayout.ColumnCount = 3;
			this._tabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this._tabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
			this._tabLayout.Controls.Add(this._lblFileName, 1, 0);
			this._tabLayout.Controls.Add(this._lblStatus, 1, 2);
			this._tabLayout.Controls.Add(this._barProgress, 1, 1);
			this._tabLayout.Controls.Add(this._picPreview, 0, 0);
			this._tabLayout.Controls.Add(this.panel1, 0, 3);
			this._tabLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabLayout.Location = new System.Drawing.Point(0, 0);
			this._tabLayout.Name = "_tabLayout";
			this._tabLayout.RowCount = 4;
			this._tabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this._tabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
			this._tabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this._tabLayout.Size = new System.Drawing.Size(188, 47);
			this._tabLayout.TabIndex = 3;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this._tabLayout.SetColumnSpan(this.panel1, 3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 46);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(188, 1);
			this.panel1.TabIndex = 0;
			// 
			// _panOverlay
			// 
			this._panOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panOverlay.Location = new System.Drawing.Point(0, 0);
			this._panOverlay.Margin = new System.Windows.Forms.Padding(0);
			this._panOverlay.Name = "_panOverlay";
			this._panOverlay.Size = new System.Drawing.Size(188, 47);
			this._panOverlay.TabIndex = 4;
			this._panOverlay.DoubleClick += new System.EventHandler(this._panOverlay_DoubleClick);
			this._panOverlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this._panOverlay_MouseDown);
			// 
			// UploadFileItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this._panOverlay);
			this.Controls.Add(this._tabLayout);
			this.Name = "UploadFileItem";
			this.Size = new System.Drawing.Size(188, 47);
			((System.ComponentModel.ISupportInitialize)(this._picPreview)).EndInit();
			this._tabLayout.ResumeLayout(false);
			this._tabLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.PictureBox _picPreview;
		public System.Windows.Forms.Label _lblFileName;
		public System.Windows.Forms.Label _lblStatus;
		private System.Windows.Forms.TableLayoutPanel _tabLayout;
		public System.Windows.Forms.ProgressBar _barProgress;
		private TransparentPanel _panOverlay;
		private System.Windows.Forms.Panel panel1;
	}
}
