using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

Console.SetOut(new DomTextWriter());
Console.WriteLine("National Geographic: \u001b[1m\u001b[34mBeyond Earth\u001b[0m");
await Program.ReadLineAsync();

partial class Program
{
  [JSImport("dom.appendOutput", "main.js")]
  internal static partial void AppendOutput(string text);

  [JSImport("dom.readLineAsync", "main.js")]
  internal static partial Task<string?> ReadLineAsync();
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