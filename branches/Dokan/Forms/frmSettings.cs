using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dtxUpload.Classes;

namespace dtxUpload {
	public partial class frmSettings : Form {
		public frmSettings() {
			Client.form_Settings = this;
			InitializeComponent();
		}

		private void frmSettings_Load(object sender, EventArgs e) {

		}


	}
}
