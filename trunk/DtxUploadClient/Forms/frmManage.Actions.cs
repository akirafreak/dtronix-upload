using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DtxUpload {
	public partial class ClientActions {

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

	}
}
