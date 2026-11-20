using System.Text.RegularExpressions;

public enum CommandType
{
  Invalid,
  Quit,
  Sweep,
  PlaceMarker,
}

public sealed record Command(
  CommandType CommandType,
  int? RowIndex = null,
  int? ColIndex = null
);

public class CommandParser
{
  public int BoardSize { get; set; } = 5;

  public Command Parse(string? input)
  {
    input ??= string.Empty;
    if (string.Equals("quit", input, StringComparison.Ordinal))
    {
      return new Command(CommandType.Quit);
    }

    string cellPattern = $@"^([A-Z])([1-9A-Z])\s+(sweep|marker)$";
    Match match = Regex.Match(input, cellPattern);
    if (!match.Success)
    {
      return new Command(CommandType.Invalid);
    }

    int rowIndex = match.Groups[1].Value[0] - 'A';
    int colIndex = LabelUtility.MapToColumnIndex(match.Groups[2].Value[0]);
    string action = match.Groups[3].Value;

    if (string.Equals("sweep", action, StringComparison.Ordinal))
    {
      return new Command(CommandType.Sweep, rowIndex, colIndex);
    }
    return new Command(CommandType.PlaceMarker, rowIndex, colIndex);
  }
}