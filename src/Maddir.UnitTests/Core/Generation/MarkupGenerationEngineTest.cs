// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Generation;
using Maddir.Core.Model;
using Moq;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Generation {
  [TestFixture]
  public class MarkupGenerationEngineTest : BaseTestCase {
    [Test]
    public void TestApplyWithEmptyLayout() {
      mMarkupBuilder
        .Setup(b => b.Build())
        .Returns("markup");
      Assert.That(mEngine.Apply(new Layout()), Is.EqualTo("markup"));
    }

    [Test]
    public void TestApplyWithLayoutWithSingleCommand() {
      mMarkupBuilder
        .Setup(b => b.Add(IsEq(new DirectoryEntry(0, "abc"))));
      mMarkupBuilder
        .Setup(b => b.Build())
        .Returns("markup");
      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand(0, "abc"))), Is.EqualTo("markup"));
    }

    [Test]
    public void TestApplyWithLayoutWithMultipleCommands() {
      mMarkupBuilder
        .Setup(b => b.Add(IsEq(new DirectoryEntry(0, "abc"))));
      mMarkupBuilder
        .Setup(b => b.Add(IsEq(new DirectoryEntry(1, "def"))));
      mMarkupBuilder
        .Setup(b => b.Add(IsEq(new DirectoryEntry(2, "xyz"))));
      mMarkupBuilder
        .Setup(b => b.Build())
        .Returns("markup");

      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand(0, "abc"),
                                           new AddDirectoryCommand(1, "def"),
                                           new AddDirectoryCommand(2, "xyz"))), Is.EqualTo("markup"));
    }

    [SetUp]
    public void DoSetup() {
      mMarkupBuilder = Mok<IMarkupBuilder>();
      mEngine = new MarkupGenerationEngine(mMarkupBuilder.Object);
    }

    private MarkupGenerationEngine mEngine;
    private Mock<IMarkupBuilder> mMarkupBuilder;
  }
}