using Freelance_bot.Application.IServieces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.Services.Keyboards;

namespace TelegramBot.Handlers.Commands;

public class ProjectsCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IProjectService _projectService;
    private readonly KeyboardFactory _keyboards;

    public ProjectsCommandHandler(
        ITelegramBotClient bot,
        IProjectService projectService,
        KeyboardFactory keyboards)
    {
        _bot = bot;
        _projectService = projectService;
        _keyboards = keyboards;
    }

    public bool CanHandle(string input)
        => input is "/projects" or BotButtons.MyProjects;

    public async Task HandleAsync(Message message)
    {
        var telegramId = message.From!.Id;
        var projects = (await _projectService.GetByTelegramIdAsync(telegramId)).ToList();

        if (!projects.Any())
        {
            var mainMenu = await _keyboards.GetMainMenuAsync(telegramId);
            await _bot.SendTextMessageAsync(message.Chat.Id,
                "📭 مفيش مشاريع لحد دلوقتي.",
                replyMarkup: mainMenu);
            return;
        }

        var inline = await _keyboards.GetProjectsInlineAsync(telegramId);
        await _bot.SendTextMessageAsync(message.Chat.Id,
            $"📁 مشاريعك ({projects.Count}):",
            replyMarkup: inline);
    }
}