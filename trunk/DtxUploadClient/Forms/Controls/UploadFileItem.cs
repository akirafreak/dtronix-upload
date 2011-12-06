using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using dtxCore.Json;

namespace DtxUpload {
	public partial class UploadFileItem : UserControl {

		public DC_FileInformation file_info;
		private ServerConnector connector;
		private ServerConnectorResponse upload_response;
		private TransparentPanel overlay_panel = new TransparentPanel();

		// Events
		public MouseEventHandler onEnter;
		public EventHandler onDoubleClick;

		public UploadFileItem(DC_FileInformation file_info) {
			connector = new ServerConnector();

			connector.upload_progress += uploadProgress;
			connector.upload_completed += uploadCompleted;
			connector.upload_canceled += uploadCanceled;

			this.file_info = file_info;
			InitializeComponent();

			file_info.status = DC_FileInformationStatus.PendingUpload;
			_barProgress.Value = 0;
			_lblFileName.Text = file_info.file_name;
			_lblStatus.Text = "[WAITING] File Size: " + file_info.file_size.ToString();
		}


		private void _panOverlay_DoubleClick(object sender, EventArgs e) {
			onDoubleClick(sender, e);
		}

		private void _panOverlay_MouseDown(object sender, MouseEventArgs e) {
			BackColor = SystemColors.Highlight;
			_lblFileName.ForeColor = SystemColors.ControlLightLight;
			_lblStatus.ForeColor = SystemColors.ControlLightLight;
			onEnter(sender, e);
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

		private void uploadCompleted(ServerConnectorResponse response) {
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

		private void uploadCanceled() {
			this.Invoke((MethodInvoker)delegate {
				connectionClosed();

				_barProgress.Value = 1;
				_barProgress.Maximum = 1;
				_lblStatus.Text = "Canceled";
				_picPreview.BackgroundImage = Properties.Resources.icon_24_em_cross;
				file_info.status = DC_FileInformationStatus.UploadCanceled;
			});
		}

		private void uploadProgress(ServerConnectorUploadProgressChangedEventArgs e) {
			this.Invoke((MethodInvoker)delegate {
				_picPreview.BackgroundImage = Properties.Resources.icon_24_em_up;
				_lblStatus.Text = "(" + dtxCore.Utilities.formattedSize(e.bytes_sent) + "/" + dtxCore.Utilities.formattedSize(e.total_bytes_to_send) + ")";
				_barProgress.Maximum = (int)e.total_bytes_to_send;
				_barProgress.Value = (int)e.bytes_sent;
			});

		}

		private void upload_failed_exceeded_toal_used_space() {
			uploadCanceled();
			_lblStatus.Text = "File exceeds alotted space";
		}

		private void upload_failed_exceeded_file_size() {
			uploadCanceled();
			_lblStatus.Text = "File size too large";
		}


		private void upload_successful() {
			_lblStatus.Visible = false;
			_barProgress.Visible = false;
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_check;
			file_info.status = DC_FileInformationStatus.Uploaded;

			JsonReader jr = new JsonReader(upload_response.body);
			DC_FileInformation info = jr.Deserialize<DC_FileInformation>();

			file_info.url_id = info.url_id;
			file_info.url = connector.server_info.upload_base_url + info.url_id;

			// Automatically copy the URL to clipboard if the user so desires.
			if(Client.config.get<bool>("frmquickupload.copy_upload_clipboard")) {
				Clipboard.SetText(file_info.url, TextDataFormat.UnicodeText);
			}
		}

		private void upload_failed_db_error() {
			uploadFailed();
			_lblStatus.Text = "Server DB Error";
		}

		private void upload_failed_could_not_handle_file() {
			uploadFailed();
			_lblStatus.Text = "Server File Error";
		}

		private void upload_failed_not_connected() {
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

		public void deleteUpload() {
			if(file_info.status != DC_FileInformationStatus.Uploaded)
				return;

			connector.callServerMethod("Files:Delete", deleteUpload_response, file_info.url_id);
		}

		private void deleteUpload_response(ServerConnectorResponse response) {
			switch(response.getCalledMethod()) {
				case "file_delete_failure_owner":
					this.Invoke(new MethodInvoker(file_delete_failure_owner));
					break;

				case "file_delete_failure":
					this.Invoke(new MethodInvoker(file_delete_failure));
					break;

				case "file_delete_confirmation":
					this.Invoke(new MethodInvoker(file_delete_confirmation));
					break;

				default:
					connector.execServerResponse(response);
					break;
			}
		}

		private void file_delete_failure_owner() {
			_lblStatus.Text = "File Delete Failure";
		}

		private void file_delete_failure() {
			_lblStatus.Text = "File Delete Failure";

		}

		private void file_delete_confirmation() {
			// Remove this control from the container.
			this.Parent.Controls.Remove(this);
		}

		public void onLeave() {
			BackColor = SystemColors.ControlLightLight;
			_lblFileName.ForeColor = SystemColors.ControlText;
			_lblStatus.ForeColor = SystemColors.ControlText;
		}
	}
}