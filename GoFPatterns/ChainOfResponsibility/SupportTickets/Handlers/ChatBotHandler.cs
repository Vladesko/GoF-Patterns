namespace GoFPatterns.ChainOfResponsibility.SupportTickets.Handlers
{
    // Manejador concreto: solo responde a las preguntas frecuentes.
    public class ChatBotHandler : SupportHandler
    {
        private static readonly string[] _faqKeywords = ["contraseña", "inicio de sesión", "plazo de entrega"];

        public override void Handle(SupportTicket ticket)
        {
            if (ticket.Severity == TicketSeverity.Low &&
                _faqKeywords.Any(k => ticket.Description.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"  ChatBot: ticket n.º {ticket.Id} respondido con un artículo de las FAQ");
                return;
            }
            base.Handle(ticket);
        }
    }
}
