﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Core;
using Core.Json;
using System.Net;

namespace dtxUpload {
	public partial class frmLogin : Form {

		private ServerConnector connector = new ServerConnector();
		private List<DC_Server> server_list;

		private Tween tween_image_height;
		private Tween tween_form_width;
		private Tween tween_form_position = new Tween(Core.EasingEquations.expoEaseOut);

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
#if DEBUG
			new frmConnector().Show();
#endif
			Client.form_Login = this;
			InitializeComponent();

			tween_form_width = new Tween(this, "Width", Core.EasingEquations.expoEaseOut);
			tween_image_height = new Tween(_picLogo, "Height", Core.EasingEquations.expoEaseOut);
			_picLogo.LoadCompleted += new AsyncCompletedEventHandler(_picLogo_LoadCompleted);

			// Threading
			loadLogoWorker.DoWork += new DoWorkEventHandler(loadLogoWorker_DoWork);
			loadLogoWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadLogoWorker_RunWorkerCompleted);

			// Load configurations.
			_cmbScreenshotFormat.SelectedIndex = Client.config.get<int>("frmlogin.screenshot_upload_format", 0);
			_cmbScreenshotQuality.Text = Client.config.get<string>("frmlogin.screenshot_jpeg_quality", "90");

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
					_itxtPassword.Value = Core.Utilities.md5Sum(_itxtPassword.Value);
				}

				_btnLogin.Enabled = false;
				connector.user_info.client_username = _itxtUsername.Value;
				connector.user_info.client_password = _itxtPassword.Value;
				connector.connect();
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
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

		private void _cmbServer_SelectedValueChanged(object sender, EventArgs e) {

		}

		private void _cmbServer_Leave(object sender, EventArgs e) {
			if(Client.server_info.server_url != null && !Client.server_info.server_url.Contains(_cmbServer.Text)) {
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
				invalidServer();
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
			_itxtUsername.SetInfo("Username is not registerd on server.", Color.Red);
			_itxtUsername.Focus();
		}

		public void invalidServer() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();

			_btnLogin.Enabled = true;
			_lblWarnServer.Text = "Server is not responding.";
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
		/// Method that is called when the client successfully connectes to the server.
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


			// Check to see if this server already exsits in our list.  If so, update the username and password.
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
			_logoutToolStripMenuItem.Visible = true;
			_loginToolStripMenuItem.Visible = false;
			_toolStripSearator1.Visible = true;
			_manageFilesToolStripMenuItem.Visible = true;
			_uploadFilesToolStripMenuItem.Visible = true;
			_uploadCropScreenshotToolStripMenuItem.Visible = true;
			_uploadScreenshotToolStripMenuItem.Visible = true;

			// Hide the login window untill we need to login again.
			if(this.WindowState == FormWindowState.Normal) this.Hide();

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

			//Client.form_Login.Activate();
			_logoutToolStripMenuItem.Visible = false;
			_loginToolStripMenuItem.Visible = true;
			_toolStripSearator1.Visible = false;
			_manageFilesToolStripMenuItem.Visible = false;
			_uploadFilesToolStripMenuItem.Visible = false;
			_uploadCropScreenshotToolStripMenuItem.Visible = false;
			_uploadScreenshotToolStripMenuItem.Visible = false;


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
				// Otherwise, juse use the default logo.
				_picLogo.Image = dtxUpload.Properties.Resources.LoginLogoRev2;

			} else {
				_picLogo.Image = dtxUpload.Properties.Resources.LoginLogoRev2;
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


		private int original_form_width = -1;

		private void _btnSettings_Click(object sender, EventArgs e) {
			int width_new = 500;
			int width_current = Width;
			int original_point = Location.X;

			if(original_form_width == -1) {
				// Form is small.  Expand it!

				original_form_width = width_current;

				tween_form_width.start(width_new);
				tween_form_position.start(original_point, original_point - 250, delegate(int current) {
					Location = new Point(current, Location.Y);
				});
			} else {
				// Form is already expanded.  Shrink it!
				original_form_width = -1;

				tween_form_width.start(250);
				tween_form_position.start(original_point, original_point + 250, delegate(int current) {
					Location = new Point(current, Location.Y);
				});
				
			}
			
		}

		private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				connector.disconnect();
			}
			

		}

		private void _loginToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Show();
		}

		private void _settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				// TODO: Open settings for everything else.  Store settings on server?
			} else {
				//this.Show();
				//_btnSettings_Click(sender, e);
			}
		}

		private void _btnConfigDone_Click(object sender, EventArgs e) {
			_btnSettings_Click(sender, e);
		}

		private void _manageFilesToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(Client.server_info.server_url);
		}

		private void _uploadFilesToolStripMenuItem_Click(object sender, EventArgs e) {
			Client.form_QuickUpload.Show();
			Client.form_QuickUpload.TopMost = true;
			Client.form_QuickUpload.TopMost = false;
		}

		#region Settings on contextmenu

		// Load all the settings from the config file.
		private void _settingsToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
			_confirmClipboardUploadToolStripMenuItem.Checked = Client.config.get<bool>("frmquickupload.show_clipboard_confirmation");
			_copyLastUploadToClipboardToolStripMenuItem.Checked = Client.config.get<bool>("frmquickupload.copy_upload_clipboard");
		}

		private void _confirmClipboardUploadToolStripMenuItem_Click(object sender, EventArgs e) {
			_confirmClipboardUploadToolStripMenuItem.Checked = !_confirmClipboardUploadToolStripMenuItem.Checked;
			Client.config.set("frmquickupload.show_clipboard_confirmation", _confirmClipboardUploadToolStripMenuItem.Checked);
			Client.config.save();
		}

		private void _copyLastUploadToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
			_copyLastUploadToClipboardToolStripMenuItem.Checked = !_copyLastUploadToClipboardToolStripMenuItem.Checked;
			Client.config.set("frmquickupload.copy_upload_clipboard", _copyLastUploadToClipboardToolStripMenuItem.Checked);
			Client.config.save();
		}

		#endregion

		private void _cmbScreenshotFormat_SelectedIndexChanged(object sender, EventArgs e) {
			switch(_cmbScreenshotFormat.SelectedIndex) {
				case 0: // Auto detect
					_cmbScreenshotQuality.Enabled = true;
					break;

				case 1: // JPEG
					_cmbScreenshotQuality.Enabled = true;
					break;

				case 2: // JPEG
					_cmbScreenshotQuality.Enabled = false;
					break;
			}

			Client.config.set("frmlogin.screenshot_upload_format", _cmbScreenshotFormat.SelectedIndex);
			Client.config.save();
		}

		private void _cmbScreenshotQuality_SelectedIndexChanged(object sender, EventArgs e) {
			Client.config.set("frmlogin.screenshot_jpeg_quality", _cmbScreenshotQuality.Text);
			Client.config.save();
		}

		private void _uploadScreenshotToolStripMenuItem_Click(object sender, EventArgs e) {
			if(Client.server_info.is_connected) {
				Client.form_QuickUpload.uploadScreenshot(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			}
		}

		private void _uploadCropScreenshotToolStripMenuItem_Click(object sender, EventArgs e) {
			new frmCropScreen().Show();
		}
	}
}