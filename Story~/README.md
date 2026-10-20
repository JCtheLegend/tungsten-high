# Tungsten High — Story Bible

This folder is the **writers' room** for Tungsten High: character profiles, per-chapter/per-day
plot planning, worldbuilding, and the overarching plot threads. It is plain Markdown so it can be
read, searched, and edited by hand or by Claude in future sessions. It lives at the project **root
(outside `Assets/`)** on purpose, so Unity does not import these files as assets.

> This is a **design/planning** document, not code. Nothing here is loaded by the game at runtime.
> When a plot point becomes a real playable beat, it is authored as scene/cutscene JSON under
> `Assets/Resources/` — see the mapping below.

## Premise (one paragraph)

Superheroes live openly among ordinary people, and gifted students attend schools that train their
powers. The player is a student at **Tungsten High** with a rare ability: he can walk in and out of
the **Dream World** — a parallel of the real-world map — and *into other people*. Over the school
year he makes friends, faces enemies, and works to uncover a plot to kidnap the school.

## How this folder is organized

| File / folder | What it holds |
|---|---|
| `README.md` | This index + the code-mapping conventions below. |
| `Interview-Log.md` | State for the `/story-interview` flow — the queue of story questions Claude asks you, parked questions, and a session log. |
| `Chapter1-Schedule.md` | **The Chapter 1 build backlog** — every task left to ship Chapter 1, tagged story / web / editor. |
| `Worldbuilding.md` | The setting, the powers system, the Dream World, factions, tone, and a glossary. |
| `PlotThreads.md` | The central mystery and every subplot, tracked as threads that span chapters. |
| `Characters/_Roster.md` | One-line index of every character, with a link to each profile. |
| `Characters/_TEMPLATE.md` | Copy this to start a new character profile. |
| `Characters/*.md` | One profile per character. |
| `Chapters/_TEMPLATE-Chapter.md` | Copy this to start a new chapter. |
| `Chapters/Chapter-1.md` … `Chapter-6.md` | One file per chapter (week). Each breaks its days (Day 0 intro dream + Days 1–5) into stages/scenes. Only **Chapter 1** is built today. |

## Story shape

- **6 chapters total.** Each chapter is **one week** of the school year; in-story time passes
  *between* chapters (so the year is spread across the 6 weeks that are actually played).
- **Days within a chapter** are tracked by `dayCounter` (0–5):
  - **Day 0** = the **pre-week intro dream** — the night *before* the first school day, spent in the Dream World.
  - **Days 1–5** = the five weekdays. Most are school days.
- **4 classes each school day**, in this order: **Public Relations → Gym → Psychological Powers →
  Science of Superpowers** (with Lunch between Gym and Psych).
- **After school (Post)**: some days have **Dodgeball** practice or a game, clubs, home life, town errands, etc.
- **Night (Dream)**: the player goes to bed and is transported into the **Dream World**, a parallel
  of the real map, which he explores.

> **Only Chapter 1 is built.** Its cutscenes live in `Assets/Resources/Cutscenes/Chapter1/{0..5}/`
> and cover the full week (see `Chapters/Chapter-1.md`). Chapters 2–6 are planning stubs.

## The daily stage loop (this is also the `stage` enum in code)

Each played day flows through these stages, in order. They are the authoritative
`stage` enum in `Assets/Scripts/Enviroment/GameManager.cs`:

| # | Stage (`stage` enum) | In-fiction meaning |
|---|---|---|
| 0 | `pre` | Morning — wake up at home, commute to school |
| 1 | `pr` | **Public Relations** class |
| 2 | `gym` | **Gym** class |
| 3 | `lunch` | Cafeteria — the social hub |
| 4 | `psych` | **Psychological Powers** class |
| 5 | `sci` | **Science of Superpowers** class |
| 6 | `post` | After school — Dodgeball, clubs, home, dinner, go to bed |
| 7 | `dream` | The **Dream World** that night |

## How the planning docs map to the game's progression counters

The game tracks exactly where you are with counters in `GameManager` (progression is persisted in
`gameData.json`). **The story docs use the same coordinates**, so a plot beat here points straight at
the JSON that implements it:

| Story concept | Code counter | Notes |
|---|---|---|
| **Chapter / week** | `chapterCounter` | Selects the `Resources/Cutscenes/Chapter<N>/` folder. Defaults to `1`; only Chapter 1 has content. |
| **Day within the week** (0–5) | `dayCounter` | 0 = pre-week intro dream, 1–5 = the weekdays. Picks the `…/Chapter<N>/<dayCounter>/` subfolder. |
| **Time-of-day / class** | `stageCounter` (`stage` enum) | The 8 stages above. |
| **Beat within a stage** | `sceneCounter` | Advances as scenes play out; cutscenes filter on `validScenes` ranges. |

> Scene *geometry* JSON (`SceneLoader`) is still keyed by day only: `Resources/Scenes/<dayCounter>/…`.
> That folder was **not** moved under a chapter folder — only the cutscenes were.

### Cutscene file naming & location

Cutscene JSON lives at `Resources/Cutscenes/Chapter<N>/<dayCounter>/` and is named:

```
<dayCounter>.<Stage>.<SceneName>[.<variant/sceneCounter>].json
```

Examples that exist in Chapter 1:
- `Cutscenes/Chapter1/1/1.Pre.SchoolOutside.1.json` — Day 1, Pre stage, "SchoolOutside" scene, beat 1
- `Cutscenes/Chapter1/1/1.Gym.PunchingBagFight.2.json` — Day 1, Gym, the punching-bag fight, beat 2
- `Cutscenes/Chapter1/0/0.Dream.303.1.json` — Day 0 (intro dream), the "303" (room 303) scene, beat 1
- `Cutscenes/Chapter1/4/4.BankVault.1.json` — Day 4, the bank-vault set-piece, beat 1

### Friendships

The player can **become friends** with any NPC who has a `SimpleDialogTrigger`. The mechanical payoff
is that a friend skips the conversation check (the talk-meter quicktime) — you can just talk to them.
Friendships persist in `gameData.json`, and the pause menu has a **Friends** tab listing everyone the
player has met.

Three ways to move a friendship along, from a writer's point of view:

| Where | How | Notes |
|---|---|---|
| Scene JSON | `friendPoints` on a `SimpleDialogTrigger` asset | What passing that NPC's talk meter is worth. Defaults to 1. |
| Scene JSON | `friendDialog` / `friendIsTyped` | Alternate lines for once you're friends. Leave out to reuse `dialog`. |
| Cutscene JSON | `friendship` | `name` = the NPC. `text` = points to add (may be negative). Omit `text` to make them a friend outright — for a beat where the story simply decides it. |

Three successful conversations makes a friend (`Friendship.FRIEND_THRESHOLD`), and `successTalk`
resets when a scene reloads, so the intended shape is **a few good conversations spread across the
week**, not three in a row in one room.

To branch on it, use `checkFriend` in a cutscene the way you'd use `checkCondition`:
`skips[0]` if they're a friend, `skips[1]` if not. Good for giving a friend warmer dialog, or letting
them tell the player something a stranger wouldn't.

> Friendship is keyed by the NPC's **name** — the `name` in the scene JSON, which is also the name of
> their portrait in `Resources/Heads/Heads`. Keep them consistent and the Friends tab shows a face.

### Quiz files (the multiple-choice class quiz)

Any class that tests the player uses the shared **quiz minigame** scene. The questions are authored
here, as one JSON file per quiz, at:

```
Resources/Quizzes/Chapter<N>/<dayCounter>.<Stage>[.<sceneCounter>].json
```

Same coordinates as cutscenes, so `Quizzes/Chapter1/5.Sci.json` is *the Day 5 Science pop quiz*. The
`sceneCounter` part is optional and only needed if one class holds more than one quiz.

Each file carries its own questions (four options each), clock, pass mark, intro/outro lines, the
`onFinishCutscene` that continues the story afterwards, and the condition flags to set on a pass or
fail so later cutscenes can `checkCondition` the result. **The tutorial lines live in whichever quiz
the player reaches first** — currently `2.PR.json` — and only ever play once.

Written today: `Quizzes/Chapter1/2.PR.json` (Day 2 PR test, carries the tutorial) and
`Quizzes/Chapter1/5.Sci.json` (Day 5 Science pop quiz).

**Shared cutscenes** (`common.` prefix, e.g. `common.LockedDoor`) load from
`Resources/Cutscenes/Chapter<N>/Common/`. The chapter/day path is built in
`CutsceneManager.CreateCutsceneFromTextFile` from `GameManager.chapterCounter` + `dayCounter`.

> Every cutscene now uses the correct spelling `Psych`; the old `*.Pysch.*` files were renamed.
> When renaming an asset, move the `.json` and its `.meta` together — the GUID lives in the `.meta`,
> so moving the pair keeps scene/prefab references intact (see the repo `CLAUDE.md`).

## Working conventions for these docs

- **Status markers:** `☐` planned · `✍️` drafting · `🔨` being built in Unity · `✅` implemented & playable.
- **Cross-links:** link characters as `[Name](Characters/Name.md)` and threads as
  `[Thread](PlotThreads.md#thread-anchor)` so the web stays navigable.
- **Don't invent canon silently.** Anything not yet decided is written as `[TBD]` so it's obvious
  what still needs a decision. Names pulled from existing game files are marked *(from existing content)*.
