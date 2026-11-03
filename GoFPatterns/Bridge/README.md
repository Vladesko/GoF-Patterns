🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Bridge) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Bridge) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Bridge) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Bridge)

# Bridge

**Type:** structural pattern.

**Bridge** splits one large class (or hierarchy) into two independent hierarchies — the **abstraction** and the **implementation** — which can be developed separately from each other. The link between them is a reference (the "bridge") from the abstraction to the implementation.

---

## The problem

There are remotes (basic, advanced) and devices (TV, radio). If you solve the task with inheritance, you get a class for every combination:

```text
Remote
 ├── TvRemote
 ├── RadioRemote
 ├── AdvancedTvRemote
 └── AdvancedRadioRemote
```

Add a projector — two more classes. Add a "voice remote" — one more for each device. The number of classes grows as a **product** (N remotes × M devices), and code gets duplicated.

The reason is that the class tries to extend in **two independent directions** at once (the kind of remote and the kind of device).

## Solution

Replace inheritance with **composition**. One direction ("what we do" — remote/report) becomes the **abstraction** hierarchy, the other ("how we do it" — device/format) becomes the **implementation** hierarchy. The abstraction holds a reference to an implementation object and delegates work to it.

Now there are N + M classes, and any remote works with any device.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Abstraction** | `Remote`, `Report` | High-level control logic. Holds a reference to the Implementor and delegates work to it |
| **RefinedAbstraction** | `AdvancedRemote`, `SalesReport`, `InventoryReport` | Extends the abstraction without touching the implementations |
| **Implementor** | `IDevice`, `IReportRenderer` | The implementation interface — a set of primitive operations. It does not have to match the abstraction's interface |
| **ConcreteImplementor** | `Tv`, `Radio`, `PlainTextRenderer`, `HtmlRenderer` | Platform-specific implementations of the primitives |
| **Client** | demo classes | Links the abstraction with the required implementation |

## Diagram

```text
        Abstraction                         Implementor
 ┌──────────────────────┐   bridge   ┌────────────────────────┐
 │ Remote               │◆─────────► │ «interface» IDevice    │
 │ - Device: IDevice    │            │ Enable(), Disable()    │
 │ + TogglePower()      │            │ SetVolume(), SetChannel│
 │ + VolumeUp()         │            └────────────────────────┘
 └──────────────────────┘                  ▲          ▲
            ▲                              │          │
 ┌──────────────────────┐                 Tv        Radio
 │ AdvancedRemote       │
 │ + Mute(), Unmute()   │
 └──────────────────────┘
```

---

## Example 1. Remotes and devices (`RemoteControl/`)

**Implementor** — `IDevice` describes the **primitive** device operations: enable, disable, set volume and channel.

**ConcreteImplementor** — `Tv` and `Radio` (folder `Devices/`) implement them differently: the TV has volume 0–100, the radio has 0–20, and its channels are a list of stations cycled in a loop.

**Abstraction** — `Remote` (folder `Remotes/`) implements **high-level** actions through the device primitives:

```csharp
public class Remote(IDevice device)
{
    protected readonly IDevice Device = device;   // this is the "bridge"

    public void VolumeUp() => Device.SetVolume(Device.Volume + 5);
    public void ChannelUp() => Device.SetChannel(Device.Channel + 1);
}
```

**RefinedAbstraction** — `AdvancedRemote` adds `Mute`, `Unmute`, `SetFavoriteChannel`. Neither `Tv` nor `Radio` had to be changed for this: "mute" is simply `SetVolume(0)` while remembering the previous volume.

**Client** — the demo builds the combinations `Remote + Tv`, `AdvancedRemote + Radio`, `AdvancedRemote + Tv` — 4 possible combinations from 4 classes, rather than 4 separate combination classes plus a base.

## Example 2. Reports and formats (`Reports/`)

**Task:** there are report types (sales, inventory) and output formats (plain text, HTML). Tomorrow a customer report and a PDF format may be added.

**Implementor** — `IReportRenderer` can render primitives: a title, a "name — value" row, a footer.

**ConcreteImplementor** — `PlainTextRenderer` (column alignment) and `HtmlRenderer` (tags) in the `Renderers/` folder.

**Abstraction** — the abstract `Report` holds an `IReportRenderer` and declares `Build()`.

**RefinedAbstraction** — `SalesReport` and `InventoryReport` (folder `ReportTypes/`) know **what** to output: the sales report computes a total, the inventory report marks items that are running low. **How** it looks is decided by the renderer:

```csharp
var lines = new List<string> { Renderer.RenderTitle("Inventory") };
foreach (var (item, count) in stock)
{
    var value = count < LowStockLimit ? $"{count} (low!)" : count.ToString();
    lines.Add(Renderer.RenderRow(item, value));
}
```

**Client** — the demo iterates over all renderers and all reports; the implementation can be chosen at runtime (e.g. based on the request's `Accept` header).

---

## When to use

- A class needs to be extended in two (or more) independent directions.
- You need to switch the implementation at runtime.
- You want to split a "monolithic" class that has several variants of the same functionality (e.g. working with different databases/platforms).
- Cross-platform code: the abstraction is the GUI/logic, the implementation is the API of a specific OS.

## When not to use

- There is only one implementation and no other is expected — an unnecessary level of abstraction.

## Pros and cons

**Pros**
- The number of classes grows as a sum, not a product.
- The abstraction and the implementation evolve independently (Open/Closed).
- The client works only with the high-level abstraction without seeing platform details.
- The implementation can be swapped at runtime.

**Cons**
- Complicates the code if there is really only one hierarchy.

## Relations with other patterns

- **Adapter** is applied to already existing incompatible classes to make them work together. **Bridge** is designed up front so that the parts can evolve independently.
- **Strategy** has a similar structure (an object delegates work to another object through an interface), but solves a different problem: swapping an algorithm. In Bridge the abstraction itself is also a hierarchy.
- **Abstract Factory** can create matching "abstraction + implementation" pairs.
- **Builder** + **Bridge**: the director plays the role of the abstraction, the builders play the implementations.
