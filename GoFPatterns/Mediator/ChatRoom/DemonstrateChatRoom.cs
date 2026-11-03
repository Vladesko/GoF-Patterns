namespace GoFPatterns.Mediator.ChatRoom
{
    public class DemonstrateChatRoom
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Mediator: Chatraum ===\n");

            var room = new ChatRoomMediator();
            var anna = new ChatUser("Anna", room);
            var ivan = new ChatUser("Ivan", room);
            var olga = new ChatUser("Olga", room);
            var bot = new ChatBot(room);

            room.Join(anna);
            room.Join(ivan);
            room.Join(olga);
            room.Join(bot);

            anna.Send("Hallo zusammen!");
            ivan.SendPrivate("Anna", "Hallo, wie geht's?");
            olga.Send("Kauft hier billigen SPAM");
            ivan.Send("/time");

            room.Leave(olga);
            anna.SendPrivate("Olga", "Bist du da?");
        }
    }
}
