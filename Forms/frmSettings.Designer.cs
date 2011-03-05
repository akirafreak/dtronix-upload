namespace dtxUpload {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this._cmbMaxUploadSpeed = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this._cmbTotalConcurrentUploads = new System.Windows.Forms.ComboBox();
			this.spacer = new System.Windows.Forms.Panel();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this._btnRemoveLocalSettings = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this._btnManageFiles = new System.Windows.Forms.Button();
			this._btnDeleteAllFiles = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._btnSaveServer = new System.Windows.Forms.Button();
			this._btnDeleteServer = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this._btnNewServer = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this._cbxServerPresets = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.tabAccount = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this._lblSpaceUsed = new System.Windows.Forms.Label();
			this._lblTotalFiles = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._btnCreateAccount = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this._itxtPassword = new dtxUpload.TextBoxAndInfo();
			this._itxtUsername = new dtxUpload.TextBoxAndInfo();
			this._lblServerAddress = new System.Windows.Forms.Label();
			this._itxtServerKey = new dtxUpload.TextBoxAndInfo();
			this.panel1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.tabAccount.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.groupBox5);
			this.panel1.Controls.Add(this.spacer);
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(2);
			this.panel1.Size = new System.Drawing.Size(485, 254);
			this.panel1.TabIndex = 0;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.label7);
			this.groupBox5.Controls.Add(this._cmbMaxUploadSpeed);
			this.groupBox5.Controls.Add(this.label6);
			this.groupBox5.Controls.Add(this._cmbTotalConcurrentUploads);
			this.groupBox5.Location = new System.Drawing.Point(5, 153);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(458, 81);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Bandwith";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 49);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(101, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Max Upload Speed:";
			// 
			// _cmbMaxUploadSpeed
			// 
			this._cmbMaxUploadSpeed.Enabled = false;
			this._cmbMaxUploadSpeed.FormattingEnabled = true;
			this._cmbMaxUploadSpeed.Location = new System.Drawing.Point(143, 46);
			this._cmbMaxUploadSpeed.Name = "_cmbMaxUploadSpeed";
			this._cmbMaxUploadSpeed.Size = new System.Drawing.Size(309, 21);
			this._cmbMaxUploadSpeed.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 22);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(131, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Total Concurrent Uploads:";
			// 
			// _cmbTotalConcurrentUploads
			// 
			this._cmbTotalConcurrentUploads.Enabled = false;
			this._cmbTotalConcurrentUploads.FormattingEnabled = true;
			this._cmbTotalConcurrentUploads.Location = new System.Drawing.Point(143, 19);
			this._cmbTotalConcurrentUploads.Name = "_cmbTotalConcurrentUploads";
			this._cmbTotalConcurrentUploads.Size = new System.Drawing.Size(309, 21);
			this._cmbTotalConcurrentUploads.TabIndex = 1;
			// 
			// spacer
			// 
			this.spacer.Location = new System.Drawing.Point(5, 384);
			this.spacer.Name = "spacer";
			this.spacer.Size = new System.Drawing.Size(458, 10);
			this.spacer.TabIndex = 3;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this._btnRemoveLocalSettings);
			this.groupBox4.Location = new System.Drawing.Point(5, 327);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(458, 51);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Misc";
			// 
			// _btnRemoveLocalSettings
			// 
			this._btnRemoveLocalSettings.Location = new System.Drawing.Point(6, 19);
			this._btnRemoveLocalSettings.Name = "_btnRemoveLocalSettings";
			this._btnRemoveLocalSettings.Size = new System.Drawing.Size(446, 23);
			this._btnRemoveLocalSettings.TabIndex = 0;
			this._btnRemoveLocalSettings.Text = "Remove All Local Settings";
			this._btnRemoveLocalSettings.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this._btnManageFiles);
			this.groupBox3.Controls.Add(this._btnDeleteAllFiles);
			this.groupBox3.Location = new System.Drawing.Point(5, 240);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(458, 81);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Files";
			// 
			// _btnManageFiles
			// 
			this._btnManageFiles.Location = new System.Drawing.Point(6, 19);
			this._btnManageFiles.Name = "_btnManageFiles";
			this._btnManageFiles.Size = new System.Drawing.Size(446, 23);
			this._btnManageFiles.TabIndex = 0;
			this._btnManageFiles.Text = "Manage Uploaded Files";
			this._btnManageFiles.UseVisualStyleBackColor = true;
			// 
			// _btnDeleteAllFiles
			// 
			this._btnDeleteAllFiles.Location = new System.Drawing.Point(6, 48);
			this._btnDeleteAllFiles.Name = "_btnDeleteAllFiles";
			this._btnDeleteAllFiles.Size = new System.Drawing.Size(446, 23);
			this._btnDeleteAllFiles.TabIndex = 1;
			this._btnDeleteAllFiles.Text = "Delete All Uploaded Files";
			this._btnDeleteAllFiles.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._itxtServerKey);
			this.groupBox1.Controls.Add(this._lblServerAddress);
			this.groupBox1.Controls.Add(this._btnSaveServer);
			this.groupBox1.Controls.Add(this._btnDeleteServer);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this._btnNewServer);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this._cbxServerPresets);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(458, 142);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server Settings";
			// 
			// _btnSaveServer
			// 
			this._btnSaveServer.Location = new System.Drawing.Point(403, 112);
			this._btnSaveServer.Name = "_btnSaveServer";
			this._btnSaveServer.Size = new System.Drawing.Size(49, 23);
			this._btnSaveServer.TabIndex = 9;
			this._btnSaveServer.Text = "Save";
			this._btnSaveServer.UseVisualStyleBackColor = true;
			// 
			// _btnDeleteServer
			// 
			this._btnDeleteServer.Location = new System.Drawing.Point(348, 112);
			this._btnDeleteServer.Name = "_btnDeleteServer";
			this._btnDeleteServer.Size = new System.Drawing.Size(49, 23);
			this._btnDeleteServer.TabIndex = 8;
			this._btnDeleteServer.Text = "Delete";
			this._btnDeleteServer.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Server Key:";
			// 
			// _btnNewServer
			// 
			this._btnNewServer.Location = new System.Drawing.Point(403, 17);
			this._btnNewServer.Name = "_btnNewServer";
			this._btnNewServer.Size = new System.Drawing.Size(49, 23);
			this._btnNewServer.TabIndex = 2;
			this._btnNewServer.Text = "New";
			this._btnNewServer.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Server Name:";
			// 
			// _cbxServerPresets
			// 
			this._cbxServerPresets.FormattingEnabled = true;
			this._cbxServerPresets.Location = new System.Drawing.Point(94, 19);
			this._cbxServerPresets.Name = "_cbxServerPresets";
			this._cbxServerPresets.Size = new System.Drawing.Size(303, 21);
			this._cbxServerPresets.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Server Address:";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabGeneral);
			this.tabControl1.Controls.Add(this.tabAccount);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(499, 286);
			this.tabControl1.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.panel1);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabGeneral.Size = new System.Drawing.Size(491, 260);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "General";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// tabAccount
			// 
			this.tabAccount.Controls.Add(this.panel2);
			this.tabAccount.Location = new System.Drawing.Point(4, 22);
			this.tabAccount.Name = "tabAccount";
			this.tabAccount.Padding = new System.Windows.Forms.Padding(3);
			this.tabAccount.Size = new System.Drawing.Size(491, 260);
			this.tabAccount.TabIndex = 1;
			this.tabAccount.Text = "Account";
			this.tabAccount.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.groupBox6);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(2);
			this.panel2.Size = new System.Drawing.Size(485, 254);
			this.panel2.TabIndex = 1;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this._lblSpaceUsed);
			this.groupBox6.Controls.Add(this._lblTotalFiles);
			this.groupBox6.Controls.Add(this.label9);
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Location = new System.Drawing.Point(3, 137);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(475, 74);
			this.groupBox6.TabIndex = 2;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Account Information";
			// 
			// _lblSpaceUsed
			// 
			this._lblSpaceUsed.Location = new System.Drawing.Point(91, 42);
			this._lblSpaceUsed.Name = "_lblSpaceUsed";
			this._lblSpaceUsed.Size = new System.Drawing.Size(375, 20);
			this._lblSpaceUsed.TabIndex = 4;
			this._lblSpaceUsed.Text = "N/A";
			this._lblSpaceUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblTotalFiles
			// 
			this._lblTotalFiles.Location = new System.Drawing.Point(91, 17);
			this._lblTotalFiles.Name = "_lblTotalFiles";
			this._lblTotalFiles.Size = new System.Drawing.Size(375, 20);
			this._lblTotalFiles.TabIndex = 3;
			this._lblTotalFiles.Text = "N/A";
			this._lblTotalFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 46);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 13);
			this.label9.TabIndex = 2;
			this.label9.Text = "Space Used:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 21);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(58, 13);
			this.label8.TabIndex = 1;
			this.label8.Text = "Total Files:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._itxtPassword);
			this.groupBox2.Controls.Add(this._itxtUsername);
			this.groupBox2.Controls.Add(this._btnCreateAccount);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(5, 5);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(475, 126);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Account Settings";
			// 
			// _btnCreateAccount
			// 
			this._btnCreateAccount.Location = new System.Drawing.Point(366, 95);
			this._btnCreateAccount.Name = "_btnCreateAccount";
			this._btnCreateAccount.Size = new System.Drawing.Size(103, 23);
			this._btnCreateAccount.TabIndex = 5;
			this._btnCreateAccount.Text = "Create Account";
			this._btnCreateAccount.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 59);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Password:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Username:";
			// 
			// _itxtPassword
			// 
			this._itxtPassword.Location = new System.Drawing.Point(92, 56);
			this._itxtPassword.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this._itxtPassword.Name = "_itxtPassword";
			this._itxtPassword.Size = new System.Drawing.Size(377, 35);
			this._itxtPassword.TabIndex = 7;
			this._itxtPassword.TextInfo = "";
			this._itxtPassword.Value = "";
			// 
			// _itxtUsername
			// 
			this._itxtUsername.Location = new System.Drawing.Point(92, 19);
			this._itxtUsername.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this._itxtUsername.Name = "_itxtUsername";
			this._itxtUsername.Size = new System.Drawing.Size(377, 35);
			this._itxtUsername.TabIndex = 6;
			this._itxtUsername.TextInfo = "";
			this._itxtUsername.Value = "";
			// 
			// _lblServerAddress
			// 
			this._lblServerAddress.Location = new System.Drawing.Point(94, 49);
			this._lblServerAddress.Name = "_lblServerAddress";
			this._lblServerAddress.Size = new System.Drawing.Size(358, 13);
			this._lblServerAddress.TabIndex = 10;
			// 
			// _itxtServerKey
			// 
			this._itxtServerKey.Location = new System.Drawing.Point(94, 73);
			this._itxtServerKey.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this._itxtServerKey.Name = "_itxtServerKey";
			this._itxtServerKey.Size = new System.Drawing.Size(358, 35);
			this._itxtServerKey.TabIndex = 11;
			this._itxtServerKey.TextInfo = "";
			this._itxtServerKey.Value = "";
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(518, 306);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmSettings";
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.frmSettings_Load);
			this.panel1.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabAccount.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button _btnNewServer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button _btnDeleteAllFiles;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button _btnRemoveLocalSettings;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Panel spacer;
		private System.Windows.Forms.Button _btnManageFiles;
		private System.Windows.Forms.Button _btnDeleteServer;
		private System.Windows.Forms.Button _btnSaveServer;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabAccount;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button _btnCreateAccount;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox _cmbMaxUploadSpeed;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox _cmbTotalConcurrentUploads;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label9;
		private TextBoxAndInfo _itxtUsername;
		private TextBoxAndInfo _itxtPassword;
		public System.Windows.Forms.ComboBox _cbxServerPresets;
		public TextBoxAndInfo _itxtServerKey;
		private System.Windows.Forms.Label _lblServerAddress;
		public System.Windows.Forms.Label _lblSpaceUsed;
		public System.Windows.Forms.Label _lblTotalFiles;


	}
}

