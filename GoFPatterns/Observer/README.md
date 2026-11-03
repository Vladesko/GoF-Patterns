🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Observer) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Observer) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Observer) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Observer)

# Observer (Osservatore)

**Tipo:** pattern comportamentale.

**Observer** crea un meccanismo di **sottoscrizione**: alcuni oggetti (gli observer) si iscrivono alle modifiche di un altro oggetto (il publisher) e ricevono automaticamente una notifica quando in esso accade qualcosa. È una relazione «uno a molti».

---

## Quale problema risolve

Il prezzo di un prodotto cambia. Diverse parti del sistema devono reagire:
- l'**acquirente** aspetta che il prezzo scenda abbastanza per comprare;
- il **servizio sconti** annuncia una promozione quando il prezzo è sotto una soglia;
- il **raccoglitore di statistiche** conta tutte le variazioni di prezzo.

Varianti senza il pattern:
- **Polling**: ogni oggetto interessato chiede periodicamente il prezzo. Carico inutile e ritardo nella reazione.
- **Il prodotto chiama tutti da solo**: `buyer.Check(price); discounts.Check(price); stats.Add(price);`: il prodotto conosce ogni classe concreta. Un nuovo interessato significa modificare il prodotto. Non si può rimuovere qualcuno durante l'esecuzione.

## Soluzione

Il publisher (il prodotto) conserva una **lista di sottoscrittori** e offre i metodi `Subscribe`/`Unsubscribe`. Tutti i sottoscrittori implementano un'interfaccia comune con il metodo `Update`. Quando lo stato cambia, il publisher scorre la lista e chiama `Update` su ciascuno.

Del sottoscrittore il publisher sa solo che implementa l'interfaccia. I sottoscrittori si possono aggiungere e rimuovere a runtime; un sottoscrittore può anche annullare da solo la propria iscrizione.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Subject (Observable)** | `IObservable` | Interfaccia del publisher: sottoscrizione e annullamento |
| **ConcreteSubject** | `Product` | Conserva lo stato (il prezzo) e la lista dei sottoscrittori, li avvisa delle modifiche |
| **Observer** | `IObserver` | Interfaccia del sottoscrittore con il metodo `Update(decimal price)` |
| **ConcreteObserver** | `Buyer`, `DiscountNotifier`, `StatisticsCollector` | Reagiscono alle notifiche ciascuno a modo suo |
| **Client** | `ProductObserverDemonstrate` | Crea il publisher e i sottoscrittori, modifica lo stato |

## Schema

```text
 ┌───────────────────────────┐        ┌──────────────────────────┐
 │ Product (ConcreteSubject) │        │ «interface» IObserver    │
 │ - _observers: HashSet ────┼──────► │ Update(price)            │
 │ + Subscribe(o)            │  0..*  └──────────────────────────┘
 │ + Unsubscribe(o)          │            ▲         ▲          ▲
 │ + ChangePrice(p):         │            │         │          │
 │     NotifyObservers()     │         Buyer  DiscountNotifier  StatisticsCollector
 └───────────────────────────┘     (≤ 50: compra  (≤ 100:       (conta ogni
              ▲                     e si disiscrive) sconto)      variazione)
              │ implementa
   «interface» IObservable
```

---

## Esempio. Variazione del prezzo di un prodotto (`ProductObserver/`)

**Subject**: `IObservable` con i metodi `Subscribe(IObserver)` e `Unsubscribe(IObserver)`.

**Observer**: `IObserver` con il metodo `Update(decimal price)`. Il publisher passa il nuovo valore direttamente nella notifica (il cosiddetto **modello push**).

**ConcreteSubject**: `Product`:

```csharp
public class Product(decimal initialPrice) : IObservable
{
    private readonly HashSet<IObserver> _observers = [];
    private decimal _price = initialPrice;

    public void ChangePrice(decimal newPrice)
    {
        if (_price == newPrice) return;      // non notifichiamo se non è cambiato nulla
        _price = newPrice;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers.ToArray())
            observer.Update(_price);
    }
}
```

Dettagli importanti dell'implementazione:
- `HashSet`: lo stesso sottoscrittore non verrà iscritto due volte;
- `ArgumentNullException.ThrowIfNull` in `Subscribe`/`Unsubscribe`: protezione da `null`;
- `NotifyObservers` è `private`: solo il prodotto stesso può notificare i sottoscrittori;
- l'iterazione avviene su una **copia** della lista (`_observers.ToArray()`), perché un sottoscrittore può **disiscriversi proprio durante la notifica** (lo fa `Buyer`). Non si può modificare un `HashSet` mentre lo si sta scorrendo: provocherebbe una `InvalidOperationException`.

**ConcreteObserver** (cartella `Observers/`): ognuno si iscrive nel costruttore (`_observable.Subscribe(this)`) e reagisce al prezzo a modo suo:
- `StatisticsCollector`: conta ogni variazione di prezzo e stampa il contatore;
- `DiscountNotifier`: con un prezzo ≤ 100 annuncia uno sconto;
- `Buyer`: con un prezzo ≤ 50 compra il prodotto e **si disiscrive da solo**, perché non è più interessato:

```csharp
public void Update(decimal price)
{
    if (price > 50) return;
    Console.WriteLine("Acquisto del prodotto!");
    _observable.Unsubscribe(this);
}
```

**Client**: `ProductObserverDemonstrate` crea un prodotto con prezzo 150 e tre sottoscrittori, poi cambia il prezzo: 120 → 90 → 40. A ogni variazione si attivano le statistiche; a 90 compare lo sconto; a 40 lo sconto e l'acquisto.

---

## Push e pull

- **Push** (come qui): il publisher passa i dati in `Update(price)`. Semplice, ma l'interfaccia è legata a dati specifici.
- **Pull**: il publisher passa se stesso (`Update(IObservable sender)`) e il sottoscrittore legge da solo le proprietà che gli servono. Più flessibile, ma il sottoscrittore dipende dal publisher.

## Observer in .NET

- **Eventi C#** (`event EventHandler<T>`): implementazione integrata del pattern: `+=` è la sottoscrizione, `-=` l'annullamento, l'invocazione dell'evento è la notifica.
- Le interfacce `IObservable<T>` / `IObserver<T>` della BCL e la libreria **Rx.NET**: flussi reattivi.
- `INotifyPropertyChanged` in WPF/MAUI: il binding della UI al modello.

## Quando usarlo

- La modifica di un oggetto richiede la reazione di altri, e non si sa in anticipo di quali né di quanti.
- I sottoscrittori devono essere aggiunti e rimossi durante l'esecuzione.
- Bisogna allentare il legame tra la sorgente degli eventi e le reazioni a essi.

## Quando non serve

- Il sottoscrittore è esattamente uno ed è noto in anticipo: una chiamata diretta è più semplice.
- Sono importanti un ordine rigoroso e la transazionalità delle reazioni: negli observer l'ordine delle notifiche di solito non è garantito.

## Pro e contro

**Pro**
- Il publisher non dipende dalle classi concrete dei sottoscrittori (Open/Closed).
- Sottoscrizione e annullamento a runtime.
- Un solo evento, quante reazioni si vuole.

**Contro**
- L'ordine di notifica dei sottoscrittori non è garantito.
- **Memory leak**: se un sottoscrittore si dimentica di disiscriversi, il publisher mantiene un riferimento a esso e il garbage collector non lo elimina (un problema classico con gli eventi in .NET).
- Una cascata di notifiche può essere difficile da debuggare.

## Relazioni con altri pattern

- **Mediator**: è spesso implementato tramite Observer: i componenti pubblicano eventi e il mediatore vi è iscritto. Gli scopi sono diversi: Observer è una distribuzione dinamica, Mediator una logica di interazione centralizzata.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer**: modi diversi di collegare mittenti e destinatari delle richieste.
- **Singleton**: a volte il publisher che gestisce gli eventi è un singleton (event bus).
