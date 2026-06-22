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
    public class WorkspaceCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConversationStateStore _stateStore;
        private readonly IUserNavigationStore _navStore;

        public WorkspaceCommandHandler(
            ITelegramBotClient bot,
            IConversationStateStore stateStore,
            IUserNavigationStore navStore)
        {
            _bot = bot;
            _stateStore = stateStore;
            _navStore = navStore;
        }

        public bool CanHandle(string input)
            => input is "/workspace" or BotButtons.CreateWorkspace;

        public async Task HandleAsync(Message message)
        {
            _navStore.GoTo(message.Chat.Id, NavigationScreen.Workspace);

            _stateStore.Set(message.Chat.Id, new ConversationContext
            {
                Step = ConversationStep.AwaitingWorkspaceName
            });

            await _bot.SendTextMessageAsync(message.Chat.Id,
                "🏢 اكتب اسم الـ Workspace:",
                replyMarkup: KeyboardFactory.GetBackKeyboard());
        }
    }
}
