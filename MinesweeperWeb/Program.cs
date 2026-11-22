extern alias MinesweeperApp;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

Console.SetOut(new DomTextWriter());
MinesweeperApp::ConsoleReader.ReadLineHandler = () => Program.ReadLineAsync();

var entryPoint = typeof(MinesweeperApp::Program).GetMethod(
  "<Main>$",
  BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
);

var result = entryPoint?.Invoke(null, [Array.Empty<string>()]);
try
{
  if (result is Task task)
  {
    await task;
  }
  Console.WriteLine("\n[Session ended]");
}
catch (Exception ex)
{
  Console.WriteLine($"\n\x1b[31m[Unhandled Error] {ex.GetType().Name}: {ex.Message}\x1b[0m");
}
finally
{
  Program.onProgramExited();
}

partial class Program
{
  [JSImport("dom.appendOutput", "main.js")]
  internal static partial void AppendOutput(string text);

  [JSImport("dom.readLineAsync", "main.js")]
  internal static partial Task<string?> ReadLineAsync();

  [JSImport("dom.onExit", "main.js")]
  internal static partial void onProgramExited();
}

internal class DomTextWriter : TextWriter
{
  public override Encoding Encoding => Encoding.UTF8;
  public override void Write(char value) => Program.AppendOutput(value.ToString());
  public override void Write(string? value)
  {
    if (value is not null)
    {
      Program.AppendOutput(value);
    }
  }
  public override void WriteLine() => Program.AppendOutput("\n");
  public override void WriteLine(string? value) => Program.AppendOutput((value ?? string.Empty) + "\n");
}