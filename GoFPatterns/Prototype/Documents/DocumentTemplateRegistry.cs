namespace GoFPatterns.Prototype.Documents
{
    // Registro de prototipos: almacena prototipos preconfigurados y devuelve sus copias por clave.
    // El cliente obtiene un documento nuevo a partir de una plantilla sin saber cómo se configuró.
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
                throw new KeyNotFoundException($"La plantilla '{key}' no está registrada.");

            return template.Clone();
        }
    }
}
