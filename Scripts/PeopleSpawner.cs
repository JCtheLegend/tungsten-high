using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawner : MonoBehaviour
{
    [SerializeField] float startY;
    [SerializeField] float endY;
    [SerializeField] float[] xPos;

    [SerializeField] int wait;

    public GameObject walkingPerson;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPeople());
    }

    IEnumerator SpawnPeople()
    {
       
        while (true)
        {
            float[] randXPos = Reshuffle(xPos);
            for (int i = 0; i < 6; i++)
            {
                WalkingPerson w = Instantiate(walkingPerson, new Vector2(randXPos[i], startY), Quaternion.identity).GetComponent<WalkingPerson>();
                StartCoroutine(w.StartWalking(endY));
            }
            yield return new WaitForSeconds(wait);
        }
    }

    float[] Reshuffle(float[] nums)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < nums.Length; t++)
        {
            float tmp = nums[t];
            int r = Random.Range(t, nums.Length);
            nums[t] = nums[r];
            nums[r] = tmp;
        }
        return nums;
    }
}
