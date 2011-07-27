namespace dtxUpload.Forms {
	partial class frmAdminUsers {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdminUsers));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._txtSearch = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._lblTotalUsedSpace = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this._lbltotalFilesUploaded = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this._lblEmailVerification = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this._btnMaskPasswordInput = new System.Windows.Forms.Button();
			this._cmbAccountPermissions = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this._lblLastConnected = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this._txtEmailAddress = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this._txtPassword = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this._lblEditing = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 467);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(555, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(555, 467);
			this.splitContainer1.SplitterDistance = 165;
			this.splitContainer1.TabIndex = 2;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this._txtSearch, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(165, 467);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _txtSearch
			// 
			this._txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this._txtSearch.Location = new System.Drawing.Point(3, 443);
			this._txtSearch.Name = "_txtSearch";
			this._txtSearch.Size = new System.Drawing.Size(159, 20);
			this._txtSearch.TabIndex = 0;
			this._txtSearch.Text = "Search Users";
			// 
			// listView1
			// 
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.Location = new System.Drawing.Point(3, 3);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(159, 434);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Tile;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.label1);
			this.splitContainer2.Panel1.Controls.Add(this.label7);
			this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
			this.splitContainer2.Panel1.Controls.Add(this.comboBox2);
			this.splitContainer2.Panel1.Controls.Add(this._lblEditing);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.button1);
			this.splitContainer2.Panel2.Controls.Add(this.button2);
			this.splitContainer2.Size = new System.Drawing.Size(386, 467);
			this.splitContainer2.SplitterDistance = 431;
			this.splitContainer2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Editing:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 257);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(42, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Actions";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._lblTotalUsedSpace);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this._lbltotalFilesUploaded);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this._lblEmailVerification);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.comboBox1);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this._btnMaskPasswordInput);
			this.groupBox2.Controls.Add(this._cmbAccountPermissions);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this._lblLastConnected);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this._txtEmailAddress);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this._txtPassword);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(6, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(375, 208);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Account Information";
			// 
			// _lblTotalUsedSpace
			// 
			this._lblTotalUsedSpace.AutoSize = true;
			this._lblTotalUsedSpace.Location = new System.Drawing.Point(119, 182);
			this._lblTotalUsedSpace.Name = "_lblTotalUsedSpace";
			this._lblTotalUsedSpace.Size = new System.Drawing.Size(53, 13);
			this._lblTotalUsedSpace.TabIndex = 17;
			this._lblTotalUsedSpace.Text = "Unknown";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(6, 182);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(96, 13);
			this.label12.TabIndex = 16;
			this.label12.Text = "Total Used Space:";
			// 
			// _lbltotalFilesUploaded
			// 
			this._lbltotalFilesUploaded.AutoSize = true;
			this._lbltotalFilesUploaded.Location = new System.Drawing.Point(119, 165);
			this._lbltotalFilesUploaded.Name = "_lbltotalFilesUploaded";
			this._lbltotalFilesUploaded.Size = new System.Drawing.Size(53, 13);
			this._lbltotalFilesUploaded.TabIndex = 15;
			this._lbltotalFilesUploaded.Text = "Unknown";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 165);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(107, 13);
			this.label10.TabIndex = 14;
			this.label10.Text = "Total Files Uploaded:";
			// 
			// _lblEmailVerification
			// 
			this._lblEmailVerification.AutoSize = true;
			this._lblEmailVerification.ForeColor = System.Drawing.Color.Red;
			this._lblEmailVerification.Location = new System.Drawing.Point(119, 131);
			this._lblEmailVerification.Name = "_lblEmailVerification";
			this._lblEmailVerification.Size = new System.Drawing.Size(55, 13);
			this._lblEmailVerification.TabIndex = 5;
			this._lblEmailVerification.Text = "Unverified";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 131);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Email Verification:";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "Pending Activation",
            "Activated",
            "Banned"});
			this.comboBox1.Location = new System.Drawing.Point(122, 103);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(247, 21);
			this.comboBox1.TabIndex = 13;
			this.toolTip1.SetToolTip(this.comboBox1, "Select an existing permission set from the drop-down or click \"Create New Permiss" +
        "ions\".");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 106);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Account Status:";
			// 
			// _btnMaskPasswordInput
			// 
			this._btnMaskPasswordInput.Location = new System.Drawing.Point(338, 22);
			this._btnMaskPasswordInput.Name = "_btnMaskPasswordInput";
			this._btnMaskPasswordInput.Size = new System.Drawing.Size(31, 23);
			this._btnMaskPasswordInput.TabIndex = 11;
			this._btnMaskPasswordInput.Text = "***";
			this.toolTip1.SetToolTip(this._btnMaskPasswordInput, "Will toggle masking of password input for instances of when you are in a public p" +
        "lace.");
			this._btnMaskPasswordInput.UseVisualStyleBackColor = true;
			// 
			// _cmbAccountPermissions
			// 
			this._cmbAccountPermissions.FormattingEnabled = true;
			this._cmbAccountPermissions.Location = new System.Drawing.Point(122, 76);
			this._cmbAccountPermissions.Name = "_cmbAccountPermissions";
			this._cmbAccountPermissions.Size = new System.Drawing.Size(247, 21);
			this._cmbAccountPermissions.TabIndex = 10;
			this.toolTip1.SetToolTip(this._cmbAccountPermissions, "Select an existing permission set from the drop-down or click \"Create New Permiss" +
        "ions\".");
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 79);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(108, 13);
			this.label8.TabIndex = 9;
			this.label8.Text = "Account Permissions:";
			// 
			// _lblLastConnected
			// 
			this._lblLastConnected.AutoSize = true;
			this._lblLastConnected.Location = new System.Drawing.Point(119, 148);
			this._lblLastConnected.Name = "_lblLastConnected";
			this._lblLastConnected.Size = new System.Drawing.Size(53, 13);
			this._lblLastConnected.TabIndex = 7;
			this._lblLastConnected.Text = "Unknown";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 148);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Last Connected:";
			// 
			// _txtEmailAddress
			// 
			this._txtEmailAddress.Location = new System.Drawing.Point(122, 50);
			this._txtEmailAddress.Name = "_txtEmailAddress";
			this._txtEmailAddress.Size = new System.Drawing.Size(247, 20);
			this._txtEmailAddress.TabIndex = 3;
			this.toolTip1.SetToolTip(this._txtEmailAddress, "No verification will be done on the email once set here.");
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Email Address:";
			// 
			// _txtPassword
			// 
			this._txtPassword.Location = new System.Drawing.Point(122, 24);
			this._txtPassword.Name = "_txtPassword";
			this._txtPassword.Size = new System.Drawing.Size(210, 20);
			this._txtPassword.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Password:";
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "Delete user and all files.",
            "Delete all user\'s files."});
			this.comboBox2.Location = new System.Drawing.Point(60, 254);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(315, 21);
			this.comboBox2.TabIndex = 18;
			this.toolTip1.SetToolTip(this.comboBox2, "Select from a list an actions to apply to the user.");
			// 
			// _lblEditing
			// 
			this._lblEditing.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblEditing.Location = new System.Drawing.Point(64, 14);
			this._lblEditing.Name = "_lblEditing";
			this._lblEditing.Size = new System.Drawing.Size(313, 23);
			this._lblEditing.TabIndex = 3;
			this._lblEditing.Text = "-";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(302, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(221, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// frmAdminUsers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 489);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "frmAdminUsers";
			this.Text = "Administrate Users";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox _txtSearch;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label _lblTotalUsedSpace;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label _lbltotalFilesUploaded;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label _lblEmailVerification;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button _btnMaskPasswordInput;
		private System.Windows.Forms.ComboBox _cmbAccountPermissions;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label _lblLastConnected;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox _txtEmailAddress;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _txtPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label _lblEditing;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;

	}
}