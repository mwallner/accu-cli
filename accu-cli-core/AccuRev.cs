using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AccuCLI.helpers
{
  class AccuRev
  {
    public static bool IsCurrentDirectoryAccurevWS()
    {
      var accurevInfoOutput = ShellExec.SimpleExecOutput("accurev", "info");
      var workspaceRefPattern = @"Workspace/ref:\s*(.+)";
      Regex r = new Regex(workspaceRefPattern);
      Match m = r.Match(accurevInfoOutput);
      return m.Success;
    }

    public static string GetCurrentDirectoryAccurevWS()
    {
      var accurevInfoOutput = ShellExec.SimpleExecOutput("accurev", "info");
      var workspaceRefPattern = "Workspace/ref:\\s*(([^\\s-])+)";
      Regex r = new Regex(workspaceRefPattern);
      Match m = r.Match(accurevInfoOutput);
      return m.Groups[1].Value;
    }

    private static List<string> AccuRevStatCommandToLocationList(string param)
    {
      XDocument xmlDoc = XDocument.Parse(ShellExec.SimpleExecOutput("accurev", $"stat -{param} -fx"));
      return xmlDoc.Descendants()
                      .Attributes("location")
                      .Select(x => x.Value)
                      .ToList();
    }

    internal static List<string> GetKeptFiles()
    {
      return AccuRevStatCommandToLocationList("k");
    }

    internal static List<string> GetModifiedFiles()
    {
      return AccuRevStatCommandToLocationList("m");
    }

    internal static List<string> GetExternalFiles()
    {
      return AccuRevStatCommandToLocationList("x");
    }
    internal static List<string> GetMissingFiles()
    {
      return AccuRevStatCommandToLocationList("M");
    }

    internal static List<string> GetDefunctFiles()
    {
      return AccuRevStatCommandToLocationList("D");
    }


    internal static void PrintHistory(int numEntires, string streamRef)
    {
      XDocument xmlDoc = XDocument.Parse(ShellExec.SimpleExecOutput("accurev", $"hist -a -fvx -t now.{numEntires} -s {streamRef}"));
      var list = xmlDoc.Descendants()
            .Elements("transaction")
            .Select(x => new
            {
              Id = x.Attribute("id").Value,
              Type = x.Attribute("type").Value,
              User = x.Attribute("user").Value,
              Time = Converter.UnixTimeStampToDateTime(Convert.ToDouble(x.Attribute("time").Value)),
              StreamName = x.Element("stream")?.Attribute("name")?.Value,
              StreamType = x.Element("stream")?.Attribute("type")?.Value,
              Comment = x.Element("comment")?.Value,
              //TODO add file status (change, defunct, ..?)
              Files = x.Elements("version")?.Select(f => f.Attribute("path").Value)
            })
            .ToList();

      foreach (var e in list)
      {
        Console.Write("* ");
        ConsoleOutput.PrintColor(e.Id, -1);
        Console.Write(" : ");
        ConsoleOutput.PrintColor(e.Type, 2);
        if (!string.IsNullOrEmpty(e.Comment))
        {
          Console.Write($" - {e.Comment}");
        }
        else if (!string.IsNullOrEmpty(e.StreamType))
        {
          ConsoleOutput.PrintColor($" [{e.StreamType}]", 4);
          if (!string.IsNullOrEmpty(e.StreamName))
          {
            Console.Write($" | {e.StreamName}");
          }
        }
        Console.Write(" ");
        ConsoleOutput.PrintColor($"({e.Time})", 7);
        ConsoleOutput.PrintColor($" <{e.User}>", 3);
        Console.WriteLine("");
        foreach (var f in e.Files)
        {
          ConsoleOutput.PrintColor("\tfile: ", 4);
          ConsoleOutput.PrintColor($"{f}", 5);
          Console.WriteLine("");
        }
        Console.WriteLine("");
      }
    }

    internal static string Diff(string file, string workspaceOrStreamRef)
    {
      return ShellExec.SimpleExecOutput("accurev", $"diff {file}");
    }
  }
}
