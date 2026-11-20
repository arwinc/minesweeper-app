namespace MinesweeperApp.Tests;

public class CellTests
{
  [Fact]
  public void IsBomb_ReturnsCorrectly()
  {
    Cell sut = new(0, 0, Generator.BOMB);
    Assert.True(sut.IsBomb);

    sut = new(0, 1, 0);
    Assert.False(sut.IsBomb);

    sut = new(0, 1, 6);
    Assert.False(sut.IsBomb);
  }

  [Fact]
  public void Sweep_ReturnsCorrectly()
  {
    Cell sut = new(0, 0, 0);
    Assert.False(sut.IsCleaned);
    Assert.True(sut.IsSweepable);
    Assert.True(sut.Sweep());

    sut = new(0, 1, Generator.BOMB);
    Assert.False(sut.IsCleaned);
    Assert.False(sut.IsSweepable);
    Assert.False(sut.Sweep());
  }


  [Fact]
  public void ToggleMarker_ReturnsCorrectly()
  {
    Cell sut = new(0,0,0);
    Assert.False(sut.HasMarker);

    sut.ToggleMarker();
    Assert.True(sut.HasMarker);

    sut.ToggleMarker();
    Assert.False(sut.HasMarker);
  }
}