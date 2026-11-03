🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Decorator) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Decorator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Decorator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Decorator)

# Decorator

**Type:** structural pattern.

**Decorator** lets you dynamically add new behavior to an object by "wrapping" it in wrapper objects with the same interface. Wrappers can be nested in each other in any order and number.

---

## The problem

A coffee shop sells drinks to which you can add milk, syrup, a larger size... If you solve this with inheritance:

```text
Espresso
 ├── EspressoWithMilk
 ├── EspressoWithSyrup
 ├── EspressoWithMilkAndSyrup
 ├── LargeEspressoWithMilkAndSyrup
 └── ...  (and all the same for tea)
```

The number of subclasses grows combinatorially. Moreover, inheritance is **static**: you cannot "add milk" to an already created object while the program is running. And double milk cannot be expressed at all without yet another class.

The same goes for data storage: sometimes you need compression, sometimes encryption, sometimes both, sometimes logging on top — you cannot keep up with a subclass for every combination.

## Solution

Replace inheritance with **composition**. A decorator:
- implements **the same interface** as the wrapped object;
- holds a reference to the wrapped object;
- delegates calls to it, adding its own behavior **before or after**.

Since a decorator looks like a regular component to the client, it can be wrapped in yet another decorator — you get a "nesting doll". The client does not know whether it works with the original object or with a chain of wrappers.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Component** | `IBeverage`, `IDataSource` | The common interface for both wrappers and wrapped objects |
| **ConcreteComponent** | `Espresso`, `BlackTea`, `InMemoryDataSource` | The base object to which behavior is added |
| **BaseDecorator** | `BeverageDecorator`, `DataSourceDecorator` | Holds a reference to Component and by default simply delegates calls to it |
| **ConcreteDecorator** | `MilkDecorator`, `SyrupDecorator`, `LargeSizeDecorator`, `CompressionDecorator`, `EncryptionDecorator`, `LoggingDecorator` | Add specific behavior before/after calling the wrapped object |
| **Client** | demo classes | Assembles the required combination of wrappers and works with it through Component |

## Diagram

```text
            ┌───────────────────────┐
 Client ───►│ «interface» IBeverage │◄─────────────────┐
            │ GetDescription()      │                  │ wraps
            │ GetCost()             │                  │
            └───────────────────────┘                  │
               ▲                 ▲                     │
               │                 │                     │
          Espresso    ┌──────────────────────────┐     │
          BlackTea    │ «abstract» BeverageDecorator│◆──┘
                      │ - Beverage: IBeverage    │
                      └──────────────────────────┘
                         ▲          ▲           ▲
                   MilkDecorator  SyrupDecorator  LargeSizeDecorator

  new LargeSize( new Syrup( new Milk( new Espresso() ) ) )
       │             │          │          └─ 2.00
       │             │          └─ +0.50
       │             └─ +0.70
       └─ ×1.5
```

---

## Example 1. Coffee shop (`Coffee/`)

**Component** — `IBeverage` with the methods `GetDescription()` and `GetCost()`.

**ConcreteComponent** — `Espresso` and `BlackTea` (folder `Beverages/`) know only their base price.

**BaseDecorator** — `BeverageDecorator` (folder `AddOns/`) holds the wrapped beverage and delegates everything to it via `virtual` methods:

```csharp
public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
{
    protected IBeverage Beverage { get; } = beverage;

    public virtual string GetDescription() => Beverage.GetDescription();
    public virtual decimal GetCost() => Beverage.GetCost();
}
```

**ConcreteDecorator** — each add-on overrides the methods, calling `base` and extending the result:

```csharp
public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
{
    public override string GetDescription() => $"{base.GetDescription()} + milk";
    public override decimal GetCost() => base.GetCost() + 0.50m;
}
```

- `SyrupDecorator` shows that a decorator can have **its own state** (the syrup flavor).
- `LargeSizeDecorator` does not add to the price but **multiplies** it. That is why the demo shows that **the order of wrappers matters**: "large (espresso + syrup)" costs more than "large espresso + syrup".

**Client** — assembles drinks as "nesting dolls": `new MilkDecorator(new MilkDecorator(new Espresso()))` — double milk without a separate class. The same add-ons work with tea without any changes.

## Example 2. Data source (`DataStream/`)

**Task:** data (e.g. customer data with a card number) is saved to storage. Depending on the settings, it must be compressed, encrypted, logged — in any combination.

**Component** — `IDataSource` with `Write`/`Read`.

**ConcreteComponent** — `InMemoryDataSource` stores the string as is (simulating a file or a database record).

**BaseDecorator** — `DataSourceDecorator` (folder `Decorators/`).

**ConcreteDecorator** — the "before" and "after" behavior is clearly visible here:

```csharp
// CompressionDecorator
public override void Write(string data) => base.Write(Compress(data));   // BEFORE writing — compress
public override string Read() => Decompress(base.Read());                // AFTER reading — decompress
```

- `CompressionDecorator` — GZip + Base64.
- `EncryptionDecorator` — XOR with a key (for demonstration only, this is not real encryption!).
- `LoggingDecorator` — does not change the data, only writes its size to the console.

**Client** — the demo wraps the storage:

```csharp
IDataSource source = new LoggingDecorator(
    new CompressionDecorator(
        new EncryptionDecorator(protectedStorage, "secret")),
    "client");
```

When writing, the data passes from the outside in (log → compression → encryption → storage); when reading, from the inside out. The storage holds a compressed, encrypted string, while the client reads the original text without even knowing it.

This is exactly how streams are designed in .NET: `new GZipStream(new CryptoStream(new FileStream(...)))` is a classic Decorator. `BufferedStream` and `StreamReader` (on top of `Stream`) are decorators too, and in ASP.NET Core so are the middleware pipeline and service decoration in DI (e.g. via Scrutor).

---

## When to use

- You need to add responsibilities to objects **dynamically** and transparently to the client.
- Extension through inheritance is impossible (the class is `sealed`) or leads to an explosion of subclasses.
- You need to combine independent "add-ons": caching, logging, retries, validation, metrics.

## When not to use

- You need one fixed combination of behavior — it is simpler to write a class.
- Behavior must depend on which wrappers are already present (decorators should not know about each other).

## Pros and cons

**Pros**
- More flexible than inheritance: behavior is added and combined at runtime.
- Each decorator is responsible for one thing (SRP).
- Can be applied several times (double milk).
- Lets you extend `sealed` classes and third-party code through their interface.

**Cons**
- Many small classes.
- It is hard to remove a specific wrapper from the middle of the chain.
- Behavior depends on the order of wrappers — this is easy to overlook.
- The code that configures the "nesting doll" can be cumbersome.

## Relations with other patterns

- **Adapter** changes an object's interface; **Decorator** keeps the interface the same and extends behavior; **Proxy** keeps the interface the same and **controls access** (it usually manages the object's lifecycle itself, while a decorator receives the object from the client).
- **Composite** is structurally similar (recursive composition), but a decorator has one nested object and aims to add behavior.
- **Chain of Responsibility** also passes a request along a chain, but its links can **stop** the processing; a decorator always passes the call on.
- **Strategy** changes an object's "guts", **Decorator** changes its "skin".
