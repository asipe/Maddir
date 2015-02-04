﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using Maddir.Core.Model;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Core.TreeGeneration {
  public class Engine {
    public Engine(IDirectory directory, IFile file) {
      mDirectory = directory;
      mFile = file;
    }

    public void Apply(string root, Layout layout) {
      Array.ForEach(layout.Commands, command => ProcessCommand(root, command));
    }

    private void ProcessCommand(string root, ICommand command) {
      new EntryAction(entry => ProcessFile(root, entry),
                      entry => ProcessDirectory(root, entry))
        .Execute(command.Entry);
    }

    private void ProcessFile(string root, IEntry entry) {
      mFile
        .WriteAllText(Path.Combine(root, entry.Name), "");
    }

    private void ProcessDirectory(string root, IEntry entry) {
      mDirectory
        .CreateDirectory(Path.Combine(root, entry.Name));
    }

    private readonly IDirectory mDirectory;
    private readonly IFile mFile;
  }
}