using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using dtxCore;
using System.Media;

namespace dtxUpload {
	public partial class frmQuickUpload : Form {

		private List<UploadFileItem> uploading_itmes = new List<UploadFileItem>();
		private List<UploadFileItem> selected_upload_items = new List<UploadFileItem>();
		private EncoderParameters encoder_params_jpg = new EncoderParameters(1);
		private ImageCodecInfo codec_jpeg;
		private Tween drop_files_tween;
		
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

			_panDropUpload.Height = 0;
			
			// Immediately hide the confirmation row.
			_tlpUploadTable.RowStyles[1].Height = 0;
			_panConfirmUpload.Visible = false;

			// Make the form load the previous saved height if there is any.
			int saved_win_height = Client.config.get<int>("frmquickupload.window.height");
			this.Height = (saved_win_height == 0) ? this.Height : saved_win_height;

			// Make sure the form is the correct size.  Windows XP's form widths are less than 7's.
			OperatingSystemInfo osi = Utilities.getOSInfo();
			if(osi.os.Contains("XP")) {
				Size min_size = new Size() {
					Height = this.MinimumSize.Height - 8,
					Width = this.MinimumSize.Width - 8
				};

				Size max_size = new Size() {
					Height = this.MaximumSize.Height - 8,
					Width = this.MaximumSize.Width - 8
				};

				this.MinimumSize = min_size;
				this.MaximumSize = max_size;
				this.Size = min_size;
			}

		}

		private void completeAllUploads() {
			SoundPlayer player = new SoundPlayer(Properties.Resources.bubble_pop);
			player.Play();
		}

		private void frmQuickUpload_Load(object sender, EventArgs e) {
			drop_files_tween = new Tween(_panDropUpload, "Height", EasingEquations.expoEaseOut);

			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 13, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 13);

		}

		private UploadFileItem addUploadItem(DC_FileInformation upload_info) {
			UploadFileItem control = new UploadFileItem(upload_info) {
				Dock = DockStyle.Top,
				Location = new Point(0, 50),
				ContextMenu = _uploadItemContext
			};

			control.onEnter += delegate(object sender, MouseEventArgs e) {
				if (Control.ModifierKeys == Keys.Control){
					if(selected_upload_items.Contains(control)){
						if (e.Button != MouseButtons.Right) {
							selected_upload_items.Remove(control);
						}
					}else{
						selected_upload_items.Add(control);
					}
				}else{
					if (e.Button != MouseButtons.Right ||
						(e.Button == MouseButtons.Right && !selected_upload_items.Contains(control))) {
						selected_upload_items.Clear();
						selected_upload_items.Add(control);

					} else if (!selected_upload_items.Contains(control)) {
						selected_upload_items.Add(control);
					}
				}

				
				foreach (UploadFileItem loop_control in _panFileItemContainer.Controls) {
					if (!selected_upload_items.Contains(loop_control)) {
						loop_control.onLeave();
					}
				}
			};

			

			_panFileItemContainer.Controls.Add(control);
			uploading_itmes.Add(control);

			return control;
		}

		private void uploadFile(string location) {
			FileInfo fi = new FileInfo(location);
			DC_FileInformation file_info = new DC_FileInformation() {
				status = DC_FileInformationStatus.PendingUpload,
				file_name = fi.Name,
				file_size = fi.Length,
				local_file_location = location,
			};
			uploadFile(file_info);
		}

		private void uploadFile(DC_FileInformation file_info) {
			UploadFileItem control = addUploadItem(file_info);
			control.startUpload();
		}

		private void uploadZipFile(string location) {

		}

		private void frmQuickUpload_FormClosing(object sender, FormClosingEventArgs e) {
			e.Cancel = true;
			this.Hide();
		}

		private Tween confirm_upload_tween = new Tween(EasingEquations.expoEaseOut);
		private bool show_clipboard_confirmation;

		private void _btnUploadClipboard_Click(object sender, EventArgs e) {
			_chkHideClipConfirmation.Checked = false;

			// Check to see if the user has disabled verification of upload.
			show_clipboard_confirmation = Client.config.get<bool>("frmquickupload.show_clipboard_confirmation");

			if(show_clipboard_confirmation == true) {
				showConfirmMenu();
			} else {
				_btnConfirmClipboardUpload_Click(sender, e);
			}
		}

		private void _btnConfirmClipboardUpload_Click(object sender, EventArgs e) {

			// Check to see if the user has disabled verification of upload.
			if(show_clipboard_confirmation == true) {
				hideConfirmMenu();
			}

			if(Clipboard.ContainsFileDropList()) {
				foreach(string file in Clipboard.GetFileDropList()) {
					uploadFile(file);
				}

			} else if(Clipboard.ContainsImage()) {
				uploadImage(Clipboard.GetImage(), "Clipboard_");

			} else if(Clipboard.ContainsText()) {
				string temp_file = Path.GetTempFileName();
				StreamWriter sw = new StreamWriter(temp_file);
				sw.Write(Clipboard.GetText());
				sw.Close();

				int total_text = Client.config.getAndIncrement("uploads.total_text_files");
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

		public void uploadScreenshot(int start_x, int start_y, int width, int height) {
			Bitmap base_bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			Graphics graphic = Graphics.FromImage(base_bitmap);
			Size sz = new Size(width, height);

			graphic.CopyFromScreen(start_x, start_y, 0, 0, sz, CopyPixelOperation.SourceCopy);
			uploadImage(base_bitmap, "Screenshot_");
		}

		private void uploadImage(Image image, string upload_prefix) {
			int screenshot_type = Client.config.get<int>("frmlogin.screenshot_upload_format");
			DC_ImageInformation file;

			switch(screenshot_type) {
				case 0: // Auto detect best method.
					file = tmpSaveImageAuto(image, upload_prefix);
					break;

				case 1: // JPEG
					file = tmpSaveImageJpeg(image, upload_prefix);
					break;

				case 2: // PNG
					file = tmpSaveImagePng(image, upload_prefix);
					break;

				default: // Just incase somebody inputted a number like -1 for jokes.
					file = tmpSaveImageAuto(image, upload_prefix);
					break;
			}

			DC_FileInformation file_info = new DC_FileInformation() {
				file_name = Path.GetFileName(file.full_location),
				delete_after_upload = true,
				file_size = file.size,
				local_file_location = file.full_location
			};

			uploadFile(file_info);
		}

		private DC_ImageInformation tmpSaveImageJpeg(Image img, string file_prefix) {

			int screenshots = Client.config.getAndIncrement("uploads.total_screenshots");
			string jpg_file = Path.GetTempPath() + "\\" + file_prefix + "Image_" + screenshots.ToString() + ".jpg";

			img.Save(jpg_file, codec_jpeg, encoder_params_jpg);

			return new DC_ImageInformation() {
				full_location = jpg_file,
				size = new FileInfo(jpg_file).Length,
				type = "jpg"
			};
		}

		private DC_ImageInformation tmpSaveImagePng(Image img, string file_prefix) {

			int screenshots = Client.config.getAndIncrement("uploads.total_screenshots");
			string jpg_file = Path.GetTempPath() + "\\" + file_prefix + "Image_" + screenshots.ToString() + ".png";

			img.Save(jpg_file, ImageFormat.Png);

			return new DC_ImageInformation() {
				full_location = jpg_file,
				size = new FileInfo(jpg_file).Length,
				type = "png"
			};
		}

		/// <summary>
		/// Automatically determine which image format is smaller and return that file.
		/// </summary>
		/// <param name="img"></param>
		/// <returns>Full location of the temp image file.</returns>
		private DC_ImageInformation tmpSaveImageAuto(Image img, string file_prefix) {
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

			int screenshots = Client.config.getAndIncrement("uploads.total_screenshots");

			string new_file = Path.GetTempPath() + "\\" + file_prefix + "Image_" + screenshots.ToString() + "." + type;

			File.Move(temp_file, new_file);
			return new DC_ImageInformation() {
				full_location = new_file,
				size = image_size,
				type = type
			};
		}

		private void _btnCancelAll_Click(object sender, EventArgs e) {
			foreach(UploadFileItem upload in uploading_itmes) {
				upload.cancelUpload();
			}
		}

		private void _chkHideClipConfirmation_CheckedChanged(object sender, EventArgs e) {
			Client.config.set("frmquickupload.show_clipboard_confirmation", !_chkHideClipConfirmation.Checked);
			Client.config.save();
		}

		private void frmQuickUpload_ResizeEnd(object sender, EventArgs e) {
			Client.config.set("frmquickupload.window.height",this.Height);
			Client.config.save();
		}

		private void _btnCancelConfirmUpload_Click(object sender, EventArgs e) {
			hideConfirmMenu();
		}

		private void hideConfirmMenu() {
			int start = (int)_tlpUploadTable.RowStyles[1].Height;

			confirm_upload_tween.start(start, 0, delegate(int current) {
				_tlpUploadTable.RowStyles[1].Height = current;
				if(current == 0) {
					_panConfirmUpload.Visible = false;
				}
			});
		}

		private void showConfirmMenu() {
			int start_height = (int)_tlpUploadTable.RowStyles[1].Height;
			_panConfirmUpload.Visible = true;

			confirm_upload_tween.start(start_height, 30, delegate(int current) {
				_tlpUploadTable.RowStyles[1].Height = current;
			});
		}


		private void _btnDropUploadFile_DragEnter(object sender, DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
				e.Effect = DragDropEffects.All;
			}
		}

		private bool is_dragging = false;
		private bool is_drop_window_open = false;

		private void _btnDropUploadFile_DragDrop(object sender, DragEventArgs e) {
			drop_files_tween.start(0);
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in files) {
				uploadFile(file);
			}
		}


		private void _btnDropZip_DragDrop(object sender, DragEventArgs e) {
			drop_files_tween.start(0);
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in files) {
				uploadZipFile(file);
			}
		}

		private void _panFileItemContainer_DragLeave(object sender, EventArgs e) {
			drop_files_tween.start(0);
		}

		private void _panDropUpload_DragEnter(object sender, DragEventArgs e) {
			_panFileItemContainer_DragEnter(sender, e);
		}

		private void _panFileItemContainer_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.All;
			//drop_files_tween.start(74);
		}

		private void _panDropUpload_DragDrop(object sender, DragEventArgs e) {
			_panFileItemContainer_DragLeave(null, null);
		}

		private void _btnDropZip_DragEnter(object sender, DragEventArgs e) {
			_panFileItemContainer_DragEnter(null, null);
		}

		private void _btnDropPrivate_DragEnter(object sender, DragEventArgs e) {
			_panFileItemContainer_DragEnter(null, null);
		}

		private void _btnDropEncrypt_DragEnter(object sender, DragEventArgs e) {
			_panFileItemContainer_DragEnter(null, null);
		}

		private void _panFileItemContainer_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach(string file in files) {
				uploadFile(file);
				//uploadZipFile(file);
			}
		}

		private void _btnDropUploadFile_Click(object sender, EventArgs e) {

		}

		private void _uploadItemContext_Popup(object sender, EventArgs e) {
			bool is_uploading = false; // Check to see if we have any uploading items.
			bool is_uploaded = false;

			foreach (UploadFileItem item in selected_upload_items) {
				if (item.file_info.status == DC_FileInformationStatus.Uploading || item.file_info.status == DC_FileInformationStatus.PendingUpload) {
					is_uploading = true;
				} else if (item.file_info.status == DC_FileInformationStatus.Uploaded) {
					is_uploaded = true;
				}
			}

			_mItemCancel.Visible = is_uploading;
			_mItemDelete.Visible = is_uploaded;
		}

		private void _mItemOpenLinks_Click(object sender, EventArgs e) {
			if (selected_upload_items.Count > 6) {
				DialogResult result = MessageBox.Show("Are you sure you want to open " + selected_upload_items.Count.ToString() + " files?", "Confirm Open", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result != DialogResult.OK)
					return;
			}
			foreach (UploadFileItem item in selected_upload_items) {
				if (item.file_info.status == DC_FileInformationStatus.Uploaded) {
					System.Diagnostics.Process.Start(item.file_info.url);					
				}
			}
		}

		private void _mItemCopyLinks_Click(object sender, EventArgs e) {
			StringBuilder clip_text = new StringBuilder();

			foreach (UploadFileItem item in selected_upload_items) {
				if (item.file_info.status == DC_FileInformationStatus.Uploaded) {
					clip_text.Append(item.file_info.url);
					clip_text.Append("\r\n");
				}
			}
			// No need to over write the user's clipboard if there is nothing to copy.
			if(clip_text.Length > 0){
				Clipboard.SetText(clip_text.ToString());
			}
		}

		private void _mItemDelete_Click(object sender, EventArgs e) {

		}

		private void _mItemCancel_Click(object sender, EventArgs e) {
			foreach(UploadFileItem item in selected_upload_items) {
				item.cancelUpload();
			}
		}




		
	}
}
