namespace ByteOrderTest
{
	partial class FrmByteOrderTest
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlTool = new System.Windows.Forms.Panel();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.btnTest1 = new System.Windows.Forms.Button();
			this.pnlTool.SuspendLayout();
			this.pnlInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTool
			// 
			this.pnlTool.Controls.Add(this.btnTest1);
			this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTool.Location = new System.Drawing.Point(0, 0);
			this.pnlTool.Name = "pnlTool";
			this.pnlTool.Size = new System.Drawing.Size(584, 48);
			this.pnlTool.TabIndex = 0;
			// 
			// pnlInfo
			// 
			this.pnlInfo.Controls.Add(this.txtLog);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 48);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(584, 314);
			this.pnlInfo.TabIndex = 1;
			// 
			// txtLog
			// 
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(584, 314);
			this.txtLog.TabIndex = 0;
			this.txtLog.WordWrap = false;
			// 
			// btnTest1
			// 
			this.btnTest1.Location = new System.Drawing.Point(12, 12);
			this.btnTest1.Name = "btnTest1";
			this.btnTest1.Size = new System.Drawing.Size(75, 23);
			this.btnTest1.TabIndex = 0;
			this.btnTest1.Text = "Test1";
			this.btnTest1.UseVisualStyleBackColor = true;
			this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
			// 
			// FrmByteOrderTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 362);
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlTool);
			this.Name = "FrmByteOrderTest";
			this.Text = "ByteOrderTest";
			this.pnlTool.ResumeLayout(false);
			this.pnlInfo.ResumeLayout(false);
			this.pnlInfo.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTool;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.Button btnTest1;
	}
}

