// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Maddir.Core.Model;

namespace Maddir.Core.Generation {
  public class MarkupBuilder : IMarkupBuilder {
    public string Build() {
      var lines = mEntries
        .Select(entry => new {
                               entry.Name,
                               Marker = GetMarker(entry),
                               Indent = GetIndent(entry)
                             })
        .Select(entry => string.Format("{0}{1}{2}", entry.Marker, new string(' ', entry.Indent), entry.Name))
        .ToArray();

      return StringUtils
        .ToNewLineSepString(lines);
    }

    public void Add(IEntry info) {
      mEntries.Add(info);
    }

    private static string GetMarker(IEntry entry) {
      return (entry.Type == EntryType.Directory)
               ? "d"
               : "f";
    }

    private static int GetIndent(IEntry entry) {
      return (entry.Level + 1) * 2;
    }

    private readonly List<IEntry> mEntries = new List<IEntry>();
  }
}