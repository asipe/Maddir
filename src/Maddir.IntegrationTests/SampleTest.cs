using NUnit.Framework;

namespace Maddir.IntegrationTests {
  [TestFixture]
  public class SampleTest {
    [Test]
    public void TestNothing() {
      Assert.That(true, Is.True);
    }
  }
}