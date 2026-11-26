using LogHub;
using LogHub.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var settings = LogHubSettings.Instance;
configuration.GetSection("LogHub").Bind(settings);

var services = new ServiceCollection();

services.AddTransient<ILogFilter, DebugLogFilter>();
services.AddTransient<ILogFormatter, PlainTextLogFormatter>();

if (settings.UseConsole) services.AddTransient<ILogWriter, ConsoleLogWriter>();
if (settings.UseFile) services.AddTransient<ILogWriter>(sp => new FileLogWriter(settings.OutputFile, settings.MaxFileSize));
if (settings.UseTelegram) services.AddTransient<ILogWriter>(sp => new TelegramLogWriter("fake_token", "123456"));

// Регистрация импортеров (Стратегий)
services.AddTransient<PlainTextLogImporter>(sp => new PlainTextLogImporter(
    new PlainTextLogParser(),
    sp.GetRequiredService<ILogFilter>(),
    sp.GetRequiredService<ILogFormatter>(),
    sp.GetServices<ILogWriter>()));

services.AddTransient<JsonLogImporter>(sp => new JsonLogImporter(
    new JsonLogParser(),
    sp.GetRequiredService<ILogFilter>(),
    sp.GetRequiredService<ILogFormatter>(),
    sp.GetServices<ILogWriter>()));

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("--- Запуск LogHub (Plain Text) ---");
var plainImporter = serviceProvider.GetRequiredService<PlainTextLogImporter>();
plainImporter.Import(settings.SourcePath);

Console.WriteLine("\n--- Запуск LogHub (JSON) ---");
var jsonFile = "logs.json";
if (File.Exists(jsonFile))
{
    var jsonImporter = serviceProvider.GetRequiredService<JsonLogImporter>();
    jsonImporter.Import(jsonFile);
}

// Демонстрация работы Итератора
Console.WriteLine("\n--- Демонстрация Итератора ---");
var collection = new LogCollection();
collection.Add(new LogEntry { Date = "2026", Level = "INFO", Message = "Итератор 1" });
collection.Add(new LogEntry { Date = "2026", Level = "ERROR", Message = "Итератор 2" });

foreach (var entry in collection)
{
    Console.WriteLine($"Обход: {entry.Level} - {entry.Message}");
}

Console.WriteLine("--- Завершено ---");