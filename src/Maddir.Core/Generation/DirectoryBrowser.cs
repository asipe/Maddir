// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

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
      return new Layout(mDirectory
                          .GetDirectories(root)
                          .Select(path => path.Split(Path.DirectorySeparatorChar))
                          .Select(parts => parts.Last())
                          .Select(path => new AddDirectoryCommand(path))
                          .ToArray());
    }

    private readonly IDirectory mDirectory;
  }
}