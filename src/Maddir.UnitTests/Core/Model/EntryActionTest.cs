using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class EntryActionTest : BaseTestCase {
    [Test]
    public void TestExecuteWithFileEntryCallsCorrectAction() {
      var entry = CA<FileEntry>();
      IEntry actual = null;
      new EntryAction(e => actual = e,
                      e => Assert.Fail("Should not be called"))
        .Execute(entry);
      Assert.That(actual, Is.EqualTo(entry));
    }

    [Test]
    public void TestExecuteWithDirectoryEntryCallsCorrectAction() {
      var entry = CA<DirectoryEntry>();
      IEntry actual = null;
      new EntryAction(e => Assert.Fail("Should not be called"),
                      e => actual = e)
        .Execute(entry);
      Assert.That(actual, Is.EqualTo(entry));
    }
  }
}