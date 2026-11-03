namespace GoFPatterns.Mediator.ChatRoom
{
    // Mediador: los participantes se comunican solo a través de él, no directamente entre sí.
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(ChatUser sender, string message);
        void SendPrivate(ChatUser sender, string receiverName, string message);
    }
}
