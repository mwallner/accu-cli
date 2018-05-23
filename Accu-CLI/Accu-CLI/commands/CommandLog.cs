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

    public int Do()
    {
      AccuRev.PrintHistory(opts.historyDepth);
      return 0;
    }
  }
}
