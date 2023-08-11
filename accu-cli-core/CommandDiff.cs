using System.Collections.Generic;
using System.ComponentModel;
using AccuCLI.helpers;
using CommandLine;

namespace AccuCLI.commands
{ 

  [Verb("diff", HelpText = "diff file(s) against backed")]
  class DiffOption : CommonOption
  {
    [Value(0, MetaName = "file", HelpText = "file to diff")]
    public string file { get; set; }
  }

  class CommandDiff : ICommand
  {

    private DiffOption opts;

    public CommandDiff(DiffOption opts)
    {
      this.opts = opts;
    }

    public override int Do()
    {
      var workspaceOrStreamRef = AccuRev.GetCurrentDirectoryAccurevWS();
      List<string> files; 
      if (string.IsNullOrEmpty(opts.file))
      { files = AccuRev.GetModifiedFiles();
      }
      else
      {
        files = new List<string> { opts.file };
      }
      files.ForEach(file =>
      {
        var fileDiff = AccuRev.Diff(file, workspaceOrStreamRef);
        ConsoleOutput.Section(file, fileDiff, System.ConsoleColor.Magenta);
      });
      return 0;
    }
  }
}
