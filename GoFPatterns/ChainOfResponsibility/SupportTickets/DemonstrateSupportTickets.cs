using GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers;

namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    public class DemonstrateSupportTickets
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Chain of Responsibility: ticket di supporto ===\n");

            // Catena: bot -> operatore -> ingegnere -> manager
            var chain = new ChatBotHandler();
            chain.SetNext(new OperatorHandler())
                 .SetNext(new EngineerHandler())
                 .SetNext(new ManagerHandler());

            SupportTicket[] tickets =
            [
                new(1, "Ho dimenticato la mia password", TicketSeverity.Low),
                new(2, "Articolo sbagliato nel mio ordine", TicketSeverity.Medium),
                new(3, "La pagina di pagamento restituisce l'errore 500", TicketSeverity.High),
                new(4, "Il sito non è raggiungibile per tutti gli utenti", TicketSeverity.Critical)
            ];

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Ticket #{ticket.Id} [{ticket.Severity}]: {ticket.Description}");
                chain.Handle(ticket); // Il client invia la richiesta solo al primo handler.
                Console.WriteLine();
            }

            // La catena può essere configurata diversamente - ad esempio, di notte senza bot e manager.
            Console.WriteLine("Catena del turno di notte (solo operatore):");
            new OperatorHandler().Handle(new SupportTicket(5, "Il database è danneggiato", TicketSeverity.Critical));
        }
    }
}
