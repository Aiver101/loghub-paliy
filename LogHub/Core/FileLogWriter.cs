namespace LogHub.Core;

public class FileLogWriter : ILogWriter
{
    private readonly string _filePath;
    private readonly long _maxFileSize;

    public FileLogWriter(string filePath, long maxFileSize)
    {
        _filePath = filePath;
        _maxFileSize = maxFileSize;
    }

    public void Write(string formattedLine)
    {
        // Ротация файлов по размеру
        if (File.Exists(_filePath) && new FileInfo(_filePath).Length > _maxFileSize)
        {
            var backupPath = _filePath + ".bak";
            File.Move(_filePath, backupPath, overwrite: true);
        }

        File.AppendAllText(_filePath, formattedLine + Environment.NewLine);
    }
}