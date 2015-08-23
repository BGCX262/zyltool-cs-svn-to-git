namespace zylToolTest
{
	partial class FrmTest_zylTool
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
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.mnuDebug = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDebugPixelFormat = new System.Windows.Forms.ToolStripMenuItem();
			this.btnTest = new System.Windows.Forms.Button();
			this.zpfBetterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnTest1 = new System.Windows.Forms.Button();
			this.pnlTool.SuspendLayout();
			this.pnlInfo.SuspendLayout();
			this.mnuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTool
			// 
			this.pnlTool.Controls.Add(this.btnTest1);
			this.pnlTool.Controls.Add(this.btnTest);
			this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTool.Location = new System.Drawing.Point(0, 25);
			this.pnlTool.Name = "pnlTool";
			this.pnlTool.Size = new System.Drawing.Size(584, 32);
			this.pnlTool.TabIndex = 0;
			// 
			// pnlInfo
			// 
			this.pnlInfo.Controls.Add(this.txtLog);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 57);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(584, 305);
			this.pnlInfo.TabIndex = 1;
			// 
			// txtLog
			// 
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(584, 305);
			this.txtLog.TabIndex = 0;
			this.txtLog.WordWrap = false;
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuDebug});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(584, 25);
			this.mnuMain.TabIndex = 2;
			this.mnuMain.Text = "menuStrip1";
			// 
			// mnuDebug
			// 
			this.mnuDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDebugPixelFormat});
			this.mnuDebug.Name = "mnuDebug";
			this.mnuDebug.Size = new System.Drawing.Size(59, 21);
			this.mnuDebug.Text = "&Debug";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(39, 21);
			this.mnuFile.Text = "&File";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Name = "mnuFileExit";
			this.mnuFileExit.Size = new System.Drawing.Size(152, 22);
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuDebugPixelFormat
			// 
			this.mnuDebugPixelFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zpfBetterToolStripMenuItem});
			this.mnuDebugPixelFormat.Name = "mnuDebugPixelFormat";
			this.mnuDebugPixelFormat.Size = new System.Drawing.Size(152, 22);
			this.mnuDebugPixelFormat.Text = "&PixelFormat";
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(3, 3);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 0;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// zpfBetterToolStripMenuItem
			// 
			this.zpfBetterToolStripMenuItem.Name = "zpfBetterToolStripMenuItem";
			this.zpfBetterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.zpfBetterToolStripMenuItem.Text = "zpfBetter";
			this.zpfBetterToolStripMenuItem.Click += new System.EventHandler(this.zpfBetterToolStripMenuItem_Click);
			// 
			// btnTest1
			// 
			this.btnTest1.Location = new System.Drawing.Point(84, 3);
			this.btnTest1.Name = "btnTest1";
			this.btnTest1.Size = new System.Drawing.Size(75, 23);
			this.btnTest1.TabIndex = 1;
			this.btnTest1.Text = "btnTest1";
			this.btnTest1.UseVisualStyleBackColor = true;
			this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
			// 
			// FrmTest_zylTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 362);
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlTool);
			this.Controls.Add(this.mnuMain);
			this.MainMenuStrip = this.mnuMain;
			this.Name = "FrmTest_zylTool";
			this.Text = "Test_zylTool";
			this.pnlTool.ResumeLayout(false);
			this.pnlInfo.ResumeLayout(false);
			this.pnlInfo.PerformLayout();
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlTool;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
		private System.Windows.Forms.ToolStripMenuItem mnuDebug;
		private System.Windows.Forms.ToolStripMenuItem mnuDebugPixelFormat;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.ToolStripMenuItem zpfBetterToolStripMenuItem;
		private System.Windows.Forms.Button btnTest1;
	}
}

