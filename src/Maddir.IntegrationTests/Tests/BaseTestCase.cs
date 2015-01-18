using Maddir.IntegrationTests.Infrastructure;
using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public abstract class BaseTestCase {
    [SetUp]
    public void DoSetup() {
      Helper.Setup();
    }

    [TestFixtureSetUp]
    public void DoFixtureSetup() {
      Helper = new MaddirHelper();
    }

    protected MaddirHelper Helper{get;private set;}
  }
}