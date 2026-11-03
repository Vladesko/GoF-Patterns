namespace GoFPatterns.Mediator.ChatRoom
{
    // Konkreter Vermittler – enthält die gesamte Kommunikationslogik: wer die Nachricht erhält,
    // Filtern verbotener Wörter, private Nachrichten, Benachrichtigungen über Betreten/Verlassen.
    public class ChatRoomMediator : IChatMediator
    {
        private static readonly string[] _bannedWords = ["spam", "scam"];
        private readonly Dictionary<string, ChatUser> _users = [];

        public void Join(ChatUser user)
        {
            _users[user.Name] = user;
            Console.WriteLine($"  [Raum] {user.Name} ist beigetreten");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user.Name);
            Console.WriteLine($"  [Raum] {user.Name} hat den Raum verlassen");
        }

        public void SendMessage(ChatUser sender, string message)
        {
            Console.WriteLine($"  {sender.Name}: {message}");
            if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  [Raum] Nachricht von {sender.Name} wurde blockiert");
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
                receiver.Receive($"{sender.Name} (privat)", message);
            else
                sender.Receive("Raum", $"Benutzer {receiverName} ist nicht im Raum");
        }
    }
}
