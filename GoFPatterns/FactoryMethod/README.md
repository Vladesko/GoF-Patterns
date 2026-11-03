🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/FactoryMethod) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/FactoryMethod) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/FactoryMethod) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/FactoryMethod)

# Factory Method

**Type:** creational pattern.

**Factory Method** defines a method for creating an object in a base class but lets **subclasses decide which class to instantiate**. The code that uses the object depends only on its interface, not on the concrete class.

---

## The problem

At first the notification service could only send SMS, and the code looked like this:

```csharp
var sms = new SmsNotification();
sms.Notify(message);
```

Then email and push were needed. If you continue in the same vein, the code fills up with `switch`/`if` statements with `new` for each type — in every place where notifications are sent. The code is tightly bound to concrete classes, and adding a new channel requires edits all over the project.

The same goes for fitness club memberships: "gym", "gym + pool", "personal training". If the client code calls `new GymMembership()` itself, it depends on all kinds of memberships.

## Solution

Replace the direct `new` call with a call to a special **factory method**. The method is declared in the base class (the Creator) and returns the **product interface**, while Creator subclasses override it and return concrete products.

Importantly, the Creator usually also has **its own logic** that works with the product through the interface and does not know which product exactly was created (in the notification example, the `Notify` method).

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Product** | `INotificationService`, `IMembership` | The common interface of the created objects |
| **ConcreteProduct** | `SmsNotification`, `UmailNotification`, `PushNotification`, `GymMembership`, `GymPoolMembership`, `PersonalTrainingMembership` | Concrete product implementations |
| **Creator** | `NotificationFactory`, `MembershipFactory` | Declares the factory method; may contain logic that works with the product |
| **ConcreteCreator** | `SmsNotificator`, `UmailNotificator`, `PushNotificator`, `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` | Overrides the factory method and returns a concrete product |
| **Client** | `DemondtrsteFactoryMethod` | Works with Creators and products |

## Diagram

```text
 ┌───────────────────────────────────────────┐          ┌─────────────────────────────┐
 │ «abstract» NotificationFactory (Creator)  │          │ «interface»                 │
 │ # CreateNotification(): INotificationService ──────►│ INotificationService        │
 │ + Notify(message):                        │ creates  │ Notify(message)             │
 │     var s = CreateNotification();         │          └─────────────────────────────┘
 │     s.Notify(message);                    │                ▲        ▲        ▲
 └───────────────────────────────────────────┘                │        │        │
        ▲                ▲                ▲          SmsNotification  Umail   Push
        │                │                │                          Notification Notification
  SmsNotificator   UmailNotificator  PushNotificator
  Create… => new SmsNotification()  ...
```

---

## Example 1. Notification system (`NotifySystem/`) — a Creator with its own logic

**Product** — `INotificationService` with the `Notify(string message)` method.

**ConcreteProduct** (folder `NotifiationServices/`) — `SmsNotification`, `UmailNotification`, `PushNotification`: each "sends" the message through its own channel.

**Creator** — the abstract `NotificationFactory` (folder `Factories/`):

```csharp
public abstract class NotificationFactory
{
    protected abstract INotificationService CreateNotification();   // factory method

    public void Notify(string message)                               // Creator logic
    {
        var notificationService = CreateNotification();
        notificationService.Notify(message);
    }
}
```

This is the classic form of the pattern: the `Notify` method is implemented **once** in the base class and works with the product only through the interface. The factory method is `protected` — the client does not create the product itself but simply calls `Notify`.

**ConcreteCreator** — `SmsNotificator`, `UmailNotificator`, `PushNotificator` override only the factory method:

```csharp
internal class SmsNotificator : NotificationFactory
{
    protected override INotificationService CreateNotification() => new SmsNotification();
}
```

**Client** — `DemondtrsteFactoryMethod.DemonstrateNotify()` creates the required Creator and calls `Notify`. To add, for example, Telegram, you need two classes — `TelegramNotification` and `TelegramNotificator`; the existing code does not change.

## Example 2. Fitness club memberships (`Fitnes/`) — product factories

**Product** — `IMembership` (name, description, price).

**ConcreteProduct** (folder `Memberships/`):
- `GymMembership` — gym, 50;
- `GymPoolMembership` — gym + pool, 75;
- `PersonalTrainingMembership` — personal training, 100.

**Creator** — the abstract `MembershipFactory` with a public factory method:

```csharp
internal abstract class MembershipFactory
{
    public abstract IMembership CreateMembership();
}
```

**ConcreteCreator** (folder `Factories/`) — `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` — each returns its own membership.

**Client** — `DemondtrsteFactoryMethod.DemonstrateFitnes()` picks the required factory, obtains a membership via `CreateMembership()` and then works with it as an `IMembership`.

Here the Creator has no logic of its own — this is a simplified form of the pattern that is often used in practice: a factory (e.g. chosen by settings or by the user) is passed into code that creates objects without knowing their classes.

---

## When to use

- It is not known in advance which classes of objects will need to be created.
- You want to allow extending the created objects through inheritance (frameworks, libraries).
- The base class contains a common algorithm, and subclasses define the concrete product for it.
- You need to centralize object creation so that objects can be reused when needed (cache, pool).

## When not to use

- There is one product and it will not change — a plain `new` is clearer.
- You need to create **families** of matching objects — that is Abstract Factory.

## Pros and cons

**Pros**
- Frees the code from binding to concrete product classes.
- Product creation code is in one place (SRP).
- New products are added without changing existing code (Open/Closed).

**Cons**
- Each product needs its own Creator subclass; the hierarchies grow in parallel.

## Relations with other patterns

- **Abstract Factory** is often implemented with a set of factory methods, but it creates a **family** of products and is passed around as an object.
- **Template Method** — a factory method is often a step of a template method: `NotificationFactory.Notify()` is a template method that calls the factory method `CreateNotification()`.
- **Prototype** does not require Creator subclasses but requires initializing the prototypes. Factory Method is the other way around.
- **Iterator** — `GetEnumerator()` in .NET collections is a factory method that returns an iterator.
- Factory Method often evolves into **Builder** or **Abstract Factory** when creation becomes more complex.
