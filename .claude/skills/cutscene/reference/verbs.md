# Cutscene verb reference

Every verb is a `case` in `HandleCutscene` in `Assets/Scripts/Cutscene/CutsceneManager.cs`.
The fields below are the ones that case actually reads — anything else on the object is ignored.
Fields come from `CutsceneAction` (bottom of the same file):

```
type  name  sprite  direction  text  speed  animateOverride  choices
skip  skips  isTyped  isContinue  bars  loadObjs  moveSet  talkMeter
```

`name` is almost always resolved with `GameObject.Find(name)` — active objects only.
Positions and colors are `"x,y"` / `"r,g,b,a"` strings. Booleans may be written `true` or
`"true"`; existing files use both. An unknown `type` logs `INVALID CUTSCENE ACTION` and is skipped.

**Blocking** below means the loop waits for it. Anything non-blocking that forgets to set
`actionDone` hangs the game — relevant if you add a new verb.

---

## Dialog

| Verb | Fields | Notes |
|---|---|---|
| `dialog` | `text`, `name` (speaker, optional), `isTyped` | Blocks until the player presses Z. No `name` = narration: no portrait, no beep. `name` picks the portrait from `Resources/Heads/Heads` and the voice pitch band. |
| `dialogAndChoice` | `text`, `choices[2]`, `skips[2]`, `isTyped` | Line plus two options. Branches to `index + skips[choice] + 1`. |
| `choice` | `choices[2]`, `skips[2]` | Options with no line above them. |
| `closeDialog` | — | Hides the box. Use before movement the player should watch and before `enableMovement`. |

Exactly two choices — the box has `choice1` and `choice2` and `choiceSize` is hardcoded to 2.
Left/right select, Z confirms. Choice text is never typed out.

## Control flow

| Verb | Fields | Notes |
|---|---|---|
| `goto` | `text` | Absolute 0-based index into `Items` to run next. |
| `endCutscene` | — | Stops here. Does **not** re-enable movement on its own. |
| `chooseHeroName` | — | Opens a text box (`HeroNameEntry`) and waits until the player confirms a name; stores it in `GameManager.heroName` (saved with the game). `closeDialog` first. |
| *(any action)* | `skip` | Added after the action runs: next index is `index + skip + 1`. Only applied when > 0. |

## Progression flags

| Verb | Fields | Notes |
|---|---|---|
| `addCondition` / `removeCondition` | `name` | A string flag in `PlayerInventory.condition`, persisted in the save. |
| `checkCondition` | `name`, `skips[2]` | `skips[0]` if the flag is set, `skips[1]` if not. |
| `checkInventory` | `name` (item), `text` (threshold), `skips[2]` | `skips[0]` if count >= threshold. |
| `updateInventory` | `name`, `text` (amount, may be negative) | |
| `setSceneCounter` | `text` | Beat within the stage. |
| `setStageCounter` | `text` | Display name: `Pre` `PR` `Gym` `Lunch` `Psych` `Sci` `Post` `Dream`. Also zeroes `sceneCounter`. |
| `setDayCounter` | `text` | |

## Friendship

| Verb | Fields | Notes |
|---|---|---|
| `friendship` | `name`, `text` (points, optional) | Omit `text` to make them a friend outright. Pops the `FriendAlert` banner if it crosses the threshold. |
| `checkFriend` | `name`, `skips[2]` | `skips[0]` if a friend. |
| `talkMeter` | `name`, `talkMeter: {speed, range}`, `isContinue`, `text` | The conversation quicktime. On failure with `isContinue: true`, jumps to the absolute index in `text`; without it, ends the cutscene and hands control back. Skipped entirely for an NPC already passed or befriended. |

## Movement

| Verb | Fields | Notes |
|---|---|---|
| `move` | `name`, `text` (target `"x,y"`), `direction`, `speed`, `isContinue`, `animateOverride`, `isTyped` | Blocks until it arrives unless `isContinue`. `direction` sets the walk animation and the final facing. `animateOverride: true` suppresses the walk animation (for a character already playing something else). `isTyped: true` keeps the mover's collider enabled — by default it is turned off for the move and restored after. |
| `moveBack` | `name`, `direction`, `text` (distance), `speed` | Moves relative to the object's current position; animates with its current facing. |
| `moveSet` | `moveSet[]`, `isContinue` | A sub-list for one sequence. Supported inside: `move`, `teleport`, `wait`, `moveFace`, `animate`, `hide`, `unhide`, `disable`, `disableCollider`, `enableCollider`, `goto` (index within the sub-list). **Anything else is treated as a move and throws.** |
| `teleport` | `name`, `text` | Instant, unanimated. |
| `moveFace` | `name`, `direction` | Turn in place. On the Player only `up`/`down`/`right` exist; leave him facing left by ending a `move` with `direction: "left"`. Other objects need a `Faces` component. |
| `rotate` | `name`, `direction`, `text` (degrees), `speed`, `isContinue` | Spins the transform. |
| `addFollow` / `removeFollow` | `name` (leader), `text` (follower) | Toggles a `FollowLeader` so an NPC trails someone. |
| `disableMovement` / `enableMovement` | — | Swaps the player between `PlayerMovementController` and `CutsceneMoveableObject`. Handles `Player`, `Grid Player` and the `Hero Cell` puzzle root. |

Directions: `up` `down` `left` `right` `upLeft` `upRight` `downLeft` `downRight`.
Anything unrecognized (or missing) parses as `up`.

## Camera

| Verb | Fields | Notes |
|---|---|---|
| `moveCamera` | `text`, `speed`, `isContinue` | Detaches the Cinemachine follow target and pans to the point. Restore with `cameraFollow` afterwards. |
| `cameraFollow` | `name` | Re-attaches the camera, normally to `Player`. |
| `setCameraBounds` | `name` | Points the Cinemachine confiner at that object's `Collider2D`. |

## Visuals

| Verb | Fields | Notes |
|---|---|---|
| `startBlack` / `startWhite` | — | Cover the screen **and** cancel the scene's automatic fade-in. Use as the first action when the cutscene sets up behind a covered screen. |
| `fillFade` / `clearFade` | — | `BlackFade` to alpha 1 / 0, keeping its current tint. `fillFade` also cancels the auto fade-in. |
| `setBlack` / `setWhite` | — | Recolor the fade only, keeping alpha. |
| `fadeIn` / `fadeOut` | `name` | Fades every `SpriteRenderer` under that object, blocking until it lands (~3.4s at the default speed). **On `BlackFade` the names invert: `fadeIn` fades the screen TO black, `fadeOut` reveals the scene** — it is the sprite that fades, not the picture. Nothing to fade finishes instantly. |
| `color` | `name`, `text` (`"r,g,b,a"` 0–1) | Sets the sprite color; how a ghosted/dream-state character is made translucent. |
| `hide` / `unhide` | `name` | Toggles the `SpriteRenderer`. **Reversible — prefer this over `disable`.** |
| `disable` | `name` | `SetActive(false)`. One-way: `GameObject.Find` can never reach it again. |
| `enableChildren` | `name` | Activates every direct child. |
| `enableCollider` / `disableCollider` | `name` | |
| `enableComponents` / `disableComponents` | `name` | Every `MonoBehaviour` on the object. |
| `changeSortLayer` | `name`, `text` (layer), `speed` (y-coord override) | For walking behind/in front of scenery. |
| `changeCostume` | `text` | `""` default, `"pjs"`, `"lunch"`. Prefixes the player's animation state names. |
| `animate` | `name`, `text` (animator state), `isContinue` | Waits for the clip's length unless `isContinue`. `name: "Player"` routes to the player's animator. Unknown state names log a warning and do not wait. |

Scenes fade in automatically — a cutscene does **not** need a leading fade. Only cancel it
(`startBlack`/`startWhite`/`fillFade`) when you need to set up behind a covered screen.

Because `fadeIn`/`fadeOut` block for a few seconds, they are also the timer for anything started
with `"isContinue": true` just before them — that is the idiom for "this happens while the screen
fades".

## Objects and scene

| Verb | Fields | Notes |
|---|---|---|
| `loadObjects` | `loadObjs[]` | Spawns objects mid-cutscene using the same schema as scene JSON. |
| `editObjects` | `loadObjs[]` | Mutates objects already in the scene (position, facing, trigger filename, bolted-on assets). |
| `pickup` | `name` | Plays the player's `get_item` animation and floats the item above him. |
| `unlockDoor` | `name` | Clears `locked` on a `RoomExit`. |
| `destroy` | `name` | |
| `loadScene` | `name`, `entrance` (optional) | **`name`, not `text`.** Ends the cutscene by changing scenes. `entrance` is the `id` of an `Entrance` marker/door in the destination; blank lands at its `RoomManager.defaultEntranceId`. Use a full path (`Assets/Scenes/.../X.unity`) when two scenes share a name. Set the counters before calling it. |
| `dreamExit` | — | The dream-entrance transition animation (~5s). |
| `timeline` | `text` (path under `Resources/`) | Plays a `PlayableDirector` asset; waits for its signal. Timelines live at `Resources/Timelines/` and `Resources/Cutscenes/` root, not in a chapter folder. |
| `wait` | `text` (seconds) | |

## Combat

| Verb | Fields | Notes |
|---|---|---|
| `switchCombat` / `switchNormal` | — | Toggles `Player` ↔ `Grid Player`. |
| `startFight` | `bars` (optional) | Reveals the bars and calls `enemy.StartFight()`. |
| `showBars` / `hideBars` | `bars` | Comma-separated subset of `player`, `enemy`, `energy`; empty means all. The energy bar stays hidden until `GameManager.energyUnlocked`. |

## Audio

| Verb | Fields | Notes |
|---|---|---|
| `changeSong` | `name` | From `Resources/Music/`. |
| `stopMusic` | — | One-second fade out. |
| `playSound` | `name` | From `Resources/Sounds/`. |

## Legacy fields

`sprite` is on `CutsceneAction` but nothing reads it — the portrait comes from the speaker `name`.
