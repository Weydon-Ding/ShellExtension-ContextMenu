using System;
using System.Windows.Forms;
using SharpShell.Diagnostics;
using SharpShell.Helpers;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string installPath = Environment.Is64BitOperatingSystem ?
                @"C:\Program Files\MyGit\MyGit.dll" :
                @"C:\Program Files (x86)\MyGit\MyGit.dll";
            var regasm = new RegAsm();
            var success = Environment.Is64BitOperatingSystem ?
                regasm.Unregister64(installPath) :
                regasm.Unregister64(installPath);
            if (success)
            {
                MessageBox.Show("successfully");
            }
            else
            {
                MessageBox.Show("Failed");
            }
            ExplorerManager.RestartExplorer();
        }
    }
}
