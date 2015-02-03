// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.IO;
using System.Linq;

namespace Maddir.Core.MarkupGeneration {
  public class Level {
    public Level(string root) {
      mRoot = root;
    }

    public int GetForPath(string path) {
      return path
               .Replace(mRoot, "")
               .Trim('\\')
               .Split(Path.DirectorySeparatorChar)
               .Count() - 1;
    }

    private readonly string mRoot;
  }
}