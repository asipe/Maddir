// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;

namespace Maddir.Core.Events {
  public class FileCreatedEventArgs : EventArgs {
    public FileCreatedEventArgs(FileInfo info) {
      Info = info;
    }

    public FileInfo Info{get;private set;}
  }
}