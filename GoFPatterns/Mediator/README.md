🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Mediator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Mediator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Mediator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Mediator) | **Español**

# Mediator (Mediador)

**Tipo:** patrón de comportamiento.

**Mediator** reduce el acoplamiento entre muchos objetos obligándolos a comunicarse **no directamente, sino a través de un objeto mediador**. Los objetos solo conocen al mediador, y toda la lógica de interacción se concentra en él.

---

## Qué problema resuelve

**Formulario de registro.** La casilla «Soy una empresa» muestra el campo «Nombre de la empresa». El botón «Registrarse» solo está activo si el email es correcto, la contraseña es larga, se han aceptado las condiciones y, en el caso de una empresa, se ha rellenado el nombre. Si los componentes se comunican directamente:

```text
 CheckBox(IsCompany) ──► TextBox(CompanyName)
        │                       │
        ▼                       ▼
     Button(Submit) ◄── TextBox(Email) ◄── TextBox(Password) ◄── CheckBox(Terms)
```

…cada componente conoce a un montón de otros y las conexiones forman una «telaraña». No se puede reutilizar `TextBox` en otro formulario, porque arrastra el conocimiento del botón y de las casillas. Cualquier regla nueva exige cambios en varias clases.

**Chat.** Si cada usuario guarda referencias a todos los demás, cada entrada/salida obliga a actualizar a todos, y el filtrado de spam y los mensajes privados acaban repartidos entre los usuarios.

## Solución

Sacar toda la lógica de interacción a un objeto aparte: el **mediador**. Los componentes:
- guardan una referencia **solo** al mediador;
- le comunican los eventos («me han cambiado», «envía un mensaje»);
- no saben quién reaccionará ni cómo.

El mediador conoce a todos los participantes y decide a quién y cómo notificar. Las relaciones «muchos a muchos» se convierten en «uno a muchos».

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Mediator** | `IChatMediator`, `IFormMediator` | Interfaz de comunicación de los componentes con el mediador |
| **ConcreteMediator** | `ChatRoomMediator`, `RegistrationDialog` | Conoce a todos los participantes y contiene la lógica de su interacción |
| **Colleague** | `ChatUser`, `FormComponent` | Clase base del participante; guarda una referencia al mediador |
| **ConcreteColleague** | `ChatUser`, `ChatBot`, `TextBox`, `CheckBox`, `Button` | Participantes concretos. Comunican eventos al mediador sin saber nada unos de otros |

## Esquema

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

## Ejemplo 1. Sala de chat (`ChatRoom/`)

**Mediator**: `IChatMediator`: `Join`, `Leave`, `SendMessage`, `SendPrivate`.

**ConcreteMediator**: `ChatRoomMediator` guarda un diccionario de participantes y contiene **todas las reglas** de comunicación:
- envío del mensaje a todos excepto al remitente;
- bloqueo de los mensajes con palabras prohibidas;
- mensajes privados por nombre y respuesta si el destinatario no está en la sala;
- avisos de entrada/salida.

```csharp
public void SendMessage(ChatUser sender, string message)
{
    if (_bannedWords.Any(w => message.Contains(w, StringComparison.OrdinalIgnoreCase)))
        return;   // la regla de moderación está solo aquí

    foreach (var user in _users.Values.Where(u => u != sender))
        user.Receive(sender.Name, message);
}
```

**Colleague**: `ChatUser` solo conoce a su mediador:

```csharp
public void Send(string message) => chat.SendMessage(this, message);
```

**ConcreteColleague**: `ChatBot` también es un participante, pero con su propia reacción: responde al comando `/time` con un mensaje privado. Los usuarios no saben nada del bot: es simplemente otro participante de la sala.

## Ejemplo 2. Formulario de registro (`RegistrationForm/`): el ejemplo clásico de GoF

**Colleague**: la clase abstracta `FormComponent` (nombre, `IsEnabled`, `IsVisible`, referencia al mediador).

**ConcreteColleague** (carpeta `Components/`): `TextBox`, `CheckBox`, `Button`. Ante una acción del usuario, cada uno se limita a comunicar el evento al mediador:

```csharp
public void Type(string text)
{
    Text = text;
    Mediator?.Notify(this, "changed");
}
```

Estos componentes son **universales**: se pueden usar en cualquier formulario y no saben nada de otros campos.

**ConcreteMediator**: `RegistrationDialog` crea los componentes, se asigna a sí mismo como su mediador e implementa en `Notify` las reglas del formulario:
- la casilla `IsCompany` muestra/oculta el campo `CompanyName`;
- tras cualquier cambio se recalcula si el botón `Submit` está disponible;
- pulsar `Submit` realiza el registro.

```csharp
var isValid = Email.Text.Contains('@')
              && Password.Text.Length >= 6
              && AcceptTerms.IsChecked
              && (!IsCompany.IsChecked || CompanyName.Text.Length > 0);
```

**Client**: la demo simula las acciones del usuario. Se ve cómo el botón se activa, luego vuelve a desactivarse (se marcó «empresa», pero no se introdujo el nombre) y después se activa de nuevo.

---

## Cuándo usarlo

- Muchos objetos están conectados de forma compleja y las conexiones son difíciles de entender y cambiar.
- Se quieren reutilizar componentes, pero dependen demasiado de otros.
- La lógica de interacción debe estar en un solo lugar y configurarse sin modificar los componentes.
- Ejemplos: diálogos y formularios en interfaces gráficas, chats, una torre de control (los aviones se comunican a través de la torre), coordinación de microservicios (orquestador).

## Cuándo no hace falta

- Hay pocos objetos y las conexiones son sencillas: el mediador sería un nivel superfluo.

## Ventajas y desventajas

**Ventajas**
- Elimina las dependencias directas entre componentes.
- La lógica de interacción está en un solo lugar (SRP).
- Los componentes son fáciles de reutilizar.
- Las reglas nuevas se añaden en el mediador, sin modificar los componentes.

**Desventajas**
- El mediador puede convertirse en un **«objeto dios»** que lo sabe y lo hace todo.

## Relación con otros patrones

- **Facade** también simplifica el trabajo con un grupo de objetos, pero el subsistema **no conoce** la fachada y la relación es unidireccional. Los participantes de Mediator **conocen** al mediador y se comunican a través de él.
- **Observer**: el mediador suele implementarse con eventos/suscripciones: los componentes publican eventos y el mediador está suscrito a ellos. La diferencia está en el objetivo: Observer es una suscripción dinámica «uno a muchos»; Mediator, una lógica de interacción centralizada.
- **Chain of Responsibility**, **Command**, **Mediator** y **Observer** son distintas formas de conectar emisores y receptores de peticiones.
- La biblioteca **MediatR** de .NET se llama así por el patrón: los manejadores no se conocen entre sí y las peticiones se envían a través de `IMediator`.
