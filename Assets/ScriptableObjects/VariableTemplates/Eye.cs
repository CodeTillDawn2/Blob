using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Eye
{
    public GameObject gameObject;
    public RaycastHit? hit;
    public RaycastHit? edibleHit;
    public RaycastHit? groundHit;


    public GameObject hitObject
    {
        get
        {
            return hit?.collider?.gameObject;
        }
    }

    public Eye(GameObject go)
    {
        gameObject = go;

    }

}
