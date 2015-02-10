// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Maddir.Core.Events;

namespace Maddir.Core.Model {
  public class Settings {
    public Settings() {
      Handlers = new EventHandlers();
    }

    public EventHandlers Handlers{get;private set;}

    public event EventHandler<DirectoryCreatedEventArgs> OnDirectoryCreated {
      add {Handlers.OnDirectoryCreated += value;}
      remove {Handlers.OnDirectoryCreated -= value;}
    }

    public event EventHandler<FileCreatedEventArgs> OnFileCreated {
      add {Handlers.OnFileCreated += value;}
      remove {Handlers.OnFileCreated -= value;}
    }
  }
}