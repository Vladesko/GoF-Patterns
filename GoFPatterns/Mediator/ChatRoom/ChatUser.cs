namespace GoFPatterns.Mediator.ChatRoom
{
    // Коллега - знает только посредника и не знает других пользователей.
    public class ChatUser(string name, IChatMediator chat)
    {
        public string Name { get; } = name;

        public void Send(string message) => chat.SendMessage(this, message);

        public void SendPrivate(string receiverName, string message) => chat.SendPrivate(this, receiverName, message);

        public virtual void Receive(string from, string message) =>
            Console.WriteLine($"    {Name} получает от {from}: {message}");
    }
}
