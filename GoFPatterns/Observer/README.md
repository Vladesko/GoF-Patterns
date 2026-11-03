🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Observer) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Observer) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Observer) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Observer)

# Observer

**Type:** behavioral pattern.

**Observer** defines a **subscription** mechanism: some objects (observers) subscribe to changes of another object (the publisher) and automatically receive notifications when something happens to it. A "one-to-many" relationship.

---

## The problem

A product's price changes. Different parts of the system must react to this:
- a **buyer** waits for the price to drop low enough to buy;
- a **discount service** announces a sale when the price is below a threshold;
- a **statistics collector** counts all price changes.

Options without the pattern:
- **Polling**: every interested object periodically asks for the price. Extra load and delayed reaction.
- **The product calls everyone itself**: `buyer.Check(price); discounts.Check(price); stats.Add(price);` — the product knows about every concrete class. A new interested party means editing the product. Nobody can be removed at runtime.

## Solution

The publisher (the product) stores a **list of subscribers** and provides `Subscribe`/`Unsubscribe` methods. All subscribers implement a common interface with an `Update` method. When the state changes, the publisher goes through the list and calls `Update` on each one.

The publisher knows about the subscribers only that they implement the interface. Subscribers can be added and removed at runtime — a subscriber can even unsubscribe itself.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Subject (Observable)** | `IObservable` | The publisher interface: subscribe and unsubscribe |
| **ConcreteSubject** | `Product` | Stores the state (the price) and the list of subscribers; notifies them about changes |
| **Observer** | `IObserver` | The subscriber interface with the `Update(decimal price)` method |
| **ConcreteObserver** | `Buyer`, `DiscountNotifier`, `StatisticsCollector` | React to notifications in their own way |
| **Client** | `ProductObserverDemonstrate` | Creates the publisher and the subscribers, changes the state |

## Diagram

```text
 ┌───────────────────────────┐        ┌──────────────────────────┐
 │ Product (ConcreteSubject) │        │ «interface» IObserver    │
 │ - _observers: HashSet ────┼──────► │ Update(price)            │
 │ + Subscribe(o)            │  0..*  └──────────────────────────┘
 │ + Unsubscribe(o)          │            ▲         ▲          ▲
 │ + ChangePrice(p):         │            │         │          │
 │     NotifyObservers()     │         Buyer  DiscountNotifier  StatisticsCollector
 └───────────────────────────┘     (≤ 50: buy  (≤ 100:          (count every
              ▲                     & unsubscribe) discount)      change)
              │ implements
   «interface» IObservable
```

---

## Example. Product price changes (`ProductObserver/`)

**Subject** — `IObservable` with the methods `Subscribe(IObserver)` and `Unsubscribe(IObserver)`.

**Observer** — `IObserver` with the `Update(decimal price)` method. The publisher passes the new value directly in the notification (the so-called **push model**).

**ConcreteSubject** — `Product`:

```csharp
public class Product(decimal initialPrice) : IObservable
{
    private readonly HashSet<IObserver> _observers = [];
    private decimal _price = initialPrice;

    public void ChangePrice(decimal newPrice)
    {
        if (_price == newPrice) return;      // do not notify if nothing has changed
        _price = newPrice;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers.ToArray())
            observer.Update(_price);
    }
}
```

Important implementation details:
- `HashSet` — the same subscriber cannot be subscribed twice;
- `ArgumentNullException.ThrowIfNull` in `Subscribe`/`Unsubscribe` — protection against `null`;
- `NotifyObservers` is `private`: only the product itself can notify subscribers;
- the iteration goes over a **copy** of the list (`_observers.ToArray()`) because a subscriber can **unsubscribe right during the notification** (as `Buyer` does). Modifying a `HashSet` while iterating over that same `HashSet` is not allowed — it would throw an `InvalidOperationException`.

**ConcreteObserver** (folder `Observers/`) — each one subscribes in its constructor (`_observable.Subscribe(this)`) and reacts to the price in its own way:
- `StatisticsCollector` — counts every price change and prints the counter;
- `DiscountNotifier` — announces a discount when the price is ≤ 100;
- `Buyer` — buys the product when the price is ≤ 50 and **unsubscribes itself**, since it is no longer interested:

```csharp
public void Update(decimal price)
{
    if (price > 50) return;
    Console.WriteLine("Buying product!");
    _observable.Unsubscribe(this);
}
```

**Client** — `ProductObserverDemonstrate` creates a product with a price of 150 and three subscribers, then changes the price: 120 → 90 → 40. Statistics fire on every change; at 90 a discount appears; at 40, a discount and a purchase.

---

## Push and pull

- **Push** (as here) — the publisher passes the data in `Update(price)`. Simple, but the interface is tied to specific data.
- **Pull** — the publisher passes itself (`Update(IObservable sender)`), and the subscriber reads the properties it needs. More flexible, but the subscriber depends on the publisher.

## Observer in .NET

- **C# events** (`event EventHandler<T>`) — a built-in implementation of the pattern: `+=` subscribes, `-=` unsubscribes, raising the event notifies.
- The `IObservable<T>` / `IObserver<T>` interfaces in the BCL and the **Rx.NET** library — reactive streams.
- `INotifyPropertyChanged` in WPF/MAUI — binding the UI to the model.

## When to use

- A change in one object requires a reaction from others, and it is not known in advance which ones and how many.
- Subscribers must be added and removed at runtime.
- You need to loosen the coupling between the source of events and the reactions to them.

## When not to use

- There is exactly one subscriber, known in advance — a direct call is simpler.
- A strict order and transactional reactions are important — observers usually do not get a guaranteed notification order.

## Pros and cons

**Pros**
- The publisher does not depend on concrete subscriber classes (Open/Closed).
- Subscribing and unsubscribing at runtime.
- One event — any number of reactions.

**Cons**
- The order in which subscribers are notified is not guaranteed.
- **Memory leaks**: if a subscriber forgets to unsubscribe, the publisher keeps a reference to it and the garbage collector will not collect it (a classic problem with events in .NET).
- A cascade of notifications can be hard to debug.

## Relations with other patterns

- **Mediator** — often implemented through Observer: components publish events, and the mediator subscribes to them. The goals differ: Observer is dynamic broadcasting, Mediator is centralized interaction logic.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer** — different ways to connect senders and receivers of requests.
- **Singleton** — an event manager publisher is sometimes made a singleton (an event bus).
