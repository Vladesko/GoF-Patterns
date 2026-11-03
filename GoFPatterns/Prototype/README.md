🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Prototype) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Prototype) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Prototype) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Prototype)

# Prototype

**Type:** creational pattern.

**Prototype** lets you create new objects by copying existing ones (prototypes) without depending on their concrete classes.

---

## The problem

You need a copy of an object. The naive way is to create an object of the same class and copy all fields into it. The problems:

- some fields may be **private** and inaccessible from the outside;
- the client must **know the concrete class** of the object, while it often has only an interface (`IDocumentPrototype`, `Unit`);
- the object may be **expensive to set up** (long initialization, loading configuration), and repeating that for every copy is costly.

## Solution

Delegate copying to the object itself. All copyable objects implement a common interface with a `Clone()` method. The object knows its own fields (including private ones) and decides itself how to copy them.

Often a **prototype registry** is added as well — a store of preconfigured objects from which the client takes copies by key.

### Shallow and deep copy

- **Shallow copy** (`MemberwiseClone()`) copies the fields "as is": value types are copied, while reference fields of the copy and the original point to **the same** objects.
- **Deep copy** also copies nested objects; the copy is completely independent.

For immutable objects (`string`, a `record` with init properties) the difference does not matter — they can be safely shared.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Prototype** | `IDocumentPrototype`, `Unit` | Declares the cloning method `Clone()` |
| **ConcretePrototype** | `Contract`, `Invoice`, `Archer`, `Mage` | Implements `Clone()` by copying its state |
| **Prototype Registry** | `DocumentTemplateRegistry` | Stores configured prototypes and hands out their copies by key (an optional participant) |
| **Client** | `Spawner`, `DemonstratePrototype` | Creates new objects by calling `Clone()` on a prototype |

## Diagram

```text
 Client ───────────► «interface» IDocumentPrototype
                        Clone(): IDocumentPrototype
                              ▲              ▲
                              │              │
                          Contract        Invoice
                     Clone() => new Contract(this)

 DocumentTemplateRegistry
   _templates: key ─► prototype
   Create(key) => _templates[key].Clone()
```

---

## Example 1. Document templates (`Documents/`)

**Task:** a document management system has templates (an NDA, an EU invoice). A new document is created by copying a template, after which the specific data is filled in.

**Prototype** — `IDocumentPrototype` with the `Clone()` method.

**ConcretePrototype** — `Contract` contains the reference fields `DocumentStyle Style` and `List<string> Sections`. Deep copying is implemented through a **private copy constructor** — this way the class has access to all its fields:

```csharp
private Contract(Contract source)
{
    Title = source.Title;
    Company = source.Company;
    Style = source.Style.Copy();       // copy of the nested object
    Sections = [.. source.Sections];   // copy of the list
}

public IDocumentPrototype Clone() => new Contract(this);
```

For comparison there is `ShallowClone()` via `MemberwiseClone()`. The demo shows that after changing `shallow.Style.FontSize` and `shallow.Sections`, **the original changes too**, because they share one style object and one list. Changes to the deep copy do not affect the original.

`Invoice` is the second prototype; it has its own set of fields and its own `Clone()` implementation.

**Registry** — `DocumentTemplateRegistry` stores templates by key and always returns a **copy** from `Create(key)`, so the template itself never gets corrupted:

```csharp
var ndaForAcme = (Contract)registry.Create("nda");
ndaForAcme.Company = "Acme";
```

## Example 2. Game unit spawner (`GameUnits/`)

**Task:** a game needs to spawn many similar units ("elite archer", "fire mage") whose settings are loaded from configuration.

**Prototype** — the abstract class `Unit` with `abstract Unit Clone()`.

**ConcretePrototype** — `Archer` and `Mage` (folder `Units/`) copy their specific fields; `Mage` copies its list of spells so that clones do not share it.

**Client** — `Spawner` receives **any** `Unit` and spawns new instances without knowing whether it is an archer or a mage:

```csharp
public Unit Spawn(Position position)
{
    var unit = prototype.Clone();
    unit.Name = $"{prototype.Name} #{++_counter}";
    unit.Position = position;
    return unit;
}
```

`Position` is a `record`. It is immutable, so it does not need to be copied and can be shared between clones. The demo also shows C#'s built-in mechanism for cloning records — the `with` expression: `start with { X = 7 }`.

---

## When to use

- Code should not depend on the concrete classes of the objects being copied.
- Creating an object is expensive, and many similar instances are needed.
- There is a set of typical configurations, and instead of many subclasses it is more convenient to store configured prototypes.

## When not to use

- The objects are simple and cheap — it is easier to call the constructor.
- The objects are immutable — they can be shared without copying.

## Pros and cons

**Pros**
- Cloning without binding to concrete classes.
- Less repetitive initialization code.
- An alternative to inheritance for obtaining "variations" of an object.

**Cons**
- Hard to clone objects with complex graphs (circular references, resources such as connections and files).
- You must pay close attention to shallow vs. deep copying.

## Relations with other patterns

- **Abstract Factory** can store a set of prototypes and clone them instead of creating products via `new`.
- **Memento** — for simple objects, a state snapshot can be made by cloning.
- **Composite** and **Decorator** — cloning helps copy complex structures built with these patterns.
- A prototype registry is often made a **Singleton**.

> .NET has the `ICloneable` interface, but its use is not recommended: it returns `object` and does not say whether the copy is deep or shallow. A typed interface of your own, like `IDocumentPrototype`, is better.
