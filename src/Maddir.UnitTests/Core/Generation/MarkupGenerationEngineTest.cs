using Maddir.Core.Generation;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Generation {
  [TestFixture]
  public class MarkupGenerationEngineTest : BaseTestCase {
    [Test]
    public void TestApplyWithEmptyLayoutGivesEmptyMarkup() {
      Assert.That(mEngine.Apply(BA<string>()), Is.Empty);
    }

    [Test]
    public void TestApplyWithLayoutWithSingleDirectoryGivesCorrectMarkup() {
      Assert.That(mEngine.Apply(BA("abc")), Is.EqualTo("abc"));
    }
    
    [Test]
    public void TestApplyWithLayoutWithMultipleDirectoriesGivesCorrectMarkup() {
      Assert.That(mEngine.Apply(BA("abc", "def", "xyz")), Is.EqualTo("abc\r\ndef\r\nxyz"));
    }


    [SetUp]
    public void DoSetup() {
      mEngine = new MarkupGenerationEngine();
    }

    private MarkupGenerationEngine mEngine;
  }
}