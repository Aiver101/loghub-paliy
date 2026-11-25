namespace LogHub.AntiPatterns;

public class LogProcessingModule
{
    public void Process(string inputFile, string outputFile, int maxFileSize)
    {
        // НАРУШЕНИЕ SRP: Чтение конфигурации (в реальности тут был бы ConfigurationManager)
        // и чтение файла, и парсинг, и фильтрация, и логика записи.
        
        var lines = File.ReadAllLines(inputFile);
        var validLines = new List<string>();

        foreach (var line in lines)
        {
            // Разбор строки лога
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            // Ожидаем формат: [дата] УРОВЕНЬ сообщение
            var parts = line.Split(' ', 3);
            if (parts.Length < 3) 
            {
                continue; // Плохая строка
            }

            var date = parts[0];
            var level = parts[1];
            var message = parts[2];

            // Фильтрация
            if (level == "DEBUG") continue; // Отбрасываем дебаг

            // Форматирование
            var formatted = $"{date} | {level} | {message}";
            validLines.Add(formatted);
        }

        // НАРУШЕНИЕ OCP: switch по типу приёмника.
        // Если завтра понадобится отправлять в Telegram или БД, придется менять этот метод.
        Console.WriteLine($"--- Начинаем запись в {outputFile} ---");
        foreach (var line in validLines)
        {
            Console.WriteLine(line); // Консольный приёмник
            
            // Файловый приёмник с ротацией по размеру
            // НАРУШЕНИЕ DIP: прямое обращение к File и Console. Тесты потребуют реального диска.
            if (new FileInfo(outputFile).Exists && new FileInfo(outputFile).Length > maxFileSize)
            {
                File.Move(outputFile, outputFile + ".bak", true); // Ротация
            }
            File.AppendAllText(outputFile, line + Environment.NewLine);
        }
    }
}