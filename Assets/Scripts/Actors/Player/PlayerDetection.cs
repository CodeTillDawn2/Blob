using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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



public class PlayerDetection : MonoBehaviour
{

    public List<GameObject> Eyes;
    public float sightDistance;

    private Eye Front_0_0;
    private Eye Front_4_0;
    private Eye Front_8_0;
    private Eye Front_0_4;
    private Eye Front_4_4;
    private Eye Front_8_4;
    private Eye Front_0_8;
    private Eye Front_4_8;
    private Eye Front_8_8;

    private Eye Back_0_0;
    private Eye Back_4_0;
    private Eye Back_8_0;
    private Eye Back_0_4;
    private Eye Back_4_4;
    private Eye Back_8_4;
    private Eye Back_0_8;
    private Eye Back_4_8;
    private Eye Back_8_8;

    private Eye Right_0_0;
    private Eye Right_4_0;
    private Eye Right_8_0;
    private Eye Right_0_4;
    private Eye Right_4_4;
    private Eye Right_8_4;
    private Eye Right_0_8;
    private Eye Right_4_8;
    private Eye Right_8_8;

    private Eye Left_0_0;
    private Eye Left_4_0;
    private Eye Left_8_0;
    private Eye Left_0_4;
    private Eye Left_4_4;
    private Eye Left_8_4;
    private Eye Left_0_8;
    private Eye Left_4_8;
    private Eye Left_8_8;

    private static List<Eye> AllEyes;

    public RaycastHit[] InsideHits;

    private void Awake()
    {
        Front_0_0 = new Eye(Eyes.Where(x => x.name == "Front_0-0").First());
        Front_4_0 = new Eye(Eyes.Where(x => x.name == "Front_4-0").First());
        Front_8_0 = new Eye(Eyes.Where(x => x.name == "Front_8-0").First());
        Front_0_4 = new Eye(Eyes.Where(x => x.name == "Front_0-4").First());
        Front_4_4 = new Eye(Eyes.Where(x => x.name == "Front_4-4").First());
        Front_8_4 = new Eye(Eyes.Where(x => x.name == "Front_8-4").First());
        Front_0_8 = new Eye(Eyes.Where(x => x.name == "Front_0-8").First());
        Front_4_8 = new Eye(Eyes.Where(x => x.name == "Front_4-8").First());
        Front_8_8 = new Eye(Eyes.Where(x => x.name == "Front_8-8").First());

        Back_0_0 = new Eye(Eyes.Where(x => x.name == "Back_0-0").First());
        Back_4_0 = new Eye(Eyes.Where(x => x.name == "Back_4-0").First());
        Back_8_0 = new Eye(Eyes.Where(x => x.name == "Back_8-0").First());
        Back_0_4 = new Eye(Eyes.Where(x => x.name == "Back_0-4").First());
        Back_4_4 = new Eye(Eyes.Where(x => x.name == "Back_4-4").First());
        Back_8_4 = new Eye(Eyes.Where(x => x.name == "Back_8-4").First());
        Back_0_8 = new Eye(Eyes.Where(x => x.name == "Back_0-8").First());
        Back_4_8 = new Eye(Eyes.Where(x => x.name == "Back_4-8").First());
        Back_8_8 = new Eye(Eyes.Where(x => x.name == "Back_8-8").First());

        Left_0_0 = new Eye(Eyes.Where(x => x.name == "Left_0-0").First());
        Left_4_0 = new Eye(Eyes.Where(x => x.name == "Left_4-0").First());
        Left_8_0 = new Eye(Eyes.Where(x => x.name == "Left_8-0").First());
        Left_0_4 = new Eye(Eyes.Where(x => x.name == "Left_0-4").First());
        Left_4_4 = new Eye(Eyes.Where(x => x.name == "Left_4-4").First());
        Left_8_4 = new Eye(Eyes.Where(x => x.name == "Left_8-4").First());
        Left_0_8 = new Eye(Eyes.Where(x => x.name == "Left_0-8").First());
        Left_4_8 = new Eye(Eyes.Where(x => x.name == "Left_4-8").First());
        Left_8_8 = new Eye(Eyes.Where(x => x.name == "Left_8-8").First());

        Right_0_0 = new Eye(Eyes.Where(x => x.name == "Right_0-0").First());
        Right_4_0 = new Eye(Eyes.Where(x => x.name == "Right_4-0").First());
        Right_8_0 = new Eye(Eyes.Where(x => x.name == "Right_8-0").First());
        Right_0_4 = new Eye(Eyes.Where(x => x.name == "Right_0-4").First());
        Right_4_4 = new Eye(Eyes.Where(x => x.name == "Right_4-4").First());
        Right_8_4 = new Eye(Eyes.Where(x => x.name == "Right_8-4").First());
        Right_0_8 = new Eye(Eyes.Where(x => x.name == "Right_0-8").First());
        Right_4_8 = new Eye(Eyes.Where(x => x.name == "Right_4-8").First());
        Right_8_8 = new Eye(Eyes.Where(x => x.name == "Right_8-8").First());

        AllEyes = new List<Eye> { Front_0_0, Front_4_0, Front_8_0, Front_0_4, Front_4_4, Front_8_4, Front_0_8, Front_4_8, Front_8_8,
                                        Back_0_0, Back_4_0, Back_8_0, Back_0_4, Back_4_4, Back_8_4, Back_0_8, Back_4_8, Back_8_8,
                                        Left_0_0, Left_4_0, Left_8_0, Left_0_4, Left_4_4, Left_8_4, Left_0_8, Left_4_8, Left_8_8,
                                        Right_0_0, Right_4_0, Right_8_0, Right_0_4, Right_4_4, Right_8_4, Right_0_8, Right_4_8, Right_8_8};
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SeeThings();
    }

    private void SeeThings()
    {

        float halfExtents = .449f;
        Vector3 CubeHalfExtents = new Vector3(PlayerController.Player.CubeWidth * halfExtents, PlayerController.Player.CubeWidth * halfExtents, PlayerController.Player.CubeWidth * halfExtents);
        InsideHits = Physics.BoxCastAll(PlayerController.Player.transform.position, CubeHalfExtents, 
            PlayerController.Player.transform.up, Quaternion.identity, PlayerController.Player.CubeWidth / 2f, (int)LayerMasks.LayerMask_NotGround);
       
        foreach (Eye eye in AllEyes)
        {
            Ray ray = new Ray(eye.gameObject.transform.position, eye.gameObject.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit groundHit, sightDistance, (int)LayerMasks.LayerMask_GroundOnly))
            {
                eye.groundHit = groundHit;
            }
            else
            {
                eye.groundHit = null;
            }
            if (Physics.Raycast(ray, out RaycastHit edibleHit, sightDistance, (int)LayerMasks.LayerMask_EdibleOnly))
            {
                eye.edibleHit = edibleHit;
            }
            else
            {
                eye.edibleHit = null;
            }
            if (Physics.Raycast(ray, out RaycastHit hit, sightDistance, (int)LayerMasks.LayerMask_NotPlayer))
            {
                eye.hit = hit;
            }
            else
            {
                eye.hit = null;
            }
        }
    }


    public bool CanTurnRight()
    {

        foreach (Eye eye in new List<Eye>() { Front_0_8, Front_0_4, Left_0_8, Left_0_4 })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }
        }

        return true;

    }

    public bool CanTurnLeft()
    {

        foreach (Eye eye in new List<Eye>() { Front_8_8, Front_8_4, Right_8_8, Right_8_4 })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }

        }

        return true;

    }

    public bool CanMoveForward()
    {

        foreach (Eye eye in new List<Eye>() { Front_0_4, Front_4_4, Front_8_4, Front_0_8, Front_4_8, Front_8_8 })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }
        }

        return true;

    }

    public bool CanMoveBackwards()
    {

        foreach (Eye eye in new List<Eye>() { Back_0_4, Back_4_4, Back_8_4, Back_0_8, Back_4_8, Back_8_8 })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }
        }

        return true;

    }

    public GameObject FindEdibleBehind()
    {
        foreach (Eye eye in new List<Eye>() { Back_0_0, Back_4_0, Back_8_0, Back_0_4, Back_4_4, Back_8_4, Back_0_8, Back_4_8, Back_8_8 })
        {
            if (eye.edibleHit != null)
            {
                return ((RaycastHit)eye.edibleHit).collider.gameObject;
            }

        }
        return null;
    }

    public enum UnityLayers
    {
        @Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Ground = 3,
        Water = 4,
        UI = 5,
        Player = 6,
        CanBeEaten = 7,
        BeingEaten = 8
    }

    public enum LayerMasks
    {
        LayerMask_GroundOnly = 1 << UnityLayers.Ground,
        LayerMask_EdibleOnly = 1 << UnityLayers.CanBeEaten,
        LayerMask_NotPlayer = ~(1 << UnityLayers.Player),
        LayerMask_NotGround = ~(1 << UnityLayers.Ground)
    }
}
