using LogHub.AntiPatterns;
using LogHub.Core;

var sourceFile = "logs.txt";
var outputFileOld = "output_old.txt";
var outputFileNew = "output_new.txt";

if (File.Exists(outputFileOld)) File.Delete(outputFileOld);
if (File.Exists(outputFileNew)) File.Delete(outputFileNew);

// 1. Запуск старого god-класса
var godClass = new LogProcessingModule();
godClass.Process(sourceFile, outputFileOld, 1024 * 1024 * 5); // 5 MB

// 2. Запуск нового рефакторинга
var service = new LogService(
    new FileLogReader(),
    new PlainTextLogParser(),
    new DebugLogFilter(),
    new PlainTextLogFormatter(),
    new ILogWriter[] 
    { 
        new ConsoleLogWriter(), 
        new FileLogWriter(outputFileNew, 1024 * 1024 * 5) 
    }
);
service.Process(sourceFile);

Console.WriteLine($"\nСтарый файл: {outputFileOld}");
Console.WriteLine($"Новый файл: {outputFileNew}");