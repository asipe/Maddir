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
      public Validation(string testName, Action<string> setup, string expected) {
        TestName = testName;
        Setup = setup;
        Expected = expected;
      }

      public string TestName{get;private set;}
      public Action<string> Setup{get;private set;}
      public string Expected{get;private set;}
    }

    [TestCaseSource("GetUsageTests")]
    public void TestUsages(Validation validation) {
      validation.Setup.Invoke(Helper.PathInfo.TestDataDir);
      var layout = new DirectoryBrowser(new DotNetDirectory()).Browse(Helper.PathInfo.TestDataDir);
      Assert.That(new MarkupGenerationEngine().Apply(layout), Is.EqualTo(validation.Expected));
    }

    public IEnumerable GetUsageTests() {
      yield return new Validation("TestBuildWithNoDirectoriesGivesEmptyMarkup",
                                  root => {},
                                  StringUtils.ToNewLineSepString(""));
      yield return new Validation("TestBuildWithSingleDirectoryGivesCorrectMarkup",
                                  root => Directory.CreateDirectory(Path.Combine(root, "abc")),
                                  StringUtils.ToNewLineSepString("abc"));
      yield return new Validation("TestBuildWithSingleDirectoryGivesCorrectMarkup",
                                  root => {
                                    Directory.CreateDirectory(Path.Combine(root, "abc"));
                                    Directory.CreateDirectory(Path.Combine(root, "def"));
                                  },
                                  StringUtils.ToNewLineSepString("abc", "def"));
    }
  }
}