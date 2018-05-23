using System;
using accucli.commands;
using accucli.helpers;
using CommandLine;

namespace Accu_CLI
{
  class Program
  {
    static int Main(string[] args)
    {

      return CommandLine.Parser.Default.ParseArguments<StatusOption, AddOption, CommitOption, PushOption, LogOption>(args).MapResult(
        (StatusOption opts) => new CommandStatus(opts).Do(),
        (AddOption opts) => new CommandAdd(opts).Do(),
        (CommitOption opts) => new CommandCommit(opts).Do(),
        (PushOption opts) => new CommandPush(opts).Do(),
        (LogOption opts) => new CommandLog(opts).Do(),
        errs => 1);
    }

  }
}
