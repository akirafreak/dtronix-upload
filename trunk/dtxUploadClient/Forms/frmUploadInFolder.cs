using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using dtxCore;

namespace dtxUpload {
	public partial class frmUploadInFolder : Form {
		private int image_count = 0;

		public frmUploadInFolder(string[] initial_files) {
			
			InitializeComponent();

			// Check to see if there are any files that were passed to the form.
			if(initial_files == null)
				return;

			foreach(string file in initial_files) {
				addFile(file);
			}
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void _btnRandomPass_Click(object sender, EventArgs e) {
			_txtPassword.Text = Utilities.createHash(8, false);
		}

		private void _lstFiles_KeyPress(object sender, KeyPressEventArgs e) {
			foreach(ListViewItem item in _lstFiles.SelectedItems) {
				item.Remove();
			}
		}

		/// <summary>
		/// Adds the file to the queue to upload.
		/// </summary>
		/// <param name="file">File location.</param>
		private void addFile(string file) {
			Icon file_icon = Icon.ExtractAssociatedIcon(file);
			_imlFiles.Images.Add(image_count.ToString(), file_icon.ToBitmap());

			_lstFiles.Items.Add(Path.GetFileName(file), Path.GetFileName(file), image_count.ToString());
			image_count++;
		}

		private void _lstFiles_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.All;
		}

		private void _lstFiles_DragDrop(object sender, DragEventArgs e) {
			string[] drop_files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in drop_files) {
				addFile(file);
			}
		}
	}
}
