public enum SweepStatus
{
  Bomb,
  Marker,
  Swept
}

public class Board
{
  public IReadOnlyList<Cell> Cells { get; }
  public int RowCount { get; }
  public int ColumnCount { get; }

  public Board(IEnumerable<Cell> cells)
  {
    List<Cell> boardCells = cells.ToList();
    int rowCount = boardCells.Count == 0 ? 0 : boardCells.Max(cell => cell.RowIndex) + 1;
    int columnCount = boardCells.Count == 0 ? 0 : boardCells.Max(cell => cell.ColumnIndex) + 1;

    if (rowCount > LabelUtility.MaxRowCount || columnCount > LabelUtility.MaxColumnCount)
    {
      throw new ArgumentException($"Board cannot exceed {LabelUtility.MaxRowCount} rows or {LabelUtility.MaxColumnCount} columns.", nameof(cells));
    }

    Cells = boardCells;
    RowCount = rowCount;
    ColumnCount = columnCount;
  }

  public int BombCount => Cells.Count(x => x.IsBomb);

  public Cell? GetCell(int row, int col) =>
    Cells.FirstOrDefault(x => x.RowIndex == row && x.ColumnIndex == col);

  public SweepStatus SweepMine(Cell cell)
  {
    if (cell.HasMarker) return SweepStatus.Marker;
    if (cell.IsBomb) return SweepStatus.Bomb;
    if (cell.IsCleaned) return SweepStatus.Swept;

    SweepNeighbours(cell);
    return SweepStatus.Swept;
  }

  public Cell[] GetNeighbours(Cell? cell)
  {
    if (cell is null) return [];

    List<Cell> neighbours = [];
    for (int row = Math.Max(0, cell.RowIndex - 1);
      row <= Math.Min(RowCount - 1, cell.RowIndex + 1);
      row++)
    {
      for (int col = Math.Max(0, cell.ColumnIndex - 1);
      col <= Math.Min(ColumnCount - 1, cell.ColumnIndex + 1);
      col++)
      {
        if (row == cell.RowIndex && col == cell.ColumnIndex) continue;
        Cell? neighbour = GetCell(row, col);
        if (neighbour is not null)
        {
          neighbours.Add(neighbour);
        }
      }
    }
    return neighbours.ToArray();
  }

  private void SweepNeighbours(Cell cell)
  {
    if (cell.HasMarker) return;
    if (!cell.Sweep()) return;

    if (cell.HasValue) return;

    var neighbours = GetNeighbours(cell);
    foreach (var neighbour in neighbours)
    {
      SweepNeighbours(neighbour);
    }
  }
}