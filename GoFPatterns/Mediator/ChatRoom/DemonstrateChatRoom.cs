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

            anna.Send("Ciao a tutti!");
            ivan.SendPrivate("Anna", "Ciao, come stai?");
            olga.Send("Compra SPAM a poco prezzo qui");
            ivan.Send("/time");

            room.Leave(olga);
            anna.SendPrivate("Olga", "Ci sei?");
        }
    }
}
