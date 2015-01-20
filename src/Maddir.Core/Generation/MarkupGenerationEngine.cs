// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Maddir.Core.Generation {
  public class MarkupGenerationEngine {
    public string Apply(object layout) {
      return StringUtils.ToNewLineSepString((string[])layout);
    }
  }
}