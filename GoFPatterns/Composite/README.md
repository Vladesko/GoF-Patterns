🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Composite) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Composite) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Composite) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Composite)

# Composite

**Type:** structural pattern.

**Composite** lets you group objects into a **tree structure** and work with it as if it were a single object. The client treats an individual element and a whole group in the same way.

---

## The problem

There is a "part — whole" hierarchy: files and folders, employees and departments, products and boxes of products, menu items and submenus.

You need to compute, for example, the size of a folder. Without the pattern the client has to:
- check the type of each element (`if (item is File) ... else if (item is Folder) ...`);
- write the tree traversal with nesting itself;
- rewrite this code every time a new element type appears (e.g. a shortcut or an archive).

## Solution

Introduce a **common interface** for simple and composite elements. This interface has a method that makes sense for both (`GetSize()`):

- a **leaf** (file) simply returns its own size;
- a **container** (folder) calls `GetSize()` on all its children and sums up the result.

Since the children also implement the common interface, the recursion comes naturally, and the client calls one method on the root without thinking about what is inside.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Component** | `FileSystemItem`, `IOrgUnit` | The common interface for simple and composite elements |
| **Leaf** | `FileItem`, `Employee` | A simple element without children. Does the actual work |
| **Composite** | `FolderItem`, `Department` | A container. Stores children (of type Component), delegates work to them and combines the result |
| **Client** | demo classes | Works with all elements through Component |

## Diagram

```text
           ┌──────────────────────────┐
 Client ──►│ «abstract» FileSystemItem │◄──────────────┐
           │ + GetSize()              │                │ children
           │ + Print()                │                │ 0..*
           └──────────────────────────┘                │
                 ▲                ▲                    │
                 │                │                    │
          ┌──────────┐     ┌──────────────────────┐    │
          │ FileItem │     │ FolderItem           │◆───┘
          │ (Leaf)   │     │ + Add(), Remove()    │
          └──────────┘     │ GetSize() = Σ children│
                           └──────────────────────┘
```

---

## Example 1. File system (`FileSystem/`)

**Component** — the abstract `FileSystemItem` with the operations `GetSize()`, `Print()` and `Find()`.

**Leaf** — `FileItem` (folder `Items/`) knows its size and checks only itself when searching:

```csharp
public override long GetSize() => size;

public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
    predicate(this) ? [this] : [];
```

**Composite** — `FolderItem` stores a list of children and **delegates** all operations to them:

```csharp
public override long GetSize() => _children.Sum(c => c.GetSize());

public override void Print(string indent = "")
{
    Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} B)");
    foreach (var child in _children)
        child.Print(indent + "    ");
}
```

The `Add` method returns `this`, so the tree can conveniently be built as a "ladder" right in the demo.

**Client** — the demo calls `GetSize()` both on a single file and on the whole project — the calling code is the same. `Find(i => i.Name.EndsWith(".cs"))` recursively finds all C# files at any depth.

### Where to put `Add`/`Remove`

Here the child management methods exist **only in `FolderItem`** — this is the "safe" variant: you cannot call `Add` on a file. The GoF book also describes the "transparent" variant, where `Add`/`Remove` are declared in Component: then the client does not distinguish elements at all, but a leaf has to throw an exception on `Add`.

## Example 2. Company structure (`OrgStructure/`)

**Task:** compute the headcount and payroll budget for any unit — from a single employee to the whole company.

**Component** — the `IOrgUnit` interface (`GetSalaryBudget()`, `GetHeadcount()`, `Display()`).

**Leaf** — `Employee`: headcount 1, the budget is their salary.

**Composite** — `Department`: sums up the values of all nested employees and departments. The `Development` department contains both an employee (the CTO) and two sub-departments — leaves and containers can be mixed on the same level.

```csharp
public decimal GetSalaryBudget() => _units.Sum(u => u.GetSalaryBudget());
public int GetHeadcount() => _units.Sum(u => u.GetHeadcount());
```

**Client** — in the demo the array `IOrgUnit[] units = [company, development, backend, new Employee(...)]` is processed in a single loop, even though it contains both the whole company and a single intern.

---

## When to use

- You need to represent a "part — whole" hierarchy as a tree.
- The client must work uniformly with both simple and composite objects.
- Real-life examples: a file system, UI components (containers and elements), menus, XML/HTML DOM, expressions in a calculator, an order made of products and bundles.

## When not to use

- The structure is flat, there is no nesting.
- Leaves and containers behave too differently, and the common interface becomes artificial.

## Pros and cons

**Pros**
- Simplifies the client: no need to check element types.
- Easy to add new kinds of elements (Open/Closed).
- Complex operations on the tree are written recursively and concisely.

**Cons**
- The common interface can become too general: it ends up with methods that make no sense for some elements.
- It is hard to restrict which elements can be put into a container (e.g. "only files of a certain type in a folder") — runtime checks are required.

## Relations with other patterns

- **Builder** is convenient for building complex Composite trees step by step.
- **Iterator** — for traversing a Composite tree (see the `Iterator/Tree` example).
- **Visitor** — for performing operations on all tree nodes without changing their classes.
- **Chain of Responsibility** often works on top of Composite: a request is passed from a leaf up to its parents.
- **Flyweight** — tree leaves can be made flyweights to save memory.
- **Decorator** is structurally similar (recursive composition), but a decorator has one nested object and aims to add behavior, while a composite has many children and aims to combine the result.
