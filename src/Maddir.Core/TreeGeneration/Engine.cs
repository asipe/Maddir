// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using Maddir.Core.Events;
using Maddir.Core.Model;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core.TreeGeneration {
  public class Engine : ITreeGenerationEngine {
    public Engine(IDirectory directory, IFile file) {
      mDirectory = directory;
      mFile = file;
    }

    public void Apply(Settings settings, string root, Layout layout) {
      Array.ForEach(layout.Commands, command => ProcessCommand(settings, root, command));
    }

    private void ProcessCommand(Settings settings, string root, ICommand command) {
      new EntryAction(entry => ProcessFile(settings, root, entry),
                      entry => ProcessDirectory(settings, root, entry))
        .Execute(command.Entry);
    }

    private void ProcessFile(Settings settings, string root, FileEntry entry) {
      CreateFile(settings, Path.Combine(root, entry.Name), entry.Contents);
    }

    private void CreateFile(Settings settings, string path, string contents) {
      mFile
        .WriteAllText(path, contents);
      settings
        .Handlers
        .HandleFileCreated(new FileCreatedEventArgs(new FileInfo(path)));
    }

    private void ProcessDirectory(Settings settings, string root, IEntry entry) {
      CreateDirectory(settings, Path.Combine(root, entry.Name));
    }

    private void CreateDirectory(Settings settings, string path) {
      mDirectory
        .CreateDirectory(path);
      settings
        .Handlers
        .HandleDirectoryCreated(new DirectoryCreatedEventArgs(new DirectoryInfo(path)));
    }

    private readonly IDirectory mDirectory;
    private readonly IFile mFile;
  }
}