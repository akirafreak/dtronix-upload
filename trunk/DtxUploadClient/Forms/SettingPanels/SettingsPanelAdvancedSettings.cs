using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DtxUpload {
	public partial class SettingsPanelAdvancedSettings : SettingsControlPanel {
		public SettingsPanelAdvancedSettings() {
			this.panel_image = Properties.Resources.arrow_incident_blue_24;
			this.panel_name = "Advanced Settings";

			InitializeComponent();
		}

		private void _btnOpenApplicationDataDir_Click(object sender, EventArgs e) {
			Process.Start(Client.directory_app_data);
		}

		private void _btnProgramTempDir_Click(object sender, EventArgs e) {
			Process.Start(Client.directory_temp);
		}

		private void _btnProgramFilesDir_Click(object sender, EventArgs e) {
			Process.Start(Application.StartupPath);
		}

		private void _btnOpenConfig_Click(object sender, EventArgs e) {
			Process.Start("notepad", Path.Combine(Client.directory_app_data, "settings.dcf"));
		}
	}
}
