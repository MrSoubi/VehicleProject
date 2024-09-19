using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachablePiece : MonoBehaviour
{
    [SerializeField] Transform arenaTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] Vector3 throwingDirection;

    public bool detache;

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }

    private void Update()
    {
        if (detache)
        {
            Detache();
        }
        detache = false;
    }
    public void Detache()
    {
        col.enabled = true;
        rb.isKinematic = false;
        gameObject.transform.SetParent(arenaTransform);
        rb.AddForce(throwingDirection * 100, ForceMode.Impulse);
        rb.AddTorque(new Vector3(0.2f, 0.5f, 0.8f) * 500);
    }
}
