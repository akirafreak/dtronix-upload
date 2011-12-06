using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DtxUpload {
	[Serializable()]
	public class ClientActionsInternalException : Exception {
		public ClientActionsInternalException() : base() { }
		public ClientActionsInternalException(string message) : base(message) { }
		public ClientActionsInternalException(string message, Exception e) : base(message, e) { }

		protected ClientActionsInternalException(SerializationInfo info, StreamingContext context) { }
	}
}
