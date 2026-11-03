namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Manejador: clase base que guarda una referencia al siguiente manejador.
    // Comportamiento por defecto: pasar la petición al siguiente manejador de la cadena.
    public abstract class SupportHandler
    {
        private SupportHandler? _next;

        // Devuelve el siguiente manejador, para poder construir la cadena de forma fluida: a.SetNext(b).SetNext(c).
        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public virtual void Handle(SupportTicket ticket)
        {
            if (_next is not null)
            {
                Console.WriteLine($"  {GetType().Name} -> pasa a {_next.GetType().Name}");
                _next.Handle(ticket);
            }
            else
            {
                Console.WriteLine($"  Nadie pudo gestionar el ticket n.º {ticket.Id}");
            }
        }
    }
}
