-- Script SQL pentru aplicarea migrației IsDeleted
-- Rulează acest script în SQL Server Management Studio

USE eBooks;
GO

-- Verifică și adaugă câmpul IsDeleted la tabela Categorie
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Categorie') 
               AND name = 'IsDeleted')
BEGIN
    ALTER TABLE dbo.Categorie
    ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT 'Câmpul IsDeleted a fost adăugat la tabela Categorie';
END
ELSE
BEGIN
    PRINT 'Câmpul IsDeleted există deja în tabela Categorie';
END
GO

-- Verifică și adaugă câmpul IsDeleted la tabela Carte
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Carte') 
               AND name = 'IsDeleted')
BEGIN
    ALTER TABLE dbo.Carte
    ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT 'Câmpul IsDeleted a fost adăugat la tabela Carte';
END
ELSE
BEGIN
    PRINT 'Câmpul IsDeleted există deja în tabela Carte';
END
GO

-- Verificare finală
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Categorie', 'Carte')
AND COLUMN_NAME = 'IsDeleted'
ORDER BY TABLE_NAME;
GO

PRINT 'Migrarea a fost aplicată cu succes!';
GO

