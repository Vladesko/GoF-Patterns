namespace GoFPatterns.ChainOfResponsibility.SupportTickets
{
    // Anfrage, die entlang der Kette wandert.
    public record SupportTicket(int Id, string Description, TicketSeverity Severity);
}
