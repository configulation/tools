namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmDataImportExport
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.grpExport = new Sunny.UI.UIGroupBox();
            this.btnExportRecords = new Sunny.UI.UIButton();
            this.btnExportTools = new Sunny.UI.UIButton();
            this.btnExportEquipments = new Sunny.UI.UIButton();
            this.lblExportDesc = new Sunny.UI.UILabel();
            this.grpImport = new Sunny.UI.UIGroupBox();
            this.btnImportTools = new Sunny.UI.UIButton();
            this.btnImportEquipments = new Sunny.UI.UIButton();
            this.lblImportDesc = new Sunny.UI.UILabel();
            this.grpBackup = new Sunny.UI.UIGroupBox();
            this.btnRestore = new Sunny.UI.UIButton();
            this.btnBackup = new Sunny.UI.UIButton();
            this.lblBackupDesc = new Sunny.UI.UILabel();
            this.grpExport.SuspendLayout();
            this.grpImport.SuspendLayout();
            this.grpBackup.SuspendLayout();
            this.SuspendLayout();
            
            this.grpExport.Controls.Add(this.btnExportRecords);
            this.grpExport.Controls.Add(this.btnExportTools);
            this.grpExport.Controls.Add(this.btnExportEquipments);
            this.grpExport.Controls.Add(this.lblExportDesc);
            this.grpExport.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.grpExport.Location = new System.Drawing.Point(20, 50);
            this.grpExport.Name = "grpExport";
            this.grpExport.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.grpExport.Size = new System.Drawing.Size(960, 180);
            this.grpExport.TabIndex = 0;
            this.grpExport.Text = "数据导出";
            this.grpExport.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.btnExportRecords.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportRecords.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnExportRecords.Location = new System.Drawing.Point(640, 120);
            this.btnExportRecords.Name = "btnExportRecords";
            this.btnExportRecords.Size = new System.Drawing.Size(280, 40);
            this.btnExportRecords.TabIndex = 3;
            this.btnExportRecords.Text = "导出保养记录";
            this.btnExportRecords.Click += new System.EventHandler(this.btnExportRecords_Click);
            
            this.btnExportTools.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportTools.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnExportTools.Location = new System.Drawing.Point(340, 120);
            this.btnExportTools.Name = "btnExportTools";
            this.btnExportTools.Size = new System.Drawing.Size(280, 40);
            this.btnExportTools.TabIndex = 2;
            this.btnExportTools.Text = "导出工装数据";
            this.btnExportTools.Click += new System.EventHandler(this.btnExportTools_Click);
            
            this.btnExportEquipments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportEquipments.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnExportEquipments.Location = new System.Drawing.Point(40, 120);
            this.btnExportEquipments.Name = "btnExportEquipments";
            this.btnExportEquipments.Size = new System.Drawing.Size(280, 40);
            this.btnExportEquipments.TabIndex = 1;
            this.btnExportEquipments.Text = "导出设备数据";
            this.btnExportEquipments.Click += new System.EventHandler(this.btnExportEquipments_Click);
            
            this.lblExportDesc.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblExportDesc.Location = new System.Drawing.Point(40, 50);
            this.lblExportDesc.Name = "lblExportDesc";
            this.lblExportDesc.Size = new System.Drawing.Size(880, 50);
            this.lblExportDesc.TabIndex = 0;
            this.lblExportDesc.Text = "将设备、工装或保养记录数据导出为CSV文件，可用于数据备份、报表生成或与其他系统交换数据。";
            this.lblExportDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.grpImport.Controls.Add(this.btnImportTools);
            this.grpImport.Controls.Add(this.btnImportEquipments);
            this.grpImport.Controls.Add(this.lblImportDesc);
            this.grpImport.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.grpImport.Location = new System.Drawing.Point(20, 250);
            this.grpImport.Name = "grpImport";
            this.grpImport.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.grpImport.Size = new System.Drawing.Size(960, 180);
            this.grpImport.TabIndex = 1;
            this.grpImport.Text = "数据导入";
            this.grpImport.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.btnImportTools.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportTools.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnImportTools.Location = new System.Drawing.Point(490, 120);
            this.btnImportTools.Name = "btnImportTools";
            this.btnImportTools.Size = new System.Drawing.Size(280, 40);
            this.btnImportTools.TabIndex = 2;
            this.btnImportTools.Text = "导入工装数据";
            this.btnImportTools.Click += new System.EventHandler(this.btnImportTools_Click);
            
            this.btnImportEquipments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportEquipments.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnImportEquipments.Location = new System.Drawing.Point(190, 120);
            this.btnImportEquipments.Name = "btnImportEquipments";
            this.btnImportEquipments.Size = new System.Drawing.Size(280, 40);
            this.btnImportEquipments.TabIndex = 1;
            this.btnImportEquipments.Text = "导入设备数据";
            this.btnImportEquipments.Click += new System.EventHandler(this.btnImportEquipments_Click);
            
            this.lblImportDesc.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblImportDesc.Location = new System.Drawing.Point(40, 50);
            this.lblImportDesc.Name = "lblImportDesc";
            this.lblImportDesc.Size = new System.Drawing.Size(880, 50);
            this.lblImportDesc.TabIndex = 0;
            this.lblImportDesc.Text = "从CSV文件导入设备或工装数据，支持批量录入。导入时会自动跳过重复的ID/编码。";
            this.lblImportDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.grpBackup.Controls.Add(this.btnRestore);
            this.grpBackup.Controls.Add(this.btnBackup);
            this.grpBackup.Controls.Add(this.lblBackupDesc);
            this.grpBackup.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.grpBackup.Location = new System.Drawing.Point(20, 450);
            this.grpBackup.Name = "grpBackup";
            this.grpBackup.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.grpBackup.Size = new System.Drawing.Size(960, 180);
            this.grpBackup.TabIndex = 2;
            this.grpBackup.Text = "数据备份与恢复";
            this.grpBackup.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.btnRestore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestore.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnRestore.Location = new System.Drawing.Point(490, 120);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(280, 40);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "恢复数据";
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            
            this.btnBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackup.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnBackup.Location = new System.Drawing.Point(190, 120);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(280, 40);
            this.btnBackup.TabIndex = 1;
            this.btnBackup.Text = "备份数据";
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            
            this.lblBackupDesc.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblBackupDesc.Location = new System.Drawing.Point(40, 50);
            this.lblBackupDesc.Name = "lblBackupDesc";
            this.lblBackupDesc.Size = new System.Drawing.Size(880, 50);
            this.lblBackupDesc.TabIndex = 0;
            this.lblBackupDesc.Text = "备份所有数据文件到指定位置，或从备份文件夹恢复数据。建议定期备份重要数据。";
            this.lblBackupDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 660);
            this.Controls.Add(this.grpBackup);
            this.Controls.Add(this.grpImport);
            this.Controls.Add(this.grpExport);
            this.Name = "FrmDataImportExport";
            this.Text = "数据导入导出";
            this.grpExport.ResumeLayout(false);
            this.grpImport.ResumeLayout(false);
            this.grpBackup.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Sunny.UI.UIGroupBox grpExport;
        private Sunny.UI.UIButton btnExportRecords;
        private Sunny.UI.UIButton btnExportTools;
        private Sunny.UI.UIButton btnExportEquipments;
        private Sunny.UI.UILabel lblExportDesc;
        private Sunny.UI.UIGroupBox grpImport;
        private Sunny.UI.UIButton btnImportTools;
        private Sunny.UI.UIButton btnImportEquipments;
        private Sunny.UI.UILabel lblImportDesc;
        private Sunny.UI.UIGroupBox grpBackup;
        private Sunny.UI.UIButton btnRestore;
        private Sunny.UI.UIButton btnBackup;
        private Sunny.UI.UILabel lblBackupDesc;
    }
}
