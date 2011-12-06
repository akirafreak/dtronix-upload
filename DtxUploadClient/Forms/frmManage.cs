using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using dtxCore;

namespace DtxUpload {
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



		

		public void fileDeleteConfirmation() {
			_lblStatusText.Text = "File(s) have been deleted.";
		}

		public void fileDeleteFailure() {
			_lblStatusText.Text = "Error while deleteting files.  Please try again.";
		}


#endregion

		private void frmManage_Load(object sender, EventArgs e) {
			_lstFiles.ContextMenu = _contextMenuFiles;

			_imlFiles.Images.Add("file", Properties.Resources.square_green_16_ns);
			_imlFiles.Images.Add("loading", Properties.Resources.lightening_16_ns);

			_dtlDirectories.directory_selected += new DirectoryTreeListDirectorySelected(_dtlDirectories_directory_selected);
			_dtlDirectories.status_update += new DirectoryTreeListStatusDelegate(_dtlDirectories_status_update);

			_dtlDirectories.refreshDirectories();
		}

		private void _dtlDirectories_status_update(string text, DirectoryTreeListStatusEnum status) {
			_lblStatusText.Text = text;
		}

		private void _dtlDirectories_directory_selected(DC_DirectoryInformation directory) {
			_lstFiles.Items.Clear();
			_lstFiles.Items.Add("Loading", "loading");
			connector.callServerMethod("Files:directoryFiles", directory.url_id);
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



		private void _miMainMenuClose_Click(object sender, EventArgs e) {
			this.Close();
		}


		private void _lstFiles_ItemDrag(object sender, ItemDragEventArgs e) {
			dragging_files.Clear();
			foreach(ListViewItem item in _lstFiles.SelectedItems){
				dragging_files.Add(item);
			}

			DoDragDrop(true, DragDropEffects.Move);
		}


		private void _lstFiles_KeyDown(object sender, KeyEventArgs e) {
			if(e.Modifiers == Keys.Control && e.KeyCode == Keys.A) {
				foreach(ListViewItem item in _lstFiles.Items) {
					item.Selected = true;
				}

			} else if(e.KeyCode == Keys.Delete && DialogResult.Yes == MessageBox.Show("Are you sure you want to permanently delete these files?", "Delete Files", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) {
				_cmiDeleteFiles_Click(sender, new EventArgs());

			} else if(e.KeyCode == Keys.F5) {
				// Reselect the same node to refresh the view.
				//TreeNode current_node = _treDirectories.SelectedNode;
				//_treDirectories.SelectedNode = node_root;
				//_treDirectories.SelectedNode = current_node;
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

		private void _miRefreshDirectories_Click(object sender, EventArgs e) {
			_dtlDirectories.refreshDirectories();
		}

		//private void _cmiOpenDirectory_Click(object sender, EventArgs e) {
		//    DC_DirectoryInformation dir = _treDirectories.SelectedNode.Tag as DC_DirectoryInformation;
		//    if(dir == null)
		//        return;

		//    System.Diagnostics.Process.Start(Client.server_info.server_url + "/" + dir.url_id);
		//}

		private void frmManage_FormClosed(object sender, FormClosedEventArgs e) {
			// Remove the last reference to this form to allow it to disappear.
			Client.form_Manage = null;
		}

		private void _lstFiles_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.All;
		}

		private void _lstFiles_DragDrop(object sender, DragEventArgs e) {
			// Clear the internal list of files being drug.
			dragging_files.Clear();



		}

		private void _dtlDirectories_DragEnter(object sender, DragEventArgs e) {
			if(dragging_files.Count > 0) {
				e.Effect = DragDropEffects.Move;

			} else {
				string[] new_files = e.Data.GetData(typeof(string[])) as string[];
				if(new_files != null) {
					// TODO: Handle new files to upload
				}
			}
		}

		private void _dtlDirectories_DragDrop(object sender, DragEventArgs e) {
			List<string> move_url_ids = new List<string>();

			DC_DirectoryInformation dir = _dtlDirectories.getDirectoryAtCursor();

			if(dir == null) {
				dragging_files.Clear();
				return;
			}

			_lblStatusText.Text = "Moving (" + dragging_files.Count.ToString() + ") File(s) into directory " + dir.name;

			foreach(ListViewItem item in dragging_files) {
				move_url_ids.Add(((DC_FileInformation)item.Tag).url_id); // Potentially a null reference error...
				item.Remove();
			}
			dragging_files.Clear();

			connector.callServerMethod("Files:move", move_url_ids.ToArray(), dir.url_id);
		}
	}
}
