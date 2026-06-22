using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.Navigation;
using TelegramBot.Services.Keyboards;

namespace TelegramBot.Handlers.Commands
{
    public class ReportsCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly IUserNavigationStore _navStore;

        public ReportsCommandHandler(ITelegramBotClient bot, IUserNavigationStore navStore)
        {
            _bot = bot;
            _navStore = navStore;
        }

        public bool CanHandle(string input)
            => input is "/reports" or BotButtons.Reports;

        public async Task HandleAsync(Message message)
        {
            _navStore.GoTo(message.Chat.Id, NavigationScreen.Reports);

            await _bot.SendTextMessageAsync(message.Chat.Id,
                "📈 *التقارير*\n━━━━━━━━━━━━━━\n🚧 قريباً...",
                parseMode: ParseMode.Markdown,
                replyMarkup: KeyboardFactory.GetBackKeyboard());
        }
    }
}
