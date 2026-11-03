namespace GoFPatterns.Prototype.Documents
{
    // Registro dei prototype - conserva prototype preconfigurati e ne restituisce copie per chiave.
    // Il client ottiene un nuovo documento dal modello senza sapere come è stato configurato.
    public class DocumentTemplateRegistry
    {
        private readonly Dictionary<string, IDocumentPrototype> _templates = [];

        public void Register(string key, IDocumentPrototype template)
        {
            ArgumentNullException.ThrowIfNull(template);
            _templates[key] = template;
        }

        public IDocumentPrototype Create(string key)
        {
            if (!_templates.TryGetValue(key, out var template))
                throw new KeyNotFoundException($"Il modello '{key}' non è registrato.");

            return template.Clone();
        }
    }
}
