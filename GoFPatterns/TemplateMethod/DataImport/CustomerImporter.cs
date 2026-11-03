namespace GoFPatterns.TemplateMethod.DataImport
{
    // Abstrakte Klasse – definiert das Gerüst des Importalgorithmus in der Schablonenmethode Import().
    // Schritte, die sich je nach Format unterscheiden, sind abstrakt, optionale Schritte sind virtuelle Hooks.
    public abstract class CustomerImporter
    {
        // Schablonenmethode. Sie ist nicht virtuell, daher können Unterklassen die Reihenfolge der Schritte nicht ändern.
        public IReadOnlyList<Customer> Import(string rawData)
        {
            Console.WriteLine($"  [{GetType().Name}] Import gestartet");

            var content = Preprocess(rawData);
            var records = Parse(content);
            var valid = records.Where(Validate).ToList();
            Console.WriteLine($"  eingelesen: {records.Count}, gültig: {valid.Count}");

            Save(valid);
            AfterImport(valid);
            return valid;
        }

        // Hook – optionaler Schritt mit Standardimplementierung.
        protected virtual string Preprocess(string rawData) => rawData.Trim();

        // Abstrakter Schritt – jedes Format muss ihn implementieren.
        protected abstract List<Customer> Parse(string content);

        // Gemeinsamer Schritt – für alle Formate gleich, kann aber überschrieben werden.
        protected virtual bool Validate(Customer customer)
        {
            var isValid = customer.Email.Contains('@') && customer.Age is > 0 and < 120;
            if (!isValid)
                Console.WriteLine($"  ungültiger Datensatz übersprungen: {customer}");
            return isValid;
        }

        // Gemeinsamer Schritt – nicht überschreibbar.
        private static void Save(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"  gespeichert: {customer.Name} <{customer.Email}>, {customer.Age}");
            }
        }

        // Hook – standardmäßig leer.
        protected virtual void AfterImport(List<Customer> customers) { }
    }
}
