🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/AbtractFactory) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/AbtractFactory) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/AbtractFactory) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/AbtractFactory)

# Abstract Factory (Fabbrica astratta)

**Tipo:** pattern creazionale.

**Abstract Factory** permette di creare **famiglie di oggetti correlati** senza legarsi alle classi concrete di questi oggetti. Il client lavora con la factory e con i prodotti solo tramite interfacce.

---

## Quale problema risolve

Un servizio di pagamento lavora con provider di pagamento (nell'esempio, gli inventati **PoolPay** e **Strope**). Per ogni provider serve un **insieme** di oggetti:
- un processore dei pagamenti;
- un processore dei rimborsi;
- un validatore dei pagamenti.

Questi oggetti formano una **famiglia**: il processore di PoolPay deve lavorare insieme al validatore e ai rimborsi di PoolPay. Mescolare il processore di PoolPay con i rimborsi di Strope è un errore (la transazione di un provider non esiste presso l'altro).

Se il servizio di pagamento crea gli oggetti da solo:

```csharp
if (provider == "PoolPay") { processor = new PoolPayPaymentProcess(); refund = new PoolPayRefundProcessor(); ... }
else if (provider == "Strope") { processor = new StropePaymentProcessor(); refund = new StropeRefundProcessor(); ... }
```

- il servizio dipende da **tutte** le classi concrete di tutti i provider;
- aggiungere un provider significa modificare il servizio;
- è facile sbagliare e creare un «insieme incompatibile».

## Soluzione

1. Per ogni tipo di prodotto definire un'**interfaccia** (`IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator`).
2. Dichiarare una **fabbrica astratta**: un'interfaccia con i metodi di creazione di ogni prodotto della famiglia.
3. Per ogni famiglia (provider) creare una **factory concreta**, che crea solo i prodotti della propria famiglia.
4. Il client riceve la factory dall'esterno e crea i prodotti solo tramite essa: così non conosce le classi concrete e **non può mescolare le famiglie**.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **AbstractFactory** | `IPaymentProviderFactory` | Dichiara i metodi di creazione di tutti i prodotti della famiglia |
| **ConcreteFactory** | `PoolPayPaymentFactory`, `StropePaymentFactory` | Crea i prodotti di una sola famiglia (di un solo provider) |
| **AbstractProduct** | `IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator` | Interfacce dei prodotti |
| **ConcreteProduct** | `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`, `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator` | Implementazioni concrete dei prodotti per ogni provider |
| **Client** | `PaymentService` (+ `IPaymentService`) | Usa solo le interfacce della factory e dei prodotti |

## Schema

```text
                       «interface» IPaymentProviderFactory
                         CreatePaymentProcess(): IPaymentProcessor
                         CreateRefundProcess():  IRefundProcessor
                         CreatePaymentValidator(): IPaymentValidator
                              ▲                         ▲
                              │                         │
               PoolPayPaymentFactory            StropePaymentFactory
                 │      │       │                 │      │       │
                 ▼      ▼       ▼                 ▼      ▼       ▼
         PoolPayPayment PoolPay PoolPay      StropePayment Strope Strope
           Process      Refund  Validator      Processor   Refund Validator

 PaymentService(IPaymentProviderFactory factory)   ← Client
   Pay(amount):  validator.ValidatePayment → processor.ProcessPayment
   Refund(id):   refundProcessor.ProcessRefund
```

---

## Esempio. Provider di pagamento (`Payments/`)

**AbstractProduct** (cartella `Interfaces/Abstractions/`):

```csharp
public interface IPaymentProcessor { Guid ProcessPayment(decimal amount); }
public interface IRefundProcessor  { void ProcessRefund(Guid transactionId); }
public interface IPaymentValidator { bool ValidatePayment(decimal amount); }
```

**AbstractFactory** (cartella `Interfaces/AbstractFactories/`):

```csharp
public interface IPaymentProviderFactory
{
    IPaymentProcessor CreatePaymentProcess();
    IRefundProcessor CreateRefundProcess();
    IPaymentValidator CreatePaymentValidator();
}
```

**ConcreteFactory + ConcreteProduct**: ogni provider ha la propria cartella:
- `PoolPays/`: `PoolPayPaymentFactory` crea `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`;
- `Stopes/`: `StropePaymentFactory` crea `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator`.

Tutte le classi concrete sono marcate `internal`: al di fuori dell'assembly nessuno le conosce, sono accessibili solo le interfacce.

```csharp
internal class PoolPayPaymentFactory : IPaymentProviderFactory
{
    public IPaymentProcessor CreatePaymentProcess() => new PoolPayPaymentProcess();
    public IPaymentValidator CreatePaymentValidator() => new PoolPayPaymentValidator();
    public IRefundProcessor CreateRefundProcess() => new PoolPayRefundProcessor();
}
```

**Client**: `PaymentService` (cartella `Services/`) riceve la factory nel costruttore e crea tramite essa **una sola volta** l'intero insieme di prodotti. Da lì in poi lavora solo con le interfacce:

```csharp
internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
    private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
    private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

    public Guid Pay(decimal amount)
    {
        if (!_paymentValidator.ValidatePayment(amount))
            throw new InvalidOperationException("Importo del pagamento non valido.");
        return _paymentProcessor.ProcessPayment(amount);
    }

    public void Refund(Guid transactionId) => _refundProcessor.ProcessRefund(transactionId);
}
```

Poiché tutti e tre i prodotti sono ottenuti dalla stessa factory, appartengono **sicuramente alla stessa famiglia**.

**Dimostrazione**: `DemontrateAbstractFactory` crea `PaymentService` prima con la factory di PoolPay, poi con quella di Strope, ed esegue le stesse operazioni `Pay` + `Refund`. Il codice del servizio è lo stesso: cambia solo la factory passata.

Per collegare un nuovo provider basta aggiungere una cartella con la factory e i tre prodotti. `PaymentService` e i provider esistenti non cambiano.

---

## Quando usarlo

- Il sistema deve lavorare con diverse **famiglie** di oggetti correlati, che devono essere usati **insieme**.
- Bisogna nascondere al client le classi concrete dei prodotti.
- La famiglia viene scelta una volta sola (tramite configurazione o impostazioni) e poi usata ovunque.
- Esempi: provider di pagamento, temi UI (pulsante + checkbox + finestra per Windows/macOS), provider di database (`DbProviderFactory` in ADO.NET: connessione + comando + parametro), provider cloud (storage + coda + cache).

## Quando non serve

- C'è un solo prodotto e non una famiglia: basta il Factory Method.
- C'è una sola famiglia e non verrà estesa.

## Pro e contro

**Pro**
- Garantisce la compatibilità dei prodotti creati.
- Il client non dipende dalle classi concrete.
- Il codice di creazione dei prodotti è raccolto in un unico punto (SRP).
- Una nuova famiglia si aggiunge senza modificare il client (Open/Closed).

**Contro**
- Molte interfacce e classi.
- Aggiungere un **nuovo tipo di prodotto** (ad esempio `ISubscriptionProcessor`) è difficile: bisogna modificare l'interfaccia della factory e **tutte** le factory concrete.

## Relazioni con altri pattern

- **Factory Method**: una fabbrica astratta è spesso composta da più factory method. Factory Method crea **un solo** prodotto e si basa sull'ereditarietà; Abstract Factory crea una **famiglia** e viene passata come oggetto (composizione).
- **Builder** assembla **un solo oggetto complesso** passo dopo passo; Abstract Factory restituisce subito un **insieme** di oggetti semplici.
- **Prototype**: la factory può creare i prodotti clonando dei prototipi.
- **Singleton**: di solito una factory concreta serve in un'unica istanza.
- **Facade**: una fabbrica astratta può essere usata al posto di una facade per nascondere come vengono creati gli oggetti del sottosistema.
- **Strategy**: dal punto di vista di `PaymentService`, la factory appare come una strategia «quale provider usare».
