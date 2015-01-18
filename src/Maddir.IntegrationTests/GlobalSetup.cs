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