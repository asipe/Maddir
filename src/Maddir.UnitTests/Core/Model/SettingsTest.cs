// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Maddir.Core.Events;
using Maddir.Core.Model;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Model {
  [TestFixture]
  public class SettingsTest : BaseTestCase {
    [Test]
    public void TestDefaultHasNoDirectoryHandlersRegistered() {
      mSettings
        .Handlers
        .HandleDirectoryCreated(new DirectoryCreatedEventArgs(null));
    }

    [Test]
    public void TestHandleDirectoryCreatedWithSingleHandleRegistered() {
      var count = 0;
      var original = new DirectoryCreatedEventArgs(null);
      mSettings
        .OnDirectoryCreated += (sender, args) => {
                                 Assert.That(args, Are.EqualTo(original));
                                 Assert.That(sender, Is.Null);
                                 ++count;
                               };
      mSettings
        .Handlers
        .HandleDirectoryCreated(original);
      Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void TestHandleDirectoryCreatedWithMultipleHandleRegistered() {
      var count = 0;
      var original = new DirectoryCreatedEventArgs(null);
      EventHandler<DirectoryCreatedEventArgs> evt = (sender, args) => {
                                                      Assert.That(args, Are.EqualTo(original));
                                                      Assert.That(sender, Is.Null);
                                                      ++count;
                                                    };
      mSettings
        .OnDirectoryCreated += evt;
      mSettings
        .OnDirectoryCreated += evt;
      mSettings
        .OnDirectoryCreated += evt;
      mSettings
        .Handlers
        .HandleDirectoryCreated(original);
      Assert.That(count, Is.EqualTo(3));
    }

    [Test]
    public void TestDefaultHasNoFileCreatedHandlers() {
      mSettings
        .Handlers
        .HandleFileCreated(new FileCreatedEventArgs(null));
    }

    [Test]
    public void TestHandleFileCreatedWithSingleHandleRegistered() {
      var count = 0;
      var original = new FileCreatedEventArgs(null);
      mSettings
        .OnFileCreated += (sender, args) => {
                            Assert.That(args, Are.EqualTo(original));
                            Assert.That(sender, Is.Null);
                            ++count;
                          };
      mSettings
        .Handlers
        .HandleFileCreated(original);
      Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void TestHandleFileCreatedWithMultipleHandleRegistered() {
      var count = 0;
      var original = new FileCreatedEventArgs(null);
      EventHandler<FileCreatedEventArgs> evt = (sender, args) => {
                                                 Assert.That(args, Are.EqualTo(original));
                                                 Assert.That(sender, Is.Null);
                                                 ++count;
                                               };
      mSettings
        .OnFileCreated += evt;
      mSettings
        .OnFileCreated += evt;
      mSettings
        .OnFileCreated += evt;
      mSettings
        .Handlers
        .HandleFileCreated(original);
      Assert.That(count, Is.EqualTo(3));
    }

    [SetUp]
    public void DoSetup() {
      mSettings = new Settings();
    }

    private Settings mSettings;
  }
}