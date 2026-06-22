using Freelance_bot.Application.IServieces;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Handlers.Interface;
using TelegramBot.Services.Keyboards;

namespace FreelanceOS.Bot.Handlers.Commands;

public class StartCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserService _userService; 
    private readonly KeyboardFactory _keyboards;

    public StartCommandHandler(
        ITelegramBotClient bot,
        IUserService userService,
        KeyboardFactory keyboards)
    {
        _bot = bot;
        _userService = userService;
        _keyboards = keyboards;
    }

    public bool CanHandle(string input)
        => input is "/start" or "🏠 الرئيسية";

    public async Task HandleAsync(Message message)
    {
        var telegramId = message.From!.Id;
        var name = message.From.FirstName;

        await _userService.GetOrCreateByTelegramIdAsync(telegramId, name);

        var keyboard = await _keyboards.GetMainMenuAsync(telegramId);

        await _bot.SendTextMessageAsync(
            message.Chat.Id,
            $"👋 أهلاً {name}!\n\nاختار من القائمة:",
            replyMarkup: keyboard);
    }
}