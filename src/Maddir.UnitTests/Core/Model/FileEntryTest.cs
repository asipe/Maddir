// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class FileEntryTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var entry = new FileEntry(0, "aname", "contents");
      Assert.That(entry.Type, Is.EqualTo(EntryType.File));
      Assert.That(entry.Level, Is.EqualTo(0));
      Assert.That(entry.Name, Is.EqualTo("aname"));
      Assert.That(entry.Contents, Is.EqualTo("contents"));
    }
  }
}