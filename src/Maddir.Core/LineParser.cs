// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using Maddir.Core.Model;

namespace Maddir.Core {
  public class LineParser {
    public IEntry Parse(string line) {
      var match = _Pattern.Match(line);

      if (!match.Success)
        throw new MaddirException("Cannot Parse Markup: '{0}'", line);

      var level = match.Groups[2].Captures.Count - 1;
      var name = match.Groups[3].Value;
      var type = match.Groups[1].Value;

      return (type == "f")
               ? (IEntry)new FileEntry(level, name)
               : new DirectoryEntry(level, name);
    }

    private static readonly Regex _Pattern = new Regex(@"^(f|d)(  )+\b(.+)$", RegexOptions.Compiled);
  }
}