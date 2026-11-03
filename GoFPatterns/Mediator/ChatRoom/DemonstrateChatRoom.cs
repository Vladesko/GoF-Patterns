namespace GoFPatterns.Mediator.ChatRoom
{
    public class DemonstrateChatRoom
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Посредник: чат-комната ===\n");

            var room = new ChatRoomMediator();
            var anna = new ChatUser("Анна", room);
            var ivan = new ChatUser("Иван", room);
            var olga = new ChatUser("Ольга", room);
            var bot = new ChatBot(room);

            room.Join(anna);
            room.Join(ivan);
            room.Join(olga);
            room.Join(bot);

            anna.Send("Всем привет!");
            ivan.SendPrivate("Анна", "Привет, как дела?");
            olga.Send("Дёшево купить SPAM здесь");
            ivan.Send("/time");

            room.Leave(olga);
            anna.SendPrivate("Ольга", "Ты здесь?");
        }
    }
}
