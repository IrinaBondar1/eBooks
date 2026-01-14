# Ghid de Testare - Scenariu: Limita Plan Free si Upgrade

## Scenariu de Testat
**Utilizator cu plan Free citește 3 cărți (limita maximă), apoi încearcă să citească a 4-a carte și trebuie să facă upgrade.**

---

## Pregatire Initiala

### Pasul 1: Verifica Baza de Date

1. **Deschide SQL Server Management Studio (SSMS)**
2. **Conectează-te la baza de date `eBooks`**
3. **Verifică că există planul Free cu limita corectă:**

```sql
USE eBooks;
GO

-- Verifica planurile de abonament
SELECT * FROM TipAbonament;
-- Ar trebui să vezi:
-- id_tip_abonament = 1, denumire = 'Free', limita_carti_pe_luna = 3
```

### Pasul 2: Creează sau Resetează Contul de Test

**Opțiunea A: Creează un cont nou pentru test**

1. **Deschide aplicația** în browser
2. **Accesează pagina de înregistrare**: `http://localhost:port/Account/Register`
3. **Completează formularul:**
   - Nume: `TestFree`
   - Email: `testfree@example.com`
   - Parolă: `test123`
4. **Click pe "Inregistrare"**
5. **Notează ID-ul utilizatorului** (vezi în URL sau în baza de date)

**Opțiunea B: Resetează un cont existent**

```sql
-- Gaseste ID-ul utilizatorului
SELECT id_utilizator, nume_utilizator, email, id_tip_abonament, carti_citite_luna 
FROM Utilizator 
WHERE email = 'testfree@example.com';

-- Reseteaza contorul si planul la Free
UPDATE Utilizator 
SET id_tip_abonament = 1,  -- Free
    carti_citite_luna = 0
WHERE email = 'testfree@example.com';
```

---

## Testarea Scenariului

### Pasul 3: Verifică Planul Curent

1. **Autentifică-te** cu contul de test:
   - URL: `http://localhost:port/Account/Login`
   - Email: `testfree@example.com`
   - Parolă: `test123`

2. **Accesează pagina de profil:**
   - URL: `http://localhost:port/Utilizator/Profil`
   - **Verifică că:**
     - Planul curent este **Free**
     - Contorul este **0 / 3** (sau valoarea actuală)
     - Există opțiune de upgrade la Premium sau VIP

3. **Accesează pagina de cărți:**
   - URL: `http://localhost:port/Utilizator/Carti`
   - **Verifică că:**
     - Se afișează mesajul: `Plan curent: Free - 0 / 3 carti citite luna curenta`
     - Sunt listate cărțile disponibile

### Pasul 4: Citește Prima Carte

1. **Din pagina `/Utilizator/Carti`**, click pe o carte sau accesează direct:
   - URL: `http://localhost:port/Utilizator/DetaliiCarte/1` (înlocuiește `1` cu ID-ul unei cărți existente)

2. **Verifică pagina de detalii:**
   - Se afișează titlul, autorul, descrierea
   - Există butonul **"Citeste"**

3. **Click pe butonul "Citeste"**

4. **Verifică rezultatul:**
   - ✅ Ar trebui să vezi mesaj de succes: `Ati inceput sa cititi: [Titlu Carte]`
   - ✅ Contorul ar trebui să crească la **1 / 3**

5. **Verifică în baza de date:**
```sql
SELECT id_utilizator, carti_citite_luna, id_tip_abonament 
FROM Utilizator 
WHERE email = 'testfree@example.com';
-- carti_citite_luna ar trebui să fie 1
```

6. **Verifică istoricul:**
```sql
SELECT TOP 1 * 
FROM IstoricCitire 
WHERE id_utilizator = [ID_UTILIZATOR]
ORDER BY data_accesare DESC;
-- Ar trebui să vezi o înregistrare nouă cu actiune = 'citire'
```

### Pasul 5: Citește a Doua Carte

1. **Reveniți la lista de cărți**: `http://localhost:port/Utilizator/Carti`
2. **Alegeți o altă carte** (diferită de prima)
3. **Click pe "Citeste"**
4. **Verifică că:**
   - ✅ Mesaj de succes apare
   - ✅ Contorul este acum **2 / 3**

### Pasul 6: Citește a Treia Carte (Ultima Permisă)

1. **Reveniți la lista de cărți**
2. **Alegeți o a treia carte** (diferită de primele două)
3. **Click pe "Citeste"**
4. **Verifică că:**
   - ✅ Mesaj de succes apare
   - ✅ Contorul este acum **3 / 3** (limita atinsă!)

5. **Verifică în profil:**
   - Accesează: `http://localhost:port/Utilizator/Profil`
   - **Ar trebui să vezi:**
     - `3 / 3` cu mesajul `(Limita atinsa)` în roșu

### Pasul 7: Încearcă să Citești a Patra Carte (Ar Trebui să Fie Blocat)

1. **Reveniți la lista de cărți**: `http://localhost:port/Utilizator/Carti`
2. **Verifică mesajul de atenționare:**
   - ✅ Ar trebui să vezi un alert roșu:
     ```
     Atentie! Ati atins limita de carti pentru aceasta luna. 
     Upgrade la Premium sau VIP pentru mai multe carti!
     ```

3. **Alegeți o a patra carte** (diferită de primele trei)
4. **Click pe "Citeste"**

5. **Verifică rezultatul:**
   - ❌ **NU ar trebui să poți citi!**
   - ✅ Ar trebui să vezi mesaj de eroare: 
     ```
     Nu mai puteti citi carti in aceasta luna sau nu aveti acces la aceasta carte!
     ```
   - ✅ Contorul rămâne **3 / 3** (nu crește la 4)

6. **Verifică în baza de date:**
```sql
SELECT carti_citite_luna 
FROM Utilizator 
WHERE email = 'testfree@example.com';
-- Ar trebui să fie încă 3, NU 4
```

### Pasul 8: Face Upgrade la Premium sau VIP

1. **Accesează pagina de profil:**
   - URL: `http://localhost:port/Utilizator/Profil`

2. **Verifică secțiunea "Upgrade Cont":**
   - ✅ Ar trebui să vezi un dropdown cu planuri disponibile:
     - `Premium - 10 carti/luna`
     - `VIP - Nelimitat carti/luna`

3. **Selectează un plan superior** (ex: Premium)
4. **Click pe butonul "Upgrade Cont"**

5. **Verifică rezultatul:**
   - ✅ Ar trebui să vezi un mesaj de succes
   - ✅ Planul curent se actualizează la Premium (sau VIP)
   - ✅ Contorul rămâne **3 / 10** (sau **3 / Nelimitat** pentru VIP)

6. **Verifică în baza de date:**
```sql
SELECT id_utilizator, id_tip_abonament, carti_citite_luna 
FROM Utilizator 
WHERE email = 'testfree@example.com';
-- id_tip_abonament ar trebui să fie 2 (Premium) sau 3 (VIP)
```

### Pasul 9: După Upgrade, Citește a Patra Carte (Acum Ar Trebui să Funcționeze)

1. **Reveniți la lista de cărți**: `http://localhost:port/Utilizator/Carti`
2. **Verifică mesajul:**
   - ✅ Mesajul de atenționare roșu ar trebui să dispară
   - ✅ Ar trebui să vezi: `Plan curent: Premium - 3 / 10 carti citite luna curenta`
     (sau `VIP - 3 / Nelimitat` dacă ai ales VIP)

3. **Alegeți a patra carte**
4. **Click pe "Citeste"**

5. **Verifică rezultatul:**
   - ✅ **Acum ar trebui să poți citi!**
   - ✅ Mesaj de succes apare
   - ✅ Contorul crește la **4 / 10** (sau **4 / Nelimitat** pentru VIP)

---

## Verificări Suplimentare

### Verificare Loguri

Dacă ai configurat NLog, verifică logurile pentru mesaje de tip:
- `ACCES : Utilizator [ID] a atins limita de carti pentru luna curenta`
- `ACCES : Inregistrat acces pentru Utilizator [ID], Carte [ID], Actiune: citire`

### Verificare Istoric Citiri

```sql
-- Vezi toate citirile utilizatorului
SELECT 
    ic.id_istoric,
    ic.data_accesare,
    ic.actiune,
    c.titlu,
    u.nume_utilizator
FROM IstoricCitire ic
INNER JOIN Carte c ON ic.id_carte = c.id_carte
INNER JOIN Utilizator u ON ic.id_utilizator = u.id_utilizator
WHERE u.email = 'testfree@example.com'
ORDER BY ic.data_accesare DESC;
```

---

## Rezultate Așteptate - Checklist

- [ ] **Pasul 3**: Planul curent este Free, contorul este 0/3
- [ ] **Pasul 4**: Prima carte se citește cu succes, contorul devine 1/3
- [ ] **Pasul 5**: A doua carte se citește cu succes, contorul devine 2/3
- [ ] **Pasul 6**: A treia carte se citește cu succes, contorul devine 3/3
- [ ] **Pasul 7**: A patra carte **NU** se poate citi, mesaj de eroare apare, contorul rămâne 3/3
- [ ] **Pasul 8**: Upgrade-ul funcționează, planul se schimbă la Premium/VIP
- [ ] **Pasul 9**: După upgrade, a patra carte **SE POATE** citi, contorul devine 4/10 (sau 4/Nelimitat)

---

## Troubleshooting

### Problema: Contorul nu se incrementează

**Soluție:**
- Verifică că butonul "Citeste" folosește POST cu `ValidateAntiForgeryToken`
- Verifică logurile pentru erori
- Verifică că `InregistrareAccesCarte` este apelată corect

### Problema: Pot citi mai mult de 3 cărți fără upgrade

**Soluție:**
- Verifică că `PoateCitireInca()` verifică corect limita:
  ```csharp
  return utilizator.carti_citite_luna < utilizator.TipAbonament.limita_carti_pe_luna;
  ```
- Verifică că `PoateAccesaCarte()` apelează `PoateCitireInca()` înainte de a permite accesul

### Problema: Upgrade-ul nu funcționează

**Soluție:**
- Verifică că dropdown-ul afișează planuri superioare (id > planul curent)
- Verifică că `UpgradeCont()` validează că `idTipAbonamentNou > utilizator.id_tip_abonament`
- Verifică logurile pentru erori

### Problema: Mesajul de atenționare nu apare

**Soluție:**
- Verifică că `ViewBag.CanReadMore` este setat corect în `Carti()` action:
  ```csharp
  ViewBag.CanReadMore = accesService.PoateCitireInca(currentUser);
  ```

---

## Resetare Pentru Teste Repetate

Dacă vrei să repeți testul, resetează contorul:

```sql
-- Reseteaza contorul si planul
UPDATE Utilizator 
SET id_tip_abonament = 1,  -- Free
    carti_citite_luna = 0
WHERE email = 'testfree@example.com';

-- Sau reseteaza doar contorul (pastreaza planul)
UPDATE Utilizator 
SET carti_citite_luna = 0
WHERE email = 'testfree@example.com';
```

---

## Note Importante

1. **Resetare automată a contorului**: Contorul `carti_citite_luna` nu se resetează automat la începutul lunii. Pentru testare, resetează-l manual în baza de date.

2. **VIP are acces nelimitat**: Dacă faci upgrade la VIP, contorul nu se mai incrementează (vezi `AccesService.InregistrareAccesCarte`).

3. **Verificare în timp real**: Poți verifica starea în timp real accesând `/Utilizator/Profil` sau verificând direct în baza de date.

---

**Succes la testare! 🚀**
