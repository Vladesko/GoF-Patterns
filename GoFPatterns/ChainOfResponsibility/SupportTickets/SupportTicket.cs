namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Richiesta che viaggia lungo la catena.
    public record SupportTicket(int Id, string Description, TicketSeverity Severity);
}
