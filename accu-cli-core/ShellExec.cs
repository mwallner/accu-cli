using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu_CLI.helpers
{
  class ShellExec
  {

    internal static string SimpleExecOutput(string filename, string arguments)
    {
      using (Process p = new Process())
      {
        string output = "";
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        //p.StartInfo.RedirectStandardError = true;
        p.StartInfo.FileName = filename;
        p.StartInfo.Arguments = arguments;

        p.OutputDataReceived += (sender, args) => { output += args.Data; };
        p.Start();
        p.BeginOutputReadLine();
        p.WaitForExit();

        return output;
      }
    }

    internal static int SimpleExec(string filename, string arguments)
    {
      ConsoleOutput.PrintColor($"> {filename} {arguments}\n", 1);
      using (Process p = new Process())
      {
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        //p.StartInfo.RedirectStandardError = true;
        p.StartInfo.FileName = filename;
        p.StartInfo.Arguments = arguments;

        p.OutputDataReceived += (sender, args) => { ConsoleOutput.PrintColor($"{args.Data}\n", 1); };
        p.Start();
        p.BeginOutputReadLine();
        p.WaitForExit();

        return p.ExitCode;
      }
    }

  }
}
