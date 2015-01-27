// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class DirectoryEntryTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var entry = new DirectoryEntry(0, "aname");
      Assert.That(entry.Type, Is.EqualTo(EntryType.Directory));
      Assert.That(entry.Level, Is.EqualTo(0));
      Assert.That(entry.Name, Is.EqualTo("aname"));
    }
  }
}