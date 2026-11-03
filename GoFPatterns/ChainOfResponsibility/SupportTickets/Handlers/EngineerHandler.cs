namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Конкретный обработчик - вторая линия поддержки, технические проблемы.
    public class EngineerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.High)
            {
                Console.WriteLine($"  Инженер: исправил ошибку из заявки #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
