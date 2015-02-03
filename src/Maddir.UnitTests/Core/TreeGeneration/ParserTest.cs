// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using Maddir.Core.Utility;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.TreeGeneration {
  [TestFixture]
  public class ParserTest : BaseTestCase {
    [TestCase(new string[0], "")]
    [TestCase(new[] {""}, "")]
    [TestCase(new[] {" "}, "")]
    [TestCase(new[] {" ", "", "     "}, "")]
    public void TestEmptyMarkupGivesEmptyLayout(string[] lines, string dummy) {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString(lines));
      Assert.That(layout, Are.EqualTo(new Layout()));
    }

    [Test]
    public void TestSingleDirectoryMarkupGivesLayout() {
      var layout = mParser.Parse("d  dir0");
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"))));
    }

    [Test]
    public void TestMultipleDirectoryMarkupGivesLayout() {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString("d  dir0",
                                                                "d  dir1",
                                                                "d  dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(0, "dir1"),
                                                 new AddDirectoryCommand(0, "dir2"))));
    }

    [Test]
    public void TestSingleFileMarkupGivesLayout() {
      var layout = mParser.Parse("f  file0.txt");
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt"))));
    }

    [Test]
    public void TestMultipleFileMarkupGivesLayout() {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString("f  file0.txt",
                                                                "f  file1.txt",
                                                                "f  file2.txt"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt"),
                                                 new AddFileCommand(0, "file1.txt"),
                                                 new AddFileCommand(0, "file2.txt"))));
    }

    [Test]
    public void TestMultipleMixedMarkupGivesLayout() {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString("f  file0.txt",
                                                                "f  file1.txt",
                                                                "f  file2.txt",
                                                                "d  dir0",
                                                                "d  dir1",
                                                                "d  dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt"),
                                                 new AddFileCommand(0, "file1.txt"),
                                                 new AddFileCommand(0, "file2.txt"),
                                                 new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(0, "dir1"),
                                                 new AddDirectoryCommand(0, "dir2"))));
    }

    [Test]
    public void TestDirectoryTreeMarkupGivesLayout() {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString("d  dir0",
                                                                "d    dir1",
                                                                "d      dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(1, @"dir0\dir1"),
                                                 new AddDirectoryCommand(2, @"dir0\dir1\dir2"))));
    }

    [Test]
    public void TestDirectoryTreeWithFilesMarkupGivesLayout() {
      var layout = mParser.Parse(StringUtils.ToNewLineSepString("f  file0.txt",
                                                                "d  dir0",
                                                                "f    file1.txt",
                                                                "d    dir1",
                                                                "f      file2.txt",
                                                                "d      dir2",
                                                                "f        file3.txt"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt"),
                                                 new AddDirectoryCommand(0, "dir0"),
                                                 new AddFileCommand(1, @"dir0\file1.txt"),
                                                 new AddDirectoryCommand(1, @"dir0\dir1"),
                                                 new AddFileCommand(2, @"dir0\dir1\file2.txt"),
                                                 new AddDirectoryCommand(2, @"dir0\dir1\dir2"),
                                                 new AddFileCommand(3, @"dir0\dir1\dir2\file3.txt"))));
    }

    [SetUp]
    public void DoSetup() {
      mParser = new Parser();
    }

    private Parser mParser;
  }
}