# Ghid Pas cu Pas - Aplicare Migrații SQL

## Problema
Când încerci să te înregistrezi sau să accesezi aplicația, apare eroarea:
```
SqlException: Invalid column name 'IsDeleted'.
Invalid column name 'id_categorie'.
Invalid column name 'id_serie'.
Invalid column name 'nr_volum'.
```

Aceasta înseamnă că baza de date nu are coloanele necesare. Trebuie să le adăugăm manual.

---

## Soluție: Rulare Script SQL

### Pasul 1: Deschide SQL Server Management Studio (SSMS)

1. **Caută în Windows** "SQL Server Management Studio" sau "SSMS"
2. **Click pe aplicație** pentru a o deschide
3. Dacă nu ai SSMS instalat, descarcă-l de pe site-ul Microsoft

### Pasul 2: Conectează-te la Server

1. **În fereastra "Connect to Server"** care apare:
   - **Server name**: `localhost` sau `.` sau `(local)` sau numele serverului tău
   - **Authentication**: Selectează **SQL Server Authentication**
   - **Login**: `sa` (sau utilizatorul tău)
   - **Password**: `administrator12345` (sau parola ta)
   
2. **Click pe butonul "Connect"**

### Pasul 3: Deschide Scriptul SQL

1. **În SSMS**, din meniul de sus:
   - Click pe **File** → **Open** → **File...**
   - SAU apasă **Ctrl+O**

2. **Navighează la folderul proiectului**:
   - Mergi la: `d:\ProiectPPAW\`
   - Selectează fișierul: **`ApplyAllMigrations.sql`**
   - Click **Open**

3. **Scriptul se va deschide** într-o fereastră nouă în SSMS

### Pasul 4: Selectează Baza de Date

1. **În bara de sus din SSMS**, vezi un dropdown cu numele bazelor de date
2. **Selectează** `eBooks` din dropdown
   - Dacă nu vezi `eBooks`, înseamnă că baza de date nu există și trebuie creată mai întâi

### Pasul 5: Rulează Scriptul

1. **Asigură-te că scriptul este deschis** în fereastra principală
2. **Click pe butonul "Execute"** din toolbar (sau apasă **F5**)
   - Butonul "Execute" arată ca un triunghi verde ▶️
   - Sau din meniu: **Query** → **Execute**

3. **Așteaptă** până când scriptul se termină
   - Vei vedea mesaje în fereastra "Messages" de jos
   - Ar trebui să vezi mesaje de tipul:
     ```
     Câmpul IsDeleted a fost adăugat la tabela Categorie
     Câmpul IsDeleted a fost adăugat la tabela Carte
     ...
     Migrarea a fost aplicată cu succes!
     ```

### Pasul 6: Verifică Rezultatul

1. **În fereastra de query**, scrie următoarea comandă:

```sql
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Carte', 'Categorie', 'Autor')
AND COLUMN_NAME IN ('IsDeleted', 'id_categorie', 'id_serie', 'nr_volum')
ORDER BY TABLE_NAME, COLUMN_NAME;
```

2. **Rulează query-ul** (F5)
3. **Ar trebui să vezi** o listă cu toate coloanele adăugate

---

## Dacă Baza de Date Nu Există

Dacă nu vezi baza de date `eBooks` în dropdown:

1. **Click dreapta** pe "Databases" în Object Explorer (stânga)
2. **Selectează** "New Database..."
3. **Nume**: `eBooks`
4. **Click OK**
5. **Apoi continuă** cu Pasul 4 de mai sus

---

## Dacă Ai Erori la Conectare

### Eroare: "Cannot connect to server"

**Soluții:**
1. Verifică că **SQL Server este pornit**:
   - Deschide **Services** (Win+R → `services.msc`)
   - Caută **SQL Server (MSSQLSERVER)** sau **SQL Server (SQLEXPRESS)**
   - Click dreapta → **Start** (dacă este oprit)

2. Verifică **connection string** în `Web.config`:
   - Deschide: `eBooks_MVC\Web.config`
   - Caută secțiunea `<connectionStrings>`
   - Verifică că `Data Source` este corect

### Eroare: "Login failed for user"

**Soluții:**
1. Verifică că **username** și **password** sunt corecte
2. Verifică că **SQL Server Authentication** este activată:
   - Click dreapta pe server în Object Explorer
   - **Properties** → **Security**
   - Asigură-te că **SQL Server and Windows Authentication mode** este selectat
   - Restart SQL Server după modificare

---

## Dacă Scriptul Dă Eroare

### Eroare: "Table does not exist"

**Soluție:** Baza de date nu are tabelele create. Trebuie să creezi mai întâi structura de bază.

### Eroare: "Column already exists"

**Soluție:** Nu e problemă! Scriptul verifică automat dacă coloana există și o sare dacă există deja.

---

## După Aplicarea Migrațiilor

1. **Închide SSMS** (sau lasă-l deschis pentru verificări)
2. **Reîncarcă aplicația** în browser
3. **Încearcă din nou** să te înregistrezi
4. **Eroarea ar trebui să dispară**

---

## Verificare Finală

După aplicarea scriptului, verifică că totul funcționează:

1. **Deschide aplicația** în browser
2. **Încearcă să te înregistrezi** cu un cont nou
3. **Dacă funcționează** → Migrațiile au fost aplicate cu succes! ✅
4. **Dacă mai apare eroarea** → Verifică mesajele din SSMS pentru detalii

---

## Screenshot-uri de Referință

### Cum Arată SSMS:
```
┌─────────────────────────────────────┐
│ File Edit View Tools Help          │
├─────────────────────────────────────┤
│ [Connect] [New Query] [Execute] ▶️ │
├─────────────────────────────────────┤
│ Object Explorer    │  Query Window  │
│ ├─ Databases       │  (scriptul)   │
│ │  └─ eBooks       │               │
│ └─ Security        │               │
└─────────────────────────────────────┘
```

### Cum Arată Butonul Execute:
- **Toolbar**: Triunghi verde ▶️ cu text "Execute"
- **Sau**: Meniu → Query → Execute
- **Sau**: Tastă: **F5**

---

## Succes! 🎉

După aplicarea scriptului, aplicația ar trebui să funcționeze fără erori SQL!
