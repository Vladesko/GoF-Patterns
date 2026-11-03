namespace GoFPatterns.Mediator.ChatRoom
{
    public class DemonstrateChatRoom
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Mediator: sala de chat ===\n");

            var room = new ChatRoomMediator();
            var anna = new ChatUser("Anna", room);
            var ivan = new ChatUser("Ivan", room);
            var olga = new ChatUser("Olga", room);
            var bot = new ChatBot(room);

            room.Join(anna);
            room.Join(ivan);
            room.Join(olga);
            room.Join(bot);

            anna.Send("¡Hola a todos!");
            ivan.SendPrivate("Anna", "Hola, ¿qué tal?");
            olga.Send("Compra SPAM barato aquí");
            ivan.Send("/time");

            room.Leave(olga);
            anna.SendPrivate("Olga", "¿Estás ahí?");
        }
    }
}
