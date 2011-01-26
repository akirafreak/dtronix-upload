using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dtxUpload.Classes;

namespace dtxUpload {
	public partial class frmDropBox : Form {
		//UploadingQueue file_queue = new UploadingQueue();

		public frmDropBox() {
			InitializeComponent();
			Client.form_DropBox = this;
		}

		private void frmDropBox_DragDrop(object sender, DragEventArgs e) {
			Array data = (Array)e.Data.GetData(DataFormats.FileDrop);

			//file_queue.addFile(data.GetValue(0).ToString());
		}

		private void frmDropBox_DragEnter(object sender, DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			} else {
				e.Effect = DragDropEffects.None;
			}

		}

		private void frmDropBox_Load(object sender, EventArgs e) {
			ServerConnector connector = new ServerConnector();

			connector.callServerMethod("test", "4321", "1234");
		}

		private void notifyIcon_MouseMove(object sender, MouseEventArgs e) {
			MessageBox.Show("Moved!");
		}
	}
}
