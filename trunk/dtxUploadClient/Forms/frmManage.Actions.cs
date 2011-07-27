using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dtxUpload {
	public partial class ClientActions {

		public void directory_list(string input) {
			if(input == "false") {
				Client.form_Manage.Invoke((MethodInvoker)delegate {
					Client.form_Manage.directoryList(null);
				});
				return;
			}

			DC_DirectoryInformation[] directories = deserializeJson<DC_DirectoryInformation[]>(input);

			Client.form_Manage.Invoke((MethodInvoker)delegate {
				Client.form_Manage.directoryList(directories);
			});
		}

		public void directory_files(string input) {
			if(input != "false") {
				var files = deserializeJson<DC_FileInformation[]>(input);
				Client.form_Manage.Invoke((MethodInvoker)delegate {
					Client.form_Manage.directoryFiles(files);
				});
			} else {
				Client.form_Manage.Invoke((MethodInvoker)delegate {
					Client.form_Manage.directoryFiles(null);
				});
			}
		}

		public void directory_create_confirmation(string input) {
			var dir = deserializeJson<DC_DirectoryInformation>(input);

			Client.form_Manage.Invoke((MethodInvoker)delegate {
				Client.form_Manage.directoryCreateConfirmation(dir);
			});
		}

		public void directory_create_failure() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.directoryDeletePending);
		}

		public void directory_delete_successful(string directory) {
			string url_id = deserializeJson<string>(directory);

			Client.form_Manage.Invoke((MethodInvoker)delegate{
				Client.form_Manage.directoryDeleteConfirmation(url_id);
			});
		}

		public void directory_rename_confirmation(string input) {
			string[] change_info = deserializeJson<string[]>(input);

			Client.form_Manage.Invoke((MethodInvoker)delegate {
				Client.form_Manage.directoryRenameConfirmation(change_info[0], change_info[1]);
			});
		}

		public void file_move_confirmation(string input) {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.fileMoveConfirmation);
		}

		public void file_move_failure() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.fileMoveFailure);
		}

		public void file_delete_confirmation() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.fileDeleteConfirmation);
		}

		public void file_delete_failure() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.fileDeleteFailure);
		}

		public void directory_delete_confirmation() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.directoryDeleteConfirmation);
		}

		public void directory_delete_failure() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.directoryDeleteFailure);
		}

		public void directory_set_properties_failure() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.directorySetPropertiesFailure);
		}

		public void directory_set_properties_confirmation() {
			Client.form_Manage.Invoke((MethodInvoker)Client.form_Manage.directorySetPropertiesConfirmation);
		}
	}
}
