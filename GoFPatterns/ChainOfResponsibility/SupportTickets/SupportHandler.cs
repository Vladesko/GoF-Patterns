namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Handler - base class which keeps reference to the next handler.
    // Default behavior: pass request to the next handler in the chain.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Returns next handler, so chain can be built fluently: a.SetNext(b).SetNext(c).
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public virtual void Handle(SupportTicket ticket)
        {
            if (_next is not null)
            {
                Console.WriteLine($"  {GetType().Name} -> passes to {_next.GetType().Name}");
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"  Nobody could handle ticket #{ticket.Id}");
            }
        }
    }
}
