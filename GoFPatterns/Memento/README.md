🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Memento) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Memento) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Memento) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Memento)

# Memento

**Type:** behavioral pattern.

**Memento** lets you **save and restore** the previous state of an object **without revealing** the details of its implementation and without violating encapsulation.

---

## The problem

You need to implement "Undo" in an editor or "Save/Load" in a game. To do this, copies of the object's state have to be stored somewhere.

The naive approach is to read all the object's fields from the outside and save them. But:
- the fields are usually **private** — you would have to make them public and break encapsulation;
- the code that saves the state starts **depending on the object's internal structure**: add a field — and the history code has to change;
- the state can be accidentally **modified** from the outside, and a rollback will return the object to an invalid state.

## Solution

Delegate the creation of the snapshot to **the object itself** — only it knows its full state. The object creates a **snapshot** (memento) — an immutable object with a copy of the state — and knows how to restore itself from it.

Snapshots are stored by a separate object — the **caretaker**. It decides *when* to save and restore, but **cannot look inside** the snapshot (or can read only its metadata).

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Originator** | `TextEditor`, `Hero` | The object whose state is saved. Creates snapshots and restores itself from them |
| **Memento** | `TextEditor.Snapshot` (via `IEditorMemento`), `HeroMemento` | An immutable snapshot of the state |
| **Caretaker** | `EditorHistory`, `SaveManager` | Stores snapshots and decides when to save/restore. Does not change the contents of snapshots |

## Diagram

```text
 ┌──────────────────────┐   Save()    ┌────────────────────────────┐
 │ TextEditor           │────────────►│ Snapshot  «private nested» │
 │ (Originator)         │             │ Text, Cursor, Font         │
 │ - _text, _cursor,    │◄────────────│ implements IEditorMemento  │
 │   _font              │  Restore()  └────────────────────────────┘
 └──────────────────────┘                       ▲
            ▲                                   │ sees only
            │ Save / Restore                    │ IEditorMemento
 ┌──────────────────────────┐                   │ (Label, CreatedAt)
 │ EditorHistory (Caretaker)│───────────────────┘
 │ Stack<IEditorMemento>    │
 └──────────────────────────┘
```

---

## Example 1. Text editor history (`TextEditorHistory/`) — strict encapsulation

**Originator** — `TextEditor` with private state: the text, the cursor position, the font. There are no public getters for these fields.

**Memento** — `Snapshot` is declared as a **private nested class** of the editor. The nested class sees everything inside the editor, but its type is not accessible from the outside — clients receive the snapshot only through the **narrow interface** `IEditorMemento` (a label and the creation time):

```csharp
public IEditorMemento Save(string label) => new Snapshot(label, _text, _cursor, _font);

public void Restore(IEditorMemento memento)
{
    if (memento is not Snapshot snapshot)
        throw new ArgumentException("Unknown memento type.");
    _text = snapshot.Text;
    _cursor = snapshot.Cursor;
    _font = snapshot.Font;
}

private sealed class Snapshot(...) : IEditorMemento { ... }
```

This is the C# equivalent of the "wide" and "narrow" interfaces from GoF: the editor sees the full contents of the snapshot, the caretaker only its metadata.

**Caretaker** — `EditorHistory` stores snapshots in a stack. `Backup` takes a snapshot from the editor, `Undo` pops the last one and passes it to the editor. The caretaker neither reads nor can modify the snapshot itself.

**Client** — the demo makes edits, saving the state before each one, and rolls back — the cursor and font are restored along with the text.

## Example 2. Game saves (`GameSave/`) — a snapshot as a `record`

**Originator** — `Hero`: level, health, gold, location, inventory. The properties are read-only from the outside (`private set`) and change through game actions (`Travel`, `Fight`, `Buy`).

**Memento** — `HeroMemento` is a `sealed record`. A C# record is a convenient way to describe an immutable snapshot. The inventory is **copied** into a new array (`[.. _inventory]`); otherwise the snapshot would share the list with the hero, and purchases made after saving would "leak" into the save.

**Caretaker** — `SaveManager` stores snapshots in named slots and can load them. Encapsulation is softer here: the snapshot is public, and the manager reads data from it for the list of saves. But it can **change** the hero only through `hero.Load(memento)`, and the snapshot itself is immutable.

**Client** — the demo plays part of the game, saves, loses to the boss and loads first the autosave and then an earlier save.

---

## When to use

- You need to store state snapshots for undo, transaction rollback, game saves, drafts.
- Accessing the object's state directly would expose implementation details.

## When not to use

- The object's state is simple and public — it is enough to copy it.
- The state is huge but changes little by little — storing full snapshots is expensive; it is better to store the changes (the Command approach with `Undo`).

## Pros and cons

**Pros**
- Does not violate the encapsulation of the original object.
- Simplifies the original object: it does not need to store the history.

**Cons**
- Memory consumption if there are many snapshots or they are large.
- The caretaker must manage the lifecycle of snapshots (delete outdated ones).
- In dynamically typed languages it is hard to guarantee that nobody modifies a snapshot; in C#, nested `private` classes and immutable `record`s help.

## Relations with other patterns

- **Command** + **Memento**: before execution a command saves a snapshot of the receiver, and `Undo` restores it. Command stores "how to undo", Memento stores "what it was".
- **Iterator** — a snapshot of the iteration position can be saved.
- **Prototype** — if the state is simple, a snapshot can be made by cloning the object.
- **State** — a snapshot may include the current state object.
