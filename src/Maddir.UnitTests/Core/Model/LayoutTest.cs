using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class LayoutTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var commands = BA<ICommand>();
      var layout = new Layout(commands);
      Assert.That(layout.Commands, Is.EqualTo(commands));
    }
  }
}