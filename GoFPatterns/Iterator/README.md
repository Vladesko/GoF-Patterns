🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Iterator) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Iterator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Iterator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Iterator)

# Iterator

**Type:** behavioral pattern.

**Iterator** provides a way to **traverse the elements** of a composite object (a collection) **sequentially without exposing its internal structure** — whether it is an array, a list, a tree or something else.

---

## The problem

Collections come in different shapes: an array, a linked list, a tree, a graph, a database query result. The client just needs to "go through all the elements", but:

- if the traversal is written in the client, the client is **tied to the collection's structure** (indexes for an array, recursion for a tree). Replace the array with a tree — and all clients have to be rewritten;
- there may be **several** traversals (forward, reverse, filtered, depth-first, breadth-first). If they are added to the collection itself, it accumulates responsibilities that are not its own;
- sometimes **several independent traversals at the same time** are needed — which means the traversal position cannot be stored in the collection itself.

## Solution

Move the traversal logic into a separate object — the **iterator**. The iterator:
- stores the **current position**;
- provides a simple interface: "is there a next one?" and "give me the next one";
- knows the collection's internal structure, while the client does not.

The collection (the **aggregate**) can create iterators for itself. Each call creates a new iterator with its own position.

### Iterator in .NET

In C# the pattern is built into the language:

| GoF | .NET |
|-----|------|
| Aggregate | `IEnumerable<T>` (`GetEnumerator()`) |
| Iterator | `IEnumerator<T>` (`MoveNext()`, `Current`, `Reset()`) |
| Client loop | `foreach` |
| Writing an iterator | `yield return` — the compiler generates the iterator class for you |

That is why in real code you usually do not write your own `IBookIterator` — you implement `IEnumerable<T>`. Both approaches are shown below.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Iterator** | `IBookIterator`, `IEnumerator<T>` | The traversal interface: `HasNext()`, `Next()`, `Reset()` |
| **ConcreteIterator** | `ForwardBookIterator`, `ReverseBookIterator`, `AuthorFilterIterator`, iterators generated from `yield return` | Implements a specific traversal algorithm and stores the current position |
| **Aggregate** | `IBookAggregate`, `IEnumerable<T>` | The interface of a collection that creates iterators |
| **ConcreteAggregate** | `Library`, `CategoryNode` | A collection that returns the required iterators |
| **Client** | demo classes | Traverses the collection only through an iterator |

## Diagram

```text
 Client ─────► «interface» IBookAggregate        «interface» IBookIterator
                 CreateIterator()        ─────►    HasNext(), Next(), Reset()
                        ▲                                   ▲
                        │                ┌──────────────────┼──────────────────┐
                     Library             │                  │                  │
                  (Book[] inside) ◄── ForwardBookIterator  ReverseBookIterator  AuthorFilterIterator
                                         _position          _position           _position, author
```

---

## Example 1. Book library (`BookCollection/`) — classic Iterator and `IEnumerable<T>`

**ConcreteAggregate** — `Library` stores books in an **array** that is grown manually (`Array.Resize`). Clients do not know this — the array can be replaced with anything without touching them.

**Iterator** — `IBookIterator` with the methods `HasNext()`, `Next()`, `Reset()`.

**ConcreteIterator** (folder `Iterators/`):
- `ForwardBookIterator` — from the first book to the last;
- `ReverseBookIterator` — from the last to the first;
- `AuthorFilterIterator` — skips books by other authors:

```csharp
public bool HasNext()
{
    while (_position < library.Count && library.GetAt(_position).Author != author)
        _position++;
    return _position < library.Count;
}
```

The iterators access the collection through the `internal` method `GetAt` — it is not visible to clients.

**Aggregate** — `IBookAggregate` declares factory methods for iterators; `Library` implements them.

**Client** — the `Print` method in the demo accepts **any** `IBookIterator` and traverses it with the same `while (iterator.HasNext())` loop.

`Library` also implements `IEnumerable<Book>` via `yield return`:

```csharp
public IEnumerator<Book> GetEnumerator()
{
    for (var i = 0; i < _count; i++)
        yield return _books[i];
}
```

This immediately gives you `foreach` and all of LINQ (`library.Where(...).OrderBy(...)`).

## Example 2. Category tree traversal (`Tree/`)

**ConcreteAggregate** — `CategoryNode`: a node of the catalog tree. The list of children is private.

**ConcreteIterator** — two traversal algorithms written with `yield return`:
- `DepthFirst()` — **depth-first** (like an indented menu). An explicit `Stack` is used instead of recursion so that a deep tree does not cause a `StackOverflowException`;
- `BreadthFirst()` — **breadth-first**, level by level, using a `Queue`.

```csharp
public IEnumerable<(CategoryNode Node, int Depth)> BreadthFirst()
{
    var queue = new Queue<(CategoryNode, int)>();
    queue.Enqueue((this, 0));
    while (queue.Count > 0)
    {
        var (node, depth) = queue.Dequeue();
        yield return (node, depth);
        foreach (var child in node._children)
            queue.Enqueue((child, depth + 1));
    }
}
```

**Client** — traverses the same tree in two ways using a regular `foreach`.

An important property of `yield`-based iterators is **laziness**: elements are computed one at a time, on demand. In the demo, the search `First(n => n.Name == "Laptops")` stops the traversal as soon as the node is found and visits only part of the tree.

---

## When to use

- You need to hide the internal structure of a complex collection from the client.
- You need several ways of traversing the same collection.
- You need a uniform traversal interface for different collections.
- You need lazy traversal (large or infinite sequences, paged loading from a database).

## When not to use

- The collection is simple (`List<T>`, an array) — use the built-in `foreach`.

## Pros and cons

**Pros**
- Simplifies collections: the traversal logic is extracted (SRP).
- Different traversal algorithms without changing the collection (Open/Closed).
- Several independent traversals at the same time.
- Lazy evaluation.

**Cons**
- Unnecessary complexity for simple collections.
- Modifying the collection during traversal is a separate problem (standard .NET collections throw `InvalidOperationException`).

## Relations with other patterns

- **Composite** — iterators are often used to traverse Composite trees.
- **Factory Method** — `CreateIterator()` in a collection is a factory method that returns the required iterator subclass.
- **Memento** — the iteration state can be saved and restored later.
- **Visitor** + Iterator: the iterator traverses the structure, and the visitor performs an operation on each element.
