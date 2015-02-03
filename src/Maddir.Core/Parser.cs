// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;

namespace Maddir.Core {
  public class Parser {
    public Layout Parse(string markup) {
      var map = new Dictionary<int, string>();
      map[0] = "";

      var commands = markup
        .Split(new[] {Environment.NewLine}, StringSplitOptions.None)
        .Select(line => line.Trim())
        .Where(line => line.Length > 0)
        .Select(line => ProcessLine(map, line))
        .ToArray();
      return new Layout(commands);
    }

    private static ICommand ProcessLine(IDictionary<int, string> map, string line) {
      var entry = _LineParser.Parse(line);

      if (entry.Type == EntryType.File)
        return new AddFileCommand(entry.Level,
                                  Path.Combine(map[entry.Level], entry.Name));

      var name = Path.Combine(map[entry.Level], entry.Name);
      var command = new AddDirectoryCommand(entry.Level, name);
      map[entry.Level + 1] = name;
      return command;
    }

    private static readonly LineParser _LineParser = new LineParser();
  }
}