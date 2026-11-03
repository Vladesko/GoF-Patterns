namespace GoFPatterns.Mediator.ChatRoom
{
    // Otro colega con su propia reacción a los mensajes.
    public class ChatBot(IChatMediator chat) : ChatUser("Bot", chat)
    {
        public override void Receive(string from, string message)
        {
            if (message.StartsWith("/time"))
                SendPrivate(from, $"La hora actual es {DateTime.Now:HH:mm}");
        }
    }
}
