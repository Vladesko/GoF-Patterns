namespace GoFPatterns.Builder.HttpRequest
{
    public class DemonstrateHttpRequestBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: Fluent-HTTP-Anfrage ===\n");

            var searchRequest = new HttpRequestBuilder()
                .Get("https://api.shop.com/products")
                .WithQuery("category", "phones")
                .WithQuery("page", "2")
                .WithHeader("Accept", "application/json")
                .Build();
            Console.WriteLine(searchRequest);

            Console.WriteLine();

            var createOrderRequest = new HttpRequestBuilder()
                .Post("https://api.shop.com/orders")
                .WithBearerToken("secret-token")
                .WithJsonBody("{ \"productId\": 42, \"count\": 1 }")
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Build();
            Console.WriteLine(createOrderRequest);

            // Der Builder schützt uns davor, ein ungültiges Objekt zu erzeugen.
            try
            {
                new HttpRequestBuilder().WithHeader("Accept", "text/html").Build();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nUngültige Anfrage wurde nicht erstellt: {ex.Message}");
            }
        }
    }
}
