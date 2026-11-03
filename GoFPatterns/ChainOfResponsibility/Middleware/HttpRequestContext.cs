namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Объект запроса (и ответа), который передаётся по цепочке.
    public class HttpRequestContext
    {
        public required string Path { get; init; }
        public required string ClientIp { get; init; }
        public string? Token { get; init; }
        public string? Body { get; init; }

        public string? UserName { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Response { get; set; } = string.Empty;
    }
}
