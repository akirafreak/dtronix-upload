using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Core;
using Core.Json;

namespace dtxUpload {
	public partial class frmLogin : Form {

		private ServerConnector connector = new ServerConnector();
		private DC_Server[] server_list;

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

			frmLogin_Activated(new object(), new EventArgs());
		}


		private void frmLogin_Load(object sender, EventArgs e) {
			server_list = Config.get<DC_Server[]>("frmlogin.servers_list");
			foreach(DC_Server server in server_list) {
				_cmbServer.Items.Add(server.server_url);
			}

			_cmbServer.SelectedIndex = 0;
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


		private void frmLogin_Deactivate(object sender, EventArgs e) {
			this.Hide();
		}

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

				} else {
					this.Show();
				}
			}
		}

		//private void _cmbServer_TextChanged(object sender, EventArgs e) {
		//    //try {
		//        // Test the uri for validity.


		//    //} catch {
		//    //    _lblWarnServer.ForeColor = Color.Red;
		//    //    _lblWarnServer.Text = "Server url is invalid.";
		//    //}
		//}

		public void invalidPassword() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();
			_itxtPassword.SetInfo("Password is invalid.", Color.Red);
			_itxtPassword.Focus();
		}

		public void invalidUsername() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();
				
			_itxtUsername.SetInfo("Username is not registerd on server.", Color.Red);
			_itxtUsername.Focus();
		}

		public void invalidServer() {
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();
			_lblWarnServer.Text = "Server is not responding.";
			_lblWarnServer.ForeColor = Color.Red;
			_cmbServer.Focus();
		}

		public void sessionExpired() {
			Client.form_QuickUpload.Hide();
			if(this.WindowState != FormWindowState.Normal) this.ShowDialog();
			_lblWarnServer.Text = "Session has expired.";
			_lblWarnServer.ForeColor = Color.Red;
			_cmbServer.Focus();
		}

		public void serverConnected() {
			bool updated = false;
			string pass;

			// Only save the password if the user wants to.
			if(_chkSavePassword.Checked) {
				pass = _itxtPassword.Value;
			} else {
				pass = "";
			}

			// Check to see if this server already exsits in our list.  If so, update the username and password.
			for(int i = 0; i < server_list.Length; i++) {
				if(server_list[i].server_url == _cmbServer.Text) {
					server_list[i].last_username = _itxtUsername.Value;
					server_list[i].last_password = pass;
					updated = true;
				}
			}

			// Need to create new entry for new server if it did not exist.
			if(!updated) {
				Array.Resize<DC_Server>(ref server_list, server_list.Length + 1);
				server_list[server_list.Length - 1] = new DC_Server {
					server_url = _cmbServer.Text,
					last_username = _itxtUsername.Value,
					last_password = pass,
					times_connected = 1
				};
			}

			Config.set("frmlogin.servers_list", server_list);
			Config.save();

			_lblWarnServer.Text = "";
			if(this.WindowState == FormWindowState.Normal) this.Hide();

			if(Client.form_QuickUpload != null) {
				Client.form_QuickUpload.Show();
			} else {
				new frmQuickUpload().Show();
			}
		}

		public void serverOnline() {
			_lblWarnServer.Text = "Server is online.";
			_lblWarnServer.ForeColor = Color.DarkGreen;
		}

		private void _cmbServer_SelectedValueChanged(object sender, EventArgs e) {

		}

		private void _cmbServer_Leave(object sender, EventArgs e) {
			_cmbServer_SelectedIndexChanged(sender, e);
		}

		private void _cmbServer_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				new Uri(_cmbServer.Text);
			} catch {
				invalidServer();
				return;
			}
			_lblWarnServer.Text = "";
			connector.server_info.server_url = _cmbServer.Text;
			connector.getServerInfo();

			for(int i = 0; i < server_list.Length; i++) {
				if(server_list[i].server_url == _cmbServer.Text) {
					_itxtUsername.Value = server_list[i].last_username;
					_itxtPassword.Value = server_list[i].last_password;
				}
			}
		}







	}
}
