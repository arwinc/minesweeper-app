public class LabelUtility
{
  public const byte MaxRowCount = 26;
  public const byte MaxColumnCount = 35;

  public static char GetColumnLabel(int columnIndex)
  {
    columnIndex = Math.Min(columnIndex, MaxColumnCount);
    if (columnIndex < 9)
    {
      return (char)('1' + columnIndex);
    }
    else
    {
      return (char)('A' + (columnIndex - 9));
    }
  }

  public static char GetRowLabel(int rowIndex)
  {
    return (char)('A' + Math.Min(rowIndex, MaxRowCount));
  }

  public static int MapToColumnIndex(char label)
  {
    if (label >= '1' && label <= '9')
    {
      return label - '1';
    }
    else if (label >= 'A' && label <= 'Z')
    {
      return 9 + label - 'A';
    }
    else
    {
      throw new ArgumentOutOfRangeException(nameof(label));
    }
  }
}