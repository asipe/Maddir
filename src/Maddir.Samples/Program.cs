// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using Maddir.Core;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Samples {
  internal class Program {
    private static int Main() {
      try {
        Execute2();
        return 0;
      } catch (Exception e) {
        Console.WriteLine(e);
      }
      return 1;
    }

    private static void Execute2() {
      var workingDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test");
      Console.WriteLine("Working Directory is {0}", workingDir);
      Console.Write("Continue (Y|N): ");
      if (Console.ReadLine() != "Y")
        return;

      if (Directory.Exists(workingDir))
        new DotNetDirectory().Delete(workingDir, 250);

      Directory.CreateDirectory(workingDir);

      //create a single directory with a single file
      var markup = @"
d  single
f    file1.txt [file1.txt contents]".Trim();
      Maddirs.ApplyMarkup(new Settings(), workingDir, markup);

      //create a directory tree
      markup = @"
d  tree
d    directory1
d      directory2
d        directory 3
f          file1.txt [file1.txt contents]".Trim();
      Maddirs.ApplyMarkup(new Settings(), workingDir, markup);

      //create a directory tree using a custom set of delimters for file content
      markup = @"
d  customdelim
d    directory1
d      directory2
d        directory 3
f          file1.txt <file1.txt contents>".Trim();
      var settings = new Settings {
                                    ContentStartDelimiter = '<',
                                    ContentEndDelimiter = '>'
                                  };
      Maddirs.ApplyMarkup(settings, workingDir, markup);

      //create a directory tree with all file contents specified by callbacks
      markup = @"
d  callback1
d    directory1
f      file1.txt []
d      directory2
f        file2.txt []
d        directory 3
f          file3.txt []
f          file4.txt []".Trim();
      settings = new Settings();
      settings.OnFileCreated += (sender, args) => File.WriteAllText(args.Info.FullName, "sample text");
      Maddirs.ApplyMarkup(settings, workingDir, markup);

      //create a directory tree with directory contens specified by callbacks
      markup = @"
d  callback2
d    directory1
d      directory2
d        directory 3".Trim();
      settings = new Settings();
      settings.OnDirectoryCreated += (sender, args) => File.WriteAllText(Path.Combine(args.Info.FullName, "file.txt"), "data");
      Maddirs.ApplyMarkup(settings, workingDir, markup);

      //create a single directory with a single file with multiline comments
      markup = @"
d  multilinecontents
f    file1.txt [
  supports multi
  line file contents
]".Trim();
      Maddirs.ApplyMarkup(new Settings(), workingDir, markup);

      //generate a layout and reuse/apply it to multiple locations
      markup = @"
d  cloned
d    clonedsub
f      file1.txt []".Trim();
      var layout = new Parser(new Settings()).Parse(markup);
      var cloned1 = Path.Combine(workingDir, "cloned1");
      var cloned2 = Path.Combine(workingDir, "cloned2");
      var cloned3 = Path.Combine(workingDir, "cloned3");
      Directory.CreateDirectory(cloned1);
      Directory.CreateDirectory(cloned2);
      Directory.CreateDirectory(cloned3);
      var engine = new Engine(new DotNetDirectory(), new DotNetFile());
      engine.Apply(new Settings(), cloned1, layout);
      engine.Apply(new Settings(), cloned2, layout);
      engine.Apply(new Settings(), cloned3, layout);

      //generate markup for out test directories using standard settings
      Console.WriteLine(Maddirs.BuildMarkup(new Settings(), workingDir));

      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine();

      //generate markup for out test directories using custom file content delimiters
      settings = new Settings {
                                ContentStartDelimiter = '<',
                                ContentEndDelimiter = '>'
                              };
      Console.WriteLine(Maddirs.BuildMarkup(settings, workingDir));
    }
  }
}