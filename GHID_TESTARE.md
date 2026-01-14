# Ghid de Testare - Aplicația eBooks MVC

## Pași pentru Lansarea Aplicației în Execuție

### 1. Verificări Preliminare

#### A. Verifică Baza de Date SQL Server

- Asigură-te că SQL Server este pornit
- Verifică connection string în `Web.config`:
  ```xml
  <connectionStrings>
    <add name="eBooks"
         connectionString="Data Source=localhost;Initial Catalog=eBooks;User ID=sa;Password=administrator12345;..." />
  </connecti
  ```
- Baza de date `eBooks` trebuie să existe
- Dacă nu există, creează-o sau aplică migrațiile

#### B. Verifică Dependențele

- Asigură-te că toate pachetele NuGet sunt restaurate
- În Visual Studio: Right-click pe soluție → **Restore NuGet Packages**

### 2. Lansarea Aplicației în Visual Studio

#### Opțiunea A: Rulare Directă din Visual Studio

1. **Deschide Proiectul**

   - Deschide `eBooks_MVC.csproj` în Visual Studio
   - SAU deschide soluția care conține proiectul

2. **Verifică Proiectul de Start**

   - Right-click pe `eBooks_MVC` în Solution Explorer
   - Selectează **Set as StartUp Project**

3. **Lansează Aplicația**

   - Apasă **F5** (Debug) sau **Ctrl+F5** (Run without debugging)
   - SAU click pe butonul **Start** (triunghi verde) din toolbar
   - SAU din meniu: **Debug** → **Start Debugging**

4. **Aplicația se va deschide în browser**
   - URL implicit: `http://localhost:port_number/`
   - Default route: `/Admin/Index` (Panou Admin)

#### Opțiunea B: Rulare cu IIS Express (Recomandat)

1. **Configurare IIS Express**

   - Visual Studio va folosi automat IIS Express
   - Portul va fi alocat automat (ex: `http://localhost:12345/`)

2. **Verifică Application Settings**
   - În `Project Properties` → **Web** tab
   - Verifică că **Use Local IIS Web Server** sau **Use IIS Express** este selectat

### 3. Verificarea Funcționalităților

#### A. Panou Admin (Pornire Default)

- URL: `http://localhost:port/Admin` sau `http://localhost:port/`
- Verifică:
  - ✅ Afișează panou cu linkuri către Categorii și Cărți

#### B. Secțiune Utilizator - Planuri

- URL: `http://localhost:port/Utilizator` sau click pe "Planuri" din meniu
- Verifică:
  - ✅ Afișează planurile Basic și Premium
  - ✅ Detalii: limită cărți/lună, acces serii, descărcare

#### C. Gestionare Categorii (Admin)

- URL: `http://localhost:port/Admin/Categorie`
- Testează:
  - ✅ Create - Adaugă categorie nouă
  - ✅ Read - Vezi lista categorii
  - ✅ Update - Editează o categorie
  - ✅ Delete - Ștergere logică (SoftDelete)

#### D. Gestionare Cărți (Admin)

- URL: `http://localhost:port/Admin/Carte`
- Testează:
  - ✅ Create - Adaugă carte nouă (cu dropdown pentru autori)
  - ✅ Read - Vezi lista cărți
  - ✅ Update - Editează o carte
  - ✅ Delete - Ștergere logică (SoftDelete)

#### E. Cărți Disponibile (Utilizator)

- URL: `http://localhost:port/Utilizator/Carti`
- Verifică:
  - ✅ Listează cărțile disponibile
  - ✅ Link către detalii carte

#### F. Profil Utilizator

- URL: `http://localhost:port/Utilizator/Profil/1`
- Verifică:
  - ✅ Afișează informații utilizator
  - ✅ Statistici: cărți citite vs limită

### 4. Testarea Bazei de Date

#### Verifică Tabelele

Rulează în SQL Server Management Studio:

```sql
USE eBooks;
GO
SELECT * FROM TipAbonament;
SELECT * FROM Categorie;
SELECT * FROM Carte;
SELECT * FROM Autor;
SELECT * FROM Utilizator;
```

#### Dacă Tabelele Nu Există

1. Aplică migrațiile manual:
   - În Package Manager Console:
   ```
   Update-Database -ProjectName Repository_CodeFirst
   ```
2. SAU rulează scriptul SQL:
   - `ApplyMigration.sql`

### 5. Debugging și Troubleshooting

#### Erori Comune

**Eroare: "Cannot attach the file ... as database"**

- Soluție: Verifică connection string în `Web.config`
- Verifică că SQL Server permite conexiuni

**Eroare: "Entity Framework migrations"**

- Soluție: Aplică migrațiile sau creează baza de date manual

**Eroare: "404 - Not Found"**

- Soluție: Verifică routing în `RouteConfig.cs`
- Verifică că controllerul și view-ul există

**Eroare: "Model binding" sau "View not found"**

- Soluție: Verifică că ViewModels sunt corecte
- Verifică că view-urile sunt în folder-ul corect

#### Verificare în Browser

- Deschide **Developer Tools** (F12)
- Tab **Console** - verifică erori JavaScript
- Tab **Network** - verifică request-urile HTTP

### 6. Testare Funcționalități CRUD

#### Test Complete Flow:

1. **Admin → Categorii**

   - Create: `Admin/CreateCategorie`
   - Verify: `Admin/Categorie` (vezi noua categorie)
   - Edit: `Admin/EditCategorie/1`
   - Delete: `Admin/DeleteCategorie/1` (soft delete)

2. **Admin → Cărți**

   - Create: `Admin/CreateCarte` (selectează autor din dropdown)
   - Verify: `Admin/Carte` (vezi noua carte)
   - Edit: `Admin/EditCarte/1`
   - Delete: `Admin/DeleteCarte/1` (soft delete)

3. **Utilizator → Planuri**
   - View: `Utilizator/Index` (vezi planurile)
   - View Books: `Utilizator/Carti`
   - View Details: `Utilizator/DetaliiCarte/1`
   - View Profile: `Utilizator/Profil/1`

### 7. Port și URL Configurare

Dacă vrei să schimbi portul:

1. Right-click pe proiect `eBooks_MVC`
2. **Properties** → **Web** tab
3. Schimbă **Project Url** (ex: `http://localhost:5000/`)
4. Click **Create Virtual Directory**

### 8. Rulare în Production Mode

Pentru Release build:

1. Change Configuration: **Debug** → **Release**
2. Build Solution: **Build** → **Rebuild Solution**
3. Publish: Right-click pe proiect → **Publish**

---

## Quick Start (Comandă Rapidă)

1. Deschide Visual Studio
2. File → Open → Project/Solution → `eBooks_MVC.csproj`
3. Apasă **F5**
4. Browser se deschide automat cu aplicația!

---

## Note Importante

- Aplicația folosește **Entity Framework Code First**
- Migrațiile sunt **disabled** automat (`AutomaticMigrationsEnabled = false`)
- Ștergerea este **logică** (soft delete) - setează `IsDeleted = true`
- Default route: `/Admin/Index` (panou admin)
- Baza de date trebuie să existe sau să fie creată prin migrații

---

**Succes la testare! 🚀**
