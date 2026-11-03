🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/State) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/State) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/State) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/State)

# State (Stato)

**Tipo:** pattern comportamentale.

**State** permette a un oggetto di **cambiare comportamento quando cambia il suo stato interno**. Dall'esterno sembra che l'oggetto abbia cambiato classe.

---

## Quale problema risolve

Un ordine in un negozio online attraversa diverse fasi: *Nuovo → Pagato → Spedito → Consegnato* (oppure *Annullato*). La stessa azione si comporta in modo diverso a seconda della fase: annullare un ordine nuovo significa semplicemente annullarlo; uno pagato richiede il rimborso; uno spedito non si può annullare.

L'implementazione tipica è un campo `Status` con uno `switch` in ogni metodo:

```csharp
public void Cancel()
{
    switch (Status)
    {
        case Status.New: Status = Status.Cancelled; break;
        case Status.Paid: Refund(); Status = Status.Cancelled; break;
        case Status.Shipped: throw ...;
        case Status.Delivered: throw ...;
        ...
    }
}
```

Questi `switch` si trovano in `Pay`, `Ship`, `Deliver`, `Cancel`... Se si aggiunge un nuovo stato («Reso») bisogna ripassare tutti i metodi. Le regole di transizione sono sparse nella classe ed è facile sbagliare.

## Soluzione

Spostare ogni stato in una **classe separata** con un'interfaccia comune che contiene tutte le azioni. L'oggetto originale (il **contesto**) mantiene un riferimento all'oggetto stato corrente e gli **delega** tutte le azioni. Il passaggio a un altro stato è semplicemente la sostituzione dell'oggetto stato.

Invece di un grande `switch` su tutti gli stati si ottengono diverse piccole classi, ognuna delle quali descrive il **comportamento in un solo stato** e **verso quali stati si può passare**.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Context** | `Order`, `VendingMachine` | Mantiene un riferimento allo stato corrente e gli delega le azioni. Offre un metodo per cambiare stato |
| **State** | `IOrderState`, `IVendingState` | Interfaccia comune di tutti gli stati: tutte le azioni disponibili per il contesto |
| **ConcreteState** | `NewState`, `PaidState`, `ShippedState`, `DeliveredState`, `CancelledState`, `IdleState`, `HasMoneyState`, `SoldOutState` | Implementa il comportamento in uno stato specifico e decide quando far cambiare stato al contesto |

## Schema

```text
 ┌───────────────────────┐        ┌────────────────────────┐
 │ Order (Context)       │◆──────►│ «interface» IOrderState│
 │ - State               │        │ Pay(), Ship(),         │
 │ + Pay() => State.Pay()│        │ Deliver(), Cancel()    │
 │ + TransitionTo(state) │        └────────────────────────┘
 └───────────────────────┘                    ▲
                                ┌─────────────┼──────────────┬────────────┐
                             NewState     PaidState     ShippedState   ...

 Transizioni:  New ──Pay──► Paid ──Ship──► Shipped ──Deliver──► Delivered
                │            │
                └──Cancel──► Cancelled ◄──Cancel (rimborso)
```

---

## Esempio 1. Ciclo di vita dell'ordine (`OrderLifecycle/`)

**Context**: `Order` non contiene nemmeno uno `switch`. Tutte le azioni pubbliche vengono semplicemente delegate allo stato corrente:

```csharp
public void Pay() => State.Pay(this);
public void Cancel() => State.Cancel(this);

internal void TransitionTo(IOrderState state) => State = state;
```

`TransitionTo` è marcato `internal`: lo stato può essere cambiato solo dalle classi stato, non dal codice esterno.

**State**: `IOrderState` elenca tutte le azioni sull'ordine.

**Stato base**: `OrderStateBase` (cartella `States/`) di default **vieta** tutte le azioni. Così gli stati concreti ridefiniscono solo ciò che in essi è consentito, e non serve scrivere «non si può» per le altre azioni in ogni classe.

**ConcreteState**:
- `NewState`: sono consentiti il pagamento (→ `Paid`) e l'annullamento (→ `Cancelled`);
- `PaidState`: la spedizione (→ `Shipped`) e l'annullamento **con rimborso**:

```csharp
public override void Cancel(Order order)
{
    Console.WriteLine($"Rimborso di {order.Amount:F2} al cliente");
    order.TransitionTo(new CancelledState());
}
```

- `ShippedState`: solo la consegna (→ `Delivered`);
- `DeliveredState`, `CancelledState`: stati finali, non ridefiniscono nulla.

Si vede che l'azione «annulla» si comporta in modo diverso in due stati ed è vietata negli altri, senza un solo `if` sullo stato.

**Client**: la demo prova a eseguire le azioni nell'ordine sbagliato (spedire un ordine non pagato, annullarne uno spedito) e gli stati rifiutano correttamente.

## Esempio 2. Distributore automatico (`Vending/`)

**Context**: `VendingMachine` conserva i **dati** (scorte, prezzo, credito) e lo stato corrente.

**ConcreteState** (cartella `States/`):
- `IdleState`: aspetta il denaro; moneta → `HasMoney`;
- `HasMoneyState`: accumula il denaro, eroga il prodotto e il resto; se il prodotto è esaurito → `SoldOut`, altrimenti → `Idle`;
- `SoldOutState`: restituisce subito qualsiasi moneta.

Qui si vede una tecnica del libro della GoF: gli stati **non hanno campi propri**, tutti i dati sono conservati nel contesto e passati come parametro. Per questo ogni stato può essere creato una sola volta e **condiviso** (sono flyweight, vedi Flyweight):

```csharp
internal static readonly IVendingState Idle = new IdleState();
internal static readonly IVendingState HasMoney = new HasMoneyState();
internal static readonly IVendingState SoldOut = new SoldOutState();
```

**Client**: la demo simula dei clienti: tentativo di acquisto senza denaro, denaro insufficiente, acquisto con resto, restituzione, acquisto dell'ultimo prodotto e tentativo di inserire una moneta nel distributore vuoto. Dopo ogni azione viene stampato lo stato corrente.

---

## Quando usarlo

- Il comportamento dell'oggetto dipende fortemente dal suo stato, e gli stati sono molti.
- Il codice è pieno di `switch`/`if` sul campo di stato, ripetuti in metodi diversi.
- Le regole di transizione tra gli stati sono complesse e cambiano spesso.
- Esempi: ordini, gestione documentale (bozza → in approvazione → approvato), connessioni TCP, lettore multimediale, personaggi nei giochi, macchine a stati finiti.

## Quando non serve

- Gli stati sono pochi e cambiano raramente: uno `switch` su un `enum` sarà più semplice e chiaro.

## Pro e contro

**Pro**
- Elimina i grandi costrutti condizionali.
- Il codice di ogni stato è nella propria classe (SRP).
- Nuovi stati si aggiungono senza modificare quelli esistenti (Open/Closed).
- Le transizioni sono esplicite: si vede da quale stato si può passare a quale.

**Contro**
- Molte classi anche con pochi stati.
- La logica delle transizioni è distribuita tra le classi stato: è più difficile vedere l'intera macchina a stati (aiuta un diagramma nella documentazione).

## Relazioni con altri pattern

- **Strategy**: la struttura è quasi identica (il contesto delega a un oggetto con un'interfaccia). La differenza è nell'intento: la strategia **la sceglie il client**, e le strategie non si conoscono tra loro; gli stati **si scambiano da soli** e conoscono le transizioni.
- **Flyweight**: gli stati senza campi si possono condividere tra i contesti (esempio `Vending`).
- **Singleton**: gli oggetti stato a volte sono singleton.
- Anche **Bridge** e **Strategy** si basano su composizione e delega, ma risolvono problemi diversi.
