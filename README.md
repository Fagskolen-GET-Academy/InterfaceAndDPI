# Forelesning: Interface og Dependency Injection i C#

## Læringsmål

Etter denne forelesningen skal studentene kunne:

- Forklare hva et interface er og hvorfor vi bruker det
- Lage et interface i C#
- Implementere et interface i en klasse
- Forklare hva dependency injection er
- Bruke constructor injection i egne klasser

---

## Ordliste

| Begrep | Forklaring |
|---|---|
| **Interface** | En kontrakt som sier hvilke metoder en klasse *må* ha |
| **Implementere** | En klasse som oppfyller kravene i et interface |
| **Kontrakt** | En avtale om hva som skal være tilgjengelig — ikke hvordan det gjøres |
| **Abstraksjon** | Skjule detaljer og kun vise det som er nødvendig |
| **Dependency** | En avhengighet — noe en klasse trenger for å fungere |
| **Injection** | Å gi klassen det den trenger utenfra |
| **Constructor injection** | Å sende inn avhengigheten via konstruktøren |

---

## Del 1 — Interface

### Hva er et interface?

Et interface definerer *hva* en klasse skal kunne gjøre, ikke *hvordan*.

```csharp
// Kontrakten — alle som implementerer denne MÅ ha en Area()-metode
interface IShape
{
    double Area();
}
```

> Interface-navn starter alltid med stor `I` — det er en konvensjon i C#.

### Implementer interfacet i to klasser

```csharp
class Circle : IShape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double Area()
    {
        return Math.PI * Radius * Radius;
    }
}

class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double Area()
    {
        return Width * Height;
    }
}
```

Begge klassene oppfyller kontrakten — de har begge en `Area()`-metode.

### Bruk

```csharp
// Vi kan jobbe med IShape — ikke Circle eller Rectangle direkte
IShape circle = new Circle(5);
IShape rectangle = new Rectangle(4, 6);

Console.WriteLine(circle.Area());    // 78.54...
Console.WriteLine(rectangle.Area()); // 24
```

### Lag en metode som tar inn et interface

```csharp
static void PrintArea(IShape shape)
{
    Console.WriteLine($"Areal: {shape.Area()}");
}

PrintArea(new Circle(5));       // Areal: 78.54...
PrintArea(new Rectangle(4, 6)); // Areal: 24
```

`PrintArea` bryr seg ikke om hvilken form det er — bare at den kan `Area()`.

### Viktige regler

- Et interface har **ingen kode** — bare metodesignaturer
- En klasse **må** implementere alle metodene i interfacet
- En klasse kan implementere **flere** interfaces
- Interface-navn starter alltid med **`I`**

---

## Del 2 — Bygg videre: Et mer praktisk eksempel

La oss se på et problem du kan møte i praksis.

### Problemet — uten interface

```csharp
class EmailService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

class NotificationManager
{
    EmailService service = new EmailService(); // ← tett koblet

    public void Notify(string message)
    {
        service.Send(message);
    }
}
```

**Spørsmål:** Hva skjer hvis vi vil bytte fra e-post til SMS?

Vi må endre koden inne i `NotificationManager`. Hvis vi bruker `EmailService` mange steder, må vi endre det *overalt*.

### Løsningen — interface

```csharp
interface IMessageService
{
    void Send(string message);
}

class EmailService : IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

class SmsService : IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}
```

```csharp
class NotificationManager
{
    private IMessageService _service;

    public NotificationManager(IMessageService service)
    {
        _service = service;
    }

    public void Notify(string message)
    {
        _service.Send(message);
    }
}
```

### Analogi

En strømkontakt er et interface:

- Den definerer formen og hva som forventes
- En støpsel fra Philips, Samsung eller Apple kan alle plugges inn
- Kontakten bryr seg ikke om *hvem* som lager støpselet — bare at den passer

---

## Del 3 — Dependency Injection


Selv med interface kan vi gjøre det feil:

```csharp
class NotificationManager
{
    private IMessageService _service;

    public NotificationManager()
    {
        _service = new EmailService(); // ← fortsatt låst til EmailService
    }
}
```

**Spørsmål:** Hvem bestemmer hvilken service som brukes?

`NotificationManager` bestemmer selv. Vil du bytte? Du må endre koden inne i klassen.

### Løsningen — Dependency Injection

I stedet for at klassen lager avhengigheten selv, **får den den utenfra via konstruktøren**:

```csharp
class NotificationManager
{
    private IMessageService _service;

    // Vi får service utenfra i stedet for å lage den selv
    public NotificationManager(IMessageService service)
    {
        _service = service;
    }

    public void Notify(string message)
    {
        _service.Send(message);
    }
}
```

### Analogi

Tenk på en kokk som trenger en kniv:

**Uten DI:**
```
Kokken lager sin egen kniv
→ Kokken er låst til den kniven
→ Vil du bytte kniv? Endre koden inne i kokken
```

**Med DI:**
```
Noen gir kokken en kniv utenfra
→ Kokken bryr seg ikke om hvem som lagde den
→ Vil du bytte kniv? Bare gi kokken en annen
```

### Bruk

```csharp
// Du bestemmer hvilken service som brukes her — ikke inne i NotificationManager
var manager = new NotificationManager(new EmailService());
manager.Notify("Hei!"); // Sending email: Hei!

// Bytt til SMS — ingen endringer i NotificationManager
var manager2 = new NotificationManager(new SmsService());
manager2.Notify("Hei!"); // Sending SMS: Hei!
```

### Legg til en ny service — uten å endre eksisterende kode

```csharp
class PushNotificationService : IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending push notification: {message}");
    }
}
```

```csharp
var manager3 = new NotificationManager(new PushNotificationService());
manager3.Notify("Hei!"); // Sending push notification: Hei!
```

`NotificationManager` trenger ingen endringer.

---

## Oppsummering

| | Uten interface + DI | Med interface + DI |
|---|---|---|
| Bytte implementasjon | Endre koden overalt | Bytt ved å sende inn noe annet |
| Legge til ny service | Endre eksisterende kode | Lag en ny klasse |
| Fleksibilitet | Lav | Høy |

**Interface i én setning:**
> Definer *hva* som skal gjøres — ikke *hvordan*.

**DI i én setning:**
> I stedet for at klassen lager sine egne avhengigheter, får den dem utenfra.

---

## Neste steg — prøv selv

Bygg videre på `IShape`-eksempelet:

- Lag et interface `IShapeService` med metoden `void PrintArea(IShape shape)`
- Lag klassen `ShapeService` som implementerer det
- Lag en klasse `ShapeManager` som får `IShapeService` inn via konstruktøren
- Test med `Circle` og `Rectangle`

---



## Del 4 — Dependency Inversion Principle (DIP)

DIP er ett av **SOLID**-prinsippene — en samling retningslinjer for god objektorientert kode.

> **Høynivå-moduler skal ikke avhenge av lavnivå-moduler. Begge skal avhenge av abstraksjon.**

Vi har allerede brukt DIP uten å nevne det.

### DIP brutt

```csharp
// NotificationManager (høynivå) avhenger direkte av EmailService (lavnivå)
class NotificationManager
{
    EmailService service = new EmailService(); // ← direkte avhengighet
}
```

### DIP fulgt

```csharp
// Begge avhenger av IMessageService (abstraksjon)
class NotificationManager
{
    IMessageService _service; // ← avhenger av interface, ikke konkret klasse
}
```

### Analogi

Du lodder ikke lampens kabel direkte til stikkontakten i veggen — du bruker en støpsel.

- **Stikkontakten** er interfacet — den definerer formen og hva som forventes
- **Støpselet** er implementasjonen — kan byttes ut med en annen lampe
- **Lampen** er høynivå-modulen — den bryr seg ikke om hva som er i veggen, bare at den får strøm

Lodder du kabelen direkte, kan du aldri bytte lampe uten å rive opp veggen.

### DI vs DIP — hva er forskjellen?

| | Hva det er |
|---|---|
| **DIP** | Et prinsipp — høynivå skal ikke avhenge av lavnivå |
| **DI** | En teknikk — send avhengigheter inn utenfra |

DI er *teknikken* vi bruker for å oppfylle DIP-*prinsippet*.

Med andre ord: vi bruker Dependency Injection for å oppnå Dependency Inversion.
