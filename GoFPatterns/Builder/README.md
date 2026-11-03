🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Builder) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Builder) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Builder) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Builder)

# Builder

**Type:** creational pattern.

**Builder** lets you construct complex objects step by step. The same construction process can produce different representations of an object, and the client does not need to know how exactly the object is assembled.

---

## The problem

Imagine an object with many parameters, some of which are optional and some depend on each other (a pizza: size, dough, sauce, toppings, cheese; an HTTP request: method, URL, headers, body, timeout).

Without the pattern you usually end up with one of two things:

- **A "telescoping" constructor** — `new Pizza("Large", "thin", "tomato", null, null, true, ...)`. It is unclear which argument means what, and every combination needs its own overload.
- **An object with public setters** — the object can be obtained "half-built" and in an invalid state, and validation is scattered all over the code.

## Solution

Move the construction process out of the object itself into a separate class — the **builder**. The builder provides a set of steps (`SetSize`, `AddTopping`, `WithHeader`...), the client calls only the ones it needs, and at the end receives the finished result (`GetResult()` / `Build()`), which can be validated before it is handed out.

If the same sequences of steps are used often, they are moved into a **director** — a class that knows the "recipes" and drives the builder.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Builder** | `IPizzaBuilder` | An interface declaring the construction steps common to all builders |
| **ConcreteBuilder** | `ItalianPizzaBuilder`, `AmericanPizzaBuilder`, `HttpRequestBuilder` | Implements the steps in its own way and holds the object being built; returns the result |
| **Product** | `Pizza`, `HttpRequestMessageModel` | The complex object being built. Products of different builders do not have to share a common interface |
| **Director** | `PizzaDirector` | Knows the order of steps for typical configurations. Works with the builder only through the interface. An optional participant |
| **Client** | `DemonstratePizzaBuilder`, `DemonstrateHttpRequestBuilder` | Creates a builder and passes it to the director or calls the steps itself |

## Diagram

```text
 Client ──► Director ──────────► «interface» IPizzaBuilder
              MakeMargherita()      Reset(), SetSize(), SetDough(),
              MakePepperoni()       SetSauce(), AddTopping(), GetResult()
                                          ▲                 ▲
                                          │                 │
                              ItalianPizzaBuilder   AmericanPizzaBuilder
                                          │                 │
                                          └──── creates ────┴──► Pizza
```

---

## Example 1. Pizzeria (`PizzaShop/`) — classic Builder with a director

**Task:** a pizzeria makes the same recipes (margherita, pepperoni) in different styles. Italian pizza — thin dough, parmesan; American pizza — thick dough, BBQ sauce, double toppings, cheddar.

**Product** — `Pizza`: a regular class with properties and a list of toppings.

**Builder** — `IPizzaBuilder` declares the steps:

```csharp
public interface IPizzaBuilder
{
    void Reset();
    void SetSize(string size);
    void SetDough(string dough);
    void SetSauce(string sauce);
    void AddTopping(string topping);
    void AddExtraCheese();
    Pizza GetResult();
}
```

**ConcreteBuilder** — each builder interprets the steps in its own way. For example, `AmericanPizzaBuilder` makes the dough thick and doubles the toppings:

```csharp
public void SetDough(string dough) => _pizza.Dough = $"thick {dough}";
public void AddTopping(string topping) => _pizza.Toppings.Add($"double {topping}");
```

`GetResult()` returns the assembled pizza and immediately calls `Reset()` so that the builder is ready for the next order.

**Director** — `PizzaDirector` stores the "recipes". It does not know which builder it has been given and calls only interface methods:

```csharp
public Pizza MakeMargherita()
{
    _builder.Reset();
    _builder.SetSize("Medium");
    _builder.SetDough("wheat");
    _builder.SetSauce("tomato");
    _builder.AddTopping("mozzarella");
    _builder.AddTopping("basil");
    return _builder.GetResult();
}
```

**Client** — `DemonstratePizzaBuilder` shows three things:
1. the same recipe with `ItalianPizzaBuilder` and `AmericanPizzaBuilder` yields different pizzas;
2. the director's builder can be replaced via `ChangeBuilder`;
3. the builder can also be used without a director — to assemble a "custom" pizza manually.

## Example 2. HTTP request (`HttpRequest/`) — fluent Builder

**Task:** build an HTTP request in which almost all parameters are optional, while the request itself must be immutable once created.

This is the most common variant of the pattern in C# (cf. `StringBuilder`, `UriBuilder`, `WebApplication.CreateBuilder`, `HostBuilder`). There is no director here, and no builder interface is needed — there is only one builder.

**Product** — `HttpRequestMessageModel`: all properties are read-only and the constructor is `internal`, so the object can be obtained "from outside" only through the builder.

**ConcreteBuilder** — `HttpRequestBuilder`. Each step returns `this`, so the calls can be chained:

```csharp
var request = new HttpRequestBuilder()
    .Post("https://api.shop.com/orders")
    .WithBearerToken("secret-token")
    .WithJsonBody("{ \"productId\": 42 }")
    .WithTimeout(TimeSpan.FromSeconds(5))
    .Build();
```

Steps can be "composite": `WithJsonBody` not only sets the body but also sets the `Content-Type` header, and `WithBearerToken` builds the `Authorization` header.

`Build()` is the only place where validity is checked (a URL is present, a GET has no body). Therefore an invalid object simply cannot be obtained — the demo shows the exception thrown when trying to build a request without a URL.

---

## When to use

- An object has many optional parameters and its constructor turns into a "telescope".
- You need to create different representations of the same object using the same steps.
- The object must be immutable but assembled gradually.
- You need to build composite structures (Composite trees, documents, requests) step by step.

## When not to use

- The object has 2–3 parameters — a constructor or C# `required init` properties are enough.
- The object is simple and does not require checking parameter consistency.

## Pros and cons

**Pros**
- Step-by-step construction; steps can be deferred or executed recursively.
- The same construction code for different products.
- Isolates complex assembly logic from the product's business logic (SRP).
- Makes it possible to guarantee the validity and immutability of the result.

**Cons**
- More classes (builders, director).
- The client is tied to concrete builders if they have no common interface.

## Relations with other patterns

- **Abstract Factory** creates families of objects and returns them immediately; **Builder** assembles one complex object step by step and returns it at the end.
- **Composite** — Builder is often the one that assembles its trees.
- **Factory Method** often evolves into Builder when there are too many parameters.
- A builder can be a **Singleton** if it holds no state.
