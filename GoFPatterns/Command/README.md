🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Command) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Command) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Command) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Command)

# Command

**Type:** behavioral pattern.

**Command** turns a request (an action) into a **standalone object** that contains all information about the request. Such an object can be passed as a parameter, put into a queue, logged, executed later and **undone**.

---

## The problem

**Text editor.** There are "insert", "delete" and "replace" operations that are invoked from the menu, hotkeys and the toolbar. They must support **undo and redo**. If a button calls `document.Delete(...)` directly, then:
- the calling code is duplicated everywhere (menu, button, hotkey);
- it is unclear how to undo the action: the deleted text is already lost;
- there is no single place where the history is stored.

**Smart home.** A remote with buttons to which the user assigns actions: "turn on the light", "leave home" (turn everything off + arm the alarm). The remote must not know about all devices, and scenarios should be assembled from simple actions.

## Solution

Implement each action as an object with a common interface (`Execute()` and, if needed, `Undo()`). A command stores:
- a reference to the **receiver** — the object that actually does the work;
- all call **parameters**;
- data for **undoing** (e.g. the deleted text).

The **invoker** (a button, a remote, a history) works only with the command interface and knows neither the receiver nor the details of the operation.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Command** | `ICommand`, `IHomeCommand` | The command interface: `Execute()`, sometimes `Undo()` |
| **ConcreteCommand** | `InsertTextCommand`, `DeleteTextCommand`, `ReplaceTextCommand`, `ActionCommand`, `MacroCommand` | Stores the receiver and the parameters; implements `Execute` by calling the receiver's methods |
| **Receiver** | `Document`, `Light`, `Thermostat`, `SecuritySystem` | Contains the business logic. Knows nothing about commands |
| **Invoker** | `CommandHistory`, `RemoteControl` | Runs commands; may store their history or a queue |
| **Client** | demo classes | Creates commands, binds them to receivers and passes them to the invoker |

## Diagram

```text
 Client ── creates ──► InsertTextCommand(document, pos, text)
                               │
                               ▼
 CommandHistory (Invoker)   «interface» ICommand          Document (Receiver)
   _undo: Stack<ICommand> ─► Execute()  ──── calls ────►   Insert(pos, text)
   _redo: Stack<ICommand>    Undo()     ──── calls ────►   Delete(pos, len)
```

---

## Example 1. Text editor with Undo/Redo (`TextEditor/`)

**Receiver** — `Document`: a wrapper around `StringBuilder` with `Insert`, `Delete` and `GetText` methods. It knows nothing about commands or history.

**Command** — `ICommand` with `Execute()`, `Undo()` and `Name` (for output).

**ConcreteCommand** (folder `Commands/`):

`InsertTextCommand` — undoing an insert = deleting the inserted text:

```csharp
public void Execute() => document.Insert(position, text);
public void Undo() => document.Delete(position, text.Length);
```

`DeleteTextCommand` — **remembers** the text being deleted before deleting it; otherwise undoing would be impossible:

```csharp
public void Execute()
{
    _deletedText = document.GetText(position, length);
    document.Delete(position, length);
}
public void Undo() => document.Insert(position, _deletedText);
```

`ReplaceTextCommand` — a **composite** command made of a delete and an insert; it undoes them in reverse order.

**Invoker** — `CommandHistory` keeps two stacks:

```csharp
public void Execute(ICommand command)
{
    command.Execute();
    _undo.Push(command);
    _redo.Clear();          // a new action invalidates the "future"
}

public void Undo()
{
    if (!_undo.TryPop(out var command)) return;
    command.Undo();
    _redo.Push(command);
}
```

**Client** — the demo performs a series of edits, then Undo/Redo. At the end it shows that after a new action Redo is no longer possible — this is how all editors work.

## Example 2. Smart home remote (`SmartHome/`)

**Receiver** — the devices in the `Devices/` folder: `Light`, `Thermostat`, `SecuritySystem`.

**Command** — `IHomeCommand` (`Description`, `Execute()`), without undo.

**ConcreteCommand** (folder `Commands/`):
- `ActionCommand` — a **delegate**-based command. In C# a simple command does not have to be a separate class per action — a lambda or a method reference is enough: `new ActionCommand("Arm security", security.Arm)`. In essence, an `Action` is a "command without undo".
- `MacroCommand` — a **macro command**: stores a list of commands and executes them in turn. Since it is also an `IHomeCommand`, other macros can be nested in it — the "Leave home" scenario contains the "Turn off all lights" macro (this is also a Composite).

**Invoker** — `RemoteControl`:
- the buttons (`Dictionary<int, IHomeCommand>`) are configured at runtime via `SetCommand`;
- commands can be **queued** (`Schedule`) and executed later (`RunScheduled`) — this is possible precisely because the action has become an object.

**Client** — the demo creates the devices, assembles the scenarios and assigns them to buttons. The remote never accesses the devices directly.

---

## When to use

- You need to parameterize objects with actions (buttons, menu items, hotkeys).
- You need undo/redo of operations.
- You need to queue operations, run them on a schedule or remotely (message queues, background jobs).
- You need to log operations so they can be "replayed" after a failure.
- You need transactions/scenarios made of several actions (macro commands).

## When not to use

- There is a single action, called from a single place, that needs no undo or queueing — a plain method call is clearer.

## Pros and cons

**Pros**
- The invoker is decoupled from the receiver.
- Undo, redo, deferred execution, queues, logging.
- Simple commands can be combined into complex ones (macros).
- New commands are added without changing existing code (Open/Closed).

**Cons**
- Many small classes (partly solved with delegates, as in `ActionCommand`).
- Undo can be complicated: the command must save enough data to restore the state.

## Relations with other patterns

- **Memento** — for complex undo, a command can save a snapshot of the receiver's state before execution.
- **Chain of Responsibility** — a request in a chain is often implemented as a command.
- **Composite** — `MacroCommand` is a composite of commands.
- **Strategy** also packs behavior into an object, but strategies are **different ways** of doing the same thing, while commands are **different actions** that can be stored, deferred and undone.
- **Prototype** — commands that need to be put into the history are sometimes copied.
- In .NET, `ICommand` in WPF/MAUI (MVVM) and `IRequest` requests in MediatR (CQRS) are commands.
