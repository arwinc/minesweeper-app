
public record Cell(int RowIndex, int ColumnIndex, int Value)
{
  private bool _isCleaned;
  private bool _hasMarker;

  public bool IsCleaned => _isCleaned;
  public bool HasMarker => _hasMarker;

  public bool IsBomb => Value == Generator.BOMB;

  public bool HasValue => Value > 0 && !IsBomb;

  public bool IsSweepable => !IsBomb && !IsCleaned;

  public char RowLabel => LabelUtility.GetRowLabel(RowIndex);
  public char ColLabel => LabelUtility.GetColumnLabel(ColumnIndex);

  public bool Sweep()
  {
    if (IsSweepable)
    {
      _isCleaned = true;
      return true;
    }
    return false;
  }

  public void ToggleMarker()
  {
    _hasMarker = !_hasMarker;
  }
}