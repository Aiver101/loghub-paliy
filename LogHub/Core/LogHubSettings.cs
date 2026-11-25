namespace LogHub.Core;

public sealed class LogHubSettings
{
    private static readonly Lazy<LogHubSettings> _instance = new(() => new LogHubSettings());

    public static LogHubSettings Instance => _instance.Value;

    public string SourcePath { get; set; } = "logs.txt";
    public long MaxFileSize { get; set; } = 5242880;
    public string OutputFile { get; set; } = "output.txt";
    public bool UseConsole { get; set; } = true;
    public bool UseFile { get; set; } = true;
    public bool UseTelegram { get; set; } = false;

    private LogHubSettings() { } // Закрытый конструктор для синглтона
}