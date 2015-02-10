// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using SupaCharge.Core.IOAbstractions;

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
      _Directory.Delete(mInfo.TestWorkingDir, 25);
    }

    private void CreateDirectories() {
      if (!_Directory.Exists(mInfo.TestRootDir))
        throw new Exception(string.Format("{0} directory does not exist", mInfo.TestRootDir));
      _Directory.CreateDirectory(mInfo.TestDataDir);
    }

    private void Validate() {
      if (_Directory.Exists(mInfo.TestWorkingDir))
        throw new Exception(string.Format("{0} already exists", mInfo.TestWorkingDir));
    }

    private static readonly DotNetDirectory _Directory = new DotNetDirectory();
    private readonly PathInfo mInfo;
  }
}