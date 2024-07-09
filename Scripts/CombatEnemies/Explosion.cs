using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip clip;
    AudioSource a;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        StartCoroutine(Explode()); 
    }

    IEnumerator Explode()
    {
        a.pitch = Random.Range(0.5f, 1.5f);
        a.PlayOneShot(clip);
        animator.Play("explosion", 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(this.gameObject);
    }
}
