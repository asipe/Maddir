// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.MarkupGeneration;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;
using Engine = Maddir.Core.MarkupGeneration.Engine;

namespace Maddir.Core {
  public static class Maddirs {
    public static string BuildMarkup(string path) {
      return new Engine(new Builder())
        .Apply(new DirectoryBrowser(Snarfzer.NewScanner(), new DotNetFile()).Browse(path));
    }

    public static void ApplyMarkup(Settings settings, string path, string markup) {
      new TreeGeneration.Engine(new DotNetDirectory(), new DotNetFile())
        .Apply(settings, path, new Parser().Parse(markup));
    }
  }
}