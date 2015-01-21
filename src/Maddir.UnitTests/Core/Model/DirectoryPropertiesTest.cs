using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class DirectoryPropertiesTest : BaseTestCase {
    [Test]
    public void TestDefault() {
      var props = new DirectoryProperties("apath");
      Assert.That(props.Path, Is.EqualTo("apath"));
    }
  }
}