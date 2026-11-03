namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Конкретный обработчик - отвечает только на часто задаваемые вопросы.
    public class ChatBotHandler : SupportHandler
    {
        private static readonly string[] _faqKeywords = ["пароль", "логин", "время доставки"];

        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Low &&
                _faqKeywords.Any(k => ticket.Description.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  Чат-бот: ответил на заявку #{ticket.Id} статьёй из FAQ");
                return;
            }
            base.Handle(ticket);
        }
    }
}
