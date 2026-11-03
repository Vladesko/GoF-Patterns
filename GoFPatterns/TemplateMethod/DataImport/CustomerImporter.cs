namespace GoFPatterns.TemplateMethod.DataImport
{
    // Clase abstracta: define el esqueleto del algoritmo de importación en el método plantilla Import().
    // Los pasos que difieren entre formatos son abstractos; los pasos opcionales son hooks virtuales.
    public abstract class CustomerImporter
    {
        // Método plantilla. No es virtual, así que las subclases no pueden cambiar el orden de los pasos.
        public IReadOnlyList<Customer> Import(string rawData)
        {
            Console.WriteLine($"  [{GetType().Name}] importación iniciada");

            var content = Preprocess(rawData);
            var records = Parse(content);
            var valid = records.Where(Validate).ToList();
            Console.WriteLine($"  analizados: {records.Count}, válidos: {valid.Count}");

            Save(valid);
            AfterImport(valid);
            return valid;
        }

        // Hook: paso opcional con implementación por defecto.
        protected virtual string Preprocess(string rawData) => rawData.Trim();

        // Paso abstracto: cada formato debe implementarlo.
        protected abstract List<Customer> Parse(string content);

        // Paso común: igual para todos los formatos, pero se puede sobrescribir.
        protected virtual bool Validate(Customer customer)
        {
            var isValid = customer.Email.Contains('@') && customer.Age is > 0 and < 120;
            if (!isValid)
                Console.WriteLine($"  registro no válido omitido: {customer}");
            return isValid;
        }

        // Paso común: no se puede sobrescribir.
        private static void Save(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"  guardado {customer.Name} <{customer.Email}>, {customer.Age}");
            }
        }

        // Hook: vacío por defecto.
        protected virtual void AfterImport(List<Customer> customers) { }
    }
}
