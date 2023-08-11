using AccuCLI.commands;
using CommandLine;

namespace AccuCLI
{
  class Program
  {
    static int Main(string[] args) => Parser.Default.ParseArguments<
        StatusOption, 
        AddOption, 
        CommitOption, 
        PushOption, 
        PullOption, 
        LogOption, 
        DiffOption>(args)
      .MapResult(
      (StatusOption opts) => new CommandStatus(opts).Invoke(),
      (AddOption opts) => new CommandAdd(opts).Invoke(),
      (CommitOption opts) => new CommandCommit(opts).Invoke(),
      (PushOption opts) => new CommandPush(opts).Invoke(),
      (PullOption opts) => new CommandPull(opts).Invoke(),
      (LogOption opts) => new CommandLog(opts).Invoke(),
      (DiffOption opts) => new CommandDiff(opts).Invoke(),
      errs => 1);
  }
}
