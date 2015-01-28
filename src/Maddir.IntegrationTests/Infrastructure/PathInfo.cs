// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.IO;

namespace Maddir.IntegrationTests.Infrastructure {
  public class PathInfo {
    public PathInfo(string root, string id) {
      Root = root;
      TestRootDir = Path.Combine(root, "integrationtestworking");
      TestWorkingDir = Path.Combine(TestRootDir, id);
      TestDataDir = Path.Combine(TestWorkingDir, "data");
    }

    public string TestDataDir{get;private set;}
    public string TestRootDir{get;private set;}
    public string Root{get;private set;}
    public string TestWorkingDir{get;private set;}

    public string GetDataPath(string path) {
      return Path.Combine(TestDataDir, path);
    }
  }
}