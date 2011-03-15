using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using dtxCore;

namespace dtxUpload {
	public class DtxHttpRequest {
		//private NetworkStream stream;
		private Socket server_socket;
		public WebHeaderCollection headers = new WebHeaderCollection();
		public Dictionary<string, string> post_data = new Dictionary<string,string>();
		private bool headers_sent = false;
        public string method = "GET";
		public Uri address;
		public int receive_timeout = 30000;
		public int send_timeout = 30000;

		private string nl = "\r\n";

		public DtxHttpRequest(Uri address){
			this.address = address;
			server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server_socket.Blocking = true;
			server_socket.ReceiveTimeout = receive_timeout;
			server_socket.SendTimeout = send_timeout;
			server_socket.NoDelay = true;

			server_socket.Connect(address.Host, address.Port);
			//stream = new NetworkStream(server_socket, true);

			// Create default headers.
			headers.Add(HttpRequestHeader.UserAgent, "DtronixCore/1.0");
			headers.Add(HttpRequestHeader.Host, "DtronixCore");
			headers.Add(HttpRequestHeader.Accept, "text/html");
			headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8");
			headers.Add(HttpRequestHeader.KeepAlive, "115");
			headers.Add(HttpRequestHeader.Connection, "keep-alive");

			/*
			
POST http://nfgaming.com/forum/index.php?action=post2;start=0;board=10 HTTP/1.1
Host: nfgaming.com
User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15
Accept: text/html
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip,deflate
Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7
Keep-Alive: 115
Connection: keep-alive
Referer: http://nfgaming.com/forum/index.php?action=post;board=10.0
Cookie: SMFCookie463=a%3A4%3A%7Bi%3A0%3Bs%3A1%3A%224%22%3Bi%3A1%3Bs%3A40%3A%223dc9175373a05b1293c3d29b75e542d1549a8048%22%3Bi%3A2%3Bi%3A1488168095%3Bi%3A3%3Bi%3A3%3B%7D; PHPSESSID=41436971bd691a4d10d105ededf360b1
Content-Type: multipart/form-data; boundary=---------------------------288301069040
Content-Length: 110458
			 */
			//headers.Add(
		}

		private void writeHeaders(){
			StringBuilder sb = new StringBuilder();

			sb.Append(method.ToUpper());
			sb.Append(" ");
			sb.Append(address.ToString());
			sb.Append(" HTTP/1.1");
			sb.Append(nl);

			foreach (string header in headers.AllKeys) {
				sb.Append(header);
				sb.Append(": ");
				sb.Append(headers[header]);
				sb.Append(nl);
			}
			sb.Append(nl);

			string sbsaf = sb.ToString();

			byte[] header_array = Encoding.UTF8.GetBytes(sb.ToString());
			server_socket.Send(header_array);
			//stream.Write(header_array, 0, header_array.Length);

			headers_sent = true;
		}

		public void write(byte[] buffer, int offset, int size){
			// Ensure headers have already been sent.
			if(!headers_sent)
				writeHeaders();

			//stream.Write(buffer, offset, size);
			int sent = server_socket.Send(buffer, offset, size, SocketFlags.None);
		}

		public DtxHttpResponse getResponse() {
			// Ensure headers have already been sent.
			if(!headers_sent)
				writeHeaders();

			return new DtxHttpResponse(server_socket);
		}

		public void close(){
			server_socket.Close();
			//stream.Close();
		}
	}

	public class DtxHttpResponse {
		private Socket server_socket;
		public WebHeaderCollection headers = new WebHeaderCollection();
		public HttpStatusCode status_code;
		private bool first_read = true;
		private byte[] internal_buffer;
		private byte[] http_sep = Encoding.UTF8.GetBytes("\r\n\r\n");

		//public DtxHttpResponse(NetworkStream stream) {
		public DtxHttpResponse(Socket socket) {
			server_socket = socket;
			//this.stream = stream;
			readHeaders();
		}

		private void readHeaders() {
			string line;
			int index = -1,
				status_int = -1,
				offset = 0,
				read = 0;

			byte[] header_buffer = new byte[1024 * 16];

			while ((read = server_socket.Receive(header_buffer, offset, 64, SocketFlags.None)) > 0) {
				offset += read;

				if ((index = Utilities.byteIndexOf(header_buffer, http_sep)) == -1) {
					continue;
				} else {
					break;
				}
			}

			if(index == -1){
				throw new Exception("Server's header exceeded the maxumum 16 KB limit.");
			}

			internal_buffer = new byte[offset - index - 4];
			Array.Copy(header_buffer, index + 4, internal_buffer, 0, internal_buffer.Length);

			string header = Encoding.UTF8.GetString(header_buffer, 0, index);
			string[] header_lines = header.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			index = header_lines[0].IndexOf(' ');
			string status = header_lines[0].Substring(index + 1, 3);


			try {
				status_int = int.Parse(status);
				status_code = (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), status_int);
			} catch {
				throw new Exception("Server did not respond with non-standard status code: "+ status_int.ToString());
			}

			for (int i = 1; i < header_lines.Length; i++) {
				index = header_lines[i].IndexOf(':');
				headers.Add(header_lines[i].Substring(0, index), header_lines[i].Substring(index +2));
			}

			return;
		}


		public string readString() {
			int length = 0;
			byte[] buffer = new byte[512];
			StringBuilder sb = new StringBuilder();

			//stream.

			length = read(buffer, 0, buffer.Length);
			sb.Append(Encoding.UTF8.GetString(buffer, 0, length));
			while((length = read(buffer, 0, buffer.Length)) > 0) {
				sb.Append(Encoding.UTF8.GetString(buffer, 0, length));
			}

			return sb.ToString().Trim('\n','\r');

		}

		public int read(byte[] buffer, int offset, int count) {

			// If we have buffered content, flush that first.
			if (internal_buffer != null) {
				// Make sure we don't try to copy too much.
				int read_length = Math.Min(count, internal_buffer.Length);
				Array.Copy(internal_buffer, 0, buffer, offset, read_length);

				if (read_length < internal_buffer.Length) {
					byte[] new_internal_buffer = new byte[internal_buffer.Length - read_length];
					Array.Copy(internal_buffer, read_length, new_internal_buffer, 0, new_internal_buffer.Length);
				} else {
					internal_buffer = null;
				}

				return read_length;
			}

			// If we do not have a buffer left, just output the information
			return server_socket.Receive(buffer, offset, count, SocketFlags.None);

		}

		public void close() {
			server_socket.Close();
		}

		public int readByte() {
			//return stream.ReadByte();
			return -1;
		}
	}
}
