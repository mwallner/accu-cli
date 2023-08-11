using System;
using System.Collections.Generic;

namespace AccuCLI.helpers
{
  class ConsoleOutput
  {

    const string delim = " **** ";

    public static void Section(string name, string content, ConsoleColor color)
    {
      ConsoleColor oldColor = Console.ForegroundColor;
      Console.WriteLine($"{delim} {name} {delim}");
      Console.ForegroundColor = color;
      Console.WriteLine(content);
      Console.ForegroundColor = oldColor;
    }

    private static string PadThis(string s, int count)
    {
      string res = "";
      for (int i = 0; i < count; ++i)
      {
        res += s;
      }
      return res;
    }

    internal static string PrettyList(List<string> list, string prefix)
    {
      string res = "";
      foreach (var f in list)
      {
        res += " " + prefix + " " + f + "\n";
      }
      return res;
    }

    internal static void PrintColor(string str, int colorIncrement)
    {
      ConsoleColor oldColor = Console.ForegroundColor;
      Console.ForegroundColor = Console.ForegroundColor + colorIncrement;
      Console.Write(str);
      Console.ForegroundColor = oldColor;
    }
  }
}
