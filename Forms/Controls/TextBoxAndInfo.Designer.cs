namespace dtxUpload {
	partial class TextBoxAndInfo {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._txtInput = new System.Windows.Forms.TextBox();
			this._lblInformation = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _txtInput
			// 
			this._txtInput.Dock = System.Windows.Forms.DockStyle.Top;
			this._txtInput.Location = new System.Drawing.Point(0, 0);
			this._txtInput.Name = "_txtInput";
			this._txtInput.Size = new System.Drawing.Size(193, 20);
			this._txtInput.TabIndex = 0;
			this._txtInput.TextChanged += new System.EventHandler(this._txtInput_TextChanged);
			// 
			// _lblInformation
			// 
			this._lblInformation.AutoSize = true;
			this._lblInformation.Location = new System.Drawing.Point(3, 20);
			this._lblInformation.Name = "_lblInformation";
			this._lblInformation.Size = new System.Drawing.Size(0, 13);
			this._lblInformation.TabIndex = 1;
			// 
			// TextBoxAndInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._txtInput);
			this.Controls.Add(this._lblInformation);
			this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.Name = "TextBoxAndInfo";
			this.Size = new System.Drawing.Size(193, 35);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _txtInput;
		private System.Windows.Forms.Label _lblInformation;
	}
}
