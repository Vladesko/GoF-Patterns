namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Handler concreto - primo livello di supporto.
    public class OperatorHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity <= TicketSeverity.Medium)
            {
                Console.WriteLine($"  Operatore: ha risolto il ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
