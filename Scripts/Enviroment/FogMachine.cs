using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMachine : MonoBehaviour
{

    [SerializeField] Sprite[] fogOptions;

    [SerializeField] GameObject fogTemplate;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [SerializeField] float speed;
    [SerializeField] float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunFogMachine());
        for(int i = 0; i < 10; i++)
        {
            bool left = Random.value > 0.5;
            Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject newFog = Instantiate(fogTemplate, spawnPoint, Quaternion.identity);
            newFog.GetComponent<SpriteRenderer>().sprite = fogOptions[Random.Range(0, 6)];
            if (left)
            {
                StartCoroutine(newFog.GetComponent<MoveableObject>().Move(new Vector2(maxX + 10, newFog.transform.position.y), speed));
            }
            else
            {
                StartCoroutine(newFog.GetComponent<MoveableObject>().Move(new Vector2(minX - 10, newFog.transform.position.y), speed));
            }
        }
    }

    IEnumerator RunFogMachine()
    {
        while (true)
        {
            bool left = Random.value > 0.5;
            Vector2 spawnPoint = new Vector2(left ? minX - 5 : maxX + 5, Random.Range(minY, maxY));
            GameObject newFog = Instantiate(fogTemplate, spawnPoint, Quaternion.identity);
            newFog.GetComponent<SpriteRenderer>().sprite = fogOptions[Random.Range(0, 6)];
            if (left)
            {
                StartCoroutine(newFog.GetComponent<MoveableObject>().Move(new Vector2(maxX + 10, newFog.transform.position.y), speed));
            }
            else
            {
                StartCoroutine(newFog.GetComponent<MoveableObject>().Move(new Vector2(minX - 10, newFog.transform.position.y), speed));
            }
            yield return new WaitForSeconds(reloadTime);
        }
    }
}
