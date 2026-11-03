namespace GoFPatterns.Mediator.ChatRoom
{
    // Concrete mediator - contains all logic of communication: who receives the message,
    // filtering of banned words, private messages, join/leave notifications.
    public class ChatRoomMediator : IChatMediator
    {
        private static readonly string[] _bannedWords = ["spam", "scam"];
        private readonly Dictionary<string, ChatUser> _users = [];

        public void Join(ChatUser user)
        {
            _users[user.Name] = user;
            Console.WriteLine($"  [room] {user.Name} joined");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user.Name);
            Console.WriteLine($"  [room] {user.Name} left");
        }

        public void SendMessage(ChatUser sender, string message)
        {
            Console.WriteLine($"  {sender.Name}: {message}");
            if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  [room] message from {sender.Name} is blocked");
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
                receiver.Receive($"{sender.Name} (private)", message);
            else
                sender.Receive("room", $"user {receiverName} is not in the room");
        }
    }
}
