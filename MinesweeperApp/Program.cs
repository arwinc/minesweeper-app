// See https://aka.ms/new-console-template for more information

Level[] levels =
[
  new("Beginner", 5, 5, 5),
  new("Intermediate", 9, 9, 12),
  new("Expert", 16, 16, 40),
];

Level selectedLevel = SelectLevel();
BoardRenderer renderer = new();
MinesweeperGame game;
CommandParser commandParser = new();

InitBoard();
renderer.Render(game.Board);

string? input;
do
{
  Console.WriteLine("\nEnter command (e.g., A3 sweep, C5 marker, quit):");
  input = Console.ReadLine();

  if (input is null)
  {
    break;
  }

  Command command = commandParser.Parse(input);

  if (command.CommandType == CommandType.Quit)
  {
    break;
  }

  var result = game.ProcessCellCommand(command);
  WriteCellCommandMessage(result);

  if (result.Status == CellCommandStatus.GameOver)
  {
    renderer.Render(game.Board, true);
    Console.WriteLine("\n>>> G A M E - O V E R! <<<");
    RestartGame();
  }
  else
  {
    Console.WriteLine("\nCurrent grid:");
    renderer.Render(game.Board);
    Console.WriteLine($"\nTotal Marker: {game.MarkerCount}");

    if (game.IsSolved)
    {
      Console.WriteLine("\nYou have successfully completed the puzzle!");
      RestartGame();
    }
  }
}
while (true);

void WriteCellCommandMessage(CellCommandResult result)
{
  switch (result.Status)
  {
    case CellCommandStatus.InvalidCommand:
      Console.WriteLine("\nInvalid command.");
      break;
    case CellCommandStatus.Accepted:
      Console.WriteLine("\nMove accepted.");
      break;
    case CellCommandStatus.CellNotFound:
      Console.WriteLine("\nInvalid cell.");
      break;
    case CellCommandStatus.MarkerIgnored:
      Console.WriteLine("\nYou don't need marker there.");
      break;
    case CellCommandStatus.NotEnoughMarker:
      Console.WriteLine("\nYou cannot place any marker.");
      break;
    case CellCommandStatus.GameOver:
      Console.WriteLine("\nIt's a bomb!!");
      break;
  }
}

void InitBoard()
{
  Generator g = new();
  game = new(g.Generate(
    selectedLevel.RowCount,
    selectedLevel.ColumnCount,
    selectedLevel.BombCount
  ));
}

void RestartGame()
{
  Console.WriteLine("Press Enter to choose a level...\n");
  Console.ReadLine();

  selectedLevel = SelectLevel();
  InitBoard();
  renderer.Render(game.Board);
}

Level SelectLevel()
{
  Console.WriteLine("Select a level:");
  Console.WriteLine("1. Beginner - 5 x 5, 5 bombs");
  Console.WriteLine("2. Intermediate - 9 x 9, 12 bombs");
  Console.WriteLine("3. Expert - 16 x 16, 40 bombs");
  Console.Write("Choose 1, 2, or 3: ");

  string? selection = Console.ReadLine();
  int index = selection switch
  {
    "1" => 0,
    "2" => 1,
    "3" => 2,
    _ => 0,
  };
  Level level = levels[index];
  Console.WriteLine($"\nStarting {level.Name}: {level.RowCount} x {level.ColumnCount}, {level.BombCount} bombs");
  return level;
}

record Level(string Name, int RowCount, int ColumnCount, int BombCount);
