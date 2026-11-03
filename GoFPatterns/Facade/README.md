🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Facade) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Facade) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Facade) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Facade)

# Facade

**Type:** structural pattern.

**Facade** provides a **simple interface** to a complex subsystem (a set of classes, a library, a framework). The facade itself knows which subsystem objects to forward a request to and in what order.

---

## The problem

To watch a movie on a home theater you need to: dim the lights, turn on the projector, select the input, enable widescreen mode, turn on the sound, select surround, set the volume, turn on the player, start the movie. And at the end, turn all of it off in the right order.

To place an order in an online store: check stock, reserve the product, calculate shipping, charge the money, release the reservation if the payment is declined, create a shipment, notify the customer.

If the client code (UI, controller) does this itself:
- it is **tightly coupled** to a dozen subsystem classes and knows the details of how they work;
- the same sequence is **duplicated** in every place it is needed;
- any change to the subsystem breaks all clients.

## Solution

Create a **facade** class that:
- holds references to the subsystem objects;
- provides a few high-level methods (`WatchMovie`, `PlaceOrder`);
- internally calls the required subsystem objects in the right order.

The client depends only on the facade. The subsystem is **not hidden completely**, though: if something non-standard is needed, its classes can be accessed directly.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Facade** | `HomeTheaterFacade`, `OrderFacade` | Provides a simple interface; knows to whom and in what order to delegate work |
| **Subsystem classes** | `Projector`, `SoundSystem`, `MediaPlayer`, `Lights`, `InventoryService`, `PaymentGateway`, `ShippingService`, `NotificationService` | Do the actual work. **Know nothing about the facade** |
| **Client** | demo classes | Works with the subsystem through the facade |

## Diagram

```text
            Client
              │  WatchMovie("Inception")
              ▼
   ┌──────────────────────┐
   │  HomeTheaterFacade   │
   └──────────────────────┘
     │       │       │       │
     ▼       ▼       ▼       ▼
 Projector Sound  MediaPlayer Lights      ← subsystem
            System
```

---

## Example 1. Home theater (`HomeTheater/`)

**Subsystem** — the classes in the `Subsystems/` folder (`Projector`, `SoundSystem`, `MediaPlayer`, `Lights`). Each has its own set of low-level methods. They depend neither on each other nor on the facade.

**Facade** — `HomeTheaterFacade` receives the subsystem objects through its constructor and combines them into scenarios:

```csharp
public void WatchMovie(string movie)
{
    lights.Dim(10);
    projector.On();
    projector.SetInput("HDMI-1");
    projector.WideScreenMode();
    sound.On();
    sound.SetSurroundSound();
    sound.SetVolume(25);
    player.On();
    player.Play(movie);
}
```

`EndMovie()` turns everything off in reverse order.

**Client** — calls `theater.WatchMovie("Inception")` — one line instead of ten. At the end the demo shows that direct access to the subsystem (`new Lights().Dim(50)`) is still possible.

## Example 2. Order placement (`OrderPlacement/`)

**Task:** a business process involving several services — with checks and rollback on failure.

**Subsystem** — `InventoryService` (stock and reservations), `PaymentGateway` (charging; cards ending in `0000` are declined), `ShippingService` (cost and shipment creation), `NotificationService` (emails).

**Facade** — `OrderFacade.PlaceOrder(OrderRequest)` encapsulates the whole process:

```csharp
if (!inventory.IsAvailable(order.Product, order.Count))
    return new OrderResult(false, $"{order.Product} is out of stock");

inventory.Reserve(order.Product, order.Count);

var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
if (!payments.Charge(order.CardNumber, total))
{
    inventory.Release(order.Product, order.Count);   // rollback
    return new OrderResult(false, "Payment was declined");
}
```

The facade knows not only the order of calls but also the **coordination rules** of the subsystems: for example, that the reservation must be released if the payment fails. Input and output data are packed into simple `record`s (`OrderRequest`, `OrderResult`), so the client does not need to know the subsystem types.

**Client** — the demo sends three orders: a successful one, one with an out-of-stock product, and one with a declined card. The client sees only the result.

In a real ASP.NET Core application such a facade is usually called an "application service" (`OrderService`, `CheckoutService`), and the controller depends only on it.

---

## When to use

- You need a simple interface to a complex subsystem or a third-party library.
- You need to reduce the client's coupling to many classes.
- You need to split the system into layers: a facade is the entry point to each layer/module.

## When not to use

- The subsystem is already simple — the facade would be an unnecessary layer.
- Clients constantly need different low-level capabilities — the facade would only get in the way.

## Pros and cons

**Pros**
- Isolates clients from subsystem components.
- Reduces coupling: the subsystem can be changed without touching the clients.
- Removes duplication of scenarios.

**Cons**
- Risk of turning into a **"god object"** that knows about everything and does everything. Several small task-specific facades are better.

## Relations with other patterns

- **Adapter** adapts **one** existing interface to the required one; **Facade** invents a **new simplified** interface for a whole subsystem.
- **Mediator** also centralizes interaction, but the subsystem objects **know** about the mediator and communicate through it; in Facade the subsystem does not know about the facade, and the communication is one-way.
- **Abstract Factory** can be used together with Facade to hide how subsystem objects are created.
- A facade is often made a **Singleton**, since usually one instance is needed.
- **Proxy** has the same interface as the object; **Facade** has its own interface.
