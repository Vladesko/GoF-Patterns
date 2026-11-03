namespace GoFPatterns.Mediator.ChatRoom
{
    // Mediator concreto - contiene tutta la logica di comunicazione: chi riceve il messaggio,
    // il filtro delle parole vietate, i messaggi privati, le notifiche di ingresso/uscita.
    public class ChatRoomMediator : IChatMediator
    {
        private static readonly string[] _bannedWords = ["spam", "scam"];
        private readonly Dictionary<string, ChatUser> _users = [];

        public void Join(ChatUser user)
        {
            _users[user.Name] = user;
            Console.WriteLine($"  [stanza] {user.Name} è entrato");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user.Name);
            Console.WriteLine($"  [stanza] {user.Name} è uscito");
        }

        public void SendMessage(ChatUser sender, string message)
        {
            Console.WriteLine($"  {sender.Name}: {message}");
            if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  [stanza] il messaggio di {sender.Name} è bloccato");
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
                receiver.Receive($"{sender.Name} (privato)", message);
            else
                sender.Receive("stanza", $"l'utente {receiverName} non è nella stanza");
        }
    }
}
