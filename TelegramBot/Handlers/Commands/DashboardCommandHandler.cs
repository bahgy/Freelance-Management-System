using Freelance_bot.Application.IServieces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.Navigation;
using TelegramBot.Services.Keyboards;

namespace TelegramBot.Handlers.Commands;

public class DashboardCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserNavigationStore _navStore;

    public DashboardCommandHandler(ITelegramBotClient bot, IUserNavigationStore navStore)
    {
        _bot = bot;
        _navStore = navStore;
    }

    public bool CanHandle(string input)
        => input is "/dashboard" or BotButtons.Dashboard;

    public async Task HandleAsync(Message message)
    {
        _navStore.GoTo(message.Chat.Id, NavigationScreen.Dashboard);

        await _bot.SendTextMessageAsync(message.Chat.Id,
            "📊 *Dashboard*\n━━━━━━━━━━━━━━\n🚧 قريباً...",
            parseMode: ParseMode.Markdown,
            replyMarkup: KeyboardFactory.GetBackKeyboard());
    }
}