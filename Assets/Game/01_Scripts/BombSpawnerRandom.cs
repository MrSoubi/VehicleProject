using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawnerRandom : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private List<Transform> objectSpawnTransform;

    private int randomTransfrom;
    private float delayTemp;
    private bool isSpawn;

    void Update()
    {
        if (!isSpawn)
        {
            if (delayTemp >= delay)
            {
                randomTransfrom = Random.Range(0, objectSpawnTransform.Count);
                GameObject bomb = Instantiate(objectToSpawn, objectSpawnTransform[randomTransfrom].position, Quaternion.identity);
                bomb.GetComponent<ExplosiveObject>().SpawnDetonate();
                isSpawn = true;
            }
            else
            {
                delayTemp += Time.deltaTime;
            }
        }
        else
        {
            delayTemp = 0;
            isSpawn = false;
        }

    }
}
