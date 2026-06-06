# Esercitazione-01 — Onboarding

[Torna al README](README.md) | [Vai alle REGOLE](RULES.md)

Questo è un progetto Unity XR pubblico che uso per insegnare sviluppo immersivo su Meta Quest. Se sei uno studente del corso, benvenuto. Se sei arrivato qui per conto tuo, benvenuto lo stesso — ho lasciato tutto leggibile di proposito.

Quello che trovi qui è il mio metodo, non il metodo. Prendilo come punto di partenza, adattalo, miglioralo.

---

## 0 — Setup obbligatorio

Prima di aprire Unity, fai queste tre cose nell'ordine esatto.

**1. Verifica Git LFS**
```bash
git lfs install   # va fatto una volta sola per macchina, non per repo
git lfs pull
```
Senza LFS pull i modelli 3D e le texture non ci sono. La scena si apre ma è vuota. Non è un bug.

**2. Apri con la versione giusta**
```
Unity 6000.3.15f1
```
Versioni diverse = errori di compilazione garantiti. Usa Unity Hub per installare la versione esatta se non ce l'hai.

**3. Lascia importare i package**
Al primo avvio Unity importa Meta XR SDK, OpenXR e URP. Ci vuole qualche minuto. Non toccare niente finché la progress bar in basso non sparisce.

---

## 1 — Struttura del progetto

Tutto quello che ho scritto io sta sotto `Assets/_Project/`. È l'unica cartella che ti riguarda.

```
Assets/
├── _Project/          ← tutto tuo
│   ├── Scenes/        ← la scena Main
│   ├── Scripts/       ← codice di gioco
│   ├── Prefabs/       ← Gun, Bullet, Duck, DuckSpawner, UI
│   ├── Models/        ← asset 3D, un subfolder per asset
│   ├── Textures/      ← texture e materiali
│   ├── Settings/      ← configurazioni URP e XR
│   └── Audio/         ← sound effects e musica
│
├── Oculus/            ← Meta XR SDK. Non toccare.
├── XR/                ← OpenXR. Non toccare.
├── Plugins/           ← plugin nativi. Non toccare.
└── ThirdParty/        ← librerie esterne. Non toccare.
```

La regola è semplice: se non è dentro `_Project`, non è roba tua.

---

## 2 — Convenzioni della scena

Tutte le scene di questo progetto seguono la stessa gerarchia. Impararla una volta vale per tutto.

La hierarchy è organizzata per **ciclo di vita a runtime** — non per area di lavoro, non per tipo di asset, ma per quello che l'oggetto fa mentre il gioco gira.

```
## LIGHT        tutto ciò che emette o raccoglie luce
                → luci dirette, point light, reflection probe, light probe

## STATIC       tutto ciò che non cambia mai a runtime
                → floor, pareti, props decorativi, skybox

## DYNAMIC      tutto ciò che si muove senza input dell'utente
                → animazioni ambientali, oggetti procedurali, spawn automatici

## INTERACTIVE  tutto ciò che risponde all'utente
                → Gun, Duck, trigger volumes, UI interattiva
                → regola: se ha almeno un'interazione con l'utente, va qui

## GAME LOGIC   oggetti che conoscono le regole di questo gioco
                → GameManager, ScoreManager, DuckSpawner, timer
                → sa cosa è un Duck, sa cosa significa "hai vinto"

## SYSTEM       infrastruttura tecnica invisibile, indipendente dal gioco
                → NavMesh Surface, AudioManager, configurazioni XR
```

Se aggiungi un oggetto e non sai dove metterlo, chiediti: *risponde all'utente?* → INTERACTIVE. *Conosce le regole del gioco?* → GAME LOGIC. *Si muove da solo?* → DYNAMIC. *Sta fermo?* → STATIC. *Tiene in piedi la scena ma non sa niente del gioco?* → SYSTEM.

---

## 3 — Architettura prefab

Il progetto è costruito interamente a prefab. La scena è un contenitore — la logica sta nei prefab.

| Prefab | Categoria | Cosa fa |
|---|---|---|
| `Gun` | INTERACTIVE | Modello 3D agganciato alla mano, XR Grab Interactable, spara Bullet al trigger |
| `Bullet` | INTERACTIVE | Proiettile fisico, Rigidbody, si distrugge e notifica score al contatto |
| `Duck` | INTERACTIVE | Bersaglio volante, ShaderGraph toon material, DuckBehaviour script |
| `DuckSpawner` | GAME LOGIC | Spawna Duck su un path con timer configurabile |
| `GameManager` | GAME LOGIC | State machine: Idle → Playing → GameOver |
| `UI_HUD` | INTERACTIVE | Canvas world space, mostra timer e score durante il gioco |
| `UI_StartScreen` | INTERACTIVE | Canvas world space, schermata iniziale con Start |
| `UI_ScoreScreen` | INTERACTIVE | Canvas world space, mostra il punteggio finale |

---

## 4 — Contribuire

Questo progetto è pubblico e lasciato aperto di proposito. Se trovi qualcosa che non va, un approccio migliore, o vuoi aggiungere qualcosa — fallo. Una PR è benvenuta quanto un'issue o un commento.

Il codice e le scelte architetturali riflettono un approccio, non una verità assoluta. Se vedi un modo migliore, usalo.

---

## 5 — Workflow Git

Commit spesso, commit piccoli, commit con senso.

```bash
# Prima di iniziare a lavorare
git pull

# Regola generale
git add .
git commit -m "Duck shader: cambio palette colori warm/cool"
git push
```

Committa sempre i file `.meta`. Senza di loro Unity perde i riferimenti agli asset e la scena si rompe in modo silenzioso.

Se modifichi una scena condivisa, dillo prima agli altri. Unity non ha lock dei file — due persone che modificano la stessa scena in parallelo generano conflitti quasi impossibili da risolvere.

---

## Stack tecnico

| | |
|---|---|
| Unity | 6000.3.15f1 |
| Render Pipeline | Universal Render Pipeline (URP) 17.3.0 |
| XR | Meta XR SDK 201.0.0 · OpenXR Plugin 1.16.1 |
| Input | Unity Input System 1.19.0 |
| Navigation | AI Navigation 2.0.12 |
| Version Control | Git + Git LFS |
| Target platform | Meta Quest |

---

*Saverio Trapasso*
*Assisted by: Claude (Anthropic)*
