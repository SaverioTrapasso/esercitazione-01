# Briefing — Sessione Sviluppo Minigioco XR

Documento di contesto per iniziare la sessione di sviluppo del minigioco in Unity. Incollalo all'inizio della conversazione.

---

## Contesto

Stiamo costruendo un minigioco XR per Meta Quest in Unity. Il progetto si chiama **esercitazione-01** ed è un progetto didattico pubblico. Il minigioco è il contenuto principale della lezione — la scena sarà pronta all'uso, gli studenti la leggono, la capiscono e modificano ShaderGraph e VFX.

Repository: https://github.com/SaverioTrapasso/esercitazione-01

---

## Il gioco

**Concept:** Duck shooting gallery in XR. Semplice, leggibile, didatticamente denso.

**Loop:**
```
START → timer 60s → spara duck → score++ → GAME OVER → print score → START
```

**Meccanica:**
- Il giocatore impugna una pistola (modello 3D agganciato alla mano)
- Preme il trigger → spara una pallina fisica
- I duck volano nella scena spawnati automaticamente
- Colpisci un duck → +1 punto
- Allo scadere del timer → schermata punteggio → restart

---

## Architettura prefab

Il progetto è costruito interamente a prefab. La scena è un contenitore — la logica sta nei prefab.

| Prefab | Categoria hierarchy | Cosa fa |
|---|---|---|
| `Gun_Left` / `Gun_Right` | INTERACTIVE | Modello 3D figlio delle ancore mano, appare al grab, spara raycast al trigger |
| `Duck` | INTERACTIVE | Bersaglio volante, ShaderGraph toon material, blendshape ali, DuckBehaviour script |
| `DuckSpawner` | GAME LOGIC | Spawna Duck a intervalli configurabili |
| `GameManager` | GAME LOGIC | State machine: Idle → Playing → GameOver |
| `Canvas` | UI | Score, timer, Start button — world space |

---

## Gerarchia scena

Convenzione adottata per tutte le scene del progetto, organizzata per ciclo di vita a runtime:

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

## Obiettivi didattici

La scena è pronta e funzionante. Gli studenti non costruiscono — leggono e modificano.

**Cosa modificano:**
- ShaderGraph toon sul prefab `Duck` — threshold, outline, palette colori
- ShaderGraph toon sul prefab `Gun` — stesso sistema
- Particle System di impatto sul prefab `Bullet` — colore, burst, lifetime, scale

**Cosa non toccano:**
- Logica C# dei prefab
- Camera Rig XR
- Qualsiasi cosa fuori da `Assets/_Project/`

---

## Stack tecnico

| | |
|---|---|
| Unity | 6000.3.15f1 |
| Render Pipeline | URP 17.3.0 |
| XR | Meta XR SDK 201.0.0 · OpenXR 1.16.1 |
| Input | Unity Input System 1.19.0 |
| Navigation | AI Navigation 2.0.12 |
| Target | Meta Quest |

---

## Stato attuale

- [x] Documentazione repo (README, ONBOARDING, RULES)
- [x] Progetto Unity creato
- [x] Scena Main strutturata
- [x] Gun (Left + Right, raycast, grab)
- [x] Duck + blendshape ali
- [x] DuckSpawner
- [x] GameManager (Idle → Playing → GameOver)
- [x] UI (Score, Timer, Start Button)
- [x] VFX impatto
- [x] Test su Quest
- [ ] ShaderGraph toon Duck
- [ ] ShaderGraph toon Gun
- [ ] Push sul repo

---

*Saverio Trapasso*
*Assisted by: Claude (Anthropic)*