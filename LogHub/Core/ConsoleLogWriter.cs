namespace LogHub.Core;

public class ConsoleLogWriter : ILogWriter
{
    public void Write(string formattedLine)
    {
        Console.WriteLine(formattedLine);
    }
}