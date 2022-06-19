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
        private static StringBuilder strOutput = null;

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
        /// 执行git指令
        /// </summary>
        public static int ExcuteGitCommand(string strCommnad, string strWorkingDir, out string strOutLines)
        {
            var strGitPath = System.IO.Path.Combine(GitPath, "git.exe");

            if (string.IsNullOrEmpty(strGitPath))
            {
                strOutLines = ">>>>>strEnvironmentVariable: enviromentVariable is not config!!!!";
                return -1;
            }

            Process p = new Process();
            p.StartInfo.FileName = strGitPath;
            p.StartInfo.Arguments = strCommnad;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory = strWorkingDir;
            strOutput = new StringBuilder();
            p.OutputDataReceived += OnOutputDataReceived;

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            int exitcode = p.ExitCode;
            p.Close();
            strOutLines = strOutput.ToString();
            return exitcode;
        }

        /// <summary>
        /// 输出git指令执行结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                strOutput.Append(e.Data);
            }
        }
    }
}
