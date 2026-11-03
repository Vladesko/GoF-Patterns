namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Concrete handler - second line of support, technical problems.
    public class EngineerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.High)
            {
                Console.WriteLine($"  Engineer: fixed the bug from ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
