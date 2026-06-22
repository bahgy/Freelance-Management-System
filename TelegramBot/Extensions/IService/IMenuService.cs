using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Services;

public interface IMenuService
{
    Task<ReplyKeyboardMarkup>
        BuildMenu(long telegramId);
}