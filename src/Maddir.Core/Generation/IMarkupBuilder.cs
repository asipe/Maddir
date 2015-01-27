// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using Maddir.Core.Model;

namespace Maddir.Core.Generation {
  public interface IMarkupBuilder {
    string Build();
    void Add(IEntry info);
  }
}