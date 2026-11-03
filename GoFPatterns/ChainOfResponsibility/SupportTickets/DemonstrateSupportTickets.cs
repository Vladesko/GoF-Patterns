using GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers;

namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    public class DemonstrateSupportTickets
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Chain of Responsibility: tickets de soporte ===\n");

            // Cadena: bot -> operador -> ingeniero -> responsable
            var chain = new ChatBotHandler();
            chain.SetNext(new OperatorHandler())
                 .SetNext(new EngineerHandler())
                 .SetNext(new ManagerHandler());

            SupportTicket[] tickets =
            [
                new(1, "He olvidado mi contraseña", TicketSeverity.Low),
                new(2, "Artículo equivocado en mi pedido", TicketSeverity.Medium),
                new(3, "La página de pago devuelve el error 500", TicketSeverity.High),
                new(4, "El sitio no funciona para ningún usuario", TicketSeverity.Critical)
            ];

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Ticket n.º {ticket.Id} [{ticket.Severity}]: {ticket.Description}");
                chain.Handle(ticket); // El cliente envía la petición solo al primer manejador.
                Console.WriteLine();
            }

            // La cadena se puede configurar de otra forma; por ejemplo, de noche sin bot ni responsable.
            Console.WriteLine("Cadena del turno de noche (solo operador):");
            new OperatorHandler().Handle(new SupportTicket(5, "La base de datos está dañada", TicketSeverity.Critical));
        }
    }
}
