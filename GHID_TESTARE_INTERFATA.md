# Ghid de Testare - Interfață: Limita Plan Free și Upgrade

## Scenariu
**Utilizator cu plan Free citește 3 cărți, apoi încearcă să citească a 4-a carte și trebuie să facă upgrade.**

---

## Pași de Testare în Browser

### 📋 Pasul 1: Pregătire - Creează Cont Free

1. **Deschide aplicația în browser**
   - URL: `http://localhost:[PORT]/` (sau portul tău)
   - Ar trebui să vezi pagina de Login

2. **Click pe "Inregistreaza-te acum"** (sau accesează direct `/Account/Register`)

3. **Completează formularul:**
   - **Nume**: `TestFree`
   - **Email**: `testfree@example.com`
   - **Parola**: `test123`
   - **Confirma Parola**: `test123`

4. **Click pe butonul "Inregistrare"**

5. **Rezultat așteptat:**
   - ✅ Ești redirecționat către pagina principală (Home/Index)
   - ✅ Ești autentificat automat
   - ✅ Planul tău este **Free** (implicit)

---

### 📋 Pasul 2: Verifică Planul Curent

1. **Click pe "Profil"** în meniu (sau accesează `/Utilizator/Profil`)

2. **Verifică informațiile:**
   - ✅ **Plan**: Free
   - ✅ **Carti citite luna**: `0 / 3`
   - ✅ **Mesaj**: `(3 ramase)` în verde

3. **Verifică secțiunea "Upgrade Cont":**
   - ✅ Ar trebui să vezi un dropdown cu:
     - `Premium - 10 carti/luna`
     - `VIP - Nelimitat carti/luna`
   - ✅ Butonul "Upgrade Cont" este vizibil

---

### 📋 Pasul 3: Vezi Cărțile Disponibile

1. **Click pe "Carti"** în meniu (sau accesează `/Utilizator/Carti`)

2. **Verifică pagina:**
   - ✅ Se afișează un mesaj albastru (alert-info):
     ```
     Plan curent: Free - 0 / 3 carti citite luna curenta
     ```
   - ✅ Sunt listate cărțile disponibile într-un tabel
   - ✅ Fiecare carte are un link "Detalii"

3. **Notează ID-urile sau titlurile a 4 cărți diferite** (vei avea nevoie de ele)

---

### 📋 Pasul 4: Citește Prima Carte

1. **Din lista de cărți**, click pe **"Detalii"** pentru prima carte

2. **Pe pagina de detalii, verifică:**
   - ✅ Se afișează titlul, autorul, descrierea
   - ✅ Există butonul **"Citeste"** (buton albastru)

3. **Click pe butonul "Citeste"**

4. **Rezultat așteptat:**
   - ✅ Mesaj verde de succes: `Ati inceput sa cititi: [Titlu Carte]`
   - ✅ Ești redirecționat înapoi la pagina de detalii

5. **Verifică contorul:**
   - Click pe "Carti" în meniu
   - ✅ Mesajul se actualizează: `Plan curent: Free - 1 / 3 carti citite luna curenta`

6. **Verifică în profil:**
   - Click pe "Profil"
   - ✅ **Carti citite luna**: `1 / 3`
   - ✅ **Mesaj**: `(2 ramase)` în verde

---

### 📋 Pasul 5: Citește a Doua Carte

1. **Click pe "Carti"** în meniu

2. **Alege o altă carte** (diferită de prima)

3. **Click pe "Detalii"** → **Click pe "Citeste"**

4. **Verifică:**
   - ✅ Mesaj de succes apare
   - ✅ Contorul devine `2 / 3`

5. **Verifică în profil:**
   - ✅ **Carti citite luna**: `2 / 3`
   - ✅ **Mesaj**: `(1 ramasa)` în verde

---

### 📋 Pasul 6: Citește a Treia Carte (Ultima Permisă)

1. **Click pe "Carti"** în meniu

2. **Alege o a treia carte** (diferită de primele două)

3. **Click pe "Detalii"** → **Click pe "Citeste"**

4. **Verifică:**
   - ✅ Mesaj de succes apare
   - ✅ Contorul devine `3 / 3` (limita atinsă!)

5. **Verifică în profil:**
   - Click pe "Profil"
   - ✅ **Carti citite luna**: `3 / 3`
   - ✅ **Mesaj**: `(Limita atinsa)` în roșu

---

### 📋 Pasul 7: Încearcă să Citești a Patra Carte (Ar Trebui să Fie Blocat)

1. **Click pe "Carti"** în meniu

2. **Verifică mesajul de atenționare:**
   - ✅ Ar trebui să vezi un **alert roșu** (alert-danger) în partea de sus:
     ```
     ⚠️ Atentie! Ati atins limita de carti pentru aceasta luna. 
     Upgrade la Premium sau VIP pentru mai multe carti!
     ```

3. **Alege o a patra carte** (diferită de primele trei)

4. **Click pe "Detalii"** → **Click pe "Citeste"**

5. **Rezultat așteptat:**
   - ❌ **NU ar trebui să poți citi!**
   - ✅ Mesaj roșu de eroare apare:
     ```
     Nu mai puteti citi carti in aceasta luna sau nu aveti acces la aceasta carte!
     ```
   - ✅ Ești redirecționat înapoi la lista de cărți

6. **Verifică că contorul NU s-a schimbat:**
   - Click pe "Profil"
   - ✅ **Carti citite luna**: `3 / 3` (rămâne la 3, NU devine 4)

---

### 📋 Pasul 8: Face Upgrade la Premium sau VIP

1. **Click pe "Profil"** în meniu

2. **Găsește secțiunea "Upgrade Cont"** (panou galben)

3. **În dropdown-ul "Selecteaza noul plan":**
   - Selectează **"Premium - 10 carti/luna"** (sau VIP dacă preferi)

4. **Click pe butonul "Upgrade Cont"**

5. **Rezultat așteptat:**
   - ✅ Mesaj de succes apare (dacă este implementat)
   - ✅ Pagina se reîncarcă
   - ✅ **Plan**: Se schimbă la **Premium** (sau VIP)
   - ✅ **Carti citite luna**: `3 / 10` (sau `3 / Nelimitat` pentru VIP)
   - ✅ Secțiunea "Upgrade Cont" dispare sau se actualizează (doar VIP rămâne disponibil)

---

### 📋 Pasul 9: După Upgrade, Citește a Patra Carte (Acum Ar Trebui să Funcționeze)

1. **Click pe "Carti"** în meniu

2. **Verifică mesajul:**
   - ✅ **Alert-ul roșu de atenționare a dispărut!**
   - ✅ Mesajul albastru se actualizează:
     - Pentru Premium: `Plan curent: Premium - 3 / 10 carti citite luna curenta`
     - Pentru VIP: `Plan curent: VIP - 3 / Nelimitat carti citite luna curenta`

3. **Alege a patra carte** (aceeași pe care ai încercat-o la Pasul 7)

4. **Click pe "Detalii"** → **Click pe "Citeste"**

5. **Rezultat așteptat:**
   - ✅ **Acum ar trebui să poți citi!**
   - ✅ Mesaj verde de succes: `Ati inceput sa cititi: [Titlu Carte]`

6. **Verifică contorul:**
   - Click pe "Carti"
   - ✅ Pentru Premium: `Plan curent: Premium - 4 / 10 carti citite luna curenta`
   - ✅ Pentru VIP: `Plan curent: VIP - 4 / Nelimitat carti citite luna curenta`

7. **Verifică în profil:**
   - Click pe "Profil"
   - ✅ **Carti citite luna**: `4 / 10` (sau `4 / Nelimitat`)

---

## Checklist Vizual - Ce Ar Trebui să Vezi

### ✅ După Pasul 4 (Prima carte):
- [ ] Mesaj verde: "Ati inceput sa cititi: [Titlu]"
- [ ] Contor: `1 / 3` în pagina Carti
- [ ] Profil: `1 / 3 (2 ramase)` în verde

### ✅ După Pasul 5 (A doua carte):
- [ ] Contor: `2 / 3`
- [ ] Profil: `2 / 3 (1 ramasa)` în verde

### ✅ După Pasul 6 (A treia carte):
- [ ] Contor: `3 / 3`
- [ ] Profil: `3 / 3 (Limita atinsa)` în roșu

### ✅ După Pasul 7 (Încearcă a patra carte):
- [ ] Alert roșu în pagina Carti: "Atentie! Ati atins limita..."
- [ ] Mesaj roșu de eroare: "Nu mai puteti citi carti..."
- [ ] Contorul rămâne `3 / 3` (NU devine 4)

### ✅ După Pasul 8 (Upgrade):
- [ ] Planul se schimbă la Premium/VIP
- [ ] Contorul devine `3 / 10` (sau `3 / Nelimitat`)
- [ ] Secțiunea Upgrade se actualizează

### ✅ După Pasul 9 (A patra carte după upgrade):
- [ ] Alert-ul roșu dispare
- [ ] Poți citi a patra carte cu succes
- [ ] Contorul devine `4 / 10` (sau `4 / Nelimitat`)

---

## Screenshot-uri de Referință

### Cum Arată Pagina "Carti" cu Limita Atinsă:
```
┌─────────────────────────────────────────┐
│ ⚠️ Atentie! Ati atins limita de carti  │
│    pentru aceasta luna. Upgrade la     │
│    Premium sau VIP pentru mai multe!   │
└─────────────────────────────────────────┘

Plan curent: Free - 3 / 3 carti citite luna curenta

[Tabel cu carti...]
```

### Cum Arată Profilul cu Limita Atinsă:
```
Plan: Free
Carti citite luna: 3 / 3 (Limita atinsa) [în roșu]

┌─────────────────────────────────────┐
│ Upgrade Cont                        │
│ Selecteaza noul plan:               │
│ [Premium - 10 carti/luna ▼]        │
│ [Upgrade Cont]                      │
└─────────────────────────────────────┘
```

---

## Probleme Comune și Soluții

### ❌ Problema: Nu văd butonul "Citeste"
**Soluție:**
- Verifică că ești autentificat (vezi numele tău în meniu)
- Verifică că ai accesat pagina de detalii a unei cărți
- Verifică că nu ai atins deja limita

### ❌ Problema: Pot citi mai mult de 3 cărți fără upgrade
**Soluție:**
- Verifică că contorul se incrementează corect (vezi Profil)
- Verifică că limita planului Free este 3 (vezi `/Utilizator` - Planuri)
- Verifică că `PoateCitireInca()` funcționează corect

### ❌ Problema: Nu apare alert-ul roșu când atinge limita
**Soluție:**
- Verifică că `ViewBag.CanReadMore` este setat în controller
- Reîncarcă pagina după ce ai citit a treia carte
- Verifică că nu există erori JavaScript în consolă (F12)

### ❌ Problema: Upgrade-ul nu funcționează
**Soluție:**
- Verifică că ai selectat un plan din dropdown
- Verifică că dropdown-ul afișează planuri superioare
- Verifică consola browser-ului pentru erori (F12 → Console)
- Verifică că nu există erori în Network tab (F12 → Network)

---

## Testare Rapidă (5 Minute)

Dacă vrei să testezi rapid:

1. **Creează cont** → `/Account/Register`
2. **Vezi cărți** → `/Utilizator/Carti`
3. **Citește 3 cărți** → Click "Detalii" → "Citeste" (x3)
4. **Verifică limita** → Vezi alert roșu în pagina Carti
5. **Încearcă a 4-a carte** → Ar trebui să fie blocat
6. **Upgrade** → Profil → Selectează Premium → "Upgrade Cont"
7. **Citește a 4-a carte** → Acum ar trebui să funcționeze!

---

## Note Importante

1. **Contorul nu se resetează automat** - Pentru a testa din nou, resetează-l manual în baza de date sau creează un cont nou

2. **VIP are acces nelimitat** - Dacă faci upgrade la VIP, contorul nu se mai incrementează după upgrade

3. **Verifică în timp real** - Poți verifica starea accesând `/Utilizator/Profil` oricând

---

**Succes la testare! 🚀**
