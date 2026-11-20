// See https://aka.ms/new-console-template for more information

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
  game = new(g.Generate(3, 2, 1));
}

void RestartGame()
{
  Console.WriteLine("Press any key to play again...\n");
  Console.ReadLine();

  InitBoard();
  renderer.Render(game.Board);
}