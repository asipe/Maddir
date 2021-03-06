﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;

namespace Maddir.Core.Commands {
  public class AddDirectoryCommand : ICommand {
    public AddDirectoryCommand(int level, string path) {
      Entry = new DirectoryEntry(level, path);
    }

    public IEntry Entry{get;private set;}
  }
}