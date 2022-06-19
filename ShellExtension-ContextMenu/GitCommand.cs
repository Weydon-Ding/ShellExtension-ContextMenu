using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellExtension_ContextMenu
{
    public class GitCommand
    {

        /// <summary>
        /// 获取环境git.ext的环境变量路径
        /// </summary>
        public static string GitPath
        {
            get
            {
                string strPath = System.Environment.GetEnvironmentVariable("Path");
                if (string.IsNullOrEmpty(strPath))
                {
                    return null;
                }

                string[] strResults = strPath.Split(';');
                foreach (var path in strResults)
                {
                    if (!path.Contains(@"Git\cmd"))
                        continue;

                    return path;                    
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// git工作路径
        /// </summary>
        private static string m_strWorkingDir;
        public static string strWorkingDir
        {
            get { return m_strWorkingDir; }
            set { m_strWorkingDir = value; }
        }


        /// <summary>
        /// 执行git指令
        /// </summary>
        public static void ExcuteGitCommand(string strCommnad, DataReceivedEventHandler call)
        {
            var strGitPath = System.IO.Path.Combine(GitPath, "git.exe");
            if (string.IsNullOrEmpty(strGitPath))
            {
                Console.WriteLine(">>>>>strEnvironmentVariable: enviromentVariable is not config!!!!");
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = strGitPath;
            p.StartInfo.Arguments = strCommnad;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory = strWorkingDir;

            p.OutputDataReceived += call;
            p.OutputDataReceived -= OnOutputDataReceived;
            p.OutputDataReceived += OnOutputDataReceived;

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
        }

        /// <summary>
        /// 输出git指令执行结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (null == e || string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(">>>>>>Git command error!!!!!");
                return;
            }

            Console.WriteLine(e.Data);
        }
    }
}
