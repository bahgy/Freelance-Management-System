

using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.States;
using TelegramBot.Services.Keyboards;

namespace TelegramBot.Handlers.Commands;

public class NewProjectCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IConversationStateStore _stateStore;
    private readonly KeyboardFactory _keyboardFactory;

    public NewProjectCommandHandler(
        ITelegramBotClient bot,
        IConversationStateStore stateStore,
        KeyboardFactory keyboardFactory)
    {
        _bot = bot;
        _stateStore = stateStore;
        _keyboardFactory = keyboardFactory;
    }

    public bool CanHandle(string input)
        => input is "/new_project"
        or BotButtons.NewProject;

    public async Task HandleAsync(
        Message message)
    {
        _stateStore.Set(
            message.Chat.Id,
            new ConversationContext
            {
                Step = ConversationStep
                    .AwaitingProjectName
            });

        await _bot.SendTextMessageAsync(
            message.Chat.Id,
            "📝 اكتب اسم المشروع:",
           replyMarkup: KeyboardFactory.GetBackKeyboard());
    }
}