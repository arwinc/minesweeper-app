namespace Minesweeper.Tests;

public class GeneratorTests
{
  private readonly Generator _sut = new(new Random(42));

  [Theory]
  [InlineData(5, 5, 2)]
  [InlineData(2, 3, 2)]
  [InlineData(3, 2, 1)]
  public void Generate_ReturnsCorrectDimensionsAndBombCount(
    int rowCount,
    int columnCount,
    int totalBomb
  )
  {
    var grid = _sut.Generate(rowCount, columnCount, totalBomb);

    Assert.Equal(rowCount, grid.GetLength(0));
    Assert.Equal(columnCount, grid.GetLength(1));
    Assert.Equal(rowCount * columnCount, grid.Cast<int>().Count());
    Assert.Equal(totalBomb, grid.Cast<int>().Count(value => value == Generator.BOMB));
  }

  [Theory]
  [InlineData(0, 0, 100)]
  [InlineData(1, 1, 100)]
  public void Generate_ReturnsMinimumDimensionsAndBombCount(
    int rowCount,
    int columnCount,
    int totalBomb
  )
  {
    var grid = _sut.Generate(rowCount, columnCount, totalBomb);

    Assert.Equal(2, grid.GetLength(0));
    Assert.Equal(2, grid.GetLength(1));
    Assert.Equal(3, grid.Cast<int>().Count(value => value == Generator.BOMB));
  }

  [Fact]
  public void Generate_ReturnsCorrectMaximumBombCount()
  {
    var grid = _sut.Generate(4, 5, 100);

    Assert.Equal(4, grid.GetLength(0));
    Assert.Equal(5, grid.GetLength(1));
    Assert.Equal(19, grid.Cast<int>().Count(value => value == Generator.BOMB));
  }

  [Fact]
  public void Generate_ReturnsMaximumDimensionsAndMinimalBombCount()
  {
    var grid = _sut.Generate(LabelUtility.MaxRowCount + 1, LabelUtility.MaxColumnCount + 1, 0);

    Assert.Equal(LabelUtility.MaxRowCount, grid.GetLength(0));
    Assert.Equal(LabelUtility.MaxColumnCount, grid.GetLength(1));
    Assert.Equal(1, grid.Cast<int>().Count(value => value == Generator.BOMB));
  }

  public static TheoryData<int[,], int?[]> BombCountCases => new()
  {
    { new int[,] { {9, 0}, {0, 0} }, new int?[] { null, 1, 1, 1 } },
    { new int[,] { {0, 9}, {0, 0} }, new int?[] { 1, null, 1, 1 } },
    { new int[,] { {0, 0}, {9, 0} }, new int?[] { 1, 1, null, 1 } },
    { new int[,] { {0, 0}, {0, 9} }, new int?[] { 1, 1, 1, null } },
    { new int[,] { {9, 0}, {0, 9} }, new int?[] { null, 2, 2, null } },
    { new int[,] { {0, 9}, {9, 0} }, new int?[] { 2, null, null, 2 } },
    {
      new int[,] { {0, 0, 0}, {0, 9, 0}, {0, 0, 0} },
      new int?[] { 1, 1, 1, 1, null, 1, 1, 1, 1 }
    },
    // Safe cell has no neighbouring bombs
    {
      new int[,] { {9, 0, 0}, {0, 0, 0}, {0, 0, 0} },
      new int?[] { null, 1, 0, 1, 1, 0, 0, 0, 0 }
    },
  };

  [Theory]
  [MemberData(nameof(BombCountCases))]
  public void GetBombCount_ReturnsCorrectly(int[,] grid, int?[] expected)
  {
    List<int?> result = [];
    for (int i = 0; i < grid.GetLength(0); i++)
    {
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        result.Add(_sut.GetBombCount(grid, i, j));
      }
    }
    Assert.Equal(expected, result);
  }
}