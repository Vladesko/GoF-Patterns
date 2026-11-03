namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Konkreter Handler – beantwortet nur häufig gestellte Fragen.
    public class ChatBotHandler : SupportHandler
    {
        private static readonly string[] _faqKeywords = ["Passwort", "Login", "Lieferzeit"];

        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Low &&
                _faqKeywords.Any(k => ticket.Description.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  ChatBot: Ticket Nr. {ticket.Id} mit FAQ-Artikel beantwortet");
                return;
            }
            base.Handle(ticket);
        }
    }
}
