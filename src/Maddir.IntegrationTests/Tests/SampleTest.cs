using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class SampleTest {
    [Test]
    public void TestNothing() {
      Assert.That(true, Is.True);
    }
  }
}