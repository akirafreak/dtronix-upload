using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using Core;

namespace dtxUpload {
	public partial class frmQuickUpload : Form {

		private List<UploadFileItem> uploading_itmes = new List<UploadFileItem>();
		EncoderParameters encoder_params_jpg = new EncoderParameters(1);
		ImageCodecInfo codec_jpeg;
		
		public frmQuickUpload() {
			Client.form_QuickUpload = this;
			encoder_params_jpg.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);

			ImageCodecInfo[] images = ImageCodecInfo.GetImageDecoders();

			// Get all the codecs for encoding the images.
			foreach(ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders()) {
				if(codec.MimeType == "image/jpeg") {
					codec_jpeg = codec;
				}
			}

			InitializeComponent();
		}

		private void frmQuickUpload_Load(object sender, EventArgs e) {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 13, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 13);
		}

		private UploadFileItem addUploadItem(DC_FileInformation upload_info) {

			UploadFileItem control = new UploadFileItem(upload_info) {
				Dock = DockStyle.Top,
				Location = new Point(0, 50),
				MaximumSize = new Size(250, 50),
				MinimumSize = new Size(250, 36),
				Size = new Size(250, 50),
				TabIndex = 1

			};
			_panFileItemContainer.Controls.Add(control);
			uploading_itmes.Add(control);

			return control;
		}

		private void uploadFile(string file_location) {
			FileInfo fi = new FileInfo(file_location);
			DC_FileInformation file_info = new DC_FileInformation() {
				status = 5,
				file_name = fi.Name,
				file_size = fi.Length,
				local_file_location = file_location,
			};
			uploadFile(file_info);
		}

		private void uploadFile(DC_FileInformation file_info) {
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

			if(Clipboard.ContainsFileDropList()) {	
				foreach(string file in Clipboard.GetFileDropList()) {
					uploadFile(file);
				}

			} else if(Clipboard.ContainsImage()) {
				DC_ImageInformation file = smartImage(Clipboard.GetImage(), "Clipboard_");

				DC_FileInformation file_info = new DC_FileInformation() {
					file_name = Path.GetFileName(file.full_location),
					delete_after_upload = true,
					file_size = file.size,
					local_file_location = file.full_location
				};

				uploadFile(file_info);

			} else if(Clipboard.ContainsText()) {
				string temp_file = Path.GetTempFileName();
				StreamWriter sw = new StreamWriter(temp_file);
				sw.Write(Clipboard.GetText());
				sw.Close();

				

				int total_text = Config.getAndIncrement("uploads.total_text_files");
				string new_filename = Path.GetTempPath() + "\\Clipboard_Text_" + total_text.ToString() + ".txt";

				File.Move(temp_file, new_filename);
				FileInfo fi = new FileInfo(new_filename);

				DC_FileInformation file_info = new DC_FileInformation() {
					file_name = Path.GetFileName(new_filename),
					delete_after_upload = true,
					file_size = fi.Length,
					local_file_location = new_filename
				};

				uploadFile(file_info);

			}
		}

		/// <summary>
		/// Automatically determine which image format is smaller and return that file.
		/// </summary>
		/// <param name="img"></param>
		/// <returns>Full location of the temp image file.</returns>
		private DC_ImageInformation smartImage(Image img, string file_prefix) {
			string temp_file = Path.GetTempFileName();
				


			long image_size;
			string type;
			MemoryStream stream_jpeg = new MemoryStream();
			MemoryStream stream_png = new MemoryStream();
			FileStream output_file = new FileStream(temp_file, FileMode.Append);

			// Save the image in multiple formats.
			img.Save(stream_jpeg, codec_jpeg, encoder_params_jpg);
			img.Save(stream_png, ImageFormat.Png);

			// If the Jpeg is smaller, upload that.
			if(stream_jpeg.Length < stream_png.Length) {
				image_size = stream_jpeg.Length;
				type = "jpg";
				stream_jpeg.WriteTo(output_file);

			} else {
				// Otherewise, save the PNG format due to lossless format.
				image_size = stream_png.Length;
				type = "png";
				stream_png.WriteTo(output_file);
			}

			// Close all the streams.
			stream_jpeg.Close();
			stream_png.Close();
			output_file.Close();

			int screenshots = Config.getAndIncrement("uploads.total_screenshots");

			string new_file = Path.GetTempPath() + "\\" + file_prefix + "Image_" + screenshots.ToString() + "." + type;

			File.Move(temp_file, new_file);
			return new DC_ImageInformation() {
				full_location = new_file,
				size = image_size,
				type = type
			};
		}

		private void _btnClearList_Click(object sender, EventArgs e) {
			foreach(UploadFileItem upload in uploading_itmes) {
				// Do not remove the file if it is currently uploading.
				if(upload.file_info.status != 1) {
					_panFileItemContainer.Controls.Remove(upload);
				}
			}
		}

		private void _btnCancelAll_Click(object sender, EventArgs e) {
			foreach(UploadFileItem upload in uploading_itmes) {
				upload.cancelUpload();
			}
		}
	}
}
