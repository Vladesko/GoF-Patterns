namespace GoFPatterns.Builder.HttpRequest
{
    // Product - richiesta immutabile. Può essere creata solo tramite il builder.
    public class HttpRequestMessageModel
    {
        public string Method { get; }
        public string Url { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }
        public IReadOnlyDictionary<string, string> QueryParameters { get; }
        public string? Body { get; }
        public TimeSpan Timeout { get; }

        internal HttpRequestMessageModel(
            string method,
            string url,
            IReadOnlyDictionary<string, string> headers,
            IReadOnlyDictionary<string, string> queryParameters,
            string? body,
            TimeSpan timeout)
        {
            Method = method;
            Url = url;
            Headers = headers;
            QueryParameters = queryParameters;
            Body = body;
            Timeout = timeout;
        }

        public override string ToString()
        {
            var query = QueryParameters.Count == 0
                ? string.Empty
                : "?" + string.Join("&", QueryParameters.Select(p => $"{p.Key}={p.Value}"));
            var headers = string.Join(Environment.NewLine, Headers.Select(h => $"  {h.Key}: {h.Value}"));
            var body = Body is null ? string.Empty : $"{Environment.NewLine}  Corpo: {Body}";
            return $"{Method} {Url}{query} (timeout {Timeout.TotalSeconds}s){Environment.NewLine}{headers}{body}";
        }
    }
}
