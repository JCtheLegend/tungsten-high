using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballPlayer : GridMover
{
    CutsceneManager cutscene;
    public DodgeballClassController classController;
    public Planner planner;
    public PickupController pickup;
    public GameObject arrow;
    bool aiming;
    float aimAngle;
    float dodgeballVelocity;
    public float rotateSpeed;

    public Color maxSpeedColor;
    public Color midSpeedColor;
    public Color minSpeedColor;

    public float maxThrowSpeed;
    public float minThrowSpeed;

    float throwTime = 0;

    public GameObject hitAlert;
    public GameObject caughtAlert;

    bool hit;
    public int lives = 3;
    public SpriteRenderer[] Lives;

    public DodgeballMatchController matchController;
    void Update()
    {
        if (moveEnabled)
        {
            DodgeballMove();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !cutscene.inCutscene)
        {
            if (GameManager.sceneCounter >= 7)
            {
                if (!planner.gameObject.activeInHierarchy)
                {
                    Time.timeScale = 0;
                    DisableInput();
                    planner.gameObject.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    planner.gameObject.SetActive(false);
                    EnableInput();
                }
            }
        }
    }

    void DodgeballMove()
    {
        GridMove();
        if (Input.GetKeyDown(KeyCode.Z) && pickup.holdingObject)
        {
            if (aiming)
            {
                StartCoroutine(ThrowDodgeball());
            }
            else
            {
                AimDodgeball();
            }

        }
        if (pickup.holdingObject)
        {
            animator.Play("hold_dodgeball");
        }
        if (!hit && !pickup.holdingObject && pickup.isObjectInRange && Input.GetKeyDown(KeyCode.Z) && (pickup.objectInRange.name == "Opponent Dodgeball" || pickup.objectInRange.name == "Dodgeball"))
        {
            PickupDodgeball();
        }
    }


    void AimDodgeball()
    {
        aiming = true;
        StartCoroutine(RotateArrow());
    }

    void PickupDodgeball()
    {
        pickup.PickupObject();
        if (pickup.pickupObject.gameObject.name == "Opponent Dodgeball")
        {
            StartCoroutine(CatchDodgeball());
        }
        pickup.pickupObject.name = "Ally Dodgeball";
        pickup.pickupObject.gameObject.GetComponent<Dodgeball>().Pickup();
        pickup.pickupObject.gameObject.GetComponent<Dodgeball>().vel = Vector2.zero;
        pickup.pickupObject.gameObject.GetComponent<ArcJump>().jumping = false;      
    }

    IEnumerator CatchDodgeball()
    {
        GameObject g = Instantiate(caughtAlert, this.transform.position + new Vector3(1, 1, 0), Quaternion.identity, this.transform);
        yield return new WaitForSeconds(1);
        Destroy(g);
    }

    IEnumerator ThrowDodgeball()
    {
        dodgeballVelocity = maxThrowSpeed - throwTime + 3;
        if (dodgeballVelocity < minThrowSpeed) dodgeballVelocity = minThrowSpeed;
        if (dodgeballVelocity > maxThrowSpeed) dodgeballVelocity = maxThrowSpeed;
        throwTime = 0;
        aiming = false;
        pickup.holdingObject = false;
        Dodgeball dodgeball = pickup.pickupObject.GetComponent<Dodgeball>();
        animator.Play("throw_dodgeball");
        yield return new WaitForSeconds(0.1f);
        dodgeball.transform.parent = null;
        dodgeball.Throw(new Vector2(Mathf.Sin(aimAngle * Mathf.Deg2Rad) * -1, Mathf.Cos(aimAngle * Mathf.Deg2Rad)) * dodgeballVelocity);
    }

    IEnumerator RotateArrow()
    {
        bool goingLeft = true;
        SpriteRenderer arrowSprite = arrow.GetComponent<SpriteRenderer>();
        arrowSprite.enabled = true;
        while (aiming)
        {
            if (goingLeft)
            {
                aimAngle -= Time.deltaTime * rotateSpeed;
            }
            else
            {
                aimAngle += Time.deltaTime * rotateSpeed;
            }
            if(aimAngle < -60)
            {
                goingLeft = false;
            }
            if(aimAngle > 60)
            {
                goingLeft = true;
            }
            arrow.transform.localEulerAngles = new Vector3(0, 0, aimAngle);
            if(throwTime < 3)
            {
               
            }
            else if (throwTime < 6)
            {
                arrowSprite.color = Color.Lerp(arrowSprite.color, midSpeedColor, Time.deltaTime);
            }
            else
            {
                arrowSprite.color = Color.Lerp(arrowSprite.color, minSpeedColor, Time.deltaTime);
            }
            throwTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        arrowSprite.color = maxSpeedColor;
        arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator Hit()
    {
        hit = true;
        GameObject g = Instantiate(hitAlert, this.transform.position + new Vector3(1, 1, 0), Quaternion.identity, this.transform);
        if(classController != null || classController.stage > 2)
        {
            lives--;
            Lives[lives].enabled = false;
            if (lives == 0)
            {
                Out();
            }
        }      
        yield return new WaitForSeconds(1);
        Destroy(g);        
        hit = false;
    }
    void Out()
    {
        matchController.CheckGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected virtual void HandleCollision(GameObject collisionObject)
    {
        if (collisionObject.name == "Opponent Dodgeball" && !hit)
        {
            StartCoroutine(Hit());
        }
        if (collisionObject.CompareTag("Enemy Attack"))
        {
           
        }
    }


}
