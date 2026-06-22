namespace TelegramBot.States;

public interface IConversationStateStore
{
    ConversationContext Get(long chatId);

    void Set(
        long chatId,
        ConversationContext context);

    void Clear(long chatId);
}

public class ConversationStateStore
    : IConversationStateStore
{
    private readonly Dictionary<long, ConversationContext>
        _store = new();

    public ConversationContext Get(
        long chatId)
    {
        return _store.TryGetValue(
            chatId,
            out var ctx)
            ? ctx
            : new ConversationContext();
    }

    public void Set(
        long chatId,
        ConversationContext context)
    {
        _store[chatId] = context;
    }

    public void Clear(
        long chatId)
    {
        _store.Remove(chatId);
    }
}