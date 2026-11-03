namespace GoFPatterns.Mediator.ChatRoom
{
    // Посредник - участники общаются только через него, а не напрямую друг с другом.
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(ChatUser sender, string message);
        void SendPrivate(ChatUser sender, string receiverName, string message);
    }
}
