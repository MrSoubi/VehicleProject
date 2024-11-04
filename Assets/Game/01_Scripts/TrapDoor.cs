using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefabs;
    [SerializeField] private Transform spawnRocket;
    [SerializeField] private GameObject trapdoor;
    [SerializeField] private Light lightTrapdoor;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float delay;
    [SerializeField] float warningTime;
    [SerializeField] float openingTime;
    [SerializeField] float warningForce;


    public void StartShake()
    {
        lightTrapdoor.enabled = true;
        var em = smoke.emission;
        em.enabled = true;
        transform.DOShakeRotation(warningTime, warningForce).OnComplete(OpenTrapdoor);
    }
     void OpenTrapdoor()
    {
        var em = smoke.emission;
        em.enabled = false;
        trapdoor.transform.DORotate(new Vector3(90, 0, 0), openingTime).OnComplete(StartDelay);
    }
    void CloseTrapdoor()
    {
        trapdoor.transform.DORotate(new Vector3(0, 0, 0), openingTime);
        lightTrapdoor.enabled = false;
        
    }

    void RocketInstantiate()
    {
        GameObject rocket = Instantiate(rocketPrefabs, spawnRocket.position, spawnRocket.rotation);
        rocket.GetComponent<Rocket>().RocketMovement();
    }
    IEnumerator DelayBeforeSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        RocketInstantiate();
        yield return new WaitForSeconds(delay);
        CloseTrapdoor();
    }

    void StartDelay()
    {
        StartCoroutine(DelayBeforeSpawn(delay));
    }
}
