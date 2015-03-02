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

      return GetMatches(mFilePattern, markup)
        .Concat(GetMatches(mDirPattern, markup))
        .OrderBy(match => match.Index)
        .Select(BuildEntry)
        .ToArray();
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