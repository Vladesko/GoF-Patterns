namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Concrete handler - first line of support.
    public class OperatorHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity <= TicketSeverity.Medium)
            {
                Console.WriteLine($"  Operator: resolved ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
