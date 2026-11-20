public static class MatrixExtensions
{
  public static IEnumerable<Cell> ToEnumerableCells(this int[,] grid)
  {
    int rowCount = grid.GetLength(0);
    int columnCount = grid.GetLength(1);
    for (int row = 0; row < rowCount; row++)
    {
      for (int column = 0; column < columnCount; column++)
      {
        yield return new Cell(row, column, grid[row, column]);
      }
    }
  }
}