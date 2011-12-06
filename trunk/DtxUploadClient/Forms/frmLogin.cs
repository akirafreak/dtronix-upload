using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dtxCore;
using dtxCore.Json;
using System.Net;
using System.Reflection;

namespace DtxUpload {
	public partial class frmLogin : Form {
		private ServerConnector connector;

		private List<DC_Server> server_list;
		public DateTime last_ping_time;

		private Tween tween_image_height;
		private Timer ping_timer = new Timer();

		private BackgroundWorker loadLogoWorker = new BackgroundWorker();

		// Hides the resize on the window.
		protected override void WndProc(ref Message m) {
			if(m.Msg == 0x20) {
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}

		public frmLogin() {
			Client.form_Login = this;
			connector = new ServerConnector(this);

			InitializeComponent();

			tween_image_height = new Tween(_picLogo, "Height", dtxCore.EasingEquations.expoEaseOut);
			_picLogo.LoadCompleted += new AsyncCompletedEventHandler(_picLogo_LoadCompleted);

			// Threading
			loadLogoWorker.DoWork += new DoWorkEventHandler(loadLogoWorker_DoWork);
			loadLogoWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadLogoWorker_RunWorkerCompleted);

			// Add the native context menu.
			_notifyIcon.ContextMenu = _contextMenu;

			// Make sure the form is the correct size.  Windows XP's form widths are less than 7's.
			OperatingSystemInfo osi = Utilities.getOSInfo();
			if(osi.os.Contains("XP")) {
				Size min_size = new Size() {
					Height = this.MinimumSize.Height - 8,
					Width = this.MinimumSize.Width - 8
				};

				Size max_size = new Size() {
					Height = this.MaximumSize.Height - 8,
					Width = this.MaximumSize.Width - 8
				};

				this.MinimumSize = min_size;
				this.MaximumSize = max_size;
				this.Size = min_size;
			}

			// Timer to ensure the session does not expire.
			ping_timer.Enabled = false;
			ping_timer.Tick += new EventHandler(ping_timer_Tick);
			ping_timer.Interval = 1000 * 30; // 20 seconds.


			frmLogin_Activated(new object(), new EventArgs());
		}

		private void frmLogin_Load(object sender, EventArgs e) {
			// Select the last server connected to.
			string last_server = Client.config.get<string>("frmlogin.last_server");
			server_list = new List<DC_Server>(Client.config.get<DC_Server[]>("frmlogin.servers_list"));

			foreach(DC_Server server in server_list) {
				_cmbServer.Items.Add(server.url);

				if(last_server == server.name) {
					_cmbServer.Text = server.url;
				}
			}
		}

		#region Moving events for the dragging of the form.

		private bool start_move = false;

		private static class previous_mouse_location {
			public static int X;
			public static int Y;
		}

		private void frmLogin_MouseDown(object sender, MouseEventArgs e) {
			start_move = true;
			previous_mouse_location.X = e.X;
			previous_mouse_location.Y = e.Y;
		}

		private void frmLogin_MouseUp(object sender, MouseEventArgs e) {
			start_move = false;
		}

		private void frmLogin_MouseMove(object sender, MouseEventArgs e) {
			if(start_move) {
				this.Left += (e.X - previous_mouse_location.X);
				this.Top += (e.Y - previous_mouse_location.Y);


			}
		}

		private void frmLogin_MouseLeave(object sender, EventArgs e) {
			start_move = false;
		}

		private void picLogo_MouseDown(object sender, MouseEventArgs e) {
			frmLogin_MouseDown(sender, e);
		}

		private void picLogo_MouseUp(object sender, MouseEventArgs e) {
			frmLogin_MouseUp(sender, e);
		}

		private void picLogo_MouseMove(object sender, MouseEventArgs e) {
			frmLogin_MouseMove(sender, e);
		}

		private void picLogo_MouseLeave(object sender, EventArgs e) {
			frmLogin_MouseLeave(sender, e);
		}
		#endregion

		private void _btnCancel_Click(object sender, EventArgs e) {
			this.Hide();
		}

		private void frmLogin_Activated(object sender, EventArgs e) {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 8, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 8);
		}


		void ping_timer_Tick(object sender, EventArgs e) {
			connector.callServerMethod("Server:ping");
		}

		private void _btnLogin_Click(object sender, EventArgs e) {
			if(_itxtUsername.Value == "") {
				_itxtUsername.SetInfo("Enter a username.", Color.Red);
				_itxtUsername.Focus();

			} else if(_itxtPassword.Value == "") {
				_itxtPassword.SetInfo("Enter a password.", Color.Red);
				_itxtPassword.Focus();

			} else if(_cmbServer.Text == "") {
				_lblWarnServer.Text = "Enter a server URL.";
				_lblWarnServer.ForeColor = Color.Red;
				_cmbServer.Focus();

			} else {
				// CHANGE?  If a user's password is exactly 32 characters, then it will never be hashed when sent to the server. Lets just hope nobody has a password an exact width of 32 characters.
				if(_itxtPassword.Value.Length != 32) {
					_itxtPassword.Value = dtxCore.Utilities.md5Hash(_itxtPassword.Value);
				}

				_btnLogin.Enabled = false;
				connector.user_info.username = _itxtUsername.Value;
				connector.user_info.password = _itxtPassword.Value;
				connector.connect();
			}
		}

		private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
			if(e.Button == MouseButtons.Left) {
				if(Client.server_info.is_connected) {
					Client.form_QuickUpload.Show();
					Client.form_QuickUpload.Activate();

				} else {
					this.Show();
					this.Activate();
				}
			}
		}


		private void _cmbServer_Leave(object sender, EventArgs e) {
			if(Client.server_info.server_url == null || !Client.server_info.server_url.Contains(_cmbServer.Text)) {
				_cmbServer_SelectedIndexChanged(sender, e);
			}
		}

		private void _cmbServer_SelectedIndexChanged(object sender, EventArgs e) {
			string protocol = "";
			if(!_cmbServer.Text.ToLower().Contains("http://")) {
				protocol = "http://";
			}

			try {
				new Uri(protocol + _cmbServer.Text);
			} catch {
				serverInvalid();
				return;
			}

			_lblWarnServer.Text = "";
			connector.server_info.server_url = protocol + _cmbServer.Text;
			connector.getServerInfo();

			if(server_list != null) {
				for(int i = 0; i < server_list.Count; i++) {
					if(server_list[i].url == _cmbServer.Text) {
						_itxtUsername.Value = server_list[i].username;
						_itxtPassword.Value = server_list[i].password;
						_chkSavePassword.Checked = server_list[i].save_pass;
					}
				}
			}
		}

		public void invalidPassword() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();

			_btnLogin.Enabled = true;
			_itxtPassword.SetInfo("Password is invalid.", Color.Red);
			_itxtPassword.Focus();
		}

		public void invalidUsername() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();

			_btnLogin.Enabled = true;
			_itxtUsername.SetInfo("Username is not registered on server.", Color.Red);
			_itxtUsername.Focus();
		}

		public void serverInvalid() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();

			_btnLogin.Enabled = true;
			_lblWarnServer.Text = "Server is not responding.";
			_lblWarnServer.ForeColor = Color.Red;
		}

		public void serverMaintenanceMode() {
			if (this.WindowState != FormWindowState.Normal) this.ShowDialog();

			_btnLogin.Enabled = true;
			_lblWarnServer.Text = "Server is in maintenance mode.";
			_lblWarnServer.ForeColor = Color.Red;
		}

		/// <summary>
		/// Method that is called when the server reports that the session has expired.
		/// </summary>
		public void sessionExpired() {
			loggedOut();

			_btnLogin.Enabled = true;
			_lblWarnServer.Text = "Session has expired.";
			_lblWarnServer.ForeColor = Color.Red;
			_cmbServer.Focus();
		}

		/// <summary>
		/// Method that is called when the client successfully connects to the server.
		/// </summary>
		public void serverConnected() {
			bool updated = false;
			string pass;

			// Remove the warning text.
			_lblWarnServer.Text = "";

			_btnLogin.Enabled = true;

			// Only save the password if the user wants to.
			if(_chkSavePassword.Checked) {
				pass = _itxtPassword.Value;
			} else {
				pass = "";
			}


			// Check to see if this server already exists in our list.  If so, update the username and password.
			for(int i = 0; i < server_list.Count; i++) {
				if(server_list[i].url == _cmbServer.Text) {
					server_list[i].name = connector.server_info.server_name;
					server_list[i].username = _itxtUsername.Value;
					server_list[i].password = pass;
					server_list[i].times_connected = server_list[i].times_connected++;
					server_list[i].save_pass = _chkSavePassword.Checked;
					updated = true;
				}
			}

			// Need to create new entry for new server, if it did not exist.
			if(!updated) {
				server_list.Add(new DC_Server {
					name = connector.server_info.server_name,
					url = _cmbServer.Text,
					username = _itxtUsername.Value,
					password = pass,
					times_connected = 1,
					save_pass = _chkSavePassword.Checked
				});
			}

			Client.config.set("frmlogin.last_server", connector.server_info.server_name);
			Client.config.set("frmlogin.servers_list", server_list);
			Client.config.save();

			// Show the logout function on the menu bar.
			_cmiLogout.Visible = true;
			_cmiLogin.Visible = false;
			_cmiLoggedSeparator.Visible = true;
			_cmiManageFiles.Visible = true;
			_cmiUploadFiles.Visible = true;
			_cmiUploadCropScreenshot.Visible = true;
			_cmiUploadScreenshot.Visible = true;
			_cmiBrowseToServer.Visible = true;

			// Hide the login window until we need to login again.
			if(this.WindowState == FormWindowState.Normal) this.Hide();

			ping_timer.Enabled = true;

			// Check to see if the quick upload form has already been loaded, if not, then create it, otherwise use the existing form.
			if(Client.form_QuickUpload != null) {
				Client.form_QuickUpload.Show();

			} else {
				new frmQuickUpload().Show();
			}
		}

		public void loggedOut() {
			if(Client.form_QuickUpload != null)
				Client.form_QuickUpload.Hide();

			if(Client.form_Manage != null)
				Client.form_Manage.Close();

			ping_timer.Enabled = false;

			_cmiLogout.Visible = !true;
			_cmiLogin.Visible = !false;
			_cmiLoggedSeparator.Visible = !true;
			_cmiManageFiles.Visible = !true;
			_cmiUploadFiles.Visible = !true;
			_cmiUploadCropScreenshot.Visible = !true;
			_cmiUploadScreenshot.Visible = !true;
			_cmiBrowseToServer.Visible = !true;

			this.Show();
		}

		public void serverOnline() {
			_lblWarnServer.Text = "Server is online.";
			_lblWarnServer.ForeColor = Color.DarkGreen;

			string im = _picLogo.ImageLocation;

			// If the server has a logo, load it.
			if(connector.server_info.server_logo != null) {
				loadLogo(connector.server_info.server_logo);

			} else if(_picLogo.ImageLocation != null) {
				// Otherwise, use use the default logo.
				_picLogo.Image = DtxUpload.Properties.Resources.LoginLogoRev2;

			} else {
				_picLogo.Image = DtxUpload.Properties.Resources.LoginLogoRev2;
			}


		}

		private void loadLogo(string url) {
			if(!loadLogoWorker.IsBusy) {
				tween_image_height.start(0);
				loadLogoWorker.RunWorkerAsync(url);
			}
		}

		void loadLogoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			tween_image_height.start(70);

			if(e.Result != null) {
				_picLogo.Image = (Image)e.Result;
			}
		}

		void loadLogoWorker_DoWork(object sender, DoWorkEventArgs e) {
			try {
				WebRequest request = HttpWebRequest.Create((string)e.Argument);
				WebResponse response = request.GetResponse();
				int image_length = int.Parse(response.Headers.Get("Content-Length"));

				// If the image is over 100k, we do not care to use it.  Reduce the image size!
				if(image_length > 100000) {
					response.Close();
					return;
				}

				// Make sure this stream is closed...
				e.Result = Image.FromStream(response.GetResponseStream());

			} catch { }
		}

		void _picLogo_LoadCompleted(object sender, AsyncCompletedEventArgs e) {
			tween_image_height.start(70);
		}

		private void _btnSettings_Click(object sender, EventArgs e) {
			new frmSettings().ShowDialog();
		}

		public void displayNotification(string title, string text, bool error) {
			_notifyIcon.ShowBalloonTip(3, title, text, (error) ? ToolTipIcon.Error : ToolTipIcon.Info);
		}

		#region ContextMenu items and events.

		private void _cmiManageFiles_Click(object sender, EventArgs e) {
			if (Client.form_Manage == null) {
				new frmManage().Show();
			} else {
				Client.form_Manage.Show();
			}
		}

		private void _cmiBrowseToServer_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(Client.server_info.server_url);
		}


		private void _cmiUploadFiles_Click(object sender, EventArgs e) {
			Client.form_QuickUpload.Show();
			Client.form_QuickUpload.TopMost = true;
			Client.form_QuickUpload.TopMost = false;
		}

		private void _cmiUploadCropScreenshot_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				new frmCropScreen().Show();
			}
		}

		private void _cmiUploadScreenshot_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				Client.form_QuickUpload.uploadScreenshot(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			}
		}


		private void _cmiLogin_Click(object sender, EventArgs e) {
			this.Show();
		}

		private void _cmiLogout_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				connector.disconnect();
			} else {
				Client.form_QuickUpload.Hide();
				Client.form_Login.Show();
			}
		}

		private void _cmiExit_Click(object sender, EventArgs e) {
			this.Close();
		}

		#endregion

		#region Settings on contextmenu


		// Load all the settings from the config file.
		private void _contextMenu_Popup(object sender, EventArgs e) {
			_cmiSettingsConfirmUpload.Checked = Client.config.get<bool>("frmquickupload.show_clipboard_confirmation");
			_cmiSettingsUploadCopy.Checked = Client.config.get<bool>("frmquickupload.copy_upload_clipboard");
		}

		private void _cmiSettingsConfirmUpload_Click(object sender, EventArgs e) {
			_cmiSettingsConfirmUpload.Checked = !_cmiSettingsConfirmUpload.Checked;
			Client.config.set("frmquickupload.show_clipboard_confirmation", _cmiSettingsConfirmUpload.Checked);
			Client.config.save();
		}

		private void _cmiSettingsUploadCopy_Click(object sender, EventArgs e) {
			_cmiSettingsUploadCopy.Checked = !_cmiSettingsUploadCopy.Checked;
			Client.config.set("frmquickupload.copy_upload_clipboard", _cmiSettingsUploadCopy.Checked);
			Client.config.save();
		}

		#endregion

		private void _cmiAbout_Click(object sender, EventArgs e) {
			new dtxCore.Forms.frmAbout(Assembly.GetExecutingAssembly(), Properties.Resources.License_Dtronix_Upload, Properties.Resources.AboutLogoRev0).ShowDialog();
		}

		private void frmLogin_FormClosing(object sender, FormClosingEventArgs e) {
			// Make sure we unmount the drive if it is mounted.
			if(Client.drive_mount_thread != null) {
				Client.drive_mount_thread.Abort();
			}
		}

		private void frmLogin_FormClosed(object sender, FormClosedEventArgs e) {
			// Make sure we unmount the drive if it is mounted.
			if(Client.drive_mount_thread != null) {
				Client.drive_mount_thread.Abort();
			}
		}

		private void _miSettingsPanel_Click(object sender, EventArgs e) {
			new frmSettings().ShowDialog();
		}



	}
}
