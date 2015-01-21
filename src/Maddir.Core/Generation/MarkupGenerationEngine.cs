// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Maddir.Core.Commands;
using Maddir.Core.Model;

namespace Maddir.Core.Generation {
  public class MarkupGenerationEngine : ICommandVisitor {
    public void Visit(AddDirectoryCommand command) {
      mLines.Add(command.Properties.Path);
    }

    public string Apply(Layout layout) {
      mLines.Clear();
      Array.ForEach(layout.Commands, cmd => cmd.Accept(this));
      return StringUtils.ToNewLineSepString(mLines.ToArray());
    }

    private readonly List<string> mLines = new List<string>();
  }
}