namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Handler - classe base che mantiene il riferimento all'handler successivo.
    // Comportamento predefinito: passare la richiesta all'handler successivo della catena.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Restituisce l'handler successivo, così la catena può essere costruita in modo fluente: a.SetNext(b).SetNext(c).
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public virtual void Handle(SupportTicket ticket)
        {
            if (_next is not null)
            {
                Console.WriteLine($"  {GetType().Name} -> passa a {_next.GetType().Name}");
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"  Nessuno ha potuto gestire il ticket #{ticket.Id}");
            }
        }
    }
}
