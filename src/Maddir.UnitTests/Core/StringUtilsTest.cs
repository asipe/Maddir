// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core;
using NUnit.Framework;

namespace Maddir.UnitTests.Core {
  [TestFixture]
  public class StringUtilsTest : BaseTestCase {
    [TestCase(new string[0], "")]
    [TestCase(new[] {"a"}, "a")]
    [TestCase(new[] {"a", "b", "c"}, "a\r\nb\r\nc")]
    public void TestToNewLineSepString(string[] values, string expected) {
      Assert.That(StringUtils.ToNewLineSepString(values), Is.EqualTo(expected));
    }
  }
}