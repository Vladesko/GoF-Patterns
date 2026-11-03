using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Каждая демонстрация: группа паттерна, название в меню и действие, которое её запускает.
(string Group, string Name, Action Run)[] demos =
[
    ("Порождающие", "Абстрактная фабрика: платежи", () => new GoFPatterns.AbtractFactory.Payments.DemontrateAbstractFactory().Demonstrate()),
    ("Порождающие", "Строитель: пиццерия", () => new GoFPatterns.Builder.PizzaShop.DemonstratePizzaBuilder().Demonstrate()),
    ("Порождающие", "Строитель: HTTP-запрос", () => new GoFPatterns.Builder.HttpRequest.DemonstrateHttpRequestBuilder().Demonstrate()),
    ("Порождающие", "Фабричный метод: абонементы в фитнес-клуб", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateFitnes),
    ("Порождающие", "Фабричный метод: уведомления", GoFPatterns.FactoryMethod.DemondtrsteFactoryMethod.DemonstrateNotify),
    ("Порождающие", "Прототип: документы", () => new GoFPatterns.Prototype.Documents.DemonstratePrototype().Demonstrate()),
    ("Порождающие", "Прототип: игровые юниты", () => new GoFPatterns.Prototype.GameUnits.DemonstrateGameUnits().Demonstrate()),
    ("Порождающие", "Одиночка: конфигурация", () => new GoFPatterns.Singleton.Configuration.DemonstrateConfiguration().Demonstrate()),
    ("Порождающие", "Одиночка: логгер", () => new GoFPatterns.Singleton.Logger.DemonstrateLogger().Demonstrate()),

    ("Структурные", "Адаптер: датчики температуры", () => new GoFPatterns.Adapter.SensorFeature.DemonstarteAdapter().Demonstarte()),
    ("Структурные", "Мост: пульт управления", () => new GoFPatterns.Bridge.RemoteControl.DemonstrateRemoteControl().Demonstrate()),
    ("Структурные", "Мост: отчёты", () => new GoFPatterns.Bridge.Reports.DemonstrateReports().Demonstrate()),
    ("Структурные", "Компоновщик: файловая система", () => new GoFPatterns.Composite.FileSystem.DemonstrateFileSystem().Demonstrate()),
    ("Структурные", "Компоновщик: структура организации", () => new GoFPatterns.Composite.OrgStructure.DemonstrateOrgStructure().Demonstrate()),
    ("Структурные", "Декоратор: кофейня", () => new GoFPatterns.Decorator.Coffee.DemonstrateCoffee().Demonstrate()),
    ("Структурные", "Декоратор: источник данных", () => new GoFPatterns.Decorator.DataStream.DemonstrateDataStream().Demonstrate()),
    ("Структурные", "Фасад: домашний кинотеатр", () => new GoFPatterns.Facade.HomeTheater.DemonstrateHomeTheater().Demonstrate()),
    ("Структурные", "Фасад: оформление заказа", () => new GoFPatterns.Facade.OrderPlacement.DemonstrateOrderPlacement().Demonstrate()),
    ("Структурные", "Легковес: лес", () => new GoFPatterns.Flyweight.Forest.DemonstrateForest().Demonstrate()),
    ("Структурные", "Легковес: форматирование текста", () => new GoFPatterns.Flyweight.TextFormatting.DemonstrateTextFormatting().Demonstrate()),
    ("Структурные", "Заместитель: ленивая загрузка изображений", () => new GoFPatterns.Proxy.ImageLoading.DemonstrateImageLoading().Demonstrate()),
    ("Структурные", "Заместитель: банковский счёт", () => new GoFPatterns.Proxy.BankAccount.DemonstrateBankAccount().Demonstrate()),

    ("Поведенческие", "Цепочка обязанностей: заявки в поддержку", () => new GoFPatterns.ChainOfResponsibility.SupportTickets.DemonstrateSupportTickets().Demonstrate()),
    ("Поведенческие", "Цепочка обязанностей: middleware", () => new GoFPatterns.ChainOfResponsibility.Middleware.DemonstrateRequestPipeline().Demonstrate()),
    ("Поведенческие", "Команда: текстовый редактор", () => new GoFPatterns.Command.TextEditor.DemonstrateTextEditor().Demonstrate()),
    ("Поведенческие", "Команда: умный дом", () => new GoFPatterns.Command.SmartHome.DemonstrateSmartHome().Demonstrate()),
    ("Поведенческие", "Интерпретатор: математические выражения", () => new GoFPatterns.Interpreter.MathExpressions.DemonstrateMathExpressions().Demonstrate()),
    ("Поведенческие", "Интерпретатор: поиск товаров", () => new GoFPatterns.Interpreter.ProductSearch.DemonstrateProductSearch().Demonstrate()),
    ("Поведенческие", "Итератор: коллекция книг", () => new GoFPatterns.Iterator.BookCollection.DemonstrateBookCollection().Demonstrate()),
    ("Поведенческие", "Итератор: обход дерева", () => new GoFPatterns.Iterator.Tree.DemonstrateTree().Demonstrate()),
    ("Поведенческие", "Посредник: чат-комната", () => new GoFPatterns.Mediator.ChatRoom.DemonstrateChatRoom().Demonstrate()),
    ("Поведенческие", "Посредник: форма регистрации", () => new GoFPatterns.Mediator.RegistrationForm.DemonstrateRegistrationForm().Demonstrate()),
    ("Поведенческие", "Снимок: история текстового редактора", () => new GoFPatterns.Memento.TextEditorHistory.DemonstrateTextEditorHistory().Demonstrate()),
    ("Поведенческие", "Снимок: сохранения игры", () => new GoFPatterns.Memento.GameSave.DemonstrateGameSave().Demonstrate()),
    ("Поведенческие", "Наблюдатель: цена товара", () => new GoFPatterns.Observer.ProductObserver.ProductObserverDemonstrate().Demonstrate()),
    ("Поведенческие", "Состояние: жизненный цикл заказа", () => new GoFPatterns.State.OrderLifecycle.DemonstrateOrderLifecycle().Demonstrate()),
    ("Поведенческие", "Состояние: торговый автомат", () => new GoFPatterns.State.Vending.DemonstrateVendingMachine().Demonstrate()),
    ("Поведенческие", "Стратегия: стоимость доставки", () => new GoFPatterns.Strategy.Delivery.DemonstrateDelivery().Demonstrate()),
    ("Поведенческие", "Стратегия: сортировка", () => new GoFPatterns.Strategy.Sorting.DemonstrateSorting().Demonstrate()),
    ("Поведенческие", "Шаблонный метод: импорт данных", () => new GoFPatterns.TemplateMethod.DataImport.DemonstrateDataImport().Demonstrate()),
    ("Поведенческие", "Шаблонный метод: горячие напитки", () => new GoFPatterns.TemplateMethod.Beverages.DemonstrateBeverages().Demonstrate()),
    ("Поведенческие", "Посетитель: фигуры", () => new GoFPatterns.Visitor.Shapes.ShapeDemostrateVisitor().Demonstrate()),
];

while (true)
{
    PrintMenu();
    Console.Write("Выберите номер демонстрации (0 - выход): ");
    var input = Console.ReadLine();

    // null означает, что входной поток закрыт (например, ввод перенаправлен из файла).
    if (input is null || input.Trim() == "0")
        break;

    if (!int.TryParse(input, out var number) || number < 1 || number > demos.Length)
    {
        Console.WriteLine("Неверный номер, попробуйте ещё раз.\n");
        continue;
    }

    Console.WriteLine();
    try
    {
        demos[number - 1].Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка демонстрации: {ex.Message}");
    }
    Console.WriteLine();
}

void PrintMenu()
{
    Console.WriteLine("=== Паттерны проектирования GoF ===");
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
