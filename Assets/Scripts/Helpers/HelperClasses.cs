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

}
