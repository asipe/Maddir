// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;

namespace Maddir.Core.TreeGeneration {
  public class Parser : IMarkupParser {
    public Layout Parse(string markup) {
      var map = new Dictionary<int, string> {{0, ""}};

      var commands = markup
        .Split(new[] {Environment.NewLine}, StringSplitOptions.None)
        .Select(line => line.Trim())
        .Where(line => line.Length > 0)
        .Select(line => ProcessLine(map, line))
        .ToArray();
      return new Layout(commands);
    }

    private static ICommand ProcessLine(IDictionary<int, string> map, string line) {
      ICommand command = null;
      new EntryAction(entry => command = ProcessFile(map, entry),
                      entry => command = ProcessDirectory(map, entry))
        .Execute(_LineParser.Parse(line));
      return command;
    }

    private static ICommand ProcessDirectory(IDictionary<int, string> map, IEntry entry) {
      var name = Path.Combine(map[entry.Level], entry.Name);
      map[entry.Level + 1] = name;
      return new AddDirectoryCommand(entry.Level, name);
    }

    private static ICommand ProcessFile(IDictionary<int, string> map, IEntry entry) {
      return new AddFileCommand(entry.Level,
                                Path.Combine(map[entry.Level], entry.Name));
    }

    private static readonly LineParser _LineParser = new LineParser();
  }
}