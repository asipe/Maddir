// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;

namespace Maddir.Core.Events {
  public class DirectoryCreatedEventArgs : EventArgs {
    public DirectoryCreatedEventArgs(DirectoryInfo info) {
      Info = info;
    }

    public DirectoryInfo Info{get;private set;}
  }
}