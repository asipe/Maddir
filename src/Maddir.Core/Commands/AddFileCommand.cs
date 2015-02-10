// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;

namespace Maddir.Core.Commands {
  public class AddFileCommand : ICommand {
    public AddFileCommand(int level, string path, string contents) {
      Entry = new FileEntry(level, path, contents);
    }

    public IEntry Entry{get;private set;}
  }
}