🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/FactoryMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/FactoryMethod) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/FactoryMethod) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/FactoryMethod)

# Factory Method (Metodo fabbrica)

**Tipo:** pattern creazionale.

**Factory Method** definisce nella classe base un metodo per creare un oggetto, ma lascia che siano **le sottoclassi a decidere quale classe istanziare**. Il codice che usa l'oggetto dipende solo dalla sua interfaccia, non dalla classe concreta.

---

## Quale problema risolve

All'inizio il servizio di notifiche sapeva inviare solo SMS, e il codice era così:

```csharp
var sms = new SmsNotification();
sms.Notify(message);
```

Poi sono serviti e-mail e push. Se si continua allo stesso modo, nel codice compaiono `switch`/`if` con un `new` per ogni tipo, in tutti i punti in cui si inviano notifiche. Il codice è rigidamente legato alle classi concrete, e aggiungere un nuovo canale richiede modifiche in tutto il progetto.

Lo stesso vale per gli abbonamenti di una palestra: «sala pesi», «sala pesi + piscina», «allenamenti personalizzati». Se il codice client chiama da solo `new GymMembership()`, dipende da tutti i tipi di abbonamento.

## Soluzione

Sostituire la chiamata diretta a `new` con la chiamata a un apposito **factory method**. Il metodo è dichiarato nella classe base (il creator) e restituisce l'**interfaccia del prodotto**, mentre le sottoclassi creator lo ridefiniscono e restituiscono prodotti concreti.

Importante: di solito il creator ha anche **una propria logica**, che lavora con il prodotto tramite l'interfaccia e non sa quale prodotto sia stato creato (nell'esempio delle notifiche, il metodo `Notify`).

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Product** | `INotificationService`, `IMembership` | Interfaccia comune degli oggetti creati |
| **ConcreteProduct** | `SmsNotification`, `UmailNotification`, `PushNotification`, `GymMembership`, `GymPoolMembership`, `PersonalTrainingMembership` | Implementazioni concrete del prodotto |
| **Creator** | `NotificationFactory`, `MembershipFactory` | Dichiara il factory method; può contenere logica che lavora con il prodotto |
| **ConcreteCreator** | `SmsNotificator`, `UmailNotificator`, `PushNotificator`, `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` | Ridefinisce il factory method e restituisce un prodotto concreto |
| **Client** | `DemondtrsteFactoryMethod` | Lavora con i creator e i prodotti |

## Schema

```text
 ┌───────────────────────────────────────────┐          ┌─────────────────────────────┐
 │ «abstract» NotificationFactory (Creator)  │          │ «interface»                 │
 │ # CreateNotification(): INotificationService ──────►│ INotificationService        │
 │ + Notify(message):                        │ crea     │ Notify(message)             │
 │     var s = CreateNotification();         │          └─────────────────────────────┘
 │     s.Notify(message);                    │                ▲        ▲        ▲
 └───────────────────────────────────────────┘                │        │        │
        ▲                ▲                ▲          SmsNotification  Umail   Push
        │                │                │                          Notification Notification
  SmsNotificator   UmailNotificator  PushNotificator
  Create… => new SmsNotification()  ...
```

---

## Esempio 1. Sistema di notifiche (`NotifySystem/`): un creator con logica propria

**Product**: `INotificationService` con il metodo `Notify(string message)`.

**ConcreteProduct** (cartella `NotifiationServices/`): `SmsNotification`, `UmailNotification`, `PushNotification`: ognuno «invia» il messaggio attraverso il proprio canale.

**Creator**: la classe astratta `NotificationFactory` (cartella `Factories/`):

```csharp
public abstract class NotificationFactory
{
    protected abstract INotificationService CreateNotification();   // factory method

    public void Notify(string message)                               // logica del creator
    {
        var notificationService = CreateNotification();
        notificationService.Notify(message);
    }
}
```

È la forma classica del pattern: il metodo `Notify` è implementato **una sola volta** nella classe base e lavora con il prodotto solo tramite l'interfaccia. Il factory method è `protected`: il client non crea da solo il prodotto, ma chiama semplicemente `Notify`.

**ConcreteCreator**: `SmsNotificator`, `UmailNotificator`, `PushNotificator` ridefiniscono solo il factory method:

```csharp
internal class SmsNotificator : NotificationFactory
{
    protected override INotificationService CreateNotification() => new SmsNotification();
}
```

**Client**: `DemondtrsteFactoryMethod.DemonstrateNotify()` crea il creator desiderato e chiama `Notify`. Per aggiungere, ad esempio, Telegram servono due classi, `TelegramNotification` e `TelegramNotificator`; il codice esistente non cambia.

## Esempio 2. Abbonamenti della palestra (`Fitnes/`): factory di prodotti

**Product**: `IMembership` (nome, descrizione, prezzo).

**ConcreteProduct** (cartella `Memberships/`):
- `GymMembership`: sala pesi, 50;
- `GymPoolMembership`: sala pesi + piscina, 75;
- `PersonalTrainingMembership`: allenamenti personalizzati, 100.

**Creator**: la classe astratta `MembershipFactory` con un factory method pubblico:

```csharp
internal abstract class MembershipFactory
{
    public abstract IMembership CreateMembership();
}
```

**ConcreteCreator** (cartella `Factories/`): `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory`; ognuna restituisce il proprio abbonamento.

**Client**: `DemondtrsteFactoryMethod.DemonstrateFitnes()` sceglie la factory desiderata e ottiene l'abbonamento tramite `CreateMembership()`, poi ci lavora come con un `IMembership`.

Qui il creator non ha una logica propria: è una forma semplificata del pattern, molto usata in pratica. La factory (scelta ad esempio in base alle impostazioni o alla scelta dell'utente) viene passata al codice che crea gli oggetti senza conoscerne le classi.

---

## Quando usarlo

- Non si sa in anticipo di quali classi bisognerà creare gli oggetti.
- Bisogna permettere di estendere gli oggetti creati tramite l'ereditarietà (framework, librerie).
- La classe base contiene un algoritmo comune, e il prodotto concreto per esso viene definito dalle sottoclassi.
- Bisogna centralizzare la creazione degli oggetti per poterli riutilizzare se necessario (cache, pool).

## Quando non serve

- Il prodotto è uno solo e non cambierà: un semplice `new` è più chiaro.
- Bisogna creare **famiglie** di oggetti coerenti: questo è il compito di Abstract Factory.

## Pro e contro

**Pro**
- Libera il codice dal legame con le classi concrete dei prodotti.
- Il codice di creazione dei prodotti è in un unico punto (SRP).
- Nuovi prodotti si aggiungono senza modificare il codice esistente (Open/Closed).

**Contro**
- Per ogni prodotto serve una sottoclasse creator: le gerarchie crescono in parallelo.

## Relazioni con altri pattern

- **Abstract Factory** è spesso implementata con un insieme di factory method, ma crea una **famiglia** di prodotti e viene passata come oggetto.
- **Template Method**: il factory method è spesso un passo di un template method: `NotificationFactory.Notify()` è un template method che chiama il factory method `CreateNotification()`.
- **Prototype** non richiede sottoclassi creator, ma richiede l'inizializzazione dei prototipi. Factory Method è l'opposto.
- **Iterator**: `GetEnumerator()` nelle collezioni .NET è un factory method che restituisce un iterator.
- Factory Method spesso evolve in **Builder** o **Abstract Factory** quando la creazione diventa più complessa.
