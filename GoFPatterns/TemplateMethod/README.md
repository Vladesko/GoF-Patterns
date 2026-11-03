🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/TemplateMethod) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/TemplateMethod) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/TemplateMethod) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/TemplateMethod)

# Template Method

**Type:** behavioral pattern.

**Template Method** defines the **skeleton of an algorithm** in a base class method, deferring the implementation of individual steps to subclasses. Subclasses can override the steps but **cannot change the structure** of the algorithm.

---

## The problem

You need to import customers from CSV and from JSON. The process is the same in both cases: prepare the data → parse → validate → save → (do something after the import). Only the **parsing** step differs.

If you write two independent classes, `CsvImporter` and `JsonImporter`:
- validation and saving are **duplicated**;
- you fix a validation bug in one class and forget the other;
- when XML appears, you have to copy everything a third time;
- nothing guarantees that all importers perform the steps in the same order.

## Solution

Move the common algorithm into a **base class** as a method (the template method) that calls the steps in order. There are three kinds of steps:

- **abstract** — every subclass must implement them (`Parse`);
- **common** — implemented in the base class and the same for everyone (`Save`);
- **hooks** — `virtual` methods with a default implementation (often empty) that a subclass **may** override to "plug into" the algorithm (`Preprocess`, `AfterImport`).

The template method itself is **not virtual** — subclasses cannot change the order of the steps.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **AbstractClass** | `CustomerImporter`, `HotBeverage` | Contains the template method (`Import`, `Prepare`) and declares the steps: abstract, common and hooks |
| **ConcreteClass** | `CsvCustomerImporter`, `JsonCustomerImporter`, `PartnerCsvCustomerImporter`, `Tea`, `GreenTea`, `Coffee` | Implements the abstract steps and overrides hooks if needed. Does not touch the algorithm itself |

## Diagram

```text
 ┌──────────────────────────────────────────┐
 │ «abstract» CustomerImporter              │
 │ + Import(raw)          ← template method │
 │     Preprocess()       ← hook (virtual)  │
 │     Parse()            ← abstract        │
 │     Validate()         ← virtual         │
 │     Save()             ← common (private)│
 │     AfterImport()      ← hook (empty)    │
 └──────────────────────────────────────────┘
          ▲                         ▲
 ┌──────────────────────┐  ┌──────────────────────┐
 │ CsvCustomerImporter  │  │ JsonCustomerImporter │
 │ Preprocess(): skip   │  │ Parse(): JSON        │
 │   header             │  │ AfterImport(): email │
 │ Parse(): CSV         │  └──────────────────────┘
 └──────────────────────┘
          ▲
 ┌────────────────────────────┐
 │ PartnerCsvCustomerImporter │
 │ Validate(): stricter rules │
 └────────────────────────────┘
```

---

## Example 1. Customer import (`DataImport/`)

**AbstractClass** — `CustomerImporter`. The template method `Import`:

```csharp
public IReadOnlyList<Customer> Import(string rawData)
{
    var content = Preprocess(rawData);          // hook
    var records = Parse(content);               // abstract
    var valid = records.Where(Validate).ToList();   // virtual
    Save(valid);                                // common step
    AfterImport(valid);                         // hook
    return valid;
}
```

- `Parse` is abstract; each format has its own.
- `Validate` is a common check (the email contains `@`, the age is within reasonable limits), but it can be made stricter.
- `Save` is `private`: a subclass can neither override this step nor call it separately.
- `Preprocess` and `AfterImport` are hooks.

**ConcreteClass** (folder `Importers/`):
- `CsvCustomerImporter` — parses lines separated by `;` and overrides the `Preprocess` hook to cut off the header line;
- `JsonCustomerImporter` — parses JSON via `System.Text.Json` and overrides the `AfterImport` hook (sending welcome emails);
- `PartnerCsvCustomerImporter` — inherits from the **CSV importer** and only makes `Validate` stricter (adults with a corporate email), calling `base.Validate`. It gets the CSV parsing and the whole algorithm for free.

**Client** — the demo works with all importers through the base class and calls the same `Import`. The output shows that the steps are the same; only the details differ.

## Example 2. Hot beverages (`Beverages/`) — the classic example with hooks

**AbstractClass** — `HotBeverage` with the template method `Prepare()`:

```csharp
public void Prepare()
{
    BoilWater();
    Brew();
    PourInCup();
    if (CustomerWantsCondiments())
        AddCondiments();
}
```

- `BoilWater`, `PourInCup` — common steps;
- `Brew`, `AddCondiments` — abstract;
- `WaterTemperature` and `CustomerWantsCondiments()` — **hooks**: by default 100°C and "yes, condiments are wanted".

**ConcreteClass** (folder `Drinks/`):
- `Tea` — only the mandatory steps;
- `GreenTea` — overrides the hooks: 80°C water and no condiments;
- `Coffee` — the `CustomerWantsCondiments()` hook depends on the customer's choice (a constructor parameter).

The hooks show how a subclass can **control the flow** of the algorithm (skip a step) without changing the algorithm itself.

---

## The "Hollywood principle"

*"Don't call us, we'll call you."* The base class calls the subclass methods, not the other way around. A subclass does not control the algorithm — it only provides the details when asked.

## When to use

- Several classes do the same thing and differ only in individual steps.
- You want to allow extending only certain parts of the algorithm without allowing its structure to change.
- You need to remove duplication of common code in similar classes.
- Examples: data import/export, report generation, request processing (in ASP.NET Core, `ControllerBase`, `BackgroundService.ExecuteAsync`), test frameworks (`SetUp` → test → `TearDown`).

## When not to use

- Algorithm variants need to be switched **at runtime** — Strategy is better.
- There are many steps and subclasses override almost everything — there is effectively no common algorithm.

## Pros and cons

**Pros**
- Common code in one place, no duplication.
- The structure of the algorithm is protected from changes.
- Subclasses override only what really differs.

**Cons**
- Tight coupling through **inheritance**: changes to the base class affect all subclasses.
- Limited by the structure of the skeleton — a new step in the middle requires changing the base class.
- The Liskov substitution principle can be violated if a subclass overrides a step "unexpectedly" (e.g. suppresses a mandatory step).
- The more steps there are, the harder it is to maintain.

## Relations with other patterns

- **Strategy** solves a similar problem through **composition** and lets you replace the whole algorithm at runtime. **Template Method** does it through **inheritance**; the variant is fixed at the class level.
- **Factory Method** is a special case of Template Method: a step of the algorithm is object creation. In this project, `NotificationFactory.Notify()` from `FactoryMethod/NotifySystem` is a template method that calls the factory method `CreateNotification()`.
