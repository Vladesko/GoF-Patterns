🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Proxy) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Proxy) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Proxy) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Proxy)

# Proxy

**Type:** structural pattern.

**Proxy** is a substitute object that implements **the same interface** as the real object and **controls access** to it: it intercepts calls and can do something before or after forwarding the call to the original (or not forward it at all).

---

## The problem

There is an object that cannot or should not be accessed directly:

- it is **heavy** — a gallery of 100 photos of 20 MB each should not load all of them when opened if the user is going to look at only two;
- it needs **access control** — only the owner may withdraw money from an account;
- its calls are **expensive**, and the result is worth **caching** — a balance request to the core banking system;
- calls need to be **logged/audited**;
- the object is **remote** (another service, another machine).

Adding all of this to the class itself is bad: it becomes responsible for security, caching and logging instead of its business logic. And often the class cannot be changed at all (third-party code).

## Solution

Create a proxy class with the same interface. The proxy holds a reference to the real object (or creates it itself when needed) and delegates work to it, adding its own logic. The client works with the interface and **does not notice the substitution**.

### Kinds of proxies

| Kind | What it does | Example in the project |
|------|--------------|------------------------|
| **Virtual proxy** | Defers creation of a heavy object until the first access | `LazyImageProxy` |
| **Protection proxy** | Checks access rights | `ProtectionAccountProxy` |
| **Caching proxy** | Caches results | `CachingAccountProxy` |
| **Logging proxy** | Logs calls | `LoggingAccountProxy` |
| **Remote proxy** | Represents an object from another address space (gRPC/WCF clients, `HttpClient` wrappers) | — |
| **Smart reference** | Reference counting, releasing resources when the object is no longer needed | — |

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Subject** | `IImage`, `IBankAccount` | The common interface of the real object and the proxy. Thanks to it, the proxy can be substituted for the original |
| **RealSubject** | `HighResolutionImage`, `BankAccount` | The real object with useful business logic |
| **Proxy** | `LazyImageProxy`, `ProtectionAccountProxy`, `CachingAccountProxy`, `LoggingAccountProxy` | Holds a reference to RealSubject, controls access to it, may manage its lifecycle |
| **Client** | demo classes | Works through Subject and does not know whether it deals with the original or a proxy |

## Diagram

```text
               ┌──────────────────────┐
  Client ─────►│ «interface» IImage   │
               │ Display()            │
               └──────────────────────┘
                    ▲             ▲
                    │             │
     ┌──────────────────┐   ┌───────────────────────┐
     │ LazyImageProxy   │──►│ HighResolutionImage   │
     │ - _realImage?    │   │ (heavy, loaded in     │
     │ Display():       │   │  the constructor)     │
     │  create if null, │   └───────────────────────┘
     │  then delegate   │
     └──────────────────┘
```

---

## Example 1. Lazy image loading (`ImageLoading/`) — virtual proxy

**Subject** — `IImage` (`FileName`, `Display()`).

**RealSubject** — `HighResolutionImage` loads the image from disk right in its constructor (simulated with `Thread.Sleep`). Just creating such an object is already expensive.

**Proxy** — `LazyImageProxy` stores only the file name and creates the real object on the **first** call to `Display()`:

```csharp
public void Display()
{
    _realImage ??= new HighResolutionImage(FileName);
    _realImage.Display();
}
```

**Client** — the demo opens a gallery of 5 images in two ways:
- without a proxy — all 5 files are loaded at once (~1 s);
- with a proxy — the gallery opens instantly, and only the 2 images the user opened are loaded. Displaying an image again does not load the file anymore.

In .NET a similar role is played by `Lazy<T>`, and in Entity Framework by **lazy-loading proxies** for navigation properties.

## Example 2. Bank account (`BankAccount/`) — protection, caching, logging

**Subject** — `IBankAccount` (`GetBalance()`, `Withdraw()`).

**RealSubject** — `BankAccount` contains **only** business logic. The balance request is slow (simulating a call to the core banking system), and the `BalanceRequests` counter shows how many times the bank was actually called.

**Proxies** (folder `Proxies/`):

`ProtectionAccountProxy` — only `UserRole.Owner` can withdraw money:

```csharp
public void Withdraw(decimal amount)
{
    if (role != UserRole.Owner)
        throw new UnauthorizedAccessException($"{role} can't withdraw money.");
    account.Withdraw(amount);
}
```

`CachingAccountProxy` — remembers the balance and **invalidates the cache** after a withdrawal:

```csharp
public decimal GetBalance() => _cachedBalance ??= account.GetBalance();
```

`LoggingAccountProxy` — writes an audit log of all operations, including failed ones.

**Client** — since all proxies implement `IBankAccount`, they can be nested in each other:

```csharp
IBankAccount ownerAccess = new LoggingAccountProxy(
    new ProtectionAccountProxy(
        new CachingAccountProxy(realAccount), UserRole.Owner));
```

The demo shows that three balance requests result in **one** actual call to the bank; after a withdrawal the cache is invalidated and the balance is up to date; a guest is denied, and this ends up in the audit log.

---

## When to use

- Lazy initialization of a heavy object (virtual proxy).
- Access control (protection proxy).
- Caching results (caching proxy).
- Logging, metrics, auditing of calls.
- Working with a remote object as if it were local (remote proxy).

## When not to use

- The object is lightweight and needs no control — an unnecessary level of indirection.
- You do not need to control but rather to **add** new behavior that the client chooses and combines itself — that is closer to Decorator.

## Pros and cons

**Pros**
- Control over the object, transparent to the client.
- The lifecycle of the real object can be managed (create it later, release it earlier).
- Works even if the real object has not been created yet or is unavailable.
- Open/Closed: new proxies are added without changing the service or the client.

**Cons**
- An extra level of indirection; responses may be delayed.
- The number of classes grows.

## Relations with other patterns

- **Adapter** provides a **different** interface; **Proxy** provides **the same** one; **Decorator** provides the same one, but extended.
- **Decorator** and **Proxy** are structured almost identically (a wrapper + delegation); the difference is in intent: a decorator **adds** behavior, and the client determines its composition; a proxy **controls access** and usually manages the object's lifecycle itself (like `LazyImageProxy`, which creates `HighResolutionImage` itself).
- **Facade** also hides an object behind itself, but a facade has its own simplified interface, while a proxy has the interface of the original.
