# The Player (Protagonist)

**Role:** PC (player character)
**Status in story:** 🔨 in game (playable across existing content)
**First appears:** Ch.0 · Dream

## One-liner

A Tungsten High student who can walk in and out of the **Dream World** — and *into other people* —
and who ends up pulling at the thread of a plot to kidnap his school.

## Powers / abilities

- **Power class:** `[TBD — is dream-walking a recognized class, or unique to him?]`
- **What he can do:**
  - **Dream-walking** — cross into the Dream World and back; transported there each night (`dream` stage).
  - **Walk into people** — enter a **sleeping** person's dream: by **touch** while awake (straight into their
    dreamspace), or while asleep by traveling to their **floating head** in the Dream World. His body goes into a
    **meditative state**. Inside, he can **read memories**, **talk to them** as they act out their dream, or **wake
    them up**. (Full rules: [Worldbuilding](../Worldbuilding.md#the-protagonists-gift-dream-walking).)
- **In combat:** grid-based movement with a **punch** (costs energy) and a **block/shield**; energy
  regenerates over time. (See `PlayerCombat/` — the `Grid Player` root.)
- **Skills learned during the year:** **waking himself up** from a dream (**Ch.3**); the
  **Mystic Punch**, from a special training session with the Gym/combat teacher, Mr. Brock, (**Ch.4**).
  Both pay off in the Ch.6 Day 4 final battle.
- **Limits / weaknesses:** `[TBD — the cost/danger of his power is a key stakes lever, see PlotThreads B]`

## Appearance

- Has multiple **costumes** that change his look and animations: default, **pajamas** (`pjs`, worn at
  home/dream transitions), and **lunch** outfit. (Costume = animation-name prefix in code.)
- `[TBD: name, age, design notes, colors.]`

## Personality

- **Wants:** `[TBD]`
- **Fears / flaws:** A **fear of talking to strangers** (not paralyzing, just one most people have). Faced in the Day 1 dream. `[TBD]`
- **Voice:** `[TBD]`

## Backstory

- Lives at home with **Mom** (who drives him to/from school) and a **pet goblin**. `[TBD: father? siblings?]`
- As a child he got **pinched by a crab on the beach**. The memory feeds his Fear (the Day 1 dream Crab).
- `[TBD: how long has he had his power?]` **Everyone at school knows about it:** you have to disclose your power when
  applying to a power academy.
- He sees **flashbacks** of Slim's and the school's past throughout the game, through his interactions with the [Janitor](Janitor.md), a fellow dream-walker; in them he **plays as the Janitor**. This is revealed in **Chapter 4**.

## Relationships

- **Mom** — drives him, dinners at home. `[TBD]`
- **Friends** — made through Lunch, clubs, and Dodgeball across chapters. `[TBD]`
- **Dream figures** — the **Dream Stranger** (a manifestation of his own Fear), the Stair Guardian `[TBD]`.

## Role in the plot

- **Threads:** central to [A. Central mystery](../PlotThreads.md#a-the-central-mystery--the-plot-to-kidnap-the-school)
  and [B. His power & its cost](../PlotThreads.md#b-the-protagonists-power--its-cost).
- **Arc across the 6 chapters:** `[TBD — ordinary student → uncovers the plot → confronts it]` Ends by defeating [Slim](Slim.md) in an epic final boss fight in both the real world and the Dream World (Ch.6), besting Slim despite having **weaker powers** than him. On Ch.6 Day 4 he wakes himself from Sandman's dust,
  talks the students out of their dream-potion visions of upgraded power and wakes them, beats Sandman, then Slim
  on the school roof. Ch.6 Day 5: commended, then fireworks with his friends before summer break.

## Appearances

Present throughout. In **Chapter 1** (the built chapter), day by day:

| Day | Where / what they do | Status |
|---|---|---|
| 0 | Intro dream — introduced to the Dream World (the Stair Guardian); sees a flashback of [Slim](Slim.md) and [Marcy](Marcy.md) discussing their upgrade machine. | 🔨 |
| 1 | First school day; Planner pickup; Gym punching-bag fight; picks a lunch table; Dream hub, facing his fear of talking to strangers (Crab, Fear swirl, the Dream Stranger); club signup. | 🔨 |
| 2 | PR town assignment (bank, quarters quest); Gym race + dummy fight; Science puzzles; Catworld dream. | 🔨 |
| 3 | PR bank thread + disguise; lunchroom conflict (Sandman, Bubba fight); the Dodgeball game. | 🔨 |
| 4 | The bank-vault heist set-piece. | 🔨 |
| 5 | Turns in the essay and **chooses his hero name**; final Dodgeball; psych dreamwalk. | 🔨 |
| Ch.2–6 | `[TBD]` | ☐ |

## Notes / open questions

- Decide his **name** (currently "the Player" / `GameObject.Find("Player")` in code).
- Pin down the **rules and cost** of walking into people — most of his arc hangs on it.
