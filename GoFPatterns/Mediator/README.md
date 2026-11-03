🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Mediator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Mediator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Mediator) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Mediator)

# Mediator (Mediatore)

**Tipo:** pattern comportamentale.

**Mediator** riduce l'accoppiamento tra molti oggetti, costringendoli a comunicare **non direttamente, ma tramite un oggetto mediatore**. Gli oggetti conoscono solo il mediatore, e tutta la logica di interazione è concentrata in esso.

---

## Quale problema risolve

**Modulo di registrazione.** La casella «Sono un'azienda» mostra il campo «Nome dell'azienda». Il pulsante «Registrati» è attivo solo se l'e-mail è corretta, la password è abbastanza lunga, le condizioni sono accettate e, per un'azienda, il nome è compilato. Se i componenti comunicano direttamente:

```text
 CheckBox(IsCompany) ──► TextBox(CompanyName)
        │                       │
        ▼                       ▼
     Button(Submit) ◄── TextBox(Email) ◄── TextBox(Password) ◄── CheckBox(Terms)
```

…ogni componente conosce una marea di altri componenti, e i collegamenti formano una «ragnatela». Non si può riutilizzare `TextBox` in un altro modulo, perché si porta dietro la conoscenza del pulsante e delle caselle. Qualsiasi nuova regola richiede modifiche in più classi.

**Chat.** Se ogni utente conserva i riferimenti a tutti gli altri, a ogni ingresso/uscita bisogna aggiornare tutti, e il filtro antispam e i messaggi privati si sparpagliano tra gli utenti.

## Soluzione

Spostare tutta la logica di interazione in un oggetto separato, il **mediatore**. I componenti:
- conservano un riferimento **solo** al mediatore;
- gli comunicano gli eventi («sono stato modificato», «invia un messaggio»);
- non sanno chi reagirà e come.

Il mediatore conosce tutti i partecipanti e decide chi avvisare e come. I collegamenti «molti a molti» diventano «uno a molti».

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Mediator** | `IChatMediator`, `IFormMediator` | Interfaccia di comunicazione dei componenti con il mediatore |
| **ConcreteMediator** | `ChatRoomMediator`, `RegistrationDialog` | Conosce tutti i partecipanti e contiene la logica della loro interazione |
| **Colleague** | `ChatUser`, `FormComponent` | Classe base del partecipante, conserva il riferimento al mediatore |
| **ConcreteColleague** | `ChatUser`, `ChatBot`, `TextBox`, `CheckBox`, `Button` | Partecipanti concreti. Comunicano gli eventi al mediatore senza sapere nulla l'uno dell'altro |

## Schema

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

## Esempio 1. Chat room (`ChatRoom/`)

**Mediator**: `IChatMediator`: `Join`, `Leave`, `SendMessage`, `SendPrivate`.

**ConcreteMediator**: `ChatRoomMediator` conserva un dizionario dei partecipanti e contiene **tutte le regole** di comunicazione:
- invio del messaggio a tutti tranne il mittente;
- blocco dei messaggi con parole vietate;
- messaggi privati per nome e risposta se il destinatario non è nella stanza;
- notifiche di ingresso/uscita.

```csharp
public void SendMessage(ChatUser sender, string message)
{
    if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
        return;   // regola di moderazione: solo qui

    foreach (var user in _users.Values.Where(u => u != sender))
        user.Receive(sender.Name, message);
}
```

**Colleague**: `ChatUser` conosce solo il proprio mediatore:

```csharp
public void Send(string message) => chat.SendMessage(this, message);
```

**ConcreteColleague**: anche `ChatBot` è un partecipante, ma con una propria reazione: al comando `/time` risponde con un messaggio privato. Gli utenti non sanno nulla del bot: è semplicemente un altro partecipante della stanza.

## Esempio 2. Modulo di registrazione (`RegistrationForm/`): l'esempio classico della GoF

**Colleague**: la classe astratta `FormComponent` (nome, `IsEnabled`, `IsVisible`, riferimento al mediatore).

**ConcreteColleague** (cartella `Components/`): `TextBox`, `CheckBox`, `Button`. A ogni azione dell'utente ciascuno si limita a comunicare l'evento al mediatore:

```csharp
public void Type(string text)
{
    Text = text;
    Mediator?.Notify(this, "changed");
}
```

Questi componenti sono **universali**: si possono usare in qualsiasi modulo, perché non conoscono nessun altro campo.

**ConcreteMediator**: `RegistrationDialog` crea i componenti, si assegna come loro mediatore e in `Notify` implementa le regole del modulo:
- la casella `IsCompany` mostra/nasconde il campo `CompanyName`;
- dopo ogni modifica viene ricalcolata l'abilitazione del pulsante `Submit`;
- la pressione di `Submit` esegue la registrazione.

```csharp
var isValid = Email.Text.Contains('@')
              && Password.Text.Length >= 6
              && AcceptTerms.IsChecked
              && (!IsCompany.IsChecked || CompanyName.Text.Length > 0);
```

**Client**: la demo simula le azioni dell'utente. Si vede come il pulsante diventa attivo, poi di nuovo inattivo (è stata selezionata «azienda» ma non è stato inserito il nome), e poi di nuovo attivo.

---

## Quando usarlo

- Molti oggetti sono collegati in modo complesso, e i collegamenti sono difficili da capire e da modificare.
- Si vogliono riutilizzare i componenti, ma dipendono troppo dagli altri.
- La logica di interazione deve stare in un unico punto ed essere configurabile senza modificare i componenti.
- Esempi: finestre di dialogo e moduli nelle UI, chat, torre di controllo (gli aerei comunicano tramite la torre), coordinamento di microservizi (orchestratore).

## Quando non serve

- Gli oggetti sono pochi e i collegamenti semplici: il mediatore diventerebbe un livello superfluo.

## Pro e contro

**Pro**
- Elimina le dipendenze dirette tra i componenti.
- La logica di interazione è in un unico punto (SRP).
- I componenti sono facili da riutilizzare.
- Le nuove regole si aggiungono al mediatore, senza modificare i componenti.

**Contro**
- Il mediatore può trasformarsi in un **«god object»** che sa e fa tutto.

## Relazioni con altri pattern

- Anche **Facade** semplifica il lavoro con un gruppo di oggetti, ma il sottosistema **non conosce** la facade e il collegamento è unidirezionale. I partecipanti di Mediator **conoscono** il mediatore e comunicano tramite esso.
- **Observer**: il mediatore viene spesso implementato con eventi/sottoscrizioni: i componenti pubblicano eventi e il mediatore vi è sottoscritto. La differenza è nello scopo: Observer è una sottoscrizione dinamica «uno a molti», Mediator è una logica di interazione centralizzata.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer**: modi diversi di collegare mittenti e destinatari delle richieste.
- La libreria **MediatR** in .NET prende il nome dal pattern: gli handler non si conoscono tra loro e le richieste vengono inviate tramite `IMediator`.
