using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class BlobBody_Solid : BlobBody
{

    [SerializeField] LayerMask BodySees;

    [SerializeField] protected Vector3Variable bodyDims;
    [SerializeField] protected Vector3Variable bodyConstraints;
    public override Vector3Variable BodyDims { get { return bodyDims; } set { bodyDims = value; } }
    public Vector3Variable BodyConstraints { get { return bodyConstraints; } set { bodyConstraints = value; } }

    [Serialize] public GameObject MeshObject;

    protected override Rigidbody rb { get; set; }

    private SkinnedMeshRenderer meshRenderer;

    private BoxCollider cuboidCollider;
    private LayerMask canBeEatenAndBeingEatenMask;

    protected Vector3 TargetSideLengths;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        cuboidCollider = GetComponent<BoxCollider>();
        meshRenderer = MeshObject.GetComponent<SkinnedMeshRenderer>();
        canBeEatenAndBeingEatenMask = LayerMask.GetMask("CanBeEaten", "BeingEaten");
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

    }

    public void FindObjectsContainedAndTouching(out List<GameObject> fullyContainedObjects, out List<GameObject> touchingObjects)
    {
        fullyContainedObjects = new List<GameObject>();
        touchingObjects = new List<GameObject>();

        if (cuboidCollider != null)
        {
            // Get the oriented center of the cuboid in world space
            Vector3 orientedCenter = transform.TransformPoint(cuboidCollider.center);

            Collider[] colliders = Physics.OverlapBox(orientedCenter, BodyDims.Value * 0.5f, transform.rotation, canBeEatenAndBeingEatenMask);

            foreach (Collider collider in colliders)
            {
                if (collider != cuboidCollider)
                {
                    List<Vector3> pointsToCheck = new List<Vector3>();

                    if (collider is BoxCollider box)
                    {
                        Vector3[] boxVertices = {
                        box.center + new Vector3(box.size.x, box.size.y, box.size.z) * 0.5f,
                        box.center + new Vector3(-box.size.x, box.size.y, box.size.z) * 0.5f,
                        box.center + new Vector3(box.size.x, -box.size.y, box.size.z) * 0.5f,
                        box.center + new Vector3(box.size.x, box.size.y, -box.size.z) * 0.5f,
                        box.center + new Vector3(-box.size.x, -box.size.y, box.size.z) * 0.5f,
                        box.center + new Vector3(-box.size.x, box.size.y, -box.size.z) * 0.5f,
                        box.center + new Vector3(box.size.x, -box.size.y, -box.size.z) * 0.5f,
                        box.center - new Vector3(box.size.x, box.size.y, box.size.z) * 0.5f
                    };
                        pointsToCheck.AddRange(boxVertices);
                    }
                    else if (collider is SphereCollider sphere)
                    {
                        // This is a simple sampling approach; in reality, you might want more points
                        pointsToCheck.Add(sphere.center + Vector3.up * sphere.radius);
                        pointsToCheck.Add(sphere.center - Vector3.up * sphere.radius);
                        pointsToCheck.Add(sphere.center + Vector3.right * sphere.radius);
                        pointsToCheck.Add(sphere.center - Vector3.right * sphere.radius);
                        pointsToCheck.Add(sphere.center + Vector3.forward * sphere.radius);
                        pointsToCheck.Add(sphere.center - Vector3.forward * sphere.radius);
                    }
                    // Extend similarly for CapsuleCollider
                    else if (collider is MeshCollider meshCollider)
                    {
                        MeshFilter meshFilter = collider.gameObject.GetComponent<MeshFilter>();
                        if (meshFilter != null)
                        {
                            pointsToCheck.AddRange(meshFilter.mesh.vertices);
                        }
                    }


                    // Find the vertex furthest from the oriented center
                    float maxDistanceSquared = 0;
                    Vector3 furthestPoint = Vector3.zero;
                    foreach (var point in pointsToCheck)
                    {
                        Vector3 worldPoint = collider.transform.TransformPoint(point);
                        float distanceSquared = (worldPoint - orientedCenter).sqrMagnitude;
                        if (distanceSquared > maxDistanceSquared)
                        {
                            maxDistanceSquared = distanceSquared;
                            furthestPoint = worldPoint;
                        }
                    }

                    Debug.DrawLine(orientedCenter, furthestPoint, Color.cyan);

                    // Transform furthestPoint from world space to the local space of the cuboid
                    Vector3 localPoint = cuboidCollider.transform.InverseTransformPoint(furthestPoint);

                    // Get half the size of the cuboid for comparison
                    Vector3 halfSize = cuboidCollider.size * 0.5f;

                    // Check if the point is inside the cuboid in its local space
                    if (Mathf.Abs(localPoint.x) <= halfSize.x &&
                        Mathf.Abs(localPoint.y) <= halfSize.y &&
                        Mathf.Abs(localPoint.z) <= halfSize.z)
                    {

                        fullyContainedObjects.Add(collider.gameObject);
                        touchingObjects.Add(collider.gameObject);
                    }
                    else
                    {

                        touchingObjects.Add(collider.gameObject);
                    }



                }
            }
        }
    }






    public override void CalculateIntersections()
    {
        FindObjectsContainedAndTouching(out List<GameObject> fullyContained, out List<GameObject> touching);

        ContainedInStomach.MatchList(fullyContained);
        IntersectsPlayer.MatchList(touching);

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
