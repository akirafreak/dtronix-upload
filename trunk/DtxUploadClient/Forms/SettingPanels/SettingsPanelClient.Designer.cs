namespace DtxUpload {
	partial class SettingsPanelClient {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._nudMaxUpload = new System.Windows.Forms.NumericUpDown();
			this._nudMaxDownload = new System.Windows.Forms.NumericUpDown();
			this._chkMaxDownload = new System.Windows.Forms.CheckBox();
			this._chkMaxUpload = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._lblJpegCompression = new System.Windows.Forms.Label();
			this._barJpegCompression = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this._cmbScreenshotCompression = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._nudMaxUpload)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._nudMaxDownload)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._barJpegCompression)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._nudMaxUpload);
			this.groupBox1.Controls.Add(this._nudMaxDownload);
			this.groupBox1.Controls.Add(this._chkMaxDownload);
			this.groupBox1.Controls.Add(this._chkMaxUpload);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(3, 77);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(644, 48);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bandwidth Limitations";
			// 
			// _nudMaxUpload
			// 
			this._nudMaxUpload.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this._nudMaxUpload.Location = new System.Drawing.Point(108, 19);
			this._nudMaxUpload.Maximum = new decimal(new int[] {
            51200,
            0,
            0,
            0});
			this._nudMaxUpload.Name = "_nudMaxUpload";
			this._nudMaxUpload.Size = new System.Drawing.Size(76, 20);
			this._nudMaxUpload.TabIndex = 5;
			this._nudMaxUpload.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this._nudMaxUpload.ValueChanged += new System.EventHandler(this._nudMaxUpload_ValueChanged);
			// 
			// _nudMaxDownload
			// 
			this._nudMaxDownload.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this._nudMaxDownload.Location = new System.Drawing.Point(344, 19);
			this._nudMaxDownload.Maximum = new decimal(new int[] {
            51200,
            0,
            0,
            0});
			this._nudMaxDownload.Name = "_nudMaxDownload";
			this._nudMaxDownload.Size = new System.Drawing.Size(76, 20);
			this._nudMaxDownload.TabIndex = 4;
			this._nudMaxDownload.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this._nudMaxDownload.ValueChanged += new System.EventHandler(this._nudMaxDownload_ValueChanged);
			// 
			// _chkMaxDownload
			// 
			this._chkMaxDownload.AutoSize = true;
			this._chkMaxDownload.Checked = true;
			this._chkMaxDownload.CheckState = System.Windows.Forms.CheckState.Checked;
			this._chkMaxDownload.Location = new System.Drawing.Point(238, 20);
			this._chkMaxDownload.Name = "_chkMaxDownload";
			this._chkMaxDownload.Size = new System.Drawing.Size(100, 17);
			this._chkMaxDownload.TabIndex = 1;
			this._chkMaxDownload.Text = "Max Download:";
			this._chkMaxDownload.UseVisualStyleBackColor = true;
			this._chkMaxDownload.CheckedChanged += new System.EventHandler(this._chkMaxDownload_CheckedChanged);
			// 
			// _chkMaxUpload
			// 
			this._chkMaxUpload.AutoSize = true;
			this._chkMaxUpload.Checked = true;
			this._chkMaxUpload.CheckState = System.Windows.Forms.CheckState.Checked;
			this._chkMaxUpload.Location = new System.Drawing.Point(16, 20);
			this._chkMaxUpload.Name = "_chkMaxUpload";
			this._chkMaxUpload.Size = new System.Drawing.Size(86, 17);
			this._chkMaxUpload.TabIndex = 0;
			this._chkMaxUpload.Text = "Max Upload:";
			this._chkMaxUpload.UseVisualStyleBackColor = true;
			this._chkMaxUpload.CheckedChanged += new System.EventHandler(this._chkMaxUpload_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(419, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "KBps";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(183, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "KBps";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._lblJpegCompression);
			this.groupBox2.Controls.Add(this._barJpegCompression);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this._cmbScreenshotCompression);
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(644, 68);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Screenshot Settings";
			// 
			// _lblJpegCompression
			// 
			this._lblJpegCompression.AutoSize = true;
			this._lblJpegCompression.Location = new System.Drawing.Point(351, 22);
			this._lblJpegCompression.Name = "_lblJpegCompression";
			this._lblJpegCompression.Size = new System.Drawing.Size(106, 13);
			this._lblJpegCompression.TabIndex = 4;
			this._lblJpegCompression.Text = "JPEG Compression ()";
			// 
			// _barJpegCompression
			// 
			this._barJpegCompression.LargeChange = 2;
			this._barJpegCompression.Location = new System.Drawing.Point(470, 19);
			this._barJpegCompression.Minimum = 1;
			this._barJpegCompression.Name = "_barJpegCompression";
			this._barJpegCompression.Size = new System.Drawing.Size(168, 45);
			this._barJpegCompression.TabIndex = 3;
			this._barJpegCompression.Value = 9;
			this._barJpegCompression.Scroll += new System.EventHandler(this._barJpegCompression_Scroll);
			this._barJpegCompression.ValueChanged += new System.EventHandler(this._barJpegCompression_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(109, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Compression Method:";
			// 
			// _cmbScreenshotCompression
			// 
			this._cmbScreenshotCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbScreenshotCompression.FormattingEnabled = true;
			this._cmbScreenshotCompression.Items.AddRange(new object[] {
            "Auto detect best compression",
            "JPEG (For pictures)",
            "PNG (For graphics)"});
			this._cmbScreenshotCompression.Location = new System.Drawing.Point(121, 19);
			this._cmbScreenshotCompression.Name = "_cmbScreenshotCompression";
			this._cmbScreenshotCompression.Size = new System.Drawing.Size(214, 21);
			this._cmbScreenshotCompression.TabIndex = 0;
			this._cmbScreenshotCompression.SelectedIndexChanged += new System.EventHandler(this._cmbScreenshotCompression_SelectedIndexChanged);
			// 
			// SettingsPanelClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "SettingsPanelClient";
			this.Size = new System.Drawing.Size(650, 134);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._nudMaxUpload)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._nudMaxDownload)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._barJpegCompression)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox _chkMaxDownload;
		private System.Windows.Forms.CheckBox _chkMaxUpload;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label _lblJpegCompression;
		private System.Windows.Forms.TrackBar _barJpegCompression;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox _cmbScreenshotCompression;
		private System.Windows.Forms.NumericUpDown _nudMaxUpload;
		private System.Windows.Forms.NumericUpDown _nudMaxDownload;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;

	}
}
