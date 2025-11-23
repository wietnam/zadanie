## Serwer GÓRA — Port + przyciski

### 1. **Label**

* **Text:** `Port:`

### 2. **TextBox**

* **Name:** `txtPort`
* **Text:** `9000`

### 3. **Button**

* **Name:** `btnStart`
* **Text:** `Start nasłuchu`

### 4. **Button**

* **Name:** `btnSend`
* **Text:** `Wyślij notatki`

### 5. **Button**

* **Name:** `btnSaveXml`
* **Text:** `Zapisz do XML`

---

## 📄 LEWA STRONA — Lista notatek

### 6. **ListBox**

* **Name:** `lstNotes`

---

## ✍️ PRAWA STRONA — Dane nowej notatki

### 7. **Label**

* **Text:** `Tytuł:`

### 8. **TextBox**

* **Name:** `txtTitle`

---

### 9. **Label**

* **Text:** `Treść:`

### 10. **TextBox**

* **Name:** `txtBody`
* **Multiline:** `True`
* **ScrollBars:** `Vertical`

---

### 11. **Label**

* **Text:** `Tagi (po przecinku):`

### 12. **TextBox**

* **Name:** `txtTags`

---

### 13. **Button**

* **Name:** `btnAddNote`
* **Text:** `Dodaj notatkę`

---

## ✔️ Efekt końcowy

Po dodaniu powyższych elementów formularz zawiera:

* pole do wpisania portu,
* przyciski do sterowania serwerem,
* listę notatek,
* pola do tworzenia nowych notatek,
* przycisk „Dodaj notatkę”.



Aaaa, teraz rozumiem — **chcesz listę kontrolek i ich ustawień TAK SAMO jak zrobiłem dla serwera**, tylko dla **KLIENTA**.

Czyli w formie:

* 🔼 sekcja górna — IP, port, hasło, connect
* 📄 lewa — lista notatek
* ✍️ prawa — podgląd notatki
* 🔍 dół — wyszukiwarka
* 💾 zapis lokalny

Dokładnie taką samą listę kroków opisowych jak wcześniej.

To robimy 👇

---

# KLIENT

## 🔼 GÓRA — Dane połączenia z serwerem

### 1. Label

**Text:** `Server IP:`

### 2. TextBox

**Name:** `txtServerIp`
**Text:** `127.0.0.1`
**Width:** 140

---

### 3. Label

**Text:** `Port:`

### 4. TextBox

**Name:** `txtPort`
**Text:** `9000`
**Width:** 60

---

### 5. Label

**Text:** `Pass:`

### 6. TextBox

**Name:** `txtPassword`
**Text:** `haslo`
**Width:** 120

---

### 7. Button

**Name:** `btnConnect`
**Text:** `Connect`

---

## 📄 LEWA STRONA — Lista odebranych notatek

### 8. ListBox

**Name:** `lstNotes`
**Width:** ~300
**Height:** ~360

---

## ✍️ PRAWA STRONA — Podgląd wybranej notatki

### 9. TextBox

**Name:** `txtNoteBody`
**Multiline:** `True`
**ScrollBars:** `Vertical`
**Width:** ~400
**Height:** ~330

---

### 10. Button

**Name:** `btnSaveLocal`
**Text:** `Save locally (XML)`

---

## 🔍 DÓŁ — Wyszukiwarka notatek

### 11. TextBox

**Name:** `txtSearch`
**Width:** 220

### 12. Button

**Name:** `btnSearch`
**Text:** `Search`

---

# ✔️ Efekt końcowy (opis GUI)

Po dodaniu wszystkich powyższych elementów **formularz klienta zawiera**:

### 🔼 Sekcja połączenia:

* pole `Server IP`
* pole `Port`
* pole `Pass`
* przycisk `Connect`

### 📄 Lewa strona:

* lista odebranych notatek (`lstNotes`)

### ✍️ Prawa strona:

* pole podglądu treści notatki (`txtNoteBody`)
* przycisk zapisu lokalnego (`btnSaveLocal`)

### 🔍 Dół:

* wyszukiwarka (`txtSearch`)
* przycisk `Search`




