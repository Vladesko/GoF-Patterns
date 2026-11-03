namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Konkreter Handler – erste Support-Ebene.
    public class OperatorHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity <= TicketSeverity.Medium)
            {
                Console.WriteLine($"  Operator: Ticket Nr. {ticket.Id} gelöst");
                return;
            }
            base.Handle(ticket);
        }
    }
}
