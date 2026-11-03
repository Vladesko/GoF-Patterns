🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Mediator) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Mediator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Mediator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Mediator)

# Mediator

**Type:** behavioral pattern.

**Mediator** reduces coupling between many objects by forcing them to communicate **not directly but through a mediator object**. The objects know only about the mediator, and all interaction logic is concentrated in it.

---

## The problem

**Registration form.** The "I am a company" checkbox shows the "Company name" field. The "Register" button is enabled only if the email is valid, the password is long enough, the terms are accepted and, for a company, the name is filled in. If the components communicate directly:

```text
 CheckBox(IsCompany) ──► TextBox(CompanyName)
        │                       │
        ▼                       ▼
     Button(Submit) ◄── TextBox(Email) ◄── TextBox(Password) ◄── CheckBox(Terms)
```

…each component knows about a bunch of others, and the connections form a "web". A `TextBox` cannot be reused in another form — it drags along knowledge about the button and the checkboxes. Any new rule requires changes in several classes.

**Chat.** If each user keeps references to all the others, everyone has to be updated whenever someone joins or leaves, and spam filtering and private messages get spread across the users.

## Solution

Move all interaction logic into a separate object — the **mediator**. The components:
- hold a reference **only** to the mediator;
- notify it about events ("I was changed", "send a message");
- do not know who will react to it and how.

The mediator knows all participants and decides whom to notify and how. "Many-to-many" relationships turn into "one-to-many".

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Mediator** | `IChatMediator`, `IFormMediator` | The interface through which components communicate with the mediator |
| **ConcreteMediator** | `ChatRoomMediator`, `RegistrationDialog` | Knows all participants and contains the logic of their interaction |
| **Colleague** | `ChatUser`, `FormComponent` | The base participant class; holds a reference to the mediator |
| **ConcreteColleague** | `ChatUser`, `ChatBot`, `TextBox`, `CheckBox`, `Button` | Concrete participants. Notify the mediator about events without knowing anything about each other |

## Diagram

```text
        TextBox(Email)   TextBox(Password)   CheckBox(Terms)
                 \              │              /
                  \             │             /
                   ▼            ▼            ▼
                 ┌──────────────────────────────┐
                 │ RegistrationDialog (Mediator)│
                 │ Notify(sender, event)        │
                 └──────────────────────────────┘
                   ▲            ▲            ▲
                  /             │             \
      CheckBox(IsCompany)  TextBox(CompanyName)  Button(Submit)
```

---

## Example 1. Chat room (`ChatRoom/`)

**Mediator** — `IChatMediator`: `Join`, `Leave`, `SendMessage`, `SendPrivate`.

**ConcreteMediator** — `ChatRoomMediator` stores a dictionary of participants and contains **all the rules** of communication:
- broadcasting a message to everyone except the sender;
- blocking messages with banned words;
- private messages by name, and a reply if the recipient is not in the room;
- join/leave notifications.

```csharp
public void SendMessage(ChatUser sender, string message)
{
    if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
        return;   // the moderation rule lives only here

    foreach (var user in _users.Values.Where(u => u != sender))
        user.Receive(sender.Name, message);
}
```

**Colleague** — `ChatUser` knows only its mediator:

```csharp
public void Send(string message) => chat.SendMessage(this, message);
```

**ConcreteColleague** — `ChatBot` is also a participant, but with its own reaction: it replies to the `/time` command with a private message. The users know nothing about the bot — it is just another participant of the room.

## Example 2. Registration form (`RegistrationForm/`) — the classic GoF example

**Colleague** — the abstract `FormComponent` (name, `IsEnabled`, `IsVisible`, a reference to the mediator).

**ConcreteColleague** (folder `Components/`) — `TextBox`, `CheckBox`, `Button`. On a user action, each of them only notifies the mediator about the event:

```csharp
public void Type(string text)
{
    Text = text;
    Mediator?.Notify(this, "changed");
}
```

These components are **universal** — they can be used in any form, and they know nothing about any other fields.

**ConcreteMediator** — `RegistrationDialog` creates the components, sets itself as their mediator, and implements the form rules in `Notify`:
- the `IsCompany` checkbox shows/hides the `CompanyName` field;
- after any change the availability of the `Submit` button is recalculated;
- clicking `Submit` performs the registration.

```csharp
var isValid = Email.Text.Contains('@')
              && Password.Text.Length >= 6
              && AcceptTerms.IsChecked
              && (!IsCompany.IsChecked || CompanyName.Text.Length > 0);
```

**Client** — the demo simulates user actions. You can see how the button becomes enabled, then disabled again ("company" checked but no name entered), and then enabled again.

---

## When to use

- Many objects are connected in a complex way, and the connections are hard to understand and change.
- You want to reuse components, but they depend too much on others.
- The interaction logic should live in one place and be configurable without changing the components.
- Examples: UI dialogs and forms, chats, air traffic control (planes communicate through the tower), microservice coordination (an orchestrator).

## When not to use

- There are few objects and the connections are simple — the mediator would be an unnecessary layer.

## Pros and cons

**Pros**
- Eliminates direct dependencies between components.
- The interaction logic is in one place (SRP).
- Components are easy to reuse.
- New rules are added to the mediator without changing the components.

**Cons**
- The mediator can turn into a **"god object"** that knows and does everything.

## Relations with other patterns

- **Facade** also simplifies working with a group of objects, but the subsystem **does not know** about the facade, and the communication is one-way. Mediator participants **know** about the mediator and communicate through it.
- **Observer** — a mediator is often implemented through events/subscriptions: components publish events, and the mediator subscribes to them. The difference is in the goal: Observer is a dynamic "one-to-many" subscription, Mediator is centralized interaction logic.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer** — different ways to connect senders and receivers of requests.
- The **MediatR** library in .NET is named after the pattern: handlers do not know about each other, and requests are sent through `IMediator`.
