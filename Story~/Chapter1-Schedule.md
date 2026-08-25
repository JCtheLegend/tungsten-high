# Chapter 1 — Completion Schedule (target: October 1, 2026)

A weekly checklist to take Chapter 1 from "heavily built" to **done** by October. Check items off
(`[x]`) as you go; if a week slips, pull its unfinished items into the next week rather than
rescheduling everything. Built from the actual state of `Resources/Cutscenes/Chapter1/`,
[Chapter-1.md](Chapters/Chapter-1.md), the [Roster](Characters/_Roster.md),
[PlotThreads](PlotThreads.md), and [Worldbuilding](Worldbuilding.md) as of **July 29, 2026**.

## Definition of "Chapter 1 complete"

- [ ] Every day (0–5) is playable **start to finish** from a fresh save — no dead ends, no placeholders.
- [ ] All `[TBD]` class beats in Chapter-1.md are either written & built, or consciously cut.
- [ ] The new story canon is **in the game**, not just in the docs (Bubba Day 5 battle, The Hole reveal, Sparks' letter, Slim's Day 0 presence).
- [ ] All 8 set-pieces play correctly (punching bag, lawnmower, crab, fear, race+dummy, Bubba ×2, bank vault).
- [ ] Day 5 ends on a hook into Chapter 2.
- [ ] Story files (characters / plot threads / worldbuilding) are filled in for everything Chapter 1 touches.

## Known content gaps (canon written, game content missing)

These came from diffing your story docs against the cutscene JSON — they are the *new build work*,
as opposed to polish of existing scenes:

1. **Bubba Day 5 battle** — the summary says his transfer is denied and the player witnesses "the death
   of his dream" after a battle on Day 5. Only `3.Lunch.BubbaFight` exists; **no Day 5 Bubba fight files**.
2. **Slim on Day 0** — roster says Slim first appears Week 1 Day 0, but no Day 0 cutscene features him.
   (Open question: is the **Dream Stranger** — or the removed Morpheus figure — actually Slim?)
3. **Sparks' letter** — "a letter appears out of thin air in Sparks' classroom" (the Ch.2 hook). No scene yet;
   natural home is Day 5 `Sci` (currently just `5.Sci.End`).
4. **The Hole reveal** — the vault money "mysteriously disappears, and The Hole wasn't working alone."
   Confirm the Day 4 vault scenes actually land this beat.
5. **Placeholder files** — `2.Pysch.Placeholder.json`, `holder.json`, and the empty `[TBD]` class beats
   (Day 3 Gym/Psych/Sci, Day 5 Gym/Sci).

---

## Week 0 · Jul 29 – Aug 2 — Kickoff: play it, triage it

Small week (3 days). Goal: know exactly what you have.

- [ ] **Full playthrough of Days 0–5** as they exist (use `SceneLoader`'s debug counters to jump).
      In Chapter-1.md, mark every scene ✅ (works), 🔨 (broken/rough), or ☐ (missing).
- [ ] List every bug you hit in a `Bugs` section at the bottom of Chapter-1.md.
- [ ] Decide the week's **one big open question**: what is the Bank *really*, and how do The Hole,
      Slim, and the letter connect? Write the answer into [PlotThreads §A](PlotThreads.md).

## Week 1 · Aug 3 – 9 — Story lock: villains & the mystery spine

Writing week. Lock the canon that every remaining scene depends on.

- [ ] Character profiles (from `_TEMPLATE.md`): **Slim**, **The Hole**, **Bubba** — the three the plot
      hangs on. Bubba's needs his full arc: bully → transfer denied → Day 5 battle → "death of his dream."
- [ ] Resolve in PlotThreads: is the **Dream Stranger** Slim? Sandman? Someone else? (This decides how
      Day 0/Day 1 dream scenes get revised.)
- [ ] Fill PlotThreads **Setup → Payoff ledger** for Chapter 1: quarters, vault, letter, Bubba's transfer,
      hero name.
- [ ] Write the **Day 5 finale script** on paper (in Chapter-1.md): Bubba battle setup, the fight,
      the aftermath scene, Sparks' letter, the Chapter 2 hook.

## Week 2 · Aug 10 – 16 — Story lock: friends, faculty & world rules

- [ ] Character profiles: **Micycle**, **Beamo**, **Sandman** (finish its 1 TBD), **Sparks** (incl. the
      Slim friendship backstory), **Principal Phoenix**, **Psych Teacher**.
- [ ] Quick profiles (short form is fine): Bubba's ally, Map Guy, Nurse, Mom, Dad, Goblin, PR & Gym teachers.
- [ ] Worldbuilding: write the **powers system** rules, the **Dream World rules** (how dream-walking works,
      its cost), and name the **town**. Clear the 11 `[TBD]`s or mark them post-launch.
- [ ] Protagonist.md: clear its 14 `[TBD]`s — name (or confirm nameless), what his power costs, who knows.
- [ ] Write scripts (dialog on paper) for every `[TBD]` class beat you're keeping: Day 3 Gym/Psych/Sci,
      Day 5 Gym/Sci, Day 2 Psych (replaces `2.Pysch.Placeholder`). **Cut anything you won't build.**

## Week 3 · Aug 17 – 23 — Build: Days 0 & 1

- [ ] Revise **Day 0** to match locked canon (Slim/Dream Stranger presence; what room 303 means;
      is the Gym "Caught" stealth beat staying?).
- [ ] Polish **Day 1** end-to-end: fix bugs from Week 0's list; confirm Bubba's *introduction* lands
      (canon says he's introduced Day 1 — currently he first appears Day 3).
- [ ] Playtest Days 0–1 back-to-back from a fresh save; update statuses in Chapter-1.md.

## Week 4 · Aug 24 – 30 — Build: Day 2

Biggest existing day — polish, don't rebuild.

- [ ] Replace `2.Pysch.Placeholder` with the written Psych scene; delete `holder.json` if confirmed unused.
- [ ] Play the **quarters quest** and all 6 **science puzzles**; fix what's broken.
- [ ] Verify the Day 2 **Bank scenes** foreshadow The Hole correctly under locked canon (revise dialog if needed).
- [ ] Playtest Day 2 end-to-end; update Chapter-1.md.

## Week 5 · Aug 31 – Sep 6 — Build: Day 3

- [ ] Build the written Day 3 Gym / Psych / Sci class beats.
- [ ] Polish the **HoleDisguise** sequence and the **Sandman conflict + Bubba fight** at lunch.
- [ ] Playtest the **Dodgeball match** incl. Win *and* Lose branches.
- [ ] Playtest Day 3 end-to-end; update Chapter-1.md.

## Week 6 · Sep 7 – 13 — Build: Day 4 + start Day 5

- [ ] Day 4: polish the **bank hallway → vault puzzle → fight** flow; make the vault-empty /
      "Hole wasn't working alone" reveal land.
- [ ] Day 5: build **Bubba's battle** (new fight content — an `EnemyController` subclass or reuse of an
      existing boss pattern, arena scene, `StartFight` cutscene, win state).
- [ ] Playtest Day 4 end-to-end; update Chapter-1.md.

## Week 7 · Sep 14 – 20 — Build: finish Day 5

- [ ] Build the Bubba **aftermath** scene ("death of his dream") + remaining Day 5 class beats (Gym, Sci).
- [ ] Build **Sparks' letter** scene and the **Chapter 2 hook** ending.
- [ ] Verify **ChooseHeroName** + essay turn-in reflect the week (clubs, fights, tables) where feasible.
- [ ] Playtest Day 5 end-to-end; update Chapter-1.md.

## Week 8 · Sep 21 – 27 — Full-chapter integration pass

- [ ] **Two full playthroughs of Days 0–5 from a fresh save** (one "friendly" run, one contrarian —
      lose the dodgeball match, fail talks, pick different tables). Fix everything that breaks.
- [ ] Save/load test: quit + reload at least once per day; confirm `gameData.json` restores correctly.
- [ ] Sweep the **Bugs** list in Chapter-1.md to zero (or explicitly defer).
- [ ] Migrate the most bug-prone doors to `Entrance` markers if door spawns caused issues in testing.

## Week 9 · Sep 28 – Oct 1 — Buffer & ship

- [ ] Fix whatever Week 8 shook out.
- [ ] One final clean playthrough, then build the Windows player and play *the build* (not the editor).
- [ ] Update Chapter-1.md statuses to ✅, write the Chapter 2 stub's opening hook while it's fresh.
- [ ] 🎉 **Chapter 1 done.**

---

## Weekly rhythm

- **Write before you build**: Weeks 1–2 lock story so Weeks 3–7 never stall on "what happens here?"
- End every build week with an **end-to-end playtest of that day** and a status update in Chapter-1.md —
  the doc stays the source of truth.
- If a week overflows, cut scope (a class beat can become a 2-line scene) before cutting the playtest.
