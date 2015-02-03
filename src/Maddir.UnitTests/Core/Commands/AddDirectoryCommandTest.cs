// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Commands {
  [TestFixture]
  public class AddDirectoryCommandTest : BaseTestCase {
    [Test]
    public void TestDefaultLevelCtor() {
      Assert.That(new AddDirectoryCommand(0, "apath").Entry, Are.EqualTo(new DirectoryEntry(0, "apath")));
    }
  }
}