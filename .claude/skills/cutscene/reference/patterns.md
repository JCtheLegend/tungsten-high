# Cutscene patterns

Shapes lifted from cutscenes that already ship. Start from one of these rather than from a blank
file; the index numbers in the branching examples are the ones to recount after editing.

## The basic beat

Most cutscenes are this: freeze the player, say something, unfreeze, advance the counter.

```json
{
  "Items": [
    { "type": "disableMovement" },
    { "type": "moveFace", "name": "Player", "direction": "up" },
    { "type": "dialog", "name": "Ms Nice", "text": "You must be the new one.", "isTyped": "true" },
    { "type": "dialog", "text": "He doesn't know what to say.", "isTyped": "true" },
    { "type": "closeDialog" },
    { "type": "setSceneCounter", "text": "2" },
    { "type": "enableMovement" }
  ]
}
```

The unnamed line is narration/the player's thought — no portrait, no beep.

## Someone walks in, talks, walks out

Walk them in from offscreen, play the beat, walk them off, then `teleport` them somewhere harmless
so they are not standing in the doorway for the rest of the scene.

```json
{ "type": "move", "name": "Micycle", "direction": "down", "speed": 3, "text": "-1,1" },
{ "type": "moveFace", "name": "Micycle", "direction": "right" },
{ "type": "dialog", "name": "Micycle", "text": "Coach wants everyone in the gym.", "isTyped": "true" },
{ "type": "closeDialog" },
{ "type": "moveSet", "moveSet": [
    { "type": "move", "name": "Micycle", "direction": "up", "speed": 3, "text": "-1,6" },
    { "type": "teleport", "name": "Micycle", "text": "-23.5,5.5" }
] }
```

## A group moving at once

Give every walker but the last `"isContinue": true` so they start together; the last one blocks, so
the loop waits for the group to finish.

```json
{ "type": "moveSet", "isContinue": true, "moveSet": [
    { "type": "move", "name": "Handstand", "direction": "left", "speed": 3, "text": "-1,1" },
    { "type": "teleport", "name": "Handstand", "text": "-21,6.5" } ] },
{ "type": "moveSet", "isContinue": true, "moveSet": [
    { "type": "move", "name": "Bubba", "direction": "up", "speed": 3, "text": "7.5,1" },
    { "type": "move", "name": "Bubba", "direction": "left", "speed": 3, "text": "-1,1" } ] },
{ "type": "moveSet", "moveSet": [
    { "type": "move", "name": "Micycle", "direction": "up", "speed": 3, "text": "10.5,1" },
    { "type": "move", "name": "Micycle", "direction": "left", "speed": 3, "text": "-1,1" } ] },
{ "type": "wait", "text": "3" },
{ "type": "moveFace", "name": "Bubba", "direction": "up" }
```

## Yes/No prompt

The "No" branch falls straight through and uses `skip` to jump the "Yes" body. Indices are shown
as they would be in a real file — `skips: [2, 0]` at index 2 sends Yes to 5 and No to 3.

```json
2: { "type": "dialogAndChoice", "text": "Go to Bed?", "choices": ["Yes", "No"],
     "skips": [2, 0], "isTyped": "false" },
3: { "type": "closeDialog" },
4: { "type": "enableMovement", "skip": 29 },
5: { "type": "closeDialog" },
6: ... the Yes branch ...
```

## Once-only interaction

A flag guards the body; a `goto` at the end of the body jumps over the repeat lines. `skips[0]`
(flag set) must land exactly on the first repeat line.

```json
0: { "type": "disableMovement" },
1: { "type": "checkCondition", "name": "MailQuarters", "skips": [8, 0] },
2: { "type": "dialog", "text": "Are you here to pick up any mail?", "isTyped": "true" },
   ... first-time body ...
7: { "type": "updateInventory", "name": "Quarter", "text": "50" },
8: { "type": "addCondition", "name": "MailQuarters" },
9: { "type": "goto", "text": "11" },
10:{ "type": "dialog", "text": "I'm all out of quarters, buddy.", "isTyped": "true" },
11:{ "type": "dialog", "text": "Bye now!", "isTyped": "true" },
12:{ "type": "closeDialog" },
13:{ "type": "enableMovement" }
```

Note index 11 onward is shared tail for both branches — that is deliberate and normal.

## Waking up in bed

The bed owns the animation; the player is hidden, teleported into it, and revealed on the sit.

```json
{ "type": "disableCollider", "name": "Bed" },
{ "type": "hide", "name": "Player" },
{ "type": "teleport", "name": "Player", "text": "-3.1875,2.8125" },
{ "type": "disableMovement" },
{ "type": "changeCostume", "text": "pjs" },
{ "type": "animate", "name": "Bed", "text": "bed_wakeup", "isContinue": "false" },
{ "type": "dialog", "name": "Mom", "text": "Hurry up and get dressed!", "isTyped": "true" },
{ "type": "closeDialog" },
{ "type": "animate", "name": "Player", "text": "pj_sit", "isContinue": "false" },
{ "type": "unhide", "name": "Player" },
{ "type": "animate", "name": "Bed", "text": "bed_get_out", "isContinue": "false" },
{ "type": "enableCollider", "name": "Bed" },
{ "type": "changeSong", "name": "Home" },
{ "type": "setSceneCounter", "text": "1" },
{ "type": "enableMovement" }
```

## Leaving for another scene

`loadScene` ends the cutscene, so no `enableMovement` is needed — but set the counters first, or
the next scene loads into the wrong beat.

```json
{ "type": "playSound", "name": "teleport" },
{ "type": "startWhite" },
{ "type": "fadeIn", "name": "BlackFade" },
{ "type": "setStageCounter", "text": "Dream" },
{ "type": "loadScene", "name": "PreLawnmowerFight" }
```

## Clearing a room, then changing scenes

Everyone leaves at once and the screen darkens over the top of it. The walkers are non-blocking,
so the blocking `fadeIn` is what paces the shot; the `wait` staggers the two rows. Nobody needs to
reach their last waypoint — the scene change discards whatever is still walking.

```json
{ "type": "closeDialog" },
{ "type": "moveSet", "isContinue": true, "moveSet": [
    { "type": "move", "name": "Micycle", "direction": "right", "speed": 3, "text": "6.5,-7.5" },
    { "type": "move", "name": "Micycle", "direction": "down",  "speed": 3, "text": "6.5,-8" },
    { "type": "move", "name": "Micycle", "direction": "right", "speed": 3, "text": "9.5,-8" } ] },
{ "type": "moveSet", "isContinue": true, "moveSet": [ ... Bloom, same three legs ... ] },
{ "type": "wait", "text": "0.5" },
{ "type": "moveSet", "isContinue": true, "moveSet": [ ... front row ... ] },
{ "type": "moveSet", "isContinue": true, "moveSet": [ ... the Player ... ] },
{ "type": "fadeIn", "name": "BlackFade" },
{ "type": "setSceneCounter", "text": "1" },
{ "type": "loadScene", "name": "Gym" }
```

Route everyone through the same axis-aligned legs — into a lane, along it, out the door — rather
than one diagonal move. A mover's collider is off for the duration, so the legs only have to look
right, but reuse waypoints the player is known to walk through.

## Arriving in the new scene

The other half of the pair. Use it when `loadScene`'s `entrance` can't put the player where the beat needs
them (no marker there, or exact staging).
`startBlack` holds the cover so the teleport is never seen.

```json
{
  "Items": [
    { "type": "startBlack" },
    { "type": "disableMovement" },
    { "type": "teleport", "name": "Player", "text": "18,-12" },
    { "type": "moveFace", "name": "Player", "direction": "up" },
    { "type": "fadeOut", "name": "BlackFade" },
    { "type": "enableMovement" }
  ]
}
```

Register it in the destination's scene JSON with a window matching the counters the departing
cutscene set:

```json
{ "validScenes": [1,1], "validStages": [5,5], "type": "cutscene", "name": "1.Sci.GymArrive" }
```

The teleport target comes from the destination's `Room Manager` `entrances` array — the same
spawn point the door into that room uses.

## Setting up behind a covered screen

Scenes fade in by themselves, so only do this when the first frame must not be seen.

```json
{ "type": "startBlack" },
{ "type": "disableMovement" },
{ "type": "teleport", "name": "Player", "text": "0,-3.375" },
{ "type": "moveCamera", "text": "0,-3", "speed": 8, "isContinue": true },
{ "type": "fadeOut", "name": "BlackFade" },
{ "type": "cameraFollow", "name": "Player" }
```

## Conversation check before a chat

`talkMeter` first, with `isContinue: true` and `text` as the absolute index of the failure branch.
Passing it earns friendship points; a friend skips the check entirely.

```json
0: { "type": "talkMeter", "name": "Bloom", "talkMeter": { "speed": "3", "range": "0.4" },
     "isContinue": true, "text": "15" },
1: { "type": "disableMovement" },
   ... the successful conversation ...
14:{ "type": "goto", "text": "17" },
15:{ "type": "setSceneCounter", "text": "2" },
16:{ "type": "enableMovement" }
```

## Triggers, in the scene JSON

In `Assets/Resources/Scenes/<day>/<SceneName>.json`:

```json
{ "validDays": [1,1], "validStages": [0,0], "validScenes": [0,0],
  "type": "cutscene", "name": "1.Pre.PlayerRoom" },

{ "validStages": [6,6], "type": "CutsceneActTrigger", "name": "1.GoToBed",
  "fileName": "1.Post.GoToBed", "location": "-4,2", "size": "2,2" }
```

`type: "cutscene"` auto-starts on entry. `CutsceneActTrigger` is press-Z, `CutsceneCollider` is
walk-into. `valid*` are inclusive `[min,max]` windows; omit for "always". `name` is the object's
name in the scene, `fileName` is the cutscene to play.
