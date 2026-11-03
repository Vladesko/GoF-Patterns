namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Handler concreto - l'ultimo. Gestisce gli incidenti critici.
    public class ManagerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Critical)
            {
                Console.WriteLine($"  Manager: ha avviato la procedura di gestione incidenti per il ticket #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
