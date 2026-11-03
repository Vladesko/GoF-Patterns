namespace GoFPatterns.Mediator.ChatRoom
{
    // Un altro colleague con una propria reazione ai messaggi.
    public class ChatBot(IChatMediator chat) : ChatUser("Bot", chat)
    {
        public override void Receive(string from, string message)
        {
            if (message.StartsWith("/time"))
                SendPrivate(from, $"Ora sono le {DateTime.Now:HH:mm}");
        }
    }
}
