// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Commands {
  [TestFixture]
  public class AddFileCommandTest : BaseTestCase {
    [Test]
    public void TestDefault() {
      Assert.That(new AddFileCommand(0, "apath", "contents").Entry, Are.EqualTo(new FileEntry(0, "apath", "contents")));
    }
  }
}