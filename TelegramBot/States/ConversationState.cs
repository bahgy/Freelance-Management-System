namespace TelegramBot.States;

public enum ConversationStep
{
    None,

    // ── Workspace ─────────────────────────────────
    AwaitingWorkspaceName,

    // ── Project creation (full 6-step flow) ───────
    AwaitingProjectName,
    AwaitingProjectDescription,
    AwaitingBudget,            // ← جديد
    AwaitingDeadline,          // ← جديد
    AwaitingConfirmation,      // ← جديد (handled via CallbackQuery)

    // ── Task creation ─────────────────────────────
    AwaitingTaskTitle,
}

//public class ConversationContext
//{
//    public ConversationStep Step { get; set; } = ConversationStep.None;
//    public Dictionary<string, string> Data { get; set; } = new();
//}