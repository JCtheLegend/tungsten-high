# CLAUDE.md (repo)

This repo is the **`Assets/` folder** of the Unity project "Tungsten High" (Unity 2020.3.11f1, a 2D
top-down game). The rest of the Unity project (`ProjectSettings/`, `Packages/`, `Library/`) is not in
git, so Unity can't open or run a clone of this repo. Docs written from the Unity project root say
`Assets/...`; in this repo, drop that prefix.

On the writer's PC the project is at `/mnt/c/Users/joshu/Desktop/Unity/Tungsten High` (WSL), with a
fuller `CLAUDE.md` (code architecture) at the project root.

## Story work (the main use of web sessions)

- The story bible is **`Story~/`**: chapters (one per in-game week), characters, plot threads,
  worldbuilding. Start at `Story~/README.md`. The trailing `~` makes Unity ignore the folder, so story
  files need no `.meta` files.
- To fill it in by interviewing the writer, use the **`story-interview`** skill
  (`.claude/skills/story-interview/`, or `/story-interview`). It resumes from `Story~/Interview-Log.md`.
- Canon belongs to the writer: never invent names, motives, or plot silently. Undecided things stay `[TBD]`.

## Cutscenes

Cutscene JSON is under `Resources/Cutscenes/Chapter<N>/<day>/`, and scene population is under
`Resources/Scenes/<day>/`. Use the **`cutscene`** skill (`.claude/skills/cutscene/`) and its validator:
`python3 .claude/skills/cutscene/validate_cutscene.py <file>`. Nothing written in a web session can be
playtested, so say so, and list editor work (sprites, new rooms, Build Settings) as build tasks in
`Story~/Chapters/Chapter-<N>.md` under "Loose ends".

## Git

- In a **web/cloud session**, commit and push your work before the session ends (the sandbox is
  discarded), and tell the writer the branch name so they can merge it.
- Commit only what the task touched. Unity `.meta` files must travel with their assets.
- JSON and many `.cs` files use CRLF; match the file you're editing.
- Don't edit `.unity`, `.prefab`, or `.meta` files from a web session: those need the Unity editor.
