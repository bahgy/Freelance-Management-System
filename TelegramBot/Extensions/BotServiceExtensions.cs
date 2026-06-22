using FreelanceOS.Bot.Handlers.Commands;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramBot.Extensions.Service;
using TelegramBot.Handlers;
using TelegramBot.Handlers.Commands;
using TelegramBot.Handlers.Interface;
using TelegramBot.Navigation;
using TelegramBot.Services;
using TelegramBot.Services.Keyboards;
using TelegramBot.States;


namespace TelegramBot.Extensions;


public static class BotServiceExtensions
{
    public static IServiceCollection AddTelegramBot(
        this IServiceCollection services,
        string token)
    {
        // ── Singletons (state lives for app lifetime) ──────────────
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(token));
        services.AddSingleton<IConversationStateStore, ConversationStateStore>();
        services.AddSingleton<IUserNavigationStore, UserNavigationStore>();

        // ── Scoped ────────────────────────────────────────────────
        services.AddScoped<KeyboardFactory>();

        // ── Command handlers (all registered as ICommandHandler) ───
        services.AddScoped<ICommandHandler, StartCommandHandler>();
        services.AddScoped<ICommandHandler, NewProjectCommandHandler>();
        services.AddScoped<ICommandHandler, ProjectsCommandHandler>();
        services.AddScoped<ICommandHandler, DashboardCommandHandler>();
        services.AddScoped<ICommandHandler, ReportsCommandHandler>();
        services.AddScoped<ICommandHandler, WorkspaceCommandHandler>();
        services.AddScoped<ICommandHandler, HelpCommandHandler>();
        services.AddScoped<ICommandHandler, BackCommandHandler>();
        services.AddScoped<ICommandHandler, DeleteProjectCommandHandler>();

        // ── Main router ────────────────────────────────────────────
        services.AddScoped<BotUpdateHandler>();

        return services;
    }
}