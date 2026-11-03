namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Обработчик - базовый класс, который хранит ссылку на следующий обработчик.
    // Поведение по умолчанию: передать запрос следующему обработчику в цепочке.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Возвращает следующий обработчик, поэтому цепочку можно строить в fluent-стиле: a.SetNext(b).SetNext(c).
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public virtual void Handle(SupportTicket ticket)
        {
            if (_next is not null)
            {
                Console.WriteLine($"  {GetType().Name} -> передаёт заявку {_next.GetType().Name}");
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"  Никто не смог обработать заявку #{ticket.Id}");
            }
        }
    }
}
