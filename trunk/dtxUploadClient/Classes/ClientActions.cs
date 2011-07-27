using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dtxCore.Json;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace dtxUpload {
	public partial class ClientActions {

		private ServerConnector connector;
		private static List<server_error_class> server_errors = new List<server_error_class>();

		public ClientActions(ServerConnector connector) {
			this.connector = connector;
		}

		public void debug(string input) {
			try {
				StringBuilder sb = new StringBuilder();
				JsonReader jr = new JsonReader(input);
				JsonWriter jw = new JsonWriter(sb);

				jw.PrettyPrint = true;

				jw.Write(jr.Deserialize());

				Client.form_Login.Invoke((MethodInvoker)delegate {
					MessageBox.Show(sb.ToString());
				});

			} catch {
				Client.form_Login.Invoke((MethodInvoker)delegate {
					MessageBox.Show(input);
				});

			}
		}

		private class server_error_class{
			public string error_type = null;
			public string error_info = null;
			public string error_file = null;
			public string error_line = null;
		}

		public void server_error(string input) {
			JsonReader reader = new JsonReader(input);
			server_error_class info = reader.Deserialize<server_error_class>();
			server_errors.Add(info);

			Client.form_Login.Invoke((MethodInvoker)delegate {
				MessageBox.Show("The PHP server encountered an error.  Details: \n\n" + 
					"    Type: " + info.error_type + "\n" +
					"    Info: " + info.error_info + "\n" +
					"    File: " + info.error_file + "\n" +
					"    Line: " + info.error_line + "\n");
			});
		}

		public void error_client(string input) {
			Client.form_Login.Invoke((MethodInvoker)delegate {
				MessageBox.Show("The PHP could not recognize the requested method called.  Details: \n\n" + input);
			});
		}

		public void server_error_mysql(string input) {
			Client.form_Login.Invoke((MethodInvoker)delegate {
				MessageBox.Show("The MySQL server returned an error.  Details: \n\n" + input);
			});
		}


		private T deserializeJson<T>(string input) {
			try {
				JsonReader reader = new JsonReader(input);
				return reader.Deserialize<T>();
			} catch(Exception e) {
				Client.form_Login.Invoke((MethodInvoker)delegate {
					MessageBox.Show("The sent an unparseable respose.Details: \n\nSTART:\n" + input.Substring(0, (input.Length > 512) ? 512 : input.Length));
				});
				return default(T);
			}
		}
	}
}
