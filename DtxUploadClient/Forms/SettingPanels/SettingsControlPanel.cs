using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DtxUpload {
	public class SettingsControlPanel : UserControl {
		public string panel_name;
		public Bitmap panel_image;

		/// <summary>
		/// Save the settings to the config class.
		/// </summary>
		public virtual void saveSettings() { }

		/// <summary>
		/// Set the settings back to the intial or last saved settings.
		/// </summary>
		public virtual void resetSettings() { }
	}
}
