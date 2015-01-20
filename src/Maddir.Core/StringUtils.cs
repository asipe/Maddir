// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Maddir.Core {
  public static class StringUtils {
    public static string ToNewLineSepString(params string[] values) {
      return string.Join(Environment.NewLine, values);
    }
  }
}