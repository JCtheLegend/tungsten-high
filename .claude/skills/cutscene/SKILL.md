---
name: cutscene
description: Write, edit, or debug a Tungsten High cutscene — turning a prose description of dialog, movement, camera work, fades, branching or flag changes into the cutscene JSON the game actually runs, wiring its trigger, and validating it. Use whenever a task involves cutscene JSON under Assets/Resources/Cutscenes/, a new story beat, an NPC conversation that is more than a SimpleDialogTrigger, or a report that a cutscene hangs, skips wrong, or plays the wrong animation.
---

# Writing Tungsten High cutscenes

A cutscene is a JSON file that `CutsceneManager` runs as a coroutine: one action at a time,
top to bottom, waiting for each to finish. Your job is to turn a description in prose
("Micycle walks in, tells Mark about the assembly, then the player can move again") into
that list, with correct names, coordinates and control flow.

Read `reference/verbs.md` for the exact fields of every verb before writing anything
non-trivial. `reference/vocabulary.md` has the speaker/portrait, music, sound, stage and
costume names. `reference/patterns.md` has copy-ready versions of the common shapes.

## Where things live

> **Paths.** This skill lives in the git repo, whose root is the Unity project's `Assets/` folder
> (github.com/JCtheLegend/tungsten-high). Paths below are written from the **Unity project root**
> (`Assets/...`, `ProjectSettings/...`). In a clone of the repo (Claude Code on the web), drop the
> `Assets/` prefix. `ProjectSettings/` (including the Build Settings scene list) is not in the repo,
> so check build membership on the writer's PC. On the PC the project root is
> `/mnt/c/Users/joshu/Desktop/Unity/Tungsten High`; the validator is
> `Assets/.claude/skills/cutscene/validate_cutscene.py` (`.claude/skills/...` from a repo clone).
> Unity can't run on the web, so nothing written there is playtested.

| What | Where |
|---|---|
| Cutscene JSON | `Assets/Resources/Cutscenes/Chapter<N>/<day>/<day>.<Stage>.<Name>[.<n>].json` |
| Shared cutscenes | `Assets/Resources/Cutscenes/Chapter<N>/Common/<Name>.json`, referenced as `common.<Name>` |
| Scene population + triggers | `Assets/Resources/Scenes/<day>/<SceneName>.json` (day only, no chapter folder) |
| The interpreter | `Assets/Scripts/Cutscene/CutsceneManager.cs` |
| Story bible | `Assets/Story~/` — characters, plot threads, the Chapter 1 day-by-day plan |

Coordinates are the same in the story docs and in code: **chapter = week**, `dayCounter` 0–5
(0 = pre-week intro dream), `stage` = time of day/class, `sceneCounter` = beat within the stage.

## Workflow

1. **Place the beat.** Establish chapter, day, stage and scene counter, and which Unity scene
   it plays in. Check `Assets/Story~/Chapters/Chapter-<N>.md` and the character profiles so the
   beat and the voices match what is already planned. Ask only if the beat is genuinely ambiguous.
2. **Learn the room before moving anyone.** Every `name` is resolved with `GameObject.Find`, so it
   must match an object that exists **and is active** in that scene. **Never invent coordinates** —
   a move to an unreachable point hangs the game forever. Where to get real ones:

   ```bash
   grep -rl '<CutsceneName>' Assets/Resources/Scenes/       # which scene plays it, and its trigger
   ```

   That scene JSON lists every NPC and prop with its `location`, filtered by `valid*` — those are
   your character positions. For doors, spawn points and anything placed by hand, read the `.unity`
   scene: door objects carry `roomName` + `toEntranceNum`, and the scene's `Room Manager` carries
   the `entrances.Array.data[N]` spawn points (see *Changing scenes*). Neighbouring cutscenes in
   the same room are the best source of known-walkable waypoints — grep them before inventing a
   path.
3. **Draft the action list** from the rules below.
4. **Wire the trigger** (see *Getting it to play*) unless it already exists.
5. **Validate**: `python3 .claude/skills/cutscene/validate_cutscene.py <file>`. Fix everything it
   reports; it catches the failure modes that cost the most time to find in-editor.
6. **Update the story docs** if this adds or changes a playable beat.

## The rules that keep it from hanging

The main loop waits on `actionDone` after every action. Most ways to break a cutscene are ways to
leave that flag false forever — the game does not error, it just stops.

- **Wrap the array**: the file is `{ "Items": [ ... ] }`. A bare array silently loads as empty.
- **Bracket player control**: open with `disableMovement`, and every path must end in
  `enableMovement`, `loadScene`, or a fight/quiz handoff. A cutscene that ends without one leaves
  the player frozen.
- **`closeDialog` before you hand control back** or start movement the player should watch. The
  dialog box stays on screen otherwise.
- **Every `move`, `moveBack`, `rotate` and `moveCamera` needs a non-zero `speed`.** Speed 0 never
  arrives. 3 is the house walking speed.
- **Every `move` needs a `direction`.** It picks the walk animation *and* the facing the object is
  left in. A missing one silently parses as `up`.
- **Move to reachable points.** Movement is velocity toward a point until it arrives; the mover's
  collider is turned off during the move (pass `"isTyped": true` to keep it on), but the target
  still has to be somewhere it can get to.
- **`disable` is one-way.** `SetActive(false)` makes the object invisible to `GameObject.Find`, so
  nothing can ever address it again in this scene. If it needs to come back, use `hide`/`unhide`.
- **Only a subset of verbs works inside `moveSet`** (`move`, `teleport`, `wait`, `moveFace`,
  `animate`, `hide`/`unhide`, `disable`, `enable/disableCollider`, `goto`). Anything else is
  treated as a move and throws.
- **`loadScene` takes `name`, not `text`.** (`Common/Example.json` used to get this wrong; fixed 2026-10-20.)

## File hygiene

- **The JSON in this repo is CRLF.** Rewriting a file with a script that emits LF turns a
  ten-line change into a whole-file diff. Edit in place, or write bytes with `\r\n`; check with
  `git -C Assets diff --stat` and fix it if the numbers look too big for what you changed.
- **Match the surrounding style**: two-space indent, one object per action, and the existing files'
  habit of quoted booleans (`"isTyped": "true"`). Both spellings parse; consistency is the point.
- **A new JSON file needs a sibling `.meta`.** Unity writes one on the next import; if you add it
  yourself, copy the format from a neighbouring `.json.meta` with a fresh 32-hex `guid`.
- Remember the git work tree root is `Assets/`, so run git from there.

## Control flow is index arithmetic

There are no labels. Branches move the loop counter, and the counter is a **0-based index into
`Items`**.

- `goto` jumps to the absolute index in `text`. `{"type":"goto","text":"11"}` runs item 11 next.
- `checkCondition`, `checkInventory`, `checkFriend` and choices use `skips`: the next action is
  **`(index of this action) + skips[n] + 1`**. For a check, `skips[0]` is the true/present branch
  and `skips[1]` the false/absent one. For a choice, `skips[k]` matches choice `k`.
- Any action's own `skip` field is added *after* it runs: the next action is `index + skip + 1`.
- A jump past the last index simply ends the cutscene, which is the normal way to finish a branch.

The house pattern for "do it once" is a `checkCondition` that skips the whole body, with a `goto`
at the end of the body jumping over the already-done lines:

```json
{ "type": "checkCondition", "name": "GotTheMap", "skips": [7, 0] }
```

at index 1 sends a player who has the flag to index 9, and everyone else to index 2.

**Editing the middle of a file invalidates every offset that jumps across the edit.** After any
insertion or deletion, recount every `goto`, `skips` and `skip` in the file, then re-run the
validator — it walks the control flow and reports targets that no longer land where they should.

## Dialog

```json
{ "type": "dialog", "name": "Ms Nice", "text": "You must be the new one.", "isTyped": "true" }
```

- `name` is the speaker: it selects the portrait from the `Heads` sheet, picks the typing-beep
  pitch band, and positions the box away from them. It must match a sprite name in
  `reference/vocabulary.md` — an unknown name gives an empty portrait slot.
- **Omit `name` for narration and the player's own thoughts** — no portrait, no typing sound.
  This is how the player "speaks"; he has no portrait of his own. 146 lines in Chapter 1 do this.
- `isTyped: "true"` types the line out with the voice beep — use it for spoken dialog.
  `"false"` snaps the whole line up at once, for narration, signs and choice prompts.
- `<Name>` inside `text` is the player-name placeholder used by existing files.
- One thought per box; the box is small. Break long speeches into several `dialog` actions.
- Choices are **always exactly two** — `choice1`/`choice2` are all the box has:

```json
{ "type": "dialogAndChoice", "text": "Go to Bed?", "choices": ["Yes", "No"],
  "skips": [2, 0], "isTyped": "false" }
```

## Movement and staging

- `move` blocks until it arrives. `"isContinue": true` starts it and moves straight on to the next
  action — that is how several characters walk at once: give every walker but the last
  `isContinue: true`, and let the last one block so the loop waits for the group.
- `moveSet` runs a sub-list of moves for one character in sequence (walk left, then up, then
  teleport offscreen). `isContinue: true` on the `moveSet` itself makes the whole sequence run in
  parallel with what follows.
- `moveFace` turns someone in place. On the **Player** it only understands `up`, `down` and
  `right` — there is no `left` case, because the player's left is a mirrored `right`. To leave the
  player facing left, end a `move` with `"direction": "left"`.
- `teleport` is instant and unanimated — the standard way to get a character offscreen after they
  have walked out, so they are in position for the next beat.
- `animate` plays a named animator state and waits for its length; `"isContinue": true` fires it
  and continues (use it for looping or ambient states). State names are per-object — look them up
  in that object's Animator, or grep how other cutscenes animate the same object. Do not invent
  them; an unknown name logs a warning and does not wait.
- `moveCamera` detaches the camera from its follow target. Put the camera back with
  `{"type":"cameraFollow","name":"Player"}` before handing control back.

## Changing scenes

`loadScene` ends the cutscene and hands off to another scene. It takes an optional **`entrance`**:
the `id` of an `Entrance` marker in the destination (doors carry one, e.g.
`"entrance": "GarnetSquareBankEntrance"`). Without it, the player lands at the destination's
`RoomManager.defaultEntranceId`, which is frequently not the door the fiction says they used. Find ids
with `grep -A1 'propertyPath: id$' 'Assets/Scenes/<Area>/<Scene>.unity'` (doors) and
`defaultEntranceId` on the Room Manager.

**Two scenes can share a name** (`City Buildings/Bank Vault` and `Fights/Bank Vault`). `loadScene`
then resolves to whichever comes first in Build Settings, so pass the full path
(`"name": "Assets/Scenes/Fights/Bank Vault.unity"`). The scene must be in Build Settings either way.

When there's no marker where the player should be, or the beat needs exact staging, a scene change is
**two cutscenes and three edits**:

1. The departing cutscene sets the counters, fades to black, and calls `loadScene`.
2. An **arrival cutscene** in the destination positions the player:
   `startBlack` → `disableMovement` → `teleport` → `moveFace` → `fadeOut BlackFade` →
   `enableMovement`. `startBlack` is what keeps the teleport from being seen.
3. The destination's scene JSON registers that arrival cutscene with a `valid*` window matching
   the counters the departing cutscene just set.

Set `setSceneCounter`/`setStageCounter` **before** `loadScene`, and make the destination's
`valid*` ranges match them — that pairing is what makes the right arrival cutscene fire and the
right NPCs spawn.

To find where the player would normally arrive, read the destination `.unity` scene:

```bash
grep -A1 'propertyPath: entrances.Array.data' 'Assets/Scenes/<Area>/<Scene>.unity'
grep -B2 -A2 'propertyPath: toEntranceNum' 'Assets/Scenes/<Area>/<Scene>.unity'
```

The `entrances` array is indexed by a door's `toEntranceNum`, and `entranceDirs` holds the facing
as the `direction` enum: `0 right, 1 left, 2 up, 3 down`. Teleport the player to the entrance the
door they "used" points at.

## Fades, and doing two things at once

On the full-screen `BlackFade` sprite the names read backwards from what you expect:

- **`fadeIn` on `BlackFade` fades the screen TO black.** It is the sprite that fades in.
- **`fadeOut` on `BlackFade` reveals the scene.**

Both block until the alpha lands — about **3.4 seconds** at the default speed. That is the hook
for overlapping action with a fade: start the movement with `"isContinue": true` so it does not
block, then let the fade run over the top of it.

```json
{ "type": "moveSet", "isContinue": true, "moveSet": [ ... ] },
{ "type": "moveSet", "isContinue": true, "moveSet": [ ... ] },
{ "type": "fadeIn", "name": "BlackFade" },
{ "type": "loadScene", "name": "Gym" }
```

Everyone walks; the screen darkens while they do; the scene changes once it is black. Movement
still in flight when the scene changes is simply discarded, so walkers do not all have to finish.

A blocking `wait` between non-blocking starts is how you stagger a crowd — the back row leaves,
half a second passes, the front row follows.

## Getting it to play

Writing the JSON is half the job — something has to start it. In the scene's
`Assets/Resources/Scenes/<day>/<SceneName>.json`:

```json
{ "validDays": [1,1], "validStages": [0,0], "validScenes": [0,0],
  "type": "cutscene", "name": "1.Pre.PlayerRoom" }
```

auto-starts it on entering the scene. `valid*` are inclusive `[min,max]` ranges on
day / stage index / scene counter; omit one to mean "always". The alternatives are a
`CutsceneActTrigger` (press Z), a `CutsceneCollider` (walk into it), or an `EnemyController`'s
`onDeathCutscene`. Advance `setSceneCounter` at the end of a beat so the *next* cutscene's
`validScenes` window opens and this one's closes — otherwise it replays.

## Checklist before you call it done

- [ ] `{"Items": [...]}` wrapper; the file parses.
- [ ] Every `name` is an object that exists and is active in that scene at that moment.
- [ ] Every `move`/`rotate`/`moveCamera` has `speed` and (for moves) `direction`.
- [ ] Every path ends in `enableMovement`, `loadScene`, or a deliberate handoff.
- [ ] `closeDialog` before control returns.
- [ ] Every `goto`/`skips`/`skip` target recounted against the final file.
- [ ] Speaker names match portraits; narration lines have no `name`.
- [ ] `setSceneCounter`/`setStageCounter` advance the beat, and run *before* any `loadScene`.
- [ ] If it changes scenes: `loadScene` has the right `entrance` (or the destination has an arrival
      cutscene registered with a matching `valid*` window), and the scene is in Build Settings.
- [ ] The trigger exists in the scene JSON with the right `valid*` window.
- [ ] `validate_cutscene.py` is clean.
- [ ] Story docs updated if the beat is new.
