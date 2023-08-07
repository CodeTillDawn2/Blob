using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Senses : MonoBehaviour
{

    [Header("Stat Block")]
    [Tooltip("")]
    [Serialize] public Dict_GameObjectToLastSeen ThingsSeen;
    [Serialize] public GameObjectRuntimeSet ThingsNearby;
    [Serialize] public FloatVariable CurrentSightDistance;
    [Serialize] public List<GameObject> Eyes;

    protected abstract Shortcuts.LayerMasks OnlySeeMask { get; }
    protected float CurrentSightBoxSize;
    protected BoxCollider SightBox;
    /// <summary>
    /// Method used to filter out game objects which should not be added to "things nearby"
    /// </summary>
    /// <returns></returns>
    protected abstract bool FilterThingNearby(Collider col);

    protected virtual void Awake()
    {
        SightBox = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        CurrentSightDistance = Instantiate(SOLibrary.instance.EmptyFloatVariable);
        ThingsSeen = Instantiate(SOLibrary.instance.EmptyGameObjectToLastSeenDict);
        CurrentSightBoxSize = SightBox.size.x;
    }

    protected virtual void Update()
    {
        SeeThings();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentSightBoxSize != CurrentSightDistance.Value)
        {
            SightBox.size = new Vector3(CurrentSightDistance.Value, CurrentSightDistance.Value, CurrentSightDistance.Value);
        }
    }

    protected void OnTriggerEnter(Collider col)
    {

        if (col.gameObject == null) return;

        if (FilterThingNearby(col))
        {
            ThingsNearby.AddUpdate(col.gameObject);
        }
    }


    protected void OnTriggerExit(Collider col)
    {
        if (col.gameObject == null) return;
        ThingsNearby.Remove(col.gameObject);
    }
    protected virtual void SeeThings()
    {



        Dictionary<GameObject, LastSeenData> newThingsSeen = new Dictionary<GameObject, LastSeenData>();

        //See things
        foreach (GameObject ThingNearby in ThingsNearby.Items.Where(x => x != null))
        {
            if ((ThingNearby.transform.position - transform.position).sqrMagnitude <= CurrentSightDistance.Value * CurrentSightDistance.Value)
            {
                Collider[] ThingColliders = ThingNearby.GetComponents<Collider>();

                Collider ThingCollider = null;
                foreach (Collider CheckedThingCollider in ThingColliders)
                {
                    if (CheckedThingCollider.isTrigger) continue;
                    ThingCollider = CheckedThingCollider;
                    break;
                }

                if (ThingCollider == null) ThingColliders = ThingNearby.GetComponentsInChildren<Collider>();
                foreach (Collider CheckedThingCollider in ThingColliders)
                {
                    if (CheckedThingCollider.isTrigger) continue;
                    ThingCollider = CheckedThingCollider;
                    break;
                }
                if (ThingCollider == null) continue;


                Bounds ThingBounds = ThingCollider.bounds;

                bool Spotted = false;

                Vector3 BottomCenter = ThingCollider.transform.position;
                Vector3 MiddleCenter = BottomCenter + ThingCollider.transform.up * ThingBounds.size.y / 2f;
                Vector3 TopCenter = BottomCenter + ThingCollider.transform.up * ThingBounds.size.y;

                Vector3 GoForward = ThingCollider.transform.forward * ThingBounds.size.z / 2f;
                Vector3 GoRight = ThingCollider.transform.right * ThingBounds.size.x / 2f;

                List<Vector3> LookTargets = new List<Vector3>()
                {
                    TopCenter, // Top of bounds
                    MiddleCenter + GoRight, //Middle Right
                    MiddleCenter + -GoRight, // Middle Left
                    MiddleCenter + GoForward, // Middle Forward
                    MiddleCenter + -GoForward, // Middle Backward
                    BottomCenter //Bottom center
                };

                foreach (GameObject eyeObject in Eyes)
                {
                    Eye eye = eyeObject.GetComponent<Eye>();
                    if (eye == null) continue;

                    

                    foreach (Vector3 LookTarget in LookTargets)
                    {
                        if (!eye.IsWithinFOV(LookTarget)) continue;

                        RaycastHit? rayCast = PhysicsTools.RaycastAt(eye.transform.position,
                            LookTarget, CurrentSightDistance.Value,
                            (int)OnlySeeMask);

                        Debug.DrawLine(eye.transform.position, LookTarget, Color.red, 1f);


                        if (rayCast != null)
                        {
                            if (rayCast.Value.collider != null && rayCast.Value.collider.gameObject != null)
                            {

                                if (rayCast.Value.collider.attachedRigidbody == ThingCollider.attachedRigidbody)
                                {



                                    if (!newThingsSeen.Keys.Contains(ThingNearby))
                                    {
                                        newThingsSeen.Add(ThingNearby,
                                        new LastSeenData()
                                        {
                                            WhenSeen = Time.timeAsDouble,
                                            LastSeen = rayCast.Value.collider.gameObject.transform.position,
                                            Distance = (rayCast.Value.collider.gameObject.transform.position - gameObject.transform.position).sqrMagnitude,
                                        });
                                    }


                                    Spotted = true;
                                    break;
                                }
                            }
                        }
                        if (Spotted) break;
                    }
                    if (Spotted) break;

                }

            }



        }

        ThingsSeen.Value.MatchDictionary(newThingsSeen);

        newThingsSeen = null;


    }
}
