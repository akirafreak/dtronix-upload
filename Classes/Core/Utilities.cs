using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Core {

	public static class Utilities {
		/// <summary>
		/// Creates a random hash with specified length.
		/// </summary>
		/// <param name="length">Length of string to return.</param>
		/// <param name="uppercase">True: Allow uppercase characters to be present in the hash.  False: Just use lowercase letters and numbers.</param>
		/// <returns>Hash with specified length.</returns>
		public static string createHash(int length, bool uppercase) {
			int max_value = 35;
			char[] buffer_return = new char[length];
			Random random_seed = new Random();
			char[] character_array = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
			
			if(uppercase) {
				max_value = character_array.Length;
			}

			for(int i = 0; i < length; i++) {
				buffer_return[i] = character_array[random_seed.Next(0, max_value)];
			}
			return new string(buffer_return);
		}

		private static UTF8Encoding utf8_encoding = new UTF8Encoding();

		public static string base64Decode(string input_string) {
			Decoder utf8Decode = utf8_encoding.GetDecoder();
			byte[] to_decode_byte = Convert.FromBase64String(input_string);
			char[] decoded_char = new char[utf8Decode.GetCharCount(to_decode_byte, 0, to_decode_byte.Length)];
			utf8Decode.GetChars(to_decode_byte, 0, to_decode_byte.Length, decoded_char, 0);
			return new string(decoded_char);
		}

		public static string base64Encode(string input_string) {
			return Convert.ToBase64String(utf8_encoding.GetBytes(input_string));
		}


		public static string md5Sum(string str) {
			byte[] unicodeText = Encoding.UTF8.GetBytes(str);

			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < result.Length; i++) {
				sb.Append(result[i].ToString("X2"));
			}

			return sb.ToString();
		}

		private const long size_kylobyte = 1024;
		private const long size_megabyte = 1024 * size_kylobyte;
		private const long size_gigabyte = 1024 * size_megabyte;



		public static string formattedSize(long input_size) {
			if(input_size >= size_gigabyte) {
				return ((decimal)input_size / (decimal)size_gigabyte).ToString("F") + " GB";

			}else if(input_size >= size_megabyte){
				return ((double)input_size / (double)size_megabyte).ToString("F") + " MB";

			} else if(input_size >= (decimal)size_kylobyte) {
				return ((double)input_size / (double)size_kylobyte).ToString("F0") + " KB";

			} else {
				return input_size + " B";
			}
		}

	}

	/// <summary>
	/// Class to aid in the quick creation of encoded strings.
	/// </summary>
	public class TripleDESCrypto {

		private TripleDESCryptoServiceProvider crypto_tdes = new TripleDESCryptoServiceProvider();
		private UTF8Encoding utf8 = new UTF8Encoding();
		private object lock_object = new object();

		public TripleDESCrypto(string IV, string Key) {
			this.IV = IV;
			this.Key = Key;
		}

		/// <summary>
		/// (Set Only) IV for cypher.  Truncates after 8 bytes and padds with 0's.
		/// </summary>
		public string IV {
			set {
				byte[] buffer = new byte[crypto_tdes.IV.Length];
				byte[] bytes = utf8.GetBytes(value);
				Array.Copy(bytes, buffer, (bytes.Length > crypto_tdes.IV.Length) ? crypto_tdes.IV.Length : bytes.Length);
				crypto_tdes.IV = buffer;
			}
		}

		/// <summary>
		/// (Set Only) Key for cypher.  Truncates after 24 bytes and padds with 0's.
		/// </summary>
		public string Key {
			set {
				byte[] buffer = new byte[crypto_tdes.Key.Length];
				byte[] bytes = utf8.GetBytes(value);
				Array.Copy(bytes, buffer, (bytes.Length > crypto_tdes.Key.Length) ? crypto_tdes.Key.Length : bytes.Length);
				crypto_tdes.Key = buffer;
			}
		}

		/// <summary>
		/// Encrypt a string of utf8 characters into a base64 string of characters.
		/// </summary>
		/// <param name="value">String to encode.</param>
		/// <returns>Base64 string of characters.</returns>
		public string encrypt(string value) {
			lock (lock_object) {
				MemoryStream memory_stream = new MemoryStream();
				byte[] input = utf8.GetBytes(value);
				byte[] buffer = new byte[512];
				List<byte[]> byte_list = new List<byte[]>();
				byte[] output_buffer;
				int buffer_len;

				memory_stream.Write(input, 0, input.Length);
				memory_stream.Position = 0;

				CryptoStream cs_write = new CryptoStream(memory_stream, crypto_tdes.CreateEncryptor(), CryptoStreamMode.Read);

				while ((buffer_len = cs_write.Read(buffer, 0, buffer.Length)) > 0) {
					byte[] temp_buffer = new byte[buffer_len];
					Array.Copy(buffer, temp_buffer, buffer_len);
					byte_list.Add(temp_buffer);
				}
				memory_stream.Close();
				cs_write.Close();

				int total_bytes = (byte_list.Count > 1) ? (((byte_list.Count - 1) * buffer.Length) + byte_list[byte_list.Count - 1].Length) : byte_list[0].Length;
				output_buffer = new byte[total_bytes];

				for (int i = 0; i < byte_list.Count; i++) {
					Array.Copy(byte_list[i], 0, output_buffer, (i * buffer.Length), byte_list[i].Length);
				}

				return Convert.ToBase64String(output_buffer);
			}
		}

		/// <summary>
		/// Decrypt a string of Base64 characters to a string of utf8 characters.
		/// </summary>
		/// <param name="value">Base64 string.</param>
		/// <returns>utf8 decoded string.</returns>
		public string decrypt(string value) {
			lock (lock_object) {
				MemoryStream memory_stream = new MemoryStream();
				byte[] input = Convert.FromBase64String(value);
				byte[] buffer = new byte[512];
				List<byte[]> byte_list = new List<byte[]>();
				byte[] output_buffer;
				int buffer_len;

				memory_stream.Write(input, 0, input.Length);
				memory_stream.Position = 0;

				CryptoStream cs_write = new CryptoStream(memory_stream, crypto_tdes.CreateDecryptor(), CryptoStreamMode.Read);

				while ((buffer_len = cs_write.Read(buffer, 0, buffer.Length)) > 0) {
					byte[] temp_buffer = new byte[buffer_len];
					Array.Copy(buffer, temp_buffer, buffer_len);
					byte_list.Add(temp_buffer);
				}

				memory_stream.Close();
				cs_write.Close();

				int total_bytes = (byte_list.Count > 1) ? (((byte_list.Count - 1) * buffer.Length) + byte_list[byte_list.Count - 1].Length) : byte_list[0].Length;
				output_buffer = new byte[total_bytes];

				for (int i = 0; i < byte_list.Count; i++) {
					Array.Copy(byte_list[i], 0, output_buffer, (i * buffer.Length), byte_list[i].Length);
				}

				return utf8.GetString(output_buffer);
			}
		}
	}

}
