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
  [Verb("push", HelpText = "promote 1-n files to accurev scm (performs promote)")]
  class PushOption : CommonOption
  {
    [Option('f', "files", Required = false, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; set; }

    [Option('a', "all", Required = false, HelpText = "Add changes from all known files")]
    public bool AddAll { get; set; }

    [Option('m', "comment", Required = false, HelpText = "Comment for the operation.")]
    public string Comment { get; set; }
  }

  class CommandPush : ICommand
  {
    private PushOption opts;

    public CommandPush(PushOption opts)
    {
      this.opts = opts;
    }

    public int Do()
    {
      string files = "";
      if (opts.AddAll)
      {
        files = "-d";
      }
      else
      {
        foreach (var f in opts.InputFiles)
        {
          files += f + " ";
        }
        files = files.TrimEnd();
      }
      var comment = string.IsNullOrEmpty(opts.Comment) ? "" : $"-c \"{opts.Comment}\"";
      return ShellExec.SimpleExec("accurev", $"promote {files} {comment}");
    }
  }
}
