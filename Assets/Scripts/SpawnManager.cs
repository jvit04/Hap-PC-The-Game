using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] itemPrefab;
    public float minTime = 1f;
    public float maxTime = 2f;

    void Start()
    {
        StartCoroutine(SpawnCoroutine(0));
    }

    IEnumerator SpawnCoroutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (itemPrefab != null && itemPrefab.Length > 0)
            {
             
                Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], transform.position, Quaternion.identity);
            }

            waitTime = Random.Range(minTime, maxTime);
        }
    }
}