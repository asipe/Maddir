// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.
using System;
using System.IO;

namespace Maddir.IntegrationTests.Infrastructure {
  public class TestEnvironment {
    public TestEnvironment(PathInfo info) {
      mInfo = info;
    }

    public void Setup() {
      Validate();
      CreateDirectories();
    }

    public void TearDown() {
      if (Directory.Exists(mInfo.TestWorkingDir))
        Directory.Delete(mInfo.TestWorkingDir, true);
    }

    private void CreateDirectories() {
      if (!Directory.Exists(mInfo.TestRootDir))
        Directory.CreateDirectory(mInfo.TestRootDir);
      Directory.CreateDirectory(mInfo.TestDataDir);
    }

    private void Validate() {
      if (Directory.Exists(mInfo.TestWorkingDir))
        throw new Exception(string.Format("{0} already exists", mInfo.TestWorkingDir));
    }

    private readonly PathInfo mInfo;
  }
}