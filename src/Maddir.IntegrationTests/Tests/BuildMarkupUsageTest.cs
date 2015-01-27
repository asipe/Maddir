// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections;
using System.IO;
using Maddir.Core;
using Maddir.Core.Generation;
using NUnit.Framework;
using SupaCharge.Core.IOAbstractions;

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
    }

    [TestCaseSource("GetUsageTests")]
    public void TestUsages(Validation validation) {
      validation.Setup.Invoke(Helper.PathInfo.TestDataDir);
      var layout = new DirectoryBrowser(new DotNetDirectory()).Browse(Helper.PathInfo.TestDataDir);
      Assert.That(new MarkupGenerationEngine(new MarkupBuilder()).Apply(layout), Is.EqualTo(validation.Expected));
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
    }
  }
}