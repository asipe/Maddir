﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Model {
  public class Layout {
    public Layout(params ICommand[] commands) {
      Commands = commands;
    }

    public ICommand[] Commands{get;private set;}
  }
}