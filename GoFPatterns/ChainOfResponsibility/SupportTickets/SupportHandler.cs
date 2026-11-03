namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Handler – Basisklasse, die eine Referenz auf den nächsten Handler hält.
    // Standardverhalten: die Anfrage an den nächsten Handler der Kette weitergeben.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Gibt den nächsten Handler zurück, damit die Kette fließend aufgebaut werden kann: a.SetNext(b).SetNext(c).
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public virtual void Handle(SupportTicket ticket)
        {
            if (_next is not null)
            {
                Console.WriteLine($"  {GetType().Name} -> gibt weiter an {_next.GetType().Name}");
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"  Niemand konnte Ticket Nr. {ticket.Id} bearbeiten");
            }
        }
    }
}
