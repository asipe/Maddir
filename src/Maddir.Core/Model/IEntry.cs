// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Model {
  public interface IEntry {
    EntryType Type{get;}
    int Level{get;}
    string Name{get;}
  }
}