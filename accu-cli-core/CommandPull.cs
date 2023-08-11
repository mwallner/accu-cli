using AccuCLI.helpers;
using CommandLine;

namespace AccuCLI.commands
{
  [Verb("pull", HelpText = "pull remote changes from accurev scm (performs update)")]
  class PullOption : CommonOption
  {
  }

  class CommandPull : ICommand
  {
    private PullOption opts;

    public CommandPull(PullOption opts)
    {
      this.opts = opts;
    }

    public override int Do()
    {
      return ShellExec.SimpleExec("accurev", $"update");
    }
  }
}
