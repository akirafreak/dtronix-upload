using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DtxUpload {
	public partial class frmSettings : Form {

		private SettingsControlPanel[] panels = new SettingsControlPanel[]{
			new SettingsPanelClient(),
			new SettingsPanelAdvancedSettings()
		};

		private SettingsControlPanel current_panel = null;

		public frmSettings() {
			InitializeComponent();

			foreach(SettingsControlPanel panel in panels) {
				_imlSettingImages.Images.Add(panel.panel_name, panel.panel_image);
				ListViewItem tab_item = _lstSettingType.Items.Add(panel.panel_name, panel.panel_name, panel.panel_name);

				tab_item.Tag = panel;

				panel.Visible = false;
				_panControls.Controls.Add(panel);

				// Set all the settings to the default or last saved settings.
				panel.resetSettings();
			}


			// Automatically select the first item.
			_lstSettingType.Items[0].Selected = true;
		}



		private void frmSettings_Load(object sender, EventArgs e) {

		}

		private void _lstSettingType_SelectedIndexChanged(object sender, EventArgs e) {
			
			// Check to see if anything is selected.
			if(_lstSettingType.SelectedItems.Count == 0)
				return;

			SettingsControlPanel panel = _lstSettingType.SelectedItems[0].Tag as SettingsControlPanel;
			if(panel == null)
				return;

			// Check to see if the panel is the same as the last.  If so, do nothing.
			if(panel == current_panel)
				return;

			if(current_panel != null)
				current_panel.Visible = false;

			panel.Visible = true;
			current_panel = panel;
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void _btnSaveSettings_Click(object sender, EventArgs e) {
			// Save the settings for each to the memory.
			foreach(SettingsControlPanel panel in panels) {
				panel.saveSettings();
			}

			// Finally save all the settings to the settings file.
			Client.config.save();
			this.Close();
		}
	}
}
