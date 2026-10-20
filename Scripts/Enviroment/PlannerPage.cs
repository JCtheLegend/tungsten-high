using UnityEngine;

// One tab of the Planner: a PlannerSelect that also owns the objects making up its page, showing them
// when the tab is hovered and hiding the neighbouring pages'.
//
// PlannerSelect and SystemButton used to share this file. Unity only makes the class whose name
// matches the file attachable to a GameObject, so they each have their own file now.
public class PlannerPage : PlannerSelect
{
    public GameObject[] pageObjects;

    public override void Hover()
    {
        base.Hover();
        foreach (GameObject g in pageObjects)
        {
            g.SetActive(true);
        }
        if (left)
        {
            foreach (GameObject g in ((PlannerPage)left).pageObjects)
            {
                g.SetActive(false);
            }
        }
        if (right)
        {
            foreach (GameObject g in ((PlannerPage)right).pageObjects)
            {
                g.SetActive(false);
            }
        }

    }
}
