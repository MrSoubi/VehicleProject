using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefabs;
    [SerializeField] private Transform spawnRocket;
    [SerializeField] private GameObject leftTrapdoor;
    [SerializeField] private Vector3 openPosistionLeft;
    [SerializeField] private GameObject rightTrapdoor;
    [SerializeField] private Vector3 openPosistionRight;
    [SerializeField] private Light lightTrapdoor;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float delay;
    [SerializeField] float warningTime;
    [SerializeField] float openingTime;
    [SerializeField] float warningForce;

    private Vector3 closePosistionLeft;
    private Vector3 closePosistionRight;

    void Start()
    {
        smoke.Play();
        closePosistionLeft = leftTrapdoor.transform.position;
        closePosistionRight = rightTrapdoor.transform.position;
    }

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
        leftTrapdoor.transform.DOMove(openPosistionLeft, openingTime);
        rightTrapdoor.transform.DOMove(openPosistionRight, openingTime).OnComplete(StartDelay);
    }
    void CloseTrapdoor()
    {
        leftTrapdoor.transform.DOMove(closePosistionLeft, openingTime);
        rightTrapdoor.transform.DOMove(closePosistionRight, openingTime);
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
