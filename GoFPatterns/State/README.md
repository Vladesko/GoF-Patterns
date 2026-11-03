🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/State) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/State) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/State) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/State)

# State

**Type:** behavioral pattern.

**State** lets an object **change its behavior when its internal state changes**. From the outside it looks as if the object has changed its class.

---

## The problem

An order in an online store goes through stages: *New → Paid → Shipped → Delivered* (or *Cancelled*). The same action behaves differently depending on the stage: cancelling a new order simply cancels it; cancelling a paid one refunds the money; a shipped one cannot be cancelled.

The typical implementation is a `Status` field and a `switch` in every method:

```csharp
public void Cancel()
{
    switch (Status)
    {
        case Status.New: Status = Status.Cancelled; break;
        case Status.Paid: Refund(); Status = Status.Cancelled; break;
        case Status.Shipped: throw ...;
        case Status.Delivered: throw ...;
        ...
    }
}
```

Such `switch` statements appear in `Pay`, `Ship`, `Deliver`, `Cancel`... Add a new status ("Return") — and you have to go through all the methods. The transition rules are spread across the class, and it is easy to make a mistake.

## Solution

Move each state into a **separate class** with a common interface that contains all the actions. The original object (the **context**) holds a reference to the current state object and **delegates** all actions to it. Transitioning to another state is simply replacing the state object.

Instead of one big `switch` over all states, you get several small classes, each describing **the behavior in one state** and **where it can transition to**.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Context** | `Order`, `VendingMachine` | Holds a reference to the current state and delegates actions to it. Provides a method for changing the state |
| **State** | `IOrderState`, `IVendingState` | The common interface of all states — all actions available to the context |
| **ConcreteState** | `NewState`, `PaidState`, `ShippedState`, `DeliveredState`, `CancelledState`, `IdleState`, `HasMoneyState`, `SoldOutState` | Implements the behavior in a specific state and decides when to switch the context |

## Diagram

```text
 ┌───────────────────────┐        ┌────────────────────────┐
 │ Order (Context)       │◆──────►│ «interface» IOrderState│
 │ - State               │        │ Pay(), Ship(),         │
 │ + Pay() => State.Pay()│        │ Deliver(), Cancel()    │
 │ + TransitionTo(state) │        └────────────────────────┘
 └───────────────────────┘                    ▲
                                ┌─────────────┼──────────────┬────────────┐
                             NewState     PaidState     ShippedState   ...

 Transitions: New ──Pay──► Paid ──Ship──► Shipped ──Deliver──► Delivered
               │            │
               └──Cancel──► Cancelled ◄──Cancel (refund)
```

---

## Example 1. Order lifecycle (`OrderLifecycle/`)

**Context** — `Order` does not contain a single `switch`. All public actions are simply delegated to the current state:

```csharp
public void Pay() => State.Pay(this);
public void Cancel() => State.Cancel(this);

internal void TransitionTo(IOrderState state) => State = state;
```

`TransitionTo` is marked `internal` — only state classes can change the state, not external code.

**State** — `IOrderState` lists all actions on an order.

**Base state** — `OrderStateBase` (folder `States/`) **forbids** all actions by default. Therefore concrete states override only what is allowed in them — there is no need to write "not allowed" for the remaining actions in every class.

**ConcreteState**:
- `NewState` — payment (→ `Paid`) and cancellation (→ `Cancelled`) are allowed;
- `PaidState` — shipping (→ `Shipped`) and cancellation **with a refund**:

```csharp
public override void Cancel(Order order)
{
    Console.WriteLine($"Refund {order.Amount:F2} to customer");
    order.TransitionTo(new CancelledState());
}
```

- `ShippedState` — only delivery (→ `Delivered`);
- `DeliveredState`, `CancelledState` — final states that override nothing.

You can see that the "cancel" action behaves differently in two states and is forbidden in the others — without a single `if` on the status.

**Client** — the demo tries to perform actions in the wrong order (ship an unpaid order, cancel a shipped one) — the states correctly refuse.

## Example 2. Vending machine (`Vending/`)

**Context** — `VendingMachine` stores the **data** (items left, price, balance) and the current state.

**ConcreteState** (folder `States/`):
- `IdleState` — waits for money; a coin → `HasMoney`;
- `HasMoneyState` — accumulates money, dispenses the item and the change; if the items have run out → `SoldOut`, otherwise → `Idle`;
- `SoldOutState` — immediately returns any coin.

This shows a technique from the GoF book: the states **have no fields of their own** — all data is stored in the context and passed as a parameter. Therefore each state can be created once and **shared** (these are flyweights, see Flyweight):

```csharp
internal static readonly IVendingState Idle = new IdleState();
internal static readonly IVendingState HasMoney = new HasMoneyState();
internal static readonly IVendingState SoldOut = new SoldOutState();
```

**Client** — the demo simulates customers: an attempt to buy without money, not enough money, a purchase with change, a refund, buying the last item and an attempt to insert a coin into an empty machine. The current state is printed after each action.

---

## When to use

- An object's behavior depends heavily on its state, and there are many states.
- The code is full of `switch`/`if` statements on a status field that are repeated in different methods.
- The rules of transitions between states are complex and change often.
- Examples: orders, document workflows (draft → under review → approved), TCP connections, a media player, game characters, finite state machines.

## When not to use

- There are few states and they rarely change — a `switch` on an `enum` will be simpler and clearer.

## Pros and cons

**Pros**
- Eliminates large conditional statements.
- The code of each state lives in its own class (SRP).
- New states are added without changing existing ones (Open/Closed).
- Transitions are explicit: you can see which state leads where.

**Cons**
- Many classes when there are only a few states.
- The transition logic is distributed across the state classes — it is harder to see the whole state machine at once (a diagram in the documentation helps).

## Relations with other patterns

- **Strategy** — the structure is almost identical (a context delegates to an object with an interface). The difference is in intent: a strategy is **chosen by the client**, and strategies do not know about each other; states **switch each other themselves** and know about the transitions.
- **Flyweight** — states without fields can be shared between contexts (the `Vending` example).
- **Singleton** — state objects are sometimes made singletons.
- **Bridge** and **Strategy** are also based on composition and delegation, but solve different problems.
