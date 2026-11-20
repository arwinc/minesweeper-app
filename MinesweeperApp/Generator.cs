public interface IGenerator
{
  int[,] Generate(int rowCount, int columnCount, int totalBomb);
}

public class Generator : IGenerator
{
  private readonly Random _random;
  public const int BOMB = 9;

  public Generator(Random? random = null)
  {
    _random = random ?? Random.Shared;
  }

  public int[,] Generate(int rowCount, int columnCount, int totalBomb)
  {
    rowCount = Math.Clamp(rowCount, 2, LabelUtility.MaxRowCount);
    columnCount = Math.Clamp(columnCount, 2, LabelUtility.MaxColumnCount);

    int size = rowCount * columnCount;
    totalBomb = Math.Clamp(totalBomb, 1, size - 1);

    var grid = NewGrid(rowCount, columnCount, totalBomb);

    for (int i = 0; i < rowCount; i++)
    {
      for (int j = 0; j < columnCount; j++)
      {
        var cellValue = GetBombCount(grid, i, j);
        if (cellValue.HasValue)
        {
          grid[i, j] = cellValue.Value;
        }
      }
    }
    return grid;
  }

  public int? GetBombCount(int[,] grid, int row, int col)
  {
    if (IsBomb(grid[row, col])) return null;

    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);
    int rowStart = Math.Max(0, row - 1);
    int rowEnd = Math.Min(rowCount - 1, row + 1);
    int colStart = Math.Max(0, col - 1);
    int colEnd = Math.Min(colCount - 1, col + 1);
    int bombCount = 0;

    for (int i = rowStart; i <= rowEnd; i++)
    {
      for (int j = colStart; j <= colEnd; j++)
      {
        if (IsBomb(grid[i, j]))
        {
          bombCount++;
        }
      }
    }

    return bombCount;
  }

  private int[,] NewGrid(int rowCount, int columnCount, int totalBomb)
  {
    int size = rowCount * columnCount;
    var bombPositions = GetRandomBombs(size, totalBomb);

    int[,] grid = new int[rowCount, columnCount];

    for (int i = 0; i < rowCount; i++)
    {
      for (int j = 0; j < columnCount; j++)
      {
        int index = (i * columnCount) + j;
        if (bombPositions.Contains(index))
        {
          grid[i, j] = BOMB;
        }
      }
    }

    return grid;
  }

  protected HashSet<int> GetRandomBombs(int size, int totalBomb)
  {
    int[] positions = Enumerable.Range(0, size).ToArray();
    for (int i = 0; i < totalBomb; i++)
    {
      int selected = _random.Next(i, positions.Length);
      (positions[i], positions[selected]) = (positions[selected], positions[i]);
    }
    return positions.Take(totalBomb).ToHashSet();
  }

  protected bool IsBomb(int value) => value == BOMB;
}