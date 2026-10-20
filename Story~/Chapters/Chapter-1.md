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
- **Dream locations:** the intro-dream school (gym, floors 2F/3F, room 303), the Dream **Hub**, **Purrgatory**.
- **Mechanics:** the **Planner** menu (Day 1 Gym pickup); melee combat; a **race** + **dummy fight**
  (Day 2 Gym); the **Science puzzle** minigame (Day 2 Sci, puzzles 1–6); **Dodgeball** matches; the
  **quarters** collectible quest; lunch/assembly seat-choice social beats; **disguise** (Day 3).

---

## Day 0 — Intro Dream *(the night before the first day)*

All `Dream` stage. First contact with the Dream World; establishes the dream-walking premise before
school starts. Files: `Resources/Cutscenes/Chapter1/0/`.

| Scene | Cutscene JSON | What it (appears to) do | Status |
|---|---|---|---|
| Stair Guardian | `0.DreamStairGuardian` | **Cubert**, "the only cubic super in the school," nominated guardian of the staircase; won't let Rip through yet. A fun cameo (may return as a townsperson). | 🔨 |
| Dream Gym | `0.Dream.Gym.1`, `0.Dream.Gym.2`, `0.Dream.Gym.Caught` | The science fair in the gym. Fred: they forgot to put their names on the poster, go find a marker. **Caught:** Rip stumbles on Paperboy (Slim), Young Sparks, and Marcy arguing about **the amplifier** ("Where is the showmanship?!"; "Slim... we promised not to..."). Marcy: "You're not supposed to be here." Mark wakes in his room. | 🔨 |
| Floor 2F | `0.Dream.2F` | Explore the 2nd floor. | 🔨 |
| 2F → 3F | `0.Dream.2F-3F.1`, `0.Dream.2F-3F.2` | Stairs/transition between floors. | 🔨 |
| Floor 3F | `0.Dream.3F.1`, `0.Dream.3F.2` | Fred: "Joe's bringing the plant" (their group's living Venus flytrap); rumor that Paperboy's team has something big this year. | 🔨 |
| Room 303 | `0.Dream.303.1`, `0.Dream.303.2` | Where Rip dozed off; Fred wakes him: "It's time for the science fair!" (No deeper significance.) | 🔨 |

**Notes:** The intro dream includes the game's first **flashback**: [Slim](../Characters/Slim.md) talking with
[Marcy](../Characters/Marcy.md), his friend and project partner, about their prototype machine that genetically
alters and upgrades powers, for the school science fair ~20 years ago. [Sparks](../Characters/Sparks.md) is present as a
student; the player doesn't realize it's her until later. The flashback takes place **in the school 20 years ago**, and
the player plays it **as the [Janitor](../Characters/Janitor.md)** (then a student), which isn't revealed until later in
the game. It's the built Day 0 sequence (`0.Dream.303.1` → … → `0.Dream.Gym.Caught`).

**Framing:** on his first night, Mark **drifts into the dream of the Janitor, Ripley "Rip" Winkle**, which is Rip's
memory of the day of the science fair, when he **stumbled across Slim and his group**. Marcy's "You're not supposed to
be here" works both ways: Rip, the student, wasn't supposed to overhear them, and Mark isn't supposed to be in Rip's
memory. In the memory, Rip's project group is **Joe** (bringing the plant) and **Fred** (who wakes him in room 303).

---

## Day 1 — First School Day

The complete daily loop, and the game's tutorial for movement, interaction, the Planner, combat, and
the Dream World. Files: `Resources/Cutscenes/Chapter1/1/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake up, head downstairs, Mom drives to school. | `1.Pre.PlayerRoom`, `1.Pre.Downstairs`, `1.Pre.HomeOutside.CarLeave` | 🔨 |
| Pre | School exterior | Arrive; [Micycle](../Characters/Micycle.md) ("Mike") zooms into Mark as a bike and introduces himself; [Handstand](../Characters/_Roster.md) (2nd year, Orientation) greets them; **[Bubba](../Characters/Bubba.md) is introduced** ("this crappy school… my application to Platinum Prep is still under review"), sneering at Mark's power. | `1.Pre.SchoolOutside.1/2/3` | 🔨 |
| PR | Hallway → PR class | Enter school, hallway welcome, first Public Relations class. | `1.InSchool`, `1.PR.HallwayWelcome.1/2`, `1.PR.InClass`, `1.PR.PRClass.1/2` | 🔨 |
| Gym | Gym class | [Sandman](../Characters/Sandman.md) ("Sandy") walks the player to class and gives him the **Planner**; **punching-bag fight** (pre → fight ×3 → post). | `1.Gym.PickupPlanner`, `1.Gym.GymClass.PreFight`, `1.Gym.PunchingBagFight.1/2/3`, `1.Gym.GymClass.PostFight` | 🔨 |
| Lunch | Cafeteria | Get food (gated by `NeedFood`/`HasLunch…`), then choose a **table** (1–15) to sit and talk. | `1.Lunch.GetFood`, `1.Lunch.NeedFood`, `1.Lunch.HasLunchBottom/Left`, `1.Lunch.NeedLunchBottom/Left`, `1.Lunch.Eat.Table1`…`Table15` | 🔨 |
| Psych | Psych class | Psychological Powers class + sign-in. | `1.Psych.PsychClassSign`, `1.Psych.PsychClass.1`, `1.Psych.PsychClass.2/3/4` | 🔨 |
| Sci | Science / Assembly | Science of Superpowers class; an **Assembly** (choose a bleacher seat). **New:** after the assembly, [Nimbus](../Characters/Nimbus.md) and **Jumper** recruit Mark to try out for Dodgeball after the assembly. | `1.Sci.ScienceClass`, `1.Sci.Assembly`, `1.Sci.Assembly.Bleacher2`…`Bleacher7` | 🔨 |
| Post | Home & clubs | Mom pickup → home; **club signup** (to be rewritten: the Hero / Dodgeball / Costume choice goes, since Nimbus and Jumper already recruited Mark for Dodgeball after the assembly); **lawnmower fight**: Dad dozed off and is stuck in a nightmare about the lawnmower; Mark's **first walk into someone else's dream outside school** ("our son is a household hero"); pet Goblin (the cat); dinner; bed → dreamwalk. | `1.Post.MomPickUp`, `1.Post.ArriveHome`, `1.Post.InHome`, `1.Post.SignUp`, `1.Post.ClubHero`, `1.Post.ClubDodgeball`, `1.Post.ClubCostume`, `1.Post.PreLawnmowerFight`, `1.Post.PostLawnmowerFight`, `1.Post.AfterLawnmower`, `1.Post.PetGoblin`, `1.Post.AfterDinner`, `1.Post.GoToBed`, `1.Post.Dreamwalk` | 🔨 |
| Dream | Dream Hub | Key-gated hub; **crab fight**; **fear** encounter; the **Dream Stranger**. The player faces his own **fear of talking to strangers**, fitting a day spent meeting new people. **Fear** (from his Inner Mind's Hollow (the Hollow)) manifests as the **Crab**, from a childhood memory of being pinched on the beach, and as the **Dream Stranger** ("You do not know me, but you see me everywhere. You cannot run."). Mark **only escapes** the Stranger; he doesn't beat it. Norbert (his somnus) helps him find the lost **key to his subconscious**, won from the Crab, but can't follow him through the door. | `1.Dream.HubCenter`, `1.Dream.HubLeft`, `1.Dream.NeedKey`, `1.Dream.KeyGet`, `1.Dream.UnlockDoor`, `1.Dream.DisableDoorBox`, `1.Dream.FearSwirlPreFight`, `1.Dream.FearSwirlMove1/2`, `1.Dream.CrabFightFinish`, `1.Dream.PostCrabFight`, `1.Dream.DreamStrangerUp1/2`, `1.Dream.DreamStrangerEnd` | 🔨 |

**Notes:** The **Planner** pickup and **punching-bag fight** are tutorials. Lunch's 15-table choice and
the Assembly's bleacher choice are the reusable "pick a seat / who do you talk to" social pattern.

---

## Day 2 — The PR Assignment, Quarters Quest & Science Puzzles

Public Relations sends the player into **town** on an assignment tied to a **bank** and
**Garnet Square**, collecting **quarters** from civic locations; Gym has a race + fight; Science is a
puzzle gauntlet; the night dream is **Purrgatory**. Files: `Resources/Cutscenes/Chapter1/2/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake, downstairs, a **lawnmower** beat, car to school. | `2.Pre.PlayerRoom`, `2.Pre.Downstairs`, `2.Pre.Lawnmow`, `2.Pre.HomeOutside.CarLeave`, `2.Pre.SchoolOutside.1` | 🔨 |
| PR | Class → town | An **ordinary tour** (no foreshadowing of The Hole). Class intro & **assignment**; get a **map**; head to **Garnet Square**, the **Bank** (+ open vault), and collect **quarters** from City Hall, the Fountain, Library, Post Office/Mail, and the Nurse. The quarters only exist to get the player exploring the square's buildings; they're handed to the Bank Guy, who puts them in the vault. | `2.PR.ClassIntro`, `2.PR.Test` (the **quiz minigame** — `Quizzes/Chapter1/2.PR.json`, which also carries the one-time quiz tutorial and exits to `2.PR.Assignment`), `2.PR.Assignment`, `2.PR.Hall.GetMap`, `2.PR.FrontSteps`, `2.PR.GarnetSquare.1/2`, `2.PR.Bank.1`…`5`, `2.PR.Bank.OpenVault`, `2.PR.BankVault.Guy`, `2.PR.CityHallQuarters`, `2.PR.FountainQuarters`, `2.PR.LibraryQuarters`, `2.PR.MailQuarter`, `2.PR.NurseQuarter`, `2.PR.PostOffice.1`, `2.PR.Ending` | 🔨 |
| Gym | Track & arena | A **race** on the track, then a **dummy fight**, three times. Mr. Brock: "You won't always be in the best condition when faced with a fight": the lesson is managing **energy** when tired or hurt (energy is introduced as a fight mechanic). | `2.Gym.First`, `2.Gym.Track.1`, `2.Gym.FinishRace`, `2.Gym.AfterRace`, `2.Gym.EnableFight`, `2.Gym.DummyFight`, `2.Gym.AfterFight`, `2.Gym.Final` | 🔨 |
| Lunch | Cafeteria | Get food and eat. | `2.Lunch.GetFood`, `2.Lunch.Eat` | 🔨 |
| Psych | Psych class | Maven takes the class into the Ephemeral Realm to meet "a couple of interesting little fellows": the **somni**, and the class learns what they do in the Dream World. Places there represent the thoughts and feelings of whoever comes, and school often feels like a prison to students (and teachers), so the somni are **constructing a prison** when the class arrives. | `2.Psych.1`, `2.Psych.Placeholder` | 🔨/☐ |
| Sci | Science puzzles | The **Science puzzle** minigame — puzzles 1–6 (uses `Science/` `HeroCell` scripts). | `2.Sci.1`, `2.Sci.Puzzle1.1/1.2/1.3`, `2.Sci.Puzzle2.1`, `2.Sci.Puzzle3.1`, `2.Sci.Puzzle4.1`, `2.Sci.Puzzle5.1`, `2.Sci.Puzzle6.1`, `2.Sci.PuzzleFinish`, `2.Sci.End` | 🔨 |
| Post | Home & Map Guy | Leave school; home & dinner; talk to **Map Guy** (with fail/retry talk beats); **Bloom**; bed → **Purrgatory** dreamwalk. | `2.Post.SchoolOutside`, `2.Post.Home.Arrive`, `2.Post.Home.Dinner`, `2.Post.MapGuy.1/2/3`, `2.Post.FailTalk`, `2.Post.TryAgain`, `2.Post.Bloom`, `2.Post.Cat.Dreamwalk`, `2.Post.GoToBed` | 🔨 |
| Dream | Purrgatory | Mark's cat **Goblin** brings him into the cats' dream world (all cats are dream-walkers; they sleep so much so they can explore it together); "Cat Central"; a walkway. | `2.Catworld.Welcome`, `2.Post.CatCentral.Goblin`, `2.Post.CatWalkway` | 🔨 |

**Loose files (day 2 root):** `PickupQuarter` (the quarter collectible cutscene, shared across the
quest). (`holder` was an unused scratch file; deleted 2026-10-20.)

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
| Sci | Science class | More puzzles. Sparks announces **a quiz at the end of the week** (Day 5). | `3.Sci.End` | 🔨 |
| Post | Home | Mom pickup / picked up; home; bed. | `3.Post.MomPickUp`, `3.Post.ArriveHome`, `3.Post.GoToBed` | 🔨 |
| Dodgeball | The match | The **Dodgeball tutorial**: a practice match (rounds 1–4, with Win/Lose branches) **against other people on the team**. | `3.Dodgeball.1/2/3/4`, `3.Dodgeball.Win`, `3.Dodgeball.Lose` | 🔨 |
| Dream | Warped town | **New:** a dream of a **warped version of the town**. Mark is **chased by Fear** and eventually **tries to fight it**, but Fear taunts him that **this isn't something you can punch your way out of**. (He faces it by **talking** on Day 4.) | — | ✍️ |

**Notes:** **Sandman** (name fits the dream motif) and **Bubba** debut here as rivals — decide whether
Sandman connects to the Dream World thread. The night's `Dream` stage (warped town, Fear can't be punched) is planned but not built.

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
| Dream | `[TBD]` | **New:** a dream this night, after the heist. Mark **stands up to Fear and faces it by talking** (the Day 3 dream showed it can't be punched). It gives him **the ability to make friends**. | — | ✍️ |

**Notes:** The Bank Vault standoff/battle takes up the whole school day, so the extra classes do not happen

---

## Day 5 — Week Finale: Choose Your Hero Name

The week resolves: PR wraps its assignment (turn in the **essay**, **choose a hero name**), a sparring match during Gym,  a final
Dodgeball beat, and a psych **dreamwalk**. Files: `Resources/Cutscenes/Chapter1/5/`.

| Stage | Scene(s) | Plot / what happens | Cutscene JSON | Status |
|---|---|---|---|---|
| Pre | Home | Wake up. | `5.Pre.PlayerRoom` | 🔨 |
| PR | Class finale | **Turn in the essay**; **choose your hero name** (customization for the human player; it also renames the save file from "Mark"; the chosen name is **used in dialog later on**); class ending. | `5.PR.TurnInEssay`, `5.PR.ChooseHeroName`, `5.PR.Ending` | 🔨 |
| Gym | Gym class | Mr. Brock sets up **sparring matches with upperclassmen**; Mark's partner is **Sandy**, who uses his powder to **sap Mark's energy** during the fight. | `5.Gym.1` | 🔨 |
| Lunch | Lunchroom | Bubba steals [Laser Boy](../Characters/Laser%20Boy.md)'s books, gets a rejection letter from Platinum prep. The player enters his dream and battles him. **Bubba's dream** is being accepted into **Platinum Prep** and making his family proud; with the rejection it's **crumbling**, so the setting is Platinum Prep in an **earthquake**, falling apart. After the fight with Bubba the dream breaks completely. Returning the books afterward is what bonds the player and Laser Boy | `5.Gym.1` | 🔨 |
| Psych | Psych + Library | Bubba's dream breaks and Mark **falls out into the Dream World**, where the rest of his Psych class is. | `5.Psych.1`, `5.Psych.Class.1`, `5.Psych.Library.1`, `5.Psych.Dreamwalk` | 🔨 |
| Sci | Science class | The end-of-week quiz Sparks announced on Day 3, on the week's lessons, testing the player on the way powers work. Runs the shared **quiz minigame** — questions in `Quizzes/Chapter1/5.Sci.json`, which exits to `5.Sci.End`. | `5.Sci.End` | 🔨 |
| Principal | Principal's office | **New:** a visit to [Principal Fiona Nix](../Characters/Phoenix.md)'s office to introduce her character, in the **middle of the day**, when Fi is an **adult** (her phoenix cycle). She wants to **debrief Mark on the bank heist** the day before: she **praises** him, says **hero work is dangerous, even for those with simple powers**, and that **working with others** keeps you safer and makes victory easier, **encouraging him to make friends** (echoing the Day 4 dream). | — | ✍️ |
| Dodgeball | Tryouts | Dodgeball tryouts + ending. The player learns that he has made the team (he always makes it, whatever happens at tryouts). | `5.Dodgeball.1`, `5.Dodgeball.End` | 🔨 |

| Sci (after class) | Sparks' classroom | **New cutscene** after Dodgeball tryouts: Sparks cleans up her room after class and **finds the empty envelope** (Slim's taunt). | — | ☐ |
| Post / Night | Home → bed | **New:** at bedtime, instead of going to the Hub, the screen **fades to black**: *"You have a good and peaceful night's sleep, without any dreams."* The chapter ends (then the `Thanks` end card, back to the menu). | `5.Post.GoToBed` | 🔨 |

**Notes:** **ChooseHeroName** gives the human player some customization (and renames the save file from "Mark").
The week ends on the **envelope** (the hook into Chapter 2), then a quiet, dreamless night.

---

## Fights / set-pieces this chapter *(from existing content)*

| Set-piece | Day · Stage | Opponent | Purpose | Script | Status |
|---|---|---|---|---|---|
| Punching-bag fight | 1 · Gym | Punching bag | Combat tutorial | `PunchingBag*Controller` | 🔨 |
| Lawnmower fight | 1 · Post | Lawnmower (in Dad's nightmare) | Mark's first walk into someone else's dream outside school: a low-stakes, at-home tutorial for the power | `LawnmowerController` | 🔨 |
| Crab fight | 1 · Dream | Crab (a form of **Fear**, from a childhood beach-pinch memory) | The player's fear of talking to strangers, in battle form | `CrabEnemy` | 🔨 |
| Fear encounter | 1 · Dream | "Fear" swirl (**Fear himself**, ruler of the Hollow; true form a ghastly, shadowy monster) | The player's own manifested fear (the Hollow, his Inner Mind's Bad Area) | — | 🔨 |
| Race + dummy fight | 2 · Gym | Dummy | `[TBD]` | — | 🔨 |
| Bubba fight | 3 · Lunch | Bubba | Rival | — | 🔨 |
| Goon's mind (dream-walk) | 4 · Bank | The goon (in his own dream; guards the last number) | Retrieve the vault code from the sleeping goon's mind: collect the numbers around his luxury-lifestyle dream, then win the last one in a fight | — | ✍️ |
| Bank-vault fight | 4 · Bank | [The Hole](../Characters/The%20Hole.md) (attacks: turns into a hole in money bags, spills coins at the player) | Heist climax | `StartFight` | 🔨 |

## Loose ends to resolve

- [ ] **Build (Day 1):** new scene after the Day 1 assembly where **Nimbus and Jumper recruit Mark** to try out for Dodgeball, and rewrite the club signup to match (replace the Hero / Dodgeball / Costume choice; `1.Post.SignUp`, `1.Post.ClubHero`, `1.Post.ClubDodgeball`, `1.Post.ClubCostume`).
- [ ] **Build (Day 3):** check `3.Dodgeball.*` frames the match as the **Dodgeball tutorial**, a practice match against Mark's own teammates.
- [x] **Build (code):** **dynamic dialog**: write `<HeroName>` in a dialog line to insert the hero name chosen on Day 5 (`<Name>` = the player's name). Added 2026-10-20; needs a test in the editor.
- [ ] Define **Sandman**, **Bubba**, **Map Guy**, **Bloom**, the **Nurse**, the **Bank guy** — make profiles. *(Bubba started; the Bank Guy is an innocent employee.)*
- [ ] **Build:** the Day 5 **empty envelope** scene (new: after Dodgeball tryouts, Sparks cleaning her room after class) should look the same as The Hole's letters, and show Sparks shocked and frightened without explaining why.
- [ ] **Build (Day 5):** new **principal's office** visit (midday, adult Fi); ~~new **dreamless night** ending~~ (built 2026-10-20 as `5.Post.GoToBed` → `Thanks`; not playtested); Bubba's dream as a **crumbling Platinum Prep** with an earthquake; Mark falls out into the Dream World with the Psych class. `5.Psych.Dreamwalk.json` (JSON fixed 2026-10-20) is a copy of Day 2's Goblin dreamwalk. `5.PR.Ending` says "see you all tomorrow" on a Friday. The Lunch row reuses `5.Gym.1`: check.
- [ ] Confirm/replace placeholder file: `2.Psych.Placeholder` (`holder.json` deleted 2026-10-20).
- [ ] Fill in the many `[TBD]` class beats (Gym Day 3, Psych/Sci several days).
- [x] Fix the misspelled `*.Pysch.*` filenames — all five renamed to `Psych` (`.json` + `.meta` moved together; actually done 2026-10-20, they were still misspelled on the PC).
- [x] End Day 5 on a clear hook into Chapter 2. *(Sparks finds the envelope; then a dreamless night.)*
- [ ] **Build (Day 3 Dream):** new dream stage after `3.Post.GoToBed`: a **warped town** scene, a chase by Fear, an attempted fight that fails (Fear: this isn't something you can punch your way out of).
- [ ] **Build (Day 4 Dream):** new dream stage after `4.Post.GoToBed`: Mark faces Fear by **talking** to it (a dialog confrontation, not a fight).
- [ ] **Build (Day 3):** in `3.Psych.1` the class is a placeholder ("go into someone's dream… will get programmed later"). Add a **Day 3 Dream** stage (content TBD).
- [ ] **Build (Day 1 Psych):** rework Psych class (`1.Psych.PsychClass.*`, currently an "under construction" area) so Maven talks about **walking into other people's dreams** (setting up the lawnmower dream-walk into Dad's nightmare that evening) and **hints that now Mark has learned to dream-walk, his sleep routine will get much more interesting**.
- [x] **Build (Day 1 Dream):** Norbert's greeting in `1.Dream.HubCenter` is now a **first meeting** ("Whoa, a visitor! Uh, hi! I'm Norbert, your somnus.", draft wording); "**<THE PLACE>**" → **the Hub** in `1.Dream.PostCrabFight` (2026-10-20). The Dream Stranger encounter already ends with Mark waking, not a win.
- [ ] **Build (Day 2 Psych):** replace `2.Psych.Placeholder` with the somni scene: the class meets the somni, learns what they do, and finds them constructing the school-as-prison.
- [x] **Build:** wrong speaker labels fixed 2026-10-20 (see the backlog). "Specs" in older notes is [Laser Boy](../Characters/Laser%20Boy.md), real name Bobby Morrison ("B-mo"); "Bobby's notebook" in `5.Psych.Library.1` is his.
- [ ] **Build:** new optional beat: the **town jail** is an enterable building in town, open most times the player is walking around town (after Day 4). The arrested goon (no name, just "the goon") laments being in jail and his **ruined dream of getting rich**, and gives the clue that whoever hired him communicated **only via envelope** (no JSON yet).
- [ ] **Build (Day 0):** the flashback exists (`0.Dream.303.1` → `0.Dream.Gym.Caught`). Fred speaks as **"Friend"** in `0.Dream.Gym.2`, `0.Dream.3F.1/2`, `0.Dream.303.1`: that's the name of his portrait on the `Heads` sheet, and speaker names never show on screen, so it stays unless the sprite is renamed. `0.Dream.Gym.Caught`'s invalid `setStageCounter` value was fixed (2026-10-20). "Ant..." was changed to "Slim..." in `0.Dream.Gym.Caught` (2026-10-14).
- [ ] **Build:** Morpheus is an unused placeholder and is cut from the story. Remove or repurpose the `Morpheus` object in `Scenes/Dream/Day1Dream/80s School 2F.unity` and `Cutscenes/Chapter1/0/0.Dream.Morpheus.json` (it plays "He did not come this way").
- [ ] **Build:** make sure the Day 4 vault scene shows the money (and the goons) **vanishing into another dimension**.
- [ ] **Build:** new Day 4 content: the **hostage standoff** with the goon outside the vault, the police/school surrounding the building, **Sandman's powder through the air vents**, and a **Dream World sequence inside the goon's mind** to retrieve the vault code (no JSON yet).
- [ ] **Build (Day 4 heist):** ~~add `Fights/Bank Vault.unity` to **Build Settings**~~ (done 2026-10-20 with `BlockingFight`, `Dodgeball`, `Day3Puzzles`, `Typemaster2000`). Sprites/portraits for the **Goon**, **The Hole** (overworld), **Sheriff**, **Bank Guy** (all use the blank `Character` prefab for now). An **air vent** object in the vault room. Design + build the **goon's mind** (`DreamHubRight` stands in): a luxury-lifestyle dream with the code's numbers placed around it to collect, and a fight with the goon for the last number (replaces `4.GoonMind.Placeholder`; needs a goon dream-fight). Check the Garnet Square aftermath crowd positions (estimated, below the bank door). Superseded prototypes: `4.BankVault.1/2/3` and `4.PR.BankVault.3` deleted (2026-10-20); `4.PR.Bank.Final` kept, because the Day 4 Bank Guy still triggers it.
- [x] **Build:** Day 4 vault code is now per-day: scene JSON `{"type": "VaultCode", "name": "Enter", "code": "287128", "fileName": "4.BankVault.Open"}` (`VaultLock.Configure`). The Hole's `onDeathCutscene` = `4.BankVault.AfterFight`. *(Both were missing from the PC; written/set 2026-10-20.)*
- [ ] **Build:** Days 2–3 vault: Day 2 now uses `123456` via `VaultCode` (2026-10-20). Day 3 still needs its code chosen (✍️; `3.PR.House.1` is a stub).
- [ ] **Plant (Day 3):** the Bank Guy, stuck in the vault (`3.PR.Bank.2`), mentions the emergency code **111111** sets off the **silent alarm** (pays off in `4.BankVault.Standoff`). *(Was marked done, but the line isn't in `3.PR.Bank.2` on the PC or in git, 2026-10-20.)*
- [ ] **Build:** check the Day 4 vault fight uses The Hole's attack pattern (hole in money bags, coin spills) and the Day 3 `HoleDisguise` scenes read as him scouting the bank.
