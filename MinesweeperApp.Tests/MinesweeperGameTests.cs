namespace MinesweeperApp.Tests;

public class MinesweeperGameTests
{
  private readonly int[,] grid =
  {
    { 0, 0, 0 },
    { 0, 0, 1 },
    { 0, 1, Generator.BOMB },
  };

  [Fact]
  public void ProcessCellCommand_Sweep()
  {
    MinesweeperGame sut = new(grid);
    var result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 0, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);

    result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);

    result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 2, 2));
    Assert.Equal(CellCommandStatus.GameOver, result.Status);
  }

  [Fact]
  public void ProcessCellCommand_PlaceMarker()
  {
    MinesweeperGame sut = new(grid);
    Assert.Equal(1, sut.BombCount);
    Assert.Equal(1, sut.MarkerCount);

    var result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 0, 0));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.Equal(0, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 0, 1));
    Assert.Equal(CellCommandStatus.NotEnoughMarker, result.Status);
    Assert.Equal(0, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 0, 0));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.Equal(1, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 0, 1));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.Equal(0, sut.MarkerCount);
  }

  [Fact]
  public void ProcessCellCommand_PlaceMarkerReturnsMarkerIgnored_OnCleanedCell()
  {
    MinesweeperGame sut = new(grid);
    Assert.Equal(1, sut.BombCount);
    Assert.Equal(1, sut.MarkerCount);

    var result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.True(sut.Board.Cells[5].IsCleaned);
    Assert.False(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(1, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 1, 2));
    Assert.Equal(CellCommandStatus.MarkerIgnored, result.Status);
    Assert.True(sut.Board.Cells[5].IsCleaned);
    Assert.False(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(1, sut.MarkerCount);
  }

  [Fact]
  public void ProcessCellCommand_SweepDoesNothing_OnMarkedCell()
  {
    MinesweeperGame sut = new(grid);
    Assert.Equal(1, sut.BombCount);
    Assert.Equal(1, sut.MarkerCount);

    var result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.False(sut.Board.Cells[5].IsCleaned);
    Assert.True(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(0, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.False(sut.Board.Cells[5].IsCleaned);
    Assert.True(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(0, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 1, 1));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.All(
      sut.Board.Cells.Where((_, index) => !(index == 5 || index == 8)),
      cell => Assert.True(cell.IsCleaned)
    );
    Assert.True(sut.Board.Cells[5].HasMarker);
    Assert.False(sut.Board.Cells[5].IsCleaned);
    Assert.False(sut.Board.Cells[8].IsCleaned);
  }

  [Fact]
  public void ProcessCellCommand_Sweep_OnUnMarkedCell()
  {
    MinesweeperGame sut = new(grid);
    Assert.Equal(1, sut.BombCount);
    Assert.Equal(1, sut.MarkerCount);

    var result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.False(sut.Board.Cells[5].IsCleaned);
    Assert.True(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(0, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.PlaceMarker, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.False(sut.Board.Cells[5].IsCleaned);
    Assert.False(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(1, sut.MarkerCount);

    result = sut.ProcessCellCommand(new Command(CommandType.Sweep, 1, 2));
    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.True(sut.Board.Cells[5].IsCleaned);
    Assert.False(sut.Board.Cells[5].HasMarker);
    Assert.Equal(1, sut.Board.Cells[5].Value);
    Assert.Equal(1, sut.MarkerCount);
  }

  [Fact]
  public void IsSolved_ChangesAfterAllSafeCellsAreSwept()
  {
    MinesweeperGame sut = new(new int[,] { { 0, Generator.BOMB } });

    Assert.False(sut.IsSolved);

    var result = sut.ProcessCellCommand(
      new Command(CommandType.Sweep, 0, 0)
    );

    Assert.Equal(CellCommandStatus.Accepted, result.Status);
    Assert.True(sut.IsSolved);
  }

  [Theory]
  [InlineData(CommandType.Invalid)]
  [InlineData(CommandType.Quit)]
  public void ProcessCellCommand_ReturnsInvalidCommand_ForUnsupportedType(
    CommandType commandType
  )
  {
    MinesweeperGame sut = new(grid);

    var result = sut.ProcessCellCommand(new Command(commandType));

    Assert.Equal(CellCommandStatus.InvalidCommand, result.Status);
  }

  [Theory]
  [InlineData(CommandType.Sweep)]
  [InlineData(CommandType.PlaceMarker)]
  public void ProcessCellCommand_ReturnsInvalidCommand_WhenCoordinatesAreMissing(
    CommandType commandType
  )
  {
    MinesweeperGame sut = new(grid);

    var result = sut.ProcessCellCommand(new Command(commandType));

    Assert.Equal(CellCommandStatus.InvalidCommand, result.Status);
  }

  [Theory]
  [InlineData(-1, 0)]
  [InlineData(0, 3)]
  public void ProcessCellCommand_ReturnsCellNotFound_ForOutOfBoundsCoordinates(
    int row,
    int column
  )
  {
    MinesweeperGame sut = new(grid);

    var result = sut.ProcessCellCommand(
      new Command(CommandType.Sweep, row, column)
    );

    Assert.Equal(CellCommandStatus.CellNotFound, result.Status);
  }

  [Theory]
  [InlineData(LabelUtility.MaxRowCount + 1, 2)]
  [InlineData(2, LabelUtility.MaxColumnCount + 1)]
  public void Constructor_RejectsDimensionsOutsideLabelLimits(
    int rowCount,
    int columnCount
  )
  {
    int[,] grid = new int[rowCount, columnCount];
    Assert.Throws<ArgumentException>(() => new MinesweeperGame(grid));
  }

  [Fact]
  public void Constructor_AcceptsMaximumLabelDimensions()
  {
    MinesweeperGame sut = new(new int[LabelUtility.MaxRowCount, LabelUtility.MaxColumnCount]);
    Assert.Equal(26, sut.Board.RowCount);
    Assert.Equal(35, sut.Board.ColumnCount);
  }
}