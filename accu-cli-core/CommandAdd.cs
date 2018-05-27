using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accu_CLI.commands;
using Accu_CLI.helpers;
using accucli.helpers;
using CommandLine;

namespace accucli.commands
{
  [Verb("add", HelpText = "Add 1..n files to AccuRev SCM")]
  class AddOption : CommonOption
  {
    [Option('f', "files", Required = true, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; set; }

    [Option('m', "comment", Required = false, HelpText = "Comment for the operation.")]
    public string Comment { get; set; }
  }

  class CommandAdd : ICommand
  {
    private AddOption opts;

    public CommandAdd(AddOption opts)
    {
      this.opts = opts;
    }

    public int Do()
    {
      string files = "";
      foreach (var f in opts.InputFiles)
      {
        files += f + " ";
      }
      files = files.TrimEnd();
      var comment = string.IsNullOrEmpty(opts.Comment) ? "" : $"-c {opts.Comment}";
      return ShellExec.SimpleExec("accurev", $"add {files} -R {comment}");
    }
  }
}
