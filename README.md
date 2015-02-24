# Maddir

Micro library for generating directory trees and files.  It was created to support automated tests which sometimes require a directory tree created in a specific manner to exercise the system under test. 

Maddir allows the developer to create markup which represents the desired directory tree structure and then apply the markup to a specific location on the file system.   Callbacks are available to customize all aspects of directory and file creation.

Maddir also allows markup to be created from a directory tree.  This feature was developed to help with the creation of maddir and only provides basic support for reading file contents (does not support binary files or multi line files).

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
      var settings = new Settings();
      settings.OnFileCreated += (sender, args) => File.WriteAllText(args.Info.FullName, "sample text");
      Maddirs.ApplyMarkup(settings, workingDir, markup);

      //create a directory tree with directory contents specified by callbacks
      markup = @"
d  callback2
d    directory1
d      directory2
d        directory 3".Trim();
      settings = new Settings();
      settings.OnDirectoryCreated += (sender, args) => File.WriteAllText(Path.Combine(args.Info.FullName, "file.txt"), "data");
      Maddirs.ApplyMarkup(settings, workingDir, markup);

      //generate markup for out test directories
      Console.WriteLine(Maddirs.BuildMarkup(workingDir));
```

### Nuget

coming soon

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
