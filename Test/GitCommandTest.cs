using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ShellExtension_ContextMenu;

namespace Test
{
    [TestClass]
    public class GitCommandTest
    {
        [TestMethod]
        public void TestGitPath()
        {
            Assert.IsTrue(GitCommand.GitPath.CompareTo(@"C:\Program Files\Git\cmd") == 0);
        }

        [TestMethod]
        public void TestExcuteGitCommand()
        {
            string output;

            GitCommand.ExcuteGitCommand("--version", "./", out output);
            Assert.IsTrue(output.CompareTo(@"git version 2.36.0.windows.1") == 0);

            GitCommand.ExcuteGitCommand("rev-parse --is-inside-work-tree", "./", out output);
            Assert.IsTrue(output.CompareTo("true") == 0);
        }
    }
}
