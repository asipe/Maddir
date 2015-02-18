// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Maddir.Core.Events;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.Events {
  [TestFixture]
  public class EventHandlersTest : BaseTestCase {
    [Test]
    public void TestHandleDirectoryCreatedWithNoHandlersDoesNothing() {
      mHandlers.HandleDirectoryCreated(new DirectoryCreatedEventArgs(null));
    }

    [Test]
    public void TestHandleDirectoryCreatedWithSingleHandleRegistered() {
      var count = 0;
      var original = new DirectoryCreatedEventArgs(null);
      mHandlers
        .OnDirectoryCreated += (sender, args) => {
                                 Assert.That(args, Are.EqualTo(original));
                                 Assert.That(sender, Is.SameAs(mHandlers));
                                 ++count;
                               };
      mHandlers.HandleDirectoryCreated(original);
      Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void TestHandleDirectoryCreatedWithMultipleHandleRegistered() {
      var count = 0;
      var original = new DirectoryCreatedEventArgs(null);
      EventHandler<DirectoryCreatedEventArgs> evt = (sender, args) => {
                                                      Assert.That(args, Are.EqualTo(original));
                                                      Assert.That(sender, Is.SameAs(mHandlers));
                                                      ++count;
                                                    };
      mHandlers
        .OnDirectoryCreated += evt;
      mHandlers
        .OnDirectoryCreated += evt;
      mHandlers
        .OnDirectoryCreated += evt;
      mHandlers.HandleDirectoryCreated(original);
      Assert.That(count, Is.EqualTo(3));
    }

    [Test]
    public void TestHandleFileCreatedWithNoHandlersDoesNothing() {
      mHandlers.HandleFileCreated(new FileCreatedEventArgs(null));
    }

    [Test]
    public void TestHandleFileCreatedWithSingleHandleRegistered() {
      var count = 0;
      var original = new FileCreatedEventArgs(null);
      mHandlers
        .OnFileCreated += (sender, args) => {
                            Assert.That(args, Are.EqualTo(original));
                            Assert.That(sender, Is.SameAs(mHandlers));
                            ++count;
                          };
      mHandlers.HandleFileCreated(original);
      Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void TestHandleFileCreatedWithMultipleHandleRegistered() {
      var count = 0;
      var original = new FileCreatedEventArgs(null);
      EventHandler<FileCreatedEventArgs> evt = (sender, args) => {
                                                 Assert.That(args, Are.EqualTo(original));
                                                 Assert.That(sender, Is.SameAs(mHandlers));
                                                 ++count;
                                               };
      mHandlers
        .OnFileCreated += evt;
      mHandlers
        .OnFileCreated += evt;
      mHandlers
        .OnFileCreated += evt;
      mHandlers.HandleFileCreated(original);
      Assert.That(count, Is.EqualTo(3));
    }

    [SetUp]
    public void DoSetup() {
      mHandlers = new EventHandlers();
    }

    private EventHandlers mHandlers;
  }
}