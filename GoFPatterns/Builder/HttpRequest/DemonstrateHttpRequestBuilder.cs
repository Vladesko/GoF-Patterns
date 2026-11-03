namespace GoFPatterns.Builder.HttpRequest
{
    public class DemonstrateHttpRequestBuilder
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Builder: petición HTTP fluida ===\n");

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

            // El Builder nos protege de crear un objeto no válido.
            try
            {
                new HttpRequestBuilder().WithHeader("Accept", "text/html").Build();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nLa petición no válida no se construyó: {ex.Message}");
            }
        }
    }
}
