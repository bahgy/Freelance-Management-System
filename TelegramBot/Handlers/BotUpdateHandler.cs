
using Freelance_bot.Application.Feature.Projects.Requests;
using Freelance_bot.Application.IServieces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Extensions.Constants;
using TelegramBot.Handlers.Interface;
using TelegramBot.Navigation;
using TelegramBot.Services.Keyboards;
using TelegramBot.States;

public class BotUpdateHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IConversationStateStore _stateStore;
    private readonly IUserNavigationStore _navStore;
    private readonly KeyboardFactory _keyboards;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;
    private readonly IEnumerable<ICommandHandler> _handlers;

    public BotUpdateHandler(
        ITelegramBotClient bot,
        IConversationStateStore stateStore,
        IUserNavigationStore navStore,
        KeyboardFactory keyboards,
        IProjectService projectService,
        IUserService userService,
        IEnumerable<ICommandHandler> handlers)
    {
        _bot = bot;
        _stateStore = stateStore;
        _navStore = navStore;
        _keyboards = keyboards;
        _projectService = projectService;
        _userService = userService;
        _handlers = handlers;
    }

    // ─────────────────────────────────────────────────────────────
    public async Task HandleAsync(Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message when update.Message is not null:
                await HandleMessageAsync(update.Message);
                break;

            case UpdateType.CallbackQuery when update.CallbackQuery is not null:
                await HandleCallbackAsync(update.CallbackQuery);
                break;
        }
    }

    // ─────────────────────────────────────────────────────────────
    private async Task HandleMessageAsync(Message message)
    {
        var chatId = message.Chat.Id;
        var text = message.Text?.Trim() ?? string.Empty;

        // 1. ── Conversation step FIRST (highest priority) ──────
        var ctx = _stateStore.Get(chatId);

        if (ctx.Step != ConversationStep.None)
        {
            // Back always cancels the current flow
            if (text == BotButtons.Back)
            {
                _stateStore.Clear(chatId);
                await SendMainMenuAsync(chatId, message.From!.Id);
                return;
            }

            await HandleConversationStepAsync(message, ctx);
            return;
        }

        // 2. ── Normalize command (strip params + lowercase) ────
        var command = text.StartsWith('/')
            ? text.Split(' ')[0].ToLower()
            : text;

        // 3. ── Route to matching handler ──────────────────────
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(command));
        if (handler is not null)
        {
            await handler.HandleAsync(message);
            return;
        }

        // 4. ── Fallback ───────────────────────────────────────
        var keyboard = await _keyboards.GetMainMenuAsync(message.From!.Id);
        await _bot.SendTextMessageAsync(chatId,
            "🤔 مش فاهم.\nاضغط على أي زر من القائمة.",
            replyMarkup: keyboard);
    }

    // ─────────────────────────────────────────────────────────────
    private async Task HandleCallbackAsync(CallbackQuery callback)
    {
        await _bot.AnswerCallbackQueryAsync(callback.Id);

        var chatId = callback.Message!.Chat.Id;
        var telegramId = callback.From.Id;
        var data = callback.Data ?? string.Empty;

        if (data.StartsWith("project:"))
        {
            var projectId = Guid.Parse(data["project:".Length..]);
            await ShowProjectDetailAsync(chatId, telegramId, projectId);
        }
        else if (data.StartsWith("delete_project:"))
        {
            var projectId = Guid.Parse(data["delete_project:".Length..]);
            await _bot.SendTextMessageAsync(chatId,
                "⚠️ متأكد إنك عاوز تحذف المشروع؟",
                replyMarkup: KeyboardFactory.GetConfirmDeleteInline(projectId));
        }
        else if (data.StartsWith("confirm_delete:"))
        {
            var projectId = Guid.Parse(data["confirm_delete:".Length..]);
            // TODO: await _projectService.DeleteAsync(projectId);
            var keyboard = await _keyboards.GetMainMenuAsync(telegramId);
            await _bot.SendTextMessageAsync(chatId, "✅ اتحذف بنجاح.", replyMarkup: keyboard);
            _navStore.Back(chatId);
        }
        else if (data == "confirm_create")
        {
            await ExecuteCreateProjectAsync(chatId, telegramId);
        }
        else if (data == "cancel")
        {
            _stateStore.Clear(chatId);
            await SendMainMenuAsync(chatId, telegramId);
        }
    }

    // ─────────────────────────────────────────────────────────────
    // Full 6-Step Conversation Flow
    private async Task HandleConversationStepAsync(Message message, ConversationContext ctx)
    {
        var chatId = message.Chat.Id;
        var telegramId = message.From!.Id;
        var input = message.Text?.Trim() ?? string.Empty;

        switch (ctx.Step)
        {
            // ── Step 1: Name ─────────────────────────────────────
            case ConversationStep.AwaitingProjectName:

                if (string.IsNullOrWhiteSpace(input))
                {
                    await _bot.SendTextMessageAsync(chatId, "⚠️ اكتب اسم صحيح.");
                    return;
                }

                ctx.Data["name"] = input;
                ctx.Step = ConversationStep.AwaitingProjectDescription;
                _stateStore.Set(chatId, ctx);

                await _bot.SendTextMessageAsync(chatId,
                    $"✅ الاسم: *{input}*\n\n📝 اكتب وصف المشروع:",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: KeyboardFactory.GetBackKeyboard());
                break;

            // ── Step 2: Description ──────────────────────────────
            case ConversationStep.AwaitingProjectDescription:

                ctx.Data["description"] = input;
                ctx.Step = ConversationStep.AwaitingBudget;
                _stateStore.Set(chatId, ctx);

                await _bot.SendTextMessageAsync(chatId,
                    "💰 اكتب الميزانية (رقم فقط):\nمثال: 5000",
                    replyMarkup: KeyboardFactory.GetBackKeyboard());
                break;

            // ── Step 3: Budget ───────────────────────────────────
            case ConversationStep.AwaitingBudget:

                if (!decimal.TryParse(input, out var budget))
                {
                    await _bot.SendTextMessageAsync(chatId,
                        "⚠️ اكتب رقم صحيح.\nمثال: 5000");
                    return;
                }

                ctx.Data["budget"] = budget.ToString();
                ctx.Step = ConversationStep.AwaitingDeadline;
                _stateStore.Set(chatId, ctx);

                await _bot.SendTextMessageAsync(chatId,
                    "📅 اكتب الديدلاين:\nالصيغة: dd/MM/yyyy\nمثال: 20/06/2026",
                    replyMarkup: KeyboardFactory.GetBackKeyboard());
                break;

            // ── Step 4: Deadline ─────────────────────────────────
            case ConversationStep.AwaitingDeadline:

                if (!DateTime.TryParseExact(
                    input, "dd/MM/yyyy",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var deadline))
                {
                    await _bot.SendTextMessageAsync(chatId,
                        "⚠️ الصيغة غلط.\nاكتب: dd/MM/yyyy\nمثال: 20/06/2026");
                    return;
                }

                ctx.Data["deadline"] = deadline.ToString("o");
                ctx.Step = ConversationStep.AwaitingConfirmation;
                _stateStore.Set(chatId, ctx);

                var summary = $"""
                    📋 *ملخص المشروع*
                    ━━━━━━━━━━━━━━
                    📌 الاسم: {ctx.Data["name"]}
                    📝 الوصف: {ctx.Data["description"]}
                    💰 الميزانية: {ctx.Data["budget"]} EGP
                    📅 الديدلاين: {deadline:dd/MM/yyyy}

                    هل تأكد؟
                    """;

                await _bot.SendTextMessageAsync(chatId,
                    summary,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("✅ نعم، أنشئ", "confirm_create"),
                            InlineKeyboardButton.WithCallbackData("❌ إلغاء",    "cancel")
                        }
                    }));
                break;

            // ── Step 5: Workspace name ────────────────────────────
            case ConversationStep.AwaitingWorkspaceName:

                _stateStore.Clear(chatId);
                // TODO: WorkspaceService.CreateAsync(input)

                var wKeyboard = await _keyboards.GetMainMenuAsync(telegramId);
                await _bot.SendTextMessageAsync(chatId,
                    $"🏢 *{input}* اتعمل بنجاح!",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: wKeyboard);
                break;
        }
    }

    // ─────────────────────────────────────────────────────────────
    private async Task ExecuteCreateProjectAsync(long chatId, long telegramId)
    {
        var ctx = _stateStore.Get(chatId);
        _stateStore.Clear(chatId);

        var user = await _userService.GetOrCreateByTelegramIdAsync(telegramId, "");

        await _projectService.CreateAsync(user.Id, new CreateProjectRequest(
            Title: ctx.Data.GetValueOrDefault("name", "Untitled"),
            Description: ctx.Data.GetValueOrDefault("description", ""),
            ClientId: null,
            Budget: decimal.Parse(ctx.Data.GetValueOrDefault("budget", "0")),
            Currency: "EGP",
            StartDate: DateTime.UtcNow,
            Deadline: ctx.Data.ContainsKey("deadline")
                             ? DateTime.Parse(ctx.Data["deadline"])
                             : null
        ));

        var keyboard = await _keyboards.GetMainMenuAsync(telegramId);

        await _bot.SendTextMessageAsync(chatId,
            $"🎉 *{ctx.Data.GetValueOrDefault("name")}* اتعمل بنجاح!",
            parseMode: ParseMode.Markdown,
            replyMarkup: keyboard);
    }

    // ─────────────────────────────────────────────────────────────
    private async Task ShowProjectDetailAsync(long chatId, long telegramId, Guid projectId)
    {
        _navStore.GoTo(chatId, NavigationScreen.ProjectDetail, projectId);

        var projects = await _projectService.GetByTelegramIdAsync(telegramId);
        var project = projects.FirstOrDefault(p => p.Id == projectId);

        if (project is null) { await SendMainMenuAsync(chatId, telegramId); return; }

        await _bot.SendTextMessageAsync(chatId,
            $"📋 *{project.Title}*\n━━━━━━━━━━━━━━\n📌 الحالة: {project.Status}",
            parseMode: ParseMode.Markdown,
            replyMarkup: KeyboardFactory.GetProjectDetailInline(projectId));
    }

    // ─────────────────────────────────────────────────────────────
    public async Task SendMainMenuAsync(long chatId, long telegramId)
    {
        var keyboard = await _keyboards.GetMainMenuAsync(telegramId);
        await _bot.SendTextMessageAsync(chatId, "🏠 القائمة الرئيسية:", replyMarkup: keyboard);
    }
}