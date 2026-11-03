using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Jede Demo: Mustergruppe, im Menü angezeigter Name und die Aktion, die sie startet.
(string Group, string Name, Action Run)[] demos =
[
    ("Erzeugungsmuster", "Abstract Factory: Zahlungen", () => new GoFPatterns.AbtractFactory.Payments.DemontrateAbstractFactory().Demonstrate()),
    ("Erzeugungsmuster", "Builder: Pizzeria", () => new GoFPatterns.Builder.PizzaShop.DemonstratePizzaBuilder().Demonstrate()),
    ("Erzeugungsmuster", "Builder: HTTP-Anfrage", () => new GoFPatterns.Builder.HttpRequest.DemonstrateHttpRequestBuilder().Demonstrate()),
    ("Erzeugungsmuster", "Factory Method: Fitness-Mitgliedschaften", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateFitnes),
    ("Erzeugungsmuster", "Factory Method: Benachrichtigungen", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateNotify),
    ("Erzeugungsmuster", "Prototype: Dokumente", () => new GoFPatterns.Prototype.Documents.DemonstratePrototype().Demonstrate()),
    ("Erzeugungsmuster", "Prototype: Spieleinheiten", () => new GoFPatterns.Prototype.GameUnits.DemonstrateGameUnits().Demonstrate()),
    ("Erzeugungsmuster", "Singleton: Konfiguration", () => new GoFPatterns.Singleton.Configuration.DemonstrateConfiguration().Demonstrate()),
    ("Erzeugungsmuster", "Singleton: Logger", () => new GoFPatterns.Singleton.Logger.DemonstrateLogger().Demonstrate()),

    ("Strukturmuster", "Adapter: Temperatursensoren", () => new GoFPatterns.Adapter.SensorFeature.DemonstarteAdapter().Demonstarte()),
    ("Strukturmuster", "Bridge: Fernbedienung", () => new GoFPatterns.Bridge.RemoteControl.DemonstrateRemoteControl().Demonstrate()),
    ("Strukturmuster", "Bridge: Berichte", () => new GoFPatterns.Bridge.Reports.DemonstrateReports().Demonstrate()),
    ("Strukturmuster", "Composite: Dateisystem", () => new GoFPatterns.Composite.FileSystem.DemonstrateFileSystem().Demonstrate()),
    ("Strukturmuster", "Composite: Organisationsstruktur", () => new GoFPatterns.Composite.OrgStructure.DemonstrateOrgStructure().Demonstrate()),
    ("Strukturmuster", "Decorator: Café", () => new GoFPatterns.Decorator.Coffee.DemonstrateCoffee().Demonstrate()),
    ("Strukturmuster", "Decorator: Datenquelle", () => new GoFPatterns.Decorator.DataStream.DemonstrateDataStream().Demonstrate()),
    ("Strukturmuster", "Facade: Heimkino", () => new GoFPatterns.Facade.HomeTheater.DemonstrateHomeTheater().Demonstrate()),
    ("Strukturmuster", "Facade: Bestellabwicklung", () => new GoFPatterns.Facade.OrderPlacement.DemonstrateOrderPlacement().Demonstrate()),
    ("Strukturmuster", "Flyweight: Wald", () => new GoFPatterns.Flyweight.Forest.DemonstrateForest().Demonstrate()),
    ("Strukturmuster", "Flyweight: Textformatierung", () => new GoFPatterns.Flyweight.TextFormatting.DemonstrateTextFormatting().Demonstrate()),
    ("Strukturmuster", "Proxy: Verzögertes Laden von Bildern", () => new GoFPatterns.Proxy.ImageLoading.DemonstrateImageLoading().Demonstrate()),
    ("Strukturmuster", "Proxy: Bankkonto", () => new GoFPatterns.Proxy.BankAccount.DemonstrateBankAccount().Demonstrate()),

    ("Verhaltensmuster", "Chain of Responsibility: Support-Tickets", () => new GoFPatterns.ChainOfResponsibility.SupportTickets.DemonstrateSupportTickets().Demonstrate()),
    ("Verhaltensmuster", "Chain of Responsibility: Middleware", () => new GoFPatterns.ChainOfResponsibility.Middleware.DemonstrateRequestPipeline().Demonstrate()),
    ("Verhaltensmuster", "Command: Texteditor", () => new GoFPatterns.Command.TextEditor.DemonstrateTextEditor().Demonstrate()),
    ("Verhaltensmuster", "Command: Smart Home", () => new GoFPatterns.Command.SmartHome.DemonstrateSmartHome().Demonstrate()),
    ("Verhaltensmuster", "Interpreter: Mathematische Ausdrücke", () => new GoFPatterns.Interpreter.MathExpressions.DemonstrateMathExpressions().Demonstrate()),
    ("Verhaltensmuster", "Interpreter: Produktsuche", () => new GoFPatterns.Interpreter.ProductSearch.DemonstrateProductSearch().Demonstrate()),
    ("Verhaltensmuster", "Iterator: Büchersammlung", () => new GoFPatterns.Iterator.BookCollection.DemonstrateBookCollection().Demonstrate()),
    ("Verhaltensmuster", "Iterator: Baumdurchlauf", () => new GoFPatterns.Iterator.Tree.DemonstrateTree().Demonstrate()),
    ("Verhaltensmuster", "Mediator: Chatraum", () => new GoFPatterns.Mediator.ChatRoom.DemonstrateChatRoom().Demonstrate()),
    ("Verhaltensmuster", "Mediator: Registrierungsformular", () => new GoFPatterns.Mediator.RegistrationForm.DemonstrateRegistrationForm().Demonstrate()),
    ("Verhaltensmuster", "Memento: Verlauf im Texteditor", () => new GoFPatterns.Memento.TextEditorHistory.DemonstrateTextEditorHistory().Demonstrate()),
    ("Verhaltensmuster", "Memento: Spielstände", () => new GoFPatterns.Memento.GameSave.DemonstrateGameSave().Demonstrate()),
    ("Verhaltensmuster", "Observer: Produktpreis", () => new GoFPatterns.Observer.ProductObserver.ProductObserverDemonstrate().Demonstrate()),
    ("Verhaltensmuster", "State: Lebenszyklus einer Bestellung", () => new GoFPatterns.State.OrderLifecycle.DemonstrateOrderLifecycle().Demonstrate()),
    ("Verhaltensmuster", "State: Verkaufsautomat", () => new GoFPatterns.State.Vending.DemonstrateVendingMachine().Demonstrate()),
    ("Verhaltensmuster", "Strategy: Lieferkosten", () => new GoFPatterns.Strategy.Delivery.DemonstrateDelivery().Demonstrate()),
    ("Verhaltensmuster", "Strategy: Sortierung", () => new GoFPatterns.Strategy.Sorting.DemonstrateSorting().Demonstrate()),
    ("Verhaltensmuster", "Template Method: Datenimport", () => new GoFPatterns.TemplateMethod.DataImport.DemonstrateDataImport().Demonstrate()),
    ("Verhaltensmuster", "Template Method: Heißgetränke", () => new GoFPatterns.TemplateMethod.Beverages.DemonstrateBeverages().Demonstrate()),
    ("Verhaltensmuster", "Visitor: Formen", () => new GoFPatterns.Visitor.Shapes.ShapeDemostrateVisitor().Demonstrate()),
];

while (true)
{
    PrintMenu();
    Console.Write("Demo-Nummer wählen (0 – Beenden): ");
    var input = Console.ReadLine();

    // null bedeutet, dass der Eingabestrom geschlossen ist (z. B. wenn die Eingabe aus einer Datei umgeleitet wurde).
    if (input is null || input.Trim() == "0")
        break;

    if (!int.TryParse(input, out var number) || number < 1 || number > demos.Length)
    {
        Console.WriteLine("Ungültige Nummer, bitte erneut versuchen.\n");
        continue;
    }

    Console.WriteLine();
    try
    {
        demos[number - 1].Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Demo fehlgeschlagen: {ex.Message}");
    }
    Console.WriteLine();
}

void PrintMenu()
{
    Console.WriteLine("=== GoF-Entwurfsmuster ===");
    string? group = null;
    for (var i = 0; i < demos.Length; i++)
    {
        if (demos[i].Group != group)
        {
            group = demos[i].Group;
            Console.WriteLine($"\n  {group}:");
        }
        Console.WriteLine($"  {i + 1,3}. {demos[i].Name}");
    }
    Console.WriteLine();
}
