using GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers;

namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    public class DemonstrateSupportTickets
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Chain of Responsibility: Support-Tickets ===\n");

            // Kette: Bot -> Operator -> Ingenieur -> Manager
            var chain = new ChatBotHandler();
            chain.SetNext(new OperatorHandler())
                 .SetNext(new EngineerHandler())
                 .SetNext(new ManagerHandler());

            SupportTicket[] tickets =
            [
                new(1, "Ich habe mein Passwort vergessen", TicketSeverity.Low),
                new(2, "Falscher Artikel in meiner Bestellung", TicketSeverity.Medium),
                new(3, "Die Zahlungsseite liefert Fehler 500", TicketSeverity.High),
                new(4, "Die Website ist für alle Benutzer down", TicketSeverity.Critical)
            ];

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Ticket Nr. {ticket.Id} [{ticket.Severity}]: {ticket.Description}");
                chain.Handle(ticket); // Der Client sendet die Anfrage nur an den ersten Handler.
                Console.WriteLine();
            }

            // Die Kette lässt sich anders konfigurieren – z. B. nachts ohne Bot und Manager.
            Console.WriteLine("Kette der Nachtschicht (nur Operator):");
            new OperatorHandler().Handle(new SupportTicket(5, "Die Datenbank ist beschädigt", TicketSeverity.Critical));
        }
    }
}
