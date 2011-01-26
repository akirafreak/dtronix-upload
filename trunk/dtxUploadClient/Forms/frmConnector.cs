using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace dtxUpload {
	public partial class frmConnector : Form {
		ServerConnector connector = new ServerConnector();

		public frmConnector() {
			InitializeComponent();
#if DEBUG
			Client.form_connector = this;
#endif
		}


		public int addRequest(string request_func, string request_args) {
			int id = _dgvTransport.Rows.Add();
			_dgvTransport.Rows[id].Cells["id"].Value = id.ToString();
			_dgvTransport.Rows[id].Cells["request_function"].Value = request_func;
			_dgvTransport.Rows[id].Cells["request_args"].Value = request_args;
			_dgvTransport.Rows[id].Cells["request_time"].Value = DateTime.Now.ToLongTimeString();

			return id;
		}

		public void updateRequest(int id, string responce) {
			_dgvTransport.Rows[id].Cells["responce_raw"].Value = responce;
			_dgvTransport.Rows[id].Cells["responce_time"].Value = DateTime.Now.ToLongTimeString();
			int height_multiplier = responce.Split('\n').Length;
			_dgvTransport.Rows[id].Height = (_dgvTransport.Rows[id].Height - 8) * height_multiplier;
		}

		private void frmConnector_Load(object sender, EventArgs e) {
			_dgvTransport.Columns["responce_raw"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			_dgvTransport.Columns["responce_raw"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
		}

		private void _btnPing_Click(object sender, EventArgs e) {
			connector.callServerMethod("ping");
		}

		private void _btnVerifyLogin_Click(object sender, EventArgs e) {
			connector.callServerMethod("ping_logged");
		}

		private void _btnClear_Click(object sender, EventArgs e) {
			_dgvTransport.Rows.Clear();
		}


	}
}
