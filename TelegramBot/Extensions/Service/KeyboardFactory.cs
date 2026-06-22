using Freelance_bot.Application.IServieces;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Extensions.Constants;

namespace TelegramBot.Services.Keyboards;

public class KeyboardFactory
{
    private readonly IProjectService _projectService;

    public KeyboardFactory(
        IProjectService projectService)
    {
        _projectService = projectService;
    }

    // ====================================================
    // Main Menu
    // ====================================================

    public async Task<ReplyKeyboardMarkup>
        GetMainMenuAsync(long telegramId)
    {
        var projects =
            await _projectService
            .GetByTelegramIdAsync(telegramId);

        var buttons =
            new List<KeyboardButton[]>
            {
                new[]
                {
                    new KeyboardButton(
                        BotButtons.MyProjects)
                },

                new[]
                {
                    new KeyboardButton(
                        BotButtons.NewProject)
                }
            };

        if (projects.Any())
        {
            buttons.Add(
                new[]
                {
                    new KeyboardButton(
                        BotButtons.Dashboard),

                    new KeyboardButton(
                        BotButtons.Workspace)
                });

            buttons.Add(
                new[]
                {
                    new KeyboardButton(
                        BotButtons.Reports),

                    new KeyboardButton(
                        BotButtons.DeleteProject)
                });
        }

        buttons.Add(
            new[]
            {
                new KeyboardButton(
                    BotButtons.Help)
            });

        return new ReplyKeyboardMarkup(
            buttons)
        {
            ResizeKeyboard = true
        };
    }

    // ====================================================
    // Back Keyboard
    // ====================================================

    public static ReplyKeyboardMarkup
        GetBackKeyboard()
    {
        return new ReplyKeyboardMarkup(
            new[]
            {
                new[]
                {
                    new KeyboardButton(
                        BotButtons.Back),

                    new KeyboardButton(
                        BotButtons.Home)
                }
            })
        {
            ResizeKeyboard = true
        };
    }

    // ====================================================
    // Confirm Delete
    // ====================================================

    public static InlineKeyboardMarkup
        GetConfirmDeleteInline(
            Guid projectId)
    {
        return new InlineKeyboardMarkup(
        new[]
        {
            new[]
            {
                InlineKeyboardButton
                .WithCallbackData(
                    "✅ نعم",
                    $"confirm_delete:{projectId}"),

                InlineKeyboardButton
                .WithCallbackData(
                    "❌ إلغاء",
                    "cancel")
            }
        });
    }

    // ====================================================
    // Project Details
    // ====================================================

    public static InlineKeyboardMarkup
        GetProjectDetailInline(
            Guid projectId)
    {
        return new InlineKeyboardMarkup(
        new[]
        {
            new[]
            {
                InlineKeyboardButton
                .WithCallbackData(
                    "📊 Dashboard",
                    $"dashboard:{projectId}")
            },

            new[]
            {
                InlineKeyboardButton
                .WithCallbackData(
                    "👥 Workspace",
                    $"workspace:{projectId}")
            },

            new[]
            {
                InlineKeyboardButton
                .WithCallbackData(
                    "📄 Reports",
                    $"reports:{projectId}")
            },

            new[]
            {
                InlineKeyboardButton
                .WithCallbackData(
                    "🗑 Delete",
                    $"delete_project:{projectId}")
            }
        });
    }

    // ====================================================
    // Projects List
    // ====================================================

    public async Task<InlineKeyboardMarkup>
        GetProjectsInlineAsync(
            long telegramId)
    {
        var projects =
            await _projectService
            .GetByTelegramIdAsync(
                telegramId);

        var buttons =
            projects
            .Select(p =>
                new[]
                {
                    InlineKeyboardButton
                    .WithCallbackData(
                        $"📁 {p.Title}",
                        $"project:{p.Id}")
                })
            .ToArray();

        return new InlineKeyboardMarkup(
            buttons);
    }
}