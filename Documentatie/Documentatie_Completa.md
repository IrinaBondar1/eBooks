# Documentație proiect – eBooks (MVC + Web API + EF)

## Cuprins

- **1. Proiectare**
  - 1.1 Paradigme utilizate (MVC, API, ORM Code First, ORM Database First etc.)
  - 1.2 De ce au fost alese?
  - 1.3 Arhitectura aplicației (modulele care interacționează)
- **2. Implementare**
  - 2.1 Business layer (Nivelul Services) – explicat
  - 2.2 Librării suplimentare utilizate
  - 2.3 Secțiuni de cod / abordări deosebite
- **3. Utilizare**
  - 3.1 Pașii de instalare
    - 3.1.1 Instalare și configurare pentru programator
    - 3.1.2 Instalare și configurare la beneficiar
  - 3.2 Mod de utilizare

---

## 1. Proiectare

### 1.1 Paradigme utilizate (MVC, API, ORM Code First, ORM Database First etc.)

Aplicația **eBooks** este construită în ecosistemul **ASP.NET (.NET Framework 4.7.2)** și implementează mai multe paradigme/pattern-uri, pentru a separa responsabilitățile și a face proiectul ușor de extins:

- **MVC (Model–View–Controller)**: partea de interfață web este implementată în proiectul `eBooks_MVC`.
  - **Controllers**: `eBooks_MVC/Controllers/*Controller.cs` (ex. `HomeController`, `UtilizatorController`, `AdminController`)
  - **Views**: `eBooks_MVC/Views/*/*.cshtml` (Razor)
  - **Models (ViewModels)**: `eBooks_MVC/Models/*ViewModel.cs`

- **Web API (REST)**: aplicația expune și un API în proiectul `eBooks_API` prin controllere Web API (`ApiController`).
  - Exemple: `eBooks_API/Controllers/CartiController.cs`, `AutoriController.cs`
  - Rutare prin `eBooks_API/App_Start/WebApiConfig.cs` (prefix `api/…`)

- **ORM – Entity Framework 6 (Code First)**:
  - Entitățile sunt definite ca POCO în `Repository_CodeFirst/LibrarieModele/*.cs`
  - Contextul EF este `Repository_CodeFirst/eBooksContext.cs`
  - Evoluția schemei DB se face prin migrații în `Repository_CodeFirst/Migrations/`

- **ORM – Entity Framework 6 (Database First)**:
  - Proiectul conține și varianta DB First prin `Repository_DBFirst/eBooks.edmx` + clase generate în `Repository_DBFirst/`
  - Aceasta demonstrează și abordarea „pornești de la DB → generezi model”

- **Service Layer (Business layer)**:
  - Reguli de business și comportamente comune sunt grupate în `NivelServicii` (ex. `CarteService`, `AutorService`, `AccesService`)

- **Dependency Injection (IoC)**:
  - În `eBooks_API` este configurat **Autofac** în `eBooks_API/Infrastructure/ContainerConfigurer.cs`
  - În `eBooks_MVC` există un fișier de configurare DI, dar este lăsat dezactivat (comentat) și controller-ele folosesc instanțiere manuală / constructor fallback.

---

### 1.2 De ce au fost alese?

- **MVC** a fost ales pentru:
  - separarea clară UI / logică / date;
  - suport nativ pentru formulare, validare, views Razor, routing;
  - implementarea rapidă a ecranelor cerute (admin CRUD + utilizator).

- **Web API** a fost ales pentru:
  - expunerea resurselor (cărți, autori) în format JSON;
  - posibilitatea de integrare cu alte aplicații (mobile / SPA / terți);
  - separarea între interfață și acces programatic.

- **Entity Framework (ORM)** a fost ales pentru:
  - maparea obiectelor C# la tabele SQL fără a scrie SQL manual peste tot;
  - query-uri LINQ ușor de citit și întreținut;
  - migrații (Code First) pentru evoluția bazei de date în timp.

- **Code First** (EF) a fost ales pentru:
  - control asupra modelului de domeniu în cod;
  - versionare a modificărilor bazei de date prin migrații;
  - seed data (planuri Free/Premium/VIP).

- **Database First** (EF) apare în proiect pentru:
  - demonstrarea ambelor abordări (cerințe educaționale);
  - scenarii în care DB este deja existentă și modelul se generează.

- **Service Layer** a fost ales pentru:
  - logica de business să nu fie îngropată în controllere;
  - reuse între MVC și API;
  - cross-cutting concerns: caching, logging, invalidare cache.

- **Dependency Injection** a fost ales pentru:
  - decuplarea controllerelor de implementările concrete;
  - configurare centralizată a dependențelor;
  - testabilitate și mentenanță.

---

### 1.3 Arhitectura aplicației (modulele care interacționează)

Arhitectura este organizată pe straturi/proiecte:

- **Presentation Layer**
  - `eBooks_MVC` – UI web (Razor + Controllers MVC)
  - `eBooks_API` – API REST (Web API 2)

- **Business Layer**
  - `NivelServicii` – servicii de business (`CarteService`, `AutorService`, `AccesService`) + caching

- **Data Access Layer**
  - `NivelAccessDate_DBFirst` – *accessori* (Repository-like), cu operații CRUD și query-uri LINQ pe `eBooksContext`

- **Data / ORM Layer**
  - `Repository_CodeFirst` – EF Code First: `eBooksContext`, migrații
  - `Repository_DBFirst` – EF Database First: `.edmx` + clase generate

- **Model Layer**
  - `Repository_CodeFirst/LibrarieModele` – entitățile domeniului (Carte, Autor, Categorie, Utilizator, TipAbonament, IstoricCitire etc.)

#### Flux general (MVC)

1. Browser → Controller MVC (ex. `UtilizatorController`)
2. Controller → Service (ex. `AccesService`, `CarteService`) pentru reguli/caching
3. Service → Accessor (DAL) → `eBooksContext` (EF) → SQL Server
4. Datele se întorc în controller → sunt mapate în ViewModels → View Razor

#### Flux general (API)

1. Client (Postman/Browser) → Controller Web API
2. Controller API → (Accessor / Service) → EF → SQL Server
3. Răspuns JSON

---

## 2. Implementare

### 2.1 Business layer (Nivelul Services) – explicat

Business layer este implementat în proiectul **`NivelServicii`** și are rolul de a centraliza:

- regulile de business (specifice aplicației);
- caching și invalidarea cache-ului;
- logging (NLog);
- interfațare (prin `I*Service`) pentru decuplare și eventual DI.

#### 2.1.1 Servicii principale

- **`CarteService`** (`NivelServicii/CarteService.cs`, interfață `ICarteService.cs`)
  - `GetAll()`, `GetById()` – folosesc cache (`carti_all`, `carte_{id}`)
  - `Add/Update/Delete/SoftDelete` – persistă schimbări și invalidează cache-ul

- **`AutorService`** (`NivelServicii/AutorService.cs`, interfață `IAutorService.cs`)
  - același model: cache pe `autori_all` și `autor_{id}` + invalidare

- **`AccesService`** (`NivelServicii/AccesService.cs`, interfață `IAccesService.cs`)
  - logică de business specifică: acces în funcție de abonament:
    - Free: limită mică/lună + doar volumul 1 dintr-o serie
    - Premium: limită mai mare + acces serii complete (flag)
    - VIP: nelimitat + download permis
  - înregistrează acțiuni în `IstoricCitire` și incrementează `carti_citite_luna` la citire (non-VIP)

#### 2.1.2 Pagina principală folosește Services

Pagina principală (`eBooks_MVC/Controllers/HomeController.cs`, `Views/Home/Index.cshtml`) afișează statistici și liste, folosind direct serviciile:

- `TotalCarti`, `CartiRecente` din `carteService.GetAll()`
- `TotalAutori`, `AutoriPopulari` din `autorService.GetAll()`
- `CartiPopulare` calculată pe baza istoricului + `carteService.GetById()`

Acest lucru demonstrează integrarea clară „UI → Services → Data”.

---

### 2.2 Librării suplimentare utilizate

Librăriile sunt gestionate prin `packages.config` (NuGet). Principalele:

- **Entity Framework 6.5.1** (`EntityFramework`) – ORM (Code First + DB First)
- **Autofac** (`Autofac`, `Autofac.WebApi`, `Autofac.WebApi2`) – DI container (folosit în `eBooks_API`)
- **NLog** (`NLog`, `NLog.Web`, `NLog.Config` etc.) – logging (folosit în Services, config în `eBooks_API/NLog.config`)
- **Newtonsoft.Json** – serializare JSON (folosit inclusiv în cache `MemoryCacheService`)
- **AutoMapper** – mapări între entități și DTO/ViewModel (proiectul are folder `eBooks_API/Mapping/`)
- **Bootstrap / jQuery / Validation** – UI & validare client-side (MVC)

---

### 2.3 Secțiuni de cod / abordări deosebite

#### 2.3.1 Soft Delete (ștergere logică)

În loc de ștergere fizică din DB, entitățile au câmp `IsDeleted` și sunt filtrate în listări:

- modele: `Repository_CodeFirst/LibrarieModele/Carte.cs`, `Categorie.cs`, `Autor.cs` (și altele)
- DAL: `NivelAccessDate_DBFirst/*Accessor.cs` conțin metode `SoftDelete`
- UI admin: `eBooks_MVC/Controllers/AdminController.cs` folosește `SoftDelete` pentru Categorie și Carte

Avantaje:
- păstrează istoricul și integritatea datelor;
- evită probleme cu FK și rapoarte.

#### 2.3.2 Cache la nivel de Services

Cache-ul este abstractizat prin `ICache` și implementat prin `MemoryCacheService`:

- `NivelServicii/Cache/ICache.cs`
- `NivelServicii/Cache/MemoryCacheService.cs` (serializează obiectele în JSON pentru a evita problemele clasice de serializare EF)

Serviciile (`CarteService`, `AutorService`) folosesc cache la `GetAll/GetById` și invalidează cache-ul după modificări.

#### 2.3.3 Logging cu NLog

Serviciile loghează:
- cache hit/miss;
- validări și blocări de acces (ex. limită lunară);
- erori la scriere în DB.

Config principal: `eBooks_API/NLog.config`.

#### 2.3.4 Dependency Injection

În `eBooks_API` dependențele sunt înregistrate prin Autofac:

- `eBooks_API/Infrastructure/ContainerConfigurer.cs`:
  - `eBooksContext` ca `IeBooksContext`
  - `CarteService` / `AutorService`
  - `MemoryCacheService` ca `ICache`

În `eBooks_MVC` există un fișier `App_Start/ContainerConfigurer.cs` pregătit pentru DI, dar lăsat comentat (controller-ele au constructor fallback).

#### 2.3.5 Reguli de acces (funcționalitatea principală)

Funcționalitatea principală (citit/descărcat/limită) este implementată server-side:

- `eBooks_MVC/Controllers/UtilizatorController.cs`:
  - listează cărți filtrate după `AccesService.PoateAccesaCarte`
  - verifică accesul și la `DetaliiCarte`, și la acțiunile `POST` (nu doar în UI)
  - `CitesteCarte` / `DownloadCarte` înregistrează în istoric
- `NivelServicii/AccesService.cs` conține regulile Free/Premium/VIP.

---

## 3. Utilizare

### 3.1 Pașii de instalare

#### 3.1.1 Instalare și configurare pentru programator

**Prerechizite:**
- Windows 10/11
- Visual Studio (recomandat 2019/2022) cu workload-uri pentru **ASP.NET** și **.NET Framework**
- **.NET Framework 4.7.2** (target framework)
- SQL Server (LocalDB / SQL Server Express / SQL Server Developer) + opțional SSMS

**Pași:**

1. **Deschide soluția/proiectul**
   - Deschide `eBooks_MVC/eBooks_MVC.csproj` (sau soluția dacă ai `.sln`)

2. **Restore NuGet Packages**
   - Visual Studio: Right-click pe Solution → **Restore NuGet Packages**

3. **Configurează baza de date**
   - Connection string este în:
     - `eBooks_MVC/Web.config`
     - `eBooks_API/Web.config`
   - Exemplu (default în proiect):
     - server: `localhost`
     - database: `eBooks`
     - user: `sa`
     - password: `administrator12345`

4. **Creează DB și aplică migrațiile**
   - Varianta recomandată (rapid): rulează `ApplyAllMigrations.sql` în SSMS (vezi `INSTRUCTIUNI_APLICARE_MIGRATII.md`)
   - Alternativ: din Package Manager Console:
     - `Update-Database -ProjectName Repository_CodeFirst -ConfigurationTypeName Repository_CodeFirst.Migrations.Configuration`

5. **Rulează aplicația**
   - Set Startup Project: `eBooks_MVC`
   - Apasă **F5**

**Dacă apare eroarea** `Invalid column name 'IsDeleted'`:
- aplică migrațiile (scriptul `ApplyAllMigrations.sql` este soluția recomandată).

#### 3.1.2 Instalare și configurare la beneficiar (deployment)

Într-un scenariu de beneficiar (utilizator final / instituție) aplicația se livrează ca site IIS.

**Prerechizite beneficiar:**
- Windows Server / Windows 10/11
- IIS instalat (cu ASP.NET 4.x)
- .NET Framework 4.7.2
- SQL Server (instanță locală sau remote)

**Pași:**

1. **Instalare DB**
   - Creează baza de date `eBooks` pe serverul SQL.
   - Rulează `ApplyAllMigrations.sql` pentru a crea/actualiza schema (inclusiv `IsDeleted`, FK, câmpurile pentru serie/categorie).

2. **Publicare aplicație**
   - Din Visual Studio → Publish (Folder / IIS)
   - Copiază artefactele pe server.

3. **Configurare IIS**
   - Creează un Website sau Application în IIS
   - Application Pool: .NET CLR v4.0, Integrated
   - Setează path către folderul publicat.

4. **Configurare `Web.config`**
   - Actualizează `<connectionStrings>` să pointeze către SQL Server-ul beneficiarului.

5. **Verificare**
   - Accesează URL-ul (ex. `http://server/eBooks`)
   - Testează Register/Login și accesul la pagini.

Notă: în proiect parolele sunt stocate în clar (pentru demo). Pentru producție, ar trebui hashing.

---

### 3.2 Mod de utilizare (scenarii principale)

#### 3.2.1 Pagina principală (Home)
- URL: `/Home/Index` (sau `/`)
- Afișează statistici: total cărți, autori, categorii, utilizatori și liste (cărți recente/populare).

#### 3.2.2 Planuri de abonament (secțiunea utilizator)
- URL: `/Utilizator`
- Afișează planurile **Free / Premium / VIP** și funcționalitățile:
  - limită cărți/lună
  - acces serii complete
  - descărcare (VIP)

#### 3.2.3 Autentificare
- Register: `/Account/Register` (utilizatorii noi pornesc Free)
- Login: `/Account/Login`
- Logout: `/Account/Logout`

#### 3.2.4 Cărți disponibile (filtrate pe plan)
- URL: `/Utilizator/Carti`
- Aplicația afișează doar cărțile la care utilizatorul are voie (reguli în `AccesService`).

#### 3.2.5 Detalii carte + acțiuni (funcționalitatea principală)
- URL: `/Utilizator/DetaliiCarte/{id}`
- Buton **Citește**:
  - POST: `/Utilizator/CitesteCarte/{id}`
  - înregistrează acțiunea și crește contorul lunar (non-VIP)
- Buton **Descarcă**:
  - POST: `/Utilizator/DownloadCarte/{id}`
  - disponibil doar dacă planul permite (VIP)

#### 3.2.6 Profil + upgrade plan
- URL: `/Utilizator/Profil`
- Arată planul curent și consumul lunar.
- Upgrade:
  - POST: `/Utilizator/UpgradeCont`
  - permite doar upgrade (Free → Premium → VIP).

#### 3.2.7 Istoric citiri
- URL: `/Utilizator/Istoric`
- Afișează toate acțiunile (citire/descărcare) cu dată și titlu.

#### 3.2.8 Admin (CRUD complet pe 2 entități)
Secțiunea de admin este implementată în MVC:

- Admin home: `/Admin/Index`
- CRUD Categorie: `/Admin/Categorie` + Create/Edit/Delete
- CRUD Carte: `/Admin/Carte` + Create/Edit/Delete
  - Delete este logic (SoftDelete) pentru ambele entități.

#### 3.2.9 API (testare rapidă)
În `eBooks_API` există endpoint-uri de tip:
- `GET /api/Carti`
- `GET /api/Carti/{id}`
- `GET /api/Autori`

Pot fi testate din browser (GET) sau cu Postman.

