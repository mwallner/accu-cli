using System;
using System.Collections.Generic;
using Accu_CLI.commands;
using Accu_CLI.helpers;
using accucli.commands;
using accucli.helpers;
using CommandLine;

namespace Accu_CLI
{
  [Verb("status", HelpText = "display status of files in current workspace")]
  class StatusOption : CommonOption
  {
    [Option('n', "noext", Default = false, HelpText = "do NOT show untracked (external) files)")]
    public bool hideExternals { get; set; }
  }

  class CommandStatus : ICommand
  {
    private StatusOption opts;

    public CommandStatus(StatusOption opts)
    {
      this.opts = opts;
    }

    private delegate List<String> AccuRevFileList();

    private void PrettyPrintListForStatCommandIffHasResult(string title, char indicator, AccuRevFileList cmd, int colorOffset)
    {
      var l = cmd();
      if (l.Count > 0)
      {
        ConsoleOutput.Section(title, ConsoleOutput.PrettyList(l, $"({indicator})"), Console.ForegroundColor + colorOffset);
      }
    }

    public int Do()
    {
      PrettyPrintListForStatCommandIffHasResult("modified", 'm', AccuRev.GetModifiedFiles, -1);
      PrettyPrintListForStatCommandIffHasResult("kept", 'k', AccuRev.GetKeptFiles, 2);
      PrettyPrintListForStatCommandIffHasResult("defunct", 'D', AccuRev.GetDefunctFiles, 3);
      PrettyPrintListForStatCommandIffHasResult("missing", ' ', AccuRev.GetMissingFiles, 4);
      if (!opts.hideExternals)
        PrettyPrintListForStatCommandIffHasResult("untracked", '?', AccuRev.GetExternalFiles, 5);

      return 0;
    }
  }
}
