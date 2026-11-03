🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/ChainOfResponsibility) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/ChainOfResponsibility) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/ChainOfResponsibility) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/ChainOfResponsibility)

# Chain of Responsibility

**Type:** behavioral pattern.

**Chain of Responsibility** lets you pass a request sequentially along a **chain of handlers**. Each handler decides whether to process the request itself, pass it further along the chain — or both.

---

## The problem

**Customer support.** A ticket may be closed by a bot (if it is a frequently asked question), an operator (a simple issue), an engineer (a bug) or a manager (an outage). If you write this as one method:

```csharp
if (ticket.Severity == Low && IsFaq(ticket)) Bot(...);
else if (ticket.Severity <= Medium) Operator(...);
else if (ticket.Severity == High) Engineer(...);
else Manager(...);
```

…then any new support line, a change of order, or a night shift "without the bot and the manager" requires editing this method. The sender is tightly coupled to all receivers.

**HTTP request processing.** Before a request reaches the business logic, you need to log it, check the rate limit, authenticate, and validate. Each check may **stop** the processing. If you write all of this in the controller, it grows and mixes different responsibilities.

## Solution

Turn each check/processing step into a separate **handler** object with a common interface. Handlers are linked into a chain: each one knows only about the **next** one. The client sends the request to the **first** handler and does not know who will eventually process it.

There are two variants of chain behavior:
1. **Classic (GoF):** the request is processed by **one** handler — the first one that can; the rest simply pass it on (the `SupportTickets` example).
2. **Pipeline (middleware):** **each** handler does its part of the work and decides whether to call the next one; any of them can break the chain (the `Middleware` example).

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Handler** | `SupportHandler`, `IRequestHandler` | The common handler interface. Often holds a reference to the next handler (or receives it as a delegate) |
| **ConcreteHandler** | `ChatBotHandler`, `OperatorHandler`, `EngineerHandler`, `ManagerHandler`, `LoggingHandler`, `RateLimitHandler`, `AuthenticationHandler`, `ValidationHandler`, `EndpointHandler` | Decides whether it can handle the request; if not, passes it on |
| **Request** | `SupportTicket`, `HttpRequestContext` | The data that travels along the chain |
| **Client** | demo classes, `RequestPipeline` | Assembles the chain and sends the request to the first link |

## Diagram

```text
 Client ──► ChatBot ──► Operator ──► Engineer ──► Manager ──► (null)
              │            │            │            │
          Low+FAQ?     ≤ Medium?     High?       Critical?
           handle       handle       handle       handle
```

---

## Example 1. Customer support (`SupportTickets/`) — classic chain

**Request** — `SupportTicket(Id, Description, Severity)` (a `record`); the severity is `TicketSeverity`.

**Handler** — the abstract `SupportHandler` holds a reference to the next handler and implements the "default" behavior — pass the request on:

```csharp
public SupportHandler SetNext(SupportHandler next)
{
    _next = next;
    return next;           // return next so the chain can be built "in a line"
}

public virtual void Handle(SupportTicket ticket)
{
    if (_next is not null)
        _next.Handle(ticket);
    else
        Console.WriteLine($"Nobody could handle ticket #{ticket.Id}");
}
```

**ConcreteHandler** (folder `Handlers/`) — each one checks "its own" condition; if it matches, it processes the ticket and **stops** the chain (`return`), otherwise it calls `base.Handle(ticket)`:

```csharp
public override void Handle(SupportTicket ticket)
{
    if (ticket.Severity <= TicketSeverity.Medium)
    {
        Console.WriteLine($"Operator: resolved ticket #{ticket.Id}");
        return;
    }
    base.Handle(ticket);
}
```

**Client** — assembles the chain `bot → operator → engineer → manager` and sends all tickets **only to the first link**. The output shows how far each ticket went. At the end a different configuration is shown ("night shift" — operator only): nobody took the critical ticket — this is a normal situation for the pattern, and it has to be anticipated.

## Example 2. Request processing pipeline (`Middleware/`) — as in ASP.NET Core

**Request** — `HttpRequestContext`: input data (path, IP, token, body) and the result (status code, text). Handlers can **enrich** the context — for example, authentication writes `UserName`.

**Handler** — `IRequestHandler.Handle(context, Action next)`. A handler does not store a reference to the next one but receives it as a **delegate** — this makes handlers completely independent.

**ConcreteHandler** (folder `Handlers/`):
- `LoggingHandler` — works **before and after** the rest of the chain (logs the request, calls `next()`, logs the response);
- `RateLimitHandler` — after N requests from the same IP responds with `429` and **does not call** `next()`;
- `AuthenticationHandler` — checks the token (`401`), otherwise writes the user into the context;
- `ValidationHandler` — validates the body (`400`);
- `EndpointHandler` — the endpoint that does the actual work (`201`).

**Client** — `RequestPipeline` assembles the chain with the `Use(...)` method and links the handlers recursively:

```csharp
private void Invoke(int index, HttpRequestContext context)
{
    if (index >= _handlers.Count) return;
    _handlers[index].Handle(context, () => Invoke(index + 1, context));
}
```

This is a simplified copy of how `app.Use(...)` works in ASP.NET Core: each middleware receives `next` and decides itself whether to pass the request on. `DelegatingHandler` in `HttpClient` and pipeline behaviors in MediatR are chains as well.

---

## When to use

- A request can be handled by different objects, and it is not known in advance which one.
- The set and order of handlers must be configurable dynamically.
- You need to execute several independent checks/steps in turn with the ability to stop.
- You need to decouple the sender of a request from its receivers.

## When not to use

- There is always one handler and it is known in advance.
- The order and set of steps never change — simple sequential code is easier to read.

## Pros and cons

**Pros**
- Reduces coupling between the sender and the receivers.
- Each handler is responsible for one thing (SRP).
- New handlers are added without changing existing ones (Open/Closed).
- The chain can be rebuilt at runtime.

**Cons**
- A request may remain unhandled.
- Harder to debug: the request's path is not visible in one place in the code.
- The order of links matters (e.g. logging must come first, and authentication before validation).

## Relations with other patterns

- **Decorator** has a similar structure (a chain of wrappers), but a decorator **always** passes the call on and adds behavior; a chain link can **stop** the processing.
- **Command** — a request in a chain is often implemented as a command.
- **Composite** — a chain is often built along a tree: the request goes from an element to its parent (e.g. event handling in a UI).
- **Mediator** and **Observer** — other ways to connect senders and receivers: through a mediator or through subscription.
