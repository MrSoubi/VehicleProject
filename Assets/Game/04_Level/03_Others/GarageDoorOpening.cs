using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GarageDoorOpening : MonoBehaviour
{
    public void Open()
    {
        transform.DOMove(transform.position + Vector3.up, 1);
    }
}
