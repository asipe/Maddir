// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core;
using Maddir.Core.Generation;
using Maddir.Core.Model;
using Maddir.Core.Utility;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Generation {
  [TestFixture]
  public class MarkupBuilderTest : BaseTestCase {
    [Test]
    public void TestEmptyBuildGivesEmptyResult() {
      Assert.That(mBuilder.Build(), Is.Empty);
    }

    [Test]
    public void TestAddSingleDirectory() {
      mBuilder.Add(new DirectoryEntry(0, "root"));
      Assert.That(mBuilder.Build(), Is.EqualTo("d  root"));
    }

    [Test]
    public void TestAddSingleFile() {
      mBuilder.Add(new FileEntry(0, "file1.txt"));
      Assert.That(mBuilder.Build(), Is.EqualTo("f  file1.txt"));
    }

    [Test]
    public void TestAddSingleDirectoryAndSingleFile() {
      mBuilder.Add(new DirectoryEntry(0, "root"));
      mBuilder.Add(new FileEntry(0, "file1.txt"));
      Assert.That(mBuilder.Build(),
                  Is.EqualTo(StringUtils.ToNewLineSepString("d  root",
                                                            "f  file1.txt")));
    }

    [Test]
    public void TestAddMultipleEntriesAtMultipleLevels() {
      mBuilder.Add(new FileEntry(0, "rootfile1.txt"));
      mBuilder.Add(new DirectoryEntry(0, "subdira0"));
      mBuilder.Add(new DirectoryEntry(1, "subdira1"));
      mBuilder.Add(new DirectoryEntry(2, "subdiraa1"));
      mBuilder.Add(new DirectoryEntry(3, "subdiraaa1"));
      mBuilder.Add(new DirectoryEntry(3, "subdiraaa2"));
      mBuilder.Add(new FileEntry(4, "subdiraaa2file1.txt"));
      mBuilder.Add(new DirectoryEntry(4, "subdiraaaa3"));
      mBuilder.Add(new FileEntry(5, "subdiraaaa3file1.txt"));
      mBuilder.Add(new DirectoryEntry(1, "subdira2"));
      mBuilder.Add(new FileEntry(2, "subdira2file1.txt"));
      mBuilder.Add(new FileEntry(2, "subdira2file2.txt"));
      Assert.That(mBuilder.Build(),
                  Is.EqualTo(StringUtils.ToNewLineSepString("f  rootfile1.txt",
                                                            "d  subdira0",
                                                            "d    subdira1",
                                                            "d      subdiraa1",
                                                            "d        subdiraaa1",
                                                            "d        subdiraaa2",
                                                            "f          subdiraaa2file1.txt",
                                                            "d          subdiraaaa3",
                                                            "f            subdiraaaa3file1.txt",
                                                            "d    subdira2",
                                                            "f      subdira2file1.txt",
                                                            "f      subdira2file2.txt"
                               )));
    }

    [SetUp]
    public void DoSetup() {
      mBuilder = new MarkupBuilder();
    }

    private MarkupBuilder mBuilder;
  }
}