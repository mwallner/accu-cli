using AccuCLI.helpers;

namespace AccuCLI.commands
{
  abstract class ICommand
  {
    public abstract int Do();

    public int Invoke()
    {
      if (AccuRev.IsCurrentDirectoryAccurevWS())
      {
        return Do();
      }
      else
      {
        ConsoleOutput.Section("ERROR", "not in a workspace?", System.ConsoleColor.Red);
        return -1;
      }
    }
  }
}
