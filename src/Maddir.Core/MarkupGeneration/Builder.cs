// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Maddir.Core.Model;
using Maddir.Core.Utility;

namespace Maddir.Core.MarkupGeneration {
  public class Builder : IMarkupBuilder {
    public string Build() {
      var lines = mEntries
        .Select(entry => _RowProvider.Execute(entry))
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

    private static readonly EntryFunc<string> _RowProvider = new EntryFunc<string>(e => string.Format("f{0}{1} [{2}]", new string(' ', GetIndent(e)), e.Name, e.Contents),
                                                                                   e => string.Format("d{0}{1}", new string(' ', GetIndent(e)), e.Name));

    private readonly List<IEntry> mEntries = new List<IEntry>();
  }
}