// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Generation;
using Maddir.Core.Model;
using Moq;
using NUnit.Framework;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.UnitTests.Core.Generation {
  [TestFixture]
  public class DirectoryBrowserTest : BaseTestCase {
    [Test]
    public void TestExecuteWithNoDirectoriesGivesEmptyLayout() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA<string>());
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout()));
    }

    [Test]
    public void TestExecuteWithSingleDirectoryGivesSingleResult() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA(@"c:\root\dir1"));
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddDirectoryCommand("dir1"))));
    }

    [Test]
    public void TestExecuteWithMultipleDirectoriesGivesMultipleResults() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA(@"c:\root\dir1",
                    @"c:\root\dir2",
                    @"c:\root\dir3"));
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddDirectoryCommand("dir1"),
                                                                  new AddDirectoryCommand("dir2"),
                                                                  new AddDirectoryCommand("dir3"))));
    }

    [SetUp]
    public void DoSetup() {
      mDirectory = Mok<IDirectory>();
      mBrowser = new DirectoryBrowser(mDirectory.Object);
    }

    private Mock<IDirectory> mDirectory;
    private DirectoryBrowser mBrowser;
  }
}