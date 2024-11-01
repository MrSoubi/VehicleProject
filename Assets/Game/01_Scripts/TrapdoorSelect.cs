using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapdoorSelect : MonoBehaviour
{
    [SerializeField] private List<GameObject> trapdoor;
    [SerializeField] private float delayBetweenTrapdoor;

    private int randomTrapdoor;

    void Start()
    {
        StartCoroutine(ChooseRandomTrapdoor(delayBetweenTrapdoor));
    }

    IEnumerator ChooseRandomTrapdoor(float delay)
    {
        randomTrapdoor = Random.Range(0, trapdoor.Count);
        yield return new WaitForSeconds(delay);
        trapdoor[randomTrapdoor].GetComponent<TrapDoor>().StartShake();
        StartCoroutine(ChooseRandomTrapdoor(delay));
    }
}
