# Worldbuilding — Tungsten High

Everything about the setting that isn't tied to one character or one scene. When something here
becomes concrete in-game, note the scene it appears in. Undeclared canon is marked `[TBD]`.

---

## The world at a glance

- **Superheroes live openly among ordinary people.** Powers are a known, accepted part of society.
- **Gifted students attend schools that train their powers.** Tungsten High is one such school.
- **A school for the lesser gifted** Like all school systems, 
- **Public image matters.** The existence of a *Public Relations* class implies heroes are managed,
  branded, and held to a public standard — powers are as much a social/political matter as a physical one.
- **The town** Around the school, the town of Garnet is the other half of the setting and is the place where the heroes and their families live, and contains the people they protect

## The protagonist's gift: Dream-walking

The player character is unusual even among the gifted:

1. **Dream-walking** — he can cross into the **Dream World** and back. Every night (the `dream`
   stage) he is transported there.
2. **Walking into people's dreams** — he can enter *inside* other people's minds while they are asleep, gaining access to whatever occupies their mind

## The Dream World

- A **parallel of the real-world map** — the same geography, altered. Explored during the `dream` stage.
- Buildings and places in the dream world reflect what people think, . School -> Prison
- Reached by going to bed, or entering into peoples dreams, or through various gateways
- Populated by its own entities and guardians, and by manifestations of real people's inner lives
- **Dream regions:**
  - **Main Area**
  - **Dreams** People dreaming make large bubbles floating in the Main Area
  - **Inner Mind** 
    Sub Areas:
      - Good Area [Name TBD]: Full of the players' good memories, idealistic goals, and optimistic dreams come true. An enticing place can trap the player in a self-fulfilling world.
      - Bad Area [Name TBD]: Full of the players' bad memories, fears, regrets, and 
      - The Mechanism:
  - **Catworld** — A dream world that all cats share when they sleep. The reason why cats sleep all the time is so they can hang out together in Cat World

## The school: Tungsten High

The four classes are the spine of every school day. Flesh each out — teacher, what's taught,
how it feeds powers/plot:

| Class (stage) | Subject | Teacher | Notes                                                                          |
|---|---|---|--------------------------------------------------------------------------------|
| **Public Relations** (`pr`) | Managing a hero's public image | `[TBD]` | Implies heroism is a watched, branded profession.                              |
| **Gym** (`gym`) | Physical/combat training | `[TBD]` | Site of the Ch.1 punching-bag fight; where the **Planner** is first picked up. |
| **Psychological Powers** (`psych`) | Mind/perception powers | `[TBD]` | Thematically closest to the protagonist's dream/possession gift.               |
| **Science of Superpowers** (`sci`) | The science behind powers | `[TBD]` | Mainly taught through the `Science/` puzzle minigames.                         |

Other school locations *(from existing content)*: hallways (`HallwayWelcome`), the cafeteria with
numbered tables (`Lunch.Eat.Table1`–`Table15`), an assembly hall with bleachers, a **track** (Gym race,
Day 2), science **puzzle labs** (Day 2), and a **library** (Day 5).

### The town *(from existing content — the PR "Bank" assignment sends you here)*

The Public Relations class runs a multi-day town assignment, which is how the wider town gets mapped:

- **Garnet Square** — the central town plaza / hub for the assignment.
- **The Bank** (and its **vault**) — focus of the week's escalating thread; a combination-lock vault
  (`VaultLock`/`VaultButton`/`VaultNumber`). Pays off as a heist set-piece on Day 4.
- Civic buildings that each hold a **quarter** in the collectible quest: **City Hall**, a **Fountain**,
  the **Library**, the **Post Office / Mail**, the **Nurse's office**.
- Player's **home** and neighborhood (`ArriveHome`, `Downstairs`, `PlayerRoom`, `HomeOutside`).
- `[TBD: town name? how big? relationship of the town to the school?]`

### After-school life (`post`)

- **Dodgeball** — the player signs up Day 1 and plays actual **matches on Day 3 and Day 5** (with
  Win/Lose branches). Backed by the `Dodgeball/` scene scripts (`DodgeballMatchController`, etc.).
- **Home life** — the player has a home he returns to, a Mom, Dad, and a **pet cat** (`Goblin`).

## Factions & groups

Track the sides of the central conflict here as they're decided:

- **The conspiracy** — whoever is plotting to destroy the school/town. `[TBD: who, why, how]`
- **Allies** — friends and mentors who help the player.
- **The school establishment** — faculty/administration; ally, obstacle, or compromised?
- **Dream World powers** — do the dream entities have a stake in the real-world plot?

## The powers system


## Tone & themes

- **Tone:** `[TBD — e.g. earnest coming-of-age with a creeping mystery underneath?]`
- **Themes to keep pulling on:** Superpowers as a gift. The birth, death, and preservation of hopes and dreams. Escaping Reality

## Glossary / naming *(fill as canon solidifies)*

- **Tungsten High** — the school; the game's title.
- **The Dream World** — the parallel dream map.
- **Dream-walking** — the protagonist's power to enter/leave the Dream World.
- **Morpheus / Dream Stranger / Stair Guardian / Fear** — dream entities (see above).
- **The Planner** — the in-game menu/journal item, picked up in Ch.1 Gym. (Also a real UI system:
  `Planner`/`PlannerPage` in code.)
