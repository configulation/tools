namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmEquipmentEdit
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
            this.lblEquipmentId = new Sunny.UI.UILabel();
            this.txtEquipmentId = new Sunny.UI.UITextBox();
            this.lblLineLocation = new Sunny.UI.UILabel();
            this.cmbLineLocation = new Sunny.UI.UIComboBox();
            this.lblCategory = new Sunny.UI.UILabel();
            this.cmbCategory = new Sunny.UI.UIComboBox();
            this.lblSubCategory = new Sunny.UI.UILabel();
            this.cmbSubCategory = new Sunny.UI.UIComboBox();
            this.lblIntervalDays = new Sunny.UI.UILabel();
            this.txtIntervalDays = new Sunny.UI.UITextBox();
            this.lblNextMaintenance = new Sunny.UI.UILabel();
            this.dtpNextMaintenance = new Sunny.UI.UIDatePicker();
            this.lblStatus = new Sunny.UI.UILabel();
            this.cmbStatus = new Sunny.UI.UIComboBox();
            this.lblOperatorId = new Sunny.UI.UILabel();
            this.txtOperatorId = new Sunny.UI.UITextBox();
            this.lblMaintenanceItems = new Sunny.UI.UILabel();
            this.txtMaintenanceItems = new Sunny.UI.UITextBox();
            this.btnSelectItems = new Sunny.UI.UISymbolButton();
            this.lblNotes = new Sunny.UI.UILabel();
            this.txtNotes = new Sunny.UI.UITextBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnCancel = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblEquipmentId
            // 
            this.lblEquipmentId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblEquipmentId.Location = new System.Drawing.Point(30, 50);
            this.lblEquipmentId.Name = "lblEquipmentId";
            this.lblEquipmentId.Size = new System.Drawing.Size(100, 23);
            this.lblEquipmentId.TabIndex = 0;
            this.lblEquipmentId.Text = "设备ID:";
            this.lblEquipmentId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEquipmentId
            // 
            this.txtEquipmentId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEquipmentId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtEquipmentId.Location = new System.Drawing.Point(140, 48);
            this.txtEquipmentId.Name = "txtEquipmentId";
            this.txtEquipmentId.Padding = new System.Windows.Forms.Padding(5);
            this.txtEquipmentId.Size = new System.Drawing.Size(250, 30);
            this.txtEquipmentId.TabIndex = 1;
            // 
            // lblLineLocation
            // 
            this.lblLineLocation.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLineLocation.Location = new System.Drawing.Point(30, 95);
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
            this.cmbLineLocation.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbLineLocation.Location = new System.Drawing.Point(140, 93);
            this.cmbLineLocation.Name = "cmbLineLocation";
            this.cmbLineLocation.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbLineLocation.Size = new System.Drawing.Size(250, 30);
            this.cmbLineLocation.TabIndex = 3;
            // 
            // lblCategory
            // 
            this.lblCategory.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblCategory.Location = new System.Drawing.Point(30, 140);
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
            this.cmbCategory.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbCategory.Location = new System.Drawing.Point(140, 138);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbCategory.Size = new System.Drawing.Size(250, 30);
            this.cmbCategory.TabIndex = 5;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // lblSubCategory
            // 
            this.lblSubCategory.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblSubCategory.Location = new System.Drawing.Point(30, 185);
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
            this.cmbSubCategory.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbSubCategory.Location = new System.Drawing.Point(140, 183);
            this.cmbSubCategory.Name = "cmbSubCategory";
            this.cmbSubCategory.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSubCategory.Size = new System.Drawing.Size(250, 30);
            this.cmbSubCategory.TabIndex = 7;
            // 
            // lblIntervalDays
            // 
            this.lblIntervalDays.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblIntervalDays.Location = new System.Drawing.Point(30, 230);
            this.lblIntervalDays.Name = "lblIntervalDays";
            this.lblIntervalDays.Size = new System.Drawing.Size(100, 23);
            this.lblIntervalDays.TabIndex = 8;
            this.lblIntervalDays.Text = "周期(天):";
            this.lblIntervalDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIntervalDays
            // 
            this.txtIntervalDays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIntervalDays.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtIntervalDays.Location = new System.Drawing.Point(140, 228);
            this.txtIntervalDays.Name = "txtIntervalDays";
            this.txtIntervalDays.Padding = new System.Windows.Forms.Padding(5);
            this.txtIntervalDays.Size = new System.Drawing.Size(250, 30);
            this.txtIntervalDays.TabIndex = 9;
            // 
            // lblNextMaintenance
            // 
            this.lblNextMaintenance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblNextMaintenance.Location = new System.Drawing.Point(30, 275);
            this.lblNextMaintenance.Name = "lblNextMaintenance";
            this.lblNextMaintenance.Size = new System.Drawing.Size(100, 23);
            this.lblNextMaintenance.TabIndex = 10;
            this.lblNextMaintenance.Text = "计划日期:";
            this.lblNextMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpNextMaintenance
            // 
            this.dtpNextMaintenance.FillColor = System.Drawing.Color.White;
            this.dtpNextMaintenance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtpNextMaintenance.Location = new System.Drawing.Point(140, 273);
            this.dtpNextMaintenance.Name = "dtpNextMaintenance";
            this.dtpNextMaintenance.Size = new System.Drawing.Size(250, 30);
            this.dtpNextMaintenance.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblStatus.Location = new System.Drawing.Point(30, 320);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "状态:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DataSource = null;
            this.cmbStatus.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbStatus.FillColor = System.Drawing.Color.White;
            this.cmbStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbStatus.Location = new System.Drawing.Point(140, 318);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbStatus.Size = new System.Drawing.Size(250, 30);
            this.cmbStatus.TabIndex = 13;
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblOperatorId.Location = new System.Drawing.Point(30, 365);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(100, 23);
            this.lblOperatorId.TabIndex = 14;
            this.lblOperatorId.Text = "操作员:";
            this.lblOperatorId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOperatorId
            // 
            this.txtOperatorId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOperatorId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtOperatorId.Location = new System.Drawing.Point(140, 363);
            this.txtOperatorId.Name = "txtOperatorId";
            this.txtOperatorId.Padding = new System.Windows.Forms.Padding(5);
            this.txtOperatorId.Size = new System.Drawing.Size(250, 30);
            this.txtOperatorId.TabIndex = 15;
            // 
            // lblMaintenanceItems
            // 
            this.lblMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblMaintenanceItems.Location = new System.Drawing.Point(30, 410);
            this.lblMaintenanceItems.Name = "lblMaintenanceItems";
            this.lblMaintenanceItems.Size = new System.Drawing.Size(100, 23);
            this.lblMaintenanceItems.TabIndex = 16;
            this.lblMaintenanceItems.Text = "保养项目:";
            this.lblMaintenanceItems.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaintenanceItems
            // 
            this.txtMaintenanceItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtMaintenanceItems.Location = new System.Drawing.Point(140, 408);
            this.txtMaintenanceItems.Name = "txtMaintenanceItems";
            this.txtMaintenanceItems.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaintenanceItems.ReadOnly = true;
            this.txtMaintenanceItems.Size = new System.Drawing.Size(210, 30);
            this.txtMaintenanceItems.TabIndex = 17;
            this.txtMaintenanceItems.Watermark = "点击放大镜选择";
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectItems.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSelectItems.Location = new System.Drawing.Point(355, 408);
            this.btnSelectItems.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(35, 30);
            this.btnSelectItems.Symbol = 61442;
            this.btnSelectItems.TabIndex = 18;
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // lblNotes
            // 
            this.lblNotes.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblNotes.Location = new System.Drawing.Point(30, 455);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(100, 23);
            this.lblNotes.TabIndex = 19;
            this.lblNotes.Text = "备注:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtNotes.Location = new System.Drawing.Point(140, 453);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Padding = new System.Windows.Forms.Padding(5);
            this.txtNotes.Size = new System.Drawing.Size(250, 60);
            this.txtNotes.TabIndex = 20;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSave.Location = new System.Drawing.Point(140, 540);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnCancel.Location = new System.Drawing.Point(250, 540);
            this.btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmEquipmentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 620);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.btnSelectItems);
            this.Controls.Add(this.txtMaintenanceItems);
            this.Controls.Add(this.lblMaintenanceItems);
            this.Controls.Add(this.txtOperatorId);
            this.Controls.Add(this.lblOperatorId);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dtpNextMaintenance);
            this.Controls.Add(this.lblNextMaintenance);
            this.Controls.Add(this.txtIntervalDays);
            this.Controls.Add(this.lblIntervalDays);
            this.Controls.Add(this.cmbSubCategory);
            this.Controls.Add(this.lblSubCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbLineLocation);
            this.Controls.Add(this.lblLineLocation);
            this.Controls.Add(this.txtEquipmentId);
            this.Controls.Add(this.lblEquipmentId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEquipmentEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设备编辑";
            this.ResumeLayout(false);
        }

        private Sunny.UI.UILabel lblEquipmentId;
        private Sunny.UI.UITextBox txtEquipmentId;
        private Sunny.UI.UILabel lblLineLocation;
        private Sunny.UI.UIComboBox cmbLineLocation;
        private Sunny.UI.UILabel lblCategory;
        private Sunny.UI.UIComboBox cmbCategory;
        private Sunny.UI.UILabel lblSubCategory;
        private Sunny.UI.UIComboBox cmbSubCategory;
        private Sunny.UI.UILabel lblIntervalDays;
        private Sunny.UI.UITextBox txtIntervalDays;
        private Sunny.UI.UILabel lblNextMaintenance;
        private Sunny.UI.UIDatePicker dtpNextMaintenance;
        private Sunny.UI.UILabel lblStatus;
        private Sunny.UI.UIComboBox cmbStatus;
        private Sunny.UI.UILabel lblOperatorId;
        private Sunny.UI.UITextBox txtOperatorId;
        private Sunny.UI.UILabel lblMaintenanceItems;
        private Sunny.UI.UITextBox txtMaintenanceItems;
        private Sunny.UI.UISymbolButton btnSelectItems;
        private Sunny.UI.UILabel lblNotes;
        private Sunny.UI.UITextBox txtNotes;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIButton btnCancel;
    }
}
