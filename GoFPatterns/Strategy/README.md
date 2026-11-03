🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Strategy) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Strategy) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Strategy) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Strategy)

# Strategy

**Type:** behavioral pattern.

**Strategy** defines a **family of interchangeable algorithms**, puts each one into a separate class and lets you swap them at runtime. The client works with the algorithm through a common interface.

---

## The problem

An online store calculates the shipping cost: by post, by courier, to a pickup point. If all of this is in one class:

```csharp
public decimal Calculate(Parcel parcel, string type)
{
    if (type == "post") return 3 + parcel.Weight * 1.2m;
    else if (type == "courier") return 10 + parcel.Distance * 0.05m + ...;
    else if (type == "pickup") return parcel.Value >= 100 ? 0 : 2.5m;
    ...
}
```

- every new delivery method means editing this class and risking breaking the others;
- the class grows, and the algorithms are mixed together;
- it is hard to test a single algorithm separately;
- the same `if` statements appear in delivery time estimation, in display logic, etc.

## Solution

Move each algorithm into a separate class — a **strategy** — with a common interface. The **context** class holds a reference to a strategy and delegates work to it without knowing which strategy exactly it was given. The client chooses the required strategy and passes it to the context; it can be replaced at any moment.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Strategy** | `IDeliveryStrategy`, `ISortStrategy` | The common interface of all algorithm variants |
| **ConcreteStrategy** | `PostDeliveryStrategy`, `CourierDeliveryStrategy`, `PickupPointStrategy`, `BubbleSortStrategy`, `QuickSortStrategy`, `BuiltInSortStrategy` | Concrete implementations of the algorithm |
| **Context** | `DeliveryCalculator`, `DataSorter` | Holds a strategy (or receives it as a parameter) and calls it through the interface |
| **Client** | demo classes | Chooses a strategy and passes it to the context |

## Diagram

```text
 Client ── chooses ───► PostDeliveryStrategy
   │                          │
   ▼                          ▼  implements
 DeliveryCalculator ◆────► «interface» IDeliveryStrategy
 (Context)                    CalculateCost(parcel)
   SetStrategy(s)             EstimateDays(parcel)
   GetQuote(parcel)                 ▲        ▲        ▲
                                    │        │        │
                                  Post   Courier   PickupPoint
```

---

## Example 1. Shipping cost (`Delivery/`)

**Strategy** — `IDeliveryStrategy` with the methods `CalculateCost` and `EstimateDays`. The input is a `record Parcel` (weight, distance, declared value).

**ConcreteStrategy** (folder `Strategies/`) — each contains only its own formula:
- `PostDeliveryStrategy` — cheap and slow; the price depends on the weight;
- `CourierDeliveryStrategy` — more expensive, depends on the distance, 1–2 days;
- `PickupPointStrategy` — free for orders of 100 or more:

```csharp
public decimal CalculateCost(Parcel parcel) =>
    parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;
```

**Context** — `DeliveryCalculator` receives a strategy in its constructor, can change it via `SetStrategy`, and builds a quote without knowing which strategy is inside:

```csharp
public string GetQuote(Parcel parcel) =>
    $"{_strategy.Name} cost: {_strategy.CalculateCost(parcel)}, days: {_strategy.EstimateDays(parcel)}";
```

**Client** — the demo runs one parcel through all strategies and then **automatically picks** the cheapest and the fastest — the strategies can be compared because they share a common interface.

To add, for example, drone delivery, it is enough to write one new class — neither the context nor the other strategies need to change.

## Example 2. Sorting algorithms (`Sorting/`)

**Strategy** — `ISortStrategy.Sort(int[] items)`.

**ConcreteStrategy** (folder `Strategies/`):
- `BubbleSortStrategy` — simple, O(n²);
- `QuickSortStrategy` — quicksort, O(n log n) on average;
- `BuiltInSortStrategy` — a wrapper around `Array.Sort` (.NET's introspective sort).

**Context** — `DataSorter`:
- `Sort(items, strategy)` — the strategy is passed as a **method parameter** rather than stored in a field. This is still Strategy: what matters is the ability to plug in an algorithm, not where the reference is stored;
- `ChooseFor(items)` — the context can **choose** a strategy **itself** based on the data (bubble sort for a tiny array, the built-in sort otherwise).

**Client** — the demo sorts the same 5000 numbers with all strategies and compares the times — it clearly shows why algorithms should be made interchangeable.

At the end it shows that in C# a simple strategy can be just a **delegate**: `Array.Sort(names, byLength)`, where `byLength` is a `Comparison<string>`. `IComparer<T>`, `IEqualityComparer<T>` and lambdas in LINQ (`OrderBy(x => ...)`) work the same way.

---

## When to use

- There are several variants of the same algorithm, and you need to switch between them.
- There are many similar classes that differ only in behavior.
- You need to get rid of a large conditional statement that selects an algorithm variant.
- You need to isolate the algorithm's details from the code that uses it.
- Examples: payment/delivery methods, discount and tax calculation, compression, sorting, routing, validation.

## When not to use

- There are two algorithms and they almost never change — an `if` is simpler.
- The client does not need to know about the variants — then Template Method or plain polymorphism may be better.

## Pros and cons

**Pros**
- Algorithms can be swapped at runtime.
- The algorithm code is isolated from the rest of the code (SRP).
- Inheritance is replaced with composition.
- New strategies are added without changing the context (Open/Closed).
- Each strategy is easy to test separately.

**Cons**
- The client must know the differences between strategies to choose the right one.
- Extra classes if there are only a few algorithms (mitigated by delegates in C#).

## Relations with other patterns

- **State** — almost the same structure, but a strategy is **chosen by the client**, and strategies are independent; states **switch between each other themselves**.
- **Template Method** varies parts of an algorithm through **inheritance** (statically, at the class level); **Strategy** varies the whole algorithm through **composition** (dynamically, at the object level).
- **Decorator** changes an object's "skin", **Strategy** changes its "guts".
- **Bridge** is also based on delegation through an interface, but there the abstraction forms a hierarchy as well.
- **Command** also packs behavior into an object, but a command is an **action** (which can be deferred or undone), while a strategy is a **way** of performing the same task.
- The **Abstract Factory** from this project (`IPaymentProviderFactory`) uses a similar idea: `PaymentService` works with any provider through an interface.
