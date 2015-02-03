// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maddir.Core.Commands;
using Maddir.Core.Model;
using Snarfz.Core;

namespace Maddir.Core.MarkupGeneration {
  public class DirectoryBrowser {
    public DirectoryBrowser(IScanner scanner) {
      mScanner = scanner;
    }

    public Layout Browse(string root) {
      var level = new Level(root);
      var commands = new List<ICommand>();
      var config = new Config(root) {
                                      EventErrorMode = EventErrorMode.Throw,
                                      ScanErrorMode = ScanErrorMode.Throw
                                    };
      var count = 0;
      config.OnDirectory += (o, a) => ProcessDirectory(commands, level, a, ref count);
      config.OnFile += (o, a) => ProcessFile(commands, level, a);
      mScanner.Start(config);
      return new Layout(commands.ToArray());
    }

    private static void ProcessFile(ICollection<ICommand> commands, Level level, BaseVisitEventArgs args) {
      commands
        .Add(new AddFileCommand(level.GetForPath(args.Path),
                                Path.GetFileName(args.Path)));
    }

    private static void ProcessDirectory(ICollection<ICommand> commands, Level level, BaseVisitEventArgs args, ref int count) {
      if (count == 0) {
        ++count;
        return;
      }

      commands
        .Add(new AddDirectoryCommand(level.GetForPath(args.Path),
                                     ExtractDirectoryName(args)));
    }

    private static string ExtractDirectoryName(BaseVisitEventArgs args) {
      return args
        .Path
        .Split(Path.DirectorySeparatorChar)
        .Last();
    }

    private readonly IScanner mScanner;
  }
}