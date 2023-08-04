using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Shortcuts;

public class BlobStomach_Solid : BlobStomach
{
    [SerializeField] LayerMask StomachSees;

    [SerializeField] protected Vector3Variable bodyDims;
    [SerializeField] protected Vector3Variable bodyConstraints;
    public override Vector3Variable BodyDims { get { return bodyDims; } set { bodyDims = value; } }
    public Vector3Variable BodyConstraints { get { return bodyConstraints; } set { bodyConstraints = value; } }

    [Serialize] public GameObject MeshObject;

    protected override Rigidbody rb { get; set; }

    private SkinnedMeshRenderer meshRenderer;

    protected Vector3 TargetSideLengths;
    protected void Awake()
    {

    }

    protected override void Start()
    {
        meshRenderer = MeshObject.GetComponent<SkinnedMeshRenderer>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        TargetSideLengths = GetBoxSideSizes(BodyConstraints.Value, MassTarget.Value / CurrentMassPerCubicFoot.Value);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CalculateIntersections();

        if (ContainedInStomach.Items.Count > 0)
        {
            string test = "";
        }
        if (ContainedInStomach.Items.Count == 0)
        {
            string test = "";
        }
    }

    public Vector3 Vector3_CubeHalfExtents
    {
        get { return new Vector3(BodyDims.Value.x * .5f, BodyDims.Value.y * .5f, BodyDims.Value.z * .5f); }
    }


    public List<GameObject> FindObjectsTouchingEdge()
    {
        List<BoxCastTemplate> boxes = new List<BoxCastTemplate>();

        float xAdj = .475f;
        float yAdj = .975f;
        float zAdj = .475f;
        float xOffset = .05f;
        float yOffset = .05f;
        float zOffset = .05f;
        float distanceMult = .9f;
        int targetCount = 20;
        float ActualDistance = BodyDims.Value.x * distanceMult;
        Vector3 dir = Vector3.zero;
        UnityEngine.Color color = UnityEngine.Color.red;

        List<Vector3> cubeFaces = new List<Vector3>() { transform.up, -transform.up,
                                                        transform.right, -transform.right,
                                                         transform.forward, -transform.forward};

        foreach (Vector3 cubeFace in cubeFaces)
        {
            if (cubeFace == -transform.right)
            { //left
                xAdj = -.475f;
                yAdj = .025f;
                zAdj = .475f;
                xOffset = 0;
                yOffset = 0;
                zOffset = .05f;
                color = UnityEngine.Color.green;
                dir = transform.up;
                ActualDistance = BodyDims.Value.y * distanceMult;
            }
            else if (cubeFace == -transform.up) //bottom
            {
                xAdj = -.425f;
                yAdj = .025f;
                zAdj = .475f;
                xOffset = 0;
                yOffset = 0;
                zOffset = .05f;
                color = UnityEngine.Color.white;
                dir = transform.right;
                ActualDistance = BodyDims.Value.x * distanceMult;
            }
            else if (cubeFace == transform.right) //right
            {
                xAdj = .475f;
                yAdj = .975f;
                zAdj = .475f;
                xOffset = 0;
                yOffset = 0;
                zOffset = .05f;
                color = UnityEngine.Color.white;
                dir = -transform.up;
                ActualDistance = BodyDims.Value.y * distanceMult;
            }
            else if (cubeFace == transform.up) // up
            {
                xAdj = -.475f;
                yAdj = .975f;
                zAdj = .475f;
                xOffset = 0;
                yOffset = 0;
                zOffset = .05f;
                color = UnityEngine.Color.red;
                dir = transform.right;
                ActualDistance = BodyDims.Value.x * distanceMult;
            }
            else if (cubeFace == transform.forward)
            {
                xAdj = .425f;
                yAdj = .925f;
                zAdj = -.475f;
                xOffset = .05f;
                yOffset = 0;
                zOffset = 0;
                color = UnityEngine.Color.white;
                dir = -transform.up;
                distanceMult = .85f;
                targetCount = 18;
                ActualDistance = BodyDims.Value.y * distanceMult;
            }
            else if (cubeFace == -transform.forward)
            {
                xAdj = .425f;
                yAdj = .925f;
                zAdj = .475f;
                xOffset = .05f;
                yOffset = 0;
                zOffset = 0;
                color = UnityEngine.Color.white;
                dir = -transform.up;
                distanceMult = .85f;
                targetCount = 18;
                ActualDistance = BodyDims.Value.y * distanceMult;
            }

            for (int a = 0; a < targetCount; a++)
            {



                boxes.Add(new BoxCastTemplate()
                {
                    StartLoc = gameObject.transform.position,
                    LocAdjustment = BodyRotation.Value * new Vector3(BodyDims.Value.x * (xAdj - a * xOffset), BodyDims.Value.y * (yAdj - a * yOffset), BodyDims.Value.z * (zAdj - a * zOffset)),
                    distance = ActualDistance,
                    Extents = bodyDims.Value * .025f,
                    Direction = dir,
                    color = color
                });
            }
        }

        //foreach (BoxCastTemplate box in boxes)
        //{
        //    PhysicsTools.DrawBoxCastBox(box.StartLoc + box.LocAdjustment,
        //        box.Extents, gameObject.transform.rotation, box.Direction, box.distance, box.color);
        //    break;
        //}

        List<GameObject> gameObjectsFound = new List<GameObject>();

        foreach (BoxCastTemplate box in boxes)
        {
            RaycastHit[] hits = Physics.BoxCastAll(box.StartLoc + box.LocAdjustment, box.Extents, box.Direction, gameObject.transform.rotation,
                box.distance, StomachSees);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    if (!gameObjectsFound.Contains(hit.collider.gameObject))
                    {
                        gameObjectsFound.Add(hit.collider.gameObject);
                    }
                }

            }


        }

        return gameObjectsFound;
    }

    public class BoxCastTemplate
    {
        public Vector3 StartLoc;
        public Vector3 Extents;
        public Vector3 LocAdjustment;
        public Vector3 Direction;
        public float distance;
        public UnityEngine.Color color = UnityEngine.Color.red;
    }

    public override void CalculateIntersections()
    {
        //PhysicsTools.DrawBoxCastBox(gameObject.transform.position + new Vector3(0, bodyDims.Value.y * .5f, 0),
        // Vector3_CubeHalfExtents * .9f, gameObject.transform.rotation, transform.up, 0, UnityEngine.Color.blue);



        List<GameObject> touchingEdge = FindObjectsTouchingEdge();



        RaycastHit[] insideStomach = Physics.BoxCastAll(gameObject.transform.position + new Vector3(0, bodyDims.Value.y * .5f, 0),
            Vector3_CubeHalfExtents * .9f,
            transform.up, gameObject.transform.rotation, 0, StomachSees);
        List<GameObject> NewIntersects = new List<GameObject>();
        List<GameObject> NewInStomach = new List<GameObject>();


        foreach (GameObject hitinfo in touchingEdge)
        {
            if (hitinfo.GetComponent<Collider>().gameObject.layer != (int)UnityLayers.Player
                && hitinfo.GetComponent<Collider>().gameObject.layer != (int)UnityLayers.PlayerTentacle
                && hitinfo.GetComponent<Collider>().gameObject.layer != (int)UnityLayers.Ground)
            {
                NewIntersects.Add(hitinfo.GetComponent<Collider>().gameObject);
            }
        }
        foreach (RaycastHit hitinfo in insideStomach)
        {
            if (hitinfo.collider.gameObject.layer != (int)UnityLayers.Player
                && hitinfo.collider.gameObject.layer != (int)UnityLayers.PlayerTentacle
                && hitinfo.collider.gameObject.layer != (int)UnityLayers.Ground
                && !NewIntersects.Contains(hitinfo.collider.gameObject))
            {
                NewInStomach.Add(hitinfo.collider.gameObject);
            }
        }


        IntersectsPlayer.MatchList(NewIntersects);
        ContainedInStomach.MatchList(NewInStomach);


        NewIntersects = null;
        NewInStomach = null;
    }

    protected Vector3 GetBoxSideSizes(Vector3 constraints, float TargetVolume)
    {
        int NumberOfConstraints = 0;
        Vector3 sideLengths = Vector3.one;

        if (constraints.x > 0)
        {
            NumberOfConstraints++;
        }
        if (constraints.y > 0)
        {
            NumberOfConstraints++;
        }
        if (constraints.z > 0)
        {
            NumberOfConstraints++;
        }

        if (NumberOfConstraints == 1)
        {
            if (constraints.x > 0)
            {
                float OtherConstraints = (float)Math.Sqrt(TargetVolume / constraints.x);
                sideLengths = new Vector3(constraints.x, OtherConstraints, OtherConstraints);
            }
            else if (constraints.y > 0)
            {
                float OtherConstraints = (float)Math.Sqrt(TargetVolume / constraints.y);
                sideLengths = new Vector3(OtherConstraints, constraints.y, OtherConstraints);
            }
            else if (constraints.z > 0)
            {
                float OtherConstraints = (float)Math.Sqrt(TargetVolume / constraints.z);
                sideLengths = new Vector3(OtherConstraints, OtherConstraints, constraints.z);
            }
        }
        else if (NumberOfConstraints == 2)
        {
            if (constraints.x == 0)
            {
                sideLengths = new Vector3(TargetVolume / constraints.y / constraints.z, constraints.y, constraints.z);
            }
            else if (constraints.y == 0)
            {
                sideLengths = new Vector3(constraints.x, TargetVolume / constraints.x / constraints.z, constraints.z);
            }
            else if (constraints.z == 0)
            {
                sideLengths = new Vector3(constraints.x, constraints.y, TargetVolume / constraints.x / constraints.y);
            }
        }

        //Vector3 BuildingBlock;
        //float Multiplier;
        //if (sideLengths.x <= sideLengths.y && sideLengths.x <= sideLengths.z)
        //{
        //    Multiplier = sideLengths.x;
        //    BuildingBlock = new Vector3(1f, sideLengths.y / sideLengths.x, sideLengths.z / sideLengths.x);
        //}
        //else if (sideLengths.y <= sideLengths.z && sideLengths.y <= sideLengths.z)
        //{
        //    Multiplier = sideLengths.y;
        //    BuildingBlock = new Vector3(sideLengths.x / sideLengths.y, 1f, sideLengths.z / sideLengths.y);
        //} 
        //else //z
        //{
        //    Multiplier = sideLengths.z;
        //    BuildingBlock = new Vector3(sideLengths.x / sideLengths.z, sideLengths.y / sideLengths.z, 1f);
        //}



        if (NumberOfConstraints == 0 || NumberOfConstraints == 3)
        {
            float EachSide = (float)Math.Cbrt(TargetVolume);
            sideLengths = new Vector3(EachSide, EachSide, EachSide);
        }


        return sideLengths;
    }

    protected override IEnumerator ChangeSize()
    {
        if (meshRenderer != null && rb != null)
        {
            ChangingSize = true;

            Vector3 startScale = RigidBodyObject.transform.localScale;

            while (Math.Abs(rb.mass - MassTarget.Value) > MassTarget.Value / 100
                || TargetSideLengths.normalized != RigidBodyObject.transform.localScale.normalized)
            {
                if (meshRenderer != null)
                {
                    float TargetVolume = MassTarget.Value / CurrentMassPerCubicFoot.Value;
                    TargetSideLengths = GetBoxSideSizes(BodyConstraints.Value, TargetVolume);
                    Vector3 OriginalSize = meshRenderer.sharedMesh.bounds.size;
                    float targetXScale = TargetSideLengths.x / OriginalSize.x;
                    float targetYScale = TargetSideLengths.y / OriginalSize.y;
                    float targetZScale = TargetSideLengths.z / OriginalSize.z;
                    Vector3 targetScale = new Vector3(targetXScale, targetYScale, targetZScale);
                    UnparentEdibles();
                    float lerpValue = Time.fixedDeltaTime * CurrentGrowthSpeedModifier.Value;
                    RigidBodyObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpValue);
                    ParentEdibles();

                    ResetCubeDims();
                    float CurrentVolume = BodyDims.Value.x * BodyDims.Value.y * BodyDims.Value.z;

                    rb.mass = CurrentVolume * CurrentMassPerCubicFoot.Value;

                    // update start scale for next frame
                    startScale = RigidBodyObject.transform.localScale;

                    yield return new WaitForFixedUpdate();
                }
            }

            ChangingSize = false;
        }
    }


    protected override void ResetCubeDims()
    {
        Vector3 OriginalSize = meshRenderer.sharedMesh.bounds.size;
        BodyDims.Value.x = RigidBodyObject.transform.localScale.x * OriginalSize.x;
        BodyDims.Value.y = RigidBodyObject.transform.localScale.y * OriginalSize.y;
        BodyDims.Value.z = RigidBodyObject.transform.localScale.z * OriginalSize.z;
        CubeVolume.Value = BodyDims.Value.x * BodyDims.Value.y * BodyDims.Value.z;

    }



}
