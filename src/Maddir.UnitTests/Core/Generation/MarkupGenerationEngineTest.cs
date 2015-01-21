// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Commands;
using Maddir.Core.Generation;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Generation {
  [TestFixture]
  public class MarkupGenerationEngineTest : BaseTestCase {
    [Test]
    public void TestApplyWithEmptyLayoutGivesEmptyMarkup() {
      Assert.That(mEngine.Apply(new Layout()), Is.Empty);
    }

    [Test]
    public void TestApplyWithLayoutWithSingleDirectoryGivesCorrectMarkup() {
      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand("abc"))), Is.EqualTo("abc"));
    }

    [Test]
    public void TestApplyWithLayoutWithMultipleDirectoriesGivesCorrectMarkup() {
      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand("abc"),
                                           new AddDirectoryCommand("def"),
                                           new AddDirectoryCommand("xyz"))), Is.EqualTo("abc\r\ndef\r\nxyz"));
    }

    [Test]
    public void TestApply2LayoutsInSequenceGivesCorrectMarkup() {
      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand("abc"))), Is.EqualTo("abc"));
      Assert.That(mEngine.Apply(new Layout(new AddDirectoryCommand("xyz"))), Is.EqualTo("xyz"));
    }

    [SetUp]
    public void DoSetup() {
      mEngine = new MarkupGenerationEngine();
    }

    private MarkupGenerationEngine mEngine;
  }
}