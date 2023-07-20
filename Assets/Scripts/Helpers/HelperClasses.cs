using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperClasses 
{
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

}
