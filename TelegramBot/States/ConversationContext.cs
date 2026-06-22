namespace TelegramBot.States;

public class ConversationContext
{
    public ConversationStep Step { get; set; }
        = ConversationStep.None;

    public Dictionary<string, string> Data
    {
        get;
        set;
    }
        = new();

    public Stack<string> NavigationHistory
    {
        get;
        set;
    }
        = new();

    public Guid? CurrentProjectId
    {
        get;
        set;
    }

}