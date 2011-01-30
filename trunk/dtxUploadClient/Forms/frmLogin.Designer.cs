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
			this.panel1 = new System.Windows.Forms.Panel();
			this._btnSettings = new System.Windows.Forms.Button();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.manageFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._chkSavePassword = new System.Windows.Forms.CheckBox();
			this._lblWarnServer = new System.Windows.Forms.Label();
			this._btnRemoveServer = new System.Windows.Forms.Button();
			this._picLogo = new System.Windows.Forms.PictureBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this._itxtPassword = new dtxUpload.TextBoxAndInfo();
			this._itxtUsername = new dtxUpload.TextBoxAndInfo();
			this.panel1.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// _cmbServer
			// 
			this._cmbServer.FormattingEnabled = true;
			this._cmbServer.Location = new System.Drawing.Point(15, 99);
			this._cmbServer.Name = "_cmbServer";
			this._cmbServer.Size = new System.Drawing.Size(207, 21);
			this._cmbServer.TabIndex = 1;
			this._cmbServer.SelectedIndexChanged += new System.EventHandler(this._cmbServer_SelectedIndexChanged);
			this._cmbServer.Leave += new System.EventHandler(this._cmbServer_Leave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 83);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 137);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Username:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 189);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Password:";
			// 
			// _btnCancel
			// 
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Location = new System.Drawing.Point(147, 2);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(75, 23);
			this._btnCancel.TabIndex = 1;
			this._btnCancel.Text = "&Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _btnLogin
			// 
			this._btnLogin.Location = new System.Drawing.Point(66, 2);
			this._btnLogin.Name = "_btnLogin";
			this._btnLogin.Size = new System.Drawing.Size(75, 23);
			this._btnLogin.TabIndex = 0;
			this._btnLogin.Text = "&Login";
			this._btnLogin.UseVisualStyleBackColor = true;
			this._btnLogin.Click += new System.EventHandler(this._btnLogin_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._btnSettings);
			this.panel1.Controls.Add(this._btnLogin);
			this.panel1.Controls.Add(this._btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 270);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(234, 32);
			this.panel1.TabIndex = 9;
			// 
			// _btnSettings
			// 
			this._btnSettings.Image = global::dtxUpload.Properties.Resources.icon_16_tool_b;
			this._btnSettings.Location = new System.Drawing.Point(12, 2);
			this._btnSettings.Name = "_btnSettings";
			this._btnSettings.Size = new System.Drawing.Size(24, 23);
			this._btnSettings.TabIndex = 2;
			this._btnSettings.UseVisualStyleBackColor = true;
			this._btnSettings.Click += new System.EventHandler(this._btnSettings_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "Dtronix Upload";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageFilesToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(144, 76);
			// 
			// manageFilesToolStripMenuItem
			// 
			this.manageFilesToolStripMenuItem.Name = "manageFilesToolStripMenuItem";
			this.manageFilesToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.manageFilesToolStripMenuItem.Text = "Manage Files";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.settingsToolStripMenuItem.Text = "&Settings";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// _chkSavePassword
			// 
			this._chkSavePassword.AutoSize = true;
			this._chkSavePassword.Location = new System.Drawing.Point(12, 244);
			this._chkSavePassword.Name = "_chkSavePassword";
			this._chkSavePassword.Size = new System.Drawing.Size(100, 17);
			this._chkSavePassword.TabIndex = 6;
			this._chkSavePassword.Text = "Save Password";
			this._chkSavePassword.UseVisualStyleBackColor = true;
			// 
			// _lblWarnServer
			// 
			this._lblWarnServer.Location = new System.Drawing.Point(12, 121);
			this._lblWarnServer.Name = "_lblWarnServer";
			this._lblWarnServer.Size = new System.Drawing.Size(210, 16);
			this._lblWarnServer.TabIndex = 10;
			// 
			// _btnRemoveServer
			// 
			this._btnRemoveServer.Location = new System.Drawing.Point(254, 97);
			this._btnRemoveServer.Name = "_btnRemoveServer";
			this._btnRemoveServer.Size = new System.Drawing.Size(55, 23);
			this._btnRemoveServer.TabIndex = 11;
			this._btnRemoveServer.Text = "Remove";
			this._btnRemoveServer.UseVisualStyleBackColor = true;
			// 
			// _picLogo
			// 
			this._picLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this._picLogo.ErrorImage = null;
			this._picLogo.Image = global::dtxUpload.Properties.Resources.LoginLogoRev2;
			this._picLogo.InitialImage = null;
			this._picLogo.Location = new System.Drawing.Point(0, 0);
			this._picLogo.Name = "_picLogo";
			this._picLogo.Size = new System.Drawing.Size(234, 70);
			this._picLogo.TabIndex = 2;
			this._picLogo.TabStop = false;
			this._picLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseDown);
			this._picLogo.MouseLeave += new System.EventHandler(this.picLogo_MouseLeave);
			this._picLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseMove);
			this._picLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseUp);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "Auto (Always smaller image)",
            "Jpeg (Smaller lossy format)",
            "PNG (Large lossless format) "});
			this.comboBox1.Location = new System.Drawing.Point(254, 153);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(157, 21);
			this.comboBox1.TabIndex = 12;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(251, 137);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(99, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Screenshot Format:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(414, 137);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Quality:";
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
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
			this.comboBox2.Location = new System.Drawing.Point(417, 153);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(55, 21);
			this.comboBox2.TabIndex = 15;
			// 
			// _itxtPassword
			// 
			this._itxtPassword.ForeColor = System.Drawing.SystemColors.ControlText;
			this._itxtPassword.InfoForeColor = System.Drawing.Color.Black;
			this._itxtPassword.Location = new System.Drawing.Point(15, 205);
			this._itxtPassword.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this._itxtPassword.Name = "_itxtPassword";
			this._itxtPassword.PassChar = '*';
			this._itxtPassword.Size = new System.Drawing.Size(207, 35);
			this._itxtPassword.TabIndex = 5;
			this._itxtPassword.TextInfo = "";
			this._itxtPassword.Value = "";
			// 
			// _itxtUsername
			// 
			this._itxtUsername.ForeColor = System.Drawing.SystemColors.ControlText;
			this._itxtUsername.InfoForeColor = System.Drawing.Color.Black;
			this._itxtUsername.Location = new System.Drawing.Point(15, 153);
			this._itxtUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this._itxtUsername.Name = "_itxtUsername";
			this._itxtUsername.PassChar = '\0';
			this._itxtUsername.Size = new System.Drawing.Size(207, 35);
			this._itxtUsername.TabIndex = 3;
			this._itxtUsername.TextInfo = "";
			this._itxtUsername.Value = "";
			// 
			// frmLogin
			// 
			this.AcceptButton = this._btnLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(234, 302);
			this.ControlBox = false;
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this._cmbServer);
			this.Controls.Add(this._lblWarnServer);
			this.Controls.Add(this._chkSavePassword);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._btnRemoveServer);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._picLogo);
			this.Controls.Add(this._itxtPassword);
			this.Controls.Add(this._itxtUsername);
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
			this.Deactivate += new System.EventHandler(this.frmLogin_Deactivate);
			this.Load += new System.EventHandler(this.frmLogin_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
			this.MouseLeave += new System.EventHandler(this.frmLogin_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseUp);
			this.panel1.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._picLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.CheckBox _chkSavePassword;
		private System.Windows.Forms.Button _btnSettings;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem manageFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.Label _lblWarnServer;
		private System.Windows.Forms.Button _btnRemoveServer;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox2;
	}
}