using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers.Interface;

namespace TelegramBot.Handlers.Commands
{
    public class HelpCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _bot;

        public HelpCommandHandler(ITelegramBotClient bot)
            => _bot = bot;

        public bool CanHandle(string input)
            => input is "/help" or "❓ مساعدة";

        public async Task HandleAsync(Message message)
        {
            const string help = """
            🤖 *FreelanceOS Bot*

            /start — القائمة الرئيسية
            /projects — مشاريعك
            /new\_project — مشروع جديد
            /dashboard — الداشبورد
            /reports — التقارير
            /help — المساعدة
            """;

            await _bot.SendTextMessageAsync(message.Chat.Id, help,
                parseMode: ParseMode.MarkdownV2);
        }
    }
}
