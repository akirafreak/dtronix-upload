using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Core.Json;

namespace dtxUpload {
	public partial class UploadFileItem : UserControl {

		public DC_FileInformation file_info;
		private ServerConnector connector;

		public UploadFileItem(DC_FileInformation file_info) {
			connector = new ServerConnector(this);

			this.file_info = file_info;
			InitializeComponent();

			file_info.status = 6;
			_barProgress.Value = 0;
			_lblFileName.Text = file_info.file_name;
			_lblStatus.Text = "[WAITING] File Size: " + file_info.file_size.ToString();
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			if(_btnCancel.Text == "Cancel") {
				// The button is it's default cancel action.
				cancelUpload();

			} else {
				// The button is the open action.
				System.Diagnostics.Process.Start(file_info.url);
			}
		}


		public void uploadProgress(UploadProgressChangedEventArgs e) {
			_picPreview.Image = Properties.Resources.icon_24_em_up;
			_lblStatus.Text = "(" + Core.Utilities.formattedSize(e.BytesSent) + "/" + Core.Utilities.formattedSize(e.TotalBytesToSend) + ")";
			_barProgress.Maximum = (int)e.TotalBytesToSend;
			_barProgress.Value = (int)e.BytesSent;
		}


		/// <summary>
		/// This executes every time the upload completed successful or otherwise.
		/// </summary>
		public void uploadPostCompleted() {
			if(file_info.delete_after_upload) {
				// Delete the file if it was a temp file created by us.
				System.IO.File.Delete(file_info.local_file_location);
			}
		}

		public void uploadCanceled() {
			_barProgress.Value = 1;
			_barProgress.Maximum = 1;
			_lblStatus.Text = "Canceled";
			_picPreview.Image = Properties.Resources.icon_24_em_cross;
			_btnCancel.Visible = false;
			_btnCopyUrl.Visible = false;
			file_info.status = 6;
		}


		public void uploadSuccessful(string server_data) {
			_lblStatus.Text = "";
			_barProgress.Visible = false;
			_btnCopyUrl.Visible = true;
			_picPreview.Image = Properties.Resources.icon_24_em_check;
			_btnCancel.Text = "Open";
			file_info.status = 2;

			JsonReader jr = new JsonReader(server_data);
			DC_FileInformation info = jr.Deserialize<DC_FileInformation>();

			file_info.url_id = info.url_id;
			file_info.is_visible = info.is_visible;
			file_info.url = connector.server_info.upload_base_url + info.url_id;

			// Automatically copy the url to clipboard if the user so desires.
			if(Client.config.get<bool>("frmquickupload.copy_upload_clipboard")) {
				Clipboard.SetText(file_info.url, TextDataFormat.UnicodeText);
			}
		}


		private void uploadFailed() {
			_barProgress.Visible = false;
			_btnCancel.Visible = false;
			_btnCopyUrl.Visible = false;
			_picPreview.Image = Properties.Resources.icon_24_em_cross;
			file_info.status = 6;
		}

		public void uploadFailedDB() {
			uploadFailed();
			_lblStatus.Text = "Server DB Error";
		}

		public void uploadFailedFile() {
			uploadFailed();
			_lblStatus.Text = "Server File Error";
		}

		public void uploadNotConnected() {
			uploadFailed();
			_lblStatus.Text = "Not Connected";
		}


		public void startUpload() {
			file_info.status = 1;
			connector.uploadFile(file_info.local_file_location);
		}

		public void cancelUpload() {
			if(file_info.status == 1 || file_info.status == 5) {
				connector.cancelActions();
			}
		}

		private void _btnCopyUrl_Click(object sender, EventArgs e) {
			Clipboard.SetText(file_info.url);
		}

	}
}