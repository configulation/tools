using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Controls
{
    /// <summary>
    /// 支持模糊搜索的下拉框辅助类
    /// </summary>
    public static class UIComboBoxExtensions
    {
        private static Dictionary<UIComboBox, List<string>> allItemsCache = new Dictionary<UIComboBox, List<string>>();
        private static Dictionary<UIComboBox, bool> isFilteringCache = new Dictionary<UIComboBox, bool>();

        public static void EnableSearch(this UIComboBox comboBox, List<string> items)
        {
            allItemsCache[comboBox] = items ?? new List<string>();
            isFilteringCache[comboBox] = false;

            comboBox.DropDownStyle = UIDropDownStyle.DropDown;
            comboBox.Items.Clear();
            comboBox.Items.AddRange(allItemsCache[comboBox].ToArray());

            comboBox.TextChanged += (sender, e) =>
            {
                if (isFilteringCache.ContainsKey(comboBox) && isFilteringCache[comboBox])
                    return;

                isFilteringCache[comboBox] = true;

                try
                {
                    string searchText = comboBox.Text;
                    int cursorPosition = comboBox.SelectionStart;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        comboBox.Items.Clear();
                        comboBox.Items.AddRange(allItemsCache[comboBox].ToArray());
                    }
                    else
                    {
                        var filtered = allItemsCache[comboBox].Where(item =>
                            item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                        ).ToArray();

                        comboBox.Items.Clear();
                        if (filtered.Length > 0)
                        {
                            comboBox.Items.AddRange(filtered);
                        }
                    }

                    comboBox.Text = searchText;
                    comboBox.SelectionStart = cursorPosition;
                    comboBox.SelectionLength = 0;
                }
                finally
                {
                    isFilteringCache[comboBox] = false;
                }
            };
        }

        public static void UpdateSearchDataSource(this UIComboBox comboBox, List<string> items)
        {
            if (allItemsCache.ContainsKey(comboBox))
            {
                allItemsCache[comboBox] = items ?? new List<string>();
                comboBox.Items.Clear();
                comboBox.Items.AddRange(allItemsCache[comboBox].ToArray());
            }
        }
    }
}
