namespace dtxUpload {
	partial class frmConnector {
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
			this._dgvTransport = new System.Windows.Forms.DataGridView();
			this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.request_function = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.request_args = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.responce_raw = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.request_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.responce_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this._btnPing = new System.Windows.Forms.Button();
			this._btnVerifyLogin = new System.Windows.Forms.Button();
			this._btnClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this._dgvTransport)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _dgvTransport
			// 
			this._dgvTransport.AllowUserToAddRows = false;
			this._dgvTransport.AllowUserToDeleteRows = false;
			this._dgvTransport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._dgvTransport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.request_function,
            this.request_args,
            this.responce_raw,
            this.request_time,
            this.responce_time});
			this._dgvTransport.Dock = System.Windows.Forms.DockStyle.Fill;
			this._dgvTransport.Location = new System.Drawing.Point(3, 3);
			this._dgvTransport.Name = "_dgvTransport";
			this._dgvTransport.Size = new System.Drawing.Size(1534, 549);
			this._dgvTransport.TabIndex = 0;
			// 
			// id
			// 
			this.id.HeaderText = "ID";
			this.id.Name = "id";
			this.id.Width = 50;
			// 
			// request_function
			// 
			this.request_function.HeaderText = "Request Function";
			this.request_function.Name = "request_function";
			this.request_function.Width = 150;
			// 
			// request_args
			// 
			this.request_args.HeaderText = "Request Arguments";
			this.request_args.Name = "request_args";
			this.request_args.Width = 200;
			// 
			// responce_raw
			// 
			this.responce_raw.HeaderText = "Responce Raw";
			this.responce_raw.Name = "responce_raw";
			this.responce_raw.Width = 500;
			// 
			// request_time
			// 
			this.request_time.HeaderText = "Request Time";
			this.request_time.Name = "request_time";
			this.request_time.Width = 120;
			// 
			// responce_time
			// 
			this.responce_time.HeaderText = "Responce Time";
			this.responce_time.Name = "responce_time";
			this.responce_time.Width = 120;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this._dgvTransport, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1540, 587);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._btnClear);
			this.panel1.Controls.Add(this._btnVerifyLogin);
			this.panel1.Controls.Add(this._btnPing);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 558);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1534, 26);
			this.panel1.TabIndex = 1;
			// 
			// _btnPing
			// 
			this._btnPing.Location = new System.Drawing.Point(9, 0);
			this._btnPing.Name = "_btnPing";
			this._btnPing.Size = new System.Drawing.Size(75, 23);
			this._btnPing.TabIndex = 0;
			this._btnPing.Text = "Ping Server";
			this._btnPing.UseVisualStyleBackColor = true;
			this._btnPing.Click += new System.EventHandler(this._btnPing_Click);
			// 
			// _btnVerifyLogin
			// 
			this._btnVerifyLogin.Location = new System.Drawing.Point(90, 0);
			this._btnVerifyLogin.Name = "_btnVerifyLogin";
			this._btnVerifyLogin.Size = new System.Drawing.Size(75, 23);
			this._btnVerifyLogin.TabIndex = 1;
			this._btnVerifyLogin.Text = "Verify Login";
			this._btnVerifyLogin.UseVisualStyleBackColor = true;
			this._btnVerifyLogin.Click += new System.EventHandler(this._btnVerifyLogin_Click);
			// 
			// _btnClear
			// 
			this._btnClear.Location = new System.Drawing.Point(255, 0);
			this._btnClear.Name = "_btnClear";
			this._btnClear.Size = new System.Drawing.Size(75, 23);
			this._btnClear.TabIndex = 2;
			this._btnClear.Text = "Clear";
			this._btnClear.UseVisualStyleBackColor = true;
			this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
			// 
			// frmConnector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1540, 587);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmConnector";
			this.Text = "frmConnector";
			this.Load += new System.EventHandler(this.frmConnector_Load);
			((System.ComponentModel.ISupportInitialize)(this._dgvTransport)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView _dgvTransport;
		private System.Windows.Forms.DataGridViewTextBoxColumn id;
		private System.Windows.Forms.DataGridViewTextBoxColumn request_function;
		private System.Windows.Forms.DataGridViewTextBoxColumn request_args;
		private System.Windows.Forms.DataGridViewTextBoxColumn responce_raw;
		private System.Windows.Forms.DataGridViewTextBoxColumn request_time;
		private System.Windows.Forms.DataGridViewTextBoxColumn responce_time;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button _btnVerifyLogin;
		private System.Windows.Forms.Button _btnPing;
		private System.Windows.Forms.Button _btnClear;
	}
}