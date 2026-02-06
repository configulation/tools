namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmToolEdit
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblToolCode = new Sunny.UI.UILabel();
            this.txtToolCode = new Sunny.UI.UITextBox();
            this.lblLineLocation = new Sunny.UI.UILabel();
            this.cmbLineLocation = new Sunny.UI.UIComboBox();
            this.lblCategory = new Sunny.UI.UILabel();
            this.cmbCategory = new Sunny.UI.UIComboBox();
            this.lblSubCategory = new Sunny.UI.UILabel();
            this.cmbSubCategory = new Sunny.UI.UIComboBox();
            this.lblWorkOrder = new Sunny.UI.UILabel();
            this.txtWorkOrder = new Sunny.UI.UITextBox();
            this.lblOrderQuantity = new Sunny.UI.UILabel();
            this.txtOrderQuantity = new Sunny.UI.UITextBox();
            this.lblPanelQuantity = new Sunny.UI.UILabel();
            this.txtPanelQuantity = new Sunny.UI.UITextBox();
            this.lblScraperCount = new Sunny.UI.UILabel();
            this.txtScraperCount = new Sunny.UI.UITextBox();
            this.lblUsageCount = new Sunny.UI.UILabel();
            this.txtUsageCount = new Sunny.UI.UITextBox();
            this.btnCalculateUsage = new Sunny.UI.UIButton();
            this.lblTotalUsage = new Sunny.UI.UILabel();
            this.txtTotalUsage = new Sunny.UI.UITextBox();
            this.lblMaintenanceInterval = new Sunny.UI.UILabel();
            this.txtMaintenanceInterval = new Sunny.UI.UITextBox();
            this.lblNextMaintenance = new Sunny.UI.UILabel();
            this.dtpNextMaintenance = new Sunny.UI.UIDatePicker();
            this.lblStatus = new Sunny.UI.UILabel();
            this.cmbStatus = new Sunny.UI.UIComboBox();
            this.lblMaintenanceItems = new Sunny.UI.UILabel();
            this.txtMaintenanceItems = new Sunny.UI.UITextBox();
            this.lblNotes = new Sunny.UI.UILabel();
            this.txtNotes = new Sunny.UI.UITextBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnCancel = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblToolCode
            // 
            this.lblToolCode.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblToolCode.Location = new System.Drawing.Point(20, 50);
            this.lblToolCode.Name = "lblToolCode";
            this.lblToolCode.Size = new System.Drawing.Size(100, 23);
            this.lblToolCode.TabIndex = 0;
            this.lblToolCode.Text = "工装编码:";
            this.lblToolCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtToolCode
            // 
            this.txtToolCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtToolCode.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtToolCode.Location = new System.Drawing.Point(130, 48);
            this.txtToolCode.Name = "txtToolCode";
            this.txtToolCode.Padding = new System.Windows.Forms.Padding(5);
            this.txtToolCode.Size = new System.Drawing.Size(200, 28);
            this.txtToolCode.TabIndex = 1;
            // 
            // lblLineLocation
            // 
            this.lblLineLocation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblLineLocation.Location = new System.Drawing.Point(20, 88);
            this.lblLineLocation.Name = "lblLineLocation";
            this.lblLineLocation.Size = new System.Drawing.Size(100, 23);
            this.lblLineLocation.TabIndex = 2;
            this.lblLineLocation.Text = "线别储位:";
            this.lblLineLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLineLocation
            // 
            this.cmbLineLocation.DataSource = null;
            this.cmbLineLocation.FillColor = System.Drawing.Color.White;
            this.cmbLineLocation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbLineLocation.Location = new System.Drawing.Point(130, 86);
            this.cmbLineLocation.Name = "cmbLineLocation";
            this.cmbLineLocation.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbLineLocation.Size = new System.Drawing.Size(200, 28);
            this.cmbLineLocation.TabIndex = 3;
            // 
            // lblCategory
            // 
            this.lblCategory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCategory.Location = new System.Drawing.Point(20, 126);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(100, 23);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "类别:";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DataSource = null;
            this.cmbCategory.FillColor = System.Drawing.Color.White;
            this.cmbCategory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbCategory.Location = new System.Drawing.Point(130, 124);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbCategory.Size = new System.Drawing.Size(200, 28);
            this.cmbCategory.TabIndex = 5;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // lblSubCategory
            // 
            this.lblSubCategory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblSubCategory.Location = new System.Drawing.Point(20, 164);
            this.lblSubCategory.Name = "lblSubCategory";
            this.lblSubCategory.Size = new System.Drawing.Size(100, 23);
            this.lblSubCategory.TabIndex = 6;
            this.lblSubCategory.Text = "子类别:";
            this.lblSubCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSubCategory
            // 
            this.cmbSubCategory.DataSource = null;
            this.cmbSubCategory.FillColor = System.Drawing.Color.White;
            this.cmbSubCategory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbSubCategory.Location = new System.Drawing.Point(130, 162);
            this.cmbSubCategory.Name = "cmbSubCategory";
            this.cmbSubCategory.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSubCategory.Size = new System.Drawing.Size(200, 28);
            this.cmbSubCategory.TabIndex = 7;
            // 
            // lblWorkOrder
            // 
            this.lblWorkOrder.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblWorkOrder.Location = new System.Drawing.Point(20, 202);
            this.lblWorkOrder.Name = "lblWorkOrder";
            this.lblWorkOrder.Size = new System.Drawing.Size(100, 23);
            this.lblWorkOrder.TabIndex = 8;
            this.lblWorkOrder.Text = "工单号:";
            this.lblWorkOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWorkOrder
            // 
            this.txtWorkOrder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWorkOrder.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtWorkOrder.Location = new System.Drawing.Point(130, 200);
            this.txtWorkOrder.Name = "txtWorkOrder";
            this.txtWorkOrder.Padding = new System.Windows.Forms.Padding(5);
            this.txtWorkOrder.Size = new System.Drawing.Size(200, 28);
            this.txtWorkOrder.TabIndex = 9;
            // 
            // lblOrderQuantity
            // 
            this.lblOrderQuantity.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOrderQuantity.Location = new System.Drawing.Point(20, 240);
            this.lblOrderQuantity.Name = "lblOrderQuantity";
            this.lblOrderQuantity.Size = new System.Drawing.Size(100, 23);
            this.lblOrderQuantity.TabIndex = 10;
            this.lblOrderQuantity.Text = "工单数量:";
            this.lblOrderQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOrderQuantity
            // 
            this.txtOrderQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOrderQuantity.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtOrderQuantity.Location = new System.Drawing.Point(130, 238);
            this.txtOrderQuantity.Name = "txtOrderQuantity";
            this.txtOrderQuantity.Padding = new System.Windows.Forms.Padding(5);
            this.txtOrderQuantity.Size = new System.Drawing.Size(200, 28);
            this.txtOrderQuantity.TabIndex = 11;
            this.txtOrderQuantity.Text = "0";
            // 
            // lblPanelQuantity
            // 
            this.lblPanelQuantity.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPanelQuantity.Location = new System.Drawing.Point(20, 278);
            this.lblPanelQuantity.Name = "lblPanelQuantity";
            this.lblPanelQuantity.Size = new System.Drawing.Size(100, 23);
            this.lblPanelQuantity.TabIndex = 12;
            this.lblPanelQuantity.Text = "拼料数量:";
            this.lblPanelQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPanelQuantity
            // 
            this.txtPanelQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPanelQuantity.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtPanelQuantity.Location = new System.Drawing.Point(130, 276);
            this.txtPanelQuantity.Name = "txtPanelQuantity";
            this.txtPanelQuantity.Padding = new System.Windows.Forms.Padding(5);
            this.txtPanelQuantity.Size = new System.Drawing.Size(200, 28);
            this.txtPanelQuantity.TabIndex = 13;
            this.txtPanelQuantity.Text = "1";
            // 
            // lblScraperCount
            // 
            this.lblScraperCount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblScraperCount.Location = new System.Drawing.Point(20, 316);
            this.lblScraperCount.Name = "lblScraperCount";
            this.lblScraperCount.Size = new System.Drawing.Size(100, 23);
            this.lblScraperCount.TabIndex = 14;
            this.lblScraperCount.Text = "刮刀数量:";
            this.lblScraperCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtScraperCount
            // 
            this.txtScraperCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtScraperCount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtScraperCount.Location = new System.Drawing.Point(130, 314);
            this.txtScraperCount.Name = "txtScraperCount";
            this.txtScraperCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtScraperCount.Size = new System.Drawing.Size(200, 28);
            this.txtScraperCount.TabIndex = 15;
            this.txtScraperCount.Text = "1";
            // 
            // lblUsageCount
            // 
            this.lblUsageCount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblUsageCount.Location = new System.Drawing.Point(20, 354);
            this.lblUsageCount.Name = "lblUsageCount";
            this.lblUsageCount.Size = new System.Drawing.Size(100, 23);
            this.lblUsageCount.TabIndex = 16;
            this.lblUsageCount.Text = "使用次数:";
            this.lblUsageCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUsageCount
            // 
            this.txtUsageCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsageCount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtUsageCount.Location = new System.Drawing.Point(130, 352);
            this.txtUsageCount.Name = "txtUsageCount";
            this.txtUsageCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtUsageCount.Size = new System.Drawing.Size(120, 28);
            this.txtUsageCount.TabIndex = 17;
            this.txtUsageCount.Text = "0";
            // 
            // btnCalculateUsage
            // 
            this.btnCalculateUsage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalculateUsage.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCalculateUsage.Location = new System.Drawing.Point(260, 350);
            this.btnCalculateUsage.Name = "btnCalculateUsage";
            this.btnCalculateUsage.Size = new System.Drawing.Size(70, 30);
            this.btnCalculateUsage.TabIndex = 18;
            this.btnCalculateUsage.Text = "计算";
            this.btnCalculateUsage.Click += new System.EventHandler(this.btnCalculateUsage_Click);
            // 
            // lblTotalUsage
            // 
            this.lblTotalUsage.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblTotalUsage.Location = new System.Drawing.Point(20, 392);
            this.lblTotalUsage.Name = "lblTotalUsage";
            this.lblTotalUsage.Size = new System.Drawing.Size(100, 23);
            this.lblTotalUsage.TabIndex = 19;
            this.lblTotalUsage.Text = "总次数:";
            this.lblTotalUsage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalUsage
            // 
            this.txtTotalUsage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotalUsage.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtTotalUsage.Location = new System.Drawing.Point(130, 390);
            this.txtTotalUsage.Name = "txtTotalUsage";
            this.txtTotalUsage.Padding = new System.Windows.Forms.Padding(5);
            this.txtTotalUsage.Size = new System.Drawing.Size(200, 28);
            this.txtTotalUsage.TabIndex = 20;
            this.txtTotalUsage.Text = "100000";
            // 
            // lblMaintenanceInterval
            // 
            this.lblMaintenanceInterval.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMaintenanceInterval.Location = new System.Drawing.Point(20, 430);
            this.lblMaintenanceInterval.Name = "lblMaintenanceInterval";
            this.lblMaintenanceInterval.Size = new System.Drawing.Size(100, 23);
            this.lblMaintenanceInterval.TabIndex = 21;
            this.lblMaintenanceInterval.Text = "保养间隔:";
            this.lblMaintenanceInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaintenanceInterval
            // 
            this.txtMaintenanceInterval.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaintenanceInterval.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtMaintenanceInterval.Location = new System.Drawing.Point(130, 428);
            this.txtMaintenanceInterval.Name = "txtMaintenanceInterval";
            this.txtMaintenanceInterval.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaintenanceInterval.Size = new System.Drawing.Size(200, 28);
            this.txtMaintenanceInterval.TabIndex = 22;
            // 
            // lblNextMaintenance
            // 
            this.lblNextMaintenance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblNextMaintenance.Location = new System.Drawing.Point(20, 468);
            this.lblNextMaintenance.Name = "lblNextMaintenance";
            this.lblNextMaintenance.Size = new System.Drawing.Size(100, 23);
            this.lblNextMaintenance.TabIndex = 23;
            this.lblNextMaintenance.Text = "计划日期:";
            this.lblNextMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpNextMaintenance
            // 
            this.dtpNextMaintenance.FillColor = System.Drawing.Color.White;
            this.dtpNextMaintenance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpNextMaintenance.Location = new System.Drawing.Point(130, 466);
            this.dtpNextMaintenance.Name = "dtpNextMaintenance";
            this.dtpNextMaintenance.Size = new System.Drawing.Size(200, 28);
            this.dtpNextMaintenance.TabIndex = 24;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblStatus.Location = new System.Drawing.Point(20, 506);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "状态:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DataSource = null;
            this.cmbStatus.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbStatus.FillColor = System.Drawing.Color.White;
            this.cmbStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbStatus.Location = new System.Drawing.Point(130, 504);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbStatus.Size = new System.Drawing.Size(200, 28);
            this.cmbStatus.TabIndex = 26;
            // 
            // lblMaintenanceItems
            // 
            this.lblMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMaintenanceItems.Location = new System.Drawing.Point(20, 544);
            this.lblMaintenanceItems.Name = "lblMaintenanceItems";
            this.lblMaintenanceItems.Size = new System.Drawing.Size(100, 23);
            this.lblMaintenanceItems.TabIndex = 27;
            this.lblMaintenanceItems.Text = "保养项目:";
            this.lblMaintenanceItems.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaintenanceItems
            // 
            this.txtMaintenanceItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtMaintenanceItems.Location = new System.Drawing.Point(130, 542);
            this.txtMaintenanceItems.Name = "txtMaintenanceItems";
            this.txtMaintenanceItems.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaintenanceItems.Size = new System.Drawing.Size(200, 28);
            this.txtMaintenanceItems.TabIndex = 28;
            this.txtMaintenanceItems.Watermark = "多个项目用逗号分隔";
            // 
            // lblNotes
            // 
            this.lblNotes.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblNotes.Location = new System.Drawing.Point(20, 582);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 29;
            this.lblNotes.Text = "备注:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtNotes.Location = new System.Drawing.Point(130, 580);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Padding = new System.Windows.Forms.Padding(5);
            this.txtNotes.Size = new System.Drawing.Size(200, 50);
            this.txtNotes.TabIndex = 30;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnSave.Location = new System.Drawing.Point(80, 650);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 31;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnCancel.Location = new System.Drawing.Point(190, 650);
            this.btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmToolEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 710);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtMaintenanceItems);
            this.Controls.Add(this.lblMaintenanceItems);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dtpNextMaintenance);
            this.Controls.Add(this.lblNextMaintenance);
            this.Controls.Add(this.txtMaintenanceInterval);
            this.Controls.Add(this.lblMaintenanceInterval);
            this.Controls.Add(this.txtTotalUsage);
            this.Controls.Add(this.lblTotalUsage);
            this.Controls.Add(this.btnCalculateUsage);
            this.Controls.Add(this.txtUsageCount);
            this.Controls.Add(this.lblUsageCount);
            this.Controls.Add(this.txtScraperCount);
            this.Controls.Add(this.lblScraperCount);
            this.Controls.Add(this.txtPanelQuantity);
            this.Controls.Add(this.lblPanelQuantity);
            this.Controls.Add(this.txtOrderQuantity);
            this.Controls.Add(this.lblOrderQuantity);
            this.Controls.Add(this.txtWorkOrder);
            this.Controls.Add(this.lblWorkOrder);
            this.Controls.Add(this.cmbSubCategory);
            this.Controls.Add(this.lblSubCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbLineLocation);
            this.Controls.Add(this.lblLineLocation);
            this.Controls.Add(this.txtToolCode);
            this.Controls.Add(this.lblToolCode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmToolEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工装编辑";
            this.ResumeLayout(false);
        }

        private Sunny.UI.UILabel lblToolCode;
        private Sunny.UI.UITextBox txtToolCode;
        private Sunny.UI.UILabel lblLineLocation;
        private Sunny.UI.UIComboBox cmbLineLocation;
        private Sunny.UI.UILabel lblCategory;
        private Sunny.UI.UIComboBox cmbCategory;
        private Sunny.UI.UILabel lblSubCategory;
        private Sunny.UI.UIComboBox cmbSubCategory;
        private Sunny.UI.UILabel lblWorkOrder;
        private Sunny.UI.UITextBox txtWorkOrder;
        private Sunny.UI.UILabel lblOrderQuantity;
        private Sunny.UI.UITextBox txtOrderQuantity;
        private Sunny.UI.UILabel lblPanelQuantity;
        private Sunny.UI.UITextBox txtPanelQuantity;
        private Sunny.UI.UILabel lblScraperCount;
        private Sunny.UI.UITextBox txtScraperCount;
        private Sunny.UI.UILabel lblUsageCount;
        private Sunny.UI.UITextBox txtUsageCount;
        private Sunny.UI.UIButton btnCalculateUsage;
        private Sunny.UI.UILabel lblTotalUsage;
        private Sunny.UI.UITextBox txtTotalUsage;
        private Sunny.UI.UILabel lblMaintenanceInterval;
        private Sunny.UI.UITextBox txtMaintenanceInterval;
        private Sunny.UI.UILabel lblNextMaintenance;
        private Sunny.UI.UIDatePicker dtpNextMaintenance;
        private Sunny.UI.UILabel lblStatus;
        private Sunny.UI.UIComboBox cmbStatus;
        private Sunny.UI.UILabel lblMaintenanceItems;
        private Sunny.UI.UITextBox txtMaintenanceItems;
        private Sunny.UI.UILabel lblNotes;
        private Sunny.UI.UITextBox txtNotes;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIButton btnCancel;
    }
}
