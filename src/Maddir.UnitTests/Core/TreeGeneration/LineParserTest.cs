﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections;
using Maddir.Core;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.TreeGeneration {
  [TestFixture]
  public class LineParserTest : BaseTestCase {
    [TestCaseSource("GetUsageTests")]
    public void TestUsage(char contentStartDelimiter, char contentEndDelimiter, string line, IEntry expected) {
      var parser = new LineParser(new Settings {
                                                 ContentStartDelimiter = contentStartDelimiter,
                                                 ContentEndDelimiter = contentEndDelimiter
                                               });
      Assert.That(parser.Parse(line), Are.EqualTo(expected));
    }

    [TestCase("", "Cannot Parse Markup: ''")]
    [TestCase("z  file1.txt []", "Cannot Parse Markup: 'z  file1.txt []'")]
    [TestCase("f file1.txt []", "Cannot Parse Markup: 'f file1.txt []'")]
    [TestCase("f   file1.txt []", "Cannot Parse Markup: 'f   file1.txt []'")]
    [TestCase("f", "Cannot Parse Markup: 'f'")]
    [TestCase("F  file1.txt []", "Cannot Parse Markup: 'F  file1.txt []'")]
    [TestCase("f  file1.txt", "Cannot Parse Markup: 'f  file1.txt'")]
    public void TestUnmatchedRegexThrowsMaddirException(string line, string expectedMessage) {
      var parser = new LineParser(new Settings());
      var ex = Assert.Throws<MaddirException>(() => parser.Parse(line));
      Assert.That(ex.Message, Is.EqualTo(expectedMessage));
    }

    public IEnumerable GetUsageTests() {
      yield return new TestCaseData('[', ']', "f  file0.txt []", new FileEntry(0, "file0.txt", ""));
      yield return new TestCaseData('[', ']', "f  file0.txt [0]", new FileEntry(0, "file0.txt", "0"));
      yield return new TestCaseData('[', ']', "f    file0.txt  [a]", new FileEntry(1, "file0.txt", "a"));
      yield return new TestCaseData('[', ']', "f      file0.txt    [abc]", new FileEntry(2, "file0.txt", "abc"));
      yield return new TestCaseData('[', ']', "d  dir0", new DirectoryEntry(0, "dir0"));
      yield return new TestCaseData('[', ']', "d    dir0", new DirectoryEntry(1, "dir0"));
      yield return new TestCaseData('[', ']', "d      dir0", new DirectoryEntry(2, "dir0"));
      yield return new TestCaseData('<', '>', "f  file0.txt <>", new FileEntry(0, "file0.txt", ""));
      yield return new TestCaseData('<', '>', "f  file0.txt <0>", new FileEntry(0, "file0.txt", "0"));
      yield return new TestCaseData('<', '>', "f    file0.txt  <a>", new FileEntry(1, "file0.txt", "a"));
      yield return new TestCaseData('<', '>', "f      file0.txt    <abc>", new FileEntry(2, "file0.txt", "abc"));
      yield return new TestCaseData('<', '>', "d  dir0", new DirectoryEntry(0, "dir0"));
      yield return new TestCaseData('<', '>', "d    dir0", new DirectoryEntry(1, "dir0"));
      yield return new TestCaseData('<', '>', "d      dir0", new DirectoryEntry(2, "dir0"));
      yield return new TestCaseData('|', '|', "f  file0.txt ||", new FileEntry(0, "file0.txt", ""));
      yield return new TestCaseData('|', '|', "f  file0.txt |0|", new FileEntry(0, "file0.txt", "0"));
      yield return new TestCaseData('|', '|', "f    file0.txt  |a|", new FileEntry(1, "file0.txt", "a"));
      yield return new TestCaseData('|', '|', "f      file0.txt    |abc|", new FileEntry(2, "file0.txt", "abc"));
      yield return new TestCaseData('|', '|', "d  dir0", new DirectoryEntry(0, "dir0"));
      yield return new TestCaseData('|', '|', "d    dir0", new DirectoryEntry(1, "dir0"));
      yield return new TestCaseData('|', '|', "d      dir0", new DirectoryEntry(2, "dir0"));
    }
  }
}