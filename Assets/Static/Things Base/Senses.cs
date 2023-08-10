using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Senses : CharacterSystem
{

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region Interface Fields

    #endregion

    #region Interface Methods

    #endregion

    #region Private methods


    #endregion



    protected virtual void Update()
    {
        if (this is ICanSee iCanSee && this is IHaveEyes iHaveEyes && this is IUseSightBox iUseSightBox)
        {
            SeeWithEyes(iCanSee, iHaveEyes, iUseSightBox);
        }
        
    }

    protected virtual void FixedUpdate()
    {
        if (this is ICanSee me && this is IUseSightBox meSB)
        {
            if (meSB.CurrentSightBoxSize.Value != me.SightDistance.Value)
            {
                meSB.sightBox.size = new Vector3(me.SightDistance.Value, me.SightDistance.Value, me.SightDistance.Value);
            }
        }
       
    }

    protected void OnTriggerEnter(Collider col)
    {
        if (this is IUseSightBox me)
        {
            if (col.gameObject == null) return;
            
            if (col.gameObject.layer != (int)me.ThingNearbyFilter)
            {
                if (!me.ThingsNearby.Contains(col.gameObject)) me.ThingsNearby.Add(col.gameObject);
            }
        }
      
    }


    protected void OnTriggerExit(Collider col)
    {
        if (this is IUseSightBox me)
        {
            if (col.gameObject == null) return;
            me.ThingsNearby.Remove(col.gameObject);
        }
      
    }
    protected virtual void SeeWithEyes(ICanSee iCanSee, IHaveEyes iHaveEyes, IUseSightBox iUseSightBox)
    {

            Dictionary<GameObject, LastSeenData> newThingsSeen = new Dictionary<GameObject, LastSeenData>();

            //See things
            foreach (GameObject ThingNearby in iUseSightBox.ThingsNearby.Where(x => x != null))
            {
                if ((ThingNearby.transform.position - transform.position).sqrMagnitude <= iCanSee.SightDistance.Value * iCanSee.SightDistance.Value)
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

                    foreach (GameObject eyeObject in iHaveEyes.Eyes.Items)
                    {
                        Eye eye = eyeObject.GetComponent<Eye>();
                        if (eye == null) continue;



                        foreach (Vector3 LookTarget in LookTargets)
                        {
                            if (!eye.IsWithinFOV(LookTarget)) continue;


                            RaycastHit? rayCast = PhysicsTools.RaycastAt(eye.transform.position,
                                LookTarget, iCanSee.SightDistance.Value,
                                (int)iCanSee.OnlySeeMask);

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

            iCanSee.ThingsSeen.Value.MatchDictionary(newThingsSeen);

            newThingsSeen = null;


        
    }


        
}
