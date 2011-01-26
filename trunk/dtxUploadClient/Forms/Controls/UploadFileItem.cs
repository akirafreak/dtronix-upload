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
			_barProgress.Value = 0;
			_lblFileName.Text = file_info.file_name;
			_lblStatus.Text = "[WAITING] File Size: " + file_info.file_size.ToString();

			//foreach(Control cont in this.Controls) {
			//    cont.Click += new EventHandler(UploadFileItem_Enter);
			//    cont.Leave += new EventHandler(UploadFileItem_Leave);
			//}
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			if(_btnCancel.Text == "Cancel") {
				// The button is it's default cancel action.

				if(file_info == null) {
					MessageBox.Show("File information for this item has not been set.");
				} else {
					connector.cancelActions();
					_barProgress.Value = 0;
					_lblStatus.Text = "Upload was canceled.";
					_picPreview.Image = Properties.Resources.icon_24_em_cross;
					_btnCancel.Visible = false;
					_btnCopyUrl.Visible = false;
				}
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
			string[] parsed_data = connector.parseServerData(UTF8Encoding.UTF8.GetString(e.Result));

			if(parsed_data[0] == "upload_successful") {
				_lblStatus.Text = "";
				_barProgress.Visible = false;
				file_info.status = 2;
				_btnCopyUrl.Visible = true;
				_picPreview.Image = Properties.Resources.icon_24_em_check;
				_btnCancel.Text = "Open";

				JsonReader jr = new JsonReader(parsed_data[1]);
				DC_FileInformation info = jr.Deserialize<DC_FileInformation>();

				file_info.url_id = info.url_id;
				file_info.is_visible = info.is_visible;
				file_info.url = connector.server_info.upload_base_url + info.url_id;

			} else {
				_barProgress.Visible = false;
				_btnCancel.Visible = false;
				_btnCopyUrl.Visible = false;
				file_info.status = 6;
				_picPreview.Image = Properties.Resources.icon_24_em_cross;
				

				if(parsed_data[0] == "upload_failed_db_error") {
					_lblStatus.Text = "Server DB Error";

				} else if(parsed_data[0] == "upload_failed_db_error") {
					_lblStatus.Text = "Server File Error";
				}
			}
		}

		public void startUpload() {
			connector.uploadFile(file_info.local_file_location);
		}

	}
}