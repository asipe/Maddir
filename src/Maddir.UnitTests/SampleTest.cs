using NUnit.Framework;

namespace Maddir.UnitTests {
  [TestFixture]
  public class SampleTest {
    [Test]
    public void TestNothing() {
      Assert.That(true, Is.True);
    }
  }
}