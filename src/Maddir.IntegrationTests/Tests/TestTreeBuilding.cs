// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.IO;
using Maddir.Core;
using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class TestTreeBuilding : BaseTestCase {
    [Test]
    public void TestEmptyMarkupGivesNoDirectoriesOrFiles() {
      var layout = new Parser().Parse("");
      new Engine().Apply(Helper.PathInfo.TestDataDir, layout);
      Assert.That(Directory.GetDirectories(Helper.PathInfo.TestDataDir), Is.Empty);
      Assert.That(Directory.GetFiles(Helper.PathInfo.TestDataDir), Is.Empty);
    }
  }
}