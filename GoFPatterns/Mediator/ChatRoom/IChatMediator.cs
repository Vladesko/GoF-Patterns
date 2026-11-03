namespace GoFPatterns.Mediator.ChatRoom
{
    // Vermittler – die Teilnehmer kommunizieren nur über ihn, nicht direkt miteinander.
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(ChatUser sender, string message);
        void SendPrivate(ChatUser sender, string receiverName, string message);
    }
}
