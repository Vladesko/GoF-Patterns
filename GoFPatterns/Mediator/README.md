🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Mediator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Mediator) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Mediator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Mediator)

# Mediator (Vermittler)

**Typ:** Verhaltensmuster.

**Mediator** verringert die Kopplung vieler Objekte, indem er sie zwingt, **nicht direkt, sondern über ein Vermittlerobjekt** zu kommunizieren. Die Objekte kennen nur den Vermittler, und die gesamte Interaktionslogik ist in ihm gebündelt.

---

## Welches Problem wird gelöst

**Registrierungsformular.** Das Kontrollkästchen „Ich bin ein Unternehmen“ blendet das Feld „Firmenname“ ein. Die Schaltfläche „Registrieren“ ist nur aktiv, wenn die E-Mail korrekt, das Passwort lang genug, die Bedingungen akzeptiert und bei einem Unternehmen der Name ausgefüllt ist. Kommunizieren die Komponenten direkt miteinander:

```text
 CheckBox(IsCompany) ──► TextBox(CompanyName)
        │                       │
        ▼                       ▼
     Button(Submit) ◄── TextBox(Email) ◄── TextBox(Password) ◄── CheckBox(Terms)
```

…kennt jede Komponente eine Menge anderer, und die Verbindungen bilden ein „Spinnennetz“. `TextBox` lässt sich in einem anderen Formular nicht wiederverwenden – sie schleppt das Wissen über die Schaltfläche und die Kontrollkästchen mit. Jede neue Regel erfordert Änderungen in mehreren Klassen.

**Chat.** Hält jeder Benutzer Referenzen auf alle anderen, müssen beim Betreten/Verlassen alle aktualisiert werden, und Spamfilter und private Nachrichten verteilen sich auf die Benutzer.

## Lösung

Die gesamte Interaktionslogik wird in ein eigenes Objekt ausgelagert – den **Vermittler**. Die Komponenten:
- halten eine Referenz **nur** auf den Vermittler;
- melden ihm Ereignisse („ich wurde geändert“, „sende eine Nachricht“);
- wissen nicht, wer wie darauf reagiert.

Der Vermittler kennt alle Beteiligten und entscheidet, wen er wie benachrichtigt. Aus „viele-zu-viele“-Beziehungen werden „eins-zu-viele“-Beziehungen.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Mediator** | `IChatMediator`, `IFormMediator` | Schnittstelle für die Kommunikation der Komponenten mit dem Vermittler |
| **ConcreteMediator** | `ChatRoomMediator`, `RegistrationDialog` | Kennt alle Beteiligten und enthält die Logik ihrer Interaktion |
| **Colleague** | `ChatUser`, `FormComponent` | Basisklasse eines Beteiligten, hält eine Referenz auf den Vermittler |
| **ConcreteColleague** | `ChatUser`, `ChatBot`, `TextBox`, `CheckBox`, `Button` | Konkrete Beteiligte. Melden dem Vermittler Ereignisse, ohne voneinander zu wissen |

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

## Beispiel 1. Chatraum (`ChatRoom/`)

**Mediator** – `IChatMediator`: `Join`, `Leave`, `SendMessage`, `SendPrivate`.

**ConcreteMediator** – `ChatRoomMediator` hält ein Dictionary der Teilnehmer und enthält **alle Regeln** der Kommunikation:
- Versand einer Nachricht an alle außer dem Absender;
- Blockieren von Nachrichten mit verbotenen Wörtern;
- private Nachrichten per Namen und eine Antwort, wenn der Empfänger nicht im Raum ist;
- Benachrichtigungen über Betreten/Verlassen.

```csharp
public void SendMessage(ChatUser sender, string message)
{
    if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
        return;   // die Moderationsregel steht nur hier

    foreach (var user in _users.Values.Where(u => u != sender))
        user.Receive(sender.Name, message);
}
```

**Colleague** – `ChatUser` kennt nur seinen Vermittler:

```csharp
public void Send(string message) => chat.SendMessage(this, message);
```

**ConcreteColleague** – `ChatBot` ist ebenfalls ein Teilnehmer, aber mit eigener Reaktion: Auf den Befehl `/time` antwortet er mit einer privaten Nachricht. Die Benutzer wissen nichts vom Bot – er ist einfach ein weiterer Teilnehmer im Raum.

## Beispiel 2. Registrierungsformular (`RegistrationForm/`) – das klassische GoF-Beispiel

**Colleague** – die abstrakte Klasse `FormComponent` (Name, `IsEnabled`, `IsVisible`, Referenz auf den Vermittler).

**ConcreteColleague** (Ordner `Components/`) – `TextBox`, `CheckBox`, `Button`. Bei einer Benutzeraktion meldet jede Komponente dem Vermittler nur das Ereignis:

```csharp
public void Type(string text)
{
    Text = text;
    Mediator?.Notify(this, "changed");
}
```

Diese Komponenten sind **universell** – sie lassen sich in jedem Formular verwenden und wissen von keinen anderen Feldern.

**ConcreteMediator** – `RegistrationDialog` erzeugt die Komponenten, setzt sich selbst als ihren Vermittler und implementiert in `Notify` die Regeln des Formulars:
- das Kontrollkästchen `IsCompany` blendet das Feld `CompanyName` ein bzw. aus;
- nach jeder Änderung wird die Verfügbarkeit der Schaltfläche `Submit` neu berechnet;
- ein Klick auf `Submit` führt die Registrierung durch.

```csharp
var isValid = Email.Text.Contains('@')
              && Password.Text.Length >= 6
              && AcceptTerms.IsChecked
              && (!IsCompany.IsChecked || CompanyName.Text.Length > 0);
```

**Client** – die Demo simuliert die Aktionen eines Benutzers. Man sieht, wie die Schaltfläche aktiv wird, dann wieder inaktiv (es wurde „Unternehmen“ angehakt, aber kein Name eingegeben) und schließlich wieder aktiv.

---

## Wann einsetzen

- Viele Objekte sind auf komplexe Weise verbunden, und die Verbindungen sind schwer zu verstehen und zu ändern.
- Komponenten sollen wiederverwendet werden, hängen aber zu stark von anderen ab.
- Die Interaktionslogik soll an einer Stelle liegen und ohne Änderung der Komponenten anpassbar sein.
- Beispiele: Dialoge und Formulare in UIs, Chats, Flugsicherung (Flugzeuge kommunizieren über den Tower), Koordination von Microservices (Orchestrator).

## Wann nicht nötig

- Es gibt wenige Objekte und einfache Verbindungen – dann wird der Vermittler zur überflüssigen Ebene.

## Vor- und Nachteile

**Vorteile**
- Beseitigt direkte Abhängigkeiten zwischen den Komponenten.
- Die Interaktionslogik liegt an einer Stelle (SRP).
- Komponenten lassen sich leicht wiederverwenden.
- Neue Regeln kommen in den Vermittler, ohne die Komponenten zu ändern.

**Nachteile**
- Der Vermittler kann zu einem **„Gottobjekt“** werden, das alles weiß und alles tut.

## Beziehung zu anderen Mustern

- **Facade** vereinfacht ebenfalls die Arbeit mit einer Gruppe von Objekten, aber das Subsystem **weiß nichts** von der Fassade, und die Verbindung ist einseitig. Die Beteiligten bei Mediator **kennen** den Vermittler und kommunizieren über ihn.
- **Observer** – ein Vermittler wird oft über Ereignisse/Abonnements umgesetzt: Die Komponenten veröffentlichen Ereignisse, der Vermittler hat sie abonniert. Der Unterschied liegt im Ziel: Observer ist ein dynamisches „eins-zu-viele“-Abonnement, Mediator eine zentralisierte Interaktionslogik.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer** – verschiedene Wege, Absender und Empfänger von Anfragen zu verbinden.
- Die Bibliothek **MediatR** in .NET ist nach dem Muster benannt: Die Handler wissen nichts voneinander, und Anfragen werden über `IMediator` verschickt.
