namespace GoFPatterns.Mediator.ChatRoom
{
    // Mediator - i partecipanti comunicano solo tramite esso, non direttamente tra loro.
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(ChatUser sender, string message);
        void SendPrivate(ChatUser sender, string receiverName, string message);
    }
}
