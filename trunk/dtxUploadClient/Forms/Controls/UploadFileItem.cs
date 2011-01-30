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
		private ServerConnector connector = new ServerConnector();

		public UploadFileItem(DC_FileInformation file_info) {
			this.file_info = file_info;
			connector.uploadProgressChanged += new UploadProgressChangedEventHandler(uploadProgress);
			connector.uploadFileCompleted += new UploadFileCompletedEventHandler(uploadCompleted);

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


		private void uploadProgress(object sender, UploadProgressChangedEventArgs e) {
			_picPreview.Image = Properties.Resources.icon_24_em_up;
			_lblStatus.Text = "(" + Core.Utilities.formattedSize(e.BytesSent) + "/" + Core.Utilities.formattedSize(e.TotalBytesToSend) + ")";
			_barProgress.Maximum = (int)e.TotalBytesToSend;
			_barProgress.Value = (int)e.BytesSent;
		}


		private void uploadCompleted(object sender, UploadFileCompletedEventArgs e) {
			if(file_info.delete_after_upload) {
				// Delete the file if it was a temp file created by us.
				System.IO.File.Delete(file_info.local_file_location);
			}
			if(e.Cancelled) {
				_barProgress.Value = 1;
				_barProgress.Maximum = 1;
				_lblStatus.Text = "Canceled";
				_picPreview.Image = Properties.Resources.icon_24_em_cross;
				_btnCancel.Visible = false;
				_btnCopyUrl.Visible = false;
				file_info.status = 6;

				return;
			}
			string[] parsed_data = connector.parseServerData(UTF8Encoding.UTF8.GetString(e.Result));

			if(parsed_data[0] == "upload_successful") {
				_lblStatus.Text = "";
				_barProgress.Visible = false;
				_btnCopyUrl.Visible = true;
				_picPreview.Image = Properties.Resources.icon_24_em_check;
				_btnCancel.Text = "Open";
				file_info.status = 2;

				JsonReader jr = new JsonReader(parsed_data[1]);
				DC_FileInformation info = jr.Deserialize<DC_FileInformation>();

				file_info.url_id = info.url_id;
				file_info.is_visible = info.is_visible;
				file_info.url = connector.server_info.upload_base_url + info.url_id;

			} else {
				_barProgress.Visible = false;
				_btnCancel.Visible = false;
				_btnCopyUrl.Visible = false;
				_picPreview.Image = Properties.Resources.icon_24_em_cross;
				file_info.status = 6;
				

				if(parsed_data[0] == "upload_failed_db_error") {
					_lblStatus.Text = "Server DB Error";

				} else if(parsed_data[0] == "upload_failed_db_error") {
					_lblStatus.Text = "Server File Error";
				}
			}
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