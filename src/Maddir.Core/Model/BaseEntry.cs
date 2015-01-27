// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Model {
  public class BaseEntry : IEntry {
    public BaseEntry(EntryType type, int level, string name) {
      Type = type;
      Level = level;
      Name = name;
    }

    public EntryType Type{get;private set;}
    public int Level{get;private set;}
    public string Name{get;private set;}
  }
}