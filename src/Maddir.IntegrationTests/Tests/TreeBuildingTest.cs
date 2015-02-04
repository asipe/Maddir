// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections;
using Maddir.Core.MarkupGeneration;
using Maddir.Core.TreeGeneration;
using Maddir.Core.Utility;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class TreeBuildingTest : BaseTestCase {
    public sealed class Validation {
      public Validation(string testName, params string[] expected) {
        TestName = testName;
        Markup = StringUtils
          .ToNewLineSepString(expected);
      }

      public string TestName{get;private set;}
      public string Markup{get;private set;}

      public override string ToString() {
        return TestName;
      }
    }

    [TestCaseSource("GetUsageTests")]
    public void TestUsages(Validation validation) {
      new Engine(new DotNetDirectory(), new DotNetFile()).Apply(Helper.PathInfo.TestDataDir, new Parser().Parse(validation.Markup));
      Validate(Helper.PathInfo.TestDataDir, validation.Markup);
    }

    public IEnumerable GetUsageTests() {
      yield return new Validation("TestEmptyMarkupGivesNoDirectoriesOrFiles");
      yield return new Validation("TestSingleDirectoryMarkup",
                                  "d  dir1");
      yield return new Validation("TestMultipleDirectoryMarkup",
                                  "d  dir1",
                                  "d  dir2",
                                  "d  dir3");
      yield return new Validation("TestSingleFileMarkup",
                                  "f  file1.txt");
      yield return new Validation("TestMultipleFileMarkup",
                                  "f  file1.txt",
                                  "f  file2.txt",
                                  "f  file3.txt");
      yield return new Validation("TestDirectoryTreeMarkup",
                                  "d  dir1",
                                  "d    dir2",
                                  "d      dir3");
      yield return new Validation("TestDirectoryTreeMarkupWithFiles",
                                  "f  file1.txt",
                                  "d  dir1",
                                  "f    file2.txt",
                                  "d    dir2",
                                  "f      file3.txt",
                                  "d      dir3",
                                  "f        file4.txt");
      yield return new Validation("TestDirectoryTreeMarkupWithMultipleFiles",
                                  "f  file1.txt",
                                  "f  file2.txt",
                                  "f  file3.txt",
                                  "d  dir1",
                                  "f    file4.txt",
                                  "f    file5.txt",
                                  "d    dir2",
                                  "f      file6.txt",
                                  "f      file7.txt",
                                  "f      file8.txt",
                                  "d      dir9",
                                  "f        file9.txt");
      yield return new Validation("TestDirectoryTreeWithMultipleLevelsMarkupWithMultipleFiles",
                                  "f  filea.txt",
                                  "d  dira",
                                  "d  dirb",
                                  "f    fileb.txt",
                                  "d    dirc",
                                  "d    dird",
                                  "f      filec.txt",
                                  "f      filed.txt",
                                  "d      dire",
                                  "d        dirf",
                                  "d          dirg",
                                  "f            filee.txt",
                                  "d          dirh",
                                  "f            filef.txt",
                                  "f            fileg.txt",
                                  "d          diri",
                                  "d        dirj",
                                  "f          fileh.txt",
                                  "d  dirk",
                                  "f    filei.txt");
    }

    private static void Validate(string root, string expected) {
      var layout = new DirectoryBrowser(Snarfzer.NewScanner()).Browse(root);
      Assert.That(new MarkupGenerationEngine(new MarkupBuilder()).Apply(layout), Is.EqualTo(expected));
    }
  }
}