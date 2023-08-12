using System;
using UnityEngine;

[Serializable]
public class RaycastHitInfo
{

    public Rigidbody rb;
    public Collider collider;
    public GameObject gameObject;
    public float distance;

    public RaycastHitInfo(RaycastHit hit)
    {
        distance = hit.distance;
        rb = hit.rigidbody;
        collider = hit.collider;
        if (rb != null)
            gameObject = hit.rigidbody.gameObject;
        if (collider != null && gameObject == null)
            gameObject = hit.collider.gameObject;
    }


}
