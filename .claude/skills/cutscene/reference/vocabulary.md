# Cutscene vocabulary

The fixed sets of names a cutscene can use. Regenerate the lists (commands at the bottom) if
something looks missing — these were taken from the project, not invented.

## Speaker portraits

`name` on a `dialog` action selects the portrait from the `Resources/Heads/Heads` sprite sheet.
An unlisted name still works but shows an empty portrait slot.

```
Bloom        Bubba        Cubert       Dad          Dreamy       Freshy
Friend       George       Gym Teacher  Handstand    Ivy          Jumper
Kid Fi       Kitchenette  Laser Boy    Marcy        Maven        Micycle
Mirror       Moe          Mom          Mr Wood      Ms Nice      Ms Sparks
Nimbus       Old Fi       Paperboy     Principal Fi Puddle       Ram
Regina       Sandman      Seagull      Seer         Slicky       Songbird
Sticky       Stilts       Wally        Young Sparks
```

Speakers used in Chapter 1 with **no** portrait on the sheet — they show an empty slot today, so
either add a head or reuse an existing character: `Bank Guy`, `Banker`, `Norbert`, `Cart Man`.

**No `name` = narration or the player's own thoughts.** The player has no portrait; his voice is
unattributed text. This is the most common `dialog` form in the game.

## Voice pitch bands

`DialogPrinter` picks the typing beep's pitch from the speaker name. A name in none of these lists
keeps whatever band the previous speaker set, so give new recurring characters a band.

- **Low** — Dad, Gym Teacher, Jumper, Bubba, Nimbus, Micycle, Moe, Maven, Sticky, Sandman, George, Ram, Stilts, Cubert
- **Mid** — Mom, Slicky, Ms Nice, Ms Sparks, Laser Boy, Mirror, Principal Fi, Old Fi, Kitchenette, Freshy, Marcy, Seer, Seagull, Handstand, Paperboy, Wally, Regina, Friend
- **High** — Bloom, Kid Fi, Songbird, Dreamy, Puddle, Young Sparks, Ivy

## Stages

`setStageCounter` takes the display name; the number is the `stage` enum index used by
`validStages` in scene JSON.

| # | `setStageCounter` | Meaning |
|---|---|---|
| 0 | `Pre` | Morning at home, commute |
| 1 | `PR` | Public Relations class |
| 2 | `Gym` | Gym class |
| 3 | `Lunch` | Cafeteria |
| 4 | `Psych` | Psychological Powers class |
| 5 | `Sci` | Science of Superpowers class |
| 6 | `Post` | After school |
| 7 | `Dream` | The Dream World that night |

## Music (`changeSong`)

`Ephemeral`, `Fight Music 1`, `Fight Music 2`, `Game Over`, `Home`, `Old`, `School`, `explosion`

## Sounds (`playSound`)

`Death`, `Well Done CCBY3`, `beep`, `car`, `car1`, `chalk`, `eraser`, `explosion`, `lightning`,
`mic`, `mower`, `mower-long`, `player-hurt`, `punch-hit`, `punch-swing`, `school-bell`, `teleport`

## Costumes (`changeCostume`)

`""` (default), `"pjs"`, `"lunch"`. These prefix the player's animation state names, so an
unrecognized costume plays nothing at all.

## Animation states (`animate`)

Per-object — each name is a state on that object's own Animator, so there is no global list.
Ones that recur in Chapter 1:

- Beds: `bed_wakeup`, `bed_sleep`, `bed_wake_calm`, `bed_get_out`
- Player: `pj_sit`, `get_item`, `sleep_spin`, `default_back`, `default_forward`, `default_side`
- Dream: `dream_bubble_grow`, `dream_bubble_shrink`, `fear_transform`, `fear_pulse`, `fear_swim_up`

Before using one, confirm it exists: check the object's Animator Controller, or

```bash
grep -rl '"name": "<Object>"' Assets/Resources/Cutscenes | xargs grep -A3 '"animate"'
```

## Directions

`up` `down` `left` `right` `upLeft` `upRight` `downLeft` `downRight` — anything else parses as `up`.

## Regenerating these lists

```bash
grep -oP '(?<=^      name: ).*' Assets/Resources/Heads/Heads.png.meta | sort   # portraits
ls Assets/Resources/Music Assets/Resources/Sounds | grep -v '\.meta$'         # audio
grep -n 'lowVoices\|midVoices\|highVoices' -A 12 Assets/Scripts/Cutscene/DialogPrinter.cs
```
