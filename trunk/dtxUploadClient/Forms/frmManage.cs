using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dtxUpload {
	public partial class frmManage : Form {
		private ServerConnector connector = new ServerConnector();
		private TreeNode last_selected;
		private TreeNode node_root;

		public frmManage() {
			Client.form_Manage = this;
			InitializeComponent();
		}

		private void frmManage_Load(object sender, EventArgs e) {
			_treFolders.ContextMenu = _contextMenuFolders;
			_lstFiles.ContextMenu = _contextMenuFiles;

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
			if (last_selected == e.Node && e.Button == MouseButtons.Right)
				return;

			last_selected = e.Node;
			_lstFiles.Items.Clear();
			if (e.Node.Name == "folder_unsorted") {
				connector.callServerMethod("files_in_directory", "/");

			} else if (e.Node.Name == "folder_private") {
				connector.callServerMethod("files_in_directory", "");

			} else {
				connector.callServerMethod("files_in_directory", "/" + e.Node.Text);
			}
		}

		public void displayFolderContents(DC_FileInformation[] files) {
			if (files == null)
				return;

			foreach(DC_FileInformation file in files){
				ListViewItem item = _lstFiles.Items.Add("file_" + file.url_id, file.file_name, "file");

				item.SubItems.Add(file.file_size.ToString());
				item.SubItems.Add("");
				item.SubItems.Add(file.upload_date);
			}
		}
	}
}
