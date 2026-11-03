using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Ogni demo: gruppo del pattern, nome mostrato nel menu e azione che la esegue.
(string Group, string Name, Action Run)[] demos =
[
    ("Creazionali", "Abstract Factory: Pagamenti", () => new GoFPatterns.AbtractFactory.Payments.DemontrateAbstractFactory().Demonstrate()),
    ("Creazionali", "Builder: Pizzeria", () => new GoFPatterns.Builder.PizzaShop.DemonstratePizzaBuilder().Demonstrate()),
    ("Creazionali", "Builder: Richiesta HTTP", () => new GoFPatterns.Builder.HttpRequest.DemonstrateHttpRequestBuilder().Demonstrate()),
    ("Creazionali", "Factory Method: Abbonamenti fitness", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateFitnes),
    ("Creazionali", "Factory Method: Notifiche", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateNotify),
    ("Creazionali", "Prototype: Documenti", () => new GoFPatterns.Prototype.Documents.DemonstratePrototype().Demonstrate()),
    ("Creazionali", "Prototype: Unità di gioco", () => new GoFPatterns.Prototype.GameUnits.DemonstrateGameUnits().Demonstrate()),
    ("Creazionali", "Singleton: Configurazione", () => new GoFPatterns.Singleton.Configuration.DemonstrateConfiguration().Demonstrate()),
    ("Creazionali", "Singleton: Logger", () => new GoFPatterns.Singleton.Logger.DemonstrateLogger().Demonstrate()),

    ("Strutturali", "Adapter: Sensori di temperatura", () => new GoFPatterns.Adapter.SensorFeature.DemonstarteAdapter().Demonstarte()),
    ("Strutturali", "Bridge: Telecomando", () => new GoFPatterns.Bridge.RemoteControl.DemonstrateRemoteControl().Demonstrate()),
    ("Strutturali", "Bridge: Report", () => new GoFPatterns.Bridge.Reports.DemonstrateReports().Demonstrate()),
    ("Strutturali", "Composite: File system", () => new GoFPatterns.Composite.FileSystem.DemonstrateFileSystem().Demonstrate()),
    ("Strutturali", "Composite: Struttura organizzativa", () => new GoFPatterns.Composite.OrgStructure.DemonstrateOrgStructure().Demonstrate()),
    ("Strutturali", "Decorator: Caffetteria", () => new GoFPatterns.Decorator.Coffee.DemonstrateCoffee().Demonstrate()),
    ("Strutturali", "Decorator: Sorgente dati", () => new GoFPatterns.Decorator.DataStream.DemonstrateDataStream().Demonstrate()),
    ("Strutturali", "Facade: Home theater", () => new GoFPatterns.Facade.HomeTheater.DemonstrateHomeTheater().Demonstrate()),
    ("Strutturali", "Facade: Invio dell'ordine", () => new GoFPatterns.Facade.OrderPlacement.DemonstrateOrderPlacement().Demonstrate()),
    ("Strutturali", "Flyweight: Foresta", () => new GoFPatterns.Flyweight.Forest.DemonstrateForest().Demonstrate()),
    ("Strutturali", "Flyweight: Formattazione del testo", () => new GoFPatterns.Flyweight.TextFormatting.DemonstrateTextFormatting().Demonstrate()),
    ("Strutturali", "Proxy: Caricamento lazy delle immagini", () => new GoFPatterns.Proxy.ImageLoading.DemonstrateImageLoading().Demonstrate()),
    ("Strutturali", "Proxy: Conto bancario", () => new GoFPatterns.Proxy.BankAccount.DemonstrateBankAccount().Demonstrate()),

    ("Comportamentali", "Chain of Responsibility: Ticket di supporto", () => new GoFPatterns.ChainOfResponsibility.SupportTickets.DemonstrateSupportTickets().Demonstrate()),
    ("Comportamentali", "Chain of Responsibility: Middleware", () => new GoFPatterns.ChainOfResponsibility.Middleware.DemonstrateRequestPipeline().Demonstrate()),
    ("Comportamentali", "Command: Editor di testo", () => new GoFPatterns.Command.TextEditor.DemonstrateTextEditor().Demonstrate()),
    ("Comportamentali", "Command: Casa intelligente", () => new GoFPatterns.Command.SmartHome.DemonstrateSmartHome().Demonstrate()),
    ("Comportamentali", "Interpreter: Espressioni matematiche", () => new GoFPatterns.Interpreter.MathExpressions.DemonstrateMathExpressions().Demonstrate()),
    ("Comportamentali", "Interpreter: Ricerca prodotti", () => new GoFPatterns.Interpreter.ProductSearch.DemonstrateProductSearch().Demonstrate()),
    ("Comportamentali", "Iterator: Collezione di libri", () => new GoFPatterns.Iterator.BookCollection.DemonstrateBookCollection().Demonstrate()),
    ("Comportamentali", "Iterator: Attraversamento di un albero", () => new GoFPatterns.Iterator.Tree.DemonstrateTree().Demonstrate()),
    ("Comportamentali", "Mediator: Chat room", () => new GoFPatterns.Mediator.ChatRoom.DemonstrateChatRoom().Demonstrate()),
    ("Comportamentali", "Mediator: Modulo di registrazione", () => new GoFPatterns.Mediator.RegistrationForm.DemonstrateRegistrationForm().Demonstrate()),
    ("Comportamentali", "Memento: Cronologia dell'editor di testo", () => new GoFPatterns.Memento.TextEditorHistory.DemonstrateTextEditorHistory().Demonstrate()),
    ("Comportamentali", "Memento: Salvataggi di gioco", () => new GoFPatterns.Memento.GameSave.DemonstrateGameSave().Demonstrate()),
    ("Comportamentali", "Observer: Prezzo del prodotto", () => new GoFPatterns.Observer.ProductObserver.ProductObserverDemonstrate().Demonstrate()),
    ("Comportamentali", "State: Ciclo di vita dell'ordine", () => new GoFPatterns.State.OrderLifecycle.DemonstrateOrderLifecycle().Demonstrate()),
    ("Comportamentali", "State: Distributore automatico", () => new GoFPatterns.State.Vending.DemonstrateVendingMachine().Demonstrate()),
    ("Comportamentali", "Strategy: Costo di consegna", () => new GoFPatterns.Strategy.Delivery.DemonstrateDelivery().Demonstrate()),
    ("Comportamentali", "Strategy: Ordinamento", () => new GoFPatterns.Strategy.Sorting.DemonstrateSorting().Demonstrate()),
    ("Comportamentali", "Template Method: Importazione dati", () => new GoFPatterns.TemplateMethod.DataImport.DemonstrateDataImport().Demonstrate()),
    ("Comportamentali", "Template Method: Bevande calde", () => new GoFPatterns.TemplateMethod.Beverages.DemonstrateBeverages().Demonstrate()),
    ("Comportamentali", "Visitor: Forme geometriche", () => new GoFPatterns.Visitor.Shapes.ShapeDemostrateVisitor().Demonstrate()),
];

while (true)
{
    PrintMenu();
    Console.Write("Scegli il numero della demo (0 - uscita): ");
    var input = Console.ReadLine();

    // null significa che il flusso di input è chiuso (ad esempio, l'input è stato reindirizzato da un file).
    if (input is null || input.Trim() == "0")
        break;

    if (!int.TryParse(input, out var number) || number < 1 || number > demos.Length)
    {
        Console.WriteLine("Numero non valido, riprova.\n");
        continue;
    }

    Console.WriteLine();
    try
    {
        demos[number - 1].Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Demo non riuscita: {ex.Message}");
    }
    Console.WriteLine();
}

void PrintMenu()
{
    Console.WriteLine("=== Design pattern GoF ===");
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
