// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Maddir.Core.Commands;
using Maddir.Core.MarkupGeneration;
using Maddir.Core.Model;
using Moq;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.UnitTests.Core.MarkupGeneration {
  [TestFixture]
  public class DirectoryBrowserTest : BaseTestCase {
    [Test]
    public void TestExecuteWithNoDirectoriesOrFilesGivesEmptyLayout() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => TriggerEvents(config,
                                                  BA(""),
                                                  BA<string>()));
      Assert.That(mBrowser.Browse("root"), Are.EqualTo(new Layout()));
    }

    [Test]
    public void TestExecuteWithSingleDirectoryGivesSingleResult() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => TriggerEvents(config,
                                                  BA("",
                                                     @"c:\root\dir1"),
                                                  BA<string>()));
      Assert.That(mBrowser.Browse(@"c:\root"), Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir1"))));
    }

    [Test]
    public void TestExecuteWithMultipleDirectoriesGivesMultipleResults() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => TriggerEvents(config,
                                                  BA("",
                                                     @"c:\root\dir1",
                                                     @"c:\root\dir2",
                                                     @"c:\root\dir3"),
                                                  BA<string>()));
      Assert.That(mBrowser.Browse(@"c:\root"), Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir1"),
                                                                      new AddDirectoryCommand(0, "dir2"),
                                                                      new AddDirectoryCommand(0, "dir3"))));
    }

    [Test]
    public void TestExecuteWithSingleFileGivesSingleResult() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => TriggerEvents(config,
                                                  BA(""),
                                                  BA(@"c:\root\file1.txt")));

      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file1.txt"))
        .Returns("file1");

      Assert.That(mBrowser.Browse(@"c:\root"), Are.EqualTo(new Layout(new AddFileCommand(0, "file1.txt", "file1"))));
    }

    [Test]
    public void TestExecuteWithMultipleFilesGivesMultipleResult() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => TriggerEvents(config,
                                                  BA(""),
                                                  BA(@"c:\root\file1.txt",
                                                     @"c:\root\file2.txt",
                                                     @"c:\root\file3.txt")));

      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file1.txt"))
        .Returns("file1");
      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file2.txt"))
        .Returns("file2");
      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file3.txt"))
        .Returns("file3");

      Assert.That(mBrowser.Browse(@"c:\root"), Are.EqualTo(new Layout(new AddFileCommand(0, "file1.txt", "file1"),
                                                                      new AddFileCommand(0, "file2.txt", "file2"),
                                                                      new AddFileCommand(0, "file3.txt", "file3"))));
    }

    [Test]
    public void TestExecuteWithMultipleFilesAndMultipleDirectoriesGivesMultipleResult() {
      mScanner
        .Setup(s => s.Start(It.IsAny<Config>()))
        .Callback<Config>(config => {
                            TriggerEvents(config,
                                          BA(""),
                                          BA(@"c:\root\file1.txt",
                                             @"c:\root\file2.txt",
                                             @"c:\root\file3.txt"));
                            TriggerEvents(config,
                                          BA(@"c:\root\dir1",
                                             @"c:\root\dir2",
                                             @"c:\root\dir3"),
                                          BA<string>());
                          });

      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file1.txt"))
        .Returns("file1");
      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file2.txt"))
        .Returns("file2");
      mFile
        .Setup(f => f.ReadAllText(@"c:\root\file3.txt"))
        .Returns("file3");

      Assert.That(mBrowser.Browse(@"c:\root"), Are.EqualTo(new Layout(new AddFileCommand(0, "file1.txt", "file1"),
                                                                      new AddFileCommand(0, "file2.txt", "file2"),
                                                                      new AddFileCommand(0, "file3.txt", "file3"),
                                                                      new AddDirectoryCommand(0, "dir1"),
                                                                      new AddDirectoryCommand(0, "dir2"),
                                                                      new AddDirectoryCommand(0, "dir3"))));
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mScanner = Mok<IScanner>();
      mBrowser = new DirectoryBrowser(mScanner.Object, mFile.Object);
    }

    private static void TriggerEvents(Config config, string[] directories, string[] files) {
      Array.ForEach(directories, dir => config.Handlers.HandleDirectory(new DirectoryVisitEventArgs(dir)));
      Array.ForEach(files, file => config.Handlers.HandleFile(new FileVisitEventArgs(file)));
    }

    private Mock<IScanner> mScanner;
    private DirectoryBrowser mBrowser;
    private Mock<IFile> mFile;
  }
}