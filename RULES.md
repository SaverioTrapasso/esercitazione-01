# Rules

[Torna al README](README.md) | [Vai all'ONBOARDING](ONBOARDING.md)

Regole condivise per lavorare su questo progetto. Valgono per tutti, sempre.

---

## Documenti

Ogni documento deve essere firmato.

- Se l'autore è un umano: firma con nome e cognome
- Se il documento è generato o assistito da una AI: aggiungi `Assisted by: [nome AI]`

Esempi:

```
Mario Rossi

Mario Rossi
Assisted by: Claude (Anthropic)

Assisted by: Claude (Anthropic)
```

La firma va in fondo al documento, dopo un separatore `---`.

---

## Gerarchia della scena

Tutte le scene Unity di questo progetto seguono la stessa struttura, organizzata per ciclo di vita a runtime:

```
## LIGHT        luci, reflection probe, light probe
## STATIC       tutto ciò che non cambia a runtime
## DYNAMIC      tutto ciò che si muove senza input utente
## INTERACTIVE  tutto ciò che risponde all'utente
## UI           tutto ciò che mostra informazioni all'utente
## GAME LOGIC   oggetti che conoscono le regole del gioco
## SYSTEM       infrastruttura tecnica, indipendente dal gioco
```

---

## Struttura del progetto

Tutto il contenuto del progetto sta sotto `Assets/_Project/`. Le cartelle di SDK e plugin esterni non si toccano e non si riorganizzano.

---

## Git

- Commit spesso, commit piccoli, commit con senso
- Committa sempre i file `.meta`
- Fai `git pull` prima di iniziare a lavorare
- Se modifichi una scena condivisa, avvisa il team prima

---

*Saverio Trapasso*
*Assisted by: Claude (Anthropic)*
