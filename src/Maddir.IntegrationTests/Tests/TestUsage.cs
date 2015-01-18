using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class TestUsage {
    [Test]
    public void TestNothing() {
      Assert.That(true, Is.True);
    }
  }
}