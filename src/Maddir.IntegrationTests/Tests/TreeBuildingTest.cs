﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections;
using System.IO;
using Maddir.Core;
using Maddir.Core.Model;
using Maddir.Core.Utility;
using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class TreeBuildingTest : BaseTestCase {
    public sealed class Validation {
      public Validation(string testName, Settings settings, params string[] expected) {
        TestName = testName;
        Settings = settings;
        Markup = StringUtils
          .ToNewLineSepString(expected);
      }

      public string TestName{get;private set;}
      public string Markup{get;private set;}
      public Settings Settings{get;set;}

      public override string ToString() {
        return TestName;
      }
    }

    [TestCaseSource("GetUsageTests")]
    public void TestBasicUsages(Validation validation) {
      Maddirs.ApplyMarkup(validation.Settings, Helper.PathInfo.TestDataDir, validation.Markup);
      Assert.That(Maddirs.BuildMarkup(validation.Settings, Helper.PathInfo.TestDataDir), Is.EqualTo(validation.Markup));
    }

    [Test]
    public void TestCallbacks() {
      var settings = new Settings();
      settings.OnFileCreated += (sender, args) => {
                                  using (var strm = new StreamWriter(args.Info.OpenWrite()))
                                    strm.Write(args.Info.FullName);
                                };
      settings.OnDirectoryCreated += (sender, args) => args.Info.CreateSubdirectory("a");
      Maddirs.ApplyMarkup(settings, Helper.PathInfo.TestDataDir, StringUtils.ToNewLineSepString("f  file1.txt []",
                                                                                                "d  dir1"));

      var filePath = Path.Combine(Helper.PathInfo.TestDataDir, "file1.txt");
      Assert.That(File.ReadAllText(filePath), Is.EqualTo(filePath));
      Assert.That(Directory.Exists(Path.Combine(Helper.PathInfo.TestDataDir, @"dir1\a")), Is.True);
    }

    public IEnumerable GetUsageTests() {
      yield return new Validation("TestEmptyMarkupGivesNoDirectoriesOrFiles",
                                  new Settings());
      yield return new Validation("TestSingleDirectoryMarkup",
                                  new Settings(),
                                  "d  dir1");
      yield return new Validation("TestMultipleDirectoryMarkup",
                                  new Settings(),
                                  "d  dir1",
                                  "d  dir2",
                                  "d  dir3");
      yield return new Validation("TestSingleFileMarkup",
                                  new Settings(),
                                  "f  file1.txt [file1]");
      yield return new Validation("TestSingleFileMarkupWithSingleCharCustomContentDelimiters",
                                  new Settings {
                                                 ContentStartDelimiter = '<',
                                                 ContentEndDelimiter = '>'
                                               },
                                  "f  file1.txt <file1>");
      yield return new Validation("TestMultipleFileMarkup",
                                  new Settings(),
                                  "f  file1.txt [file1]",
                                  "f  file2.txt []",
                                  "f  file3.txt [file3]");
      yield return new Validation("TestDirectoryTreeMarkup",
                                  new Settings(),
                                  "d  dir1",
                                  "d    dir2",
                                  "d      dir3");
      yield return new Validation("TestDirectoryTreeMarkupWithFiles",
                                  new Settings(),
                                  "f  file1.txt []",
                                  "d  dir1",
                                  "f    file2.txt []",
                                  "d    dir2",
                                  "f      file3.txt []",
                                  "d      dir3",
                                  "f        file4.txt [file4]");
      yield return new Validation("TestDirectoryTreeMarkupWithMultipleFiles",
                                  new Settings(),
                                  "f  file1.txt []",
                                  "f  file2.txt []",
                                  "f  file3.txt []",
                                  "d  dir1",
                                  "f    file4.txt []",
                                  "f    file5.txt []",
                                  "d    dir2",
                                  "f      file6.txt [file6]",
                                  "f      file7.txt [file7]",
                                  "f      file8.txt [file8]",
                                  "d      dir9",
                                  "f        file9.txt []");
      yield return new Validation("TestDirectoryTreeWithMultipleLevelsMarkupWithMultipleFiles",
                                  new Settings(),
                                  "f  filea.txt []",
                                  "d  dira",
                                  "d  dirb",
                                  "f    fileb.txt [fileb]",
                                  "d    dirc",
                                  "d    dird",
                                  "f      filec.txt []",
                                  "f      filed.txt []",
                                  "d      dire",
                                  "d        dirf",
                                  "d          dirg",
                                  "f            filee.txt []",
                                  "d          dirh",
                                  "f            filef.txt [filef]",
                                  "f            fileg.txt [fileg]",
                                  "d          diri",
                                  "d        dirj",
                                  "f          fileh.txt []",
                                  "d  dirk",
                                  "f    filei.txt [filei]");
      yield return new Validation("TestSingleFileWithMultipleLineContentsMarkup",
                                  new Settings(),
                                  "f  file1.txt [file1",
                                  "contents]");
      yield return new Validation("TestSingleFileWithJsonContentsMarkup",
                                  new Settings {
                                                 ContentStartDelimiter = '|',
                                                 ContentEndDelimiter = '|'
                                               },
                                  "f  file1.txt |",
                                  "{",
                                  "  \"name\": \"bob\"", 
                                  "  \"age\": 15",
                                  "  \"values\": [",
                                  "    1",
                                  "    2",
                                  "    3",
                                  "  ]",
                                  "}",
                                  "|");
    }
  }
}