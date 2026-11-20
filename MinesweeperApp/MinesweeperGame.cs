public enum CellCommandStatus
{
  Accepted,
  GameOver,
  CellNotFound,
  MarkerIgnored,
  NotEnoughMarker,
  InvalidCommand
};

public sealed record CellCommandResult(CellCommandStatus Status, Cell? Cell = null);

public class MinesweeperGame
{
  public int MarkerCount { get; private set; }
  public Board Board { get; private set; }

  public MinesweeperGame(int[,] grid)
  {
    Board = new(MatrixExtensions.ToEnumerableCells(grid));
    MarkerCount = Board.BombCount;
  }

  public int BombCount => Board.BombCount;

  public bool IsSolved => Board.Cells
    .Where(x => !x.IsBomb)
    .All(x => x.IsCleaned);

  public CellCommandResult ProcessCellCommand(Command command)
  {
    if (
      command.CommandType is not (CommandType.Sweep or CommandType.PlaceMarker)
      || command.RowIndex is not int rowIndex
      || command.ColIndex is not int columnIndex)
    {
      return new CellCommandResult(CellCommandStatus.InvalidCommand);
    }

    Cell? cell = Board.GetCell(rowIndex, columnIndex);
    if (cell is null)
    {
      return new CellCommandResult(CellCommandStatus.CellNotFound);
    }

    if (command.CommandType == CommandType.Sweep)
    {
      return Board.SweepMine(cell) switch
      {
        SweepStatus.Bomb => new CellCommandResult(CellCommandStatus.GameOver),
        _ => new CellCommandResult(CellCommandStatus.Accepted),
      };
    }

    if (command.CommandType == CommandType.PlaceMarker)
    {
      if (cell.IsCleaned)
      {
        return new CellCommandResult(CellCommandStatus.MarkerIgnored);
      }

      MarkerCount += cell.HasMarker ? 1 : -1;
      if (MarkerCount < 0)
      {
        MarkerCount = 0;
        return new CellCommandResult(CellCommandStatus.NotEnoughMarker);
      }
      cell.ToggleMarker();
    }

    return new CellCommandResult(CellCommandStatus.Accepted, cell);
  }
}