using System.IO;
using Maddir.Core;
using NUnit.Framework;

namespace Maddir.IntegrationTests.Tests {
  [TestFixture]
  public class TestUsage : BaseTestCase {
    [Test]
    public void TestEmptyMarkupGivesNoDirectories() {
      var layout = new Parser().Parse("");
      new Engine().Apply(Helper.PathInfo.TestDataDir, layout);

      Assert.That(Directory.GetDirectories(Helper.PathInfo.TestDataDir), Is.Empty);
      Assert.That(Directory.GetFiles(Helper.PathInfo.TestDataDir), Is.Empty);
    }
  }
}