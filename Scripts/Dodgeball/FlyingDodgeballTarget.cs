using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDodgeballTarget : DodgeballTarget
{
    public GridMover player;
    public GameObject dodgeBall;
    public int throwSpeed;
    public int maxHeight;
    public int minHeight;
    public int maxWidth;
    public int minWidth;
    public bool shooting;

    MoveableObject m;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m = GetComponent<MoveableObject>();
        StartCoroutine(GoToPoint());
        StartCoroutine(ThrowDodgeball());
    }

    IEnumerator GoToPoint()
    {
        while (!hit)
        {
            Vector2 travelPoint = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            StartCoroutine(m.Move(travelPoint, 4));
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator ThrowDodgeball()
    {
        while (!hit && shooting)
        {
            Vector3 offset = Random.Range(0, 2) == 1 ? new Vector2(1, 0) : new Vector2(-1, 0);
            Dodgeball d = Instantiate(dodgeBall, this.transform.position, Quaternion.identity).GetComponent<Dodgeball>();
            d.name = "Opponent Dodgeball";
            d.Throw((player.transform.position - transform.position).normalized * throwSpeed);
            yield return new WaitForSeconds(3);
        }
    }
}
