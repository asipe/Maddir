// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core.Generation {
  public class DirectoryBrowser {
    public DirectoryBrowser(IDirectory directory) {
      mDirectory = directory;
    }

    public Layout Browse(string root) {
      return new Layout(GetDirectories(root)
                          .Concat(GetFiles(root))
                          .ToArray());
    }

    private IEnumerable<ICommand> GetDirectories(string root) {
      return mDirectory
        .GetDirectories(root)
        .Select(path => path.Split(Path.DirectorySeparatorChar))
        .Select(parts => parts.Last())
        .Select(path => new AddDirectoryCommand(0, path));
    }

    private IEnumerable<ICommand> GetFiles(string root) {
      return mDirectory
        .GetFiles(root, "*")
        .Select(Path.GetFileName)
        .Select(path => new AddFileCommand(0, path));
    }

    private readonly IDirectory mDirectory;
  }
}