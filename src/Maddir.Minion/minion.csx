// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.
using System.Diagnostics;
using Conz.Core;

var conzole = new Conzole(Conz.Core.BuiltInStyles.ForegroundColorOnly._Instance);
 
var config = Require<FitterBuilder>().Build(new {
  RootDir = Directory.GetCurrentDirectory(),
  DebugDir = @"<rootdir>\debug",
  SourceDir = @"<rootdir>\src",
  ThirdpartyDir = @"<rootdir>\thirdparty",
  PackagesDir = @"<thirdpartydir>\packages",
  NugetExePath = @"<thirdpartydir>\nuget\nuget.exe",
  NunitConsoleExePath = @"<thirdpartydir>\packages\common\Nunit.Runners\tools\nunit-console.exe"
});

void TryDelete(string dir) {
  if (Directory.Exists(dir))
    Directory.Delete(dir, true);
}

void Clean() {
  TryDelete(config["DebugDir"]);
}

void CleanAll() {
  Clean();
  TryDelete(config["PackagesDir"]);
}

void Echo(string message) {
  conzole.WriteLine("");
  conzole.WriteLine(message);
}

void Bootstrap() {
  Run(config["NugetExePath"], @"install .\src\Maddir.Nuget.Packages\common\packages.config -OutputDirectory .\thirdparty\packages\common -ExcludeVersion");
  Run(config["NugetExePath"], @"install .\src\Maddir.Nuget.Packages\net-4.5\packages.config -OutputDirectory .\thirdparty\packages\net-4.5 -ExcludeVersion");
}

void Run(string exePath, string args) {
  var info = new ProcessStartInfo {
    FileName = exePath,
    Arguments = args,
    UseShellExecute = false
  };
  
  using (var p = Process.Start(info)) {
    p.WaitForExit();  

    if (p.ExitCode != 0)
      throw new Exception(string.Format("{0} failed with code {1}", exePath, p.ExitCode));
  }
}

void BuildAll() {
  Run(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe", @".\src\Maddir.Build\Maddir.proj /ds /maxcpucount:6");
}

void RunTests(string name, string assembly, string framework) {
  conzole.WriteLine("|y|----------- {0} Tests {1} -----------|", name, framework);
  conzole.WriteLine("|y|-- {0}|", assembly);
  Run(config["NunitConsoleExePath"], string.Format(@"{0} /nologo /framework:{1}", assembly, framework));
  conzole.WriteLine("|y|------------------------------------------|");
  conzole.WriteLine();
}

void RunUnitTestsVS() {
  RunTests("VS Unit", @".\src\Maddir.UnitTests\bin\debug\Maddir.UnitTests.dll", "net-4.5");
}

void RunIntegrationTestsVS() {
  RunTests("VS Integration", @".\src\Maddir.IntegrationTests\bin\debug\Maddir.IntegrationTests.dll", "net-4.5");
}

void RunUnitTestsDebug() {
  RunTests("Debug Unit", Path.Combine(config["DebugDir"], @"net-4.5\Maddir.UnitTests\Maddir.UnitTests.dll"), "net-4.5");
}

void RunIntegrationTestsDebug() {
  RunTests("Debug Integration", Path.Combine(config["DebugDir"], @"net-4.5\Maddir.IntegrationTests\Maddir.IntegrationTests.dll"), "net-4.5");
}

void RunAllTests() {
  RunUnitTestsVS();
  RunUnitTestsDebug();
  RunIntegrationTestsVS();
  RunIntegrationTestsDebug();
}

void ProcessCommands() {
  var exiting = false;

  while (!exiting) {
    conzole.WriteLine("");
    conzole.Write("Waiting: ");

    try {
      var commands = conzole
        .ReadLine()
        .Split(',')
        .Select(s => s.Trim());

      foreach (var command in commands) {
        switch (command) {
          case ("exit"):
          case ("x"):
            exiting = true;
            Echo("Goodbye");
            break;
          case ("clean"):
            Clean();
            break;
          case ("clean.all"):
            CleanAll();
            break;
          case ("bootstrap"):
            Bootstrap();
            break;
          case ("build.all"):
            BuildAll();
            break;  
          case ("run.unit.tests.vs"):
            RunUnitTestsVS();
            break;  
          case ("run.unit.tests.debug"):
            RunUnitTestsDebug();
            break;           
          case ("run.integration.tests.vs"):
            RunIntegrationTestsVS();
            break;             
          case ("run.integration.tests.debug"):
            RunIntegrationTestsDebug();
            break;                       
          case ("run.all.tests"):
            RunAllTests();
            break; 
          case ("cycle"):
            Clean();
            BuildAll();
            RunAllTests();
            break;
          default: 
            throw new Exception("Unknown Command: " + command);
            break;
        }
      }
    } catch (Exception e) {
      conzole.WriteLine("");
      conzole.WriteLine("|r|{0}|", e);
    }
  }
}

ProcessCommands();
