namespace GoFPatterns.Prototype.Documents
{
    // Prototype registry - stores preconfigured prototypes and returns their copies by key.
    // Client gets a new document from template without knowing how it was configured.
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
                throw new KeyNotFoundException($"Template '{key}' is not registered.");

            return template.Clone();
        }
    }
}
