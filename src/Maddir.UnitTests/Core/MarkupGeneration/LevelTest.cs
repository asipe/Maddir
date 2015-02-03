// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.MarkupGeneration;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.MarkupGeneration {
  [TestFixture]
  public class LevelTest : BaseTestCase {
    [TestCase(@"c:\app1\data", @"c:\app1\data\abc.txt", 0)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\dir1", 0)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\abc.txt", 1)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\dir2", 1)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\subdir\abc.txt", 2)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\subdir\dir3", 2)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\subdir\subdir\abc.txt", 3)]
    [TestCase(@"c:\app1\data", @"c:\app1\data\subdir\subdir\subdir\dir4", 3)]
    public void TestGetForPathUsage(string root, string location, int expected) {
      var level = new Level(root);
      Assert.That(level.GetForPath(location), Is.EqualTo(expected));
    }
  }
}