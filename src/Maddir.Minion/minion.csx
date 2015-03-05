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
  NunitConsoleExePath = @"<thirdpartydir>\packages\common\Nunit.Runners\tools\nunit-console.exe",
  IntegrationTestWorkingDir = @"<rootdir>\integrationtestworking",
  NugetWorkingDir = @"<rootdir>\nugetworking"
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
  RunSync(config["NugetExePath"], @"install .\src\Maddir.Nuget.Packages\common\packages.config -OutputDirectory .\thirdparty\packages\common -ExcludeVersion");
  RunSync(config["NugetExePath"], @"install .\src\Maddir.Nuget.Packages\net-4.5.1\packages.config -OutputDirectory .\thirdparty\packages\net-4.5.1 -ExcludeVersion");
}

void RunSync(string exePath, string args) {
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

Task<string> Run(string exePath, string args) {
  return Task<string>.Run(() => {
    var info = new ProcessStartInfo {
      FileName = exePath,
      Arguments = args,
      UseShellExecute = false,
      RedirectStandardError = true,
      RedirectStandardOutput = true,
      WindowStyle = ProcessWindowStyle.Hidden,
      CreateNoWindow = true
    };

    var result = "";

    using (var process = Process.Start(info)) {
      result = process.StandardOutput.ReadToEnd().Trim() + 
               Environment.NewLine + 
               Environment.NewLine + 
               process.StandardError.ReadToEnd().Trim();

      process.WaitForExit();

      if (process.ExitCode != 0)
        result = string.Format("{0}{1}{2} failed with code {3}", result, Environment.NewLine + Environment.NewLine, exePath, process.ExitCode);
    }

    return result;
  });
}

void BuildAll() {
  RunSync(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe", @".\src\Maddir.Build\Maddir.proj /ds /maxcpucount:6");
}

Task<string> RunTests(string name, string assembly, string framework) {
  return Task<string>.Run(() => {
    var result = new StringBuilder();
    result.AppendFormat("|y|----------- {0} Tests {1} -----------|{2}", name, framework, Environment.NewLine);
    result.AppendFormat("|y|-- {0}|{1}", assembly, Environment.NewLine);
    var task = Run(config["NunitConsoleExePath"], string.Format(@"{0} /nologo /noresult /framework:{1}", assembly, framework));
    task.Wait();
    result.AppendFormat("{0}{1}", task.Result, Environment.NewLine);
    result.AppendFormat("|y|------------------------------------------|{0}", Environment.NewLine);
    return result.ToString();
  });
}

Task<string> RunUnitTestsVS() {
  return RunTests("VS Unit", @".\src\Maddir.UnitTests\bin\debug\Maddir.UnitTests.dll", "net-4.5.1");
}

Task<string> RunIntegrationTestsVS() {
  return RunTests("VS Integration", @".\src\Maddir.IntegrationTests\bin\debug\Maddir.IntegrationTests.dll", "net-4.5.1");
}

Task<string> RunUnitTestsDebug() {
  return RunTests("Debug Unit", Path.Combine(config["DebugDir"], @"net-4.5.1\Maddir.UnitTests\Maddir.UnitTests.dll"), "net-4.5.1");
}

Task<string> RunIntegrationTestsDebug() {
  return RunTests("Debug Integration", Path.Combine(config["DebugDir"], @"net-4.5.1\Maddir.IntegrationTests\Maddir.IntegrationTests.dll"), "net-4.5.1");
}

void RunAndWaitAndEcho(params Func<Task<string>>[] runners) {
  var tasks = runners
    .Select(runner => runner.Invoke())
    .ToArray();

  Task<string>.WaitAll(tasks);

  Array.ForEach(tasks, task => conzole.WriteLine(task.Result));
}

void RunAllTests() {
  RunAndWaitAndEcho(RunUnitTestsVS,
                    RunUnitTestsDebug,
                    RunIntegrationTestsVS,
                    RunIntegrationTestsDebug);
}

void InitITWorkingDir() {
  if (!Directory.Exists(config["IntegrationTestWorkingDir"]))
    Directory.CreateDirectory(config["IntegrationTestWorkingDir"]);
}

void BuildNugetPackages() {
  TryDelete(config["NugetWorkingDir"]);
  
  var dir = Path.Combine(config["NugetWorkingDir"], @"Maddir.Core\lib\net451");
  Directory.CreateDirectory(dir);
  File.Copy(Path.Combine(config["DebugDir"], @"net-4.5.1\Maddir.Core\Maddir.Core.dll"),
            Path.Combine(dir, "Maddir.Core.dll")); 
  File.Copy(Path.Combine(config["SourceDir"], @"Maddir.Nuget.Specs\Maddir.Core.dll.nuspec"),
            Path.Combine(config["NugetWorkingDir"], @"Maddir.Core\Maddir.Core.dll.nuspec"));
  RunSync(config["NugetExePath"], @"pack .\nugetworking\Maddir.Core\Maddir.Core.dll.nuspec -OutputDirectory .\nugetworking\Maddir.Core");
}

void PushNugetPackages() {
  conzole.WriteLine("|dc|------------------------------------------|");
  conzole.WriteLine("|y|Push Nuget Packages!!|");
  conzole.WriteLine("|y|Are You Sure?|  Enter YES to Continue");
  if (conzole.ReadLine() == "YES") {
    RunSync(config["NugetExePath"], @"push .\nugetworking\Maddir.Core\Maddir.Core.1.0.0.1.nupkg");
  }
  else 
    conzole.WriteLine("|r|Operation Cancelled...|");
  conzole.WriteLine("|dc|------------------------------------------|");
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
            RunAndWaitAndEcho(RunUnitTestsVS);
            break;
          case ("run.unit.tests.debug"):
            RunAndWaitAndEcho(RunUnitTestsDebug);
            break;
          case ("run.integration.tests.vs"):
            InitITWorkingDir();
            RunAndWaitAndEcho(RunIntegrationTestsVS);
            break;
          case ("run.integration.tests.debug"):
            InitITWorkingDir();
            RunAndWaitAndEcho(RunIntegrationTestsDebug);
            break;
          case ("run.all.tests"):
            InitITWorkingDir();
            RunAllTests();
            break; 
          case ("cycle"):
            Clean();
            BuildAll();
            RunAllTests();
            break;
          case ("build.nuget.packages"):
            BuildNugetPackages();
            break;
          case ("push.nuget.packages"):
            PushNugetPackages();
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
