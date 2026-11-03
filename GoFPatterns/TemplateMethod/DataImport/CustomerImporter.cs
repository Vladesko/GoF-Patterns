namespace GoFPatterns.TemplateMethod.DataImport
{
    // Classe astratta - definisce lo scheletro dell'algoritmo di importazione nel template method Import().
    // I passi che variano tra i formati sono astratti, i passi opzionali sono hook virtuali.
    public abstract class CustomerImporter
    {
        // Template method. Non è virtuale, quindi le sottoclassi non possono cambiare l'ordine dei passi.
        public IReadOnlyList<Customer> Import(string rawData)
        {
            Console.WriteLine($"  [{GetType().Name}] importazione avviata");

            var content = Preprocess(rawData);
            var records = Parse(content);
            var valid = records.Where(Validate).ToList();
            Console.WriteLine($"  letti: {records.Count}, validi: {valid.Count}");

            Save(valid);
            AfterImport(valid);
            return valid;
        }

        // Hook - passo opzionale con implementazione di default.
        protected virtual string Preprocess(string rawData) => rawData.Trim();

        // Passo astratto - ogni formato deve implementarlo.
        protected abstract List<Customer> Parse(string content);

        // Passo comune - uguale per tutti i formati, ma può essere ridefinito.
        protected virtual bool Validate(Customer customer)
        {
            var isValid = customer.Email.Contains('@') && customer.Age is > 0 and < 120;
            if (!isValid)
                Console.WriteLine($"  scartato record non valido: {customer}");
            return isValid;
        }

        // Passo comune - non ridefinibile.
        private static void Save(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"  salvato {customer.Name} <{customer.Email}>, {customer.Age}");
            }
        }

        // Hook - vuoto di default.
        protected virtual void AfterImport(List<Customer> customers) { }
    }
}
