namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Handler concreto - risponde solo alle domande frequenti.
    public class ChatBotHandler : SupportHandler
    {
        private static readonly string[] _faqKeywords = ["password", "accesso", "tempi di consegna"];

        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Low &&
                _faqKeywords.Any(k => ticket.Description.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  ChatBot: ha risposto al ticket #{ticket.Id} con un articolo delle FAQ");
                return;
            }
            base.Handle(ticket);
        }
    }
}
