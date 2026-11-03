namespace GoFPatterns.TemplateMethod.DataImport
{
    // Abstract class - defines the skeleton of import algorithm in the template method Import().
    // Steps which differ between formats are abstract, optional steps are virtual hooks.
    public abstract class CustomerImporter
    {
        // Template method. It is not virtual, so subclasses can't change the order of steps.
        public IReadOnlyList<Customer> Import(string rawData)
        {
            Console.WriteLine($"  [{GetType().Name}] import started");

            var content = Preprocess(rawData);
            var records = Parse(content);
            var valid = records.Where(Validate).ToList();
            Console.WriteLine($"  parsed: {records.Count}, valid: {valid.Count}");

            Save(valid);
            AfterImport(valid);
            return valid;
        }

        // Hook - optional step with default implementation.
        protected virtual string Preprocess(string rawData) => rawData.Trim();

        // Abstract step - each format must implement it.
        protected abstract List<Customer> Parse(string content);

        // Common step - the same for all formats, but can be overridden.
        protected virtual bool Validate(Customer customer)
        {
            var isValid = customer.Email.Contains('@') && customer.Age is > 0 and < 120;
            if (!isValid)
                Console.WriteLine($"  skipped invalid record: {customer}");
            return isValid;
        }

        // Common step - not overridable.
        private static void Save(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"  saved {customer.Name} <{customer.Email}>, {customer.Age}");
            }
        }

        // Hook - empty by default.
        protected virtual void AfterImport(List<Customer> customers) { }
    }
}
