🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Singleton) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Singleton) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Singleton) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Singleton)

# Singleton

**Type:** creational pattern.

**Singleton** ensures that a class has **only one instance** and provides a **global point of access** to it.

---

## The problem

1. **A single instance.** Some resources must exist as a single instance for the whole application: configuration, a logger, a connection pool, a cache. If every service creates its own configuration, the file will be read many times, and changes made in one place will not be visible in another.
2. **Global access.** This instance must be accessible from different parts of the program, while not allowing it to be overwritten like a regular global variable.

## Solution

- Make the constructor **private** so that nobody can call `new`.
- Store the single instance in a **static field**.
- Expose it through a **static property** `Instance` that creates the object on first access (lazy initialization).
- Make the class `sealed` so that the restriction cannot be bypassed through a subclass.

The main difficulty is **multithreading**: if two threads access `Instance` at the same time, a naive implementation will create two objects.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Singleton** | `AppConfiguration`, `FileLogger` | Stores the single instance, hides the constructor, exposes the instance through `Instance` |
| **Client** | `CartService`, `PriceService`, demo classes | Accesses the singleton via `ClassName.Instance` |

The pattern essentially has one "actor" — the singleton class itself.

## Diagram

```text
 ┌───────────────────────────────────────┐
 │ AppConfiguration  «sealed»            │
 │ - static _instance: Lazy<AppConfig>   │
 │ - AppConfiguration()   ← private      │
 │ + static Instance: AppConfiguration   │
 │ + Get(key), Set(key, value)           │
 └───────────────────────────────────────┘
          ▲                 ▲
          │ .Instance       │ .Instance
     CartService       PriceService       ← the same object
```

---

## Example 1. Application configuration (`Configuration/`) — using `Lazy<T>`

**Task:** settings (connection string, currency, cart limit) are loaded from a file once and used by different services.

**Singleton** — `AppConfiguration`:

```csharp
public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

    public static AppConfiguration Instance => _instance.Value;

    private AppConfiguration() { /* expensive loading of settings */ }
}
```

By default `Lazy<T>` works in `ExecutionAndPublication` mode: the factory is called **exactly once**, even if several threads access `Value` at the same time. This is the recommended approach in C#.

**Clients** — `CartService` and `PriceService` (folder `Services/`) are not related to each other in any way, but both get `AppConfiguration.Instance`. The demo shows that:
- the loading message is printed only on first access (laziness);
- `InstanceId` is the same for both services;
- changing the currency via `Set` is immediately visible in `PriceService`;
- 100 parallel accesses yield 1 unique instance.

## Example 2. Logger (`Logger/`) — manual thread safety

**Task:** show why the naive implementation is dangerous and how classic double-checked locking works.

**Anti-example** — `NaiveLogger`:

```csharp
if (_instance is null)
{
    _instance = new NaiveLogger();   // two threads can end up here at the same time
}
```

A slow constructor (`Thread.Sleep`) makes the race visible: the demo starts 10 threads, and several instances are created instead of one.

**Singleton** — `FileLogger` with **double-checked locking**:

```csharp
if (_instance is null)              // 1st check: fast, without locking
{
    lock (_lock)
    {
        if (_instance is null)      // 2nd check: another thread may have already created the object
        {
            _instance = new FileLogger();
        }
    }
}
```

The field is marked `volatile` so that other threads do not see a partially constructed object. The `CreatedInstances` counter shows that exactly one instance was created, and all messages from different threads go to the same logger.

---

## Implementation options in C#

| Approach | Thread safety | Laziness | Comment |
|----------|---------------|----------|---------|
| `if (_instance == null)` without lock | ❌ | ✅ | Only for single-threaded code |
| `lock` + double check | ✅ | ✅ | Classic, but easy to get wrong |
| `static readonly Instance = new()` | ✅ | almost | Created on first access to the class (guaranteed by the CLR) |
| `Lazy<T>` | ✅ | ✅ | The recommended approach |
| DI container `AddSingleton<T>()` | ✅ | ✅ | In ASP.NET Core preferable to a hand-written Singleton |

## When to use

- The object really must be unique (configuration, logger, cache, pool).
- You need a strict guarantee of uniqueness, not just an "agreement to create only one".

## When not to use

- If there is a DI container, it is better to register the service as a singleton (`services.AddSingleton<IConfig, AppConfig>()`) and obtain it through an interface: it is the same uniqueness, but without global state and with the ability to substitute it in tests.
- If a "singleton" is needed only to avoid passing a dependency as a parameter, it is a hidden dependency, not a reason for the pattern.

## Pros and cons

**Pros**
- A guarantee of a single instance.
- A global point of access.
- Lazy initialization — the object is created only when needed.

**Cons**
- Violates SRP: the class both does its work and manages its own lifecycle.
- Hidden dependencies: the signature of `CartService` does not show that it depends on the configuration.
- Hard to test: global state survives between tests and cannot be replaced with a mock.
- Requires care in a multithreaded environment.

## Relations with other patterns

- **Facade** is often made a singleton — one facade is usually enough.
- **Abstract Factory**, **Builder**, **Prototype** (a prototype registry) can be implemented as a Singleton.
- **Flyweight** is similar in that it shares objects, but it may have many instances with different intrinsic state, and they are immutable; a Singleton is exactly one, and it can be mutable.
