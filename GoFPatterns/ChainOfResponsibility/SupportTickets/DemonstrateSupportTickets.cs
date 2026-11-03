using GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers;

namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    public class DemonstrateSupportTickets
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Chain of Responsibility: Support tickets ===\n");

            // Chain: bot -> operator -> engineer -> manager
            var chain = new ChatBotHandler();
            chain.SetNext(new OperatorHandler())
                 .SetNext(new EngineerHandler())
                 .SetNext(new ManagerHandler());

            SupportTicket[] tickets =
            [
                new(1, "I forgot my password", TicketSeverity.Low),
                new(2, "Wrong item in my order", TicketSeverity.Medium),
                new(3, "Payment page returns error 500", TicketSeverity.High),
                new(4, "Site is down for all users", TicketSeverity.Critical)
            ];

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Ticket #{ticket.Id} [{ticket.Severity}]: {ticket.Description}");
                chain.Handle(ticket); // Client sends request only to the first handler.
                Console.WriteLine();
            }

            // Chain can be configured differently - for example, without bot and manager at night.
            Console.WriteLine("Night shift chain (only operator):");
            new OperatorHandler().Handle(new SupportTicket(5, "Database is corrupted", TicketSeverity.Critical));
        }
    }
}
