using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DtxUpload {
	/// <summary>
	/// Delegate to handle the directory_selected event.
	/// </summary>
	/// <param name="directory">Selected directory information.</param>
	public delegate void DirectoryTreeListDirectorySelected(DC_DirectoryInformation directory);

	/// <summary>
	/// Delegate to handle the status_update event.
	/// </summary>
	/// <param name="text">Text that describes the current status.</param>
	/// <param name="status">Status update description.</param>
	public delegate void DirectoryTreeListStatusDelegate(string text, DirectoryTreeListStatusEnum status);

	public partial class DirectoryTreeList : UserControl {
		private TreeNode node_root;
		private TreeNode last_selected;
		private ServerConnector connector;
		private List<TreeNode> directory_nodes = new List<TreeNode>();

		public event DirectoryTreeListDirectorySelected directory_selected;
		public event DirectoryTreeListStatusDelegate status_update;

		public DirectoryTreeList() {
			InitializeComponent();
		}

		/// <summary>
		/// Forces a refresh of all the directories.
		/// </summary>
		public void refreshDirectories() {
			connector.callServerMethod("Files:directoryList");
		}

		/// <summary>
		/// Creates a new directory and sets focus on control.
		/// </summary>
		public void createDirectory() {
			this.Focus();
			_cmiCreateDirectory_Click(null, null);
		}

		public DC_DirectoryInformation getDirectoryAtCursor() {
			Point cursora = Cursor.Position;
			Point client_pt = this.PointToClient(cursora);

			TreeNode node = _treDirectories.GetNodeAt(client_pt);
			if(node == null)
				return null;

			DC_DirectoryInformation dir = node.Tag as DC_DirectoryInformation;

			if(dir == null && node.Name == "directory_unsorted") {
				dir = new DC_DirectoryInformation() {
					url_id = "0",
					name = "Unsorted"
				};
			}

			return dir;
		}


		private void DirectoryTreeList_Load(object sender, EventArgs e) {
			_imlDirectories.Images.Add("directory", Properties.Resources.asset_blue_16_ns);
			_imlDirectories.Images.Add("directory_private", Properties.Resources.asset_grey_16_ns);
			_imlDirectories.Images.Add("server", Properties.Resources.circle_blue_16_ns);
			_imlDirectories.Images.Add("updating", Properties.Resources.lightening_16_ns);

			// Hide the search bar.
			_layout.RowStyles[1].Height = 0;

			string server_name = Client.server_info.server_name;
			if(server_name == "") {
				server_name = "Server";
			}
			_treDirectories.ContextMenu = _contextMenuDirectory;
			node_root = _treDirectories.Nodes.Add("root", server_name, "server", "server");
			updatingDirectory(node_root);
			node_root.Expand();
			connector = new ServerConnector(this);
		}

		/// <summary>
		/// Sets a directory to be in "Updating" to visually let the user know that something is going on.
		/// </summary>
		/// <param name="node">The node that is being updated.</param>
		private void updatingDirectory(TreeNode node) {
			DC_DirectoryInformation dir = node.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;

			node.SelectedImageKey = "updating";
			node.ImageKey = "updating";
		}

		/// <summary>
		/// Called after the directory is updated successfully or otherwise.
		/// </summary>
		/// <param name="node">The node that has completed the work requested.</param>
		private void completedUpdatingDirectory(TreeNode node) {
			DC_DirectoryInformation dir = node.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;

			node.Text = dir.name;
			if(dir.bool_public) {
				node.SelectedImageKey = "directory";
				node.ImageKey = "directory";

			} else {
				node.SelectedImageKey = "directory_private";
				node.ImageKey = "directory_private";
			}
		}
		/// <summary>
		/// Add a directory to the TreeList
		/// </summary>
		/// <param name="dir">Directory information.</param>
		private void addDirectory(DC_DirectoryInformation dir) {
			string dir_type = (dir.bool_public) ? "directory" : "directory_private";
			TreeNode node = node_root.Nodes.Add(dir.id, dir.name, dir_type, dir_type);
			node.Tag = dir;

			directory_nodes.Add(node);
		}

		#region Server called methods

		public void directoryList(DC_DirectoryInformation[] directories) {
			node_root.Nodes.Clear();
			directory_nodes.Clear();
			TreeNode unsorted_node = node_root.Nodes.Add("directory_unsorted", "Unsorted Uploads", "directory", "directory");
			directory_nodes.Add(unsorted_node);
			node_root.Expand();

			if(directories == null)
				return;
			completedUpdatingDirectory(node_root);

			foreach(DC_DirectoryInformation directory in directories) {
				addDirectory(directory);
			}

			node_root.Expand();
			_treDirectories.SelectedNode = node_root.Nodes[0];

			if(status_update != null)
				status_update("Idle", DirectoryTreeListStatusEnum.idle);
		}

		public void directoryCreateConfirmation(DC_DirectoryInformation dir) {
			addDirectory(dir);

			if(status_update != null)
				status_update("Idle", DirectoryTreeListStatusEnum.created);
		}

		public void directoryCreateFailure() {
			if(status_update != null)
				status_update("Error creating directory.", DirectoryTreeListStatusEnum.create_error);
		}

		public void directoryRenameConfirmation(string url_id, string name) {
			foreach(TreeNode node in node_root.Nodes) {
				DC_DirectoryInformation dir = node.Tag as DC_DirectoryInformation;
				if(dir == null)
					continue;

				if(dir.url_id == url_id) {
					dir.name = name;
					completedUpdatingDirectory(node);
					break;
				}
			}
			if(status_update != null)
				status_update("Directory renamed.", DirectoryTreeListStatusEnum.renamed);
		}

		public void directorySetPropertiesConfirmation() {
			if(status_update != null)
				status_update("Directory properties set.", DirectoryTreeListStatusEnum.property_set);
		}

		public void directorySetPropertiesFailure() {
			if(status_update != null)
				status_update("Unable to change directory property.  Please try again.", DirectoryTreeListStatusEnum.property_set_error);

			// Get the current list again.
			connector.callServerMethod("Files:directoryList");
		}

		public void directoryDeleteConfirmation(string url_id) {
			foreach(TreeNode node in node_root.Nodes) {
				var dir = node.Tag as DC_DirectoryInformation;
				if(dir == null)
					continue;

				if(dir.url_id == url_id) {
					node.Remove();
					if(status_update != null)
						status_update("Directory deleted.", DirectoryTreeListStatusEnum.deleted);
					return;
				}
			}
			status_update("Directory deleted.", DirectoryTreeListStatusEnum.deleted);
		}

		public void directoryDeleteFailure() {
			if(status_update != null)
				status_update("Error while deleting directory.  Please try again.", DirectoryTreeListStatusEnum.delete_error);

			// Get the current list again.
			connector.callServerMethod("Files:directoryList");
		}
#endregion

		private void _treDirectories_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
			if(e.Label == null)
				return;

			if(e.Node.Tag == null) {
				connector.callServerMethod("Files:directoryCreate", e.Label);
				e.Node.Remove();
				if(status_update != null)
					status_update("Creating directory " + e.Label, DirectoryTreeListStatusEnum.creating);

			} else {
				DC_DirectoryInformation dir = e.Node.Tag as DC_DirectoryInformation;
				if(dir == null) {
					e.CancelEdit = true;
					return;
				}

				// Determine if the label has changed at all.
				if(dir.name == e.Label)
					return;


				if(Regex.IsMatch(e.Label, @"^[a-zA-Z0-9 _-]+$") == false) {
					MessageBox.Show("Directory name can only contain Alphanumeric and any of the following characters: _-", "Directory Name Change", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					e.CancelEdit = true;
					return;

				} else if(e.Label.Length > 128) {
					MessageBox.Show("Directory name has to be less than 128 characters.", "Directory Name Change", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					e.CancelEdit = true;
					return;
				}

				updatingDirectory(e.Node);
				connector.callServerMethod("Files:directoryRename", dir.url_id, e.Label);
				if(status_update != null)
					status_update("Renaming Directory " + dir.name, DirectoryTreeListStatusEnum.renaming);
			}
		}

		private void _treDirectories_AfterSelect(object sender, TreeViewEventArgs e) {
			if(last_selected == e.Node)
				return;

			last_selected = e.Node;
			if(e.Node.Name == "directory_unsorted") {
				if(directory_selected != null) {
					directory_selected(new DC_DirectoryInformation() {
						url_id = "0",
						name = "Unsorted"
					});
				}

			} else {
				DC_DirectoryInformation dir = e.Node.Tag as DC_DirectoryInformation;
				if(dir == null)
					return;

				if(directory_selected != null)
					directory_selected(dir);
			}
		}


		private void _treDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			if(last_selected == e.Node && e.Button == MouseButtons.Right)
				return;

			if(e.Button == MouseButtons.Right)
				_treDirectories.SelectedNode = e.Node;
		}

		private void _treDirectories_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
			if(e.Node.Name == "root" || e.Node.Name == "directory_unsorted") {
				e.CancelEdit = true;
				return;
			}
		}

		private void _contextMenuDirectory_Popup(object sender, EventArgs e) {
			var directory = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			bool is_dir = (directory == null) ? false : true;

			_cmiDirectoryCopyDirectoryUrl.Visible = is_dir;
			_cmiDirectoryBreak1.Visible = is_dir;
			_cmiDirectoryDelete.Visible = is_dir;
			_cmiDirectoryRename.Visible = is_dir;
			_cmiDirectoryMakePrivate.Visible = is_dir;
			_cmiDirectoryMakePublic.Visible = is_dir;

			// if this is not a dir, no need to do dir checks.
			if(is_dir == false)
				return;

			_cmiDirectoryMakePrivate.Visible = directory.bool_public;
			_cmiDirectoryMakePublic.Visible = !directory.bool_public;
		}

		private void _cmiCreateDirectory_Click(object sender, EventArgs e) {
			TreeNode new_directory = node_root.Nodes.Add("", "New Directory", "directory_private", "directory_private");
			new_directory.BeginEdit();
		}

		private void _cmiDirectoryMakePrivate_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;
			var set_prop = new Dictionary<string, bool>(){
				{"is_public", false}
			};
			connector.callServerMethod("Files:directorySetProperties", dir.url_id, set_prop);
			_treDirectories.SelectedNode.ImageKey = "directory_private";
			_treDirectories.SelectedNode.SelectedImageKey = "directory_private";
			dir.bool_public = false;

			if(status_update != null)
				status_update("Setting directory " + dir.name + " to Private.", DirectoryTreeListStatusEnum.property_setting);
		}

		private void _cmiDirectoryCopyDirectoryUrl_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;

			Clipboard.SetText(Client.server_info.server_url + "/" + dir.url_id);
		}

		private void _cmiDirectoryDelete_Click(object sender, EventArgs e) {

			if(_treDirectories.SelectedNode.Tag == null)
				return;

			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null) // Should never happen.
				return;

			if(dir.url_id == "")
				return;

			// Remove the node for user verification of deletion.
			_treDirectories.SelectedNode.Remove();

			connector.callServerMethod("Files:directoryDelete", dir.url_id);

			if(status_update != null)
				status_update("Deleting directory.", DirectoryTreeListStatusEnum.deleting);
		}

		private void _cmiDirectoryRename_Click(object sender, EventArgs e) {
			_treDirectories.SelectedNode.BeginEdit();
		}

		private void _cmiDirectoryMakePublic_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;
			var set_prop = new Dictionary<string, bool>(){
				{"is_public", true}
			};
			connector.callServerMethod("Files:directorySetProperties", dir.url_id, set_prop);
			_treDirectories.SelectedNode.ImageKey = "directory";
			_treDirectories.SelectedNode.SelectedImageKey = "directory";
			dir.bool_public = true;

			if(status_update != null)
				status_update("Setting directory " + dir.name + " to Public.", DirectoryTreeListStatusEnum.property_setting);
		}


		private void hideSearch() {
			_layout.RowStyles[1].Height = 0;
			_txtSearchBox.Text = "";

			filterDirectories();
		}

		private void showSearch() {
			_layout.RowStyles[1].Height = 25;
		}

		private void _txtSearchBox_Leave(object sender, EventArgs e) {
			if(_txtSearchBox.Text == "")
				hideSearch();
		}

		private void _treDirectories_KeyPress(object sender, KeyPressEventArgs e) {
			showSearch();
			
			_txtSearchBox.Text = e.KeyChar.ToString();
			_txtSearchBox.Focus();
			_txtSearchBox.Select(1, 0);
			
		}

		/// <summary>
		/// Searches for node items that match the search box.
		/// </summary>
		private void filterDirectories() {
			string search_text = _txtSearchBox.Text.ToLower();
			bool show_all = false;

			if(search_text == "")
				show_all = true;

			node_root.Nodes.Clear();

			foreach(TreeNode node in directory_nodes) {
				if(node.Text.ToLower().Contains(search_text) || show_all) {
					node_root.Nodes.Add(node);
				}
			}
		}

		private void _txtSearchBox_TextChanged(object sender, EventArgs e) {
			filterDirectories();
		}

		private void _txtSearchBox_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Escape) {
				hideSearch();
			}
		}
	}
}
