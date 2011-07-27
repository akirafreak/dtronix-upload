using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using dtxCore.Json;

namespace dtxUpload {
	public partial class UploadFileItem : UserControl {

		public DC_FileInformation file_info;
		private ServerConnector connector;
		public MouseEventHandler onEnter;
		private DC_ServerResponse upload_response;

		public UploadFileItem(DC_FileInformation file_info) {
			connector = new ServerConnector();

			connector.upload_progress_changed += uploadProgress;
			connector.upload_completed += uploadCompleted;
			connector.upload_canceled += uploadCanceled;

			this.file_info = file_info;
			InitializeComponent();

			file_info.status = DC_FileInformationStatus.PendingUpload;
			_barProgress.Value = 0;
			_lblFileName.Text = file_info.file_name;
			_lblStatus.Text = "[WAITING] File Size: " + file_info.file_size.ToString();
		}

		/// <summary>
		/// Called after uploadCompleted and uploadCanceled.
		/// </summary>
		private void connectionClosed() {
			if(file_info.delete_after_upload) {
				// Delete the file if it was a temp file created by us.
				System.IO.File.Delete(file_info.local_file_location);
			}
		}

		public void uploadCompleted(DC_ServerResponse response) {
			connectionClosed();
			upload_response = response;

			switch(response.getCalledMethod()) {
				case "upload_failed_exceeded_toal_used_space":
					this.Invoke((MethodInvoker)upload_failed_exceeded_toal_used_space);
					break;

				case "upload_failed_exceeded_file_size":
					this.Invoke((MethodInvoker)upload_failed_exceeded_file_size);
					break;

				case "upload_successful":
					this.Invoke((MethodInvoker)upload_successful);
					break;

				case "upload_failed_db_error":
					this.Invoke((MethodInvoker)upload_failed_db_error);
					break;

				case "upload_failed_could_not_handle_file":
					this.Invoke((MethodInvoker)upload_failed_could_not_handle_file);
					break;

				case "upload_failed_not_connected":
					this.Invoke((MethodInvoker)upload_failed_not_connected);
					break;

				default:
					connector.execServerResponse(response);
					break;
			}
		}

		public void uploadCanceled() {
			this.Invoke((MethodInvoker)delegate {
				connectionClosed();

				_barProgress.Value = 1;
				_barProgress.Maximum = 1;
				_lblStatus.Text = "Canceled";
				_picPreview.BackgroundImage = Properties.Resources.icon_24_em_cross;
				file_info.status = DC_FileInformationStatus.UploadCanceled;
			});
		}

		public void uploadProgress(DC_UploadProgressChangedEventArgs e) {
			this.Invoke((MethodInvoker)delegate {
				_picPreview.BackgroundImage = Properties.Resources.icon_24_em_up;
				_lblStatus.Text = "(" + dtxCore.Utilities.formattedSize(e.bytes_sent) + "/" + dtxCore.Utilities.formattedSize(e.total_bytes_to_send) + ")";
				_barProgress.Maximum = (int)e.total_bytes_to_send;
				_barProgress.Value = (int)e.bytes_sent;
			});

		}

		public void upload_failed_exceeded_toal_used_space() {
			uploadCanceled();
			_lblStatus.Text = "File exceeds alotted space";
		}

		public void upload_failed_exceeded_file_size() {
			uploadCanceled();
			_lblStatus.Text = "File size too large";
		}


		public void upload_successful() {
			_lblStatus.Visible = false;
			_barProgress.Visible = false;
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_check;
			file_info.status = DC_FileInformationStatus.Uploaded;

			JsonReader jr = new JsonReader(upload_response.body);
			DC_FileInformation info = jr.Deserialize<DC_FileInformation>();

			file_info.url_id = info.url_id;
			file_info.url = connector.server_info.upload_base_url + info.url_id;

			// Automatically copy the url to clipboard if the user so desires.
			if(Client.config.get<bool>("frmquickupload.copy_upload_clipboard")) {
				Clipboard.SetText(file_info.url, TextDataFormat.UnicodeText);
			}
		}

		public void upload_failed_db_error() {
			uploadFailed();
			_lblStatus.Text = "Server DB Error";
		}

		public void upload_failed_could_not_handle_file() {
			uploadFailed();
			_lblStatus.Text = "Server File Error";
		}

		public void upload_failed_not_connected() {
			uploadFailed();
			_lblStatus.Text = "Not Connected";
		}


		public void openUrl() {
			System.Diagnostics.Process.Start(file_info.url);
		}

		public string getFullUrl() {
			return file_info.url;
		}

		public void copyUrl() {
			Clipboard.SetText(file_info.url);
		}



		private void uploadFailed() {
			uploadCanceled();
			_barProgress.Visible = false;
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_cross;
			file_info.status = DC_FileInformationStatus.UploadFailed;
		}


		public void startUpload() {
			file_info.status = DC_FileInformationStatus.Uploading;
			Icon file_icon = Icon.ExtractAssociatedIcon(file_info.local_file_location);
			_picPreview.Image = file_icon.ToBitmap();
			connector.uploadFile(file_info.local_file_location);
		}

		public void cancelUpload() {
			if(file_info.status == DC_FileInformationStatus.Uploading || file_info.status == DC_FileInformationStatus.PendingUpload) {
				connector.cancelActions();
			}
		}



		private void _barProgress_MouseDown(object sender, MouseEventArgs e) {
			tableLayoutPanel1_MouseDown(sender, e);
		}

		private void _lblStatus_MouseDown(object sender, MouseEventArgs e) {
			tableLayoutPanel1_MouseDown(sender, e);
		}

		private void _lblFileName_MouseDown(object sender, MouseEventArgs e) {
			tableLayoutPanel1_MouseDown(sender, e);
		}

		private void _picPreview_MouseDown(object sender, MouseEventArgs e) {
			tableLayoutPanel1_MouseDown(sender, e);
		}


		private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e) {
			BackColor = SystemColors.Highlight;
			_lblFileName.ForeColor = SystemColors.ControlLightLight;
			_lblStatus.ForeColor = SystemColors.ControlLightLight;
			onEnter(sender, e);
		}

		public void onLeave() {
			BackColor = SystemColors.ControlLightLight;
			_lblFileName.ForeColor = SystemColors.ControlText;
			_lblStatus.ForeColor = SystemColors.ControlText;
		}
	}
}