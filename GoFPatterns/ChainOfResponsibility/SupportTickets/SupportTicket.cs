namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Запрос, который передаётся по цепочке.
    public record SupportTicket(int Id, string Description, TicketSeverity Severity);
}
