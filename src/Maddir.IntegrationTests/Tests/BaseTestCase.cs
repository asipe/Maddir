// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.IntegrationTests.Infrastructure;
using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public abstract class BaseTestCase {
    [SetUp]
    public void DoSetup() {
      Helper.Setup();
    }

    [TearDown]
    public void DoTearDown() {
      Helper.TearDown();
    }

    [TestFixtureSetUp]
    public void DoFixtureSetup() {
      Helper = new MaddirHelper();
    }

    protected MaddirHelper Helper{get;private set;}
  }
}