using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ShellExtension_ContextMenu;

namespace Test
{
    [TestClass]
    public class GitCommandTest
    {
        GitCommand git = new GitCommand();

        [TestMethod]
        public void TestGitPath()
        {
            Assert.IsTrue(GitCommand.GitPath.CompareTo(@"C:\Program Files\Git\cmd") == 0);
        }
    }
}
