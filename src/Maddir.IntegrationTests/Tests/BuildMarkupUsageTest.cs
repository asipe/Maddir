// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections;
using System.IO;
using Maddir.Core.MarkupGeneration;
using Maddir.Core.Utility;
using NUnit.Framework;
using Snarfz.Core;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class BuildMarkupUsageTest : BaseTestCase {
    public sealed class Validation {
      public Validation(string testName, Action<string> setup, params string[] expected) {
        TestName = testName;
        Setup = setup;
        Expected = StringUtils
          .ToNewLineSepString(expected);
      }

      public string TestName{get;private set;}
      public Action<string> Setup{get;private set;}
      public string Expected{get;private set;}

      public override string ToString() {
        return TestName;
      }
    }

    [TestCaseSource("GetUsageTests")]
    public void TestUsages(Validation validation) {
      validation.Setup.Invoke(Helper.PathInfo.TestDataDir);
      var layout = new DirectoryBrowser(Snarfzer.NewScanner()).Browse(Helper.PathInfo.TestDataDir);
      var actual = new MarkupGenerationEngine(new MarkupBuilder()).Apply(layout);
      Assert.That(actual, Is.EqualTo(validation.Expected), actual);
    }

    public IEnumerable GetUsageTests() {
      yield return new Validation("TestBuildWithNoDirectoriesGivesEmptyMarkup",
                                  root => {},
                                  "");
      yield return new Validation("TestBuildWithSingleDirectoryGivesCorrectMarkup",
                                  root => Directory.CreateDirectory(Path.Combine(root, "abc")),
                                  "d  abc");
      yield return new Validation("TestBuildWithMutlipleDirectoriesGivesCorrectMarkup",
                                  root => {
                                    Directory.CreateDirectory(Path.Combine(root, "abc"));
                                    Directory.CreateDirectory(Path.Combine(root, "def"));
                                    Directory.CreateDirectory(Path.Combine(root, "xyz"));
                                  },
                                  "d  abc",
                                  "d  def",
                                  "d  xyz");
      yield return new Validation("TestBuildWithSingleFile",
                                  root => File.WriteAllText(Path.Combine(root, "abc.txt"), "abc"),
                                  "f  abc.txt");
      yield return new Validation("TestBuildWithMutlipleFiles",
                                  root => {
                                    File.WriteAllText(Path.Combine(root, "abc.txt"), "abc");
                                    File.WriteAllText(Path.Combine(root, "def.txt"), "def");
                                    File.WriteAllText(Path.Combine(root, "xyz.txt"), "xyz");
                                  },
                                  "f  abc.txt",
                                  "f  def.txt",
                                  "f  xyz.txt");
      yield return new Validation("TestBuildWithMutlipleMixedFilesAndDirectories",
                                  root => {
                                    Directory.CreateDirectory(Path.Combine(root, "abc"));
                                    Directory.CreateDirectory(Path.Combine(root, "def"));
                                    Directory.CreateDirectory(Path.Combine(root, "xyz"));
                                    File.WriteAllText(Path.Combine(root, "abc.txt"), "abc");
                                    File.WriteAllText(Path.Combine(root, "def.txt"), "def");
                                    File.WriteAllText(Path.Combine(root, "xyz.txt"), "xyz");
                                  },
                                  "f  abc.txt",
                                  "f  def.txt",
                                  "f  xyz.txt",
                                  "d  abc",
                                  "d  def",
                                  "d  xyz");
      yield return new Validation("TestBuildWithDirectoryTree",
                                  root => {
                                    Directory.CreateDirectory(Path.Combine(root, "subdira"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0\subdirab-0"));
                                  },
                                  "d  subdira",
                                  "d    subdira-0",
                                  "d      subdirab-0");
      yield return new Validation("TestBuildWithDirectoryTree",
                                  root => {
                                    Directory.CreateDirectory(Path.Combine(root, "subdira"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0\subdirab-0"));
                                    Directory.CreateDirectory(Path.Combine(root, "subdirb"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-0"));
                                  },
                                  "d  subdira",
                                  "d    subdira-0",
                                  "d      subdirab-0",
                                  "d  subdirb",
                                  "d    subdirb-0",
                                  "d      subdirbb-0");
      yield return new Validation("TestBuildWithDirectoryTreeWithFiles",
                                  root => {
                                    File.WriteAllText(Path.Combine(root, "file1.txt"), "file1");
                                    Directory.CreateDirectory(Path.Combine(root, "subdira"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2.txt"), "file2");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3.txt"), "file3");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0\subdirab-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4.txt"), "file4");
                                  },
                                  "f  file1.txt",
                                  "d  subdira",
                                  "f    file2.txt",
                                  "d    subdira-0",
                                  "f      file3.txt",
                                  "d      subdirab-0",
                                  "f        file4.txt");
      yield return new Validation("TestBuildWithDirectoryTreeWithMultipleFiles",
                                  root => {
                                    File.WriteAllText(Path.Combine(root, "file1a.txt"), "file1a");
                                    File.WriteAllText(Path.Combine(root, "file1b.txt"), "file1b");
                                    File.WriteAllText(Path.Combine(root, "file1c.txt"), "file1c");
                                    Directory.CreateDirectory(Path.Combine(root, "subdira"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2a.txt"), "file2a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2b.txt"), "file2b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2c.txt"), "file2c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3a.txt"), "file3a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3b.txt"), "file3b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3c.txt"), "file3c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0\subdirab-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4a.txt"), "file4a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4b.txt"), "file4b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4c.txt"), "file4c");
                                  },
                                  "f  file1a.txt",
                                  "f  file1b.txt",
                                  "f  file1c.txt",
                                  "d  subdira",
                                  "f    file2a.txt",
                                  "f    file2b.txt",
                                  "f    file2c.txt",
                                  "d    subdira-0",
                                  "f      file3a.txt",
                                  "f      file3b.txt",
                                  "f      file3c.txt",
                                  "d      subdirab-0",
                                  "f        file4a.txt",
                                  "f        file4b.txt",
                                  "f        file4c.txt");
      yield return new Validation("TestBuildWithDirectoryTreeWithMultipleLevelsWithMultipleFiles",
                                  root => {
                                    File.WriteAllText(Path.Combine(root, "file1a.txt"), "file1a");
                                    File.WriteAllText(Path.Combine(root, "file1b.txt"), "file1b");
                                    File.WriteAllText(Path.Combine(root, "file1c.txt"), "file1c");
                                    Directory.CreateDirectory(Path.Combine(root, "subdira"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2a.txt"), "file2a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2b.txt"), "file2b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\file2c.txt"), "file2c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3a.txt"), "file3a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3b.txt"), "file3b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\file3c.txt"), "file3c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdira\subdira-0\subdirab-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4a.txt"), "file4a");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4b.txt"), "file4b");
                                    File.WriteAllText(Path.Combine(root, @"subdira\subdira-0\subdirab-0\file4c.txt"), "file4c");
                                    Directory.CreateDirectory(Path.Combine(root, "subdirb"));
                                    File.WriteAllText(Path.Combine(root, @"subdirb\file5a.txt"), "file5a");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\file5b.txt"), "file5b");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\file5c.txt"), "file5c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\file6a.txt"), "file6a");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\file6b.txt"), "file6b");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\file6c.txt"), "file6c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-0"));
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-0\file7a.txt"), "file7a");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-0\file7b.txt"), "file7b");
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-0\file7c.txt"), "file7c");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-1"));
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-2"));
                                    File.WriteAllText(Path.Combine(root, @"subdirb\subdirb-0\subdirbb-2\file8a.txt"), "file8a");
                                    Directory.CreateDirectory(Path.Combine(root, @"subdirb\subdirb-1"));
                                    Directory.CreateDirectory(Path.Combine(root, "subdirc"));
                                    Directory.CreateDirectory(Path.Combine(root, "subdird"));
                                  },
                                  "f  file1a.txt",
                                  "f  file1b.txt",
                                  "f  file1c.txt",
                                  "d  subdira",
                                  "f    file2a.txt",
                                  "f    file2b.txt",
                                  "f    file2c.txt",
                                  "d    subdira-0",
                                  "f      file3a.txt",
                                  "f      file3b.txt",
                                  "f      file3c.txt",
                                  "d      subdirab-0",
                                  "f        file4a.txt",
                                  "f        file4b.txt",
                                  "f        file4c.txt",
                                  "d  subdirb",
                                  "f    file5a.txt",
                                  "f    file5b.txt",
                                  "f    file5c.txt",
                                  "d    subdirb-0",
                                  "f      file6a.txt",
                                  "f      file6b.txt",
                                  "f      file6c.txt",
                                  "d      subdirbb-0",
                                  "f        file7a.txt",
                                  "f        file7b.txt",
                                  "f        file7c.txt",
                                  "d      subdirbb-1",
                                  "d      subdirbb-2",
                                  "f        file8a.txt",
                                  "d    subdirb-1",
                                  "d  subdirc",
                                  "d  subdird");
    }
  }
}