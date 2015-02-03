using System;
using System.Collections.Generic;
using Maddir.Core.MarkupGeneration;
using Maddir.Core.TreeGeneration;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Maddir.Samples {
  internal class Program {
    private static int Main(string[] args) {
      try {
        Execute(args);
        return 0;
      } catch (Exception e) {
        Console.WriteLine(e);
      }
      return 1;
    }

    private static void Execute(params string[] args) {
      switch (args[0]) {
        case "browse":
          Browse();
          break;
        case "build":
          Build();
          break;
        default:
          throw new Exception(string.Format("Unknown command: {0}", args[0]));
      }
    }

    private static void Build() {
      Console.WriteLine();
      Console.WriteLine("Enter Markup: ");
      var lines = new List<string>();
      while (true) {
        var line = Console.ReadLine();
        if (line == "")
          break;
        lines.Add(line);
      }

      var markup = string.Join(Environment.NewLine, lines.ToArray());
      var layout = new Parser().Parse(markup);

      Console.WriteLine();
      Console.Write("Enter Directory To Build Into: ");
      var dir = Console.ReadLine();

      new Engine(new DotNetDirectory(), new DotNetFile()).Apply(dir, layout);
    }

    private static void Browse() {
      Console.WriteLine();
      Console.Write("Enter Directory To Browse: ");
      var dir = Console.ReadLine();

      Console.WriteLine("Browsing {0}", dir);
      var layout = new DirectoryBrowser(Snarfzer.NewScanner()).Browse(dir);
      var markup = new MarkupGenerationEngine(new MarkupBuilder()).Apply(layout);

      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine(markup);
    }
  }
}