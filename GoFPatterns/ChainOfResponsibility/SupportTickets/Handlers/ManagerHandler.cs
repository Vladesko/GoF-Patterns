namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Конкретный обработчик - последний. Обрабатывает критические инциденты.
    public class ManagerHandler : SupportHandler
    {
        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Critical)
            {
                Console.WriteLine($"  Менеджер: запустил процедуру реагирования на инцидент по заявке #{ticket.Id}");
                return;
            }
            base.Handle(ticket);
        }
    }
}
