// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Maddir.Core.Model;

namespace Maddir.Core.TreeGeneration {
  public class Splitter {
    public Splitter(Settings settings) {
      var starting = Regex.Escape(settings.ContentStartDelimiter.ToString());
      var ending = Regex.Escape(settings.ContentEndDelimiter.ToString());
      mFilePattern = new Regex(@"^(f)(  )+\b([^" + starting + "]+) " + starting + "([^" + ending + "]*)" + ending + "\r?$", RegexOptions.Multiline);
    }

    public IEntry[] Split(string markup) {
      markup = markup.Trim();

      var matches = GetMatches(mFilePattern, markup)
        .Concat(GetMatches(mDirPattern, markup))
        .OrderBy(match => match.Index)
        .ToArray();

      ValidateMatches(markup, matches);

      return matches
        .Select(BuildEntry)
        .ToArray();
    }

    private static void ValidateMatches(string markup, IEnumerable<Match> matches) {
      if (matches.Sum(match => Clean(match.Value).Length) != Clean(markup).Length)
        throw new MaddirException("Cannot Parse Markup: '{0}'", markup);
    }

    private static string Clean(string value) {
      return value
        .Replace("\r", "")
        .Replace("\n", "");
    }

    private static IEnumerable<Match> GetMatches(Regex regex, string markup) {
      return regex
        .Matches(markup)
        .Cast<Match>();
    }

    private static IEntry BuildEntry(Match match) {
      switch (match.Groups[1].Value) {
        case "f":
          return new FileEntry(match.Groups[2].Captures.Count - 1,
                               match.Groups[3].Value.Trim(),
                               match.Groups[4].Value);
        case "d":
          return new DirectoryEntry(match.Groups[2].Captures.Count - 1,
                                    match.Groups[3].Value.Trim());
        default:
          throw new MaddirException("Invalid Type: {0}", match.Groups[1].Value);
      }
    }

    private readonly Regex mDirPattern = new Regex(@"^(d)(  )+\b(.+)\r?$", RegexOptions.Compiled | RegexOptions.Multiline);
    private readonly Regex mFilePattern;
  }
}