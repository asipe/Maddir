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
    public void TestExecuteWithNoDirectoriesOrFilesGivesEmptyLayout() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA<string>());
      mDirectory
        .Setup(d => d.GetFiles("root", "*"))
        .Returns(BA<string>());
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout()));
    }

    [Test]
    public void TestExecuteWithSingleDirectoryGivesSingleResult() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA(@"c:\root\dir1"));
      mDirectory
        .Setup(d => d.GetFiles("root", "*"))
        .Returns(BA<string>());
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir1"))));
    }

    [Test]
    public void TestExecuteWithMultipleDirectoriesGivesMultipleResults() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA(@"c:\root\dir1",
                    @"c:\root\dir2",
                    @"c:\root\dir3"));
      mDirectory
        .Setup(d => d.GetFiles("root", "*"))
        .Returns(BA<string>());
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir1"),
                                                                  new AddDirectoryCommand(0, "dir2"),
                                                                  new AddDirectoryCommand(0, "dir3"))));
    }

    [Test]
    public void TestExecuteWithSingleFileGivesSingleResult() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA<string>());
      mDirectory
        .Setup(d => d.GetFiles("root", "*"))
        .Returns(BA(@"c:\root\file1.txt"));
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddFileCommand(0, "file1.txt"))));
    }

    [Test]
    public void TestExecuteWithMultipleFilesGivesMultipleResult() {
      mDirectory
        .Setup(d => d.GetDirectories("root"))
        .Returns(BA<string>());
      mDirectory
        .Setup(d => d.GetFiles("root", "*"))
        .Returns(BA(@"c:\root\file1.txt", @"c:\root\file2.txt", @"c:\root\file3.txt"));
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout(new AddFileCommand(0, "file1.txt"),
                                                                  new AddFileCommand(0, "file2.txt"),
                                                                  new AddFileCommand(0, "file3.txt"))));
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