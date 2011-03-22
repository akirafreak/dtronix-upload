namespace dtxUpload {
	partial class frmLogin {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
			this._cmbServer = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this._btnCancel = new System.Windows.Forms.Button();
			this._btnLogin = new System.Windows.Forms.Button();
			this._panButtons = new System.Windows.Forms.Panel();
			this._btnSettings = new System.Windows.Forms.Button();
			this._btnConfigDone = new System.Windows.Forms.Button();
			this._notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this._chkSavePassword = new System.Windows.Forms.CheckBox();
			this._lblWarnServer = new System.Windows.Forms.Label();
			this._btnRemoveServer = new System.Windows.Forms.Button();
			this._cmbScreenshotFormat = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this._cmbScreenshotQuality = new System.Windows.Forms.ComboBox();
			this._panLoginInputs = new System.Windows.Forms.Panel();
			this._itxtUsername = new dtxUpload.TextBoxAndInfo();
			this._itxtPassword = new dtxUpload.TextBoxAndInfo();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._tipDefault = new System.Windows.Forms.ToolTip(this.components);
			this._picLogo = new System.Windows.Forms.PictureBox();
			this._vistaMenu = new dtxCore.Controls.VistaMenu(this.components);
			this._cmiUploadCropScreenshot = new System.Windows.Forms.MenuItem();
			this._cmiUploadScreenshot = new System.Windows.Forms.MenuItem();
			this._contextMenu = new System.Windows.Forms.ContextMenu();
			this._cmiManageFiles = new System.Windows.Forms.MenuItem();
			this._cmiUploadFiles = new System.Windows.Forms.MenuItem();
			this._cmiBrowseToServer = new System.Windows.Forms.MenuItem();
			this._cmiLoggedSeparator = new System.Windows.Forms.MenuItem();
			this._cmiLogin = new System.Windows.Forms.MenuItem();
			this._cmiLogout = new System.Windows.Forms.MenuItem();
			this._cmiSettings = new System.Windows.Forms.MenuItem();
			this._cmiSettingsConfirmUpload = new System.Windows.Forms.MenuItem();
			this._cmiSettingsUploadCopy = new System.Windows.Forms.MenuItem();
			this._cmiAbout = new System.Windows.Forms.MenuItem();
			this._cmiExit = new System.Windows.Forms.MenuItem();
			this._panButtons.SuspendLayout();
			this._panLoginInputs.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._vistaMenu)).BeginInit();
			this.SuspendLayout();
			// 
			// _cmbServer
			// 
			this._cmbServer.FormattingEnabled = true;
			this._cmbServer.Location = new System.Drawing.Point(12, 25);
			this._cmbServer.Name = "_cmbServer";
			this._cmbServer.Size = new System.Drawing.Size(210, 21);
			this._cmbServer.TabIndex = 1;
			this._cmbServer.SelectedIndexChanged += new System.EventHandler(this._cmbServer_SelectedIndexChanged);
			this._cmbServer.Leave += new System.EventHandler(this._cmbServer_Leave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Username:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 115);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Password:";
			// 
			// _btnCancel
			// 
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Location = new System.Drawing.Point(166, 3);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(56, 23);
			this._btnCancel.TabIndex = 1;
			this._btnCancel.Text = "&Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _btnLogin
			// 
			this._btnLogin.Location = new System.Drawing.Point(104, 3);
			this._btnLogin.Name = "_btnLogin";
			this._btnLogin.Size = new System.Drawing.Size(56, 23);
			this._btnLogin.TabIndex = 0;
			this._btnLogin.Text = "&Login";
			this._btnLogin.UseVisualStyleBackColor = true;
			this._btnLogin.Click += new System.EventHandler(this._btnLogin_Click);
			// 
			// _panButtons
			// 
			this._panButtons.Controls.Add(this._btnSettings);
			this._panButtons.Controls.Add(this._btnConfigDone);
			this._panButtons.Controls.Add(this._btnCancel);
			this._panButtons.Controls.Add(this._btnLogin);
			this._panButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._panButtons.Location = new System.Drawing.Point(0, 278);
			this._panButtons.Name = "_panButtons";
			this._panButtons.Size = new System.Drawing.Size(242, 32);
			this._panButtons.TabIndex = 9;
			// 
			// _btnSettings
			// 
			this._btnSettings.Image = global::dtxUpload.Properties.Resources.icon_16_tool_b;
			this._btnSettings.Location = new System.Drawing.Point(9, 3);
			this._btnSettings.Name = "_btnSettings";
			this._btnSettings.Size = new System.Drawing.Size(24, 23);
			this._btnSettings.TabIndex = 2;
			this._btnSettings.UseVisualStyleBackColor = true;
			this._btnSettings.Click += new System.EventHandler(this._btnSettings_Click);
			// 
			// _btnConfigDone
			// 
			this._btnConfigDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnConfigDone.Location = new System.Drawing.Point(416, 2);
			this._btnConfigDone.Name = "_btnConfigDone";
			this._btnConfigDone.Size = new System.Drawing.Size(56, 23);
			this._btnConfigDone.TabIndex = 3;
			this._btnConfigDone.Text = "&Done";
			this._tipDefault.SetToolTip(this._btnConfigDone, "Complete configuration changes.");
			this._btnConfigDone.UseVisualStyleBackColor = true;
			this._btnConfigDone.Click += new System.EventHandler(this._btnConfigDone_Click);
			// 
			// _notifyIcon
			// 
			this._notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_notifyIcon.Icon")));
			this._notifyIcon.Text = "Dtronix Upload";
			this._notifyIcon.Visible = true;
			this._notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
			// 
			// _chkSavePassword
			// 
			this._chkSavePassword.AutoSize = true;
			this._chkSavePassword.Location = new System.Drawing.Point(12, 170);
			this._chkSavePassword.Name = "_chkSavePassword";
			this._chkSavePassword.Size = new System.Drawing.Size(100, 17);
			this._chkSavePassword.TabIndex = 6;
			this._chkSavePassword.Text = "Save Password";
			this._chkSavePassword.UseVisualStyleBackColor = true;
			// 
			// _lblWarnServer
			// 
			this._lblWarnServer.Location = new System.Drawing.Point(9, 47);
			this._lblWarnServer.Name = "_lblWarnServer";
			this._lblWarnServer.Size = new System.Drawing.Size(213, 16);
			this._lblWarnServer.TabIndex = 10;
			// 
			// _btnRemoveServer
			// 
			this._btnRemoveServer.Location = new System.Drawing.Point(237, 95);
			this._btnRemoveServer.Name = "_btnRemoveServer";
			this._btnRemoveServer.Size = new System.Drawing.Size(55, 23);
			this._btnRemoveServer.TabIndex = 11;
			this._btnRemoveServer.Text = "Remove";
			this._tipDefault.SetToolTip(this._btnRemoveServer, "Remove currently selected server.");
			this._btnRemoveServer.UseVisualStyleBackColor = true;
			// 
			// _cmbScreenshotFormat
			// 
			this._cmbScreenshotFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbScreenshotFormat.FormattingEnabled = true;
			this._cmbScreenshotFormat.Items.AddRange(new object[] {
            "Auto (Always smaller image)",
            "Jpeg (Smaller lossy format)",
            "PNG (Larger lossless format)"});
			this._cmbScreenshotFormat.Location = new System.Drawing.Point(9, 32);
			this._cmbScreenshotFormat.Name = "_cmbScreenshotFormat";
			this._cmbScreenshotFormat.Size = new System.Drawing.Size(157, 21);
			this._cmbScreenshotFormat.TabIndex = 12;
			this._tipDefault.SetToolTip(this._cmbScreenshotFormat, "File format for screenshots.");
			this._cmbScreenshotFormat.SelectedIndexChanged += new System.EventHandler(this._cmbScreenshotFormat_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(99, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Screenshot Format:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(169, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "JPG Quality:";
			// 
			// _cmbScreenshotQuality
			// 
			this._cmbScreenshotQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbScreenshotQuality.FormattingEnabled = true;
			this._cmbScreenshotQuality.Items.AddRange(new object[] {
            "100",
            "90",
            "80",
            "70",
            "60",
            "50",
            "40",
            "30",
            "20",
            "10"});
			this._cmbScreenshotQuality.Location = new System.Drawing.Point(172, 32);
			this._cmbScreenshotQuality.Name = "_cmbScreenshotQuality";
			this._cmbScreenshotQuality.Size = new System.Drawing.Size(63, 21);
			this._cmbScreenshotQuality.TabIndex = 15;
			this._tipDefault.SetToolTip(this._cmbScreenshotQuality, "Quality of the compression used for JPEG images.  Higher is better quality and bi" +
					"gger file sizes.");
			this._cmbScreenshotQuality.SelectedIndexChanged += new System.EventHandler(this._cmbScreenshotQuality_SelectedIndexChanged);
			// 
			// _panLoginInputs
			// 
			this._panLoginInputs.Controls.Add(this._itxtUsername);
			this._panLoginInputs.Controls.Add(this._itxtPassword);
			this._panLoginInputs.Controls.Add(this.label1);
			this._panLoginInputs.Controls.Add(this.label3);
			this._panLoginInputs.Controls.Add(this.label2);
			this._panLoginInputs.Controls.Add(this._chkSavePassword);
			this._panLoginInputs.Controls.Add(this._cmbServer);
			this._panLoginInputs.Controls.Add(this._lblWarnServer);
			this._panLoginInputs.Location = new System.Drawing.Point(0, 72);
			this._panLoginInputs.Margin = new System.Windows.Forms.Padding(0);
			this._panLoginInputs.Name = "_panLoginInputs";
			this._panLoginInputs.Size = new System.Drawing.Size(234, 197);
			this._panLoginInputs.TabIndex = 16;
			// 
			// _itxtUsername
			// 
			this._itxtUsername.ForeColor = System.Drawing.SystemColors.ControlText;
			this._itxtUsername.InfoForeColor = System.Drawing.Color.Black;
			this._itxtUsername.Location = new System.Drawing.Point(12, 79);
			this._itxtUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this._itxtUsername.Name = "_itxtUsername";
			this._itxtUsername.PassChar = '\0';
			this._itxtUsername.Size = new System.Drawing.Size(210, 35);
			this._itxtUsername.TabIndex = 3;
			this._itxtUsername.TextInfo = "";
			this._itxtUsername.Value = "";
			// 
			// _itxtPassword
			// 
			this._itxtPassword.ForeColor = System.Drawing.SystemColors.ControlText;
			this._itxtPassword.InfoForeColor = System.Drawing.Color.Black;
			this._itxtPassword.Location = new System.Drawing.Point(12, 131);
			this._itxtPassword.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this._itxtPassword.Name = "_itxtPassword";
			this._itxtPassword.PassChar = '*';
			this._itxtPassword.Size = new System.Drawing.Size(210, 35);
			this._itxtPassword.TabIndex = 5;
			this._itxtPassword.TextInfo = "";
			this._itxtPassword.Value = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this._cmbScreenshotFormat);
			this.groupBox1.Controls.Add(this._cmbScreenshotQuality);
			this.groupBox1.Location = new System.Drawing.Point(237, 126);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 66);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// _picLogo
			// 
			this._picLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this._picLogo.ErrorImage = null;
			this._picLogo.Image = global::dtxUpload.Properties.Resources.LoginLogoRev2;
			this._picLogo.InitialImage = null;
			this._picLogo.Location = new System.Drawing.Point(0, 0);
			this._picLogo.Name = "_picLogo";
			this._picLogo.Size = new System.Drawing.Size(242, 70);
			this._picLogo.TabIndex = 2;
			this._picLogo.TabStop = false;
			this._picLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseDown);
			this._picLogo.MouseLeave += new System.EventHandler(this.picLogo_MouseLeave);
			this._picLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseMove);
			this._picLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseUp);
			// 
			// _vistaMenu
			// 
			this._vistaMenu.ContainerControl = this;
			// 
			// _cmiUploadCropScreenshot
			// 
			this._vistaMenu.SetImage(this._cmiUploadCropScreenshot, global::dtxUpload.Properties.Resources.icon_paper_excerpt_blue_16_ns);
			this._cmiUploadCropScreenshot.Index = 2;
			this._cmiUploadCropScreenshot.Text = "Upload Crop Screenshot";
			this._cmiUploadCropScreenshot.Visible = false;
			this._cmiUploadCropScreenshot.Click += new System.EventHandler(this._cmiUploadCropScreenshot_Click);
			// 
			// _cmiUploadScreenshot
			// 
			this._vistaMenu.SetImage(this._cmiUploadScreenshot, global::dtxUpload.Properties.Resources.icon_note_16_ns);
			this._cmiUploadScreenshot.Index = 3;
			this._cmiUploadScreenshot.Text = "Upload Screenshot";
			this._cmiUploadScreenshot.Visible = false;
			this._cmiUploadScreenshot.Click += new System.EventHandler(this._cmiUploadScreenshot_Click);
			// 
			// _contextMenu
			// 
			this._contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._cmiManageFiles,
            this._cmiUploadFiles,
            this._cmiUploadCropScreenshot,
            this._cmiUploadScreenshot,
            this._cmiBrowseToServer,
            this._cmiLoggedSeparator,
            this._cmiLogin,
            this._cmiLogout,
            this._cmiSettings,
            this._cmiAbout,
            this._cmiExit});
			this._contextMenu.Popup += new System.EventHandler(this._contextMenu_Popup);
			// 
			// _cmiManageFiles
			// 
			this._cmiManageFiles.Index = 0;
			this._cmiManageFiles.Text = "Manage Files";
			this._cmiManageFiles.Visible = false;
			this._cmiManageFiles.Click += new System.EventHandler(this._cmiManageFiles_Click);
			// 
			// _cmiUploadFiles
			// 
			this._cmiUploadFiles.Index = 1;
			this._cmiUploadFiles.Text = "Upload Files";
			this._cmiUploadFiles.Visible = false;
			this._cmiUploadFiles.Click += new System.EventHandler(this._cmiUploadFiles_Click);
			// 
			// _cmiBrowseToServer
			// 
			this._cmiBrowseToServer.Index = 4;
			this._cmiBrowseToServer.Text = "Browse to Server";
			this._cmiBrowseToServer.Visible = false;
			this._cmiBrowseToServer.Click += new System.EventHandler(this._cmiBrowseToServer_Click);
			// 
			// _cmiLoggedSeparator
			// 
			this._cmiLoggedSeparator.Index = 5;
			this._cmiLoggedSeparator.Text = "-";
			this._cmiLoggedSeparator.Visible = false;
			// 
			// _cmiLogin
			// 
			this._cmiLogin.Index = 6;
			this._cmiLogin.Text = "Login";
			this._cmiLogin.Click += new System.EventHandler(this._cmiLogin_Click);
			// 
			// _cmiLogout
			// 
			this._cmiLogout.Index = 7;
			this._cmiLogout.Text = "Logout";
			this._cmiLogout.Visible = false;
			this._cmiLogout.Click += new System.EventHandler(this._cmiLogout_Click);
			// 
			// _cmiSettings
			// 
			this._cmiSettings.Index = 8;
			this._cmiSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._cmiSettingsConfirmUpload,
            this._cmiSettingsUploadCopy});
			this._cmiSettings.Text = "Settings";
			// 
			// _cmiSettingsConfirmUpload
			// 
			this._cmiSettingsConfirmUpload.Index = 0;
			this._cmiSettingsConfirmUpload.Text = "Confirm clipboard upload";
			this._cmiSettingsConfirmUpload.Click += new System.EventHandler(this._cmiSettingsConfirmUpload_Click);
			// 
			// _cmiSettingsUploadCopy
			// 
			this._cmiSettingsUploadCopy.Index = 1;
			this._cmiSettingsUploadCopy.Text = "Copy upload URL to clipboard";
			this._cmiSettingsUploadCopy.Click += new System.EventHandler(this._cmiSettingsUploadCopy_Click);
			// 
			// _cmiAbout
			// 
			this._cmiAbout.Index = 9;
			this._cmiAbout.Text = "About";
			this._cmiAbout.Click += new System.EventHandler(this._cmiAbout_Click);
			// 
			// _cmiExit
			// 
			this._cmiExit.Index = 10;
			this._cmiExit.Text = "Exit";
			this._cmiExit.Click += new System.EventHandler(this._cmiExit_Click);
			// 
			// frmLogin
			// 
			this.AcceptButton = this._btnLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(242, 310);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._panLoginInputs);
			this.Controls.Add(this._panButtons);
			this.Controls.Add(this._picLogo);
			this.Controls.Add(this._btnRemoveServer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(500, 318);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(250, 318);
			this.Name = "frmLogin";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.Activated += new System.EventHandler(this.frmLogin_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLogin_FormClosed);
			this.Load += new System.EventHandler(this.frmLogin_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
			this.MouseLeave += new System.EventHandler(this.frmLogin_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseUp);
			this._panButtons.ResumeLayout(false);
			this._panLoginInputs.ResumeLayout(false);
			this._panLoginInputs.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._vistaMenu)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TextBoxAndInfo _itxtUsername;
		private TextBoxAndInfo _itxtPassword;
		private System.Windows.Forms.PictureBox _picLogo;
		private System.Windows.Forms.ComboBox _cmbServer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.Button _btnLogin;
		private System.Windows.Forms.Panel _panButtons;
		private System.Windows.Forms.NotifyIcon _notifyIcon;
		private System.Windows.Forms.CheckBox _chkSavePassword;
		private System.Windows.Forms.Button _btnSettings;
		private System.Windows.Forms.Label _lblWarnServer;
		private System.Windows.Forms.Button _btnRemoveServer;
		private System.Windows.Forms.ComboBox _cmbScreenshotFormat;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox _cmbScreenshotQuality;
		private System.Windows.Forms.Button _btnConfigDone;
		private System.Windows.Forms.Panel _panLoginInputs;
		private System.Windows.Forms.ToolTip _tipDefault;
		private System.Windows.Forms.GroupBox groupBox1;
		private dtxCore.Controls.VistaMenu _vistaMenu;
		private System.Windows.Forms.ContextMenu _contextMenu;
		private System.Windows.Forms.MenuItem _cmiManageFiles;
		private System.Windows.Forms.MenuItem _cmiUploadFiles;
		private System.Windows.Forms.MenuItem _cmiUploadCropScreenshot;
		private System.Windows.Forms.MenuItem _cmiUploadScreenshot;
		private System.Windows.Forms.MenuItem _cmiLoggedSeparator;
		private System.Windows.Forms.MenuItem _cmiLogin;
		private System.Windows.Forms.MenuItem _cmiLogout;
		private System.Windows.Forms.MenuItem _cmiSettings;
		private System.Windows.Forms.MenuItem _cmiSettingsConfirmUpload;
		private System.Windows.Forms.MenuItem _cmiExit;
		private System.Windows.Forms.MenuItem _cmiSettingsUploadCopy;
		private System.Windows.Forms.MenuItem _cmiAbout;
		private System.Windows.Forms.MenuItem _cmiBrowseToServer;
	}
}