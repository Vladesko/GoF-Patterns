namespace GoFPatterns.Mediator.ChatRoom
{
    public class DemonstrateChatRoom
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Mediator: Chat room ===\n");

            var room = new ChatRoomMediator();
            var anna = new ChatUser("Anna", room);
            var ivan = new ChatUser("Ivan", room);
            var olga = new ChatUser("Olga", room);
            var bot = new ChatBot(room);

            room.Join(anna);
            room.Join(ivan);
            room.Join(olga);
            room.Join(bot);

            anna.Send("Hi everyone!");
            ivan.SendPrivate("Anna", "Hi, how are you?");
            olga.Send("Buy cheap SPAM here");
            ivan.Send("/time");

            room.Leave(olga);
            anna.SendPrivate("Olga", "Are you here?");
        }
    }
}
