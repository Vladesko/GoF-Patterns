namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Concrete handler - the last one. Handles critical incidents.
    public class ManagerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Critical)
            {
                Console.WriteLine($"  Manager: started incident procedure for ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
