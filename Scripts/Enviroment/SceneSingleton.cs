using UnityEngine;

// Base class for the manager objects there is only ever one of per scene -- the Cutscene Manager,
// Room Manager, Music Manager and Scene Loader. It gives each of them a static Instance so the rest
// of the game stops reaching for them with GameObject.Find("Cutscene Manager").GetComponent<...>()
// or with a reference hand-wired in the inspector (which silently breaks whenever a scene is built
// from the template and someone forgets to drag the object in).
//
// The instance is resolved lazily instead of being registered in Awake. Two reasons:
//   * a subclass declaring its own private Awake() hides the base one, and Unity would then never
//     call the registration -- an easy trap to fall into when adding a manager later;
//   * laziness survives scene loads for free. A destroyed manager compares == null under Unity's
//     overloaded operator, so the first access after a load re-finds the new scene's copy.
//
// FindObjectOfType only sees active objects, which matches what the old GameObject.Find calls did.
// Instance returns null in a scene with no such manager, exactly as GameObject.Find did, so callers
// that already null-checked should keep doing so.
public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

    // Convenience for the "spawn one if this scene has none" checks.
    public static bool Exists
    {
        get { return Instance != null; }
    }

    // Drop the cached reference as soon as this manager goes away. The null check in Instance would
    // catch it anyway; clearing here just avoids holding a destroyed object between scenes.
    protected virtual void OnDestroy()
    {
        if (ReferenceEquals(instance, this))
        {
            instance = null;
        }
    }
}
