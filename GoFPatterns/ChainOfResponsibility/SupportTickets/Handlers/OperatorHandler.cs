namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Manejador concreto: primera línea de soporte.
    public class OperatorHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity <= TicketSeverity.Medium)
            {
                Console.WriteLine($"  Operador: resolvió el ticket n.º {ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
