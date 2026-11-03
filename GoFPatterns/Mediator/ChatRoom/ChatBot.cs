namespace GoFPatterns.Mediator.ChatRoom
{
    // Ещё один коллега со своей реакцией на сообщения.
    public class ChatBot(IChatMediator chat) : ChatUser("Бот", chat)
    {
        public override void Receive(string from, string message)
        {
            if (message.StartsWith("/time"))
                SendPrivate(from, $"Текущее время: {DateTime.Now:HH:mm}");
        }
    }
}
