namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmMaintenanceExecute
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
            this.lblTargetId = new Sunny.UI.UILabel();
            this.txtTargetId = new Sunny.UI.UITextBox();
            this.lblMaintenanceTime = new Sunny.UI.UILabel();
            this.dtpMaintenanceTime = new Sunny.UI.UIDatePicker();
            this.lblOperator = new Sunny.UI.UILabel();
            this.txtOperator = new Sunny.UI.UITextBox();
            this.lblMaintenanceItems = new Sunny.UI.UILabel();
            this.txtMaintenanceItems = new Sunny.UI.UITextBox();
            this.btnSelectItems = new Sunny.UI.UIButton();
            this.lblResult = new Sunny.UI.UILabel();
            this.cmbResult = new Sunny.UI.UIComboBox();
            this.lblNextIntervalDays = new Sunny.UI.UILabel();
            this.txtNextIntervalDays = new Sunny.UI.UITextBox();
            this.lblNotes = new Sunny.UI.UILabel();
            this.txtNotes = new Sunny.UI.UITextBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnCancel = new Sunny.UI.UIButton();
            this.dgvTasks = new Sunny.UI.UIDataGridView();
            this.lblTaskCount = new Sunny.UI.UILabel();
            this.btnScanCode = new Sunny.UI.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTargetId
            // 
            this.lblTargetId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblTargetId.Location = new System.Drawing.Point(30, 50);
            this.lblTargetId.Name = "lblTargetId";
            this.lblTargetId.Size = new System.Drawing.Size(120, 23);
            this.lblTargetId.TabIndex = 0;
            this.lblTargetId.Text = "目标ID:";
            this.lblTargetId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetId
            // 
            this.txtTargetId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTargetId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtTargetId.Location = new System.Drawing.Point(160, 48);
            this.txtTargetId.Name = "txtTargetId";
            this.txtTargetId.Padding = new System.Windows.Forms.Padding(5);
            this.txtTargetId.Size = new System.Drawing.Size(250, 30);
            this.txtTargetId.TabIndex = 1;
            // 
            // lblMaintenanceTime
            // 
            this.lblMaintenanceTime.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblMaintenanceTime.Location = new System.Drawing.Point(30, 95);
            this.lblMaintenanceTime.Name = "lblMaintenanceTime";
            this.lblMaintenanceTime.Size = new System.Drawing.Size(120, 23);
            this.lblMaintenanceTime.TabIndex = 2;
            this.lblMaintenanceTime.Text = "保养时间:";
            this.lblMaintenanceTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpMaintenanceTime
            // 
            this.dtpMaintenanceTime.FillColor = System.Drawing.Color.White;
            this.dtpMaintenanceTime.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtpMaintenanceTime.Location = new System.Drawing.Point(160, 93);
            this.dtpMaintenanceTime.Name = "dtpMaintenanceTime";
            this.dtpMaintenanceTime.Size = new System.Drawing.Size(250, 30);
            this.dtpMaintenanceTime.TabIndex = 3;
            // 
            // lblOperator
            // 
            this.lblOperator.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblOperator.Location = new System.Drawing.Point(30, 140);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(120, 23);
            this.lblOperator.TabIndex = 4;
            this.lblOperator.Text = "操作员:";
            this.lblOperator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOperator
            // 
            this.txtOperator.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOperator.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtOperator.Location = new System.Drawing.Point(160, 138);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Padding = new System.Windows.Forms.Padding(5);
            this.txtOperator.Size = new System.Drawing.Size(250, 30);
            this.txtOperator.TabIndex = 5;
            // 
            // lblMaintenanceItems
            // 
            this.lblMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblMaintenanceItems.Location = new System.Drawing.Point(30, 185);
            this.lblMaintenanceItems.Name = "lblMaintenanceItems";
            this.lblMaintenanceItems.Size = new System.Drawing.Size(120, 23);
            this.lblMaintenanceItems.TabIndex = 6;
            this.lblMaintenanceItems.Text = "保养项目:";
            this.lblMaintenanceItems.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaintenanceItems
            // 
            this.txtMaintenanceItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaintenanceItems.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtMaintenanceItems.Location = new System.Drawing.Point(160, 183);
            this.txtMaintenanceItems.Multiline = true;
            this.txtMaintenanceItems.Name = "txtMaintenanceItems";
            this.txtMaintenanceItems.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaintenanceItems.ReadOnly = true;
            this.txtMaintenanceItems.Size = new System.Drawing.Size(180, 60);
            this.txtMaintenanceItems.TabIndex = 7;
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectItems.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnSelectItems.Location = new System.Drawing.Point(350, 183);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(60, 30);
            this.btnSelectItems.TabIndex = 8;
            this.btnSelectItems.Text = "选择";
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // lblResult
            // 
            this.lblResult.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblResult.Location = new System.Drawing.Point(30, 260);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(120, 23);
            this.lblResult.TabIndex = 8;
            this.lblResult.Text = "保养结果:";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbResult
            // 
            this.cmbResult.DataSource = null;
            this.cmbResult.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbResult.FillColor = System.Drawing.Color.White;
            this.cmbResult.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbResult.Location = new System.Drawing.Point(160, 258);
            this.cmbResult.Name = "cmbResult";
            this.cmbResult.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbResult.Size = new System.Drawing.Size(250, 30);
            this.cmbResult.TabIndex = 9;
            // 
            // lblNextIntervalDays
            // 
            this.lblNextIntervalDays.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblNextIntervalDays.Location = new System.Drawing.Point(30, 305);
            this.lblNextIntervalDays.Name = "lblNextIntervalDays";
            this.lblNextIntervalDays.Size = new System.Drawing.Size(120, 23);
            this.lblNextIntervalDays.TabIndex = 10;
            this.lblNextIntervalDays.Text = "间隔天数:";
            this.lblNextIntervalDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNextIntervalDays
            // 
            this.txtNextIntervalDays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNextIntervalDays.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtNextIntervalDays.Location = new System.Drawing.Point(160, 303);
            this.txtNextIntervalDays.Name = "txtNextIntervalDays";
            this.txtNextIntervalDays.Padding = new System.Windows.Forms.Padding(5);
            this.txtNextIntervalDays.Size = new System.Drawing.Size(250, 30);
            this.txtNextIntervalDays.TabIndex = 11;
            this.txtNextIntervalDays.Text = "30";
            // 
            // lblNotes
            // 
            this.lblNotes.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblNotes.Location = new System.Drawing.Point(30, 350);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(120, 23);
            this.lblNotes.TabIndex = 12;
            this.lblNotes.Text = "备注:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtNotes.Location = new System.Drawing.Point(160, 348);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Padding = new System.Windows.Forms.Padding(5);
            this.txtNotes.Size = new System.Drawing.Size(250, 60);
            this.txtNotes.TabIndex = 13;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSave.Location = new System.Drawing.Point(160, 430);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnCancel.Location = new System.Drawing.Point(310, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvTasks
            // 
            this.dgvTasks.Location = new System.Drawing.Point(20, 90);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.Size = new System.Drawing.Size(960, 480);
            this.dgvTasks.TabIndex = 18;
            // 
            // lblTaskCount
            // 
            this.lblTaskCount.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblTaskCount.Location = new System.Drawing.Point(20, 55);
            this.lblTaskCount.Name = "lblTaskCount";
            this.lblTaskCount.Size = new System.Drawing.Size(300, 30);
            this.lblTaskCount.TabIndex = 19;
            this.lblTaskCount.Text = "共 0 项待执行任务";
            this.lblTaskCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnScanCode
            // 
            this.btnScanCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScanCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnScanCode.Location = new System.Drawing.Point(860, 50);
            this.btnScanCode.Name = "btnScanCode";
            this.btnScanCode.Size = new System.Drawing.Size(120, 35);
            this.btnScanCode.TabIndex = 20;
            this.btnScanCode.Text = "扫描设备";
            this.btnScanCode.Click += new System.EventHandler(this.btnScanCode_Click);
            // 
            // FrmMaintenanceExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 500);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNextIntervalDays);
            this.Controls.Add(this.lblNextIntervalDays);
            this.Controls.Add(this.cmbResult);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnSelectItems);
            this.Controls.Add(this.txtMaintenanceItems);
            this.Controls.Add(this.lblMaintenanceItems);
            this.Controls.Add(this.txtOperator);
            this.Controls.Add(this.lblOperator);
            this.Controls.Add(this.dtpMaintenanceTime);
            this.Controls.Add(this.lblMaintenanceTime);
            this.Controls.Add(this.txtTargetId);
            this.Controls.Add(this.lblTargetId);
            this.Controls.Add(this.dgvTasks);
            this.Controls.Add(this.lblTaskCount);
            this.Controls.Add(this.btnScanCode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMaintenanceExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "执行保养";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.ResumeLayout(false);
        }

        private Sunny.UI.UILabel lblTargetId;
        private Sunny.UI.UITextBox txtTargetId;
        private Sunny.UI.UILabel lblMaintenanceTime;
        private Sunny.UI.UIDatePicker dtpMaintenanceTime;
        private Sunny.UI.UILabel lblOperator;
        private Sunny.UI.UITextBox txtOperator;
        private Sunny.UI.UILabel lblMaintenanceItems;
        private Sunny.UI.UITextBox txtMaintenanceItems;
        private Sunny.UI.UIButton btnSelectItems;
        private Sunny.UI.UILabel lblResult;
        private Sunny.UI.UIComboBox cmbResult;
        private Sunny.UI.UILabel lblNextIntervalDays;
        private Sunny.UI.UITextBox txtNextIntervalDays;
        private Sunny.UI.UILabel lblNotes;
        private Sunny.UI.UITextBox txtNotes;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIButton btnCancel;
        private Sunny.UI.UIDataGridView dgvTasks;
        private Sunny.UI.UILabel lblTaskCount;
        private Sunny.UI.UIButton btnScanCode;
    }
}
