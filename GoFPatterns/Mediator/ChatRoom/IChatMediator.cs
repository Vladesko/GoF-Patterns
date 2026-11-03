namespace GoFPatterns.Mediator.ChatRoom
{
    // Mediator - participants communicate only through it, not directly with each other.
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(ChatUser sender, string message);
        void SendPrivate(ChatUser sender, string receiverName, string message);
    }
}
