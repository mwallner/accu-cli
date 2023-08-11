using System.Collections.Generic;
using AccuCLI.helpers;
using CommandLine;

namespace AccuCLI.commands
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

    public override int Do()
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
