# Chapter 1 — Build Backlog

**Goal:** ship Chapter 1 as a finished product (the writer's first priority, set 2026-10-20). This replaces
the old week-by-week schedule (target Oct 1, 2026), whose build weeks mostly never ran. Rebuilt on
**2026-10-20** from [Chapter-1.md](Chapters/Chapter-1.md) (day tables + "Loose ends"), a validator run over all
231 Chapter 1 cutscenes, and `Scripts/Cutscene/CutsceneManager.cs`. Target date: `[TBD]`.

**Where each task can be done:**

| Tag | Meaning |
|---|---|
| ✍️ **Story** | Needs the writer's decision first (a `/story-interview` question). |
| 🌐 **Web** | Cutscene JSON or C# that a web session can write and validate (`python3 .claude/skills/cutscene/validate_cutscene.py <file>`), but **not playtest**. |
| 🎮 **Editor** | Needs the Unity editor on the writer's PC: scenes, prefabs, sprites, `.meta` files, Build Settings, playtesting. |

Most new scenes are 🌐 + 🎮: a web session writes the JSON, then the editor wires the trigger and playtests it.
Tick items `[x]` as they're done; keep [Chapter-1.md](Chapters/Chapter-1.md)'s status column in step.

> **Sync note:** the git repo looks **behind the writer's PC**. `Chapter-1.md` says the `*.Pysch.*` files were
> renamed to `Psych` and that `Quizzes/Chapter1/2.PR.json` exists, but the repo still has `1/1.Pysch.PsychClass.1`,
> `2/2.Pysch.1`, `2/2.Pysch.Placeholder`, `3/3.Pysch.1`, `5/5.Pysch.1`, and no `Quizzes/` folder.
> Also missing from git (2026-10-20), though the docs mark them done: the Day 4 heist build (`4.BankVault.Open`,
> `4.BankVault.Standoff`, `4.BankVault.AfterFight`, `4.GoonMind.Placeholder`, `Scenes/4/BankVault.json`, `VaultLock.Configure` /
> the `VaultCode` scene type) and the trigger for `3.Lunch.Conflict`.
> **Commit and push the PC's game work** before web sessions start building, or they'll edit stale files.
> *Web work is paused until that push (writer's call, 2026-10-20).*
>
> **PC check (2026-10-20, a session on the PC):** the Day 4 heist files, `Scenes/4/BankVault.json` and `Quizzes/Chapter1/`
> *are* on the PC, but **untracked** (with the Friendship/Planner/Quiz/SceneSingleton scripts), so they still need committing.
> Three things the docs called done were **not on the PC either**, and are now built or listed below: `VaultLock.Configure` /
> the `VaultCode` scene type (written), The Hole's `onDeathCutscene` (set), and the Bank Guy's `111111` silent-alarm line
> in `3.PR.Bank.2` (not there; see Day 3). The `*.Pysch.*` files were still misspelled on the PC too (renamed now), and the
> `3.Lunch.Conflict` trigger is now the Day 3 lunch tables (`3.Lunch.GetFood`).

## Definition of "Chapter 1 complete"

- [ ] Every day (0–5) is playable **start to finish** from a fresh save — no dead ends, no placeholders.
- [ ] Every Chapter 1 cutscene passes the validator (or its warnings are understood).
- [ ] The story canon in [Chapter-1.md](Chapters/Chapter-1.md) is **in the game**, not just in the docs.
- [ ] All set-pieces play correctly (punching bag, lawnmower, crab, Fear, race + dummy, Bubba ×2, goon's mind, The Hole, Day 5 spar).
- [ ] Day 5 ends on the hook into Chapter 2 (the envelope, then the dreamless night).
- [ ] A Windows build plays through clean (the build, not the editor).

---

## 0 · Story blockers (✍️ answer these first)

Each one blocks a scene below. Ask them via `/story-interview`.

- [ ] **Day 2 Psych:** the somni scene is decided; write its dialog (replaces `2.Pysch.Placeholder`).
- [ ] **Day 2 Gym:** what the race + dummy fight is for (purpose is `[TBD]`).
- [ ] **Day 3 Gym/Psych/Sci:** Psych's dream and its dream monsters (parked 2026-10-14); keep or cut the other class beats.
- [ ] **Day 3 dream:** what the warped town looks like and what form Fear takes.
- [ ] **Day 4 dream:** where it takes place; what Mark says to Fear.
- [ ] **Day 1 recruit scene:** what Nimbus and Jumper say when they recruit Mark after the assembly.
- [ ] **Day 5:** dialog for the principal's office debrief, the Sandy spar, Bubba's crumbling-dream fight, and Sparks' envelope scene (beats are decided; lines aren't).
- [ ] **Sprites/looks:** what the Goon, The Hole, the Sheriff, the Bank Guy, Nimbus, Jumper and Puddle look like (for the 🎮 sprite work).
- [ ] Anything not worth building: **cut it** and say so in Chapter-1.md.

## 1 · Cross-cutting systems & cleanup

- [x] 🌐 **Stage-counter bug** (fixed 2026-10-20): 14 cutscenes called `setStageCounter` with a **number**, which
      `CutsceneManager.SetStageCounter` rejects ("invalid stage": stage unchanged, scene counter still reset). Each now
      uses the stage name its number meant (`0.Dream.Gym.Caught`/`1.Dream.DreamStrangerEnd` → `Pre`, `1.Pre.SchoolOutside.3` → `PR`,
      `1.PR.PRClass.2` → `Gym`, `1.Gym.GymClass.PostFight` → `Lunch`, `1.Psych.PsychClass.4` → `Sci`, `1.Sci.Assembly` + `Bleacher2…7` → `Post`,
      `1.Post.GoToBed` → `Dream`). Also fixed: the typo'd verb in `3.Lunch.Conflict`, `name`→`text` in `3.Lunch.Conflict.Sandman`, and the
      dead `setGameCounter` in `1.Post.GoToBed`. `3.Dodgeball.Win/Lose` had nonsense values (`PR`, `Dream`) and reloaded the Gym, where
      stage Post / scene 0 replays `3.Dodgeball.1`; they now set `Post`, scene 1, and load **School Front**, so Mom's pickup (`3.Post.MomPickUp`) plays.
- [ ] 🎮 **Playtest the stage fixes**, all of Day 1 and the Day 3 Dodgeball → pickup handoff: stages that used to be silently ignored
      now change, so check each next scene fires (e.g. the Day 1 dream now starts in stage `Dream`).
- [ ] 🎮 `Scenes/3/Gym.json` registers `3.Dodgeball.Final` (Post, scene 1), which doesn't exist: write it or remove the trigger.
- [x] 🌐 **Other validator errors** (fixed 2026-10-20; every Chapter 1 cutscene now validates): `5.Psych.Dreamwalk` missing comma;
      `3.Dodgeball.4` `enableScripts` → `enableComponents` (turns on Jumper's dodgeball scripts, which was being skipped);
      `2.Post.Home.Arrive` now ends with `enableMovement`; `Common/Example.json` `loadScene` uses `name`. `2.Pre.HomeOutside.CarLeave` was a
      validator false alarm (the `loadObjects` spawns a new Dad); the validator now knows that. Deleted, with their `.meta`s (nothing referenced
      them): `2/holder.json`, `3/3.Post.PickedUp`, `4/4.BankVault.2`, `4/4.BankVault.3`.
- [ ] 🎮 Playtest those: Jumper actually plays in the Day 3 match; the player can move after `2.Post.Home.Arrive`.
- [ ] ✍️ `5.Psych.Dreamwalk` is a copy of Day 2's Goblin dreamwalk (`2.Post.Cat.Dreamwalk`, loads `CatWalkway`), not Day 5 content: rewrite or drop it.
- [x] 🌐 **Dynamic dialog** (2026-10-20): dialog and choice text fill `<Name>` (the player's name, "Mark") and `<HeroName>` (the hero name,
      falling back to his name until chosen) as they print. Before this, `<Name>` showed up literally in 7 lines. `5.PR.ChooseHeroName` now
      runs the new `chooseHeroName` action: a plain text box (`Scripts/Cutscene/HeroNameEntry.cs`, IMGUI, max 16 characters, Enter to confirm).
      Both names are saved in `gameData.json`; old saves load as "Mark" with no hero name.
- [ ] 🎮 Test it: compile, choose a name on Day 5, check a `<HeroName>` line, save + reload. Restyle the text box to match the game if wanted
      (Unity will make `HeroNameEntry.cs.meta` on import; commit it).
- [ ] ✍️ Which later lines use `<HeroName>` (ledger #15). Ms Nice's reaction after he picks one?
- [x] 🌐 **Wrong speaker labels** (fixed 2026-10-20). Speaker names never show on screen; they pick the portrait and voice pitch.
      The disguised Hole was "Ms Nice" → **The Hole** (`3.PR.HoleDisguise.1/2/3`; added to the low voice band in `DialogPrinter.cs`).
      "Gym Teacher" was standing in for: Maven (`3.Pysch.1`, `5.Pysch.1`, `5.Psych.Class.1`), Bubba (`3.Lunch.Conflict`),
      Sandman (`3.Lunch.Conflict.Sandman`), Ms Nice (`5.PR.TurnInEssay`), and narration (`5.Psych.Library.1`, now unnamed).
      Mom's lines in `4.Post.Home` were "Banker" and "Micycle". Fred stays **"Friend"**: that's his portrait's name on the `Heads` sheet.
- [x] `3.Lunch.Conflict.Sandman`: "H-he was just trying to-" is **Laser Boy** (confirmed); "Principal P" → "Principal Fi".
- [x] Same scene: Sandman's "on the first day" (it's Day 3) → "in your first week".
- [ ] 🎮 Portrait for **The Hole** (in disguise) on the `Heads` sheet; until then his slot is empty.
- [x] 🎮 **Build Settings:** added `Fights/Bank Vault.unity` (after `City Buildings/Bank Vault`), `BlockingFight`, `Dodgeball`, `Day3Puzzles`,
      `Typemaster2000` (2026-10-20, edited `ProjectSettings/EditorBuildSettings.asset` with Unity closed). `4.Post.Homework` loaded
      `TypeMaster2000`; now spelled like the scene file, `Typemaster2000`.
- [x] 🌐 **Bedtime "No" went to bed anyway** (fixed 2026-10-20): in `1–4.Post.GoToBed` the "No" answer's `enableMovement` had a stale
      `skip` (3) that landed on the sleep lines. It now jumps past the end of the file.
- [x] 🌐 **Fight exits** (2026-10-20): `hideBars` / `showBars` cutscene actions now exist (`4.BankVault.AfterFight` used `hideBars`, which was
      being skipped). The Hole's `onDeathCutscene` in `Fights/Bank Vault.unity` is set to `4.BankVault.AfterFight`. The Day 3 blocking dummy had
      none, so the Gym dead-ended after the fight: it now plays the new `3.Gym.AfterFight` (back to the Gym) → `3.Gym.Final` (Day 2's staging
      with the lines from the never-triggered `3.Gym.2`) → Lunch.
- [ ] 🎮 Playtest the fight exits: the Day 3 blocking fight returns to the Gym and lunch; The Hole's death plays the aftermath.
- [ ] 🎮 `5.Dodgeball.1` loads a scene called `DodgeballScrimmage`, which doesn't exist. Make it, or point it at `Fights/Dodgeball`
      (that would also need a `Scenes/5/DodgeballClass.json`).
- [ ] 🎮 **Sprites/portraits:** Goon, The Hole (overworld), Sheriff, Bank Guy (all on the blank `Character` prefab), plus any new cast that appears on screen (Nimbus, Jumper, Puddle).
- [x] 🎮 Renamed the five `*.Pysch.*` files to `Psych` with their `.meta` files (`git mv`, 2026-10-20), and updated the four `MentalClass.json` triggers.
- [x] 🎮 Deleted the superseded `4.BankVault.1` and `4.PR.BankVault.3` (nothing referenced them). **Kept `4.PR.Bank.Final`**: the Day 4 Bank Guy
      (`Scenes/4/Bank.json`, PR / scene 3) still triggers it. Check whether the new heist flow still reaches that scene before removing it.
- [ ] 🎮 **Morpheus** (cut): remove or repurpose the object in `Scenes/Dream/Day1Dream/80s School 2F.unity` and `0.Dream.Morpheus.json`.

## 1b · Scene wiring (found 2026-10-20; fix after the PC push)

`python3 .claude/skills/cutscene/check_scene_refs.py` checks every `Resources/Scenes/<day>/*.json` trigger against the cutscene files.
A cutscene loads only from the **current day's folder** (or `Common/` via `common.<Name>`); a missing one plays as an empty cutscene, so
the beat silently doesn't happen. Re-run it after the push, since the PC copy may already fix some of these. Found on the repo copy:

**Re-run on the PC, 2026-10-20: down from ~90 findings to 7.** What was fixed, then what's left:

- [x] 🌐 **Wrong day:** `Scenes/3/MentalClass` → `3.Psych.1`, `Scenes/5/MentalClass` → `5.Psych.1`, `Scenes/5/ScienceClass` → `5.Sci.End` ×2,
      `Scenes/5/HomeOutside` → the new `5.Pre.HomeOutside.CarLeave` ×2 and `5.Post.ArriveHome`. Left alone: `Scenes/2/PunchingBagFight` → `1.Gym.PunchingBagFight.1`,
      because Day 2 never loads that scene (its Gym is the race + dummy fight).
- [x] 🌐 **`common.` prefix:** `Scenes/2/UpstairsHallway` → `common.GoblinSleeping`; `Scenes/3/StartFight` → `common.StartFight`.
- [x] 🌐 **Lunchroom helpers, Days 2, 3, 5:** the blockers now use the shared `common.Lunch.NeedFood` / `common.Lunch.BlockLeft` / `common.Lunch.BlockBottom`
      (only `fileName` changed: the Eat/Conflict cutscenes still disable them by their `<day>.Lunch.NeedLeft/NeedBottom` names). New `3.Lunch.GetFood`
      = Day 2's with every table pointing at `3.Lunch.Conflict`.
- [x] 🌐 **Missing cutscenes, written 2026-10-20** (all validate; none playtested):
      `2.InSchool` (copy of `1.InSchool`, same hallway spot); `3.Post.Home.Arrive` and `5.Post.Home.Arrive` (Mom: "<Name>! Welcome home!");
      Day 4's downstairs trigger waited for scene 3 (unreachable) and a missing file, so it now plays the written-but-unwired `4.Post.Home` at scene 2
      (plus a `closeDialog`); `3.Gym.Final` (section 1, fight exits);
      **Day 5 morning** on Day 3's route: `5.Pre.Downstairs`, `5.Pre.HomeOutside.CarLeave` ("dodgeball tryouts after school"), a new
      `Scenes/5/SchoolOutside.json` + `5.Pre.SchoolOutside.1` ("Bye <Name>! Have a good day!"), and `5.PR.1` = the Day 3 seat + bell, then
      `5.PR.TurnInEssay`'s and `5.PR.ChooseHeroName`'s lines back to back, then on to Gym;
      **Day 5 evening:** `5.Post.MomPickUp` at School Front after tryouts ("Hi honey! How were dodgeball tryouts?"), `5.Post.ArriveHome`, and
      **the dreamless night** `5.Post.GoToBed`: bed, fade to black, "You have a good and peaceful night's sleep, without any dreams.", then
      `Thanks.unity` (end card, resets the save, back to the menu). `Scenes/5/PlayerRoom`'s bed trigger moved to scene 3, where ArriveHome leaves it.
- [x] 🌐 **Typo:** `valideStages` → `validStages` on the `Car` in `Scenes/2–5/HomeOutside.json`.
- [ ] ✍️ **Still missing, waiting on content:** `2.PR.BankGuy.Final` (Days 2–3 bank: what does the Bank Guy say?), `5.Lunch.GetFood` (needs the
      Bubba book scene), `5.Gym.Final` (needs the Sandy spar), `3.Dodgeball.Final` / `5.Dodgeball.Final` (unreachable today: write or drop).
- [ ] ✍️ Review the new lines above (Mom's greetings and pickup line, the tryouts line), and the `5.PR.1` order: is turn-in → hero name → Gym right?
      `5.PR.Ending` (unwired) is a town-assignment leftover: "I'll see you all tomorrow" (a Friday) and "you'll have to write a report".
- [ ] ✍️ `5.Sci.End` still says "You'll have a quiz at the end of the week" — on Day 5, right after that quiz.
- [ ] 🎮 Playtest Day 5 start to finish: morning route, PR, then (once built) the midday, tryouts → pickup → home → the dreamless night → `Thanks`.

## 2 · Day by day

### Day 0 — Intro dream (Rip's memory)
- [x] 🌐 Speaker fixes (section 1): Fred keeps "Friend", his portrait's name.
- [ ] 🎮 Playtest Day 0 from a fresh save.

### Day 1 — First school day
- [ ] 🌐 **New:** after the assembly, **Nimbus and Jumper recruit Mark** for Dodgeball (needs ✍️ dialog). Rewrite the club signup to match
      (`1.Post.SignUp`, `1.Post.ClubHero`, `1.Post.ClubDodgeball`, `1.Post.ClubCostume`). 🎮 place Nimbus/Jumper, wire the trigger.
- [ ] 🌐 Rework Psych (`1.Psych.PsychClass.*`, now "under construction"): Maven on walking into other people's dreams, setting up the lawnmower, and hinting Mark's nights will get interesting.
- [x] 🌐 Dream (2026-10-20): `<THE PLACE>` → **the Hub** in `1.Dream.PostCrabFight`. Norbert's "Oh, uh, hey boss!" → "Whoa, a visitor! Uh, hi! I'm
      Norbert, your somnus." (✍️ check the wording; the rest of his speech still works for a first meeting). The Dream Stranger already ends with
      "You cannot run. / You are mine" and Mark waking up, not a win, so it was left as is.
- [ ] 🎮 Confirm Bubba's Day 1 intro lands (`1.Pre.SchoolOutside.1/3`). Playtest Day 1.

### Day 2 — PR assignment, quarters, science puzzles
- [ ] 🌐 Replace `2.Pysch.Placeholder` with the **somni** scene (class meets the somni building the school-as-prison). Needs ✍️ dialog.
- [x] 🌐 Day 2 vault code (2026-10-20): `VaultCode` wasn't implemented anywhere, so it's now in `ObjectLoader` (+ `LoadedObject.code`) and
      `VaultLock.Configure(code, solvedCutscene)`; `Scenes/2/BankVault.json` sets `123456` → `2.PR.Bank.OpenVault` at PR / scenes 6–7. The same code
      path makes Day 4's `287128` entry work. Compiles (checked with `dotnet build`); 🎮 playtest both keypads.
- [ ] 🎮 Play the **quarters quest** and all 6 **science puzzles**; fix what's broken. Playtest Day 2.

### Day 3 — Dodgeball tutorial, disguise, lunchroom conflict
- [x] 🌐 Check `3.Dodgeball.*` frames the match as the **tutorial**: it does ("your last exercise of today's practice", against "one of our athletes from last year", i.e. Jumper; the roster is decided Friday).
- [ ] ✍️ Day 3 vault code via `VaultCode` (the code from the Bank Guy's house): **no code has been chosen**, and `3.PR.House.1` is a stub
      ("Go back to the bank"). Pick the code and what the paper at his house says; then it's one scene-JSON line. Also missing: the Bank Guy's
      **`111111` silent-alarm** line in `3.PR.Bank.2`, which Chapter-1.md marks done and `4.BankVault.Standoff` depends on.
- [ ] 🌐 Write the kept Gym/Psych/Sci beats (✍️ first); Psych is a placeholder ("will get programmed later").
- [ ] 🌐 + 🎮 **New dream stage** after `3.Post.GoToBed`: the **warped town**, a chase by Fear, a failed attempt to fight it ("this isn't something you can punch your way out of").
- [ ] 🎮 Polish **HoleDisguise** (reads as him scouting the bank) and the Sandman conflict + Bubba fight. Playtest Dodgeball Win *and* Lose, then Day 3.

### Day 4 — The bank heist
- [ ] 🌐 + 🎮 The vault scene shows the money and goons **vanishing into another dimension**.
- [ ] 🌐 + 🎮 **New:** hostage standoff with the goon outside the vault; police/school surround the bank; Sandman's powder through the **air vents** (🎮 vent object).
- [ ] 🌐 + 🎮 **Goon's mind:** a luxury-lifestyle dream with the code's numbers to collect, then a dream-fight with the goon for the last number (replaces `4.GoonMind.Placeholder`; `DreamHubRight` stands in today).
- [ ] 🎮 The Hole's fight uses his attack pattern (holes in money bags, coin spills). Check the Garnet Square aftermath crowd positions.
- [ ] 🌐 + 🎮 **New dream stage** after `4.Post.GoToBed`: Mark faces Fear **by talking** (a dialog confrontation, not a fight).
- [ ] 🌐 + 🎮 Optional: the enterable **town jail** with the jailed goon (ruined dream; his boss only used envelopes).
- [ ] 🎮 Playtest Day 4 end to end.

### Day 5 — Week finale
- [ ] 🌐 + 🎮 **Gym:** the Sandy spar (his powder saps Mark's energy). `5.Gym.1` is a leftover blocking-class placeholder ("that's getting developed later").
- [ ] 🌐 + 🎮 **Lunch:** Bubba steals Laser Boy's books, gets the rejection letter; Mark fights him in his **crumbling Platinum Prep** dream (earthquake); returning the books bonds Mark and Laser Boy. The Lunch row currently points at `5.Gym.1`.
- [ ] 🌐 **Psych:** Mark falls out of Bubba's dream into the Dream World with his Psych class.
- [ ] 🌐 + 🎮 **Sci:** the end-of-week quiz (shared quiz minigame; check its question file exists in the repo).
- [ ] 🌐 + 🎮 **New:** the **principal's office** debrief (midday, adult Fi).
- [ ] 🌐 + 🎮 **New:** Sparks finds the **empty envelope** after Dodgeball tryouts (it should look like The Hole's letters).
- [ ] 🌐 **New:** the **dreamless night** ending (fade to black + "You have a good and peaceful night's sleep, without any dreams.").
- [ ] 🎮 Verify ChooseHeroName + the essay turn-in. Playtest Day 5.

## 3 · Integration & ship (🎮)

- [ ] **Two full playthroughs of Days 0–5 from a fresh save** (one "friendly" run, one contrarian: lose the
      Dodgeball tutorial, fail talks, pick different tables). Fix everything that breaks.
- [ ] Save/load test: quit + reload at least once per day; confirm `gameData.json` restores correctly.
- [ ] Sweep a **Bugs** list in Chapter-1.md to zero (or explicitly defer).
- [ ] Build the Windows player and play **the build**, not the editor.
- [ ] Mark Chapter-1.md statuses ✅. 🎉 **Chapter 1 done.**

---

## Working rhythm

- **Web sessions:** take 🌐 items day by day with the `cutscene` skill; validate every file; list what needs 🎮 wiring in the commit message.
- **PC sessions:** pull, wire triggers/scenes/sprites, playtest the day, update Chapter-1.md statuses, commit, push.
- If a day overflows, cut scope (a class beat can become a 2-line scene) before cutting the playtest.
