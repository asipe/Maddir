// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using Maddir.Core.Model;

namespace Maddir.Core.TreeGeneration {
  public class LineParser {
    public IEntry Parse(string line) {
      var match = _FilePattern.Match(line);

      if (match.Success)
        return new FileEntry(match.Groups[2].Captures.Count - 1,
                             match.Groups[3].Value.Trim(),
                             match.Groups[4].Value);

      match = _DirPattern.Match(line);

      if (!match.Success)
        throw new MaddirException("Cannot Parse Markup: '{0}'", line);

      return new DirectoryEntry(match.Groups[2].Captures.Count - 1,
                                match.Groups[3].Value.Trim());
    }

    private static readonly Regex _FilePattern = new Regex(@"^(f)(  )+\b([^\[]+) \[(.*)\]$", RegexOptions.Compiled);
    private static readonly Regex _DirPattern = new Regex(@"^(d)(  )+\b(.+)$", RegexOptions.Compiled);
  }
}