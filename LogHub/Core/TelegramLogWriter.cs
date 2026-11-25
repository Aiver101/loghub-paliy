namespace LogHub.Core;

// Новый приёмник. Мы НЕ меняем старые файлы (LogService, ILogWriter и т.д.)
public class TelegramLogWriter : ILogWriter
{
    private readonly string _botToken;
    private readonly string _chatId;

    public TelegramLogWriter(string botToken, string chatId)
    {
        _botToken = botToken;
        _chatId = chatId;
    }

    public void Write(string formattedLine)
    {
        // В реальности здесь был бы HTTP-запрос к API Telegram
        // Для учебной работы достаточно имитации
        Console.WriteLine($"[TELEGRAM] Отправлено в чат {_chatId}: {formattedLine}");
    }
}