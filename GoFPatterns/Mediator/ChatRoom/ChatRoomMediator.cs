namespace GoFPatterns.Mediator.ChatRoom
{
    // Mediador concreto: contiene toda la lógica de comunicación: quién recibe el mensaje,
    // el filtrado de palabras prohibidas, los mensajes privados y los avisos de entrada/salida.
    public class ChatRoomMediator : IChatMediator
    {
        private static readonly string[] _bannedWords = ["spam", "scam"];
        private readonly Dictionary<string, ChatUser> _users = [];

        public void Join(ChatUser user)
        {
            _users[user.Name] = user;
            Console.WriteLine($"  [sala] {user.Name} se ha unido");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user.Name);
            Console.WriteLine($"  [sala] {user.Name} ha salido");
        }

        public void SendMessage(ChatUser sender, string message)
        {
            Console.WriteLine($"  {sender.Name}: {message}");
            if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  [sala] el mensaje de {sender.Name} está bloqueado");
                return;
            }

            foreach (var user in _users.Values.Where(u => u != sender))
            {
                user.Receive(sender.Name, message);
            }
        }

        public void SendPrivate(ChatUser sender, string receiverName, string message)
        {
            if (_users.TryGetValue(receiverName, out var receiver))
                receiver.Receive($"{sender.Name} (privado)", message);
            else
                sender.Receive("sala", $"el usuario {receiverName} no está en la sala");
        }
    }
}
