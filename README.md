# Minesweeper

A console-based Minesweeper game built with C# and .NET.

## Overview

The board contains hidden bombs. Your goal is to reveal every safe cell without sweeping a bomb.

## Difficulty Levels

Choose a difficulty level when the game starts:

| Level | Board size | Bombs |
|---|---:|---:|
| Beginner | 5 x 5 | 5 |
| Intermediate | 9 x 9 | 12 |
| Expert | 16 x 16 | 40 |

After a game ends, you can select a difficulty level before starting again.

Invalid level selections use `Beginner` mode by default.

## Running the Game

Run the application from the repository root:

```bash
dotnet run --project MinesweeperApp
```

You can then enter commands at the prompt.

### Board

Rows are labeled from `A` to `Z`. Columns are labeled `1` through `9`, followed by `A` through `Z`.

For example:

```bash
    1   2   3   4   5
 A ░░░ ░░░ ░░░ ░░░ ░░░
 B ░░░ ░░░ ░░░ ░░░ ░░░
 C ░░░ ░░░ ░░░ ░░░ ░░░
 D ░░░ ░░░ ░░░ ░░░ ░░░
 E ░░░ ░░░ ░░░ ░░░ ░░░
```

The board uses the following symbols:

Symbol|Meaning
-|-
░░░|Hidden safe cell
▓▓▓|Revealed cell with no nearby bombs
1 to 8|Number of nearby bombs
?|Marker
B|Bomb revealed after game over
X|Incorrect marker revealed after game over

### Commands

Commands use the row, column, and action:

```
<row><column> <action>
```

Sweep a Cell

```
A1 sweep
```

- A number shows how many bombs are in the eight surrounding cells.
- Sweeping a `0` reveals nearby safe cells automatically.
- Revealing a bomb immediately ends the game.
- Sweeping an already revealed cell has no effect.

Place or Remove a Marker

```
A1 marker
```

- Running the same command again removes the marker.
- Markers are displayed as `?`.
- A marker prevents a cell from being swept.
- The number of available markers starts equal to the number of bombs.
- A marker can be placed or removed on an unrevealed safe or bomb cell.

Quit

```
quit
```

This exits the game.

### Rules
- Commands are case-sensitive: use `A1 sweep`, `A1 marker`, or `quit`.
- Sweeping a bomb loses the game.
- Markers cannot be placed on revealed cells.
- Markers cannot be placed when no markers remain.
- Sweeping a marked cell has no effect until the marker is removed.
- The puzzle is solved when every safe cell has been revealed.
- Bombs do not need to be marked to complete the puzzle.
- After winning or losing, press Enter, then choose a difficulty level for the new game.

### Nearby Bomb Count

Each number represents the bombs in the eight adjacent positions, including diagonally adjacent positions.

For example:

|   | 1 | 2 | 3 | 4 | 5 |
|-|:-:|:-:|:-:|:-:|:-:|
| A | B | B | B | Hidden | Hidden |
| B | B |Hidden| B | Hidden | Hidden |
| C | B | B | B | Hidden | Hidden |
| D | Hidden | Hidden | Hidden | Hidden | Hidden |
| E | Hidden | Hidden | Hidden | Hidden | Hidden |

The corresponding bomb count is:

|   | 1 | 2 | 3 | 4 | 5 |
|-|:-:|:-:|:-:|:-:|:-:|
| A | B | B | B | 2 | 0 |
| B | B | 8 | B | 3 | 0 |
| C | B | B | B | 2 | 0 |
| D | 2 | 3 | 2 | 1 | 0 |
| E | 0 | 0 | 0 | 0 | 0 |

### Board Limits
The board generator supports:

- A minimum size of 2 rows by 2 columns.
- A maximum size of 26 rows by 35 columns.
- At least one bomb per board.
- Fewer bombs than the total number of cells.
- Random placement of bombs.

## Running Tests

Run the test suite from the repository root:

```bash
dotnet test
```