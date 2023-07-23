using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocRoc
{

    public LocRoc(Vector3 location, Quaternion rotation)
    {
        this.location = location;
        this.rotation = rotation;
    }


    public Vector3 location;
    public Quaternion rotation;
}
