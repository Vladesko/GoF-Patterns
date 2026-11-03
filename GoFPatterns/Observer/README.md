🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Observer) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Observer) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Observer) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Observer) | **Español**

# Observer (Observador)

**Tipo:** patrón de comportamiento.

**Observer** crea un mecanismo de **suscripción**: unos objetos (los observadores) se suscriben a los cambios de otro objeto (el publicador) y reciben automáticamente notificaciones cuando en él ocurre algo. Es una relación «uno a muchos».

---

## Qué problema resuelve

El precio de un producto cambia. Distintas partes del sistema deben reaccionar a ello:
- el **comprador** espera a que el precio baje lo suficiente para comprar;
- el **servicio de descuentos** anuncia una oferta cuando el precio está por debajo de un umbral;
- el **recolector de estadísticas** cuenta todos los cambios de precio.

Variantes sin el patrón:
- **Sondeo (polling)**: cada objeto interesado pregunta periódicamente el precio. Carga innecesaria y retraso en la reacción.
- **El producto llama él mismo a todos**: `buyer.Check(price); discounts.Check(price); stats.Add(price);`: el producto conoce cada clase concreta. Un nuevo interesado implica modificar el producto. No se puede quitar a nadie mientras el programa se ejecuta.

## Solución

El publicador (el producto) guarda una **lista de suscriptores** y ofrece los métodos `Subscribe`/`Unsubscribe`. Todos los suscriptores implementan una interfaz común con el método `Update`. Cuando el estado cambia, el publicador recorre la lista y llama a `Update` en cada uno.

Lo único que el publicador sabe de los suscriptores es que implementan la interfaz. Los suscriptores se pueden añadir y quitar en tiempo de ejecución; incluso un suscriptor puede darse de baja él mismo.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Subject (Observable)** | `IObservable` | Interfaz del publicador: suscripción y baja |
| **ConcreteSubject** | `Product` | Guarda el estado (el precio) y la lista de suscriptores, y les notifica los cambios |
| **Observer** | `IObserver` | Interfaz del suscriptor con el método `Update(decimal price)` |
| **ConcreteObserver** | `Buyer`, `DiscountNotifier`, `StatisticsCollector` | Reaccionan a las notificaciones cada uno a su manera |
| **Client** | `ProductObserverDemonstrate` | Crea el publicador y los suscriptores, y cambia el estado |

## Esquema

```text
 ┌───────────────────────────┐        ┌──────────────────────────┐
 │ Product (ConcreteSubject) │        │ «interface» IObserver    │
 │ - _observers: HashSet ────┼──────► │ Update(price)            │
 │ + Subscribe(o)            │  0..*  └──────────────────────────┘
 │ + Unsubscribe(o)          │            ▲         ▲          ▲
 │ + ChangePrice(p):         │            │         │          │
 │     NotifyObservers()     │         Buyer  DiscountNotifier  StatisticsCollector
 └───────────────────────────┘    (≤ 50: compra  (≤ 100:          (cuenta cada
              ▲                    y se da baja)  descuento)        cambio)
              │ implementa
   «interface» IObservable
```

---

## Ejemplo. Cambio del precio de un producto (`ProductObserver/`)

**Subject**: `IObservable` con los métodos `Subscribe(IObserver)` y `Unsubscribe(IObserver)`.

**Observer**: `IObserver` con el método `Update(decimal price)`. El publicador pasa el nuevo valor directamente en la notificación (el llamado **modelo push**).

**ConcreteSubject**: `Product`:

```csharp
public class Product(decimal initialPrice) : IObservable
{
    private readonly HashSet<IObserver> _observers = [];
    private decimal _price = initialPrice;

    public void ChangePrice(decimal newPrice)
    {
        if (_price == newPrice) return;      // no notificamos si nada ha cambiado
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

Detalles importantes de la implementación:
- `HashSet`: un mismo suscriptor no quedará suscrito dos veces;
- `ArgumentNullException.ThrowIfNull` en `Subscribe`/`Unsubscribe`: protección contra `null`;
- `NotifyObservers` es `private`: solo el propio producto puede notificar a los suscriptores;
- el recorrido se hace sobre una **copia** de la lista (`_observers.ToArray()`), porque un suscriptor puede **darse de baja durante la propia notificación** (es lo que hace `Buyer`). No se puede modificar un `HashSet` mientras se recorre ese mismo `HashSet`: provocaría una `InvalidOperationException`.

**ConcreteObserver** (carpeta `Observers/`): cada uno se suscribe en el constructor (`_observable.Subscribe(this)`) y reacciona al precio a su manera:
- `StatisticsCollector`: cuenta cada cambio de precio y muestra el contador;
- `DiscountNotifier`: con un precio ≤ 100 anuncia un descuento;
- `Buyer`: con un precio ≤ 50 compra el producto y **se da de baja él mismo**, porque ya no le interesa:

```csharp
public void Update(decimal price)
{
    if (price > 50) return;
    Console.WriteLine("¡Comprando el producto!");
    _observable.Unsubscribe(this);
}
```

**Client**: `ProductObserverDemonstrate` crea un producto con precio 150 y tres suscriptores, y después cambia el precio: 120 → 90 → 40. Con cada cambio reaccionan las estadísticas; con 90 aparece el descuento; con 40, el descuento y la compra.

---

## Push y pull

- **Push** (como aquí): el publicador pasa los datos en `Update(price)`. Es sencillo, pero la interfaz queda ligada a datos concretos.
- **Pull**: el publicador se pasa a sí mismo (`Update(IObservable sender)`) y el suscriptor lee las propiedades que necesita. Es más flexible, pero el suscriptor depende del publicador.

## Observer en .NET

- Los **eventos de C#** (`event EventHandler<T>`) son la implementación integrada del patrón: `+=` es la suscripción, `-=` la baja, e invocar el evento es la notificación.
- Las interfaces `IObservable<T>` / `IObserver<T>` de la BCL y la biblioteca **Rx.NET**: flujos reactivos.
- `INotifyPropertyChanged` en WPF/MAUI: el enlace de la interfaz gráfica con el modelo.

## Cuándo usarlo

- El cambio de un objeto requiere la reacción de otros, y no se sabe de antemano cuáles ni cuántos.
- Los suscriptores deben poder añadirse y quitarse mientras el programa se ejecuta.
- Hay que debilitar el acoplamiento entre la fuente de eventos y las reacciones a ellos.

## Cuándo no hace falta

- Hay exactamente un suscriptor conocido de antemano: una llamada directa es más sencilla.
- Importan un orden estricto y la transaccionalidad de las reacciones: con los observadores el orden de notificación normalmente no está garantizado.

## Ventajas y desventajas

**Ventajas**
- El publicador no depende de las clases concretas de los suscriptores (Open/Closed).
- Suscripción y baja en tiempo de ejecución.
- Un evento, tantas reacciones como se quiera.

**Desventajas**
- El orden de notificación de los suscriptores no está garantizado.
- **Fugas de memoria**: si un suscriptor olvida darse de baja, el publicador mantiene una referencia a él y el recolector de basura no lo eliminará (un problema clásico de los eventos en .NET).
- Las cascadas de notificaciones pueden ser difíciles de depurar.

## Relación con otros patrones

- **Mediator**: suele implementarse mediante Observer: los componentes publican eventos y el mediador está suscrito a ellos. Los objetivos son distintos: Observer es una difusión dinámica; Mediator, una lógica de interacción centralizada.
- **Chain of Responsibility**, **Command**, **Mediator** y **Observer** son distintas formas de conectar emisores y receptores de peticiones.
- **Singleton**: a veces el publicador que gestiona los eventos se implementa como singleton (event bus).
