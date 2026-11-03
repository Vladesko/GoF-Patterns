namespace GoFPatterns.Mediator.ChatRoom
{
    // Another colleague with its own reaction to messages.
    public class ChatBot(IChatMediator chat) : ChatUser("Bot", chat)
    {
        public override void Receive(string from, string message)
        {
            if (message.StartsWith("/time"))
                SendPrivate(from, $"Current time is {DateTime.Now:HH:mm}");
        }
    }
}
