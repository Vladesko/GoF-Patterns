namespace GoFPatterns.TemplateMethod.DataImport
{
    // Абстрактный класс - определяет скелет алгоритма импорта в шаблонном методе Import().
    // Шаги, которые различаются для разных форматов, абстрактные; необязательные шаги - виртуальные хуки.
    public abstract class CustomerImporter
    {
        // Шаблонный метод. Он не виртуальный, поэтому подклассы не могут изменить порядок шагов.
        public IReadOnlyList<Customer> Import(string rawData)
        {
            Console.WriteLine($"  [{GetType().Name}] импорт начат");

            var content = Preprocess(rawData);
            var records = Parse(content);
            var valid = records.Where(Validate).ToList();
            Console.WriteLine($"  разобрано: {records.Count}, корректных: {valid.Count}");

            Save(valid);
            AfterImport(valid);
            return valid;
        }

        // Хук - необязательный шаг с реализацией по умолчанию.
        protected virtual string Preprocess(string rawData) => rawData.Trim();

        // Абстрактный шаг - каждый формат обязан его реализовать.
        protected abstract List<Customer> Parse(string content);

        // Общий шаг - одинаковый для всех форматов, но может быть переопределён.
        protected virtual bool Validate(Customer customer)
        {
            var isValid = customer.Email.Contains('@') && customer.Age is > 0 and < 120;
            if (!isValid)
                Console.WriteLine($"  пропущена некорректная запись: {customer}");
            return isValid;
        }

        // Общий шаг - не переопределяется.
        private static void Save(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"  сохранён {customer.Name} <{customer.Email}>, {customer.Age}");
            }
        }

        // Хук - по умолчанию пустой.
        protected virtual void AfterImport(List<Customer> customers) { }
    }
}
