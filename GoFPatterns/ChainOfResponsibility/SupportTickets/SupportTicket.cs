namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Request which travels along the chain.
    public record SupportTicket(int Id, string Description, TicketSeverity Severity);
}
