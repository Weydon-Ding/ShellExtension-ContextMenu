using System;
using System.Windows.Forms;
using SharpShell.Helpers;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName = @".\";
            string destFileName = Environment.Is64BitOperatingSystem ?
                @"C:\Program Files\MyGit" :
                @"C:\Program Files (x86)\MyGit";
            string[] copyFiles = new string[5];
            copyFiles[0] = "MyGit.dll";
            copyFiles[1] = "SharpShell.dll";
            copyFiles[2] = "SharpShell.xml";
            copyFiles[3] = "uninstall.exe";
            copyFiles[4] = "arrow_undo.png";

            System.IO.Directory.CreateDirectory(destFileName);
            foreach (var copyFile in copyFiles)
            {
                System.IO.File.Copy(System.IO.Path.Combine(sourceFileName, copyFile),
                    System.IO.Path.Combine(destFileName, copyFile), true);
            }

            string MyGit = System.IO.Path.Combine(destFileName, "MyGit.dll");
            var regasm = new RegAsm();
            var success = Environment.Is64BitOperatingSystem ?
                regasm.Register64(MyGit, true) :
                regasm.Register32(MyGit, true);


            if (success)
            {
                MessageBox.Show("successfully");
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }
    }
}
