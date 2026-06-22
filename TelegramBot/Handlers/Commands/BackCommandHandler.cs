using Freelance_bot.Application.IServieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.Navigation;
using TelegramBot.Services.Keyboards;
using TelegramBot.States;

namespace TelegramBot.Handlers.Commands
{
    public class BackCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConversationStateStore _stateStore;
        private readonly IUserNavigationStore _navStore;
        private readonly KeyboardFactory _keyboards;
        private readonly IUserService _userService;

        public BackCommandHandler(
            ITelegramBotClient bot,
            IConversationStateStore stateStore,
            IUserNavigationStore navStore,
            KeyboardFactory keyboards,
            IUserService userService)
        {
            _bot = bot;
            _stateStore = stateStore;
            _navStore = navStore;
            _keyboards = keyboards;
            _userService = userService;
        }

        public bool CanHandle(string input)
            => input == BotButtons.Back;

        public async Task HandleAsync(Message message)
        {
            var chatId = message.Chat.Id;
            var telegramId = message.From!.Id;

            _stateStore.Clear(chatId);
            _navStore.Back(chatId);

            var keyboard = await _keyboards.GetMainMenuAsync(telegramId);
            await _bot.SendTextMessageAsync(chatId, "🏠 القائمة الرئيسية:", replyMarkup: keyboard);
        }
    }
}
