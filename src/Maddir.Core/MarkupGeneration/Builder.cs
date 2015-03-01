// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Maddir.Core.Model;
using Maddir.Core.Utility;

namespace Maddir.Core.MarkupGeneration {
  public class Builder : IMarkupBuilder {
    public Builder(Settings settings) {
      mRowProvider = new EntryFunc<string>(e => string.Format("f{0}{1} {2}{3}{4}",
                                                              new string(' ', GetIndent(e)),
                                                              e.Name,
                                                              settings.ContentStartDelimiter,
                                                              e.Contents,
                                                              settings.ContentEndDelimiter),
                                           e => string.Format("d{0}{1}",
                                                              new string(' ', GetIndent(e)),
                                                              e.Name));
    }

    public string Build() {
      var lines = mEntries
        .Select(entry => mRowProvider.Execute(entry))
        .ToArray();

      return StringUtils
        .ToNewLineSepString(lines);
    }

    public void Add(IEntry info) {
      mEntries.Add(info);
    }

    private static int GetIndent(IEntry entry) {
      return (entry.Level + 1) * 2;
    }

    private readonly List<IEntry> mEntries = new List<IEntry>();
    private readonly EntryFunc<string> mRowProvider;
  }
}