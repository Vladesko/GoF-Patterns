namespace GoFPatterns.Prototype.Documents
{
    // Реестр прототипов - хранит заранее настроенные прототипы и возвращает их копии по ключу.
    // Клиент получает новый документ из шаблона, не зная, как тот был настроен.
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
                throw new KeyNotFoundException($"Шаблон '{key}' не зарегистрирован.");

            return template.Clone();
        }
    }
}
