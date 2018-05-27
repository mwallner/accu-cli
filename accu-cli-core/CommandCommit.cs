using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accu_CLI.commands;
using Accu_CLI.helpers;
using CommandLine;

namespace accucli.commands
{

  [Verb("commit", HelpText = "Keep files")]
  class CommitOption : CommonOption
  {
    [Option('f', "files", Required = false, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; set; }

    [Option('a', "all", Required = false, HelpText = "Add changes from all known files")]
    public bool AddAll { get; set; }

    [Option('m', "comment", Required = false, HelpText = "Comment for the operation.")]
    public string Comment { get; set; }
  }

  class CommandCommit : ICommand
  {
    private CommitOption opts;

    public CommandCommit(CommitOption opts)
    {
      this.opts = opts;
    }

    public int Do()
    {
      string files = "";
      if (opts.AddAll)
      {
        files = "-m";
      }
      else
      {
        foreach (var f in opts.InputFiles)
        {
          files += f + " ";
        }
        files += "-R";
      }
      var comment = string.IsNullOrEmpty(opts.Comment) ? "" : $"-c {opts.Comment}";
      return ShellExec.SimpleExec("accurev", $"keep {files} {comment}");
    }
  }
}
