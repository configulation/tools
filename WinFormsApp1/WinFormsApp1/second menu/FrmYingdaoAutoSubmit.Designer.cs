using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Sunny.UI;

namespace WinFormsApp1.second_menu
{
	partial class FrmYingdaoAutoSubmit
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private IContainer components = null;

		private UICheckBox chkAutoStart;
		private UIButton btnRun;
		private UIRichTextBox txtLog;
		private WebView2 web;

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
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            chkAutoStart = new UICheckBox();
            btnRun = new UIButton();
            txtLog = new UIRichTextBox();
            web = new WebView2();
            ((ISupportInitialize)web).BeginInit();
            SuspendLayout();
            // 
            // chkAutoStart
            // 
            chkAutoStart.Cursor = Cursors.Hand;
            chkAutoStart.Dock = DockStyle.Top;
            chkAutoStart.Font = new Font("微软雅黑", 10.5F);
            chkAutoStart.ForeColor = Color.FromArgb(48, 48, 48);
            chkAutoStart.Location = new Point(0, 0);
            chkAutoStart.MinimumSize = new Size(1, 1);
            chkAutoStart.Name = "chkAutoStart";
            chkAutoStart.Padding = new Padding(22, 0, 0, 0);
            chkAutoStart.Size = new Size(800, 29);
            chkAutoStart.Style = UIStyle.Custom;
            chkAutoStart.TabIndex = 0;
            chkAutoStart.Text = "启动时自动执行";
            chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
            // 
            // btnRun
            // 
            btnRun.Cursor = Cursors.Hand;
            btnRun.Dock = DockStyle.Top;
            btnRun.Font = new Font("微软雅黑", 10.5F);
            btnRun.Location = new Point(0, 29);
            btnRun.MinimumSize = new Size(1, 1);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(800, 35);
            btnRun.Style = UIStyle.Custom;
            btnRun.TabIndex = 1;
            btnRun.Text = "立即执行检查/提交";
            btnRun.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnRun.Click += btnRun_Click;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Bottom;
            txtLog.FillColor = Color.White;
            txtLog.Font = new Font("微软雅黑", 10.5F);
            txtLog.Location = new Point(0, 420);
            txtLog.Margin = new Padding(4, 5, 4, 5);
            txtLog.MinimumSize = new Size(1, 1);
            txtLog.Name = "txtLog";
            txtLog.Padding = new Padding(2);
            txtLog.ReadOnly = true;
            txtLog.ShowText = false;
            txtLog.Size = new Size(800, 180);
            txtLog.Style = UIStyle.Custom;
            txtLog.TabIndex = 2;
            txtLog.TextAlignment = ContentAlignment.MiddleLeft;
            // 
            // web
            // 
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = Color.White;
            web.Dock = DockStyle.Fill;
            web.Location = new Point(0, 64);
            web.Name = "web";
            web.Size = new Size(800, 356);
            web.TabIndex = 3;
            web.ZoomFactor = 1D;
            // 
            // FrmYingdaoAutoSubmit
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 600);
            Controls.Add(web);
            Controls.Add(txtLog);
            Controls.Add(btnRun);
            Controls.Add(chkAutoStart);
            Name = "FrmYingdaoAutoSubmit";
            Padding = new Padding(0);
            ShowTitle = false;
            Style = UIStyle.Custom;
            Text = "车位抽签-自动提交";
            ZoomScaleRect = new Rectangle(19, 19, 800, 600);
            Load += FrmYingdaoAutoSubmit_Load;
            ((ISupportInitialize)web).EndInit();
            ResumeLayout(false);

        }

        #endregion
    }
}
