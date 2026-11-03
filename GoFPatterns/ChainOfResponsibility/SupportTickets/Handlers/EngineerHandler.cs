namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Konkreter Handler – zweite Support-Ebene, technische Probleme.
    public class EngineerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.High)
            {
                Console.WriteLine($"  Ingenieur: Fehler aus Ticket Nr. {ticket.Id} behoben");
                return;
            }
            base.Handle(ticket);
        }
    }
}
