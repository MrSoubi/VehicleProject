using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareMapBehaviour : MonoBehaviour
{
    [SerializeField] float warningTime, rotationTime, delayBeforeNextRotation, warningForce;
    void Start()
    {
        StartCoroutine(DelayBeforeRotation());
    }

    IEnumerator DelayBeforeRotation()
    {
        yield return new WaitForSeconds(delayBeforeNextRotation + warningTime + rotationTime);
        StartRotation();
        StartCoroutine(DelayBeforeRotation());
    }

    bool flip = false;
    void StartRotation()
    {
        transform.DOShakeRotation(warningTime, warningForce).OnComplete(Rotate);
    }

    void Rotate()
    {
        Vector3 rotation;

        if (flip)
        {
            rotation = Vector3.right;
        }
        else
        {
            rotation = Vector3.forward;
        }

        flip = !flip;

        transform.DORotate(transform.rotation.eulerAngles + rotation * 90, rotationTime);
    }

}
