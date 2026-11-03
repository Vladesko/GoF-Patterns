namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Handler concreto - secondo livello di supporto, problemi tecnici.
    public class EngineerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.High)
            {
                Console.WriteLine($"  Ingegnere: ha corretto il bug del ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
