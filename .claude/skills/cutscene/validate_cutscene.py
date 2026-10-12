#!/usr/bin/env python3
"""Check Tungsten High cutscene JSON for the mistakes that hang or misfire in-game.

    python3 .claude/skills/cutscene/validate_cutscene.py <file.json> [...]
    python3 .claude/skills/cutscene/validate_cutscene.py --all

Reports ERRORs (the cutscene will break) and WARNs (it probably will). Exit code 1 on any ERROR.
This mirrors CutsceneManager.HandleCutscene -- when the two disagree, the C# is right.
"""

import glob
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.realpath(__file__))  # realpath: works through a ~/.claude/skills symlink
ROOT = os.path.abspath(os.path.join(HERE, "..", "..", ".."))
# The skill lives in the git repo, whose root is the Unity project's Assets/ folder. Fall back to the
# old project-root layout (ROOT/Assets/...) if the skill is ever run from there.
ASSETS = ROOT if os.path.isdir(os.path.join(ROOT, "Resources")) else os.path.join(ROOT, "Assets")
CUTSCENES = os.path.join(ASSETS, "Resources", "Cutscenes")

# Every case in the switch. An unlisted type logs INVALID CUTSCENE ACTION and is skipped.
VERBS = {
    "dialog", "dialogAndChoice", "choice", "closeDialog",
    "goto", "endCutscene", "wait",
    "addCondition", "removeCondition", "checkCondition", "checkInventory", "checkFriend",
    "updateInventory", "setSceneCounter", "setStageCounter", "setDayCounter", "friendship",
    "move", "moveBack", "moveSet", "teleport", "moveFace", "rotate",
    "addFollow", "removeFollow", "disableMovement", "enableMovement",
    "moveCamera", "cameraFollow", "setCameraBounds",
    "startBlack", "startWhite", "fillFade", "clearFade", "setBlack", "setWhite",
    "fadeIn", "fadeOut", "color", "hide", "unhide", "disable", "destroy",
    "enableChildren", "enableCollider", "disableCollider",
    "enableComponents", "disableComponents", "changeSortLayer", "changeCostume", "animate",
    "loadObjects", "editObjects", "pickup", "unlockDoor", "loadScene", "dreamExit", "timeline",
    "talkMeter", "switchCombat", "switchNormal", "startFight", "showBars", "hideBars",
    "changeSong", "stopMusic", "playSound",
}

# Types handled by their own case inside HandleMoveSet. Everything else falls through to the
# default branch, which treats it as a move and throws on a missing CutsceneMoveableObject.
MOVESET_VERBS = {
    "move", "animate", "disable", "disableCollider", "enableCollider",
    "hide", "unhide", "teleport", "wait", "moveFace", "goto",
}

NEEDS_NAME = {
    "cameraFollow", "setCameraBounds", "destroy", "color", "changeSortLayer", "disable",
    "enableComponents", "disableComponents", "enableChildren", "disableCollider",
    "enableCollider", "hide", "unhide", "teleport", "move", "moveBack", "moveFace", "rotate",
    "animate", "fadeIn", "fadeOut", "pickup", "unlockDoor", "loadScene", "changeSong",
    "playSound", "addCondition", "removeCondition", "checkCondition", "checkInventory",
    "checkFriend", "updateInventory", "friendship", "talkMeter", "addFollow", "removeFollow",
}
NEEDS_TEXT = {
    "goto", "wait", "setSceneCounter", "setStageCounter", "setDayCounter", "checkInventory",
    "updateInventory", "teleport", "move", "moveBack", "rotate", "moveCamera", "animate",
    "color", "timeline", "changeCostume",
}
NEEDS_SPEED = {"move", "moveBack", "rotate", "moveCamera"}
NEEDS_SKIPS = {"checkCondition", "checkInventory", "checkFriend", "choice", "dialogAndChoice"}
BRANCHING = NEEDS_SKIPS
TERMINAL = {"loadScene", "endCutscene"}

STAGES = {"Pre", "PR", "Gym", "Lunch", "Psych", "Sci", "Post", "Dream"}
DIRECTIONS = {"up", "down", "left", "right", "upLeft", "upRight", "downLeft", "downRight"}
VECTOR = re.compile(r"^\s*-?\d+(\.\d+)?\s*,\s*-?\d+(\.\d+)?\s*$")


def load_heads():
    meta = os.path.join(ASSETS, "Resources", "Heads", "Heads.png.meta")
    try:
        with open(meta, encoding="utf-8", errors="replace") as f:
            return {m.group(1).strip() for m in re.finditer(r"^ +name: (.+)$", f.read(), re.M)}
    except OSError:
        return None


def truthy(v):
    return v is True or (isinstance(v, str) and v.strip().lower() == "true")


def as_int(v):
    try:
        return int(str(v).strip())
    except (TypeError, ValueError):
        return None


class Report:
    def __init__(self, path):
        self.path = path
        self.rows = []
        self.seen_missing_heads = set()

    def err(self, idx, msg):
        self.rows.append(("ERROR", idx, msg))

    def warn(self, idx, msg):
        self.rows.append(("WARN", idx, msg))

    def errors(self):
        return sum(1 for level, _, _ in self.rows if level == "ERROR")

    def print(self):
        rel = os.path.relpath(self.path, ROOT)
        if not self.rows:
            print("OK    %s" % rel)
            return
        print("\n%s" % rel)
        for level, idx, msg in self.rows:
            where = "[%s]" % idx if idx is not None else "   "
            print("  %-5s %-5s %s" % (level, where, msg))


def check_action(a, i, r, heads, in_moveset=False):
    t = a.get("type")
    if not t:
        r.err(i, "no 'type'")
        return
    known = MOVESET_VERBS if in_moveset else VERBS
    if t not in known:
        if in_moveset:
            r.err(i, "'%s' is not handled inside moveSet -- it falls through to the move branch "
                     "and will throw. Supported: %s" % (t, ", ".join(sorted(MOVESET_VERBS))))
        else:
            r.err(i, "unknown verb '%s' -- logs INVALID CUTSCENE ACTION and is skipped" % t)
        return

    if t in NEEDS_NAME and not a.get("name"):
        r.err(i, "%s needs 'name'" % t)
    if t in NEEDS_TEXT and (a.get("text") is None
                            or (a["text"] == "" and t != "changeCostume")):
        r.err(i, "%s needs 'text'" % t)
    if t in NEEDS_SPEED:
        speed = a.get("speed")
        if speed in (None, "") or float(speed) == 0:
            r.err(i, "%s has no non-zero 'speed' -- it never arrives and the cutscene hangs" % t)
    if t in ("move", "moveBack", "rotate") and not a.get("direction") \
            and not truthy(a.get("animateOverride")):
        r.warn(i, "%s has no 'direction' -- parses as 'up', so the walk animation and final "
                  "facing will be wrong" % t)
    if a.get("direction") and a["direction"] not in DIRECTIONS:
        r.err(i, "unknown direction '%s'" % a["direction"])
    if t in ("teleport", "move", "moveCamera") and a.get("text") and not VECTOR.match(str(a["text"])):
        r.err(i, "'text' should be an \"x,y\" position, got %r" % a["text"])
    if t == "moveFace" and a.get("name") == "Player" and a.get("direction") == "left":
        r.warn(i, "moveFace on the Player has no 'left' case -- it does nothing. End a move with "
                  "direction 'left' instead")
    if t == "loadScene" and not a.get("name") and a.get("text"):
        r.err(i, "loadScene reads 'name', not 'text'")
    if t == "setStageCounter" and a.get("text") not in STAGES:
        r.err(i, "setStageCounter wants one of %s" % ", ".join(sorted(STAGES)))
    if t in ("setSceneCounter", "setDayCounter", "wait", "goto") and as_int(a.get("text")) is None:
        try:
            float(a.get("text"))
        except (TypeError, ValueError):
            r.err(i, "%s 'text' must be a number" % t)
    if t == "changeCostume" and a.get("text") not in ("", "pjs", "lunch"):
        r.warn(i, "costume %r is not one of \"\", \"pjs\", \"lunch\" -- the player will play "
                  "nothing" % a.get("text"))

    if t in NEEDS_SKIPS:
        skips = a.get("skips")
        if not isinstance(skips, list) or len(skips) < 2:
            r.err(i, "%s needs two 'skips' entries" % t)
    if t in ("choice", "dialogAndChoice"):
        choices = a.get("choices")
        if not isinstance(choices, list) or len(choices) != 2:
            r.err(i, "the dialog box has exactly two choice slots; got %s"
                     % (len(choices) if isinstance(choices, list) else "none"))
    if t == "dialogAndChoice" and not a.get("text"):
        r.err(i, "dialogAndChoice needs 'text' (use 'choice' for options with no line)")
    if t == "dialog":
        if not a.get("text"):
            r.err(i, "dialog needs 'text'")
        name = a.get("name")
        if name and heads is not None and name not in heads and name not in r.seen_missing_heads:
            r.seen_missing_heads.add(name)
            r.warn(i, "no portrait named %r on the Heads sheet -- the slot renders empty "
                      "(omit 'name' for narration). Reported once per file" % name)
    if t == "talkMeter":
        if not isinstance(a.get("talkMeter"), dict):
            r.err(i, "talkMeter needs a 'talkMeter' object with 'speed' and 'range'")
        if truthy(a.get("isContinue")) and as_int(a.get("text")) is None:
            r.err(i, "talkMeter with isContinue needs 'text' = the index to jump to on failure")
    if t in ("loadObjects", "editObjects") and not a.get("loadObjs"):
        r.err(i, "%s needs 'loadObjs'" % t)
    if t == "moveSet":
        sub = a.get("moveSet")
        if not isinstance(sub, list) or not sub:
            r.err(i, "moveSet needs a non-empty 'moveSet' list")
        else:
            for j, m in enumerate(sub):
                check_action(m, "%s.%s" % (i, j), r, heads, in_moveset=True)


def successors(a, i, n):
    """Indices the loop can run next, mirroring the counter arithmetic in HandleCutscene."""
    t = a.get("type")
    skip = as_int(a.get("skip")) or 0
    if t in TERMINAL:
        return []
    if t == "goto":
        target = as_int(a.get("text"))
        return [] if target is None else [target]
    out = []
    if t in BRANCHING and isinstance(a.get("skips"), list):
        for s in a["skips"]:
            s = as_int(s)
            if s is not None:
                out.append(i + s + 1)
    elif t == "talkMeter" and truthy(a.get("isContinue")):
        out.append(i + 1)
        target = as_int(a.get("text"))
        if target is not None:
            out.append(target)
    else:
        out.append(i + 1)
    if skip > 0:
        out = [x + skip for x in out]
    return out


def check_flow(items, r):
    n = len(items)
    seen, stack = set(), [0]
    while stack:
        i = stack.pop()
        if i in seen or i >= n:
            continue
        if i < 0:
            continue
        seen.add(i)
        for nxt in successors(items[i], i, n):
            if nxt < 0:
                r.err(i, "jumps to negative index %d" % nxt)
            elif nxt > n:
                r.warn(i, "jumps to %d, past the end (%d actions) -- ends the cutscene, which is "
                          "fine if that is the intent" % (nxt, n))
            elif nxt not in seen:
                stack.append(nxt)
    dead = sorted(set(range(n)) - seen)
    if dead:
        r.warn(None, "unreachable: %s -- usually a stale 'goto'/'skips' offset after an edit"
                     % ", ".join(str(d) for d in dead))


def check_shape(items, r):
    types = [a.get("type") for a in items]
    if "disableMovement" in types:
        if not ({"enableMovement", "loadScene", "startFight", "talkMeter", "switchCombat",
                 "dreamExit"} & set(types)):
            r.err(None, "disableMovement with no enableMovement, loadScene or fight/dream handoff "
                        "-- the player is left frozen")
    last = next((t for t in reversed(types) if t), None)
    if last == "disableMovement":
        r.err(None, "the last action is disableMovement")
    # HandleCutscene hides the box when the loop ends, so an unclosed dialog only shows if
    # something the player is meant to watch happens while it is still up.
    VISIBLE = ("move", "moveSet", "moveBack", "wait", "animate", "rotate", "moveCamera",
               "fadeIn", "fadeOut", "teleport", "dreamExit", "timeline")
    open_at = None
    for i, t in enumerate(types):
        if t in ("dialog", "dialogAndChoice", "choice"):
            open_at = i
        elif t == "closeDialog":
            open_at = None
        elif t in VISIBLE and open_at is not None:
            r.warn(i, "%s plays with the dialog box still open from [%d] -- closeDialog first"
                      % (t, open_at))
            open_at = None
        elif t == "enableMovement" and open_at is not None and i < len(types) - 1:
            r.warn(i, "control returns with the box still open from [%d] and the cutscene keeps "
                      "running -- closeDialog first" % open_at)
            open_at = None
    disabled = {}
    for i, a in enumerate(items):
        if a.get("type") == "disable" and a.get("name"):
            disabled.setdefault(a["name"], i)
        elif a.get("name") in disabled and a.get("type") not in ("loadObjects", "editObjects"):
            r.err(i, "%r was disabled at [%d]; GameObject.Find cannot reach an inactive object. "
                     "Use hide/unhide if it has to come back" % (a["name"], disabled[a["name"]]))
    if "moveCamera" in types and "cameraFollow" not in types:
        r.warn(None, "moveCamera detaches the camera and nothing re-attaches it -- add "
                     "cameraFollow \"Player\"")


def validate(path, heads):
    r = Report(path)
    try:
        with open(path, encoding="utf-8-sig") as f:
            data = json.load(f)
    except json.JSONDecodeError as e:
        r.err(None, "malformed JSON: %s" % e)
        return r
    except OSError as e:
        r.err(None, str(e))
        return r

    if not isinstance(data, dict) or "Items" not in data:
        r.err(None, 'must be wrapped as {"Items": [ ... ]} -- JsonHelper reads nothing else')
        return r
    items = data["Items"]
    if not isinstance(items, list) or not items:
        r.err(None, "'Items' is empty or not a list")
        return r

    for i, a in enumerate(items):
        if not isinstance(a, dict):
            r.err(i, "not an object")
            continue
        check_action(a, i, r, heads)
    check_flow(items, r)
    check_shape(items, r)
    return r


def main(argv):
    if not argv:
        print(__doc__)
        return 2
    if argv[0] == "--all":
        paths = sorted(glob.glob(os.path.join(CUTSCENES, "**", "*.json"), recursive=True))
    else:
        paths = argv
    heads = load_heads()
    failed = 0
    for p in paths:
        r = validate(p, heads)
        r.print()
        failed += r.errors()
    print("\n%d file(s), %d error(s)" % (len(paths), failed))
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
