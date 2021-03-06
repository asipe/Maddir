﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Model {
  public class FileEntry : BaseEntry {
    public FileEntry(int level, string name, string contents) : base(EntryType.File, level, name) {
      Contents = contents;
    }

    public string Contents{get;set;}
  }
}