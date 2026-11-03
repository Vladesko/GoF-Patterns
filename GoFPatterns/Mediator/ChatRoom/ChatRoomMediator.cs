namespace GoFPatterns.Mediator.ChatRoom
{
    // Конкретный посредник - содержит всю логику общения: кто получает сообщение,
    // фильтрацию запрещённых слов, личные сообщения, уведомления о входе и выходе.
    public class ChatRoomMediator : IChatMediator
    {
        private static readonly string[] _bannedWords = ["spam", "scam"];
        private readonly Dictionary<string, ChatUser> _users = [];

        public void Join(ChatUser user)
        {
            _users[user.Name] = user;
            Console.WriteLine($"  [комната] {user.Name} входит в чат");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user.Name);
            Console.WriteLine($"  [комната] {user.Name} выходит из чата");
        }

        public void SendMessage(ChatUser sender, string message)
        {
            Console.WriteLine($"  {sender.Name}: {message}");
            if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  [комната] сообщение от {sender.Name} заблокировано");
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
                receiver.Receive($"{sender.Name} (лично)", message);
            else
                sender.Receive("комната", $"пользователя {receiverName} нет в комнате");
        }
    }
}
