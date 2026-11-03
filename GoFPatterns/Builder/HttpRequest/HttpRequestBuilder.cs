namespace GoFPatterns.Builder.HttpRequest
{
    // Builder fluido: cada paso devuelve el propio builder, por lo que las llamadas se pueden encadenar.
    // Build() valida los datos recopilados y crea un producto inmutable.
    public class HttpRequestBuilder
    {
        private string _method = "GET";
        private string? _url;
        private readonly Dictionary<string, string> _headers = [];
        private readonly Dictionary<string, string> _queryParameters = [];
        private string? _body;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);

        public HttpRequestBuilder Get(string url) => WithMethod("GET", url);

        public HttpRequestBuilder Post(string url) => WithMethod("POST", url);

        public HttpRequestBuilder WithHeader(string name, string value)
        {
            _headers[name] = value;
            return this;
        }

        public HttpRequestBuilder WithBearerToken(string token) =>
            WithHeader("Authorization", $"Bearer {token}");

        public HttpRequestBuilder WithQuery(string name, string value)
        {
            _queryParameters[name] = value;
            return this;
        }

        public HttpRequestBuilder WithJsonBody(string json)
        {
            _body = json;
            return WithHeader("Content-Type", "application/json");
        }

        public HttpRequestBuilder WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public HttpRequestMessageModel Build()
        {
            if (string.IsNullOrWhiteSpace(_url))
                throw new InvalidOperationException("La URL es obligatoria.");

            if (_method == "GET" && _body is not null)
                throw new InvalidOperationException("Una petición GET no puede tener cuerpo.");

            return new HttpRequestMessageModel(
                _method,
                _url,
                new Dictionary<string, string>(_headers),
                new Dictionary<string, string>(_queryParameters),
                _body,
                _timeout);
        }

        private HttpRequestBuilder WithMethod(string method, string url)
        {
            _method = method;
            _url = url;
            return this;
        }
    }
}
