using Maddir.Core.Commands;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Commands {
  [TestFixture]
  public class AddDirectoryCommandTest : BaseTestCase {
    [Test]
    public void TestDefault() {
      var cmd = new AddDirectoryCommand("apath");
      AssertAreEqual(cmd.Properties, new DirectoryProperties("apath"));
      Assert.That(cmd.Properties, Are.EqualTo(new DirectoryProperties("apath")));
    }

    [Test]
    public void TestVisit() {
      var command = CA<AddDirectoryCommand>();
      var visitor = Mok<ICommandVisitor>();
      visitor.Setup(v => v.Visit(command));
      command.Accept(visitor.Object);
    }
  }
}