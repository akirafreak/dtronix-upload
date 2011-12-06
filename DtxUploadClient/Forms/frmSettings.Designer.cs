namespace DtxUpload {
	partial class frmSettings {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
			this._lstSettingType = new System.Windows.Forms.ListView();
			this._imlSettingImages = new System.Windows.Forms.ImageList(this.components);
			this._tlpSettingsPanel = new System.Windows.Forms.TableLayoutPanel();
			this._panControls = new System.Windows.Forms.Panel();
			this._btnSaveSettings = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this._tlpSettingsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lstSettingType
			// 
			this._lstSettingType.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lstSettingType.LargeImageList = this._imlSettingImages;
			this._lstSettingType.Location = new System.Drawing.Point(0, 0);
			this._lstSettingType.Margin = new System.Windows.Forms.Padding(0);
			this._lstSettingType.MultiSelect = false;
			this._lstSettingType.Name = "_lstSettingType";
			this._lstSettingType.Size = new System.Drawing.Size(650, 70);
			this._lstSettingType.TabIndex = 1;
			this._lstSettingType.UseCompatibleStateImageBehavior = false;
			this._lstSettingType.SelectedIndexChanged += new System.EventHandler(this._lstSettingType_SelectedIndexChanged);
			// 
			// _imlSettingImages
			// 
			this._imlSettingImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this._imlSettingImages.ImageSize = new System.Drawing.Size(24, 24);
			this._imlSettingImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _tlpSettingsPanel
			// 
			this._tlpSettingsPanel.ColumnCount = 1;
			this._tlpSettingsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tlpSettingsPanel.Controls.Add(this._lstSettingType, 0, 0);
			this._tlpSettingsPanel.Controls.Add(this._panControls, 0, 1);
			this._tlpSettingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this._tlpSettingsPanel.Location = new System.Drawing.Point(0, 0);
			this._tlpSettingsPanel.Margin = new System.Windows.Forms.Padding(0);
			this._tlpSettingsPanel.Name = "_tlpSettingsPanel";
			this._tlpSettingsPanel.RowCount = 2;
			this._tlpSettingsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
			this._tlpSettingsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this._tlpSettingsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tlpSettingsPanel.Size = new System.Drawing.Size(650, 379);
			this._tlpSettingsPanel.TabIndex = 2;
			// 
			// _panControls
			// 
			this._panControls.AutoScroll = true;
			this._panControls.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panControls.Location = new System.Drawing.Point(0, 70);
			this._panControls.Margin = new System.Windows.Forms.Padding(0);
			this._panControls.Name = "_panControls";
			this._panControls.Size = new System.Drawing.Size(650, 309);
			this._panControls.TabIndex = 2;
			// 
			// _btnSaveSettings
			// 
			this._btnSaveSettings.Location = new System.Drawing.Point(477, 382);
			this._btnSaveSettings.Name = "_btnSaveSettings";
			this._btnSaveSettings.Size = new System.Drawing.Size(75, 23);
			this._btnSaveSettings.TabIndex = 4;
			this._btnSaveSettings.Text = "Save";
			this._btnSaveSettings.UseVisualStyleBackColor = true;
			this._btnSaveSettings.Click += new System.EventHandler(this._btnSaveSettings_Click);
			// 
			// _btnCancel
			// 
			this._btnCancel.Location = new System.Drawing.Point(563, 382);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(75, 23);
			this._btnCancel.TabIndex = 5;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(650, 412);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._btnSaveSettings);
			this.Controls.Add(this._tlpSettingsPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Upload Settings";
			this.Load += new System.EventHandler(this.frmSettings_Load);
			this._tlpSettingsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView _lstSettingType;
		private System.Windows.Forms.TableLayoutPanel _tlpSettingsPanel;
		private System.Windows.Forms.ImageList _imlSettingImages;
		private System.Windows.Forms.Panel _panControls;
		private System.Windows.Forms.Button _btnSaveSettings;
		private System.Windows.Forms.Button _btnCancel;

	}
}