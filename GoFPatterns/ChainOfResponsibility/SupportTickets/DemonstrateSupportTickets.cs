using GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers;

namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    public class DemonstrateSupportTickets
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Цепочка обязанностей: заявки в поддержку ===\n");

            // Цепочка: бот -> оператор -> инженер -> менеджер
            var chain = new ChatBotHandler();
            chain.SetNext(new OperatorHandler())
                 .SetNext(new EngineerHandler())
                 .SetNext(new ManagerHandler());

            SupportTicket[] tickets =
            [
                new(1, "Я забыл свой пароль", TicketSeverity.Low),
                new(2, "В моём заказе не тот товар", TicketSeverity.Medium),
                new(3, "Страница оплаты возвращает ошибку 500", TicketSeverity.High),
                new(4, "Сайт недоступен для всех пользователей", TicketSeverity.Critical)
            ];

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Заявка #{ticket.Id} [{ticket.Severity}]: {ticket.Description}");
                chain.Handle(ticket); // Клиент отправляет запрос только первому обработчику.
                Console.WriteLine();
            }

            // Цепочку можно настроить иначе - например, ночью без бота и менеджера.
            Console.WriteLine("Цепочка ночной смены (только оператор):");
            new OperatorHandler().Handle(new SupportTicket(5, "База данных повреждена", TicketSeverity.Critical));
        }
    }
}
