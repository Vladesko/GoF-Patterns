namespace GoFPatterns.Mediator.ChatRoom
{
    // Colega: solo conoce al mediador, no conoce a los demás usuarios.
    public class ChatUser(string name, IChatMediator chat)
    {
        public string Name { get; } = name;

        public void Send(string message) => chat.SendMessage(this, message);

        public void SendPrivate(string receiverName, string message) => chat.SendPrivate(this, receiverName, message);

        public virtual void Receive(string from, string message) =>
            Console.WriteLine($"    {Name} recibió de {from}: {message}");
    }
}
