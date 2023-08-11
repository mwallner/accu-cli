using AccuCLI.helpers;
using CommandLine;

namespace AccuCLI.commands
{

  [Verb("log", HelpText = "pretty print keep / promote history")]
  class LogOption : CommonOption
  {
    [Option('n', "count", Default = 10, HelpText = "show #n entries")]
    public int historyDepth { get; set; }
  }


  class CommandLog : ICommand
  {
    private LogOption opts;

    public CommandLog(LogOption opts)
    {
      this.opts = opts;
    }

    public override int Do()
    {
      var workspaceOrStreamRef = AccuRev.GetCurrentDirectoryAccurevWS();
      AccuRev.PrintHistory(opts.historyDepth, workspaceOrStreamRef);
      return 0;
    }
  }
}
