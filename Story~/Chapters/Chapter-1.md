# Chapter 1 — "Welcome to Tungsten High"

> Code coordinate: `chapterCounter = 1` → `Resources/Cutscenes/Chapter1/`. Days are `dayCounter` 0–5:
> **Day 0** = the pre-week intro dream, **Days 1–5** = the school week. Scene geometry JSON is under
> `Resources/Scenes/<dayCounter>/`.

**When in the year:** The first played week — the game's opening chapter.
**Status:** 🔨 heavily built (all six days have cutscene content; see per-day tables).

## Chapter summary

The **first full week at Tungsten High**, and the chapter that establishes every system the game runs
on. It opens the night before term with an **intro dream** (Day 0), then walks the player through the
daily loop — four classes, lunch, after-school life, and a nightly return to the Dream World.

Two throughlines run across the week:
- **The PR "Bank" assignment** — Public Relations has the students **help out at a building in town** on
  Days 2–4. The player's (with his partner [Micycle](../Characters/Micycle.md)) is the **bank**, near the town square (**Garnet Square**), including a town-wide hunt for **quarters**, escalating to a
  **bank-vault** heist on Day 4 and a **report that is turned in** on Day 5.
- **Encounter with Bubba** — introduced Day 1 as a bully who is trying to be accepted into a better school. His resentment for weaker superheros leads him to bully others, and he fears being seen as associated with them at the school. Eventually, his transfer application is denied, and the death of his dream is witnessed by the main player after a battle (on Day 5)..

**Slim is not revealed this chapter.** The player only gets clues: the **Day 0 flashback** of
[Slim](../Characters/Slim.md) and [Marcy](../Characters/Marcy.md), signs that someone is pulling the strings of the bank
heist (the vault money is **turned into another dimension**), and signs that someone is watching the school
(the **envelope** in Sparks' classroom). Slim needs the money to work on the machine, hire goons, and finance
his plan. [The Hole](../Characters/The%20Hole.md) doesn't know who he's working for; he's guided by **letters that
appear out of thin air**.

## Threads advanced this chapter

- [A. Central mystery](../PlotThreads.md#a-the-central-mystery--the-plot-to-kidnap-the-school): the **Bank** arc (Days 2→3→4) The money in the bank vault mysteriously disappears (turned into another dimension by Slim, unrevealed), and The Hole wasn't working alone. A letter/envelope appears out of thin air in Sparks' classroom, a sign someone is watching the school. It's **empty**: Slim's taunt, and his message that **he has returned**. [Sparks](../Characters/Sparks.md) knows what it means, because Slim was called **"Paperboy"** in school. She's visibly **shocked and frightened**, but doesn't tell the class or the player why. The letters are slipped in from a lower dimension, and the player may notice that The Hole's letters and Sparks' envelope look the same. The Day 0 flashback introduces [Slim](../Characters/Slim.md) and the upgrade machine (see [Flashbacks](../PlotThreads.md#flashbacks)).
- [B. The protagonist's power](../PlotThreads.md#b-the-protagonists-power--its-cost): dream-walking established Day 0;
- [D. Friendships & social sim](../PlotThreads.md#d-friendships--the-social-sim): Makes friends with [Micycle](../Characters/Micycle.md) (his PR partner for the week) and [Laser Boy](../Characters/Laser%20Boy.md), meets mentors in the teachers of the school.
- [F. Rivals & enemies](../PlotThreads.md#f-rivals--enemies): **Bubba** as the bully of the school, **The Hole** as the bank robber

## New this chapter *(from existing content)*

- **Characters:** [The Hole](../Characters/The%20Hole.md), the **Bank Guy** (innocent employee), [Slim](../Characters/Slim.md) and [Marcy](../Characters/Marcy.md) (Day 0 flashback only), the [Janitor](../Characters/Janitor.md) (the Day 0 flashback is played as him, unrevealed), the Dream Stranger, **Sandman**, **Bubba**, **Map Guy**, **Bloom**, Mom, the
  pet goblin, a **Nurse**, a **Bank guy**, class teachers.
- **Town locations:** **Garnet Square**, the **Bank** (+ vault), **City Hall**, a **Fountain**, the
  **Library**, the **Post Office**, a **nurse's office**.
- **Dream locations:** the intro-dream school (gym, floors 2F/3F, room 303), the Dream **Hub**, **Catworld**.
- **Mechanics:** the **Planner** menu (Day 1 Gym pickup); melee combat; a **race** + **dummy fight**
  (Day 2 Gym); the **Science puzzle** minigame (Day 2 Sci, puzzles 1–6); **Dodgeball** matches; the
  **quarters** collectible quest; lunch/assembly seat-choice social beats; **disguise** (Day 3).

---

## Day 0 — Intro Dream *(the night before the first day)*

All `Dream` stage. First contact with the Dream World; establishes the dream-walking premise before
school starts. Files: `Resources/Cutscenes/Chapter1/0/`.

| Scene | Cutscene JSON | What it (appears to) do | Status |
|---|---|---|---|
| Stair Guardian | `0.DreamStairGuardian` | A guardian gating passage up. `[TBD]` | 🔨 |
| Dream Gym | `0.Dream.Gym.1`, `0.Dream.Gym.2`, `0.Dream.Gym.Caught` | Dream version of the gym; a **"Caught"** fail/stealth state. `[TBD: stealth section?]` | 🔨 |
| Floor 2F | `0.Dream.2F` | Explore the 2nd floor. | 🔨 |
| 2F → 3F | `0.Dream.2F-3F.1`, `0.Dream.2F-3F.2` | Stairs/transition between floors. | 🔨 |
| Floor 3F | `0.Dream.3F.1`, `0.Dream.3F.2` | Explore the 3rd floor. | 🔨 |
| Room 303 | `0.Dream.303.1`, `0.Dream.303.2` | A specific room. `[TBD: why is 303 significant?]` | 🔨 |

**Notes:** The intro dream includes the game's first **flashback**: [Slim](../Characters/Slim.md) talking with
[Marcy](../Characters/Marcy.md), his friend and project partner, about their prototype machine that genetically
alters and upgrades powers, for the school science fair ~20 years ago. [Sparks](../Characters/Sparks.md) is present as a
student; the player doesn't realize it's her until later. The flashback takes place **in the school 20 years ago**, and
the player plays it **as the [Janitor](../Characters/Janitor.md)** (then a student), which isn't revealed until later in
the game. `[TBD: no cutscene JSON for it exists yet]`
`[TBD: framing: is the dream a recurring nightmare, a summons, or the inciting incident?]`

---

## Day 1 — First School Day

The complete daily loop, and the game's tutorial for movement, interaction, the Planner, combat, and
the Dream World. Files: `Resources/Cutscenes/Chapter1/1/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake up, head downstairs, Mom drives to school. | `1.Pre.PlayerRoom`, `1.Pre.Downstairs`, `1.Pre.HomeOutside.CarLeave` | 🔨 |
| Pre | School exterior | Arrive; approach the school. | `1.Pre.SchoolOutside.1/2/3` | 🔨 |
| PR | Hallway → PR class | Enter school, hallway welcome, first Public Relations class. | `1.InSchool`, `1.PR.HallwayWelcome.1/2`, `1.PR.InClass`, `1.PR.PRClass.1/2` | 🔨 |
| Gym | Gym class | [Sandman](../Characters/Sandman.md) ("Sandy") walks the player to class and gives him the **Planner**; **punching-bag fight** (pre → fight ×3 → post). | `1.Gym.PickupPlanner`, `1.Gym.GymClass.PreFight`, `1.Gym.PunchingBagFight.1/2/3`, `1.Gym.GymClass.PostFight` | 🔨 |
| Lunch | Cafeteria | Get food (gated by `NeedFood`/`HasLunch…`), then choose a **table** (1–15) to sit and talk. | `1.Lunch.GetFood`, `1.Lunch.NeedFood`, `1.Lunch.HasLunchBottom/Left`, `1.Lunch.NeedLunchBottom/Left`, `1.Lunch.Eat.Table1`…`Table15` | 🔨 |
| Psych | Psych class | Psychological Powers class + sign-in. | `1.Psych.PsychClassSign`, `1.Psych.PsychClass.1`, `1.Psych.PsychClass.2/3/4` | 🔨 |
| Sci | Science / Assembly | Science of Superpowers class; an **Assembly** (choose a bleacher seat). | `1.Sci.ScienceClass`, `1.Sci.Assembly`, `1.Sci.Assembly.Bleacher2`…`Bleacher7` | 🔨 |
| Post | Home & clubs | Mom pickup → home; **club signup** (Hero / Dodgeball / Costume); **lawnmower fight**; pet goblin; dinner; bed → dreamwalk. | `1.Post.MomPickUp`, `1.Post.ArriveHome`, `1.Post.InHome`, `1.Post.SignUp`, `1.Post.ClubHero`, `1.Post.ClubDodgeball`, `1.Post.ClubCostume`, `1.Post.PreLawnmowerFight`, `1.Post.PostLawnmowerFight`, `1.Post.AfterLawnmower`, `1.Post.PetGoblin`, `1.Post.AfterDinner`, `1.Post.GoToBed`, `1.Post.Dreamwalk` | 🔨 |
| Dream | Dream Hub | Key-gated hub; **crab fight**; **fear** encounter; the **Dream Stranger**. The player faces his own **fear of talking to strangers**, fitting a day spent meeting new people. **Fear** (from his Inner Mind's Bad Area) manifests as the **Crab**, from a childhood memory of being pinched on the beach, and as the **Dream Stranger**. | `1.Dream.HubCenter`, `1.Dream.HubLeft`, `1.Dream.NeedKey`, `1.Dream.KeyGet`, `1.Dream.UnlockDoor`, `1.Dream.DisableDoorBox`, `1.Dream.FearSwirlPreFight`, `1.Dream.FearSwirlMove1/2`, `1.Dream.CrabFightFinish`, `1.Dream.PostCrabFight`, `1.Dream.DreamStrangerUp1/2`, `1.Dream.DreamStrangerEnd` | 🔨 |

**Notes:** The **Planner** pickup and **punching-bag fight** are tutorials. Lunch's 15-table choice and
the Assembly's bleacher choice are the reusable "pick a seat / who do you talk to" social pattern.

---

## Day 2 — The PR Assignment, Quarters Quest & Science Puzzles

Public Relations sends the player into **town** on an assignment tied to a **bank** and
**Garnet Square**, collecting **quarters** from civic locations; Gym has a race + fight; Science is a
puzzle gauntlet; the night dream is **Catworld**. Files: `Resources/Cutscenes/Chapter1/2/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake, downstairs, a **lawnmower** beat, car to school. | `2.Pre.PlayerRoom`, `2.Pre.Downstairs`, `2.Pre.Lawnmow`, `2.Pre.HomeOutside.CarLeave`, `2.Pre.SchoolOutside.1` | 🔨 |
| PR | Class → town | An **ordinary tour** (no foreshadowing of The Hole). Class intro & **assignment**; get a **map**; head to **Garnet Square**, the **Bank** (+ open vault), and collect **quarters** from City Hall, the Fountain, Library, Post Office/Mail, and the Nurse. | `2.PR.ClassIntro`, `2.PR.Test` (the **quiz minigame** — `Quizzes/Chapter1/2.PR.json`, which also carries the one-time quiz tutorial and exits to `2.PR.Assignment`), `2.PR.Assignment`, `2.PR.Hall.GetMap`, `2.PR.FrontSteps`, `2.PR.GarnetSquare.1/2`, `2.PR.Bank.1`…`5`, `2.PR.Bank.OpenVault`, `2.PR.BankVault.Guy`, `2.PR.CityHallQuarters`, `2.PR.FountainQuarters`, `2.PR.LibraryQuarters`, `2.PR.MailQuarter`, `2.PR.NurseQuarter`, `2.PR.PostOffice.1`, `2.PR.Ending` | 🔨 |
| Gym | Track & arena | A **race** on the track, then an **dummy fight**. Energy is introduced as a fight mechanic | `2.Gym.First`, `2.Gym.Track.1`, `2.Gym.FinishRace`, `2.Gym.AfterRace`, `2.Gym.EnableFight`, `2.Gym.DummyFight`, `2.Gym.AfterFight`, `2.Gym.Final` | 🔨 |
| Lunch | Cafeteria | Get food and eat. | `2.Lunch.GetFood`, `2.Lunch.Eat` | 🔨 |
| Psych | Psych class | Explore the Dream World, the school is represented as a prison that the  | `2.Psych.1`, `2.Psych.Placeholder` | 🔨/☐ |
| Sci | Science puzzles | The **Science puzzle** minigame — puzzles 1–6 (uses `Science/` `HeroCell` scripts). | `2.Sci.1`, `2.Sci.Puzzle1.1/1.2/1.3`, `2.Sci.Puzzle2.1`, `2.Sci.Puzzle3.1`, `2.Sci.Puzzle4.1`, `2.Sci.Puzzle5.1`, `2.Sci.Puzzle6.1`, `2.Sci.PuzzleFinish`, `2.Sci.End` | 🔨 |
| Post | Home & Map Guy | Leave school; home & dinner; talk to **Map Guy** (with fail/retry talk beats); **Bloom**; bed → **Catworld** dreamwalk. | `2.Post.SchoolOutside`, `2.Post.Home.Arrive`, `2.Post.Home.Dinner`, `2.Post.MapGuy.1/2/3`, `2.Post.FailTalk`, `2.Post.TryAgain`, `2.Post.Bloom`, `2.Post.Cat.Dreamwalk`, `2.Post.GoToBed` | 🔨 |
| Dream | Catworld | A cat-themed dream world; your pet cat Goblin at "Cat Central"; a walkway. | `2.Catworld.Welcome`, `2.Post.CatCentral.Goblin`, `2.Post.CatWalkway` | 🔨 |

**Loose files (day 2 root):** `PickupQuarter` (the quarter collectible cutscene, shared across the
quest), `holder` (`[TBD — placeholder/scratch file? confirm and remove if unused]`).

---

## Day 3 — Dodgeball Game, Disguise & Lunchroom Conflict

Files: `Resources/Cutscenes/Chapter1/3/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake, downstairs, car to school. | `3.Pre.PlayerRoom`, `3.Pre.Downstairs`, `3.Pre.HomeOutside.CarLeave`, `3.Pre.SchoolOutside.1` | 🔨 |
| PR | Class → town | Class intro & assignment; the **Bank** thread continues; **Garnet Square**. [The Hole](../Characters/The%20Hole.md) scouts the bank **in disguise** so he isn't recognized; he wants to know when the bank will be empty so he can pull the heist. The **Bank Guy** (an innocent employee) has **forgotten the vault code** and needs it to get money out of the bank, so the player and [Micycle](../Characters/Micycle.md) (his PR partner for the week) go to **his house** to get it. | `3.PR.ClassIntro`, `3.PR.Assignment`, `3.PR.Bank.1/2/3`, `3.PR.GarnetSquare.1/2`, `3.PR.HoleDisguise.1/2/3`, `3.PR.House.1`, `3.PR.Ending` | 🔨 |
| Gym | Gym class | Learn blocking from the blocking dummy fight | `3.Gym.1`, `3.Gym.2` | 🔨 |
| Lunch | Cafeteria conflict | A **conflict** with **Bubba** over [Laser Boy](../Characters/Laser%20Boy.md) being teased ("red eyes", "cat toy"); Sandman comes in to defuse the fight. | `3.Lunch.Conflict`, `3.Lunch.Conflict.Sandman`, `3.Lunch.BubbaFight` | 🔨 |
| Psych | Psych class | Explore more of the Dream World around the school. Fight a few dream monsters [TBD] | `3.Psych.1` | 🔨 |
| Sci | Science class | More puzzles | `3.Sci.End` | 🔨 |
| Post | Home | Mom pickup / picked up; home; bed. | `3.Post.MomPickUp`, `3.Post.PickedUp`, `3.Post.ArriveHome`, `3.Post.GoToBed` | 🔨 |
| Dodgeball | The match | The Dodgeball **game**: rounds 1–4, with Win/Lose branches. | `3.Dodgeball.1/2/3/4`, `3.Dodgeball.Win`, `3.Dodgeball.Lose` | 🔨 |

**Notes:** **Sandman** (name fits the dream motif) and **Bubba** debut here as rivals — decide whether
Sandman connects to the Dream World thread. No `Dream` stage this night (the Dodgeball game is the
day's set-piece instead).

---

## Day 4 — The Bank Vault (heist climax of the week)

The routine breaks: the **Bank** thread pays off as an off-campus **vault** set-piece with a scripted
fight. Uses the vault mechanics (`VaultLock`/`VaultButton`/`VaultNumber`/`VaultReset`).
Files: `Resources/Cutscenes/Chapter1/4/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake, downstairs. | `4.Pre.PlayerRoom`, `4.Pre.Downstairs` | 🔨 |
| PR | Class → bank | Class intro & assignment; **Garnet Square**; the **Bank** finale; the vault. | `4.PR.ClassIntro`, `4.PR.Assignment`, `4.PR.GarnetSquare.1`, `4.PR.Bank.1`, `4.PR.Bank.Final`, `4.PR.BankVault.3` | 🔨 |
| — | Bank set-piece | 1. **The heist (before the employee arrives):** The Hole, following Slim's letters, makes a hole in the vault so **goons** can carry the money out in bags. The goons and the money **vanish into another dimension**; nothing comes back out. Confused, The Hole goes into the vault to investigate. 2. **The hostage standoff:** when the player, **Mike** ([Micycle](../Characters/Micycle.md)), and the **Bank Guy** enter the room outside the vault (through the **bank hallway**), they see The Hole talking with the **goon** posted there, wondering why the other goons are taking so long; The Hole goes into the vault through his power to check on them himself. The goon sees the group, tells them to **freeze** and not make trouble while the heist is on, and **pulls a gun**. Mike and the Bank Guy are scared and do as he says; the player stays mostly silent. The Bank Guy hints to the player to enter **the silent-alarm code** (all 1s) on the vault keypad; the goon lets the player try, and **mocks** him when the combination doesn't work (he's reset it). The alarm brings the police: the school and the police **surround the building**. The stalemate lasts **many hours**; the goon and his hostages grow bored and tired, and The Hole realizes he **can't escape** either, with people surrounding the building. 3. **Sandman's dust:** [Sandman](../Characters/Sandman.md) ("Sandy") is there with the school. He takes the **police megaphone**, tells everyone inside to **relax**, and, **on purpose, to help**, throws his **drowsing powder** into the room **through the air vents**. (No one else from the school is seen until the heist is over and the bank is exited.) Everyone gets sleepy. 4. **Dream-walk for the code:** the player uses the chance to navigate the **Dream World** and retrieve the **vault code from the goon's mind**: the goon had **reset the vault to a code only he knows**, so the code from Day 3 no longer works. The goon's dream is a **luxury lifestyle**, built from what he plans to spend his cut on. The player walks around it **collecting the code's numbers** from different parts of the dream; the **last number is guarded by the goon himself** and is revealed after the player **fights him**. 5. **The vault:** the player enters the code (combination puzzle) and confronts The Hole, who's been silent since he went in. He wonders where the money went and when he'll get his cut, not realizing Slim has disappeared the money and the other goons into a smaller dimension, **leaving him to be framed**. Then the **fight with [The Hole](../Characters/The%20Hole.md)**. He turns into a hole in money bags and spills coins at the player. 6. **Aftermath:** The money bags vanish mid-vault (The Hole: "That wasn't me!"). The town [Sheriff](../Characters/Sheriff.md) arrests The Hole and the outside goon; The Hole is put in **power-cancelling handcuffs** (invented by [Anti Hero](../Characters/Anti%20Hero.md)) and sent to the **town jail**. Outside the bank (Garnet Square), Ms Nice, the principal, Sandy, and the player's parents are waiting: they talk about his heroics, the dangers of superhero school, and where the money went. His parents and teachers are proud of him; they know the risks of being a hero as he becomes an adult. Then home (Post). | `4.PR.Bank.1` → `4.BankHallway.1` → `4.BankVault.Standoff` → `4.BankVault.Hours` → `4.GoonMind.Placeholder` (in `DreamHubRight`) → `4.BankVault.Wake` → keypad (`287128`) → `4.BankVault.Open` → fight → `4.BankVault.AfterFight` → `4.BankVault.Arrest` → `4.PR.GarnetSquare.Aftermath` → Post. ✍️ All written 2026-10-12; the goon's mind is a placeholder until designed. Old prototypes `4.BankVault.1/2/3`, `4.PR.BankVault.3`, `4.PR.Bank.Final` are superseded. `StartFight` | 🔨 |
| Post | Home | Home, dinner, **homework** where the player plays the typing mini game, bed. | `4.Post.ArriveHome`, `4.Post.Home`, `4.Post.Dinner`, `4.Post.Homework`, `4.Post.MomPickUp`, `4.Post.GoToBed` | 🔨 |

**Notes:** The Bank Vault standoff/battle takes up the whole school day, so the extra classes do not happen

---

## Day 5 — Week Finale: Choose Your Hero Name

The week resolves: PR wraps its assignment (turn in the **essay**, **choose a hero name**), a sparring match during Gym,  a final
Dodgeball beat, and a psych **dreamwalk**. Files: `Resources/Cutscenes/Chapter1/5/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake up. | `5.Pre.PlayerRoom` | 🔨 |
| PR | Class finale | **Turn in the essay**; **choose your hero name**; class ending. | `5.PR.TurnInEssay`, `5.PR.ChooseHeroName`, `5.PR.Ending` | 🔨 |
| Gym | Gym class | Spar with Sandman  | `5.Gym.1` | 🔨 |
| Lunch | Lunchroom | Bubba steals [Laser Boy](../Characters/Laser%20Boy.md)'s books, gets a rejection letter from Platinum prep. The player enters his dream and battles him. Returning the books afterward is what bonds the player and Laser Boy | `5.Gym.1` | 🔨 |
| Psych | Psych + Library | Bubba's dream bursts, leaving him and the player in the middle of Psych class | `5.Psych.1`, `5.Psych.Class.1`, `5.Psych.Library.1`, `5.Psych.Dreamwalk` | 🔨 |
| Sci | Science class | A pop quiz on the week's lessons, testing the player on the way powers work. Runs the shared **quiz minigame** — questions in `Quizzes/Chapter1/5.Sci.json`, which exits to `5.Sci.End`. | `5.Sci.End` | 🔨 |
| Dodgeball | Final match | Dodgeball round + ending. The player learns that he has made the team. | `5.Dodgeball.1`, `5.Dodgeball.End` | 🔨 |

**Notes:** **ChooseHeroName** is a real player-identity payoff for the week — a good spot to reflect the
choices made (clubs, lunch tables, fights). `[TBD: does the week end on a hook into Chapter 2?]`

---

## Fights / set-pieces this chapter *(from existing content)*

| Set-piece | Day · Stage | Opponent | Purpose | Script | Status |
|---|---|---|---|---|---|
| Punching-bag fight | 1 · Gym | Punching bag | Combat tutorial | `PunchingBag*Controller` | 🔨 |
| Lawnmower fight | 1 · Post | Lawnmower | `[TBD]` | `LawnmowerController` | 🔨 |
| Crab fight | 1 · Dream | Crab (Fear, from a childhood beach-pinch memory) | The player's fear of talking to strangers, in battle form | `CrabEnemy` | 🔨 |
| Fear encounter | 1 · Dream | "Fear" swirl | The player's own manifested fear (Bad Area of his Inner Mind) | — | 🔨 |
| Race + dummy fight | 2 · Gym | Dummy | `[TBD]` | — | 🔨 |
| Bubba fight | 3 · Lunch | Bubba | Rival | — | 🔨 |
| Goon's mind (dream-walk) | 4 · Bank | The goon (in his own dream; guards the last number) | Retrieve the vault code from the sleeping goon's mind: collect the numbers around his luxury-lifestyle dream, then win the last one in a fight | — | ✍️ |
| Bank-vault fight | 4 · Bank | [The Hole](../Characters/The%20Hole.md) (attacks: turns into a hole in money bags, spills coins at the player) | Heist climax | `StartFight` | 🔨 |

## Loose ends to resolve

- [ ] Define **Sandman**, **Bubba**, **Map Guy**, **Bloom**, the **Nurse**, the **Bank guy** — make profiles. *(Bubba started; the Bank Guy is an innocent employee.)*
- [ ] **Build:** the Day 5 **empty envelope** scene in Sparks' classroom should look the same as The Hole's letters, and show Sparks shocked and frightened without explaining why.
- [ ] Confirm/replace placeholder files: `2.Psych.Placeholder`, `holder.json`.
- [ ] Fill in the many `[TBD]` class beats (Gym Day 3, Psych/Sci several days).
- [x] Fix the misspelled `*.Pysch.*` filenames — all five renamed to `Psych` (`.json` + `.meta` moved together).
- [ ] End Day 5 on a clear hook into Chapter 2.
- [ ] **Build:** in `3.Lunch.Conflict` and `5.Psych.Library.1`, Bubba's lines and the narration are labelled **"Gym Teacher"** (wrong speaker name/portrait). "Specs" in older notes is [Laser Boy](../Characters/Laser%20Boy.md), real name Bobby Morrison ("B-mo"); "Bobby's notebook" in `5.Psych.Library.1` is his.
- [ ] **Build:** new optional beat: the **town jail** is an enterable building in town, open most times the player is walking around town (after Day 4). The arrested goon (no name, just "the goon") laments being in jail and his **ruined dream of getting rich**, and gives the clue that whoever hired him communicated **only via envelope** (no JSON yet).
- [ ] **Build:** add the Day 0 **Slim & Marcy flashback**, with Sparks present as a student (new cutscene; no JSON yet).
- [ ] **Build:** Morpheus is an unused placeholder and is cut from the story. Remove or repurpose the `Morpheus` object in `Scenes/Dream/Day1Dream/80s School 2F.unity` and `Cutscenes/Chapter1/0/0.Dream.Morpheus.json` (it plays "He did not come this way").
- [ ] **Build:** make sure the Day 4 vault scene shows the money (and the goons) **vanishing into another dimension**.
- [ ] **Build:** new Day 4 content: the **hostage standoff** with the goon outside the vault, the police/school surrounding the building, **Sandman's powder through the air vents**, and a **Dream World sequence inside the goon's mind** to retrieve the vault code (no JSON yet).
- [ ] **Build (Day 4 heist):** add `Fights/Bank Vault.unity` to **Build Settings**, *after* `City Buildings/Bank Vault` (cutscenes load both by full path; also missing from the build: `BlockingFight`, `Dodgeball`, `Day3Puzzles`, `Typemaster2000`). Sprites/portraits for the **Goon**, **The Hole** (overworld), **Sheriff**, **Bank Guy** (all use the blank `Character` prefab for now). An **air vent** object in the vault room. Design + build the **goon's mind** (`DreamHubRight` stands in): a luxury-lifestyle dream with the code's numbers placed around it to collect, and a fight with the goon for the last number (replaces `4.GoonMind.Placeholder`; needs a goon dream-fight). Check the Garnet Square aftermath crowd positions (estimated, below the bank door). Delete the superseded prototypes `4.BankVault.1/2/3`, `4.PR.BankVault.3`, `4.PR.Bank.Final` (they fail validation and nothing triggers them).
- [x] **Build:** Day 4 vault code is now per-day: scene JSON `{"type": "VaultCode", "name": "Enter", "code": "287128", "fileName": "4.BankVault.Open"}` (new `VaultLock.Configure`). The Hole's `onDeathCutscene` = `4.BankVault.AfterFight`.
- [ ] **Build:** Days 2–3 vault: the scene's own combination is `000000` and its solved cutscene is always `2.PR.Bank.OpenVault`. Use `VaultCode` to give Day 2 (`123456`) and Day 3 (the code from the Bank Guy's house) their own.
- [x] **Plant (Day 3):** the Bank Guy, stuck in the vault (`3.PR.Bank.2`), mentions the emergency code **111111** sets off the **silent alarm** (pays off in `4.BankVault.Standoff`).
- [ ] **Build:** check the Day 4 vault fight uses The Hole's attack pattern (hole in money bags, coin spills) and the Day 3 `HoleDisguise` scenes read as him scouting the bank.
