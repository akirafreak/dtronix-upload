using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dtxCore.Json;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace DtxUpload {
	public partial class ClientActions {

		/// <summary>
		/// User defined data associated with the class.
		/// </summary>
		private object tag;
		private ServerConnector connector;
		private static List<server_error_class> server_errors = new List<server_error_class>();

		/// <param name="connector">Associated connector class that will call the action's methods.</param>
		/// <param name="tag">Object that is associated with this instance of the class.</param>
		public ClientActions(ServerConnector connector, object tag) {
			this.tag = tag;
			this.connector = connector;
		}

		public void debug(string input) {
			try {
				StringBuilder sb = new StringBuilder();
				JsonReader jr = new JsonReader(input);
				JsonWriterSettings settings = new JsonWriterSettings() {
					PrettyPrint = true,
				};
				JsonWriter jw = new JsonWriter(sb, settings);

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

		/// <summary>
		/// Generic method to convert the tag associated with this Actions instance to the specified type.
		/// </summary>
		/// <typeparam name="T">Type to try to convert the tag to.</typeparam>
		/// <returns>Converted tag.</returns>
		private T getTag<T>() {
			try {
				return (T)tag;
			} catch(Exception e) {
				throw new ClientActionsInternalException("Unable to convert tag to desired type", e);
			}
		}


		private T deserializeJson<T>(string input) {
			try {
				JsonReader reader = new JsonReader(input);
				return reader.Deserialize<T>();

			} catch(Exception e) {
				throw new ClientActionsInternalException("Unable to convert input string into JSON data.", e);
			}
		}
	}
}
