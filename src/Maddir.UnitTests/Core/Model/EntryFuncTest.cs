using System;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class EntryFuncTest : BaseTestCase {
    [Test]
    public void TestExecuteWithFileEntryCallsCorrectFunc() {
      var entry = CA<FileEntry>();
      var actual = new EntryFunc<IEntry>(e => e,
                                         e => {throw new Exception("Should not be called");})
        .Execute(entry);
      Assert.That(actual, Is.EqualTo(entry));
    }

    [Test]
    public void TestExecuteWithDirectoryEntryCallsCorrectFunc() {
      var entry = CA<DirectoryEntry>();
      var actual = new EntryFunc<IEntry>(e => {throw new Exception("Should not be called");},
                                         e => e)
        .Execute(entry);
      Assert.That(actual, Is.EqualTo(entry));
    }
  }
}