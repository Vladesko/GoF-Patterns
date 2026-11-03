🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Flyweight) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Flyweight) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Flyweight) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Flyweight)

# Flyweight

**Type:** structural pattern.

**Flyweight** lets you fit a large number of objects into memory by **sharing common state** between them instead of storing the same data in every object.

---

## The problem

A game needs to render a forest of 100,000 trees. Each tree is an object with coordinates, a species name, a color and a **texture** (say, 1 MB). If each tree stores its own copy of the texture, it takes ~100 GB of memory, even though there are only four distinct species.

The same happens in a text editor: every character has a font, size, color and boldness. There are thousands of characters, but only a handful of unique styles.

The problem is that **most of the data is repeated** across many objects.

## Solution

Split the object's state into two parts:

- **Intrinsic** — the same for many objects and independent of context: species, color, texture; font, size. It is moved into a **flyweight** object that is created **once** and shared by everyone.
- **Extrinsic** — unique to each object: the tree's coordinates, the character and its position in the text. It is stored by the **context** (or the client) and passed to the flyweight in method parameters.

Flyweights must be **immutable** — after all, a single object is used by thousands of contexts. To avoid creating duplicates, flyweights are handed out by a **factory** with a pool (cache).

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Flyweight** | `TreeType`, `CharacterStyle` | Stores the intrinsic (shared) state. Immutable. Receives the extrinsic state through parameters |
| **FlyweightFactory** | `TreeTypeFactory`, `StyleFactory` | A pool of flyweights: returns an existing one or creates a new one |
| **Context** | `Tree`, `FormattedCharacter` | Stores the extrinsic state + a reference to the flyweight. There are many such objects, but they are small |
| **Client** | `ForestMap`, `TextDocument` | Creates contexts, obtaining flyweights only through the factory |

## Diagram

```text
 ForestMap (Client)
   │ PlantTree(x, y, "Oak", ...)
   ▼
 TreeTypeFactory ── _types: "Oak|dark green|oak.png" ─► TreeType ◄──┐
                                                       (1 MB)        │
                                                                     │ shared
   Tree(x=10, y=20) ─────────────────────────────────────────────────┤
   Tree(x=55, y=13) ─────────────────────────────────────────────────┤
   Tree(x=91, y=77) ─────────────────────────────────────────────────┘
   ... 100,000 small contexts, 4 flyweights
```

---

## Example 1. Forest (`Forest/`)

**Flyweight** — `TreeType`. Stores the name, color, texture path and the "heavy" texture data itself (a 1 MB array). The class is `sealed`, and all properties are read-only. Coordinates are **not stored** in it — they are passed into the method:

```csharp
public string Draw(int x, int y) => $"{Name} ({Color}) at [{x}, {y}] using {TexturePath}";
```

**FlyweightFactory** — `TreeTypeFactory` stores a `key → TreeType` dictionary. A new object is created only if there is no such one yet (the demo shows that the "created new tree type" message is printed exactly 4 times for 100,000 trees):

```csharp
if (!_types.TryGetValue(key, out var type))
{
    type = new TreeType(name, color, texturePath);
    _types[key] = type;
}
return type;
```

**Context** — `Tree`: two `int`s and a reference to a `TreeType`. The `Draw()` method passes its coordinates to the flyweight.

**Client** — `ForestMap` plants trees through the factory and never creates a `TreeType` itself.

The demo estimates the memory footprint: without flyweights ~100 GB, with flyweights ~5.5 MB (4 textures + 100,000 small contexts).

## Example 2. Text formatting (`TextFormatting/`)

**Flyweight** — `CharacterStyle` is a `sealed record` (font, size, boldness, color). A record is immutable and compared by value — which makes it convenient as a dictionary key.

**FlyweightFactory** — `StyleFactory` keeps a `Dictionary<CharacterStyle, CharacterStyle>` pool: if a style equal by value already exists, **the same object** is returned; otherwise the candidate is added to the pool.

**Context** — `FormattedCharacter` is a `readonly record struct` made of a character, a position and a reference to a style.

**Client** — `TextDocument.Append(...)` obtains a style from the factory and creates contexts for each character. The demo shows that 123 characters use only 3 style objects, and `CountDistinctStyleObjects()` verifies this by comparing **by reference** (`ReferenceEqualityComparer`).

At the end, a flyweight built into .NET is shown — **string interning**: `string.Intern` returns the same object for equal strings. String literals in C# are interned automatically.

---

## When to use

- The program has a **huge number** of similar objects, and they do not fit into memory.
- Most of the state of these objects can be moved outside or shared.
- The objects can be made immutable.
- Examples: particles and tiles in games, characters in editors, map markers, an icon cache.

## When not to use

- There are few objects — the savings are not worth the complexity.
- The objects' state is almost entirely unique — there is nothing to share.

## Pros and cons

**Pros**
- Significant memory savings.

**Cons**
- CPU time is spent on pool lookups and on passing the extrinsic state.
- The code becomes more complex: the state is "spread" between the context and the flyweight.
- Flyweights must be immutable — otherwise changing one affects all contexts.

## Relations with other patterns

- **Composite** — tree leaves are often made flyweights (e.g. characters in a document).
- **Singleton** is similar in that the object is shared, but a singleton is one instance per class, while there are many flyweights (one per unique intrinsic state), and they are immutable.
- **Factory Method** / object pool — a flyweight factory is essentially a cache.
- **State** and **Strategy** — state and strategy objects without fields of their own are often made flyweights.
