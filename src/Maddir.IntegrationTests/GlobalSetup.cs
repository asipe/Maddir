// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.
using Maddir.IntegrationTests.Infrastructure;
using NUnit.Framework;
using SupaCharge.Core.OID;

namespace Maddir.IntegrationTests {
  [SetUpFixture]
  public sealed class GlobalSetup {
    public static TestEnvironment TestEnvironment{get;private set;}
    public static PathInfo PathInfo{get;private set;}

    [SetUp]
    public void DoSetup() {
      PathInfo = new PathInfo(new DevelopmentRoot().Get(), new GuidOIDProvider().GetID());
      TestEnvironment = new TestEnvironment(PathInfo);
      TestEnvironment.Setup();
    }

    [TearDown]
    public void DoTearDown() {
      TestEnvironment.TearDown();
    }
  }
}