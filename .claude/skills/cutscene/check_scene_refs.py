#!/usr/bin/env python3
"""Check scene JSON (Resources/Scenes/<day>/*.json) against the cutscene files.

CutsceneManager loads a cutscene from Cutscenes/Chapter<N>/<current day>/ (or Common/ for
"common.<Name>"), so a scene file that names a cutscene from another day, or one that does not
exist, plays an empty cutscene (CreateCutsceneFromTextFile logs "Can't find file name") when the
trigger fires. Also flags misspelled valid* keys, which are silently ignored.

Usage: python3 .claude/skills/cutscene/check_scene_refs.py [chapter]   (default chapter 1)
Run from the repo root (the Unity project's Assets/ folder).
"""
import glob
import json
import os
import sys

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))))
ASSETS = ROOT if os.path.isdir(os.path.join(ROOT, "Resources")) else os.path.join(ROOT, "Assets")
VALID_KEYS = {"validDays", "validStages", "validScenes", "validConditions"}
TRIGGERS = {"CutsceneActTrigger", "CutsceneCollider"}


def main():
    chapter = sys.argv[1] if len(sys.argv) > 1 else "1"
    cut_root = os.path.join(ASSETS, "Resources", "Cutscenes", "Chapter" + chapter)
    have = {}  # folder name -> set of cutscene names
    for path in glob.glob(os.path.join(cut_root, "*", "*.json")):
        have.setdefault(os.path.basename(os.path.dirname(path)), set()).add(os.path.basename(path)[:-5])
    problems = 0
    for path in sorted(glob.glob(os.path.join(ASSETS, "Resources", "Scenes", "*", "*.json"))):
        day = os.path.basename(os.path.dirname(path))
        rel = os.path.relpath(path, ASSETS)
        try:
            with open(path, encoding="utf-8-sig") as f:
                items = json.load(f)["Items"]
        except Exception as e:  # noqa: BLE001
            print("%s: malformed JSON: %s" % (rel, e))
            problems += 1
            continue

        def walk(item):
            nonlocal problems
            if not isinstance(item, dict):
                return
            for key in item:
                if key.lower().startswith("vali") and key not in VALID_KEYS:
                    print("%s: %r has misspelled key %r (ignored by the loader)" % (rel, item.get("name"), key))
                    problems += 1
            name = None
            if item.get("type") == "cutscene":
                name = item.get("name")
            elif item.get("type") != "Animation" and (item.get("type") in TRIGGERS or "fileName" in item):
                name = item.get("fileName")
            if name:
                if name.startswith("common."):
                    if name[len("common."):] not in have.get("Common", set()):
                        print("%s: %s does not exist in Common/" % (rel, name))
                        problems += 1
                elif name not in have.get(day, set()):
                    where = [d for d, names in have.items() if name in names]
                    hint = ""
                    if where == ["Common"]:
                        hint = " (it is in Common/: write common.%s)" % name
                    elif where:
                        hint = " (it is in %s; cutscenes load from the current day's folder)" % ", ".join(where)
                    print("%s: %s is not in day %s%s" % (rel, name, day, hint))
                    problems += 1
            for asset in item.get("assets") or []:
                walk(asset)

        for it in items:
            walk(it)
    print("%d problem(s)" % problems)
    return 1 if problems else 0


if __name__ == "__main__":
    sys.exit(main())
