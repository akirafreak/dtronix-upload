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

		public UploadFileItem(DC_FileInformation file_info) {
			connector = new ServerConnector(this);

			this.file_info = file_info;
			InitializeComponent();

			file_info.status = DC_FileInformationStatus.PendingUpload;
			_barProgress.Value = 0;
			_lblFileName.Text = file_info.file_name;
			_lblStatus.Text = "[WAITING] File Size: " + file_info.file_size.ToString();
		}

		public void uploadProgress(DC_UploadProgressChangedEventArgs e) {
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_up;
			_lblStatus.Text = "(" + dtxCore.Utilities.formattedSize(e.bytes_sent) + "/" + dtxCore.Utilities.formattedSize(e.total_bytes_to_send) + ")";
			_barProgress.Maximum = (int)e.total_bytes_to_send;
			_barProgress.Value = (int)e.bytes_sent;
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

		public void uploadExceededSpace() {
			uploadCanceled();
			_lblStatus.Text = "File exceeds alotted space";
		}

		public void uploadExceededSize() {
			uploadCanceled();
			_lblStatus.Text = "File size too large";
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

		public void uploadCanceled() {
			uploadPostCompleted();

			_barProgress.Value = 1;
			_barProgress.Maximum = 1;
			_lblStatus.Text = "Canceled";
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_cross;
			file_info.status = DC_FileInformationStatus.UploadCanceled;
		}


		public void uploadSuccessful(string server_data) {
			_lblStatus.Visible = false;
			_barProgress.Visible = false;
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_check;
			file_info.status = DC_FileInformationStatus.Uploaded;

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
			uploadCanceled();
			_barProgress.Visible = false;
			_picPreview.BackgroundImage = Properties.Resources.icon_24_em_cross;
			file_info.status = DC_FileInformationStatus.UploadFailed;
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