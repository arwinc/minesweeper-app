public static class ConsoleReader
{
  public static Func<Task<string?>> ReadLineHandler { get; set; } =
    () => Task.FromResult(Console.ReadLine());

  public static Task<string?> ReadLineAsync() => ReadLineHandler();
}