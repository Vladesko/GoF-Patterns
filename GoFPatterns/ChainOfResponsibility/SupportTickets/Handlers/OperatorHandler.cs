namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Конкретный обработчик - первая линия поддержки.
    public class OperatorHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity <= TicketSeverity.Medium)
            {
                Console.WriteLine($"  Оператор: решил заявку #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
