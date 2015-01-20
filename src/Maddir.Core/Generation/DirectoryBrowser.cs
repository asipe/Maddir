// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core.Generation {
  public class DirectoryBrowser {
    public DirectoryBrowser(IDirectory directory) {
      mDirectory = directory;
    }

    public object Browse(string root) {
      return mDirectory
        .GetDirectories(root)
        .Select(path => path.Split(Path.DirectorySeparatorChar))
        .Select(parts => parts.Last())
        .ToArray();
    }

    private readonly IDirectory mDirectory;
  }
}