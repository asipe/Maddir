// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.IO;
using Maddir.Core.Events;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Events {
  [TestFixture]
  public class DirectoryCreatedEventArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var info = new DirectoryInfo("abc");
      Assert.That(new DirectoryCreatedEventArgs(info).Info, Is.SameAs(info));
    }
  }
}