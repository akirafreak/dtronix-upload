using System;
using System.Collections.Generic;
using System.Text;
using dtxCore;
using dtxCore.Dokan;
using dtxCore.Forms;
using System.Threading;

namespace dtxUpload {

	/// <summary>
	/// Static class to aid in the accessing of classes and methods.
	/// </summary>
	public static class Client {
		public static frmLogin form_Login;
		public static frmQuickUpload form_QuickUpload;
		public static frmConsole form_Console;

		public static Config config;

		public static DC_ServerInformation server_info = new DC_ServerInformation();
		public static DC_UserInformation user_info = new DC_UserInformation();

		public static Thread drive_mount_thread;
	}
}
