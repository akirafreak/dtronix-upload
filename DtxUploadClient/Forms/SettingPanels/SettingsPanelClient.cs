using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DtxUpload {
	public partial class SettingsPanelClient : SettingsControlPanel {
		private int screenshot_compression_type;
		private int screenshot_compression_jpeg_level;
		private long max_download_kbps;
		private long max_upload_kbps;
		private bool max_download_enabled;
		private bool max_upload_enabled;

		public SettingsPanelClient() {
			this.panel_image = Properties.Resources.settings_24_ns;
			this.panel_name = "Client";

			InitializeComponent();

			// Initialize all items.
			_barJpegCompression_ValueChanged(null, null);
		}

		public override void resetSettings() {
			_cmbScreenshotCompression.SelectedIndex = screenshot_compression_type = Client.config.get<int>("uploads.screenshot_compression_type", 0);
			_barJpegCompression.Value = screenshot_compression_jpeg_level = Client.config.get<int>("uploads.screenshot_compression_jpeg_level", 9);
			_nudMaxDownload.Value = max_download_kbps = Client.config.get<long>("connector.max_download_kbps", 0);
			_nudMaxUpload.Value = max_upload_kbps = Client.config.get<long>("connector.max_upload_kbps", 0);
			_chkMaxDownload.Checked = max_download_enabled = Client.config.get<bool>("connector.max_download_enabled", false);
			_chkMaxUpload.Checked = max_upload_enabled = Client.config.get<bool>("connector.max_upload_enabled", false);
			
		}

		public override void saveSettings() {
			Client.config.set("uploads.screenshot_compression_type", screenshot_compression_type);
			Client.config.set("uploads.screenshot_compression_jpeg_level", screenshot_compression_jpeg_level);
			Client.config.set("connector.max_download_kbps", max_download_kbps);
			Client.config.set("connector.max_upload_kbps", max_upload_kbps);
			Client.config.set("connector.max_download_enabled", max_download_enabled);
			Client.config.set("connector.max_upload_enabled", max_upload_enabled);
		}


		private void _barJpegCompression_ValueChanged(object sender, EventArgs e) {
			_lblJpegCompression.Text = string.Format("JPEG Compression ({0})", _barJpegCompression.Value * 10);
		}

		private void _nudMaxUpload_ValueChanged(object sender, EventArgs e) {
			max_upload_kbps = Convert.ToInt64(_nudMaxUpload.Value);
		}

		private void _nudMaxDownload_ValueChanged(object sender, EventArgs e) {
			max_download_kbps = Convert.ToInt64(_nudMaxDownload.Value);
		}

		private void _cmbScreenshotCompression_SelectedIndexChanged(object sender, EventArgs e) {
			screenshot_compression_type = _cmbScreenshotCompression.SelectedIndex;

			if(screenshot_compression_type == 2) { // If we are dealing with PNG compression, no need to set JPEG compression.
				_barJpegCompression.Enabled = false;
			} else {
				_barJpegCompression.Enabled = true;
			}
		}

		private void _barJpegCompression_Scroll(object sender, EventArgs e) {
			screenshot_compression_jpeg_level = _barJpegCompression.Value;
		}

		private void _chkMaxUpload_CheckedChanged(object sender, EventArgs e) {
			_nudMaxUpload.Enabled = max_upload_enabled = _chkMaxUpload.Checked;

		}

		private void _chkMaxDownload_CheckedChanged(object sender, EventArgs e) {
			_nudMaxDownload.Enabled = max_download_enabled = _chkMaxDownload.Checked;
		}
	}
}
