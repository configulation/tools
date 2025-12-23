using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Sunny.UI;

namespace WinFormsApp1
{
	public partial class FrmMain : UIHeaderMainFrame
    {
		private readonly Dictionary<int, string> pageIndexToType = new Dictionary<int, string>();
		private readonly Dictionary<string, TreeNode> pathNodes = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);

        public FrmMain()
        {
            InitializeComponent();

			// 标签页设置
			this.TabVisible = true;
			this.TabShowCloseButton = true;
			this.TabShowActiveCloseButton = true;

			// Header
			this.Header.Text = "系统主界面";
			this.Header.TabControl = this.MainTabControl; // 显式绑定
			this.Header.NodeMouseClick += Header_NodeMouseClick; // 通过节点点击拿到Tag进行懒加载
			this.Header.MenuItemClick += Header_MenuItemClick;  // 更新Header文本

			// 从配置加载菜单（不立即创建页面）
			BuildMenuFromConfig();
		}

		private void Header_MenuItemClick(string itemText, int menuIndex, int pageIndex)
		{
			if (!string.IsNullOrWhiteSpace(itemText)) this.Header.Text = itemText;
		}

		private void Header_NodeMouseClick(System.Windows.Forms.TreeNode node, int menuIndex, int pageIndex)
		{
			if (pageIndex <= 0) return; // 分组节点
			EnsurePageLoaded(pageIndex, node);
			SelectPage(pageIndex);
		}

		private void EnsurePageLoaded(int pageIndex, System.Windows.Forms.TreeNode node)
		{
			if (ExistPage(pageIndex)) return;
			string typeName = null;
			// 优先从缓存
			if (pageIndexToType.TryGetValue(pageIndex, out var cached)) typeName = cached;
			// 再从节点携带信息（NavMenuItem.Tag）
			if (string.IsNullOrEmpty(typeName))
			{
				if (node?.Tag is NavMenuItem item && item.Tag is string s && !string.IsNullOrWhiteSpace(s))
					typeName = s;
			}
			if (string.IsNullOrEmpty(typeName)) return;

			var page = InstantiatePage(typeName);
			if (page == null) return;
			if (node?.Tag is NavMenuItem ni && !string.IsNullOrWhiteSpace(ni.Text))
			{
				page.Text = ni.Text;
			}
			AddPage(page, pageIndex);
		}

		private void BuildMenuFromConfig()
		{
			// 清空缓存
			pageIndexToType.Clear();
			pathNodes.Clear();
			Header.ClearAll();

			List<PageConfig> items = null;
			try
			{
				string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pages.json");
				if (File.Exists(configPath))
				{
					var json = File.ReadAllText(configPath);
					items = JsonConvert.DeserializeObject<List<PageConfig>>(json);
				}
			}
			catch { }

			if (items == null || items.Count == 0)
			{
				// 退化到默认：只挂菜单，不立即创建页面
				var cfg = new PageConfig { Text = "Excel数据导入", PageIndex = 1001, Type = typeof(Form1).FullName, Path = "数据导入" };
				CreateMenuNode(cfg);
				return;
			}

			foreach (var cfg in items.OrderBy(x => x.PageIndex))
			{
				CreateMenuNode(cfg);
			}

			// 处理 AlwaysOpen：这些页面启动即创建
			foreach (var cfg in items.Where(x => x.AlwaysOpen && x.PageIndex > 0))
			{
				EnsurePageLoaded(cfg.PageIndex, FindNodeByPageIndex(cfg.PageIndex));
			}
		}

		private System.Windows.Forms.TreeNode FindNodeByPageIndex(int pageIndex)
		{
			// UINavBar内部有映射，但未暴露直接查找；通过遍历 Header.Nodes 简单查找
			foreach (System.Windows.Forms.TreeNode n in Header.Nodes)
			{
				if (n.Tag is NavMenuItem it && it.PageIndex == pageIndex) return n;
				foreach (System.Windows.Forms.TreeNode c in n.Nodes)
				{
					if (c.Tag is NavMenuItem it2 && it2.PageIndex == pageIndex) return c;
					foreach (System.Windows.Forms.TreeNode d in c.Nodes)
					{
						if (d.Tag is NavMenuItem it3 && it3.PageIndex == pageIndex) return d;
					}
				}
			}
			return null;
		}

		private void CreateMenuNode(PageConfig cfg)
		{
			if (cfg == null || cfg.PageIndex <= 0 || string.IsNullOrWhiteSpace(cfg.Type)) return;
			pageIndexToType[cfg.PageIndex] = cfg.Type;

			// 解析多级路径（仅分组，不包含叶子文本）
			TreeNode parent = null;
			string path = cfg.Path;
			if (string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(cfg.Group))
			{
				path = cfg.Group; // 兼容旧Group
			}
			if (!string.IsNullOrWhiteSpace(path))
			{
				var segments = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				string acc = string.Empty;
				for (int i = 0; i < segments.Length; i++)
				{
					acc = acc.Length == 0 ? segments[i] : acc + "/" + segments[i];
					if (!pathNodes.TryGetValue(acc, out var node))
					{
						if (i == 0)
							node = Header.CreateNode(segments[i], -1);
						else
							node = Header.CreateChildNode(parent, segments[i], -1);
						pathNodes[acc] = node;
					}
					parent = node;
				}
			}

			string leafText = !string.IsNullOrWhiteSpace(cfg.Text) ? cfg.Text : ($"Page{cfg.PageIndex}");
			TreeNode leaf;
			if (parent != null)
			{
				leaf = Header.CreateChildNode(parent, leafText, cfg.PageIndex);
			}
			else
			{
				leaf = Header.CreateNode(leafText, cfg.PageIndex);
			}

			// 将类型信息保存到 NavMenuItem.Tag，便于懒加载实例化
			if (leaf.Tag is NavMenuItem item)
			{
				item.Tag = cfg.Type;
				if (cfg.Symbol > 0)
				{
					Header.SetNodeSymbol(leaf, cfg.Symbol, 24);
				}
			}
		}

		private UIPage InstantiatePage(string typeFullName)
		{
			if (string.IsNullOrWhiteSpace(typeFullName)) return null;
			// 规范化：命名空间中不应包含空格，部分目录名带空格但命名空间通常以下划线声明
			string normalized = typeFullName.Trim().Replace(" ", "_");

			Type type = Type.GetType(normalized, false) ?? Type.GetType(typeFullName, false);
			if (type == null)
			{
				var assemblies = AppDomain.CurrentDomain.GetAssemblies();
				type = assemblies
					.SelectMany(a => SafeGetTypes(a))
					.FirstOrDefault(t => string.Equals(t.FullName, normalized, StringComparison.Ordinal)
						|| string.Equals(t.FullName, typeFullName, StringComparison.Ordinal));

				// 兜底：仅用类名匹配（可能存在同名类冲突，实际项目中建议配置完整限定名）
				if (type == null)
				{
					string simple = normalized.Split('.').Last();
					type = assemblies
						.SelectMany(a => SafeGetTypes(a))
						.FirstOrDefault(t => string.Equals(t.Name, simple, StringComparison.Ordinal));
				}
			}

			if (type == null) return null;
			if (typeof(UIPage).IsAssignableFrom(type))
			{
				return (UIPage)Activator.CreateInstance(type);
			}
			if (typeof(Form).IsAssignableFrom(type))
			{
				var frm = (Form)Activator.CreateInstance(type);
				return new FormWrapperPage(frm);
			}
			return null;
		}

		private static IEnumerable<Type> SafeGetTypes(System.Reflection.Assembly a)
		{
			try { return a.GetTypes(); }
			catch { return Array.Empty<Type>(); }
		}

		private class PageConfig
		{
			public string Text { get; set; }
			public int PageIndex { get; set; }
			public string Type { get; set; }
			public int Symbol { get; set; }
			public bool AlwaysOpen { get; set; }
			public string Group { get; set; }
			public string Path { get; set; }
        }
    }
}
