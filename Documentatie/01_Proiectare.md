# Capitolul 1: Proiectare

## 1.1 Paradigme Utilizate

Aplicația **eBooks** este o platformă de gestiune a cărților electronice, construită pe baza tehnologiilor .NET și implementează următoarele paradigme de programare și design patterns:

### 1.1.1 Model-View-Controller (MVC)

Aplicația utilizează **ASP.NET MVC 5** ca framework principal pentru interfața web.

**Implementare în proiect:**
- **Models** (`eBooks_MVC/Models/`): Conține ViewModels precum `CarteViewModel`, `LoginViewModel`, `RegisterViewModel`
- **Views** (`eBooks_MVC/Views/`): Template-uri Razor (.cshtml) organizate pe controllere
- **Controllers** (`eBooks_MVC/Controllers/`): Controllere care gestionează logica de prezentare

```
eBooks_MVC/
├── Controllers/
│   ├── HomeController.cs
│   ├── CartiController.cs
│   ├── AutoriController.cs
│   ├── UtilizatorController.cs
│   ├── AccountController.cs
│   └── AdminController.cs
├── Models/
│   ├── CarteViewModel.cs
│   ├── LoginViewModel.cs
│   └── RegisterViewModel.cs
└── Views/
    ├── Home/
    ├── Carti/
    ├── Autori/
    ├── Account/
    └── Shared/
```

### 1.1.2 Web API (REST)

Aplicația expune servicii REST prin **ASP.NET Web API 2** pentru comunicare programatică.

**Implementare în proiect:**
- **API Controllers** (`eBooks_API/Controllers/`): Endpoint-uri RESTful pentru resurse
- **Rutare**: Configurată în `WebApiConfig.cs` cu prefix `/api/`

```
eBooks_API/
├── Controllers/
│   ├── CartiController.cs      // GET /api/Carti, GET /api/Carti/{id}
│   ├── AutoriController.cs     // GET /api/Autori
│   └── ValuesController.cs
└── App_Start/
    └── WebApiConfig.cs
```

**Exemple de endpoint-uri:**
| Metodă | Endpoint | Descriere |
|--------|----------|-----------|
| GET | `/api/Carti` | Returnează toate cărțile |
| GET | `/api/Carti/{id}` | Returnează o carte după ID |
| GET | `/api/Autori` | Returnează toți autorii |

### 1.1.3 ORM Code First (Entity Framework)

Aplicația utilizează **Entity Framework 6** cu abordarea **Code First** pentru definirea și evoluția schemei bazei de date.

**Implementare în proiect:**
- **Modele de domeniu** (`Repository_CodeFirst/LibrarieModele/`): Clase POCO cu Data Annotations
- **Context** (`Repository_CodeFirst/eBooksContext.cs`): DbContext cu DbSet-uri
- **Migrații** (`Repository_CodeFirst/Migrations/`): Peste 10 migrații pentru evoluția schemei

**Exemplu model Code First:**
```csharp
[Table("Carte")]
[Serializable] 
public class Carte
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_carte { get; set; }

    [Required]
    [MaxLength(150)]
    public string titlu { get; set; }

    [ForeignKey("Autor")]
    public int id_autor { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual Autor Autor { get; set; }
}
```

**Migrații disponibile:**
- `migrare1_25_10_2025` - Schema inițială
- `SeedInitialData_2025_10_25` - Date inițiale
- `AdaugaTabeleNoi_2025_10_25` - Tabele noi
- `AddIsDeletedToAutor` - Soft Delete pentru Autor
- `AddSerieCategorieToCarte` - Relații Serie/Categorie

### 1.1.4 ORM Database First (Entity Framework)

În paralel, aplicația demonstrează și abordarea **Database First** pentru scenarii în care baza de date preexistă.

**Implementare în proiect:**
- **EDMX Model** (`Repository_DBFirst/eBooks.edmx`): Model generat din schema existentă
- **Entități generate** (`Repository_DBFirst/`): Clase generate automat prin T4 templates

**Entități mapate:**
- `Autor`, `Carte`, `Categorie`, `Serie`
- `Utilizator`, `TipAbonament`, `IstoricCitire`

### 1.1.5 Dependency Injection (IoC)

Aplicația implementează **Inversion of Control** folosind containerul **Autofac**.

**Implementare în proiect:**
- **API**: `eBooks_API/Infrastructure/ContainerConfigurer.cs` - DI activ
- **MVC**: `eBooks_MVC/App_Start/ContainerConfigurer.cs` - DI pregătit

**Configurare Autofac (API):**
```csharp
public static void ConfigureContainer()
{
    var builder = new ContainerBuilder();

    builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

    builder.RegisterType<eBooksContext>()
           .As<IeBooksContext>()
           .InstancePerLifetimeScope();

    builder.RegisterType<CarteService>()
           .As<ICarteService>()
           .InstancePerLifetimeScope();

    builder.RegisterType<MemoryCacheService>()
           .As<ICache>()
           .SingleInstance();

    var container = builder.Build();
    config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
}
```

**Lifecycle-uri disponibile:**
| Pattern | Autofac | Descriere |
|---------|---------|-----------|
| Singleton | `SingleInstance()` | O instanță pe aplicație |
| Scoped | `InstancePerLifetimeScope()` | O instanță pe request |
| Transient | `InstancePerDependency()` | Instanță nouă la fiecare rezolvare |

---

## 1.2 Justificarea Alegerilor

### De ce MVC?

1. **Separarea responsabilităților**: Permite dezvoltarea independentă a UI, logicii de business și accesului la date
2. **Testabilitate**: Controllerele pot fi testate unitar fără a depinde de UI
3. **Maturitate**: Framework stabil cu documentație extensivă și comunitate activă
4. **Suport Razor**: Sintaxă elegantă pentru template-uri HTML cu C#

### De ce Web API?

1. **Interoperabilitate**: Permite consumarea datelor de către aplicații mobile, SPA, sau terți
2. **Standardizare**: Respectă convenții REST pentru predicibilitate
3. **Scalabilitate**: API-ul poate fi scalat independent de interfața web
4. **Separare frontend/backend**: Posibilitatea de a avea aplicații client separate

### De ce Code First?

1. **Control total**: Dezvoltatorul definește schema în cod, nu în designer
2. **Versionare**: Migrațiile permit evoluția schemei cu tracking în Git
3. **Domain-Driven Design**: Modelele de domeniu sunt centrale
4. **Agilitate**: Modificările rapide la model se reflectă automat în baza de date

### De ce și Database First?

1. **Scenariu academic**: Demonstrează ambele abordări ORM în același proiect
2. **Compatibilitate**: Util când baza de date este gestionată de un DBA extern
3. **Reverse Engineering**: Permite generarea rapidă de cod din schema existentă

### De ce Dependency Injection?

1. **Decuplare**: Componentele nu depind de implementări concrete
2. **Testabilitate**: Permite mock-uri și stub-uri în teste unitare
3. **Configurabilitate**: Schimbarea implementării fără modificarea codului client
4. **Single Responsibility**: Fiecare clasă se ocupă doar de responsabilitatea sa

---

## 1.3 Arhitectura Aplicației

### 1.3.1 Diagramă Arhitecturală

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           PRESENTATION LAYER                                 │
│  ┌─────────────────────────────┐    ┌─────────────────────────────────────┐ │
│  │        eBooks_MVC           │    │           eBooks_API                │ │
│  │    (ASP.NET MVC 5)          │    │       (ASP.NET Web API 2)           │ │
│  │                             │    │                                     │ │
│  │  Controllers:               │    │  Controllers:                       │ │
│  │  - HomeController           │    │  - CartiController                  │ │
│  │  - CartiController          │    │  - AutoriController                 │ │
│  │  - AutoriController         │    │  - ValuesController                 │ │
│  │  - UtilizatorController     │    │                                     │ │
│  │  - AccountController        │    │  Infrastructure:                    │ │
│  │  - AdminController          │    │  - ContainerConfigurer (Autofac)    │ │
│  │                             │    │                                     │ │
│  │  Views (Razor):             │    │  Mapping:                           │ │
│  │  - Home/, Carti/, Autori/   │    │  - AutoMapperConfig                 │ │
│  │  - Account/, Admin/         │    │                                     │ │
│  │  - Utilizator/, Shared/     │    │                                     │ │
│  │                             │    │                                     │ │
│  │  Models (ViewModels):       │    │  Models:                            │ │
│  │  - CarteViewModel           │    │  - DTOs                             │ │
│  │  - LoginViewModel           │    │                                     │ │
│  │  - RegisterViewModel        │    │                                     │ │
│  └─────────────────────────────┘    └─────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            SERVICE LAYER                                     │
│  ┌─────────────────────────────────────────────────────────────────────────┐│
│  │                          NivelServicii                                  ││
│  │                                                                         ││
│  │  Services:                        Interfaces:                           ││
│  │  - CarteService                   - ICarteService                       ││
│  │  - AutorService                   - IAutorService                       ││
│  │  - AccesService                   - IAccesService                       ││
│  │  - TestService                    - ITestService                        ││
│  │                                                                         ││
│  │  Cache:                           Funcționalități:                      ││
│  │  - ICache                         - Logică de business                  ││
│  │  - MemoryCacheService             - Validări                            ││
│  │                                   - Caching transparent                 ││
│  │                                   - Logging (NLog)                      ││
│  └─────────────────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         DATA ACCESS LAYER                                    │
│  ┌─────────────────────────────────────────────────────────────────────────┐│
│  │                      NivelAccessDate_DBFirst                            ││
│  │                                                                         ││
│  │  Accessors (Repository Pattern):                                        ││
│  │  - CarteAccessor         - GetAll(), GetById(), Add(), Update()         ││
│  │  - AutorAccessor         - Delete(), SoftDelete()                       ││
│  │  - CategorieAccessor                                                    ││
│  │  - UtilizatorAccessor    - GetByEmail(), GetByEmailAndPassword()        ││
│  │  - IstoricCitireAccessor                                                ││
│  │  - SerieAccessor                                                        ││
│  │  - TipAbonamentAccessor                                                 ││
│  │  - EdituraAccessor                                                      ││
│  └─────────────────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            DATA LAYER                                        │
│  ┌──────────────────────────────┐    ┌────────────────────────────────────┐ │
│  │    Repository_CodeFirst      │    │      Repository_DBFirst           │ │
│  │                              │    │                                    │ │
│  │  Context:                    │    │  EDMX:                             │ │
│  │  - eBooksContext             │    │  - eBooks.edmx                     │ │
│  │  - IeBooksContext            │    │  - Model1.Context.cs               │ │
│  │                              │    │                                    │ │
│  │  Migrations:                 │    │  Generated Entities:               │ │
│  │  - Configuration.cs          │    │  - Autor.cs                        │ │
│  │  - 10+ migrații              │    │  - Carte.cs                        │ │
│  │                              │    │  - Categorie.cs                    │ │
│  │  Funcționalități:            │    │  - Utilizator.cs                   │ │
│  │  - Seed Data                 │    │  - IstoricCitire.cs                │ │
│  │  - Schema Evolution          │    │  - TipAbonament.cs                 │ │
│  │  - Lazy Loading config       │    │  - Serie.cs                        │ │
│  └──────────────────────────────┘    └────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            MODEL LAYER                                       │
│  ┌─────────────────────────────────────────────────────────────────────────┐│
│  │                         LibrarieModele                                  ││
│  │                                                                         ││
│  │  Entități de domeniu (POCO):                                            ││
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐    ││
│  │  │   Carte     │  │   Autor     │  │  Categorie  │  │    Serie    │    ││
│  │  │  - id_carte │  │  - id_autor │  │-id_categorie│  │  - id_serie │    ││
│  │  │  - titlu    │  │ - nume_autor│  │ - denumire  │  │ - nume_serie│    ││
│  │  │  - descriere│  │ - IsDeleted │  │ - descriere │  │ - descriere │    ││
│  │  │  - id_autor │  └─────────────┘  │ - IsDeleted │  └─────────────┘    ││
│  │  │  - IsDeleted│                   └─────────────┘                      ││
│  │  └─────────────┘                                                        ││
│  │                                                                         ││
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────────────────┐  ││
│  │  │ Utilizator  │  │TipAbonament │  │       IstoricCitire             │  ││
│  │  │-id_utilizat │  │-id_tip_abon │  │  - id_istoric                   │  ││
│  │  │-nume_utiliz │  │ - denumire  │  │  - id_utilizator                │  ││
│  │  │  - email    │  │-limita_carti│  │  - id_carte                     │  ││
│  │  │  - parola   │  │-acces_serii │  │  - data_accesare                │  ││
│  │  │-id_tip_abon │  │-permite_desc│  │  - actiune                      │  ││
│  │  │-carti_citite│  └─────────────┘  └─────────────────────────────────┘  ││
│  │  └─────────────┘                                                        ││
│  └─────────────────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            DATABASE                                          │
│  ┌─────────────────────────────────────────────────────────────────────────┐│
│  │                        SQL Server (eBooks)                              ││
│  │                                                                         ││
│  │  Tabele:                                                                ││
│  │  - Autor, Carte, Categorie, Serie, Editura                              ││
│  │  - Utilizator, TipAbonament, IstoricCitire                              ││
│  │  - __MigrationHistory (Code First tracking)                             ││
│  └─────────────────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────────────────┘
```

### 1.3.2 Descrierea Modulelor

#### **Presentation Layer (Nivel de Prezentare)**

| Modul | Tehnologie | Responsabilitate |
|-------|------------|------------------|
| `eBooks_MVC` | ASP.NET MVC 5 | Interfață web pentru utilizatori finali |
| `eBooks_API` | ASP.NET Web API 2 | Servicii REST pentru integrări externe |

**Interacțiuni:**
- Primește cereri HTTP de la browser/client
- Orchestrează apeluri către Service Layer
- Transformă date în ViewModels/DTOs
- Returnează HTML (MVC) sau JSON (API)

#### **Service Layer (Nivel de Servicii)**

| Modul | Componente | Responsabilitate |
|-------|------------|------------------|
| `NivelServicii` | CarteService, AutorService, AccesService | Logică de business, validări, caching |

**Funcționalități implementate:**
- **Caching**: Memorare rezultate în `MemoryCacheService` pentru reducerea încărcării BD
- **Logging**: Urmărire operații cu NLog
- **Validări business**: Verificare acces utilizator la cărți bazat pe tip abonament
- **Soft Delete**: Ștergere logică cu flag `IsDeleted`

**Exemplu flux service:**
```csharp
public List<Carte> GetAll()
{
    string key = "carti_all";
    if (cache.IsSet(key))
    {
        logger.Info("CARTI : date luate din Cache");
        return cache.Get<List<Carte>>(key);
    }
    logger.Info("CARTI : date luate din BD");
    var carti = carteAccessor.GetAll();
    cache.Set(key, carti);
    return carti;
}
```

#### **Data Access Layer (Nivel de Acces la Date)**

| Modul | Pattern | Responsabilitate |
|-------|---------|------------------|
| `NivelAccessDate_DBFirst` | Repository Pattern | Abstractizare operații CRUD |

**Accessors disponibili:**
- `CarteAccessor` - CRUD pentru cărți
- `AutorAccessor` - CRUD pentru autori
- `UtilizatorAccessor` - CRUD + autentificare
- `IstoricCitireAccessor` - Tracking citiri
- `CategorieAccessor`, `SerieAccessor`, `TipAbonamentAccessor`, `EdituraAccessor`

#### **Data Layer (Nivel de Date)**

| Modul | Abordare ORM | Responsabilitate |
|-------|--------------|------------------|
| `Repository_CodeFirst` | Code First | Definire schema prin cod, migrații |
| `Repository_DBFirst` | Database First | Import schema existentă, EDMX |

**DbContext (Code First):**
```csharp
public class eBooksContext : DbContext, IeBooksContext
{
    public DbSet<Autor> Autori { get; set; }
    public DbSet<Carte> Carti { get; set; }
    public DbSet<TipAbonament> TipAbonamente { get; set; }
    public DbSet<Categorie> Categorii { get; set; }
    public DbSet<IstoricCitire> IstoricCitiri { get; set; }
    public DbSet<Serie> Serii { get; set; }
    public DbSet<Utilizator> Utilizatori { get; set; }
    public DbSet<Editura> Edituri { get; set; }
}
```

#### **Model Layer (Nivel de Modele)**

| Modul | Tip | Responsabilitate |
|-------|-----|------------------|
| `LibrarieModele` | Class Library | Definiție entități de domeniu (POCO) |

**Caracteristici modele:**
- Data Annotations pentru validare și mapare
- Navigation Properties pentru relații
- Flag `IsDeleted` pentru soft delete
- Atribut `[Serializable]` pentru caching

---

### 1.3.3 Fluxul de Date

```
┌──────────┐     HTTP      ┌──────────────┐    Service    ┌─────────────┐
│  Client  │ ───────────▶  │  Controller  │ ───────────▶  │   Service   │
│ (Browser)│               │  (MVC/API)   │               │ (Business)  │
└──────────┘               └──────────────┘               └─────────────┘
                                                                 │
                                                                 ▼
┌──────────┐     EF Core   ┌──────────────┐   Repository  ┌─────────────┐
│ SQL Server│ ◀─────────── │  DbContext   │ ◀─────────── │  Accessor   │
│ Database │               │  (EF 6)      │               │  (DAL)      │
└──────────┘               └──────────────┘               └─────────────┘
```

**Exemplu flux complet (Citire cărți):**

1. **Request**: `GET /Carti`
2. **Controller**: `CartiController.Index()` apelează `carteService.GetAll()`
3. **Service**: `CarteService.GetAll()` verifică cache, apelează `carteAccessor.GetAll()`
4. **Accessor**: `CarteAccessor.GetAll()` execută query LINQ pe `eBooksContext.Carti`
5. **EF**: Traduce LINQ în SQL și execută pe SQL Server
6. **Response**: JSON/HTML cu lista de cărți

---

### 1.3.4 Cross-Cutting Concerns

#### **Caching**
```
NivelServicii/Cache/
├── ICache.cs              // Interfață pentru caching
└── MemoryCacheService.cs  // Implementare cu MemoryCache
```

**Operații suportate:**
- `Get<T>(key)` - Recuperare din cache
- `Set(key, data, cacheTime)` - Salvare în cache
- `IsSet(key)` - Verificare existență
- `Remove(key)` - Ștergere explicită
- `RemoveByPattern(pattern)` - Invalidare pe pattern
- `Clear()` - Golire completă

#### **Logging**
Configurat prin **NLog** (`eBooks_API/NLog.config`):
- Log la consolă și fișiere
- Nivele: Debug, Info, Warn, Error
- Rotație automată fișiere

#### **Authentication**
- Forms Authentication (Web.config)
- Session pentru date utilizator
- Atribut `[Authorize]` pe controllere

---

## 1.4 Diagrama Entitate-Relație

```
┌─────────────────┐         ┌─────────────────┐         ┌─────────────────┐
│   TipAbonament  │         │   Utilizator    │         │ IstoricCitire   │
├─────────────────┤         ├─────────────────┤         ├─────────────────┤
│ PK id_tip_abon  │◀────────│ PK id_utilizator│────────▶│ PK id_istoric   │
│    denumire     │    1:N  │    nume_utiliz  │  1:N    │ FK id_utilizator│
│ limita_carti    │         │    email        │         │ FK id_carte     │
│ acces_serii     │         │    parola       │         │    data_accesare│
│ permite_desc    │         │ FK id_tip_abon  │         │    actiune      │
└─────────────────┘         │ carti_citite_luna│         └────────┬────────┘
                            └─────────────────┘                   │
                                                                  │ N:1
┌─────────────────┐         ┌─────────────────┐                   │
│     Autor       │         │     Carte       │◀──────────────────┘
├─────────────────┤         ├─────────────────┤
│ PK id_autor     │◀────────│ PK id_carte     │
│    nume_autor   │    1:N  │    titlu        │
│    IsDeleted    │         │    descriere    │
└─────────────────┘         │ FK id_autor     │
                            │ FK id_categorie │──────────┐
                            │ FK id_serie     │────────┐ │
                            │    nr_volum     │        │ │
                            │    IsDeleted    │        │ │
                            └─────────────────┘        │ │
                                                       │ │
┌─────────────────┐         ┌─────────────────┐        │ │
│     Serie       │         │   Categorie     │        │ │
├─────────────────┤         ├─────────────────┤        │ │
│ PK id_serie     │◀────────┤ PK id_categorie │◀───────┼─┘
│    nume_serie   │    1:N  │    denumire     │   1:N  │
│    descriere    │         │    descriere    │        │
└─────────────────┘         │    IsDeleted    │        │
        ▲                   └─────────────────┘        │
        │                                              │
        └──────────────────────────────────────────────┘
```

---

## 1.5 Structura Soluției Visual Studio

```
ProiectPPAW.sln
│
├── eBooks_MVC/                    # Aplicație Web MVC
│   ├── App_Start/                 # Configurare (Routing, Bundles, Autofac)
│   ├── Controllers/               # Controllere MVC
│   ├── Models/                    # ViewModels
│   ├── Views/                     # Razor Views
│   ├── Content/                   # CSS, imagini
│   ├── Scripts/                   # JavaScript
│   └── Web.config                 # Configurare aplicație
│
├── eBooks_API/                    # Aplicație Web API
│   ├── App_Start/                 # Configurare WebApi
│   ├── Controllers/               # Controllere API
│   ├── Infrastructure/            # DI Container (Autofac)
│   ├── Mapping/                   # AutoMapper profiles
│   └── Models/                    # DTOs
│
├── NivelServicii/                 # Service Layer
│   ├── Cache/                     # Caching infrastructure
│   ├── *Service.cs                # Implementări servicii
│   └── I*Service.cs               # Interfețe servicii
│
├── NivelAccessDate_DBFirst/       # Data Access Layer
│   └── *Accessor.cs               # Repository implementations
│
├── Repository_CodeFirst/          # Code First ORM
│   ├── LibrarieModele/            # Entități de domeniu
│   ├── Migrations/                # EF Migrations
│   ├── eBooksContext.cs           # DbContext
│   └── IeBooksContext.cs          # Interfață context
│
├── Repository_DBFirst/            # Database First ORM
│   ├── eBooks.edmx                # Entity Data Model
│   └── *.cs                       # Entități generate
│
├── Test_CodeFirst/                # Teste Code First
└── TestORM_SchemaFirst/           # Teste Database First
```

---

## 1.6 Concluzii

Arhitectura aplicației **eBooks** este construită pe principii solide de design software:

1. **Separarea în straturi** permite dezvoltarea și testarea independentă a componentelor
2. **Utilizarea ambelor abordări ORM** demonstrează flexibilitatea Entity Framework
3. **Pattern-ul Repository** abstractizează accesul la date și facilitează testarea
4. **Dependency Injection** reduce cuplarea și îmbunătățește mentenabilitatea
5. **Caching la nivel de serviciu** optimizează performanța fără a complica DAL-ul
6. **API REST separat** permite integrări externe și dezvoltare frontend decuplată

Aceste alegeri arhitecturale asigură scalabilitatea, mentenabilitatea și testabilitatea aplicației pe termen lung.
