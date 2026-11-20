public class BoardRenderer
{
  private readonly TextWriter _output;
  const string FMT_RED_MESSAGE = "\u001b[31m{0}\u001b[0m";

  public BoardRenderer(TextWriter? output = null)
  {
    _output = output ?? Console.Out;
  }

  public void Render(Board board, bool revealBomb = false)
  {
    _output.WriteLine($"Total Bomb: {board.BombCount}");
    WriteRow(' ', Enumerable.Range(0, board.ColumnCount));
    _output.WriteLine();

    var rowHeadings = board.Cells
      .OrderBy(cell => cell.RowIndex)
      .Select(cell => LabelUtility.GetRowLabel(cell.RowIndex))
      .Distinct();

    foreach (char heading in rowHeadings)
    {
      WriteLabel(heading);
      foreach (Cell cell in board.Cells
        .Where(cell => LabelUtility.GetRowLabel(cell.RowIndex) == heading)
        .OrderBy(cell => cell.ColumnIndex))
      {
        WriteCellValue(cell, revealBomb);
      }

      _output.WriteLine();
    }
  }

  private void WriteRow(char heading, IEnumerable<int> values)
  {
    WriteLabel(heading);
    foreach (int value in values)
    {
      WriteLabel(LabelUtility.GetColumnLabel(value));
    }
  }

  private void WriteLabel(char value) => _output.Write($" {value} ");

  private void WriteCellValue(Cell cell, bool revealBomb)
  {
    if (revealBomb)
    {
      if (cell.IsBomb)
      {
        _output.Write(string.Format(FMT_RED_MESSAGE, " B "));
      }
      else if (cell.HasMarker)
      {
        _output.Write(string.Format(FMT_RED_MESSAGE, " X "));
      }
      else
      {
        WriteNonBombValue(cell);
      }
    }
    else if (cell.HasMarker)
    {
      _output.Write(string.Format(FMT_RED_MESSAGE, " ? "));
    }
    else
    {
      WriteNonBombValue(cell);
    }
  }

  private void WriteNonBombValue(Cell cell)
  {
    if (cell.IsCleaned)
    {
      if (cell.HasValue)
      {
        _output.Write($" {cell.Value} ");
      }
      else
      {
        _output.Write("▓▓▓");
      }
    }
    else
    {
      _output.Write("░░░");
    }
  }
}