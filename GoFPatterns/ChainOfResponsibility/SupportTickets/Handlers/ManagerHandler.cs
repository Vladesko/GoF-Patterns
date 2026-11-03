namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Konkreter Handler – der letzte. Bearbeitet kritische Störungen.
    public class ManagerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Critical)
            {
                Console.WriteLine($"  Manager: Störungsprozess für Ticket Nr. {ticket.Id} gestartet");
                return;
            }
            base.Handle(ticket);
        }
    }
}
