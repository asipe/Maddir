// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.
namespace Maddir.IntegrationTests.Infrastructure {
  public class MaddirHelper {
    public TestEnvironment TestEnvironment{get;private set;}
    public PathInfo PathInfo{get;private set;}

    public void Setup() {
      TestEnvironment = GlobalSetup.TestEnvironment;
      PathInfo = GlobalSetup.PathInfo;
    }
  }
}