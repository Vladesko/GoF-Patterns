🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/AbtractFactory) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/AbtractFactory) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/AbtractFactory) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/AbtractFactory)

# Abstract Factory

**Type:** creational pattern.

**Abstract Factory** lets you create **families of related objects** without binding to the concrete classes of those objects. The client works with the factory and the products only through interfaces.

---

## The problem

A payment service works with payment providers (in the example, the fictional **PoolPay** and **Strope**). Each provider requires a **set** of objects:
- a payment processor;
- a refund processor;
- a payment validator.

These objects form a **family**: the PoolPay processor must work together with the PoolPay validator and refunds. Mixing the PoolPay processor with Strope refunds is a bug (a transaction of one provider does not exist at the other).

If the payment service creates the objects itself:

```csharp
if (provider == "PoolPay") { processor = new PoolPayPaymentProcess(); refund = new PoolPayRefundProcessor(); ... }
else if (provider == "Strope") { processor = new StropePaymentProcessor(); refund = new StropeRefundProcessor(); ... }
```

- the service depends on **all** concrete classes of all providers;
- adding a provider means editing the service;
- it is easy to make a mistake and create an "incompatible set".

## Solution

1. Extract an **interface** for each kind of product (`IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator`).
2. Declare an **abstract factory** — an interface with a creation method for each product of the family.
3. For each family (provider), create a **concrete factory** that creates only the products of its own family.
4. The client receives the factory from outside and creates products only through it — so it does not know the concrete classes and **cannot mix families**.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **AbstractFactory** | `IPaymentProviderFactory` | Declares creation methods for all products of the family |
| **ConcreteFactory** | `PoolPayPaymentFactory`, `StropePaymentFactory` | Creates the products of one family (one provider) |
| **AbstractProduct** | `IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator` | Product interfaces |
| **ConcreteProduct** | `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`, `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator` | Concrete product implementations for each provider |
| **Client** | `PaymentService` (+ `IPaymentService`) | Uses only the factory and product interfaces |

## Diagram

```text
                       «interface» IPaymentProviderFactory
                         CreatePaymentProcess(): IPaymentProcessor
                         CreateRefundProcess():  IRefundProcessor
                         CreatePaymentValidator(): IPaymentValidator
                              ▲                         ▲
                              │                         │
               PoolPayPaymentFactory            StropePaymentFactory
                 │      │       │                 │      │       │
                 ▼      ▼       ▼                 ▼      ▼       ▼
         PoolPayPayment PoolPay PoolPay      StropePayment Strope Strope
           Process      Refund  Validator      Processor   Refund Validator

 PaymentService(IPaymentProviderFactory factory)   ← Client
   Pay(amount):  validator.ValidatePayment → processor.ProcessPayment
   Refund(id):   refundProcessor.ProcessRefund
```

---

## Example. Payment providers (`Payments/`)

**AbstractProduct** (folder `Interfaces/Abstractions/`):

```csharp
public interface IPaymentProcessor { Guid ProcessPayment(decimal amount); }
public interface IRefundProcessor  { void ProcessRefund(Guid transactionId); }
public interface IPaymentValidator { bool ValidatePayment(decimal amount); }
```

**AbstractFactory** (folder `Interfaces/AbstractFactories/`):

```csharp
public interface IPaymentProviderFactory
{
    IPaymentProcessor CreatePaymentProcess();
    IRefundProcessor CreateRefundProcess();
    IPaymentValidator CreatePaymentValidator();
}
```

**ConcreteFactory + ConcreteProduct** — each provider has its own folder:
- `PoolPays/` — `PoolPayPaymentFactory` creates `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`;
- `Stopes/` — `StropePaymentFactory` creates `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator`.

All concrete classes are marked `internal` — nobody outside the assembly knows about them; only the interfaces are accessible.

```csharp
internal class PoolPayPaymentFactory : IPaymentProviderFactory
{
    public IPaymentProcessor CreatePaymentProcess() => new PoolPayPaymentProcess();
    public IPaymentValidator CreatePaymentValidator() => new PoolPayPaymentValidator();
    public IRefundProcessor CreateRefundProcess() => new PoolPayRefundProcessor();
}
```

**Client** — `PaymentService` (folder `Services/`) receives the factory in its constructor and creates the whole set of products through it **once**. From then on it works only with interfaces:

```csharp
internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
    private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
    private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

    public Guid Pay(decimal amount)
    {
        if (!_paymentValidator.ValidatePayment(amount))
            throw new InvalidOperationException("Invalid payment amount.");
        return _paymentProcessor.ProcessPayment(amount);
    }

    public void Refund(Guid transactionId) => _refundProcessor.ProcessRefund(transactionId);
}
```

Since all three products come from the same factory, they are **guaranteed to belong to the same family**.

**Demo** — `DemontrateAbstractFactory` creates a `PaymentService` first with the PoolPay factory, then with Strope, and performs the same `Pay` + `Refund` operations. The service code is the same — only the passed factory changes.

To plug in a new provider, it is enough to add a folder with a factory and three products. `PaymentService` and the existing providers do not change.

---

## When to use

- The system must work with different **families** of related objects, and they must be used **together**.
- You need to hide the concrete product classes from the client.
- The family is chosen once (by configuration or settings) and then used everywhere.
- Examples: payment providers, UI themes (button + checkbox + window for Windows/macOS), database providers (`DbProviderFactory` in ADO.NET: connection + command + parameter), cloud providers (storage + queue + cache).

## When not to use

- There is a single product rather than a family — Factory Method is enough.
- There is only one family and it will not be extended.

## Pros and cons

**Pros**
- Guarantees that the created products are compatible.
- The client does not depend on concrete classes.
- Product creation code is gathered in one place (SRP).
- A new family is added without changing the client (Open/Closed).

**Cons**
- Many interfaces and classes.
- Adding a **new kind of product** (e.g. `ISubscriptionProcessor`) is hard: you have to change the factory interface and **all** concrete factories.

## Relations with other patterns

- **Factory Method** — an abstract factory often consists of several factory methods. Factory Method creates **one** product and relies on inheritance; Abstract Factory creates a **family** and is passed around as an object (composition).
- **Builder** assembles **one complex** object step by step; Abstract Factory immediately returns a **set** of simple ones.
- **Prototype** — a factory can create products by cloning prototypes.
- **Singleton** — a concrete factory is usually needed as a single instance.
- **Facade** — an abstract factory can be used instead of a facade to hide how subsystem objects are created.
- **Strategy** — from the point of view of `PaymentService`, the factory looks like a "which provider to use" strategy.
