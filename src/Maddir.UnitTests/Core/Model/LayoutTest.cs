﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class LayoutTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var commands = BA<ICommand>();
      var layout = new Layout(commands);
      Assert.That(layout.Commands, Is.EqualTo(commands));
    }
  }
}