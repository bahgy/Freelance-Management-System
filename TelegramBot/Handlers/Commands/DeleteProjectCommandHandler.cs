using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Handlers.Interface;
using TelegramBot.Services.Keyboards;

namespace TelegramBot.Handlers.Commands
{
    public class DeleteProjectCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly KeyboardFactory _keyboards;

        public DeleteProjectCommandHandler(ITelegramBotClient bot, KeyboardFactory keyboards)
        {
            _bot = bot;
            _keyboards = keyboards;
        }

        public bool CanHandle(string input)
            => input is "/delete_project";

        public async Task HandleAsync(Message message)
        {
            var inline = await _keyboards.GetProjectsInlineAsync(message.From!.Id);

            await _bot.SendTextMessageAsync(message.Chat.Id,
                "🗑️ اختار المشروع اللى عاوز تحذفه:",
                replyMarkup: inline);
        }
    }
}
