🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/FactoryMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/FactoryMethod) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/FactoryMethod) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/FactoryMethod) | **Español**

# Factory Method (Método fábrica)

**Tipo:** patrón creacional.

**Factory Method** define en la clase base un método para crear un objeto, pero permite que **las subclases decidan qué clase concreta crear**. El código que usa el objeto depende solo de su interfaz, no de la clase concreta.

---

## Qué problema resuelve

Al principio, el servicio de notificaciones solo sabía enviar SMS, y el código era así:

```csharp
var sms = new SmsNotification();
sms.Notify(message);
```

Después hicieron falta el correo electrónico y las notificaciones push. Si se sigue por el mismo camino, aparecen `switch`/`if` con un `new` para cada tipo en todos los lugares donde se envían notificaciones. El código queda rígidamente ligado a las clases concretas, y añadir un canal nuevo exige cambios en todo el proyecto.

Lo mismo ocurre con los abonos de un gimnasio: «gimnasio», «gimnasio + piscina», «entrenamiento personal». Si el código cliente llama él mismo a `new GymMembership()`, depende de todos los tipos de abono.

## Solución

Sustituir la llamada directa a `new` por la llamada a un **método fábrica** especial. El método se declara en la clase base (el creador) y devuelve la **interfaz del producto**, y las subclases creadoras lo sobrescriben para devolver productos concretos.

Importante: el creador suele tener además **su propia lógica**, que trabaja con el producto a través de la interfaz sin saber qué producto concreto se ha creado (en el ejemplo de las notificaciones, el método `Notify`).

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Product** | `INotificationService`, `IMembership` | Interfaz común de los objetos creados |
| **ConcreteProduct** | `SmsNotification`, `UmailNotification`, `PushNotification`, `GymMembership`, `GymPoolMembership`, `PersonalTrainingMembership` | Implementaciones concretas del producto |
| **Creator** | `NotificationFactory`, `MembershipFactory` | Declara el método fábrica; puede contener lógica que trabaja con el producto |
| **ConcreteCreator** | `SmsNotificator`, `UmailNotificator`, `PushNotificator`, `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` | Sobrescribe el método fábrica y devuelve un producto concreto |
| **Client** | `DemondtrsteFactoryMethod` | Trabaja con los creadores y los productos |

## Esquema

```text
 ┌───────────────────────────────────────────┐          ┌─────────────────────────────┐
 │ «abstract» NotificationFactory (Creator)  │          │ «interface»                 │
 │ # CreateNotification(): INotificationService ──────►│ INotificationService        │
 │ + Notify(message):                        │  crea    │ Notify(message)             │
 │     var s = CreateNotification();         │          └─────────────────────────────┘
 │     s.Notify(message);                    │                ▲        ▲        ▲
 └───────────────────────────────────────────┘                │        │        │
        ▲                ▲                ▲          SmsNotification  Umail   Push
        │                │                │                          Notification Notification
  SmsNotificator   UmailNotificator  PushNotificator
  Create… => new SmsNotification()  ...
```

---

## Ejemplo 1. Sistema de notificaciones (`NotifySystem/`): creador con lógica propia

**Product**: `INotificationService` con el método `Notify(string message)`.

**ConcreteProduct** (carpeta `NotifiationServices/`): `SmsNotification`, `UmailNotification`, `PushNotification`; cada uno «envía» el mensaje por su propio canal.

**Creator**: la clase abstracta `NotificationFactory` (carpeta `Factories/`):

```csharp
public abstract class NotificationFactory
{
    protected abstract INotificationService CreateNotification();   // método fábrica

    public void Notify(string message)                               // lógica del creador
    {
        var notificationService = CreateNotification();
        notificationService.Notify(message);
    }
}
```

Esta es la forma clásica del patrón: el método `Notify` se implementa **una sola vez** en la clase base y trabaja con el producto solo a través de la interfaz. El método fábrica es `protected`: el cliente no crea el producto por sí mismo, simplemente llama a `Notify`.

**ConcreteCreator**: `SmsNotificator`, `UmailNotificator` y `PushNotificator` sobrescriben únicamente el método fábrica:

```csharp
internal class SmsNotificator : NotificationFactory
{
    protected override INotificationService CreateNotification() => new SmsNotification();
}
```

**Client**: `DemondtrsteFactoryMethod.DemonstrateNotify()` crea el creador necesario y llama a `Notify`. Para añadir, por ejemplo, Telegram, bastan dos clases, `TelegramNotification` y `TelegramNotificator`; el código existente no cambia.

## Ejemplo 2. Abonos de gimnasio (`Fitnes/`): fábricas de productos

**Product**: `IMembership` (nombre, descripción, precio).

**ConcreteProduct** (carpeta `Memberships/`):
- `GymMembership`: gimnasio, 50;
- `GymPoolMembership`: gimnasio + piscina, 75;
- `PersonalTrainingMembership`: entrenamiento personal, 100.

**Creator**: la clase abstracta `MembershipFactory` con un método fábrica público:

```csharp
internal abstract class MembershipFactory
{
    public abstract IMembership CreateMembership();
}
```

**ConcreteCreator** (carpeta `Factories/`): `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory`; cada una devuelve su propio abono.

**Client**: `DemondtrsteFactoryMethod.DemonstrateFitnes()` elige la fábrica necesaria, obtiene el abono mediante `CreateMembership()` y a partir de ahí trabaja con él como `IMembership`.

Aquí el creador no tiene lógica propia: es una forma simplificada del patrón que se usa a menudo en la práctica. La fábrica (elegida, por ejemplo, según la configuración o la elección del usuario) se pasa al código que crea los objetos sin conocer sus clases.

---

## Cuándo usarlo

- No se sabe de antemano de qué clases habrá que crear objetos.
- Hay que permitir ampliar los objetos creados mediante herencia (frameworks, bibliotecas).
- La clase base contiene un algoritmo común y las subclases determinan el producto concreto que utiliza.
- Hay que centralizar la creación de objetos para poder reutilizarlos si es necesario (caché, pool).

## Cuándo no hace falta

- Hay un único producto que no va a cambiar: un simple `new` es más claro.
- Hay que crear **familias** de objetos coherentes: eso es Abstract Factory.

## Ventajas y desventajas

**Ventajas**
- Libera al código de la dependencia de las clases concretas de los productos.
- El código de creación de productos está en un solo lugar (SRP).
- Los productos nuevos se añaden sin modificar el código existente (Open/Closed).

**Desventajas**
- Cada producto necesita su propia subclase creadora, y las jerarquías crecen en paralelo.

## Relación con otros patrones

- **Abstract Factory** suele implementarse con un conjunto de métodos fábrica, pero crea una **familia** de productos y se pasa como objeto.
- **Template Method**: el método fábrica suele ser un paso de un método plantilla; `NotificationFactory.Notify()` es un método plantilla que llama al método fábrica `CreateNotification()`.
- **Prototype** no requiere subclases creadoras, pero sí inicializar los prototipos. Factory Method, al revés.
- **Iterator**: `GetEnumerator()` en las colecciones de .NET es un método fábrica que devuelve un iterador.
- Factory Method suele evolucionar hacia **Builder** o **Abstract Factory** cuando la creación se complica.
