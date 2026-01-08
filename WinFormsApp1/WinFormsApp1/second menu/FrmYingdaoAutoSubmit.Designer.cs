using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Sunny.UI;

namespace WinFormsApp1.second_menu
{
	partial class FrmYingdaoAutoSubmit
	{
		private IContainer components = null;

		// 顶部控件
		private UIPanel panelTop;
		private UICheckBox chkAutoStart;
		private UIButton btnRun;
		
		// 星期选择控件
		private UILabel lblDays;
		private UICheckBox chkSunday;
		private UICheckBox chkMonday;
		private UICheckBox chkTuesday;
		private UICheckBox chkWednesday;
		private UICheckBox chkThursday;
		private UICheckBox chkFriday;
		private UICheckBox chkSaturday;
		
		// 主分割容器（左右分割）
		private SplitContainer splitMain;
		
		// 左侧面板 - 用户配置
		private UIPanel panelLeft;
		private UIPanel panelUserButtons;
		private UIButton btnAddUser;
		private UIButton btnDeleteUser;
		private UIButton btnSaveConfig;
		private UIDataGridView dgvUsers;
		private UIRichTextBox txtLog;
		
		// 右侧 - 浏览器
		private WebView2 web;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            panelTop = new UIPanel();
            chkAutoStart = new UICheckBox();
            btnRun = new UIButton();
            lblDays = new UILabel();
            chkSunday = new UICheckBox();
            chkMonday = new UICheckBox();
            chkTuesday = new UICheckBox();
            chkWednesday = new UICheckBox();
            chkThursday = new UICheckBox();
            chkFriday = new UICheckBox();
            chkSaturday = new UICheckBox();
            splitMain = new SplitContainer();
            panelLeft = new UIPanel();
            panelUserButtons = new UIPanel();
            btnAddUser = new UIButton();
            btnDeleteUser = new UIButton();
            btnSaveConfig = new UIButton();
            dgvUsers = new UIDataGridView();
            txtLog = new UIRichTextBox();
            web = new WebView2();
            ((ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((ISupportInitialize)dgvUsers).BeginInit();
            ((ISupportInitialize)web).BeginInit();
            panelTop.SuspendLayout();
            panelLeft.SuspendLayout();
            panelUserButtons.SuspendLayout();
            SuspendLayout();
            // ========== panelTop ==========
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 45;
            panelTop.Name = "panelTop";
            panelTop.Style = UIStyle.Custom;
            panelTop.Controls.Add(chkAutoStart);
            panelTop.Controls.Add(btnRun);
            panelTop.Controls.Add(lblDays);
            panelTop.Controls.Add(chkSunday);
            panelTop.Controls.Add(chkMonday);
            panelTop.Controls.Add(chkTuesday);
            panelTop.Controls.Add(chkWednesday);
            panelTop.Controls.Add(chkThursday);
            panelTop.Controls.Add(chkFriday);
            panelTop.Controls.Add(chkSaturday);
            // chkAutoStart
            chkAutoStart.Location = new Point(10, 10);
            chkAutoStart.Size = new Size(130, 25);
            chkAutoStart.Text = "自动执行";
            chkAutoStart.Font = new Font("微软雅黑", 9F);
            chkAutoStart.Style = UIStyle.Custom;
            chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
            // btnRun
            btnRun.Location = new Point(145, 8);
            btnRun.Size = new Size(90, 30);
            btnRun.Text = "立即执行";
            btnRun.Font = new Font("微软雅黑", 9F);
            btnRun.Style = UIStyle.Custom;
            btnRun.Click += btnRun_Click;
            // lblDays
            lblDays.Location = new Point(250, 12);
            lblDays.Size = new Size(70, 22);
            lblDays.Text = "执行日:";
            lblDays.Font = new Font("微软雅黑", 9F);
            lblDays.Style = UIStyle.Custom;
            // 星期复选框
            int dayX = 320;
            int dayW = 55;
            chkSunday.Location = new Point(dayX, 10);
            chkSunday.Size = new Size(dayW, 25);
            chkSunday.Text = "日";
            chkSunday.Font = new Font("微软雅黑", 9F);
            chkSunday.Style = UIStyle.Custom;
            chkSunday.CheckedChanged += ChkDay_CheckedChanged;
            chkMonday.Location = new Point(dayX + dayW, 10);
            chkMonday.Size = new Size(dayW, 25);
            chkMonday.Text = "一";
            chkMonday.Font = new Font("微软雅黑", 9F);
            chkMonday.Style = UIStyle.Custom;
            chkMonday.CheckedChanged += ChkDay_CheckedChanged;
            chkTuesday.Location = new Point(dayX + dayW * 2, 10);
            chkTuesday.Size = new Size(dayW, 25);
            chkTuesday.Text = "二";
            chkTuesday.Font = new Font("微软雅黑", 9F);
            chkTuesday.Style = UIStyle.Custom;
            chkTuesday.CheckedChanged += ChkDay_CheckedChanged;
            chkWednesday.Location = new Point(dayX + dayW * 3, 10);
            chkWednesday.Size = new Size(dayW, 25);
            chkWednesday.Text = "三";
            chkWednesday.Font = new Font("微软雅黑", 9F);
            chkWednesday.Style = UIStyle.Custom;
            chkWednesday.CheckedChanged += ChkDay_CheckedChanged;
            chkThursday.Location = new Point(dayX + dayW * 4, 10);
            chkThursday.Size = new Size(dayW, 25);
            chkThursday.Text = "四";
            chkThursday.Font = new Font("微软雅黑", 9F);
            chkThursday.Style = UIStyle.Custom;
            chkThursday.CheckedChanged += ChkDay_CheckedChanged;
            chkFriday.Location = new Point(dayX + dayW * 5, 10);
            chkFriday.Size = new Size(dayW, 25);
            chkFriday.Text = "五";
            chkFriday.Font = new Font("微软雅黑", 9F);
            chkFriday.Style = UIStyle.Custom;
            chkFriday.CheckedChanged += ChkDay_CheckedChanged;
            chkSaturday.Location = new Point(dayX + dayW * 6, 10);
            chkSaturday.Size = new Size(dayW, 25);
            chkSaturday.Text = "六";
            chkSaturday.Font = new Font("微软雅黑", 9F);
            chkSaturday.Style = UIStyle.Custom;
            chkSaturday.CheckedChanged += ChkDay_CheckedChanged;
            // ========== splitMain（左右分割）==========
            splitMain.Dock = DockStyle.Fill;
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Vertical;
            splitMain.Size = new Size(1200, 655);
            splitMain.Panel1MinSize = 250;
            splitMain.Panel2MinSize = 250;
            splitMain.SplitterDistance = 600;
            splitMain.SplitterWidth = 6;
            // Panel1 - 左侧（用户配置）
            splitMain.Panel1.Controls.Add(panelLeft);
            // Panel2 - 右侧（浏览器）
            splitMain.Panel2.Controls.Add(web);
            // ========== panelLeft ==========
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Style = UIStyle.Custom;
            panelLeft.Controls.Add(dgvUsers);
            panelLeft.Controls.Add(txtLog);
            panelLeft.Controls.Add(panelUserButtons);
            // panelUserButtons
            panelUserButtons.Dock = DockStyle.Top;
            panelUserButtons.Height = 40;
            panelUserButtons.Style = UIStyle.Custom;
            panelUserButtons.Controls.Add(btnAddUser);
            panelUserButtons.Controls.Add(btnDeleteUser);
            panelUserButtons.Controls.Add(btnSaveConfig);
            // btnAddUser
            btnAddUser.Location = new Point(5, 5);
            btnAddUser.Size = new Size(80, 30);
            btnAddUser.Text = "添加";
            btnAddUser.Font = new Font("微软雅黑", 9F);
            btnAddUser.Style = UIStyle.Custom;
            btnAddUser.Click += btnAddUser_Click;
            // btnDeleteUser
            btnDeleteUser.Location = new Point(90, 5);
            btnDeleteUser.Size = new Size(80, 30);
            btnDeleteUser.Text = "删除";
            btnDeleteUser.Font = new Font("微软雅黑", 9F);
            btnDeleteUser.Style = UIStyle.Custom;
            btnDeleteUser.Click += btnDeleteUser_Click;
            // btnSaveConfig
            btnSaveConfig.Location = new Point(175, 5);
            btnSaveConfig.Size = new Size(80, 30);
            btnSaveConfig.Text = "保存";
            btnSaveConfig.Font = new Font("微软雅黑", 9F);
            btnSaveConfig.Style = UIStyle.Custom;
            btnSaveConfig.FillColor = Color.FromArgb(0, 150, 136);
            btnSaveConfig.Click += btnSaveConfig_Click;
            // dgvUsers
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Font = new Font("微软雅黑", 9F);
            dgvUsers.Style = UIStyle.Custom;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.RowHeadersVisible = false;
            // txtLog
            txtLog.Dock = DockStyle.Bottom;
            txtLog.Height = 120;
            txtLog.Font = new Font("微软雅黑", 9F);
            txtLog.Style = UIStyle.Custom;
            txtLog.ReadOnly = true;
            txtLog.FillColor = Color.FromArgb(245, 245, 245);
            // ========== web ==========
            web.Dock = DockStyle.Fill;
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = Color.White;
            web.ZoomFactor = 1D;
            // ========== FrmYingdaoAutoSubmit ==========
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1200, 700);
            Controls.Add(splitMain);
            Controls.Add(panelTop);
            Name = "FrmYingdaoAutoSubmit";
            Padding = new Padding(0);
            ShowTitle = false;
            Style = UIStyle.Custom;
            Text = "车位抽签-自动提交";
            Load += FrmYingdaoAutoSubmit_Load;
            ((ISupportInitialize)splitMain).EndInit();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            splitMain.ResumeLayout(false);
            ((ISupportInitialize)dgvUsers).EndInit();
            ((ISupportInitialize)web).EndInit();
            panelTop.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelUserButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
