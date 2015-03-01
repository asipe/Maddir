// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using Maddir.Core.Model;

namespace Maddir.Core.TreeGeneration {
  public class LineParser {
    public LineParser(Settings settings) {
      var starting = Regex.Escape(settings.ContentStartDelimiter.ToString());
      var ending = Regex.Escape(settings.ContentEndDelimiter.ToString());
      mFilePattern = new Regex(@"^(f)(  )+\b([^" + starting + "]+) " + starting + "(.*)" + ending + "$", RegexOptions.Compiled);
    }

    public IEntry Parse(string line) {
      var match = mFilePattern.Match(line);

      if (match.Success)
        return new FileEntry(match.Groups[2].Captures.Count - 1,
                             match.Groups[3].Value.Trim(),
                             match.Groups[4].Value);

      match = mDirPattern.Match(line);

      if (!match.Success)
        throw new MaddirException("Cannot Parse Markup: '{0}'", line);

      return new DirectoryEntry(match.Groups[2].Captures.Count - 1,
                                match.Groups[3].Value.Trim());
    }

    private readonly Regex mDirPattern = new Regex(@"^(d)(  )+\b(.+)$", RegexOptions.Compiled);
    private readonly Regex mFilePattern;
  }
}