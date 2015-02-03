// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Maddir.Core.Model;

namespace Maddir.Core.MarkupGeneration {
  public class MarkupGenerationEngine {
    public MarkupGenerationEngine(IMarkupBuilder markupBuilder) {
      mMarkupBuilder = markupBuilder;
    }

    public string Apply(Layout layout) {
      Array.ForEach(layout.Commands, cmd => mMarkupBuilder.Add(cmd.Entry));
      return mMarkupBuilder.Build();
    }

    private readonly IMarkupBuilder mMarkupBuilder;
  }
}