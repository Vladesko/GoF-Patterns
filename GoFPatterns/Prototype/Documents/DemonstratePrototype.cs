using GoFPatterns.Prototype.Documents.Prototypes;

namespace GoFPatterns.Prototype.Documents
{
    public class DemonstratePrototype
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Прототип: документы ===\n");

            DemonstrateShallowAndDeepCopy();
            Console.WriteLine();
            DemonstrateRegistry();
        }

        private static void DemonstrateShallowAndDeepCopy()
        {
            var original = new Contract
            {
                Title = "Договор оказания услуг",
                Company = "Рога и копыта",
                Style = new DocumentStyle { Font = "Times New Roman", FontSize = 14 },
                Sections = ["Предмет договора", "Стоимость", "Ответственность"]
            };

            var shallow = original.ShallowClone();
            var deep = (Contract)original.Clone();

            // Изменяем вложенные объекты копий.
            shallow.Style.FontSize = 20;
            shallow.Sections.Add("Добавлено в поверхностной копии");
            deep.Style.Font = "Calibri";
            deep.Sections.Add("Добавлено в глубокой копии");

            Console.WriteLine("Поверхностная копия разделяет Style и Sections с оригиналом:");
            Console.WriteLine($"  Оригинал:      {original}");
            Console.WriteLine($"  Поверхностная: {shallow}");
            Console.WriteLine("Глубокая копия независима:");
            Console.WriteLine($"  Глубокая:      {deep}");
        }

        private static void DemonstrateRegistry()
        {
            // Шаблоны настраиваются один раз ...
            var registry = new DocumentTemplateRegistry();
            registry.Register("nda", new Contract
            {
                Title = "Соглашение о неразглашении (NDA)",
                Company = "<компания>",
                Sections = ["Конфиденциальная информация", "Срок действия", "Штрафы"]
            });
            registry.Register("invoice-eu", new Invoice { Title = "Счёт для ЕС", TaxRate = 0.2m });

            // ... а затем многократно копируются без повторения настройки.
            var ndaForAcme = (Contract)registry.Create("nda");
            ndaForAcme.Company = "Acme";

            var ndaForGlobex = (Contract)registry.Create("nda");
            ndaForGlobex.Company = "Globex";
            ndaForGlobex.Sections.Add("Юрисдикция");

            var invoice = (Invoice)registry.Create("invoice-eu");
            invoice.Lines.Add(("Ноутбук", 1000m));
            invoice.Lines.Add(("Мышь", 25m));

            Console.WriteLine("Документы, созданные из реестра:");
            Console.WriteLine($"  {ndaForAcme}");
            Console.WriteLine($"  {ndaForGlobex}");
            Console.WriteLine($"  {invoice}");
            Console.WriteLine($"  Шаблон не изменился: {registry.Create("nda")}");
        }
    }
}
