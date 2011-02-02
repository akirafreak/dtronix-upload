using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace dtxUpload {

	/// <summary>
	/// Static class to aid in the accessing of classes and methods.
	/// </summary>
	public static class Client {
		public static frmLogin form_Login;
		public static frmQuickUpload form_QuickUpload;

		public static Config config;

#if DEBUG
		public static frmConnector form_connector;
#endif

		public static DC_ServerInformation server_info = new DC_ServerInformation();
		public static DC_UserInformation user_info = new DC_UserInformation();
	}
}
