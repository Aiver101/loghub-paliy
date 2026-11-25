namespace LogHub.Core;

public interface ILogWriter
{
    void Write(string formattedLine);
}