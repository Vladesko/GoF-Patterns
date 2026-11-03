using GoFPatterns.Prototype.Documents.Prototypes;

namespace GoFPatterns.Prototype.Documents
{
    public class DemonstratePrototype
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: Dokumente ===\n");

            DemonstrateShallowAndDeepCopy();
            Console.WriteLine();
            DemonstrateRegistry();
        }

        private static void DemonstrateShallowAndDeepCopy()
        {
            var original = new Contract
            {
                Title = "Dienstleistungsvertrag",
                Company = "Hörner & Hufe",
                Style = new DocumentStyle { Font = "Times New Roman", FontSize = 14 },
                Sections = ["Gegenstand", "Preis", "Haftung"]
            };

            var shallow = original.ShallowClone();
            var deep = (Contract)original.Clone();

            // Verschachtelte Objekte der Kopien ändern.
            shallow.Style.FontSize = 20;
            shallow.Sections.Add("In flacher Kopie hinzugefügt");
            deep.Style.Font = "Calibri";
            deep.Sections.Add("In tiefer Kopie hinzugefügt");

            Console.WriteLine("Die flache Kopie teilt Style und Sections mit dem Original:");
            Console.WriteLine($"  Original: {original}");
            Console.WriteLine($"  Flach:    {shallow}");
            Console.WriteLine("Die tiefe Kopie ist unabhängig:");
            Console.WriteLine($"  Tief:     {deep}");
        }

        private static void DemonstrateRegistry()
        {
            // Vorlagen werden einmal konfiguriert ...
            var registry = new DocumentTemplateRegistry();
            registry.Register("nda", new Contract
            {
                Title = "NDA",
                Company = "<Firma>",
                Sections = ["Vertrauliche Informationen", "Laufzeit", "Vertragsstrafen"]
            });
            registry.Register("invoice-eu", new Invoice { Title = "EU-Rechnung", TaxRate = 0.2m });

            // ... und dann beliebig oft kopiert, ohne die Konfiguration zu wiederholen.
            var ndaForAcme = (Contract)registry.Create("nda");
            ndaForAcme.Company = "Acme";

            var ndaForGlobex = (Contract)registry.Create("nda");
            ndaForGlobex.Company = "Globex";
            ndaForGlobex.Sections.Add("Gerichtsstand");

            var invoice = (Invoice)registry.Create("invoice-eu");
            invoice.Lines.Add(("Laptop", 1000m));
            invoice.Lines.Add(("Maus", 25m));

            Console.WriteLine("Aus der Registry erzeugte Dokumente:");
            Console.WriteLine($"  {ndaForAcme}");
            Console.WriteLine($"  {ndaForGlobex}");
            Console.WriteLine($"  {invoice}");
            Console.WriteLine($"  Die Vorlage ist unverändert: {registry.Create("nda")}");
        }
    }
}
