using GoFPatterns.Prototype.Documents.Prototypes;

namespace GoFPatterns.Prototype.Documents
{
    public class DemonstratePrototype
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: documentos ===\n");

            DemonstrateShallowAndDeepCopy();
            Console.WriteLine();
            DemonstrateRegistry();
        }

        private static void DemonstrateShallowAndDeepCopy()
        {
            var original = new Contract
            {
                Title = "Contrato de servicios",
                Company = "Cuernos y Pezuñas",
                Style = new DocumentStyle { Font = "Times New Roman", FontSize = 14 },
                Sections = ["Objeto", "Precio", "Responsabilidad"]
            };

            var shallow = original.ShallowClone();
            var deep = (Contract)original.Clone();

            // Modificamos los objetos anidados de las copias.
            shallow.Style.FontSize = 20;
            shallow.Sections.Add("Añadida en la copia superficial");
            deep.Style.Font = "Calibri";
            deep.Sections.Add("Añadida en la copia profunda");

            Console.WriteLine("La copia superficial comparte Style y Sections con el original:");
            Console.WriteLine($"  Original:    {original}");
            Console.WriteLine($"  Superficial: {shallow}");
            Console.WriteLine("La copia profunda es independiente:");
            Console.WriteLine($"  Profunda:    {deep}");
        }

        private static void DemonstrateRegistry()
        {
            // Las plantillas se configuran una sola vez...
            var registry = new DocumentTemplateRegistry();
            registry.Register("nda", new Contract
            {
                Title = "NDA",
                Company = "<company>",
                Sections = ["Información confidencial", "Plazo", "Penalizaciones"]
            });
            registry.Register("invoice-eu", new Invoice { Title = "Factura UE", TaxRate = 0.2m });

            // ...y después se copian muchas veces sin repetir la configuración.
            var ndaForAcme = (Contract)registry.Create("nda");
            ndaForAcme.Company = "Acme";

            var ndaForGlobex = (Contract)registry.Create("nda");
            ndaForGlobex.Company = "Globex";
            ndaForGlobex.Sections.Add("Jurisdicción");

            var invoice = (Invoice)registry.Create("invoice-eu");
            invoice.Lines.Add(("Portátil", 1000m));
            invoice.Lines.Add(("Ratón", 25m));

            Console.WriteLine("Documentos creados a partir del registro:");
            Console.WriteLine($"  {ndaForAcme}");
            Console.WriteLine($"  {ndaForGlobex}");
            Console.WriteLine($"  {invoice}");
            Console.WriteLine($"  La plantilla no ha cambiado: {registry.Create("nda")}");
        }
    }
}
