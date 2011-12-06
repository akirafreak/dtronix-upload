using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DtxUpload {
	public partial class ClientActions {
		public void directory_list(string input) {
			var control = getTag<DirectoryTreeList>();
			var directories = deserializeJson<DC_DirectoryInformation[]>(input);

			control.Invoke((MethodInvoker)delegate {
				control.directoryList(directories);
			});
		}


		public void directory_create_confirmation(string input) {
			var control = getTag<DirectoryTreeList>();
			var dir = deserializeJson<DC_DirectoryInformation>(input);

			control.Invoke((MethodInvoker)delegate {
				control.directoryCreateConfirmation(dir);
			});
		}

		public void directory_create_failure() {
			var control = getTag<DirectoryTreeList>();
			control.Invoke((MethodInvoker)control.directoryCreateFailure);
		}


		public void directory_rename_confirmation(string input) {
			var control = getTag<DirectoryTreeList>();
			string[] change_info = deserializeJson<string[]>(input);

			control.Invoke((MethodInvoker)delegate {
				control.directoryRenameConfirmation(change_info[0], change_info[1]);
			});
		}

		public void directory_delete_confirmation(string url_id) {
			var control = getTag<DirectoryTreeList>();

			control.Invoke((MethodInvoker)delegate {
				control.directoryDeleteConfirmation(url_id);
			});
		}

		public void directory_delete_failure() {
			var control = getTag<DirectoryTreeList>();
			control.Invoke((MethodInvoker)control.directoryDeleteFailure);
		}

		public void directory_set_properties_failure() {
			var control = getTag<DirectoryTreeList>();
			control.Invoke((MethodInvoker)control.directorySetPropertiesFailure);
		}

		public void directory_set_properties_confirmation() {
			var control = getTag<DirectoryTreeList>();
			control.Invoke((MethodInvoker)control.directorySetPropertiesConfirmation);
		}
	}
}
