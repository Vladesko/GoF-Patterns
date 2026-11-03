using GoFPatterns.Prototype.Documents.Prototypes;

namespace GoFPatterns.Prototype.Documents
{
    public class DemonstratePrototype
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: Documenti ===\n");

            DemonstrateShallowAndDeepCopy();
            Console.WriteLine();
            DemonstrateRegistry();
        }

        private static void DemonstrateShallowAndDeepCopy()
        {
            var original = new Contract
            {
                Title = "Contratto di servizio",
                Company = "Corna & Zoccoli",
                Style = new DocumentStyle { Font = "Times New Roman", FontSize = 14 },
                Sections = ["Oggetto", "Prezzo", "Responsabilità"]
            };

            var shallow = original.ShallowClone();
            var deep = (Contract)original.Clone();

            // Modifica degli oggetti annidati delle copie.
            shallow.Style.FontSize = 20;
            shallow.Sections.Add("Aggiunta nella copia superficiale");
            deep.Style.Font = "Calibri";
            deep.Sections.Add("Aggiunta nella copia profonda");

            Console.WriteLine("La copia superficiale condivide Style e Sections con l'originale:");
            Console.WriteLine($"  Originale:    {original}");
            Console.WriteLine($"  Superficiale: {shallow}");
            Console.WriteLine("La copia profonda è indipendente:");
            Console.WriteLine($"  Profonda:     {deep}");
        }

        private static void DemonstrateRegistry()
        {
            // I modelli vengono configurati una sola volta ...
            var registry = new DocumentTemplateRegistry();
            registry.Register("nda", new Contract
            {
                Title = "NDA",
                Company = "<azienda>",
                Sections = ["Informazioni riservate", "Durata", "Penali"]
            });
            registry.Register("invoice-eu", new Invoice { Title = "Fattura UE", TaxRate = 0.2m });

            // ... e poi copiati molte volte senza ripetere la configurazione.
            var ndaForAcme = (Contract)registry.Create("nda");
            ndaForAcme.Company = "Acme";

            var ndaForGlobex = (Contract)registry.Create("nda");
            ndaForGlobex.Company = "Globex";
            ndaForGlobex.Sections.Add("Foro competente");

            var invoice = (Invoice)registry.Create("invoice-eu");
            invoice.Lines.Add(("Portatile", 1000m));
            invoice.Lines.Add(("Mouse", 25m));

            Console.WriteLine("Documenti creati dal registro:");
            Console.WriteLine($"  {ndaForAcme}");
            Console.WriteLine($"  {ndaForGlobex}");
            Console.WriteLine($"  {invoice}");
            Console.WriteLine($"  Il modello è invariato: {registry.Create("nda")}");
        }
    }
}
