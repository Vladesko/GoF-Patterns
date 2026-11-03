🌐 **Русский** | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Visitor) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Visitor) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Visitor) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Visitor)

# Visitor (Посетитель)

**Тип:** поведенческий паттерн.

**Visitor** позволяет **добавлять новые операции** к иерархии классов, **не изменяя сами классы**. Операция выносится в отдельный объект-посетитель, который «обходит» элементы и выполняет над каждым нужное действие.

---

## Какую проблему решает

Есть стабильная иерархия фигур: `Circle`, `Rectangle`. Над ними нужно выполнять всё новые операции: посчитать площадь, экспортировать в XML, потом в JSON, нарисовать, посчитать периметр...

Если добавлять каждую операцию методом в классы фигур:
- классы фигур **разрастаются** кодом, который к ним не относится (экспорт в XML — это не обязанность круга);
- каждая новая операция требует правки **всех** классов иерархии;
- иногда классы вообще нельзя менять (сторонний код).

Если писать операцию снаружи через проверку типов:

```csharp
foreach (var shape in shapes)
{
    if (shape is Circle c) ...
    else if (shape is Rectangle r) ...
}
```

…то при добавлении новой фигуры легко **забыть** обновить одну из таких цепочек — компилятор не подскажет.

## Решение

1. Каждую операцию вынести в отдельный класс — **посетителя**, у которого есть по методу на каждый тип элемента: `VisitCircle`, `VisitRectangle`.
2. В элементы добавить **один** метод `Accept(visitor)`, который вызывает «свой» метод у посетителя: круг вызывает `VisitCircle(this)`, прямоугольник — `VisitRectangle(this)`.

Этот приём называется **двойной диспетчеризацией**: нужный код выбирается по **двум** типам — типу элемента (через виртуальный `Accept`) и типу посетителя (через виртуальный `Visit…`). Без проверок типов и приведений.

Новая операция — это новый класс-посетитель. Классы фигур больше не меняются.

---

## Участники

| Роль | Класс в примере | Описание |
|------|-----------------|----------|
| **Visitor** | `IShapeVisitor` | Объявляет метод посещения для каждого типа элемента |
| **ConcreteVisitor** | `AreaCalculatorVisitor`, `XmlExportVisitor` | Реализует одну операцию для всех типов элементов. Может накапливать результат |
| **Element** | `IShape` | Объявляет метод `Accept(IShapeVisitor)` |
| **ConcreteElement** | `Circle`, `Rectangle` | Реализует `Accept`, вызывая соответствующий метод посетителя |
| **Client / ObjectStructure** | `ShapeDemostrateVisitor` (список `List<IShape>`) | Хранит коллекцию элементов и «проводит» посетителя по ней |

## Схема

```text
 «interface» IShape                    «interface» IShapeVisitor
   Accept(IShapeVisitor v)               VisitCircle(Circle)
      ▲              ▲                   VisitRectangle(Rectangle)
      │              │                        ▲                 ▲
   Circle        Rectangle                    │                 │
   Accept(v) =>  Accept(v) =>      AreaCalculatorVisitor   XmlExportVisitor
   v.VisitCircle(this)             TotalArea               GetXml()
                 v.VisitRectangle(this)

 shape.Accept(visitor)  ──►  visitor.VisitCircle(circle)     ← двойная диспетчеризация
```

---

## Пример. Фигуры: площадь и экспорт в XML (`Shapes/`)

**Element** — `IShape`:

```csharp
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}
```

**ConcreteElement** (папка `VisitShapes/`) — `Circle` (радиус) и `Rectangle` (ширина, высота) хранят только свои данные (`required init`-свойства) и реализуют `Accept`:

```csharp
public class Circle : IShape
{
    public required double Radius { get; init; }

    public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}
```

Обратите внимание: в фигурах **нет** ни расчёта площади, ни экспорта.

**Visitor** — `IShapeVisitor` с методами `VisitCircle(Circle)` и `VisitRectangle(Rectangle)`.

**ConcreteVisitor** (папка `Visiotrs/`):

`AreaCalculatorVisitor` — накапливает суммарную площадь всех посещённых фигур:

```csharp
public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
public void VisitRectangle(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
```

`XmlExportVisitor` — собирает XML-представление в `StringBuilder` и отдаёт его через `GetXml()`.

Оба посетителя **хранят состояние** между посещениями — это удобно, когда результат собирается по всей коллекции.

**Client** — `ShapeDemostrateVisitor` создаёт список фигур и двух посетителей, затем проходит по фигурам и «принимает» каждого посетителя:

```csharp
foreach (var shape in shapes)
{
    shape.Accept(areaVisitor);
    shape.Accept(xmlVisitor);
}
Console.WriteLine($"Общая площадь: {areaVisitor.TotalArea}");
Console.WriteLine(xmlVisitor.GetXml());
```

Чтобы добавить, например, `JsonExportVisitor` или `PerimeterVisitor`, нужен один новый класс — `Circle` и `Rectangle` не меняются.

---

## Цена гибкости

Visitor делает лёгким добавление **операций**, но трудным добавление **элементов**. Добавили `Triangle` — нужно добавить `VisitTriangle` в `IShapeVisitor` и реализовать его во **всех** посетителях. Зато компилятор сам покажет все места, которые надо обновить, — в отличие от цепочек `if (shape is ...)`.

Поэтому паттерн подходит, когда **иерархия элементов стабильна**, а **операции часто добавляются**.

> В современном C# похожего результата иногда добиваются `switch`-выражением по типам (`shape switch { Circle c => ..., Rectangle r => ... }`). Это короче, но компилятор не гарантирует, что обработаны все типы (если иерархия не закрыта). Visitor даёт эту гарантию.

## Когда применять

- Над объектами сложной структуры (дерево, AST, документ) нужно выполнять много разных несвязанных операций.
- Не хочется «засорять» классы элементов посторонней логикой.
- Иерархия элементов стабильна, а операции добавляются часто.
- Примеры: компиляторы и анализаторы кода (Roslyn — `CSharpSyntaxVisitor`), `ExpressionVisitor` в `System.Linq.Expressions`, экспорт документов в разные форматы, расчёты над структурой заказа/склада.

## Когда не нужен

- Иерархия элементов часто меняется — каждый новый класс потребует правки всех посетителей.
- Операций мало, и они естественно относятся к самим элементам.

## Плюсы и минусы

**Плюсы**
- Новые операции без изменения классов элементов (Open/Closed).
- Родственные операции собраны в одном классе (SRP).
- Посетитель может накапливать состояние при обходе структуры.
- Нет проверок типов и приведений.

**Минусы**
- Трудно добавлять новые типы элементов.
- Посетителю может понадобиться доступ к внутренностям элементов, что заставляет делать их поля публичными.

## Связь с другими паттернами

- **Composite** — посетитель часто обходит дерево компоновщика, выполняя операцию над всеми узлами.
- **Iterator** — итератор обходит структуру, посетитель выполняет действие над каждым элементом.
- **Interpreter** — посетители добавляют новые операции над синтаксическим деревом (печать, оптимизация, проверка) без изменения классов выражений.
- **Command** — посетитель можно рассматривать как расширенную команду, которая умеет работать с разными типами объектов.
