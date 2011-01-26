using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace dtxUpload {
	public partial class frmQuickUpload : Form {

		public frmQuickUpload() {
			Client.form_QuickUpload = this;
			InitializeComponent();
		}

		private void frmQuickUpload_Load(object sender, EventArgs e) {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 13, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 13);
		}

		private UploadFileItem addUploadItem(DC_FileInformation upload_info) {
			//SuspendLayout();
			UploadFileItem control = new UploadFileItem(upload_info) {
				Dock = DockStyle.Top,
				Location = new Point(0, 50),
				MaximumSize = new Size(250, 50),
				MinimumSize = new Size(250, 36),
				Name = "_uploadingItems",
				Size = new Size(250, 50),
				TabIndex = 1

			};
			_panFileItemContainer.Controls.Add(control);
			//ResumeLayout(false);

			return control;
		}

		private void uploadFile(string file_location) {
			FileInfo fi = new FileInfo(file_location);
			DC_FileInformation file_info = new DC_FileInformation(){
				status = 5,
				file_name = fi.Name,
				file_size = fi.Length,
				local_file_location = file_location,
			};

			UploadFileItem control = addUploadItem(file_info);

			control.startUpload();
		}


		private void frmQuickUpload_FormClosing(object sender, FormClosingEventArgs e) {
			e.Cancel = true;
			this.Hide();
		}

		private void _panFileItemContainer_DragEnter(object sender, DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
				e.Effect = DragDropEffects.All;
			}
		}

		private void _panFileItemContainer_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in files) {
				uploadFile(file);
			}

		}

		private void _btnUploadClipboard_Click(object sender, EventArgs e) {

		}

		private void _btnClearList_Click(object sender, EventArgs e) {

		}
	}
}
