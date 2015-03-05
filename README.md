# Maddir

[![Maddir.Core on NuGet](http://img.shields.io/nuget/v/Maddir.Core.svg?style=flat)](https://www.nuget.org/packages/Maddir.Core)
[![Maddir.Core tag](http://img.shields.io/github/tag/asipe/Maddir.svg?style=flat)](https://github.com/asipe/Maddir/tags)
[![Maddir.Core license](http://img.shields.io/badge/license-mit-blue.svg?style=flat)](https://raw.githubusercontent.com/asipe/Maddir/master/LICENSE.txt)

Micro library for generating directory trees and files.  It was initially created to support automated tests which sometimes require a directory tree created in a specific manner to exercise the system under test. 

Maddir allows the developer to create markup which represents the desired directory tree structure and then apply the markup to a specific location on the file system.   Callbacks are available to customize all aspects of directory and file creation.

Maddir also allows markup to be created from a directory tree.  This feature was developed to help with the creation of maddir and only provides basic support for reading file contents (does not support binary files very well, etc...).

### Usage
```csharp
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
```

### Nuget

nuget package (Maddir.Core): http://www.nuget.org/packages/Maddir.Core/

install via package manager:  Install-Package Maddir.Core

### License

Maddir is licensed under the MIT License

     The MIT License (MIT)
     
     Copyright (c) 2015 Andy Sipe
     
     Permission is hereby granted, free of charge, to any person obtaining a copy of
     this software and associated documentation files (the "Software"), to deal in
     the Software without restriction, including without limitation the rights to
     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
     the Software, and to permit persons to whom the Software is furnished to do so,
     subject to the following conditions:
     
     The above copyright notice and this permission notice shall be included in all
     copies or substantial portions of the Software.
     
     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
     FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
     COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
     IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
