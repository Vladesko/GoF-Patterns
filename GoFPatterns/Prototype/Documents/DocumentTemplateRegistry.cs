namespace GoFPatterns.Prototype.Documents
{
    // Prototyp-Registry – speichert vorkonfigurierte Prototypen und liefert deren Kopien per Schlüssel.
    // Der Client erhält ein neues Dokument aus der Vorlage, ohne zu wissen, wie sie konfiguriert wurde.
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
                throw new KeyNotFoundException($"Vorlage '{key}' ist nicht registriert.");

            return template.Clone();
        }
    }
}
