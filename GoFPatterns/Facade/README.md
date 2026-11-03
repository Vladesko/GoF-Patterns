🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Facade) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Facade) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Facade) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Facade) | **Español**

# Facade (Fachada)

**Tipo:** patrón estructural.

**Facade** proporciona una **interfaz sencilla** a un subsistema complejo (un conjunto de clases, una biblioteca, un framework). La fachada sabe a qué objetos del subsistema pasar la petición y en qué orden.

---

## Qué problema resuelve

Para ver una película en el cine en casa hay que: atenuar la luz, encender el proyector, elegir la entrada, activar el modo panorámico, encender el sonido, elegir el modo envolvente, ajustar el volumen, encender el reproductor y poner la película. Y al final apagarlo todo en el orden correcto.

Para realizar un pedido en una tienda online: comprobar las existencias, reservar el producto, calcular el envío, cobrar el dinero, anular la reserva si el pago falla, crear el envío y notificar al cliente.

Si el código cliente (interfaz, controlador) lo hace todo él mismo:
- está **fuertemente acoplado** a una decena de clases del subsistema y conoce los detalles de su funcionamiento;
- la misma secuencia se **duplica** en todos los lugares donde hace falta;
- cualquier cambio en el subsistema rompe a todos los clientes.

## Solución

Crear una clase **fachada** que:
- guarda referencias a los objetos del subsistema;
- ofrece unos pocos métodos de alto nivel (`WatchMovie`, `PlaceOrder`);
- internamente llama a los objetos necesarios del subsistema en el orden adecuado.

El cliente depende solo de la fachada. Aun así, el subsistema **no queda oculto del todo**: si se necesita algo no estándar, se puede acceder directamente a sus clases.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Facade** | `HomeTheaterFacade`, `OrderFacade` | Ofrece una interfaz sencilla y sabe a quién y en qué orden delegar el trabajo |
| **Subsystem classes** | `Projector`, `SoundSystem`, `MediaPlayer`, `Lights`, `InventoryService`, `PaymentGateway`, `ShippingService`, `NotificationService` | Realizan el trabajo real. **No saben nada de la fachada** |
| **Client** | clases de demo | Trabaja con el subsistema a través de la fachada |

## Esquema

```text
            Client
              │  WatchMovie("Origen")
              ▼
   ┌──────────────────────┐
   │  HomeTheaterFacade   │
   └──────────────────────┘
     │       │       │       │
     ▼       ▼       ▼       ▼
 Projector Sound  MediaPlayer Lights      ← subsistema
            System
```

---

## Ejemplo 1. Cine en casa (`HomeTheater/`)

**Subsystem**: las clases de la carpeta `Subsystems/` (`Projector`, `SoundSystem`, `MediaPlayer`, `Lights`). Cada una tiene su propio conjunto de métodos de bajo nivel. No dependen ni unas de otras ni de la fachada.

**Facade**: `HomeTheaterFacade` recibe los objetos del subsistema por el constructor y los combina en escenarios:

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

`EndMovie()` lo apaga todo en orden inverso.

**Client**: llama a `theater.WatchMovie("Origen")`, una sola línea en lugar de diez. Al final de la demo se muestra que el acceso directo al subsistema (`new Lights().Dim(50)`) sigue siendo posible.

## Ejemplo 2. Realización de pedidos (`OrderPlacement/`)

**Tarea:** un proceso de negocio que afecta a varios servicios, con comprobaciones y reversión en caso de error.

**Subsystem**: `InventoryService` (existencias y reservas), `PaymentGateway` (cobros; las tarjetas que terminan en `0000` se rechazan), `ShippingService` (coste y creación del envío), `NotificationService` (correos).

**Facade**: `OrderFacade.PlaceOrder(OrderRequest)` encapsula todo el proceso:

```csharp
if (!inventory.IsAvailable(order.Product, order.Count))
    return new OrderResult(false, $"{order.Product}: sin existencias");

inventory.Reserve(order.Product, order.Count);

var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
if (!payments.Charge(order.CardNumber, total))
{
    inventory.Release(order.Product, order.Count);   // reversión
    return new OrderResult(false, "El pago fue rechazado");
}
```

La fachada no solo conoce el orden de las llamadas, sino también las **reglas de coordinación** de los subsistemas: por ejemplo, que hay que anular la reserva si el pago no se ha realizado. Los datos de entrada y salida van empaquetados en `record` sencillos (`OrderRequest`, `OrderResult`), para que el cliente no tenga que conocer los tipos del subsistema.

**Client**: la demo envía tres pedidos: uno correcto, otro con un producto agotado y otro con una tarjeta rechazada. El cliente solo ve el resultado.

En una aplicación real de ASP.NET Core, a una fachada así se la suele llamar «servicio de aplicación» (`OrderService`, `CheckoutService`), y el controlador depende solo de ella.

---

## Cuándo usarlo

- Se necesita una interfaz sencilla para un subsistema complejo o una biblioteca de terceros.
- Hay que reducir el acoplamiento del cliente con muchas clases.
- Hay que dividir el sistema en capas: la fachada es el punto de entrada a cada capa/módulo.

## Cuándo no hace falta

- El subsistema ya es sencillo: la fachada sería una capa superflua.
- Los clientes necesitan constantemente distintas funciones de bajo nivel: la fachada solo estorbaría.

## Ventajas y desventajas

**Ventajas**
- Aísla a los clientes de los componentes del subsistema.
- Reduce el acoplamiento: el subsistema se puede cambiar sin tocar a los clientes.
- Elimina la duplicación de escenarios.

**Desventajas**
- Riesgo de convertirse en un **«objeto dios»** que lo sabe todo y lo hace todo. Es mejor tener varias fachadas pequeñas por tareas.

## Relación con otros patrones

- **Adapter** adapta **una** interfaz existente a la que se necesita; **Facade** crea una interfaz **nueva y simplificada** para todo un subsistema.
- **Mediator** también centraliza la interacción, pero los objetos del subsistema **conocen** al mediador y se comunican a través de él; en Facade el subsistema no conoce la fachada y la relación va en un solo sentido.
- **Abstract Factory** se puede usar junto con Facade para ocultar cómo se crean los objetos del subsistema.
- La fachada a menudo se implementa como **Singleton**, ya que normalmente basta con una instancia.
- **Proxy** tiene la misma interfaz que el objeto; **Facade** tiene una interfaz propia.
