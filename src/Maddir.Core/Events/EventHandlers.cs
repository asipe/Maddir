// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Maddir.Core.Events {
  public class EventHandlers {
    public event EventHandler<DirectoryCreatedEventArgs> OnDirectoryCreated;
    public event EventHandler<FileCreatedEventArgs> OnFileCreated;

    public void HandleDirectoryCreated(DirectoryCreatedEventArgs args) {
      OnDirectoryCreated.RaiseEvent(this, args);
    }

    public void HandleFileCreated(FileCreatedEventArgs args) {
      OnFileCreated.RaiseEvent(this, args);
    }
  }
}