using System.Collections.Generic;
using UnityEngine;

public class PigController : EnemyController
{



    [HideInInspector]
    public override float SqDistanceFromPlayer { get; set; }

    //private PigBrain brain;
    //private ImpulseStep EscapeStep;

    //private List<ImpulseStep> Escape()
    //{

    //    //if (target.Value != null)
    //    //{
    //    //    TransformVariable newTransform = FindSpawnLocationAndOrientation(tentacleRegions, target.Value.transform.position,
    //    //                parentRB.transform.rotation, parentRB.transform.position);
    //    //    transform.SetParent(null);
    //    //    transform.position = newTransform.position;
    //    //    TentacleVector = newTransform.vector;
    //    //    transform.rotation = Quaternion.identity;
    //    //    transform.rotation *= newTransform.rotation;
    //    //    transform.localScale = Vector3.one;
    //    //    transform.SetParent(parentObject.transform, true);

    //    //}

    //    return null;
    //}

    public void Go(GameObject parent, Rigidbody parentRB, List<Bounds> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {

        //parentObject = parent;
        //this.parentRB = parentRB;
        //this.tentacleRegions = tentacleRegions;
        //this.ObjectsSeen = ObjectsSeen;
        //transform.SetParent(parentObject.transform, true);

    }

    //private void InitializeBrain()
    //{

    //    EscapeStep = new ImpulseStep(StepAction: Escape,
    //                       OnlyDoIf: null);
    //    EscapeStep.impulseStepNameDebug = "RelocateStep";
    //    brain.AddImpulse(Impulse.ImpulseType.Move, null, EscapeStep);
    //}



    // Start is called before the first frame update
    protected override void Start()
    {

        base.Start();



    }

    protected override void Awake()
    {
        base.Awake();
        SqDistanceFromPlayer = 0;
    }

    protected override void Update()
    {
        base.Update();

    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();


    }







}
