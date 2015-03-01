// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core;
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
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString(lines));
      Assert.That(layout, Are.EqualTo(new Layout()));
    }

    [Test]
    public void TestSingleDirectoryMarkupGivesLayout() {
      var layout = _Parser.Parse("d  dir0");
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"))));
    }

    [Test]
    public void TestMultipleDirectoryMarkupGivesLayout() {
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString("d  dir0",
                                                                "d  dir1",
                                                                "d  dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(0, "dir1"),
                                                 new AddDirectoryCommand(0, "dir2"))));
    }

    [Test]
    public void TestSingleFileMarkupGivesLayout() {
      var layout = _Parser.Parse("f  file0.txt []");
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt", ""))));
    }

    [Test]
    public void TestMultipleFileMarkupGivesLayout() {
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString("f  file0.txt [0]",
                                                                "f  file1.txt [1]",
                                                                "f  file2.txt [2]"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt", "0"),
                                                 new AddFileCommand(0, "file1.txt", "1"),
                                                 new AddFileCommand(0, "file2.txt", "2"))));
    }

    [Test]
    public void TestMultipleMixedMarkupGivesLayout() {
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString("f  file0.txt [file0]",
                                                                "f  file1.txt [file1]",
                                                                "f  file2.txt [file2]",
                                                                "d  dir0",
                                                                "d  dir1",
                                                                "d  dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt", "file0"),
                                                 new AddFileCommand(0, "file1.txt", "file1"),
                                                 new AddFileCommand(0, "file2.txt", "file2"),
                                                 new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(0, "dir1"),
                                                 new AddDirectoryCommand(0, "dir2"))));
    }

    [Test]
    public void TestDirectoryTreeMarkupGivesLayout() {
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString("d  dir0",
                                                                "d    dir1",
                                                                "d      dir2"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddDirectoryCommand(0, "dir0"),
                                                 new AddDirectoryCommand(1, @"dir0\dir1"),
                                                 new AddDirectoryCommand(2, @"dir0\dir1\dir2"))));
    }

    [Test]
    public void TestDirectoryTreeWithFilesMarkupGivesLayout() {
      var layout = _Parser.Parse(StringUtils.ToNewLineSepString("f  file0.txt []",
                                                                "d  dir0",
                                                                "f    file1.txt [1]",
                                                                "d    dir1",
                                                                "f      file2.txt []",
                                                                "d      dir2",
                                                                "f        file3.txt [3]"));
      Assert.That(layout, Are.EqualTo(new Layout(new AddFileCommand(0, "file0.txt", ""),
                                                 new AddDirectoryCommand(0, "dir0"),
                                                 new AddFileCommand(1, @"dir0\file1.txt", "1"),
                                                 new AddDirectoryCommand(1, @"dir0\dir1"),
                                                 new AddFileCommand(2, @"dir0\dir1\file2.txt", ""),
                                                 new AddDirectoryCommand(2, @"dir0\dir1\dir2"),
                                                 new AddFileCommand(3, @"dir0\dir1\dir2\file3.txt", "3"))));
    }

    [TestCase("z  file1.txt []", "Cannot Parse Markup: 'z  file1.txt []'")]
    [TestCase("f file1.txt []", "Cannot Parse Markup: 'f file1.txt []'")]
    [TestCase("f   file1.txt []", "Cannot Parse Markup: 'f   file1.txt []'")]
    [TestCase("f", "Cannot Parse Markup: 'f'")]
    [TestCase("F  file1.txt []", "Cannot Parse Markup: 'F  file1.txt []'")]
    [TestCase("f  file1.txt", "Cannot Parse Markup: 'f  file1.txt'")]
    public void TestInvalidMarkupThrows(string line, string expectedMessage) {
      var ex = Assert.Throws<MaddirException>(() => _Parser.Parse(line));
      Assert.That(ex.Message, Is.EqualTo(expectedMessage));
    }

    private static readonly Parser _Parser = new Parser(new Settings());
  }
}