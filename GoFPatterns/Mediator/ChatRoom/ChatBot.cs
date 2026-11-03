namespace GoFPatterns.Mediator.ChatRoom
{
    // Ein weiterer Kollege mit eigener Reaktion auf Nachrichten.
    public class ChatBot(IChatMediator chat) : ChatUser("Bot", chat)
    {
        public override void Receive(string from, string message)
        {
            if (message.StartsWith("/time"))
                SendPrivate(from, $"Aktuelle Uhrzeit: {DateTime.Now:HH:mm}");
        }
    }
}
