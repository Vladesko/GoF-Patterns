🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Facade) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Facade) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Facade) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Facade)

# Facade (Facciata)

**Tipo:** pattern strutturale.

**Facade** fornisce un'**interfaccia semplice** verso un sottosistema complesso (un insieme di classi, una libreria, un framework). La facade sa da sola a quali oggetti del sottosistema passare la richiesta e in quale ordine.

---

## Quale problema risolve

Per guardare un film con l'home theater bisogna: abbassare le luci, accendere il proiettore, scegliere l'ingresso, attivare la modalità widescreen, accendere l'audio, scegliere il surround, impostare il volume, accendere il lettore, avviare il film. E alla fine spegnere tutto nell'ordine giusto.

Per inviare un ordine in un negozio online: verificare le scorte, prenotare il prodotto, calcolare la spedizione, addebitare il pagamento, in caso di rifiuto annullare la prenotazione, creare la spedizione, avvisare il cliente.

Se il codice client (UI, controller) fa tutto da solo:
- è **fortemente accoppiato** a una decina di classi del sottosistema e conosce i dettagli del loro funzionamento;
- la stessa sequenza viene **duplicata** in tutti i punti in cui serve;
- qualsiasi modifica del sottosistema rompe tutti i client.

## Soluzione

Creare una classe **facade** che:
- mantiene i riferimenti agli oggetti del sottosistema;
- offre alcuni metodi di alto livello (`WatchMovie`, `PlaceOrder`);
- al suo interno chiama gli oggetti del sottosistema necessari nell'ordine giusto.

Il client dipende solo dalla facade. Il sottosistema però **non viene nascosto del tutto**: se serve qualcosa di particolare, si può accedere direttamente alle sue classi.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Facade** | `HomeTheaterFacade`, `OrderFacade` | Offre un'interfaccia semplice, sa a chi e in quale ordine delegare il lavoro |
| **Subsystem classes** | `Projector`, `SoundSystem`, `MediaPlayer`, `Lights`, `InventoryService`, `PaymentGateway`, `ShippingService`, `NotificationService` | Eseguono il lavoro reale. **Non sanno nulla della facade** |
| **Client** | classi demo | Lavora con il sottosistema tramite la facade |

## Schema

```text
            Client
              │  WatchMovie("Inception")
              ▼
   ┌──────────────────────┐
   │  HomeTheaterFacade   │
   └──────────────────────┘
     │       │       │       │
     ▼       ▼       ▼       ▼
 Projector Sound  MediaPlayer Lights      ← sottosistema
            System
```

---

## Esempio 1. Home theater (`HomeTheater/`)

**Subsystem**: le classi nella cartella `Subsystems/` (`Projector`, `SoundSystem`, `MediaPlayer`, `Lights`). Ognuna ha il proprio insieme di metodi di basso livello. Non dipendono né l'una dall'altra né dalla facade.

**Facade**: `HomeTheaterFacade` riceve gli oggetti del sottosistema tramite il costruttore e li combina in scenari:

```csharp
public void WatchMovie(string movie)
{
    lights.Dim(10);
    projector.On();
    projector.SetInput("HDMI-1");
    projector.WideScreenMode();
    sound.On();
    sound.SetSurroundSound();
    sound.SetVolume(25);
    player.On();
    player.Play(movie);
}
```

`EndMovie()` spegne tutto in ordine inverso.

**Client**: chiama `theater.WatchMovie("Inception")`, una riga invece di dieci. Alla fine della demo si vede che l'accesso diretto al sottosistema (`new Lights().Dim(50)`) è ancora possibile.

## Esempio 2. Invio dell'ordine (`OrderPlacement/`)

**Problema:** un processo di business che coinvolge più servizi, con controlli e rollback in caso di errore.

**Subsystem**: `InventoryService` (scorte e prenotazione), `PaymentGateway` (addebito; le carte che terminano con `0000` vengono rifiutate), `ShippingService` (costo e creazione della spedizione), `NotificationService` (e-mail).

**Facade**: `OrderFacade.PlaceOrder(OrderRequest)` incapsula l'intero processo:

```csharp
if (!inventory.IsAvailable(order.Product, order.Count))
    return new OrderResult(false, $"{order.Product} non è disponibile in magazzino");

inventory.Reserve(order.Product, order.Count);

var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
if (!payments.Charge(order.CardNumber, total))
{
    inventory.Release(order.Product, order.Count);   // rollback
    return new OrderResult(false, "Il pagamento è stato rifiutato");
}
```

La facade conosce non solo l'ordine delle chiamate, ma anche le **regole di coordinamento** dei sottosistemi: ad esempio, che la prenotazione va annullata se il pagamento non è andato a buon fine. I dati di input e di output sono racchiusi in semplici `record` (`OrderRequest`, `OrderResult`), così il client non deve conoscere i tipi del sottosistema.

**Client**: la demo invia tre ordini: uno riuscito, uno con un prodotto esaurito e uno con la carta rifiutata. Il client vede solo il risultato.

In un'applicazione reale ASP.NET Core una facade del genere si chiama di solito «servizio applicativo» (`OrderService`, `CheckoutService`), e il controller dipende solo da essa.

---

## Quando usarlo

- Serve un'interfaccia semplice verso un sottosistema complesso o una libreria di terze parti.
- Bisogna ridurre l'accoppiamento del client con molte classi.
- Bisogna dividere il sistema in livelli: la facade è il punto di ingresso di ogni livello/modulo.

## Quando non serve

- Il sottosistema è già semplice: la facade diventerebbe uno strato superfluo.
- I client hanno costantemente bisogno di diverse funzionalità di basso livello: la facade sarebbe solo d'intralcio.

## Pro e contro

**Pro**
- Isola i client dai componenti del sottosistema.
- Riduce l'accoppiamento: il sottosistema si può modificare senza toccare i client.
- Elimina la duplicazione degli scenari.

**Contro**
- Rischia di trasformarsi in un **«god object»** che sa tutto e fa tutto. Meglio alcune piccole facade, una per compito.

## Relazioni con altri pattern

- **Adapter** adatta **un'unica** interfaccia esistente a quella richiesta; **Facade** inventa una **nuova interfaccia semplificata** per un intero sottosistema.
- Anche **Mediator** centralizza l'interazione, ma gli oggetti del sottosistema **conoscono** il mediator e comunicano tramite esso; in Facade il sottosistema non conosce la facade e il collegamento va in una sola direzione.
- **Abstract Factory** può essere usata insieme a Facade per nascondere come vengono creati gli oggetti del sottosistema.
- La facade è spesso un **Singleton**, perché di solito ne serve una sola istanza.
- **Proxy** ha la stessa interfaccia dell'oggetto; **Facade** ha un'interfaccia propria.
