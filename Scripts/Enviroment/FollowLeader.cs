using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{

    public GameObject leader;
    int stepsBehind = 30;
    List<Vector3> followSteps = new List<Vector3>();
    BoxCollider2D col;
    Vector3 nextStep;
    float speed = 3;
    Animator animator;
    AnimatableObject anim;
    direction d;

    public void NewFollower()
    {
        GameManager.followers.Add(this.gameObject.name);
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        anim = GetComponent<AnimatableObject>();
        col.enabled = false;
        anim.enabled = true;
    }

    public void SetLeader(string newLeader)
    {
        leader = GameObject.Find(newLeader);
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        anim = GetComponent<AnimatableObject>();
        col.enabled = false;
        anim.enabled = true;
    }

    public void RemoveFollower()
    {
        leader = null;
        GameManager.followers.Remove(this.gameObject.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followSteps.Count < stepsBehind || leader.transform.position != followSteps.Last())
        {
            followSteps.Add(leader.transform.position);
        }
        if (followSteps.Count > stepsBehind)
        {
            RecordLeaderSteps();
            Follow();
        }
        else
        {
            anim.StopAnimation();
        }
    }

    private void Update()
    {
        FollowAnimUpdate();
    }

    void RecordLeaderSteps()
    {
        nextStep = followSteps[0];
        followSteps.RemoveAt(0);
    }

    void FollowAnimUpdate()
    {
        if (anim)
        {
            if (nextStep.x - transform.position.x > 0.1)
            {
                d = direction.right;
            }
            else if (nextStep.x - transform.position.x < -0.1)
            {
                d = direction.left;
            }
            else if (nextStep.y - transform.position.y > 0.1)
            {
                d = direction.up;
            }
            else if (nextStep.y - transform.position.y < -0.1)
            {
                d = direction.down;
            }
        }
        anim.AnimateMove(d);
    }

    void Follow()
    {
        //Debug.Log(nextStep - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, nextStep, speed * Time.deltaTime);
    }
}
