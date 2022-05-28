using System.Windows.Forms;
using SharpShell.SharpContextMenu;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace ShellExtension_ContextMenu
{
    [ComVisible(true)]
    //如果按文件类型，按以下设置
    //[COMServerAssociation(AssociationType.ClassOfExtension, ".xlsx", ".xls")]
    //设置对全部文件和目录可用
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.DesktopBackground)]
    [COMServerAssociation(AssociationType.DirectoryBackground)]
    [COMServerAssociation(AssociationType.Folder)]
    public class GitExtension : SharpContextMenu
    {
        //创建的菜单
        private ContextMenuStrip menu = new ContextMenuStrip();

        private GitCommand git = new GitCommand();

        /// <summary>
        /// Determines whether this instance can a shell
        /// context show menu, given the specified selected file list
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance should show a shell context
        /// menu for the specified file list; otherwise, <c>false</c>
        /// </returns>
        protected override bool CanShowMenu()
        {
            //TODO: 如果不是git仓库，不显示选项
            this.UpdateMenu();
            return true;
        }

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            menu.Items.Clear();
            this.MenuDefault();
            if (SelectedItemPaths.Count() > 0)
            {
                FileAttributes attr = File.GetAttributes(this.SelectedItemPaths.First());
                //判断文件还是文件夹 也可以判断文件扩展名 甚至解析文件内容 实现类似于压缩软件的效果
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    this.MenuDirectory();
                }
                else
                {
                    this.MenuFiles();
                }
            }
            return menu;
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        private void UpdateMenu()
        {
            menu.Dispose();
            menu = CreateMenu();
        }

        protected void MenuDefault()
        {
            // 主菜单
            var mainMenu = new ToolStripMenuItem
            {
                Text = "Git Extension",
                //Image = FastGit.Properties.Resources.arrow_undo
            };

            // 次级菜单
            Dictionary<string, string> subItemInfo = new Dictionary<string, string>()
            {
                { "Clone", "clone"},
                { "Revert", "revert"},
                { "Cleanup", "cleanup"},
                { "Update", "update"}
            };
            foreach (KeyValuePair<string, string> kv in subItemInfo)
            {
                var subItem = new ToolStripMenuItem(kv.Key);
                subItem.Click += (o, e) =>
                {
                    //添加监听事件
                    Item_Click(o, e, kv.Value);
                };
                mainMenu.DropDownItems.Add(subItem);
            }

            menu.Items.Add(mainMenu);
        }

        //菜单动作
        private void Item_Click(object sender, EventArgs e, string arg)
        {
            string appFile = Path.Combine(rootPath, "AuroraTools.exe");
            if (!File.Exists(appFile))
            {
                MessageBox.Show(string.Format("找不到程序路径:{0}{1}", Environment.NewLine, appFile), "出错了", MessageBoxButtons.OK);
                return;
            }
            List<string> paths = SelectedItemPaths.ToList();
            paths.Add(arg);
            string args = string.Join(" ", paths);
            Process.Start(appFile, args);
        }

        //获取当前dll所在路径
        private string rootPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// 创建目录的菜单
        /// </summary>
        protected void MenuDirectory()
        {
        }

        // <summary>
        // 创建文件的菜单
        // </summary>
        protected void MenuFiles()
        {
        }
    }
}

//mainMenu.Click += (sender, args) =>
//{
//    if (SelectedItemPaths.Count() > 0)
//    {
//        foreach (var item in SelectedItemPaths)
//        {
//            var FilePath = item.Trim();
//            string WorkingDirectory;
//            if (Directory.Exists(FilePath)) // 路径是文件夹
//            {
//                WorkingDirectory = FilePath;
//                FilePath = ".";
//            }
//            else
//            {
//                WorkingDirectory = Path.GetDirectoryName(FilePath);
//            }
//            ExecuteGitCommand("restore " + FilePath, WorkingDirectory);                        
//        }
//    }
//    else
//    {
//        var FilePath = FolderPath.Trim();
//        string WorkingDirectory;
//        if (Directory.Exists(FilePath)) // 路径是文件夹
//        {
//            WorkingDirectory = FilePath;
//            FilePath = ".";
//        }
//        else
//        {
//            WorkingDirectory = Path.GetDirectoryName(FilePath);
//        }
//        ExecuteGitCommand("restore " + FilePath, WorkingDirectory);
//    }
//};