# Instrucțiuni pentru Aplicarea Migrațiilor

## Problema
Eroarea `Invalid column name 'IsDeleted'` apare pentru că baza de date nu are coloanele necesare adăugate prin migrații.

## Soluție 1: Rulare Script SQL (RECOMANDAT - Cel mai rapid)

1. **Deschide SQL Server Management Studio (SSMS)**
2. **Conectează-te la serverul SQL Server** (localhost sau serverul tău)
3. **Deschide fișierul** `ApplyAllMigrations.sql` din folderul proiectului
4. **Rulează scriptul** (F5 sau butonul Execute)

Scriptul va adăuga automat toate coloanele necesare:
- `IsDeleted` la tabelele `Categorie`, `Carte`, `Autor`
- `id_categorie`, `id_serie`, `nr_volum` la tabela `Carte`
- Foreign keys necesare

## Soluție 2: Aplicare Migrații prin Entity Framework

În **Visual Studio**, deschide **Package Manager Console**:

```
Tools → NuGet Package Manager → Package Manager Console
```

Apoi rulează:

```powershell
Update-Database -ProjectName Repository_CodeFirst -ConfigurationTypeName Repository_CodeFirst.Migrations.Configuration
```

## Soluție 3: Rulare Script SQL din Command Line

Dacă ai `sqlcmd` instalat:

```powershell
sqlcmd -S localhost -d eBooks -U sa -P administrator12345 -i "d:\ProiectPPAW\ApplyAllMigrations.sql"
```

## Verificare

După aplicarea migrațiilor, verifică că coloanele există:

```sql
USE eBooks;
GO

SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Carte', 'Categorie', 'Autor')
AND COLUMN_NAME IN ('IsDeleted', 'id_categorie', 'id_serie', 'nr_volum')
ORDER BY TABLE_NAME, COLUMN_NAME;
```

Ar trebui să vezi toate coloanele listate.

## După Aplicare

După aplicarea migrațiilor, aplicația ar trebui să funcționeze fără erori SQL.
