using GoFPatterns.Prototype.Documents.Prototypes;

namespace GoFPatterns.Prototype.Documents
{
    public class DemonstratePrototype
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: Documents ===\n");

            DemonstrateShallowAndDeepCopy();
            Console.WriteLine();
            DemonstrateRegistry();
        }

        private static void DemonstrateShallowAndDeepCopy()
        {
            var original = new Contract
            {
                Title = "Service agreement",
                Company = "Horns & Hooves",
                Style = new DocumentStyle { Font = "Times New Roman", FontSize = 14 },
                Sections = ["Subject", "Price", "Responsibility"]
            };

            var shallow = original.ShallowClone();
            var deep = (Contract)original.Clone();

            // Change nested objects of the copies.
            shallow.Style.FontSize = 20;
            shallow.Sections.Add("Added in shallow copy");
            deep.Style.Font = "Calibri";
            deep.Sections.Add("Added in deep copy");

            Console.WriteLine("Shallow copy shares Style and Sections with original:");
            Console.WriteLine($"  Original: {original}");
            Console.WriteLine($"  Shallow:  {shallow}");
            Console.WriteLine("Deep copy is independent:");
            Console.WriteLine($"  Deep:     {deep}");
        }

        private static void DemonstrateRegistry()
        {
            // Templates are configured once ...
            var registry = new DocumentTemplateRegistry();
            registry.Register("nda", new Contract
            {
                Title = "NDA",
                Company = "<company>",
                Sections = ["Confidential information", "Term", "Penalties"]
            });
            registry.Register("invoice-eu", new Invoice { Title = "EU invoice", TaxRate = 0.2m });

            // ... and then copied many times without repeating the configuration.
            var ndaForAcme = (Contract)registry.Create("nda");
            ndaForAcme.Company = "Acme";

            var ndaForGlobex = (Contract)registry.Create("nda");
            ndaForGlobex.Company = "Globex";
            ndaForGlobex.Sections.Add("Jurisdiction");

            var invoice = (Invoice)registry.Create("invoice-eu");
            invoice.Lines.Add(("Laptop", 1000m));
            invoice.Lines.Add(("Mouse", 25m));

            Console.WriteLine("Documents created from registry:");
            Console.WriteLine($"  {ndaForAcme}");
            Console.WriteLine($"  {ndaForGlobex}");
            Console.WriteLine($"  {invoice}");
            Console.WriteLine($"  Template is unchanged: {registry.Create("nda")}");
        }
    }
}
