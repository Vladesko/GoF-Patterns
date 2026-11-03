using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Every demo: group of pattern, name shown in menu and action which runs it.
(string Group, string Name, Action Run)[] demos =
[
    ("Creational", "Abstract Factory: Payments", () => new GoFPatterns.AbtractFactory.Payments.DemontrateAbstractFactory().Demonstrate()),
    ("Creational", "Builder: Pizza shop", () => new GoFPatterns.Builder.PizzaShop.DemonstratePizzaBuilder().Demonstrate()),
    ("Creational", "Builder: HTTP request", () => new GoFPatterns.Builder.HttpRequest.DemonstrateHttpRequestBuilder().Demonstrate()),
    ("Creational", "Factory Method: Fitness memberships", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateFitnes),
    ("Creational", "Factory Method: Notifications", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateNotify),
    ("Creational", "Prototype: Documents", () => new GoFPatterns.Prototype.Documents.DemonstratePrototype().Demonstrate()),
    ("Creational", "Prototype: Game units", () => new GoFPatterns.Prototype.GameUnits.DemonstrateGameUnits().Demonstrate()),
    ("Creational", "Singleton: Configuration", () => new GoFPatterns.Singleton.Configuration.DemonstrateConfiguration().Demonstrate()),
    ("Creational", "Singleton: Logger", () => new GoFPatterns.Singleton.Logger.DemonstrateLogger().Demonstrate()),

    ("Structural", "Adapter: Temperature sensors", () => new GoFPatterns.Adapter.SensorFeature.DemonstarteAdapter().Demonstarte()),
    ("Structural", "Bridge: Remote control", () => new GoFPatterns.Bridge.RemoteControl.DemonstrateRemoteControl().Demonstrate()),
    ("Structural", "Bridge: Reports", () => new GoFPatterns.Bridge.Reports.DemonstrateReports().Demonstrate()),
    ("Structural", "Composite: File system", () => new GoFPatterns.Composite.FileSystem.DemonstrateFileSystem().Demonstrate()),
    ("Structural", "Composite: Organization structure", () => new GoFPatterns.Composite.OrgStructure.DemonstrateOrgStructure().Demonstrate()),
    ("Structural", "Decorator: Coffee shop", () => new GoFPatterns.Decorator.Coffee.DemonstrateCoffee().Demonstrate()),
    ("Structural", "Decorator: Data source", () => new GoFPatterns.Decorator.DataStream.DemonstrateDataStream().Demonstrate()),
    ("Structural", "Facade: Home theater", () => new GoFPatterns.Facade.HomeTheater.DemonstrateHomeTheater().Demonstrate()),
    ("Structural", "Facade: Order placement", () => new GoFPatterns.Facade.OrderPlacement.DemonstrateOrderPlacement().Demonstrate()),
    ("Structural", "Flyweight: Forest", () => new GoFPatterns.Flyweight.Forest.DemonstrateForest().Demonstrate()),
    ("Structural", "Flyweight: Text formatting", () => new GoFPatterns.Flyweight.TextFormatting.DemonstrateTextFormatting().Demonstrate()),
    ("Structural", "Proxy: Lazy image loading", () => new GoFPatterns.Proxy.ImageLoading.DemonstrateImageLoading().Demonstrate()),
    ("Structural", "Proxy: Bank account", () => new GoFPatterns.Proxy.BankAccount.DemonstrateBankAccount().Demonstrate()),

    ("Behavioral", "Chain of Responsibility: Support tickets", () => new GoFPatterns.ChainOfResponsibility.SupportTickets.DemonstrateSupportTickets().Demonstrate()),
    ("Behavioral", "Chain of Responsibility: Middleware", () => new GoFPatterns.ChainOfResponsibility.Middleware.DemonstrateRequestPipeline().Demonstrate()),
    ("Behavioral", "Command: Text editor", () => new GoFPatterns.Command.TextEditor.DemonstrateTextEditor().Demonstrate()),
    ("Behavioral", "Command: Smart home", () => new GoFPatterns.Command.SmartHome.DemonstrateSmartHome().Demonstrate()),
    ("Behavioral", "Interpreter: Math expressions", () => new GoFPatterns.Interpreter.MathExpressions.DemonstrateMathExpressions().Demonstrate()),
    ("Behavioral", "Interpreter: Product search", () => new GoFPatterns.Interpreter.ProductSearch.DemonstrateProductSearch().Demonstrate()),
    ("Behavioral", "Iterator: Book collection", () => new GoFPatterns.Iterator.BookCollection.DemonstrateBookCollection().Demonstrate()),
    ("Behavioral", "Iterator: Tree traversal", () => new GoFPatterns.Iterator.Tree.DemonstrateTree().Demonstrate()),
    ("Behavioral", "Mediator: Chat room", () => new GoFPatterns.Mediator.ChatRoom.DemonstrateChatRoom().Demonstrate()),
    ("Behavioral", "Mediator: Registration form", () => new GoFPatterns.Mediator.RegistrationForm.DemonstrateRegistrationForm().Demonstrate()),
    ("Behavioral", "Memento: Text editor history", () => new GoFPatterns.Memento.TextEditorHistory.DemonstrateTextEditorHistory().Demonstrate()),
    ("Behavioral", "Memento: Game saves", () => new GoFPatterns.Memento.GameSave.DemonstrateGameSave().Demonstrate()),
    ("Behavioral", "Observer: Product price", () => new GoFPatterns.Observer.ProductObserver.ProductObserverDemonstrate().Demonstrate()),
    ("Behavioral", "State: Order lifecycle", () => new GoFPatterns.State.OrderLifecycle.DemonstrateOrderLifecycle().Demonstrate()),
    ("Behavioral", "State: Vending machine", () => new GoFPatterns.State.Vending.DemonstrateVendingMachine().Demonstrate()),
    ("Behavioral", "Strategy: Delivery cost", () => new GoFPatterns.Strategy.Delivery.DemonstrateDelivery().Demonstrate()),
    ("Behavioral", "Strategy: Sorting", () => new GoFPatterns.Strategy.Sorting.DemonstrateSorting().Demonstrate()),
    ("Behavioral", "Template Method: Data import", () => new GoFPatterns.TemplateMethod.DataImport.DemonstrateDataImport().Demonstrate()),
    ("Behavioral", "Template Method: Hot beverages", () => new GoFPatterns.TemplateMethod.Beverages.DemonstrateBeverages().Demonstrate()),
    ("Behavioral", "Visitor: Shapes", () => new GoFPatterns.Visitor.Shapes.ShapeDemostrateVisitor().Demonstrate()),
];

while (true)
{
    PrintMenu();
    Console.Write("Choose demo number (0 - exit): ");
    var input = Console.ReadLine();

    // null means that input stream is closed (for example, input was redirected from file).
    if (input is null || input.Trim() == "0")
        break;

    if (!int.TryParse(input, out var number) || number < 1 || number > demos.Length)
    {
        Console.WriteLine("Wrong number, try again.\n");
        continue;
    }

    Console.WriteLine();
    try
    {
        demos[number - 1].Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Demo failed: {ex.Message}");
    }
    Console.WriteLine();
}

void PrintMenu()
{
    Console.WriteLine("=== GoF Design Patterns ===");
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
