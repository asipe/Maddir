// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core.MarkupGeneration {
  public class DirectoryBrowser {
    public DirectoryBrowser(IScanner scanner, IFile file) {
      mScanner = scanner;
      mFile = file;
    }

    public Layout Browse(string root) {
      var level = new Level(root);
      var commands = new List<ICommand>();
      var config = new Config(root) {
                                      EventErrorMode = EventErrorMode.Throw,
                                      ScanErrorMode = ScanErrorMode.Throw
                                    };
      var count = 0;
      config.OnDirectory += (o, a) => ProcessDirectory(commands, level, a.Path, ref count);
      config.OnFile += (o, a) => ProcessFile(commands, level, a.Path);
      mScanner.Start(config);
      return new Layout(commands.ToArray());
    }

    private void ProcessFile(ICollection<ICommand> commands, Level level, string path) {
      commands
        .Add(new AddFileCommand(level.GetForPath(path),
                                Path.GetFileName(path),
                                mFile.ReadAllText(path)));
    }

    private static void ProcessDirectory(ICollection<ICommand> commands, Level level, string path, ref int count) {
      if (count == 0) {
        ++count;
        return;
      }

      commands
        .Add(new AddDirectoryCommand(level.GetForPath(path),
                                     ExtractDirectoryName(path)));
    }

    private static string ExtractDirectoryName(string path) {
      return path
        .Split(Path.DirectorySeparatorChar)
        .Last();
    }

    private readonly IFile mFile;
    private readonly IScanner mScanner;
  }
}