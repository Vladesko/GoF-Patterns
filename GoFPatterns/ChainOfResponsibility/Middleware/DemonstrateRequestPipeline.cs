using GoFPatterns.ChainOfResponsibility.Middleware.Handlers;

namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    public class DemonstrateRequestPipeline
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Цепочка обязанностей: конвейер обработки запросов (middleware) ===\n");

            var pipeline = new RequestPipeline()
                .Use(new LoggingHandler())
                .Use(new RateLimitHandler(maxRequests: 2))
                .Use(new AuthenticationHandler())
                .Use(new ValidationHandler())
                .Use(new EndpointHandler());

            HttpRequestContext[] requests =
            [
                new() { Path = "/orders", ClientIp = "10.0.0.1", Token = "token-anna", Body = "{ \"item\": 1 }" },
                new() { Path = "/orders", ClientIp = "10.0.0.2", Token = "fake-token", Body = "{ }" },
                new() { Path = "/orders", ClientIp = "10.0.0.3", Token = "token-ivan" },
                new() { Path = "/orders", ClientIp = "10.0.0.1", Token = "token-anna", Body = "{ \"item\": 2 }" },
                new() { Path = "/orders", ClientIp = "10.0.0.1", Token = "token-anna", Body = "{ \"item\": 3 }" }
            ];

            foreach (var request in requests)
            {
                pipeline.Execute(request);
                Console.WriteLine();
            }
        }
    }
}
