namespace DtxUpload {
	partial class SettingsPanelAdvancedSettings {
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
			this._btnProgramTempDir = new System.Windows.Forms.Button();
			this._btnProgramFilesDir = new System.Windows.Forms.Button();
			this._btnApplicationDataDir = new System.Windows.Forms.Button();
			this._btnOpenConfig = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._btnOpenConfig);
			this.groupBox1.Controls.Add(this._btnProgramTempDir);
			this.groupBox1.Controls.Add(this._btnProgramFilesDir);
			this.groupBox1.Controls.Add(this._btnApplicationDataDir);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(644, 65);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Places";
			// 
			// _btnProgramTempDir
			// 
			this._btnProgramTempDir.Location = new System.Drawing.Point(306, 19);
			this._btnProgramTempDir.Name = "_btnProgramTempDir";
			this._btnProgramTempDir.Size = new System.Drawing.Size(144, 36);
			this._btnProgramTempDir.TabIndex = 2;
			this._btnProgramTempDir.Text = "Program Temp Directory";
			this._btnProgramTempDir.UseVisualStyleBackColor = true;
			this._btnProgramTempDir.Click += new System.EventHandler(this._btnProgramTempDir_Click);
			// 
			// _btnProgramFilesDir
			// 
			this._btnProgramFilesDir.Location = new System.Drawing.Point(156, 19);
			this._btnProgramFilesDir.Name = "_btnProgramFilesDir";
			this._btnProgramFilesDir.Size = new System.Drawing.Size(144, 36);
			this._btnProgramFilesDir.TabIndex = 1;
			this._btnProgramFilesDir.Text = "Program Files Directory";
			this._btnProgramFilesDir.UseVisualStyleBackColor = true;
			this._btnProgramFilesDir.Click += new System.EventHandler(this._btnProgramFilesDir_Click);
			// 
			// _btnApplicationDataDir
			// 
			this._btnApplicationDataDir.Location = new System.Drawing.Point(6, 19);
			this._btnApplicationDataDir.Name = "_btnApplicationDataDir";
			this._btnApplicationDataDir.Size = new System.Drawing.Size(144, 36);
			this._btnApplicationDataDir.TabIndex = 0;
			this._btnApplicationDataDir.Text = "Application Data Directory";
			this._btnApplicationDataDir.UseVisualStyleBackColor = true;
			this._btnApplicationDataDir.Click += new System.EventHandler(this._btnOpenApplicationDataDir_Click);
			// 
			// _btnOpenConfig
			// 
			this._btnOpenConfig.Location = new System.Drawing.Point(456, 19);
			this._btnOpenConfig.Name = "_btnOpenConfig";
			this._btnOpenConfig.Size = new System.Drawing.Size(71, 36);
			this._btnOpenConfig.TabIndex = 3;
			this._btnOpenConfig.Text = "Config File";
			this._btnOpenConfig.UseVisualStyleBackColor = true;
			this._btnOpenConfig.Click += new System.EventHandler(this._btnOpenConfig_Click);
			// 
			// SettingsPanelAdvancedSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "SettingsPanelAdvancedSettings";
			this.Size = new System.Drawing.Size(650, 76);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button _btnProgramFilesDir;
		private System.Windows.Forms.Button _btnApplicationDataDir;
		private System.Windows.Forms.Button _btnProgramTempDir;
		private System.Windows.Forms.Button _btnOpenConfig;
	}
}
