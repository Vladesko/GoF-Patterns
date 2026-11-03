namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Objeto de petición (y respuesta) que se pasa a través de la cadena.
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
