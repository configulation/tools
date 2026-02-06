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
		private SplitContainer splitLeft;  // 上下分割：表格和日志
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
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
            splitLeft = new SplitContainer();
            dgvUsers = new UIDataGridView();
            txtLog = new UIRichTextBox();
            panelUserButtons = new UIPanel();
            btnAddUser = new UIButton();
            btnDeleteUser = new UIButton();
            btnSaveConfig = new UIButton();
            web = new WebView2();
            panelTop.SuspendLayout();
            ((ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelLeft.SuspendLayout();
            ((ISupportInitialize)splitLeft).BeginInit();
            splitLeft.Panel1.SuspendLayout();
            splitLeft.Panel2.SuspendLayout();
            splitLeft.SuspendLayout();
            ((ISupportInitialize)dgvUsers).BeginInit();
            panelUserButtons.SuspendLayout();
            ((ISupportInitialize)web).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
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
            panelTop.Dock = DockStyle.Top;
            panelTop.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(4, 5, 4, 5);
            panelTop.MinimumSize = new Size(1, 1);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1200, 45);
            panelTop.Style = UIStyle.Custom;
            panelTop.TabIndex = 1;
            panelTop.Text = null;
            panelTop.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // chkAutoStart
            // 
            chkAutoStart.Font = new Font("微软雅黑", 9F);
            chkAutoStart.ForeColor = Color.FromArgb(48, 48, 48);
            chkAutoStart.Location = new Point(10, 10);
            chkAutoStart.MinimumSize = new Size(1, 1);
            chkAutoStart.Name = "chkAutoStart";
            chkAutoStart.Size = new Size(130, 25);
            chkAutoStart.Style = UIStyle.Custom;
            chkAutoStart.TabIndex = 0;
            chkAutoStart.Text = "自动执行";
            chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
            // 
            // btnRun
            // 
            btnRun.Font = new Font("微软雅黑", 9F);
            btnRun.Location = new Point(145, 8);
            btnRun.MinimumSize = new Size(1, 1);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(90, 30);
            btnRun.Style = UIStyle.Custom;
            btnRun.TabIndex = 1;
            btnRun.Text = "立即执行";
            btnRun.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnRun.Click += btnRun_Click;
            // 
            // lblDays
            // 
            lblDays.Font = new Font("微软雅黑", 9F);
            lblDays.ForeColor = Color.FromArgb(48, 48, 48);
            lblDays.Location = new Point(250, 12);
            lblDays.Name = "lblDays";
            lblDays.Size = new Size(60, 22);
            lblDays.Style = UIStyle.Custom;
            lblDays.TabIndex = 2;
            lblDays.Text = "执行日:";
            // 
            // chkSunday
            // 
            chkSunday.Font = new Font("微软雅黑", 9F);
            chkSunday.ForeColor = Color.FromArgb(48, 48, 48);
            chkSunday.Location = new Point(310, 10);
            chkSunday.MinimumSize = new Size(1, 1);
            chkSunday.Name = "chkSunday";
            chkSunday.Size = new Size(45, 25);
            chkSunday.Style = UIStyle.Custom;
            chkSunday.TabIndex = 3;
            chkSunday.Text = "日";
            chkSunday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkMonday
            // 
            chkMonday.Font = new Font("微软雅黑", 9F);
            chkMonday.ForeColor = Color.FromArgb(48, 48, 48);
            chkMonday.Location = new Point(355, 10);
            chkMonday.MinimumSize = new Size(1, 1);
            chkMonday.Name = "chkMonday";
            chkMonday.Size = new Size(45, 25);
            chkMonday.Style = UIStyle.Custom;
            chkMonday.TabIndex = 4;
            chkMonday.Text = "一";
            chkMonday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkTuesday
            // 
            chkTuesday.Font = new Font("微软雅黑", 9F);
            chkTuesday.ForeColor = Color.FromArgb(48, 48, 48);
            chkTuesday.Location = new Point(400, 10);
            chkTuesday.MinimumSize = new Size(1, 1);
            chkTuesday.Name = "chkTuesday";
            chkTuesday.Size = new Size(45, 25);
            chkTuesday.Style = UIStyle.Custom;
            chkTuesday.TabIndex = 5;
            chkTuesday.Text = "二";
            chkTuesday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkWednesday
            // 
            chkWednesday.Font = new Font("微软雅黑", 9F);
            chkWednesday.ForeColor = Color.FromArgb(48, 48, 48);
            chkWednesday.Location = new Point(445, 10);
            chkWednesday.MinimumSize = new Size(1, 1);
            chkWednesday.Name = "chkWednesday";
            chkWednesday.Size = new Size(45, 25);
            chkWednesday.Style = UIStyle.Custom;
            chkWednesday.TabIndex = 6;
            chkWednesday.Text = "三";
            chkWednesday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkThursday
            // 
            chkThursday.Font = new Font("微软雅黑", 9F);
            chkThursday.ForeColor = Color.FromArgb(48, 48, 48);
            chkThursday.Location = new Point(490, 10);
            chkThursday.MinimumSize = new Size(1, 1);
            chkThursday.Name = "chkThursday";
            chkThursday.Size = new Size(45, 25);
            chkThursday.Style = UIStyle.Custom;
            chkThursday.TabIndex = 7;
            chkThursday.Text = "四";
            chkThursday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkFriday
            // 
            chkFriday.Font = new Font("微软雅黑", 9F);
            chkFriday.ForeColor = Color.FromArgb(48, 48, 48);
            chkFriday.Location = new Point(535, 10);
            chkFriday.MinimumSize = new Size(1, 1);
            chkFriday.Name = "chkFriday";
            chkFriday.Size = new Size(45, 25);
            chkFriday.Style = UIStyle.Custom;
            chkFriday.TabIndex = 8;
            chkFriday.Text = "五";
            chkFriday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // chkSaturday
            // 
            chkSaturday.Font = new Font("微软雅黑", 9F);
            chkSaturday.ForeColor = Color.FromArgb(48, 48, 48);
            chkSaturday.Location = new Point(580, 10);
            chkSaturday.MinimumSize = new Size(1, 1);
            chkSaturday.Name = "chkSaturday";
            chkSaturday.Size = new Size(45, 25);
            chkSaturday.Style = UIStyle.Custom;
            chkSaturday.TabIndex = 9;
            chkSaturday.Text = "六";
            chkSaturday.CheckedChanged += ChkDay_CheckedChanged;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 45);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(panelLeft);
            splitMain.Panel1MinSize = 250;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(web);
            splitMain.Panel2MinSize = 250;
            splitMain.Size = new Size(1200, 655);
            splitMain.SplitterDistance = 720;
            splitMain.SplitterWidth = 6;
            splitMain.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(splitLeft);
            panelLeft.Controls.Add(panelUserButtons);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(4, 5, 4, 5);
            panelLeft.MinimumSize = new Size(1, 1);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(600, 655);
            panelLeft.Style = UIStyle.Custom;
            panelLeft.TabIndex = 0;
            panelLeft.Text = null;
            panelLeft.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // splitLeft
            // 
            splitLeft.Dock = DockStyle.Fill;
            splitLeft.Location = new Point(0, 40);
            splitLeft.Name = "splitLeft";
            splitLeft.Orientation = Orientation.Horizontal;
            // 
            // splitLeft.Panel1
            // 
            splitLeft.Panel1.Controls.Add(dgvUsers);
            splitLeft.Panel1MinSize = 100;
            // 
            // splitLeft.Panel2
            // 
            splitLeft.Panel2.Controls.Add(txtLog);
            splitLeft.Panel2MinSize = 100;
            splitLeft.Size = new Size(600, 615);
            splitLeft.SplitterDistance = 300;
            splitLeft.SplitterWidth = 6;
            splitLeft.TabIndex = 3;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.ColumnHeadersHeight = 32;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.Font = new Font("微软雅黑", 9F);
            dgvUsers.GridColor = Color.FromArgb(80, 160, 255);
            dgvUsers.Location = new Point(0, 0);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle3.Font = new Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dgvUsers.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvUsers.SelectedIndex = -1;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(600, 300);
            dgvUsers.StripeOddColor = Color.FromArgb(235, 243, 255);
            dgvUsers.Style = UIStyle.Custom;
            dgvUsers.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.FillColor = Color.FromArgb(245, 245, 245);
            txtLog.Font = new Font("微软雅黑", 9F);
            txtLog.Location = new Point(0, 0);
            txtLog.Margin = new Padding(4, 5, 4, 5);
            txtLog.MinimumSize = new Size(1, 1);
            txtLog.Name = "txtLog";
            txtLog.Padding = new Padding(2);
            txtLog.ReadOnly = true;
            txtLog.ShowText = false;
            txtLog.Size = new Size(600, 309);
            txtLog.Style = UIStyle.Custom;
            txtLog.TabIndex = 1;
            txtLog.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // panelUserButtons
            // 
            panelUserButtons.Controls.Add(btnAddUser);
            panelUserButtons.Controls.Add(btnDeleteUser);
            panelUserButtons.Controls.Add(btnSaveConfig);
            panelUserButtons.Dock = DockStyle.Top;
            panelUserButtons.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            panelUserButtons.Location = new Point(0, 0);
            panelUserButtons.Margin = new Padding(4, 5, 4, 5);
            panelUserButtons.MinimumSize = new Size(1, 1);
            panelUserButtons.Name = "panelUserButtons";
            panelUserButtons.Size = new Size(600, 40);
            panelUserButtons.Style = UIStyle.Custom;
            panelUserButtons.TabIndex = 2;
            panelUserButtons.Text = null;
            panelUserButtons.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnAddUser
            // 
            btnAddUser.Font = new Font("微软雅黑", 9F);
            btnAddUser.Location = new Point(5, 5);
            btnAddUser.MinimumSize = new Size(1, 1);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(80, 30);
            btnAddUser.Style = UIStyle.Custom;
            btnAddUser.TabIndex = 0;
            btnAddUser.Text = "添加";
            btnAddUser.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAddUser.Click += btnAddUser_Click;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.Font = new Font("微软雅黑", 9F);
            btnDeleteUser.Location = new Point(90, 5);
            btnDeleteUser.MinimumSize = new Size(1, 1);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(80, 30);
            btnDeleteUser.Style = UIStyle.Custom;
            btnDeleteUser.TabIndex = 1;
            btnDeleteUser.Text = "删除";
            btnDeleteUser.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDeleteUser.Click += btnDeleteUser_Click;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.FillColor = Color.FromArgb(0, 150, 136);
            btnSaveConfig.Font = new Font("微软雅黑", 9F);
            btnSaveConfig.Location = new Point(175, 5);
            btnSaveConfig.MinimumSize = new Size(1, 1);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(80, 30);
            btnSaveConfig.Style = UIStyle.Custom;
            btnSaveConfig.TabIndex = 2;
            btnSaveConfig.Text = "保存";
            btnSaveConfig.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSaveConfig.Click += btnSaveConfig_Click;
            // 
            // web
            // 
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = Color.White;
            web.Dock = DockStyle.Fill;
            web.Location = new Point(0, 0);
            web.Name = "web";
            web.Size = new Size(594, 655);
            web.TabIndex = 0;
            web.ZoomFactor = 1D;
            // 
            // FrmYingdaoAutoSubmit
            // 
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
            ZoomScaleRect = new Rectangle(19, 19, 1200, 700);
            Load += FrmYingdaoAutoSubmit_Load;
            panelTop.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            splitLeft.Panel1.ResumeLayout(false);
            splitLeft.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitLeft).EndInit();
            splitLeft.ResumeLayout(false);
            ((ISupportInitialize)dgvUsers).EndInit();
            panelUserButtons.ResumeLayout(false);
            ((ISupportInitialize)web).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
