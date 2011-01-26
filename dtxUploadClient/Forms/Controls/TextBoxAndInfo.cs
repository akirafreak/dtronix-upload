using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace dtxUpload{
	public partial class TextBoxAndInfo : UserControl {
		public event EventHandler TextChangedBubble;

		public TextBoxAndInfo() {
			InitializeComponent();
		}

		public string Value {
			set {
				_txtInput.Text = value;
			}
			get {
				return _txtInput.Text;
			}
		}

		public string TextInfo {
			set {
				_lblInformation.Text = value;
			}
			get {
				return _lblInformation.Text;
			}
		}

		public Color InfoForeColor {
			set {
				_lblInformation.ForeColor = value;
			}
			get {
				return _lblInformation.ForeColor;
			}
		}

		public char PassChar {
			set {
				_txtInput.PasswordChar = value;
			}
			get {
				return _txtInput.PasswordChar;
			}
		}

		public void SetInfo(string text, Color color) {
			_lblInformation.Text = text;
			_lblInformation.ForeColor = color;
		}

		private void _txtInput_TextChanged(object sender, EventArgs e) {
			SetInfo("", Color.Black);
			if(TextChangedBubble != null) {
				TextChangedBubble(sender, e);
			}
		}
	}
}
