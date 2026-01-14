-- Script SQL pentru aplicarea tuturor migrațiilor necesare
-- Rulează acest script în SQL Server Management Studio

USE eBooks;
GO

-- ============================================
-- 1. Adaugă IsDeleted la Categorie
-- ============================================
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

-- ============================================
-- 2. Adaugă IsDeleted la Carte
-- ============================================
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

-- ============================================
-- 3. Adaugă IsDeleted la Autor
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Autor') 
               AND name = 'IsDeleted')
BEGIN
    ALTER TABLE dbo.Autor
    ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT 'Câmpul IsDeleted a fost adăugat la tabela Autor';
END
ELSE
BEGIN
    PRINT 'Câmpul IsDeleted există deja în tabela Autor';
END
GO

-- ============================================
-- 4. Adaugă id_categorie la Carte
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Carte') 
               AND name = 'id_categorie')
BEGIN
    ALTER TABLE dbo.Carte
    ADD id_categorie INT NULL;
    PRINT 'Câmpul id_categorie a fost adăugat la tabela Carte';
END
ELSE
BEGIN
    PRINT 'Câmpul id_categorie există deja în tabela Carte';
END
GO

-- ============================================
-- 5. Adaugă id_serie la Carte
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Carte') 
               AND name = 'id_serie')
BEGIN
    ALTER TABLE dbo.Carte
    ADD id_serie INT NULL;
    PRINT 'Câmpul id_serie a fost adăugat la tabela Carte';
END
ELSE
BEGIN
    PRINT 'Câmpul id_serie există deja în tabela Carte';
END
GO

-- ============================================
-- 6. Adaugă nr_volum la Carte
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.Carte') 
               AND name = 'nr_volum')
BEGIN
    ALTER TABLE dbo.Carte
    ADD nr_volum INT NULL;
    PRINT 'Câmpul nr_volum a fost adăugat la tabela Carte';
END
ELSE
BEGIN
    PRINT 'Câmpul nr_volum există deja în tabela Carte';
END
GO

-- ============================================
-- 7. Adaugă Foreign Keys dacă nu există
-- ============================================

-- Foreign Key pentru id_categorie
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
               WHERE name = 'FK_Carte_Categorie')
BEGIN
    ALTER TABLE dbo.Carte
    ADD CONSTRAINT FK_Carte_Categorie 
    FOREIGN KEY (id_categorie) REFERENCES dbo.Categorie(id_categorie);
    PRINT 'Foreign Key FK_Carte_Categorie a fost adăugat';
END
ELSE
BEGIN
    PRINT 'Foreign Key FK_Carte_Categorie există deja';
END
GO

-- Foreign Key pentru id_serie
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
               WHERE name = 'FK_Carte_Serie')
BEGIN
    ALTER TABLE dbo.Carte
    ADD CONSTRAINT FK_Carte_Serie 
    FOREIGN KEY (id_serie) REFERENCES dbo.Serie(id_serie);
    PRINT 'Foreign Key FK_Carte_Serie a fost adăugat';
END
ELSE
BEGIN
    PRINT 'Foreign Key FK_Carte_Serie există deja';
END
GO

-- ============================================
-- 8. Verificare finală
-- ============================================
PRINT '';
PRINT '=== Verificare coloane adăugate ===';
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Categorie', 'Carte', 'Autor')
AND COLUMN_NAME IN ('IsDeleted', 'id_categorie', 'id_serie', 'nr_volum')
ORDER BY TABLE_NAME, COLUMN_NAME;
GO

PRINT '';
PRINT '=== Verificare Foreign Keys ===';
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumnName
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fc ON fk.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(fk.parent_object_id) = 'Carte'
AND fk.name IN ('FK_Carte_Categorie', 'FK_Carte_Serie');
GO

PRINT '';
PRINT '========================================';
PRINT 'Migrarea a fost aplicată cu succes!';
PRINT '========================================';
GO
