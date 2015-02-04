// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.MarkupGeneration;
using Maddir.Core.TreeGeneration;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core {
  public static class Maddirs {
    public static string BuildMarkup(string path) {
      return new MarkupGeneration.Engine(new Builder())
        .Apply(new DirectoryBrowser(Snarfzer.NewScanner()).Browse(path));
    }

    public static void ApplyMarkup(string path, string markup) {
      new TreeGeneration.Engine(new DotNetDirectory(), new DotNetFile())
        .Apply(path, new Parser().Parse(markup));
    }
  }
}