using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using dtxCore;

namespace dtxUpload {
	public partial class frmManage : Form {
		private ServerConnector connector = new ServerConnector();
		private TreeNode last_selected;
		private TreeNode node_root;
		private List<ListViewItem> dragging_files = new List<ListViewItem>();

		public frmManage() {
			Client.form_Manage = this;
			InitializeComponent();
		}

#region Server called actions

		public void fileMoveConfirmation() {
			_lblStatusText.Text = "Files moved successfully.";
		}

		public void fileMoveFailure() {
			_lblStatusText.Text = "Failed to move files.";
		}


		public void directoryDeleteConfirmation(string url_id) {
			foreach(TreeNode node in node_root.Nodes) {
				var dir = node.Tag as DC_DirectoryInformation;
				if(dir == null)
					continue;

				if(dir.url_id == url_id) {
					node.Remove();
					return;
				}
			}
			_lblStatusText.Text = "Idle";
		}

		public void directoryCreateConfirmation(DC_DirectoryInformation dir) {
			addDirectory(dir);
			_lblStatusText.Text = "Idle";
		}

		public void directoryDeletePending() {
			_lblStatusText.Text = "Idle";
		}

		public void directoryFiles(DC_FileInformation[] files) {
			DateTime converted_time;
			_lblStatusText.Text = "Idle";
			_lstFiles.Items.Clear();

			if(files == null)
				return;

			foreach(DC_FileInformation file in files) {
				converted_time = Utilities.unixToDateTime(Convert.ToInt64(file.upload_date));

				ListViewItem item = _lstFiles.Items.Add("file_" + file.url_id, file.file_name, "file");
				item.Tag = file;
				item.SubItems.Add(Utilities.formattedSize(file.file_size));
				item.SubItems.Add("");
				item.SubItems.Add(converted_time.ToShortDateString() + " " + converted_time.ToShortTimeString());
				item.SubItems.Add(file.total_views);
			}

			_lblTotalFiles.Text = files.Length.ToString() + " Items";
			_lblStatusText.Text = "Idle";
		}

		public void directoryList(DC_DirectoryInformation[] directories) {
			node_root.Nodes.Clear();
			node_root.Nodes.Add("directory_unsorted", "Unsorted Uploads", "directory", "directory");
			node_root.Expand();

			if(directories == null)
				return;
			completedUpdatingDirectory(node_root);

			foreach(DC_DirectoryInformation directory in directories) {
				addDirectory(directory);
			}

			node_root.Expand();
			_lblStatusText.Text = "Idle";
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
			_lblStatusText.Text = "Idle";
		}

		public void fileDeleteConfirmation() {
			_lblStatusText.Text = "File(s) have been deleted.";
		}

		public void fileDeleteFailure() {
			_lblStatusText.Text = "Error while deleteting files.  Please try again.";
		}

		public void directoryDeleteConfirmation() {
			_lblStatusText.Text = "Directory deleted.";
		}

		public void directoryDeleteFailure() {
			_lblStatusText.Text = "Error while deleting directory.  Please try again.";

			// Get the current list again.
			connector.callServerMethod("Files:directoryList");
		}

		public void directorySetPropertiesConfirmation() {
			_lblStatusText.Text = "Directory property set.";
		}

		public void directorySetPropertiesFailure() {
			_lblStatusText.Text = "Unable to change directory property.  Please try again.";

			// Get the current list again.
			connector.callServerMethod("Files:directoryList");
		}

#endregion
		/// <summary>
		/// Add a directory to the TreeList
		/// </summary>
		/// <param name="dir">Directory information.</param>
		private void addDirectory(DC_DirectoryInformation dir){
			string dir_type = (dir.bool_public)? "directory" : "directory_private";
			TreeNode node = node_root.Nodes.Add(dir.id, dir.name, dir_type, dir_type);
			node.Tag = dir;
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


		private void frmManage_Load(object sender, EventArgs e) {
			_treDirectories.ContextMenu = _contextMenuDirectory;
			_lstFiles.ContextMenu = _contextMenuFiles;

			string server_name = Client.server_info.server_name;
			_imlDirectories.Images.Add("directory", Properties.Resources.asset_blue_16_ns);
			_imlDirectories.Images.Add("directory_private", Properties.Resources.asset_grey_16_ns);
			_imlDirectories.Images.Add("server", Properties.Resources.circle_blue_16_ns);
			_imlDirectories.Images.Add("updating", Properties.Resources.lightening_16_ns);

			_imlFiles.Images.Add("file", Properties.Resources.square_green_16_ns);
			_imlFiles.Images.Add("loading", Properties.Resources.lightening_16_ns);


			if(server_name == "") {
				server_name = "Server";
			}

			connector.callServerMethod("Files:directoryList");
			_lblStatusText.Text = "Loading directories...";

			node_root = _treDirectories.Nodes.Add("root", server_name, "server", "server");
			updatingDirectory(node_root);
			node_root.Expand();


		}

		private void _treDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			if(last_selected == e.Node && e.Button == MouseButtons.Right)
				return;

			if(e.Button == MouseButtons.Right)
				_treDirectories.SelectedNode = e.Node;
		}
		

		private void _cmiFilesCopyLinks_Click(object sender, EventArgs e) {
			StringBuilder clip_text = new StringBuilder();
			DC_FileInformation file;

			foreach(ListViewItem item in _lstFiles.SelectedItems) {
				file = item.Tag as DC_FileInformation;
				clip_text.Append(Client.server_info.server_url);
				clip_text.Append("/");
				clip_text.Append(file.url_id);

				if(_lstFiles.SelectedItems.Count > 1)
					clip_text.Append("\r\n");
			}

			// No need to over write the user's clipboard if there is nothing to copy.
			if(clip_text.Length > 0) {
				Clipboard.SetText(clip_text.ToString());
			}
		}

		private void _cmiFilesOpenLinks_Click(object sender, EventArgs e) {
			if(_lstFiles.SelectedItems.Count > 6) {
				DialogResult result = MessageBox.Show("Are you sure you want to open " + _lstFiles.SelectedItems.Count.ToString() + " files?", "Confirm Open", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if(result != DialogResult.OK)
					return;
			}
			DC_FileInformation file;

			foreach(ListViewItem item in _lstFiles.SelectedItems) {
				file = item.Tag as DC_FileInformation;
				System.Diagnostics.Process.Start(Client.server_info.server_url + "/" + file.url_id);
			}
		}

		private void _miNewDirectory_Click(object sender, EventArgs e) {
			TreeNode new_directory = node_root.Nodes.Add("", "New Directory", "directory_private", "directory_private");
			new_directory.BeginEdit();
		}

		private void _contextMenuDirectory_Popup(object sender, EventArgs e) {

			var directory = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			bool is_dir = (directory == null)? false : true;

			_cmiDirectoryCopyDirectoryUrl.Visible = is_dir;
			_cmiDirectoryBreak1.Visible = is_dir;
			_cmiDirectoryDelete.Visible = is_dir;
			_cmiDirectoryRename.Visible = is_dir;
			_cmiDirectoryMakePrivate.Visible = is_dir;
			_cmiDirectoryMakePublic.Visible = is_dir;
			_cmiOpenDirectory.Visible = is_dir;

			// if this is not a dir, no need to do dir checks.
			if(is_dir == false)
				return;

			_cmiDirectoryMakePrivate.Visible = directory.bool_public;
			_cmiDirectoryMakePublic.Visible = !directory.bool_public;

		}

		private void _cmiCreateDirectory_Click(object sender, EventArgs e) {
			_miNewDirectory_Click(sender, e);
		}

		private void _cmiDirectoryRename_Click(object sender, EventArgs e) {
			_treDirectories.SelectedNode.BeginEdit();
		}

		private void _treDirectories_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
			if(e.Node.Name == "root" || e.Node.Name == "directory_unsorted") {
				e.CancelEdit = true;
				return;
			}
		}


		private void _treDirectories_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
			if(e.Label == null)
				return;

			if(e.Node.Tag == null) {
				connector.callServerMethod("Files:directoryCreate", e.Label);
				e.Node.Remove();
				_lblStatusText.Text = "Creating directory " + e.Label;

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

				}else if(e.Label.Length > 128){
					MessageBox.Show("Directory name has to be less than 128 characters.", "Directory Name Change", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					e.CancelEdit = true;
					return;
				}

				updatingDirectory(e.Node);
				connector.callServerMethod("Files:directoryRename", dir.url_id, e.Label);
				_lblStatusText.Text = "Renaming Directory " + dir.name;
			}

		}

		private void _miMainMenuClose_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void _treDirectories_DragEnter(object sender, DragEventArgs e) {
			if(dragging_files.Count > 0) {
				e.Effect = DragDropEffects.Move;

			} else {
				string[] new_files = e.Data.GetData(typeof(string[])) as string[];
				if(new_files != null) {
					// TODO: Handle new files to upload
				}
			}

		}

		private void _lstFiles_ItemDrag(object sender, ItemDragEventArgs e) {
			dragging_files.Clear();
			foreach(ListViewItem item in _lstFiles.SelectedItems){
				dragging_files.Add(item);
			}

			DoDragDrop(true, DragDropEffects.Move);
		}

		private void _treDirectories_DragDrop(object sender, DragEventArgs e) {
			List<string> move_url_ids = new List<string>();
			TreeNode node = _treDirectories.GetNodeAt(this.PointToClient(Cursor.Position));
			if(node == null) {
				dragging_files.Clear();
				return;
			}
			DC_DirectoryInformation dir = node.Tag as DC_DirectoryInformation;

			if(dir == null && node.Name == "directory_unsorted"){
				dir = new DC_DirectoryInformation() {
					url_id = "0"
				};
			}
			
			_lblStatusText.Text = "Moving (" + dragging_files.Count.ToString() + ") File(s) into directory " + node.Text;

			foreach(ListViewItem item in dragging_files){
				move_url_ids.Add(((DC_FileInformation)item.Tag).url_id); // Potentially a null reference error...
				item.Remove();
			}
			dragging_files.Clear();

			connector.callServerMethod("Files:move", move_url_ids.ToArray(), dir.url_id);
		}

		private void _lstFiles_KeyDown(object sender, KeyEventArgs e) {
			if(e.Modifiers == Keys.Control && e.KeyCode == Keys.A) {
				foreach(ListViewItem item in _lstFiles.Items) {
					item.Selected = true;
				}

			}
		}

		private void _cmiDeleteFiles_Click(object sender, EventArgs e) {
			
			List<string> delete_list = new List<string>();
			DC_FileInformation file;
			foreach(ListViewItem item in _lstFiles.SelectedItems) {
				if(item.Tag == null)
					continue;

				file = item.Tag as DC_FileInformation;
				if(file == null) // Should never happen.
					continue;
				delete_list.Add(file.url_id);
				item.Remove();
			}

			if(delete_list.Count == 0)
				return;

			connector.callServerMethod("Files:delete", delete_list.ToArray());
			_lblStatusText.Text = "Deleting File(s)...";
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
			_lblStatusText.Text = "Deleting directory...";
		}

		private void _miRefreshDirectories_Click(object sender, EventArgs e) {
			connector.callServerMethod("Files:directoryList");
			_lblStatusText.Text = "Refreshing directories...";
			node_root.Nodes.Clear();
			updatingDirectory(node_root);
		}

		private void _treDirectories_AfterSelect(object sender, TreeViewEventArgs e) {
			if(dragging_files.Count > 0)
				return;

			last_selected = e.Node;
			_lstFiles.Items.Clear();
			if(e.Node.Name == "directory_unsorted") {
				connector.callServerMethod("Files:directoryFiles", "0");

			} else {
				DC_DirectoryInformation dir = e.Node.Tag as DC_DirectoryInformation;
				if(dir == null)
					return;

				connector.callServerMethod("Files:directoryFiles", dir.url_id);
			}
			_lstFiles.Items.Add("loading_text", "Loading...", "loading");
			_lblStatusText.Text = "Loading directory " + e.Node.Text + "...";
		}

		private void _treDirectories_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e) {
			if(dragging_files.Count > 0)
				e.Node.BackColor = SystemColors.Highlight;
		}

		private void _lstFiles_DragDrop(object sender, DragEventArgs e) {
			dragging_files.Clear();
		}

		private void _cmiDirectoryMakePrivate_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;
			var set_prop = new Dictionary<string, bool>(){
				{"is_public", false}
			};
			connector.callServerMethod("Files:directorySetProperties", dir.url_id, set_prop);
			_lblStatusText.Text = "Setting directory " + dir.name + " to Private";
			_treDirectories.SelectedNode.ImageKey = "directory_private";
			_treDirectories.SelectedNode.SelectedImageKey = "directory_private";
			dir.bool_public = false;
		}

		private void _cmiDirectoryMakePublic_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;
			var set_prop = new Dictionary<string, bool>(){
				{"is_public", true}
			};
			connector.callServerMethod("Files:directorySetProperties", dir.url_id, set_prop);
			_lblStatusText.Text = "Setting directory " + dir.name + " to Public";
			_treDirectories.SelectedNode.ImageKey = "directory";
			_treDirectories.SelectedNode.SelectedImageKey = "directory";
			dir.bool_public = true;
		}

		private void _cmiDirectoryCopyDirectoryUrl_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;

			Clipboard.SetText(Client.server_info.server_url + "/" + dir.url_id);
		}

		private void _cmiOpenDirectory_Click(object sender, EventArgs e) {
			DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
			if(dir == null)
				return;

			System.Diagnostics.Process.Start(Client.server_info.server_url + "/" + dir.url_id);
		}
	}
}
