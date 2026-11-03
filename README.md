🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa)

# GoF Design Patterns in C#

A learning project with all 23 classic **Gang of Four** design patterns implemented in C# (.NET 10).

Each pattern lives in its own folder and contains:
- a `README.md` explaining the problem, the solution, the participants, a diagram, when to use the pattern (and when not to), pros and cons, and relations with other patterns;
- one or two practical examples with a demo class that shows the pattern in action.

## How to run

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project GoFPatterns
```

A console menu lists all demos. Enter a demo number to run it, or `0` to exit.

## Patterns

### Creational

| Pattern | Description |
|---------|-------------|
| [Abstract Factory](GoFPatterns/AbtractFactory/README.md) | Creates families of related objects without specifying their concrete classes |
| [Builder](GoFPatterns/Builder/README.md) | Constructs complex objects step by step |
| [Factory Method](GoFPatterns/FactoryMethod/README.md) | Lets subclasses decide which class to instantiate |
| [Prototype](GoFPatterns/Prototype/README.md) | Creates new objects by copying existing ones |
| [Singleton](GoFPatterns/Singleton/README.md) | Ensures a class has only one instance with a global access point |

### Structural

| Pattern | Description |
|---------|-------------|
| [Adapter](GoFPatterns/Adapter/README.md) | Lets objects with incompatible interfaces work together |
| [Bridge](GoFPatterns/Bridge/README.md) | Splits an abstraction and its implementation into independent hierarchies |
| [Composite](GoFPatterns/Composite/README.md) | Composes objects into trees and treats them uniformly |
| [Decorator](GoFPatterns/Decorator/README.md) | Adds behavior to objects dynamically by wrapping them |
| [Facade](GoFPatterns/Facade/README.md) | Provides a simple interface to a complex subsystem |
| [Flyweight](GoFPatterns/Flyweight/README.md) | Shares common state between many objects to save memory |
| [Proxy](GoFPatterns/Proxy/README.md) | Controls access to an object through a substitute with the same interface |

### Behavioral

| Pattern | Description |
|---------|-------------|
| [Chain of Responsibility](GoFPatterns/ChainOfResponsibility/README.md) | Passes a request along a chain of handlers |
| [Command](GoFPatterns/Command/README.md) | Turns a request into an object that can be queued, logged and undone |
| [Interpreter](GoFPatterns/Interpreter/README.md) | Defines a grammar for a simple language and interprets its sentences |
| [Iterator](GoFPatterns/Iterator/README.md) | Traverses a collection without exposing its internal structure |
| [Mediator](GoFPatterns/Mediator/README.md) | Makes objects communicate through a mediator instead of directly |
| [Memento](GoFPatterns/Memento/README.md) | Saves and restores an object's state without breaking encapsulation |
| [Observer](GoFPatterns/Observer/README.md) | Notifies subscribers automatically when an object changes |
| [State](GoFPatterns/State/README.md) | Changes an object's behavior when its internal state changes |
| [Strategy](GoFPatterns/Strategy/README.md) | Defines a family of interchangeable algorithms |
| [Template Method](GoFPatterns/TemplateMethod/README.md) | Defines the skeleton of an algorithm and lets subclasses override steps |
| [Visitor](GoFPatterns/Visitor/README.md) | Adds new operations to a class hierarchy without changing it |
