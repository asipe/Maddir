// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;

namespace Maddir.Core.TreeGeneration {
  public class Parser : IMarkupParser {
    public Parser(Settings settings) {
      mSplitter = new Splitter(settings);
    }

    public Layout Parse(string markup) {
      var map = new Dictionary<int, string> {{0, ""}};
      return new Layout(mSplitter
                          .Split(markup)
                          .Select(entry => ProcessLine(map, entry))
                          .ToArray());
    }

    private static ICommand ProcessLine(IDictionary<int, string> map, IEntry entry) {
      ICommand command = null;
      new EntryAction(ety => command = ProcessFile(map, ety),
                      ety => command = ProcessDirectory(map, ety))
        .Execute(entry);
      return command;
    }

    private static ICommand ProcessDirectory(IDictionary<int, string> map, IEntry entry) {
      var name = Path.Combine(map[entry.Level], entry.Name);
      map[entry.Level + 1] = name;
      return new AddDirectoryCommand(entry.Level, name);
    }

    private static ICommand ProcessFile(IDictionary<int, string> map, FileEntry entry) {
      return new AddFileCommand(entry.Level,
                                Path.Combine(map[entry.Level], entry.Name),
                                entry.Contents);
    }

    private readonly Splitter mSplitter;
  }
}