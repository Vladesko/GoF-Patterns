namespace GoFPatterns.Mediator.ChatRoom
{
    // Colleague - knows only mediator, doesn't know other users.
    public class ChatUser(string name, IChatMediator chat)
    {
        public string Name { get; } = name;

        public void Send(string message) => chat.SendMessage(this, message);

        public void SendPrivate(string receiverName, string message) => chat.SendPrivate(this, receiverName, message);

        public virtual void Receive(string from, string message) =>
            Console.WriteLine($"    {Name} got from {from}: {message}");
    }
}
