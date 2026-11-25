using LogHub.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// 1. Загружаем конфигурацию из appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// 2. Заполняем наш Синглтон настройками
var settings = LogHubSettings.Instance;
configuration.GetSection("LogHub").Bind(settings);

// 3. Настраиваем DI-контейнер (IoC)
var services = new ServiceCollection();

// Регистрируем слои
services.AddTransient<ILogReader, FileLogReader>();
services.AddTransient<ILogParser, PlainTextLogParser>();
services.AddTransient<ILogFilter, DebugLogFilter>();
services.AddTransient<ILogFormatter, PlainTextLogFormatter>();

// Регистрируем приёмники (Writers) на основе конфигурации
if (settings.UseConsole)
{
    services.AddTransient<ILogWriter, ConsoleLogWriter>();
}
if (settings.UseFile)
{
    // Для FileLogWriter нужно передать параметры из настроек
    services.AddTransient<ILogWriter>(sp => new FileLogWriter(settings.OutputFile, settings.MaxFileSize));
}
if (settings.UseTelegram)
{
    services.AddTransient<ILogWriter>(sp => new TelegramLogWriter("fake_token", "123456"));
}

// Регистрируем сам сервис
services.AddTransient<LogService>();

// 4. Собираем провайдер и получаем сервис
var serviceProvider = services.BuildServiceProvider();
var logService = serviceProvider.GetRequiredService<LogService>();

// 5. Запускаем обработку
Console.WriteLine("--- Запуск LogHub через DI ---");
logService.Process(settings.SourcePath);
Console.WriteLine("--- Завершено ---");