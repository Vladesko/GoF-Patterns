namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Manejador concreto: segunda línea de soporte, problemas técnicos.
    public class EngineerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.High)
            {
                Console.WriteLine($"  Ingeniero: corrigió el error del ticket n.º {ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
