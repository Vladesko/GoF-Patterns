namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Manejador concreto: el último. Gestiona los incidentes críticos.
    public class ManagerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Critical)
            {
                Console.WriteLine($"  Responsable: inició el procedimiento de incidente para el ticket n.º {ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
