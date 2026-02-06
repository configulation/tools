using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmMaintenanceItemSelector : UIForm
    {
        private readonly BaseDataService _baseDataService;
        private readonly string _type;
        public List<string> SelectedItems { get; private set; }

        public FrmMaintenanceItemSelector(string type, List<string> currentItems)
        {
            InitializeComponent();
            _baseDataService = new BaseDataService();
            _type = type;
            SelectedItems = new List<string>(currentItems ?? new List<string>());
            
            LoadMaintenanceItems();
        }

        private void LoadMaintenanceItems()
        {
            var items = _baseDataService.GetActiveMaintenanceItemNames(_type);
            
            clbItems.Items.Clear();
            foreach (var item in items)
            {
                int index = clbItems.Items.Add(item);
                if (SelectedItems.Contains(item))
                {
                    clbItems.SetItemChecked(index, true);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SelectedItems.Clear();
            foreach (var item in clbItems.CheckedItems)
            {
                SelectedItems.Add(item.ToString());
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbItems.Items.Count; i++)
            {
                clbItems.SetItemChecked(i, true);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbItems.Items.Count; i++)
            {
                clbItems.SetItemChecked(i, false);
            }
        }
    }
}
