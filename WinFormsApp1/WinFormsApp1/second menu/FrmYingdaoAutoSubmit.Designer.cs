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
            this.chkAutoStart = new Sunny.UI.UICheckBox();
            this.btnRun = new Sunny.UI.UIButton();
            this.txtLog = new Sunny.UI.UIRichTextBox();
            this.web = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.web)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAutoStart.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.chkAutoStart.Location = new System.Drawing.Point(0, 35);
            this.chkAutoStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkAutoStart.Size = new System.Drawing.Size(800, 29);
            this.chkAutoStart.Style = Sunny.UI.UIStyle.Custom;
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "启动时自动执行";
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.ChkAutoStart_CheckedChanged);
            // 
            // btnRun
            // 
            this.btnRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRun.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnRun.Location = new System.Drawing.Point(0, 64);
            this.btnRun.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(800, 35);
            this.btnRun.Style = Sunny.UI.UIStyle.Custom;
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "立即执行检查/提交";
            this.btnRun.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.FillColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtLog.Location = new System.Drawing.Point(0, 420);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtLog.Name = "txtLog";
            this.txtLog.Padding = new System.Windows.Forms.Padding(2);
            this.txtLog.ReadOnly = true;
            this.txtLog.ShowText = false;
            this.txtLog.Size = new System.Drawing.Size(800, 180);
            this.txtLog.Style = Sunny.UI.UIStyle.Custom;
            this.txtLog.TabIndex = 2;
            this.txtLog.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // web
            // 
            this.web.AllowExternalDrop = true;
            this.web.CreationProperties = null;
            this.web.DefaultBackgroundColor = System.Drawing.Color.White;
            this.web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.web.Location = new System.Drawing.Point(0, 99);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(800, 321);
            this.web.TabIndex = 3;
            this.web.ZoomFactor = 1D;
            // 
            // FrmYingdaoAutoSubmit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.web);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.chkAutoStart);
            this.Name = "FrmYingdaoAutoSubmit";
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "车位抽签-自动提交";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 600);
            this.Load += new System.EventHandler(this.FrmYingdaoAutoSubmit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.web)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
	}
}
