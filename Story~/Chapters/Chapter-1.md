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
- **The PR "Bank" assignment** — Public Relations sets a multi-day project centered on a **bank** and
  the town square (**Garnet Square**), including a town-wide hunt for **quarters**, escalating to a
  **bank-vault** heist on Day 4 and a **report that is turned in** on Day 5.
- **Encounter with Bubba** — introduced Day 1 as a bully who is trying to be accepted into a better school. His resentment for weaker superheros leads him to bully others, and he fears being seen as associated with them at the school. Eventually, his transfer application is denied, and the death of his dream is witnessed by the main player after a battle (on Day 5)..

`[TBD: which beat is the first real crack in the central "plot to destroy the school/town"? The bank
thread is the obvious candidate — decide what's really going on at that bank.]`

## Threads advanced this chapter

- [A. Central mystery](../PlotThreads.md#a-the-central-mystery--the-plot-to-destroy-the-school--town): the **Bank** arc (Days 2→3→4) The money in the bank vault mysteriouly disappears, and The Hole wasn't working alone. A letter appears out of thin air in Sparks' classroom.
- [B. The protagonist's power](../PlotThreads.md#b-the-protagonists-power--its-cost): dream-walking established Day 0;
- [D. Friendships & social sim](../PlotThreads.md#d-friendships--the-social-sim): Makes friends with Micycle and Beamo, meets mentors in the teachers of the school.
- [F. Rivals & enemies](../PlotThreads.md#f-rivals--enemies): **Bubba** as the bully of the school, **The Hole** as the bank robber

## New this chapter *(from existing content)*

- **Characters:** Morpheus, the Dream Stranger, **Sandman**, **Bubba**, **Map Guy**, **Bloom**, Mom, the
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

**Notes:** `[TBD: framing — is this a recurring nightmare, a summons by Morpheus, or the inciting incident?]`

---

## Day 1 — First School Day

The complete daily loop, and the game's tutorial for movement, interaction, the Planner, combat, and
the Dream World. Files: `Resources/Cutscenes/Chapter1/1/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake up, head downstairs, Mom drives to school. | `1.Pre.PlayerRoom`, `1.Pre.Downstairs`, `1.Pre.HomeOutside.CarLeave` | 🔨 |
| Pre | School exterior | Arrive; approach the school. | `1.Pre.SchoolOutside.1/2/3` | 🔨 |
| PR | Hallway → PR class | Enter school, hallway welcome, first Public Relations class. | `1.InSchool`, `1.PR.HallwayWelcome.1/2`, `1.PR.InClass`, `1.PR.PRClass.1/2` | 🔨 |
| Gym | Gym class | Pick up the **Planner**; **punching-bag fight** (pre → fight ×3 → post). | `1.Gym.PickupPlanner`, `1.Gym.GymClass.PreFight`, `1.Gym.PunchingBagFight.1/2/3`, `1.Gym.GymClass.PostFight` | 🔨 |
| Lunch | Cafeteria | Get food (gated by `NeedFood`/`HasLunch…`), then choose a **table** (1–15) to sit and talk. | `1.Lunch.GetFood`, `1.Lunch.NeedFood`, `1.Lunch.HasLunchBottom/Left`, `1.Lunch.NeedLunchBottom/Left`, `1.Lunch.Eat.Table1`…`Table15` | 🔨 |
| Psych | Psych class | Psychological Powers class + sign-in. | `1.Psych.PsychClassSign`, `1.Pysch.PsychClass.1` *(sic — misspelled file)*, `1.Psych.PsychClass.2/3/4` | 🔨 |
| Sci | Science / Assembly | Science of Superpowers class; an **Assembly** (choose a bleacher seat). | `1.Sci.ScienceClass`, `1.Sci.Assembly`, `1.Sci.Assembly.Bleacher2`…`Bleacher7` | 🔨 |
| Post | Home & clubs | Mom pickup → home; **club signup** (Hero / Dodgeball / Costume); **lawnmower fight**; pet goblin; dinner; bed → dreamwalk. | `1.Post.MomPickUp`, `1.Post.ArriveHome`, `1.Post.InHome`, `1.Post.SignUp`, `1.Post.ClubHero`, `1.Post.ClubDodgeball`, `1.Post.ClubCostume`, `1.Post.PreLawnmowerFight`, `1.Post.PostLawnmowerFight`, `1.Post.AfterLawnmower`, `1.Post.PetGoblin`, `1.Post.AfterDinner`, `1.Post.GoToBed`, `1.Post.Dreamwalk` | 🔨 |
| Dream | Dream Hub | Key-gated hub; **crab fight**; **fear** encounter; the **Dream Stranger**. | `1.Dream.HubCenter`, `1.Dream.HubLeft`, `1.Dream.NeedKey`, `1.Dream.KeyGet`, `1.Dream.UnlockDoor`, `1.Dream.DisableDoorBox`, `1.Dream.FearSwirlPreFight`, `1.Dream.FearSwirlMove1/2`, `1.Dream.CrabFightFinish`, `1.Dream.PostCrabFight`, `1.Dream.DreamStrangerUp1/2`, `1.Dream.DreamStrangerEnd` | 🔨 |

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
| PR | Class → town | Class intro & **assignment**; get a **map**; head to **Garnet Square**, the **Bank** (+ open vault), and collect **quarters** from City Hall, the Fountain, Library, Post Office/Mail, and the Nurse. | `2.PR.ClassIntro`, `2.PR.Assignment`, `2.PR.Test`, `2.PR.Hall.GetMap`, `2.PR.FrontSteps`, `2.PR.GarnetSquare.1/2`, `2.PR.Bank.1`…`5`, `2.PR.Bank.OpenVault`, `2.PR.BankVault.Guy`, `2.PR.CityHallQuarters`, `2.PR.FountainQuarters`, `2.PR.LibraryQuarters`, `2.PR.MailQuarter`, `2.PR.NurseQuarter`, `2.PR.PostOffice.1`, `2.PR.Ending` | 🔨 |
| Gym | Track & arena | A **race** on the track, then an **dummy fight**. Energy is introduced as a fight mechanic | `2.Gym.First`, `2.Gym.Track.1`, `2.Gym.FinishRace`, `2.Gym.AfterRace`, `2.Gym.EnableFight`, `2.Gym.DummyFight`, `2.Gym.AfterFight`, `2.Gym.Final` | 🔨 |
| Lunch | Cafeteria | Get food and eat. | `2.Lunch.GetFood`, `2.Lunch.Eat` | 🔨 |
| Psych | Psych class | Explore the Dream World, the school is represented as a prison that the  | `2.Pysch.1`, `2.Pysch.Placeholder` | 🔨/☐ |
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
| PR | Class → town | Class intro & assignment; the **Bank** thread continues; **Garnet Square**; a **disguise** at a "hole"; a **house**. | `3.PR.ClassIntro`, `3.PR.Assignment`, `3.PR.Bank.1/2/3`, `3.PR.GarnetSquare.1/2`, `3.PR.HoleDisguise.1/2/3`, `3.PR.House.1`, `3.PR.Ending` | 🔨 |
| Gym | Gym class | Learn blocking from the blocking dummy fight | `3.Gym.1`, `3.Gym.2` | 🔨 |
| Lunch | Cafeteria conflict | A **conflict** with **Bubba** over Specs being teased; Sandman comes in to defuse the fight. | `3.Lunch.Conflict`, `3.Lunch.Conflict.Sandman`, `3.Lunch.BubbaFight` | 🔨 |
| Psych | Psych class | Explore more of the Dream World around the school. Fight a few dream monsters [TBD] | `3.Pysch.1` | 🔨 |
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
| — | Bank set-piece | The **bank hallway** and **vault** (combination puzzle) with a scripted **fight start**. | `4.BankHallway.1`, `4.BankVault.1/2/3`, `StartFight` | 🔨 |
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
| Lunch | Lunchroom | Bubba steals Spec's books, gets a rejection letter from Platinum prep. The player enters his dream and battles him | `5.Gym.1` | 🔨 |
| Psych | Psych + Library | Bubba's dream bursts, leaving him and the player in the middle of Psych class | `5.Pysch.1`, `5.Psych.Class.1`, `5.Psych.Library.1`, `5.Psych.Dreamwalk` | 🔨 |
| Sci | Science class | A pop quiz on the week's lessons, testing the player on the way powers work. | `5.Sci.End` | 🔨 |
| Dodgeball | Final match | Dodgeball round + ending. The player learns that he has made the team. | `5.Dodgeball.1`, `5.Dodgeball.End` | 🔨 |

**Notes:** **ChooseHeroName** is a real player-identity payoff for the week — a good spot to reflect the
choices made (clubs, lunch tables, fights). `[TBD: does the week end on a hook into Chapter 2?]`

---

## Fights / set-pieces this chapter *(from existing content)*

| Set-piece | Day · Stage | Opponent | Purpose | Script | Status |
|---|---|---|---|---|---|
| Punching-bag fight | 1 · Gym | Punching bag | Combat tutorial | `PunchingBag*Controller` | 🔨 |
| Lawnmower fight | 1 · Post | Lawnmower | `[TBD]` | `LawnmowerController` | 🔨 |
| Crab fight | 1 · Dream | Crab | `[TBD]` | `CrabEnemy` | 🔨 |
| Fear encounter | 1 · Dream | "Fear" swirl | Manifested fear | — | 🔨 |
| Race + dummy fight | 2 · Gym | Dummy | `[TBD]` | — | 🔨 |
| Bubba fight | 3 · Lunch | Bubba | Rival | — | 🔨 |
| Bank-vault fight | 4 · Bank | `[TBD]` | Heist climax | `StartFight` | 🔨 |

## Loose ends to resolve

- [ ] Define **Sandman**, **Bubba**, **Map Guy**, **Bloom**, the **Nurse**, the **Bank guy** — make profiles.
- [ ] Confirm/replace placeholder files: `2.Pysch.Placeholder`, `holder.json`.
- [ ] Fill in the many `[TBD]` class beats (Gym Day 3, Psych/Sci several days).
- [ ] Fix the misspelled `1.Pysch.PsychClass.1` / `*.Pysch.*` filenames *only if safe* (watch `.meta` GUIDs).
- [ ] End Day 5 on a clear hook into Chapter 2.
