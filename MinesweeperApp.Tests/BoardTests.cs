namespace Minesweeper.Tests;

public class BoardTests
{
  [Theory]
  [InlineData(0, 0, new int[] { 2, 4, 5 })]
  [InlineData(1, 1, new int[] { 1, 2, 3, 4, 6, 7, 8, 9 })]
  [InlineData(2, 2, new int[] { 5, 6, 8 })]
  public void GetNeighbours_ReturnsCorrectly(
    int row,
    int col,
    int[] expected
  )
  {
    int[,] grid = {
      { 1, 2, 3 },
      { 4, 5, 6 },
      { 7, 8, 9 }
    };
    Board sut = new(grid.ToEnumerableCells());

    var neighbours = sut.GetNeighbours(sut.GetCell(row, col));
    Assert.Equivalent(expected, neighbours.Select(n => n.Value), strict: true);
  }

  [Theory]
  [InlineData(0, 0, new int[] { 2, 4, 5 })]
  [InlineData(0, 1, new int[] { 1, 3, 4, 5, 6 })]
  [InlineData(1, 2, new int[] { 2, 3, 5 })]
  public void GetNeighbours_ReturnsCorrectly_ForRectangularBoard(
    int row,
    int col,
    int[] expected
  )
  {
    int[,] grid = {
      { 1, 2, 3 },
      { 4, 5, 6 },
    };

    Board sut = new(grid.ToEnumerableCells());

    Assert.Equal(2, sut.RowCount);
    Assert.Equal(3, sut.ColumnCount);

    var neighbours = sut.GetNeighbours(sut.GetCell(row, col));
    Assert.Equivalent(expected, neighbours.Select(n => n.Value), strict: true);
  }

  [Fact]
  public void EmptyBoard_ReportsZeroDimensionsAndHasNoCellOrNeighbours()
  {
    Board sut = new(Enumerable.Empty<Cell>());
    Assert.Equal(0, sut.RowCount);
    Assert.Equal(0, sut.ColumnCount);
    Assert.Null(sut.GetCell(0, 0));
    Assert.Empty(sut.GetNeighbours(null));
  }

  [Fact]
  public void SweepMine_ReturnsAllCleanedCells_GivenNoBomb()
  {
    int[,] grid = {
      { 0, 0, 0 },
      { 0, 0, 0 },
      { 0, 0, 0 }
    };
    Board sut = new(grid.ToEnumerableCells());

    Assert.All(sut.Cells, cell => Assert.False(cell.IsCleaned));

    var cell = sut.GetCell(1, 1)!;
    Assert.Equal(SweepStatus.Swept, sut.SweepMine(cell));
    Assert.All(sut.Cells, cell => Assert.True(cell.IsCleaned));
  }

  [Fact]
  public void SweepMine_ReturnsAllCleanedCells_ExceptBomb()
  {
    int[,] grid = {
      { 0, 0, 0 },
      { 0, 0, 1 },
      { 0, 1, Generator.BOMB }
    };
    Board sut = new(grid.ToEnumerableCells());

    Assert.All(sut.Cells, cell => Assert.False(cell.IsCleaned));

    var cell = sut.Cells[0];
    Assert.False(cell.HasValue);

    Assert.Equal(SweepStatus.Swept, sut.SweepMine(cell));
    Assert.All(
      sut.Cells.Where(cell => !cell.IsBomb),
      cell => Assert.True(cell.IsCleaned)
    );
    Assert.False(sut.Cells[sut.Cells.Count - 1].IsCleaned);
  }

  [Fact]
  public void SweepMine_ResweepingCleanedCellLeavesBoardUnchanged()
  {
    int[,] grid = {
      { 0, 0, 0 },
      { 0, 0, 1 },
      { 0, 1, Generator.BOMB }
    };
    Board sut = new(grid.ToEnumerableCells());

    Assert.All(sut.Cells, cell => Assert.False(cell.IsCleaned));

    var cell = sut.Cells[5];
    Assert.True(cell.HasValue);
    Assert.False(cell.IsCleaned);

    Assert.Equal(SweepStatus.Swept, sut.SweepMine(cell));
    Assert.True(cell.IsCleaned);
    Assert.All(
      sut.Cells.Where(x => x != cell),
      cell => Assert.False(cell.IsCleaned)
    );

    Assert.Equal(SweepStatus.Swept, sut.SweepMine(cell));
    Assert.True(cell.IsCleaned);
    Assert.All(
      sut.Cells.Where(x => x != cell),
      cell => Assert.False(cell.IsCleaned)
    );
  }

  [Fact]
  public void SweepMine_ReturnsCorrectStatus_ForMarkedAndBombCells()
  {
    int[,] grid = {
      { 0, 0, 1 },
      { 0, 1, Generator.BOMB }
    };
    Board sut = new(grid.ToEnumerableCells());
    sut.Cells[0].ToggleMarker();
    Assert.Equal(SweepStatus.Marker, sut.SweepMine(sut.Cells[0]));
    Assert.False(sut.Cells[0].IsCleaned);

    Assert.Equal(SweepStatus.Bomb, sut.SweepMine(sut.Cells[sut.Cells.Count - 1]));
  }
}