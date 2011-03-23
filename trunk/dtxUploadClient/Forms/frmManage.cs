using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dtxUpload {
	public partial class frmManage : Form {
		ServerConnector connector = new ServerConnector();

		public frmManage() {
			Client.form_Manage = this;
			InitializeComponent();
		}

		TreeNode node_root;

		private void frmManage_Load(object sender, EventArgs e) {
			_treFolders.ContextMenu = _contextMenuFolders;

			string server_name = Client.server_info.server_name;
			_imlFolders.Images.Add("folder", Properties.Resources.asset_blue_16_ns);
			_imlFolders.Images.Add("folder_private", Properties.Resources.asset_grey_16_ns);
			_imlFolders.Images.Add("server", Properties.Resources.circle_blue_16_ns);

			_imlFiles.Images.Add("file", Properties.Resources.square_green_16_ns);
			

			if (server_name == "") {
				server_name = "Server";
			}
			node_root = _treFolders.Nodes.Add("root", server_name, "server", "server");

			node_root.Nodes.Add("folder_unsorted", "Unsorted Uploads", "folder", "folder");
			node_root.Nodes.Add("folder_private", "Private", "folder_private", "folder_private");
			node_root.Expand();
		}

		private void _treFolders_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			if (e.Node.Name == "folder_unsorted") {
				connector.callServerMethod("files_in_directory", "/");
			}
		}

		public void displayFolderContents(DC_FileInformation[] files) {
			foreach(DC_FileInformation file in files){
				ListViewItem item = _lstFiles.Items.Add("file_" + file.url_id, file.file_name, "file");

				item.SubItems.Add(file.file_size.ToString());
				item.SubItems.Add("");
				item.SubItems.Add(file.upload_date);
			}
		}
	}
}
