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
			this.picLogo = new System.Windows.Forms.PictureBox();
			this._itxtPassword = new dtxUpload.TextBoxAndInfo();
			this._itxtUsername = new dtxUpload.TextBoxAndInfo();
			this.panel1.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
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
			// picLogo
			// 
			this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this.picLogo.Image = global::dtxUpload.Properties.Resources.LoginLogoRev1;
			this.picLogo.Location = new System.Drawing.Point(0, 0);
			this.picLogo.Name = "picLogo";
			this.picLogo.Size = new System.Drawing.Size(234, 71);
			this.picLogo.TabIndex = 2;
			this.picLogo.TabStop = false;
			this.picLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseDown);
			this.picLogo.MouseLeave += new System.EventHandler(this.picLogo_MouseLeave);
			this.picLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseMove);
			this.picLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picLogo_MouseUp);
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
			this.Controls.Add(this._cmbServer);
			this.Controls.Add(this._lblWarnServer);
			this.Controls.Add(this._chkSavePassword);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.picLogo);
			this.Controls.Add(this._itxtPassword);
			this.Controls.Add(this._itxtUsername);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(250, 318);
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
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBoxAndInfo _itxtUsername;
		private TextBoxAndInfo _itxtPassword;
		private System.Windows.Forms.PictureBox picLogo;
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
	}
}