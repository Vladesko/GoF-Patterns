🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Proxy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Proxy) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Proxy) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Proxy)

# Proxy (Sostituto)

**Tipo:** pattern strutturale.

**Proxy** è un oggetto sostituto che implementa **la stessa interfaccia** dell'oggetto reale e ne **controlla l'accesso**: intercetta le chiamate e può fare qualcosa prima o dopo averle passate all'originale (oppure non passarle affatto).

---

## Quale problema risolve

C'è un oggetto a cui non si può o non conviene accedere direttamente:

- è **pesante**: una galleria di 100 foto da 20 MB non dovrebbe caricarle tutte all'apertura se l'utente ne guarderà solo due;
- richiede un **controllo degli accessi**: solo il titolare può prelevare denaro dal conto;
- le sue chiamate sono **costose** e il risultato merita di essere **messo in cache**: la richiesta del saldo al sistema centrale della banca;
- bisogna **registrare/verificare** gli accessi (logging, audit);
- l'oggetto si trova **in remoto** (un altro servizio, un'altra macchina).

Aggiungere tutto questo alla classe stessa è una cattiva idea: inizia a occuparsi di sicurezza, cache e logging invece della propria logica di business. E spesso la classe non si può proprio modificare (codice di terze parti).

## Soluzione

Creare una classe sostituto con la stessa interfaccia. Il proxy mantiene un riferimento all'oggetto reale (o lo crea da solo quando serve) e gli delega il lavoro, aggiungendo la propria logica. Il client lavora con l'interfaccia e **non si accorge della sostituzione**.

### Tipi di proxy

| Tipo | Cosa fa | Esempio nel progetto |
|------|---------|----------------------|
| **Virtual proxy** | Rimanda la creazione di un oggetto pesante fino al primo accesso | `LazyImageProxy` |
| **Protection proxy** | Verifica i diritti di accesso | `ProtectionAccountProxy` |
| **Caching proxy** | Mette in cache i risultati | `CachingAccountProxy` |
| **Logging proxy** | Registra gli accessi | `LoggingAccountProxy` |
| **Remote proxy** | Rappresenta un oggetto di un altro spazio di indirizzamento (client gRPC/WCF, wrapper di `HttpClient`) | — |
| **Smart reference** | Conteggio dei riferimenti, rilascio delle risorse quando l'oggetto non serve più | — |

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Subject** | `IImage`, `IBankAccount` | Interfaccia comune dell'oggetto reale e del proxy. Grazie a essa il proxy può essere usato al posto dell'originale |
| **RealSubject** | `HighResolutionImage`, `BankAccount` | L'oggetto reale con la logica di business utile |
| **Proxy** | `LazyImageProxy`, `ProtectionAccountProxy`, `CachingAccountProxy`, `LoggingAccountProxy` | Mantiene un riferimento al RealSubject, ne controlla l'accesso, può gestirne il ciclo di vita |
| **Client** | classi demo | Lavora tramite il Subject e non sa se ha davanti l'originale o un proxy |

## Schema

```text
               ┌──────────────────────┐
  Client ─────►│ «interface» IImage   │
               │ Display()            │
               └──────────────────────┘
                    ▲             ▲
                    │             │
     ┌──────────────────┐   ┌───────────────────────┐
     │ LazyImageProxy   │──►│ HighResolutionImage   │
     │ - _realImage?    │   │ (pesante, caricato    │
     │ Display():       │   │  nel costruttore)     │
     │  crea se null,   │   └───────────────────────┘
     │  poi delega      │
     └──────────────────┘
```

---

## Esempio 1. Caricamento lazy delle immagini (`ImageLoading/`): virtual proxy

**Subject**: `IImage` (`FileName`, `Display()`).

**RealSubject**: `HighResolutionImage` carica l'immagine dal disco direttamente nel costruttore (simulato con `Thread.Sleep`). Già creare un oggetto del genere è costoso.

**Proxy**: `LazyImageProxy` conserva solo il nome del file e crea l'oggetto reale alla **prima** chiamata di `Display()`:

```csharp
public void Display()
{
    _realImage ??= new HighResolutionImage(FileName);
    _realImage.Display();
}
```

**Client**: la demo apre una galleria di 5 immagini in due modi:
- senza proxy: tutti e 5 i file vengono caricati subito (~1 s);
- con il proxy: la galleria si apre istantaneamente e vengono caricate solo le 2 immagini aperte dall'utente. La visualizzazione successiva non ricarica il file.

In .NET un ruolo simile è svolto da `Lazy<T>`, e in Entity Framework dai **lazy-loading proxy** per le proprietà di navigazione.

## Esempio 2. Conto bancario (`BankAccount/`): protection, caching, logging

**Subject**: `IBankAccount` (`GetBalance()`, `Withdraw()`).

**RealSubject**: `BankAccount` contiene **solo** la logica di business. La richiesta del saldo è lenta (simula l'accesso al sistema centrale della banca), e il contatore `BalanceRequests` mostra quante volte si è effettivamente acceduto alla banca.

**Proxies** (cartella `Proxies/`):

`ProtectionAccountProxy`: solo `UserRole.Owner` può prelevare denaro:

```csharp
public void Withdraw(decimal amount)
{
    if (role != UserRole.Owner)
        throw new UnauthorizedAccessException($"{role} non può prelevare denaro.");
    account.Withdraw(amount);
}
```

`CachingAccountProxy`: memorizza il saldo e **invalida la cache** dopo un prelievo:

```csharp
public decimal GetBalance() => _cachedBalance ??= account.GetBalance();
```

`LoggingAccountProxy`: scrive l'audit di tutte le operazioni, comprese quelle non riuscite.

**Client**: poiché tutti i proxy implementano `IBankAccount`, si possono annidare l'uno nell'altro:

```csharp
IBankAccount ownerAccess = new LoggingAccountProxy(
    new ProtectionAccountProxy(
        new CachingAccountProxy(realAccount), UserRole.Owner));
```

La demo mostra che: tre richieste del saldo producono **un solo** accesso reale alla banca; dopo il prelievo la cache viene invalidata e il saldo è aggiornato; l'ospite riceve un rifiuto, che finisce nell'audit.

---

## Quando usarlo

- Inizializzazione lazy di un oggetto pesante (virtual proxy).
- Controllo degli accessi (protection proxy).
- Cache dei risultati (caching proxy).
- Logging, metriche, audit degli accessi.
- Lavorare con un oggetto remoto come se fosse locale (remote proxy).

## Quando non serve

- L'oggetto è leggero e non richiede controlli: sarebbe un livello di indirezione superfluo.
- Non serve controllare, ma proprio **aggiungere** un nuovo comportamento che il client sceglie e combina da solo: questo è più vicino a Decorator.

## Pro e contro

**Pro**
- Controllo sull'oggetto in modo trasparente per il client.
- Si può gestire il ciclo di vita dell'oggetto reale (crearlo dopo, rilasciarlo prima).
- Funziona anche se l'oggetto reale non è ancora stato creato o non è disponibile.
- Open/Closed: nuovi proxy si aggiungono senza modificare il servizio e il client.

**Contro**
- Un livello di indirezione in più, con possibili ritardi nella risposta.
- Aumenta il numero di classi.

## Relazioni con altri pattern

- **Adapter** offre un'interfaccia **diversa**; **Proxy** la **stessa**; **Decorator** la stessa, ma estesa.
- **Decorator** e **Proxy** sono strutturati quasi allo stesso modo (wrapper + delega), la differenza sta nell'intento: il decorator **aggiunge** comportamento e la sua composizione è decisa dal client; il proxy **controlla l'accesso** e di solito gestisce da solo il ciclo di vita dell'oggetto (come `LazyImageProxy`, che crea da solo `HighResolutionImage`).
- Anche **Facade** nasconde un oggetto dietro di sé, ma la facade ha una propria interfaccia semplificata, mentre il proxy ha l'interfaccia dell'originale.
