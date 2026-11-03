🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/ChainOfResponsibility) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/ChainOfResponsibility) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/ChainOfResponsibility) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/ChainOfResponsibility)

# Chain of Responsibility (Catena di responsabilità)

**Tipo:** pattern comportamentale.

**Chain of Responsibility** permette di passare una richiesta in sequenza lungo una **catena di handler**. Ogni handler decide se elaborare la richiesta da solo, passarla al successivo nella catena, oppure fare entrambe le cose.

---

## Quale problema risolve

**Servizio di assistenza.** Una richiesta può essere chiusa dal bot (se è una domanda frequente), da un operatore (problema semplice), da un ingegnere (bug) o da un manager (incidente). Se lo si scrive in un solo metodo:

```csharp
if (ticket.Severity == Low && IsFaq(ticket)) Bot(...);
else if (ticket.Severity <= Medium) Operator(...);
else if (ticket.Severity == High) Engineer(...);
else Manager(...);
```

…qualsiasi nuovo livello di supporto, un cambio d'ordine o il turno di notte «senza bot e manager» richiedono di modificare questo metodo. Il mittente è rigidamente legato a tutti i destinatari.

**Elaborazione di una richiesta HTTP.** Prima che la richiesta arrivi alla logica di business bisogna: registrarla nel log, verificare il limite di richieste, autenticare, convalidare. Ogni controllo può **interrompere** l'elaborazione. Se si scrive tutto nel controller, questo crescerà a dismisura e mescolerà responsabilità diverse.

## Soluzione

Trasformare ogni controllo/elaborazione in un oggetto **handler** separato con un'interfaccia comune. Gli handler vengono collegati in una catena: ognuno conosce solo il **successivo**. Il client invia la richiesta al **primo** handler e non sa chi alla fine la elaborerà.

Esistono due varianti di comportamento della catena:
1. **Classica (GoF):** la richiesta viene elaborata da **un solo** handler, il primo che può farlo; gli altri si limitano a passarla avanti (esempio `SupportTickets`).
2. **Pipeline (middleware):** **ogni** handler esegue la propria parte di lavoro e decide se chiamare il successivo; chiunque può interrompere la catena (esempio `Middleware`).

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Handler** | `SupportHandler`, `IRequestHandler` | Interfaccia comune degli handler. Spesso contiene un riferimento all'handler successivo (o lo riceve come delegate) |
| **ConcreteHandler** | `ChatBotHandler`, `OperatorHandler`, `EngineerHandler`, `ManagerHandler`, `LoggingHandler`, `RateLimitHandler`, `AuthenticationHandler`, `ValidationHandler`, `EndpointHandler` | Decide se può elaborare la richiesta; se no, la passa avanti |
| **Request** | `SupportTicket`, `HttpRequestContext` | I dati che viaggiano lungo la catena |
| **Client** | classi demo, `RequestPipeline` | Assembla la catena e invia la richiesta al primo anello |

## Schema

```text
 Client ──► ChatBot ──► Operator ──► Engineer ──► Manager ──► (null)
              │            │            │            │
          Low+FAQ?     ≤ Medium?     High?       Critical?
          gestisce     gestisce     gestisce     gestisce
```

---

## Esempio 1. Servizio di assistenza (`SupportTickets/`): catena classica

**Request**: `SupportTicket(Id, Description, Severity)` (`record`), il livello è `TicketSeverity`.

**Handler**: la classe astratta `SupportHandler` mantiene il riferimento al successivo e implementa il comportamento «predefinito», cioè passare avanti:

```csharp
public SupportHandler SetNext(SupportHandler next)
{
    _next = next;
    return next;           // restituiamo next per costruire la catena «in linea»
}

public virtual void Handle(SupportTicket ticket)
{
    if (_next is not null)
        _next.Handle(ticket);
    else
        Console.WriteLine($"Nessuno ha potuto gestire il ticket #{ticket.Id}");
}
```

**ConcreteHandler** (cartella `Handlers/`): ognuno verifica la «propria» condizione; se è soddisfatta elabora la richiesta e **interrompe** la catena (`return`), altrimenti chiama `base.Handle(ticket)`:

```csharp
public override void Handle(SupportTicket ticket)
{
    if (ticket.Severity <= TicketSeverity.Medium)
    {
        Console.WriteLine($"Operatore: ha risolto il ticket #{ticket.Id}");
        return;
    }
    base.Handle(ticket);
}
```

**Client**: assembla la catena `bot → operatore → ingegnere → manager` e invia tutte le richieste **solo al primo anello**. Nell'output si vede fin dove è arrivata ogni richiesta. Alla fine viene mostrata un'altra configurazione («turno di notte»: solo l'operatore): nessuno ha preso in carico la richiesta critica. È una situazione normale per il pattern e va prevista.

## Esempio 2. Pipeline di elaborazione delle richieste (`Middleware/`): come in ASP.NET Core

**Request**: `HttpRequestContext`: i dati di input (percorso, IP, token, corpo) e il risultato (codice di risposta, testo). Gli handler possono **arricchire** il contesto; ad esempio l'autenticazione scrive `UserName`.

**Handler**: `IRequestHandler.Handle(context, Action next)`. L'handler non conserva il riferimento al successivo, ma lo riceve come **delegate**: così gli handler sono completamente indipendenti.

**ConcreteHandler** (cartella `Handlers/`):
- `LoggingHandler`: lavora **prima e dopo** il resto della catena (scrive la richiesta, chiama `next()`, scrive la risposta);
- `RateLimitHandler`: dopo N richieste dallo stesso IP risponde `429` e **non chiama** `next()`;
- `AuthenticationHandler`: verifica il token (`401`), altrimenti scrive l'utente nel contesto;
- `ValidationHandler`: verifica il corpo (`400`);
- `EndpointHandler`: il punto finale, esegue il lavoro (`201`).

**Client**: `RequestPipeline` assembla la catena con il metodo `Use(...)` e collega ricorsivamente gli handler:

```csharp
private void Invoke(int index, HttpRequestContext context)
{
    if (index >= _handlers.Count) return;
    _handlers[index].Handle(context, () => Invoke(index + 1, context));
}
```

È una copia semplificata di come funziona `app.Use(...)` in ASP.NET Core: ogni middleware riceve `next` e decide da solo se passare la richiesta avanti. Sono catene anche i `DelegatingHandler` di `HttpClient` e i pipeline behavior di MediatR.

---

## Quando usarlo

- La richiesta può essere elaborata da oggetti diversi, e non si sa in anticipo da quale.
- L'insieme e l'ordine degli handler devono essere configurabili dinamicamente.
- Bisogna eseguire in sequenza diversi controlli/passi indipendenti, con la possibilità di interrompere.
- Bisogna separare il mittente della richiesta dai destinatari.

## Quando non serve

- L'handler è sempre uno solo e noto in anticipo.
- L'ordine e l'insieme dei passi non cambiano mai: un semplice codice sequenziale è più leggibile.

## Pro e contro

**Pro**
- Riduce l'accoppiamento tra mittente e destinatari.
- Ogni handler è responsabile di una sola cosa (SRP).
- Nuovi handler si aggiungono senza modificare quelli esistenti (Open/Closed).
- La catena può essere ricostruita a runtime.

**Contro**
- La richiesta può restare non elaborata.
- Il debug è più difficile: il percorso della richiesta non è visibile nel codice in un unico punto.
- L'ordine degli anelli è importante (ad esempio, il logging deve stare per primo e l'autenticazione prima della convalida).

## Relazioni con altri pattern

- **Decorator** è strutturato in modo simile (una catena di wrapper), ma il decorator passa **sempre** la chiamata avanti e aggiunge comportamento; un anello della catena può **interrompere** l'elaborazione.
- **Command**: la richiesta nella catena viene spesso rappresentata come un comando.
- **Composite**: la catena viene spesso costruita su un albero, con la richiesta che passa da un elemento al suo genitore (ad esempio la gestione degli eventi nelle UI).
- **Mediator** e **Observer**: altri modi di collegare mittenti e destinatari, tramite un intermediario o tramite sottoscrizione.
