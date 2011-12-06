using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using dtxCore;
using Ionic.Zip;
using System.Threading;

namespace DtxUpload {
	public partial class frmUploadZip : Form {
		private int image_count = 0;
		private bool generated_pass = false;
		private BackgroundWorker zip_worker = new BackgroundWorker();
		private string zip_path;

		public frmUploadZip(string[] initial_files) {
			
			InitializeComponent();

			// Check to see if there are any files that were passed to the form.
			if(initial_files == null)
				return;

			foreach(string file in initial_files) {
				addItem(file);
			}

			//recalculateApproxSize();
			//loaded_form = true;
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void _btnRandomPass_Click(object sender, EventArgs e) {
			_txtPassword.Text = Utilities.createHash(8, false);
			generated_pass = true;

		}

		/// <summary>
		/// Adds the file to the queue to upload.
		/// </summary>
		/// <param name="file">File location.</param>
		private void addItem(string file) {
			Bitmap icon;

			if(File.Exists(file)) { // Determine if we are dealing with a file or directory.
				icon = Icon.ExtractAssociatedIcon(file).ToBitmap(); 
			} else {
				icon = Properties.Resources.square_green_16_ns;
			}

			_imlFiles.Images.Add(image_count.ToString(), icon);

			ListViewItem new_file = _lstFiles.Items.Add(Path.GetFileName(file), Path.GetFileName(file), image_count.ToString());
			new_file.Tag = file;
			image_count++;
		}

		private void _lstFiles_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.All;
		}

		private void _lstFiles_DragDrop(object sender, DragEventArgs e) {
			string[] drop_files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in drop_files) {
				addItem(file);
			}
		}

		private void _btnUpload_Click(object sender, EventArgs e) {
			if(generated_pass) {
				DialogResult result = MessageBox.Show("You chose the generate a random password.  Ensure you have copied this because you will not be able to access the files inside the zip without it.  Continue to upload?", "Password", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
				if(result == DialogResult.Cancel)
					return;
			}

			if(Directory.Exists(Client.directory_temp) == false)
				Directory.CreateDirectory(Client.directory_temp);

			string zip_name = _txtZipName.Text.Trim();
			zip_path = Client.directory_temp + Path.DirectorySeparatorChar + zip_name + ".zip";

			// Validation.
			if(zip_name == ""){
				MessageBox.Show("The name field must not be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				_txtZipName.Focus();
				return;
			}

			if(zip_name.Length > 32 || zip_name.Length == 0) {
				MessageBox.Show("The file name has to be shorter than 32 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				_txtZipName.Focus();
				return;
			}

			_lblStatus.Text = "Zipping files...";
			List<string> files = new List<string>();

			foreach(ListViewItem item in _lstFiles.Items) {
				files.Add(item.Tag as string);
			}

			Thread t = new Thread(zip_worker_DoWork);

			t.Start(files.ToArray());

			//zip_worker.RunWorkerAsync(files.ToArray());
			_btnUpload.Enabled = false;
			_picLoader.Visible = true;
		}


		private void zip_worker_DoWork(object start) {
			string[] files = start as string[];
			if(files == null) { // If we are not working with any files, quit.
				return;
			}

			ZipFile zip = new ZipFile(zip_path);
			zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Default;

			// If the user specified a password, add it to the zip.
			if(_txtPassword.Text != "") {
				zip.Encryption = EncryptionAlgorithm.WinZipAes256;
				zip.Password = _txtPassword.Text;
			}

			zip.SaveProgress += new EventHandler<SaveProgressEventArgs>(zip_SaveProgress);

			// TODO: If the user wants a split archive, give him one.
			//if(split_length != 0)
			//	zip.MaxOutputSegmentSize = Convert.ToInt32(split_length);


			foreach(string file in files) {
				try {
					if(Directory.Exists(file)) {
						zip.AddDirectory(file, Path.GetFileNameWithoutExtension(file));
					} else {
						zip.AddFile(file, "");
					}
				} catch(ArgumentException) {
				}
			}

			zip.Save();
		}

		private void zip_SaveProgress(object sender, SaveProgressEventArgs e) {
			if(e.EventType == ZipProgressEventType.Saving_AfterWriteEntry) {
				_lblStatus.Invoke((MethodInvoker)delegate {
					_lblStatus.Text = "Zipping... (" + e.EntriesSaved.ToString() + "/" + e.EntriesTotal.ToString() + ") " + e.CurrentEntry.FileName;
				});
			} else if(e.EventType == ZipProgressEventType.Error_Saving) {
				_lblStatus.Invoke((MethodInvoker)delegate {
					_lblStatus.Text = "Error occurred while zipping.  Please try again.";
				});

			} else if(e.EventType == ZipProgressEventType.Saving_Completed) {
				Client.form_QuickUpload.Invoke((MethodInvoker)delegate {
					_lblStatus.Text = "Completed.";
					Client.form_QuickUpload.addUploadItem(new DC_FileInformation() {
						local_file_location = zip_path,
						delete_after_upload = true,
						file_name = Path.GetFileName(zip_path),
						file_size = new FileInfo(zip_path).Length
					}).startUpload();

					this.Close();
				});
			}
		}

		private void frmUploadZip_Load(object sender, EventArgs e) {
			_txtZipName.Focus();
		}

		private void _lstFiles_KeyDown(object sender, KeyEventArgs e){
			if(e.KeyCode == Keys.Delete){
				foreach(ListViewItem item in _lstFiles.SelectedItems){
					item.Remove();
				}
			}
		}

	}
}
