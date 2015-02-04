// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.MarkupGeneration;
using Snarfz.Core;

namespace Maddir.Core {
  public static class Maddirs {
    public static string BuildMarkup(string path) {
      return new Engine(new Builder())
        .Apply(new DirectoryBrowser(Snarfzer.NewScanner()).Browse(path));
    }
  }
}