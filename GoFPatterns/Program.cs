using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Cada demo: grupo del patrón, nombre mostrado en el menú y acción que la ejecuta.
(string Group, string Name, Action Run)[] demos =
[
    ("Creacionales", "Abstract Factory: Pagos", () => new GoFPatterns.AbtractFactory.Payments.DemontrateAbstractFactory().Demonstrate()),
    ("Creacionales", "Builder: Pizzería", () => new GoFPatterns.Builder.PizzaShop.DemonstratePizzaBuilder().Demonstrate()),
    ("Creacionales", "Builder: Petición HTTP", () => new GoFPatterns.Builder.HttpRequest.DemonstrateHttpRequestBuilder().Demonstrate()),
    ("Creacionales", "Factory Method: Abonos de gimnasio", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateFitnes),
    ("Creacionales", "Factory Method: Notificaciones", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateNotify),
    ("Creacionales", "Prototype: Documentos", () => new GoFPatterns.Prototype.Documents.DemonstratePrototype().Demonstrate()),
    ("Creacionales", "Prototype: Unidades de juego", () => new GoFPatterns.Prototype.GameUnits.DemonstrateGameUnits().Demonstrate()),
    ("Creacionales", "Singleton: Configuración", () => new GoFPatterns.Singleton.Configuration.DemonstrateConfiguration().Demonstrate()),
    ("Creacionales", "Singleton: Logger", () => new GoFPatterns.Singleton.Logger.DemonstrateLogger().Demonstrate()),

    ("Estructurales", "Adapter: Sensores de temperatura", () => new GoFPatterns.Adapter.SensorFeature.DemonstarteAdapter().Demonstarte()),
    ("Estructurales", "Bridge: Mando a distancia", () => new GoFPatterns.Bridge.RemoteControl.DemonstrateRemoteControl().Demonstrate()),
    ("Estructurales", "Bridge: Informes", () => new GoFPatterns.Bridge.Reports.DemonstrateReports().Demonstrate()),
    ("Estructurales", "Composite: Sistema de archivos", () => new GoFPatterns.Composite.FileSystem.DemonstrateFileSystem().Demonstrate()),
    ("Estructurales", "Composite: Estructura organizativa", () => new GoFPatterns.Composite.OrgStructure.DemonstrateOrgStructure().Demonstrate()),
    ("Estructurales", "Decorator: Cafetería", () => new GoFPatterns.Decorator.Coffee.DemonstrateCoffee().Demonstrate()),
    ("Estructurales", "Decorator: Fuente de datos", () => new GoFPatterns.Decorator.DataStream.DemonstrateDataStream().Demonstrate()),
    ("Estructurales", "Facade: Cine en casa", () => new GoFPatterns.Facade.HomeTheater.DemonstrateHomeTheater().Demonstrate()),
    ("Estructurales", "Facade: Realización de pedidos", () => new GoFPatterns.Facade.OrderPlacement.DemonstrateOrderPlacement().Demonstrate()),
    ("Estructurales", "Flyweight: Bosque", () => new GoFPatterns.Flyweight.Forest.DemonstrateForest().Demonstrate()),
    ("Estructurales", "Flyweight: Formato de texto", () => new GoFPatterns.Flyweight.TextFormatting.DemonstrateTextFormatting().Demonstrate()),
    ("Estructurales", "Proxy: Carga diferida de imágenes", () => new GoFPatterns.Proxy.ImageLoading.DemonstrateImageLoading().Demonstrate()),
    ("Estructurales", "Proxy: Cuenta bancaria", () => new GoFPatterns.Proxy.BankAccount.DemonstrateBankAccount().Demonstrate()),

    ("De comportamiento", "Chain of Responsibility: Tickets de soporte", () => new GoFPatterns.ChainOfResponsibility.SupportTickets.DemonstrateSupportTickets().Demonstrate()),
    ("De comportamiento", "Chain of Responsibility: Middleware", () => new GoFPatterns.ChainOfResponsibility.Middleware.DemonstrateRequestPipeline().Demonstrate()),
    ("De comportamiento", "Command: Editor de texto", () => new GoFPatterns.Command.TextEditor.DemonstrateTextEditor().Demonstrate()),
    ("De comportamiento", "Command: Hogar inteligente", () => new GoFPatterns.Command.SmartHome.DemonstrateSmartHome().Demonstrate()),
    ("De comportamiento", "Interpreter: Expresiones matemáticas", () => new GoFPatterns.Interpreter.MathExpressions.DemonstrateMathExpressions().Demonstrate()),
    ("De comportamiento", "Interpreter: Búsqueda de productos", () => new GoFPatterns.Interpreter.ProductSearch.DemonstrateProductSearch().Demonstrate()),
    ("De comportamiento", "Iterator: Colección de libros", () => new GoFPatterns.Iterator.BookCollection.DemonstrateBookCollection().Demonstrate()),
    ("De comportamiento", "Iterator: Recorrido de árbol", () => new GoFPatterns.Iterator.Tree.DemonstrateTree().Demonstrate()),
    ("De comportamiento", "Mediator: Sala de chat", () => new GoFPatterns.Mediator.ChatRoom.DemonstrateChatRoom().Demonstrate()),
    ("De comportamiento", "Mediator: Formulario de registro", () => new GoFPatterns.Mediator.RegistrationForm.DemonstrateRegistrationForm().Demonstrate()),
    ("De comportamiento", "Memento: Historial del editor de texto", () => new GoFPatterns.Memento.TextEditorHistory.DemonstrateTextEditorHistory().Demonstrate()),
    ("De comportamiento", "Memento: Partidas guardadas", () => new GoFPatterns.Memento.GameSave.DemonstrateGameSave().Demonstrate()),
    ("De comportamiento", "Observer: Precio del producto", () => new GoFPatterns.Observer.ProductObserver.ProductObserverDemonstrate().Demonstrate()),
    ("De comportamiento", "State: Ciclo de vida del pedido", () => new GoFPatterns.State.OrderLifecycle.DemonstrateOrderLifecycle().Demonstrate()),
    ("De comportamiento", "State: Máquina expendedora", () => new GoFPatterns.State.Vending.DemonstrateVendingMachine().Demonstrate()),
    ("De comportamiento", "Strategy: Coste de envío", () => new GoFPatterns.Strategy.Delivery.DemonstrateDelivery().Demonstrate()),
    ("De comportamiento", "Strategy: Ordenación", () => new GoFPatterns.Strategy.Sorting.DemonstrateSorting().Demonstrate()),
    ("De comportamiento", "Template Method: Importación de datos", () => new GoFPatterns.TemplateMethod.DataImport.DemonstrateDataImport().Demonstrate()),
    ("De comportamiento", "Template Method: Bebidas calientes", () => new GoFPatterns.TemplateMethod.Beverages.DemonstrateBeverages().Demonstrate()),
    ("De comportamiento", "Visitor: Figuras", () => new GoFPatterns.Visitor.Shapes.ShapeDemostrateVisitor().Demonstrate()),
];

while (true)
{
    PrintMenu();
    Console.Write("Elija el número de la demo (0 - salir): ");
    var input = Console.ReadLine();

    // null significa que el flujo de entrada está cerrado (por ejemplo, la entrada se redirigió desde un archivo).
    if (input is null || input.Trim() == "0")
        break;

    if (!int.TryParse(input, out var number) || number < 1 || number > demos.Length)
    {
        Console.WriteLine("Número incorrecto, inténtelo de nuevo.\n");
        continue;
    }

    Console.WriteLine();
    try
    {
        demos[number - 1].Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"La demo falló: {ex.Message}");
    }
    Console.WriteLine();
}

void PrintMenu()
{
    Console.WriteLine("=== Patrones de diseño GoF ===");
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
