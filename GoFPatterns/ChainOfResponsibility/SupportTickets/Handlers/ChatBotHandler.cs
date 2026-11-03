namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Concrete handler - answers frequently asked questions only.
    public class ChatBotHandler : SupportHandler
    {
        private static readonly string[] _faqKeywords = ["password", "login", "delivery time"];

        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Low &&
                _faqKeywords.Any(k => ticket.Description.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  ChatBot: answered ticket #{ticket.Id} with FAQ article");
                return;
            }
            base.Handle(ticket);
        }
    }
}
