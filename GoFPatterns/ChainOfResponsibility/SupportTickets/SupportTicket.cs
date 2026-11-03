namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Petición que recorre la cadena.
    public record SupportTicket(int Id, string Description, TicketSeverity Severity);
}
