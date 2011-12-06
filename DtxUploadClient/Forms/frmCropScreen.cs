using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dtxCore;

namespace DtxUpload {
	public partial class frmCropScreen : Form {
		public frmCropScreen() {
			win_hooks.OnMouseUp += new MouseEventHandler(win_hooks_OnMouseUp);
			win_hooks.KeyUp += new KeyEventHandler(win_hooks_KeyUp);
			win_hooks.OnMouseDown += new MouseEventHandler(win_hooks_OnMouseDown);
			InitializeComponent();
			TopMost = true;
		}

		private Timer mouse_timer = new Timer();
		private Point starting_point;
		private WindowsHooks win_hooks = new WindowsHooks(true, true);

		private void frmCropScreen_Load(object sender, EventArgs e) {
			mouse_timer.Interval = 10;
			mouse_timer.Tick += new EventHandler(mouse_timer_Tick);
			Opacity = 0.3;

			Location = new Point(0, 0);
			Height = Screen.PrimaryScreen.Bounds.Height;
			Width = Screen.PrimaryScreen.Bounds.Width;

		}

		void mouse_timer_Tick(object sender, EventArgs e) {
			Point cursor_pos = Cursor.Position;
			Point form_pos = Location;
			int width, height;
			int form_x = form_pos.X;
			int form_y = form_pos.Y;
			bool lower_right = true;

			if(cursor_pos.X > starting_point.X) {
				width = cursor_pos.X - starting_point.X;
			} else {
				lower_right = false;
				form_x = cursor_pos.X;
				width = starting_point.X - cursor_pos.X;
			}

			if(cursor_pos.Y > starting_point.Y) {
				height = cursor_pos.Y - starting_point.Y;
			} else {
				lower_right = false;
				form_y = cursor_pos.Y;
				height = starting_point.Y - cursor_pos.Y;
			}

			if(lower_right) {
				this.Width = width;
				this.Height = height;
				this.Location = new Point(starting_point.X, starting_point.Y);
			} else {
				this.Width = width;
				this.Height = height;
				this.Location = new Point(form_x, form_y);
			}
		}

		void win_hooks_KeyUp(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Escape) {
				mouse_timer.Stop();
				this.Close();
			}
		}

		void win_hooks_OnMouseDown(object sender, MouseEventArgs e) {
			if(e.Button == System.Windows.Forms.MouseButtons.Left) {
				starting_point = Cursor.Position;
				mouse_timer.Start();
				this.Focus();
			}
		}

		void win_hooks_OnMouseUp(object sender, MouseEventArgs e) {
			mouse_timer.Stop();

			// Hide the form to prevent it from being screenshotted too.
			this.Visible = false;

			Client.form_QuickUpload.uploadScreenshot(Location.X, Location.Y, Width, Height);
			this.Close();

		}

		private void frmCropScreen_FormClosing(object sender, FormClosingEventArgs e) {
			win_hooks.Stop();
		}
	}
}

