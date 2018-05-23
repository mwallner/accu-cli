using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Accu_CLI.helpers;

namespace accucli.helpers
{
  class AccuRev
  {
    public static bool IsCurrentDirectoryAccurevWS()
    {
      Console.WriteLine("you are not in a directory known by AccuRev.");
      //TODO mw impl
      /*
        Usage
          accurev info [ -v ]
        Description
          At its most basic level, the info command displays the values for the following characteristics of the AccuRev user environment:
            Principal
            Host
            Server name
            Port
            DB Encoding
            ACCUREV_BIN
            Client time
            Server time
          When executed from within a workspace directory, info also displays the following additional fields:
            Depot
            Workspace/ref
            Basis
            Top <<- ??
       */
      return false;
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


    internal static void PrintHistory(int numEntires)
    {
      XDocument xmlDoc = XDocument.Parse(ShellExec.SimpleExecOutput("accurev", $"hist -a -fvx -t now.{numEntires}"));
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

  }
}
