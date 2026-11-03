🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Strategy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Strategy) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Strategy) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Strategy)

# Strategy (Strategia)

**Tipo:** pattern comportamentale.

**Strategy** definisce una **famiglia di algoritmi intercambiabili**, mette ciascuno in una classe separata e permette di sostituirli durante l'esecuzione del programma. Il client lavora con l'algoritmo tramite un'interfaccia comune.

---

## Quale problema risolve

Un negozio online calcola il costo di consegna: per posta, con corriere, presso un punto di ritiro. Se tutto è in un'unica classe:

```csharp
public decimal Calculate(Parcel parcel, string type)
{
    if (type == "post") return 3 + parcel.Weight * 1.2m;
    else if (type == "courier") return 10 + parcel.Distance * 0.05m + ...;
    else if (type == "pickup") return parcel.Value >= 100 ? 0 : 2.5m;
    ...
}
```

- ogni nuovo metodo di consegna significa modificare questa classe con il rischio di rompere gli altri;
- la classe cresce e gli algoritmi sono mescolati;
- è difficile testare un singolo algoritmo separatamente;
- gli stessi `if` compaiono nel calcolo dei tempi, nella visualizzazione e così via.

## Soluzione

Spostare ogni algoritmo in una classe separata, una **strategy**, con un'interfaccia comune. La classe **contesto** mantiene un riferimento alla strategy e le delega il lavoro, senza sapere quale strategy le sia stata passata. Il client sceglie la strategy desiderata e la passa al contesto; può essere sostituita in qualsiasi momento.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Strategy** | `IDeliveryStrategy`, `ISortStrategy` | Interfaccia comune di tutte le varianti dell'algoritmo |
| **ConcreteStrategy** | `PostDeliveryStrategy`, `CourierDeliveryStrategy`, `PickupPointStrategy`, `BubbleSortStrategy`, `QuickSortStrategy`, `BuiltInSortStrategy` | Implementazioni concrete dell'algoritmo |
| **Context** | `DeliveryCalculator`, `DataSorter` | Conserva la strategy (o la riceve come parametro) e la chiama tramite l'interfaccia |
| **Client** | classi demo | Sceglie la strategy e la passa al contesto |

## Schema

```text
 Client ── sceglie ──► PostDeliveryStrategy
   │                          │
   ▼                          ▼  implementa
 DeliveryCalculator ◆────► «interface» IDeliveryStrategy
 (Context)                    CalculateCost(parcel)
   SetStrategy(s)             EstimateDays(parcel)
   GetQuote(parcel)                 ▲        ▲        ▲
                                    │        │        │
                                  Post   Courier   PickupPoint
```

---

## Esempio 1. Costo di consegna (`Delivery/`)

**Strategy**: `IDeliveryStrategy` con i metodi `CalculateCost` ed `EstimateDays`. I dati di input sono un `record Parcel` (peso, distanza, valore dichiarato).

**ConcreteStrategy** (cartella `Strategies/`): ognuna contiene solo la propria formula:
- `PostDeliveryStrategy`: economica e lenta, il prezzo dipende dal peso;
- `CourierDeliveryStrategy`: più cara, dipende dalla distanza, 1–2 giorni;
- `PickupPointStrategy`: gratuita per gli ordini da 100 in su:

```csharp
public decimal CalculateCost(Parcel parcel) =>
    parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;
```

**Context**: `DeliveryCalculator` riceve la strategy nel costruttore, può cambiarla con `SetStrategy` e prepara il preventivo senza sapere quale strategy ci sia dentro:

```csharp
public string GetQuote(Parcel parcel) =>
    $"{_strategy.Name} costo: {_strategy.CalculateCost(parcel)}, giorni: {_strategy.EstimateDays(parcel)}";
```

**Client**: la demo fa passare lo stesso pacco attraverso tutte le strategy, e poi **sceglie automaticamente** la più economica e la più veloce: le strategy si possono confrontare perché hanno un'interfaccia comune.

Per aggiungere, ad esempio, la consegna con drone basta scrivere una nuova classe: non bisogna modificare né il contesto né le altre strategy.

## Esempio 2. Algoritmi di ordinamento (`Sorting/`)

**Strategy**: `ISortStrategy.Sort(int[] items)`.

**ConcreteStrategy** (cartella `Strategies/`):
- `BubbleSortStrategy`: semplice, O(n²);
- `QuickSortStrategy`: quick sort, O(n log n) in media;
- `BuiltInSortStrategy`: wrapper attorno a `Array.Sort` (l'introsort di .NET).

**Context**: `DataSorter`:
- `Sort(items, strategy)`: la strategy viene passata **come parametro del metodo**, non conservata in un campo. Anche questo è Strategy: conta la possibilità di sostituire l'algoritmo, non dove si trova il riferimento;
- `ChooseFor(items)`: il contesto può **scegliere da solo** la strategy in base ai dati (per un array minuscolo il bubble sort, altrimenti quella integrata).

**Client**: la demo ordina gli stessi 5000 numeri con tutte le strategy e ne confronta i tempi: si vede chiaramente perché conviene rendere gli algoritmi intercambiabili.

Alla fine si mostra che in C# una strategy semplice può essere semplicemente un **delegate**: `Array.Sort(names, byLength)`, dove `byLength` è un `Comparison<string>`. Allo stesso modo funzionano `IComparer<T>`, `IEqualityComparer<T>` e le lambda in LINQ (`OrderBy(x => ...)`).

---

## Quando usarlo

- Ci sono diverse varianti dello stesso algoritmo e bisogna passare dall'una all'altra.
- Ci sono molte classi simili che differiscono solo per il comportamento.
- Bisogna eliminare un grande costrutto condizionale che sceglie la variante dell'algoritmo.
- Bisogna isolare i dettagli dell'algoritmo dal codice che lo usa.
- Esempi: metodi di pagamento/consegna, calcolo di sconti e tasse, compressione, ordinamento, routing, convalida.

## Quando non serve

- Gli algoritmi sono due e non cambiano quasi mai: un `if` è più semplice.
- Il client non ha bisogno di conoscere le varianti: allora forse è meglio Template Method o il semplice polimorfismo.

## Pro e contro

**Pro**
- Gli algoritmi si sostituiscono a runtime.
- Il codice degli algoritmi è isolato dal resto del codice (SRP).
- L'ereditarietà è sostituita dalla composizione.
- Nuove strategy si aggiungono senza modificare il contesto (Open/Closed).
- Ogni strategy è facile da testare separatamente.

**Contro**
- Il client deve conoscere le differenze tra le strategy per scegliere quella giusta.
- Classi in più se gli algoritmi sono pochi (in C# il problema si attenua con i delegate).

## Relazioni con altri pattern

- **State**: struttura quasi uguale, ma la strategy la **sceglie il client** e le strategy sono indipendenti; gli stati **si scambiano da soli** tra loro.
- **Template Method** cambia parti dell'algoritmo tramite l'**ereditarietà** (staticamente, a livello di classi); **Strategy** cambia l'intero algoritmo tramite la **composizione** (dinamicamente, a livello di oggetti).
- **Decorator** cambia l'«involucro» di un oggetto, **Strategy** le sue «viscere».
- Anche **Bridge** si basa sulla delega tramite un'interfaccia, ma lì anche l'astrazione forma una gerarchia.
- Anche **Command** racchiude un comportamento in un oggetto, ma il comando è un'**azione** (che si può rimandare, annullare), mentre la strategy è un **modo** di svolgere lo stesso compito.
- L'**Abstract Factory** di questo progetto (`IPaymentProviderFactory`) usa un'idea simile: `PaymentService` lavora con qualsiasi provider tramite un'interfaccia.
