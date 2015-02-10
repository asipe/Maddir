// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Maddir.Core.Commands;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using Moq;
using NUnit.Framework;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.UnitTests.Core.TreeGeneration {
  [TestFixture]
  public class EngineTest : BaseTestCase {
    [Test]
    public void TestApplyWithEmptyLayout() {
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout());
    }

    [Test]
    public void TestApplyWithSingleAddDirectoryCommand() {
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir0"))
        .Returns<DirectoryInfo>(null);
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddDirectoryCommand(0, "dir0")));
    }

    [Test]
    public void TestApplyWithMultipleAddDirectoryCommands() {
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir0"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir1"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir2"))
        .Returns<DirectoryInfo>(null);
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddDirectoryCommand(0, "dir0"),
                                                           new AddDirectoryCommand(0, "dir1"),
                                                           new AddDirectoryCommand(0, "dir2")));
    }

    [Test]
    public void TestApplyWithSingleAddFileCommand() {
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file0.txt", ""));
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddFileCommand(0, "file0.txt")));
    }

    [Test]
    public void TestApplyWithMultipleAddFileCommands() {
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file0.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file1.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file2.txt", ""));
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddFileCommand(0, "file0.txt"),
                                                           new AddFileCommand(0, "file1.txt"),
                                                           new AddFileCommand(0, "file2.txt")));
    }

    [Test]
    public void TestApplyWithMixedCommands() {
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir0"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir1"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir2"))
        .Returns<DirectoryInfo>(null);
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file0.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file1.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file2.txt", ""));
      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddDirectoryCommand(0, "dir0"),
                                                           new AddDirectoryCommand(0, "dir1"),
                                                           new AddDirectoryCommand(0, "dir2"),
                                                           new AddFileCommand(0, "file0.txt"),
                                                           new AddFileCommand(0, "file1.txt"),
                                                           new AddFileCommand(0, "file2.txt")));
    }

    [Test]
    public void TestApplyWithMixedCommandsWithEventHandlers() {
      var seen = new List<string>();
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir0"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir1"))
        .Returns<DirectoryInfo>(null);
      mDirectory
        .Setup(d => d.CreateDirectory(@"c:\app1\data\dir2"))
        .Returns<DirectoryInfo>(null);
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file0.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file1.txt", ""));
      mFile
        .Setup(f => f.WriteAllText(@"c:\app1\data\file2.txt", ""));

      mSettings
        .OnDirectoryCreated += (sender, args) => seen.Add(args.Info.FullName);
      mSettings
        .OnFileCreated += (sender, args) => seen.Add(args.Info.FullName);

      mEngine.Apply(mSettings, @"c:\app1\data", new Layout(new AddDirectoryCommand(0, "dir0"),
                                                           new AddDirectoryCommand(0, "dir1"),
                                                           new AddDirectoryCommand(0, "dir2"),
                                                           new AddFileCommand(0, "file0.txt"),
                                                           new AddFileCommand(0, "file1.txt"),
                                                           new AddFileCommand(0, "file2.txt")));

      Assert.That(seen, Is.EqualTo(BA(@"c:\app1\data\dir0",
                                      @"c:\app1\data\dir1",
                                      @"c:\app1\data\dir2",
                                      @"c:\app1\data\file0.txt",
                                      @"c:\app1\data\file1.txt",
                                      @"c:\app1\data\file2.txt")));
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mDirectory = Mok<IDirectory>();
      mSettings = new Settings();
      mEngine = new Engine(mDirectory.Object, mFile.Object);
    }

    private Engine mEngine;
    private Mock<IDirectory> mDirectory;
    private Mock<IFile> mFile;
    private Settings mSettings;
  }
}