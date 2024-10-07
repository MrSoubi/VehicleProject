using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform objectSpawnTransform;
    private GameObject objectSpawn;
    private float delayTemp;

    private void Start()
    {
        delayTemp = 0;
    }

    private void Update()
    {
        if (objectSpawn == null)
        {
            if(delayTemp >= delay)
            {
                objectSpawn = Instantiate(objectToSpawn, objectSpawnTransform.position, Quaternion.identity);
                 
            }
            else
            {
                delayTemp += Time.deltaTime;
            }
        }
        else
        {
            delayTemp = 0;
        }
    }
}
