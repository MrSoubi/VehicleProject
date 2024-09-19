using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExploder : MonoBehaviour
{
    [SerializeField] List<DetachablePiece> detachablePieces;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
    }

    public void Explode()
    {
        foreach (DetachablePiece piece in detachablePieces)
        {
            piece.Detache();
        }
    }
}
