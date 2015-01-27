// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Model {
  public class DirectoryEntry : BaseEntry {
    public DirectoryEntry(int level, string name) : base(EntryType.Directory, level, name) {}
  }
}