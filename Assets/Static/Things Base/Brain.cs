using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Brain : CharacterSystem
{

    // Abstract property to ensure derived classes implement it


    protected bool IsReady = false;

    protected List<ImpulseVariable> SearchImpulses;

    protected List<ImpulseVariable> AttackImpulses;

    protected List<ImpulseVariable> SpawnImpulses;

    protected List<ImpulseVariable> DespawnImpulses;

    protected List<ImpulseVariable> MoveImpulses;

    [HideInInspector]
    public BooleanVariable IsAlive;

    protected bool DoingAction = false;
    protected bool DoingActions = false;
    protected override void Awake()
    {
        base.Awake();
        SearchImpulses = new List<ImpulseVariable>();
        AttackImpulses = new List<ImpulseVariable>();
        SpawnImpulses = new List<ImpulseVariable>();
        DespawnImpulses = new List<ImpulseVariable>();
        MoveImpulses = new List<ImpulseVariable>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
        if (!DoingActions)
        {
            DoingActions = true;
            StartCoroutine(DoActions());
        }
    }

    protected abstract IEnumerator DoActions();

    public void AddImpulse(Impulse.ImpulseType impuseType, Func<bool> evaluator, params ImpulseStep[] steps)
    {

        List<ImpulseVariable> ImpulseListToEdit;
        switch (impuseType)
        {
            case Impulse.ImpulseType.Spawn:
                ImpulseListToEdit = SpawnImpulses;
                break;
            case Impulse.ImpulseType.Despawn:
                ImpulseListToEdit = DespawnImpulses;
                break;
            case Impulse.ImpulseType.Attack:
                ImpulseListToEdit = AttackImpulses;
                break;
            case Impulse.ImpulseType.Move:
                ImpulseListToEdit = MoveImpulses;
                break;
            case Impulse.ImpulseType.Search:
                ImpulseListToEdit = SearchImpulses;
                break;
            default:
                Debug.LogWarning("Attempt to add unknown impulse type. Check Impulse type enum.");
                return;
        }



        Impulse impulse = new Impulse();
        foreach (ImpulseStep step in steps)
        {
            impulse.ImpulseSteps.Add(step);
        }
        foreach (ImpulseVariable impulseVariable in ImpulseListToEdit)
        {
            if (impulseVariable.Value.impulseType == impuseType && impulseVariable.Value.ImpulseSteps.SequenceEqual(impulse.ImpulseSteps))
            {
                impulse = null;
                return;
            }
        }

        ImpulseVariable impulseVar = SOLibrary.Create<ImpulseVariable>();
        impulseVar.Value = impulse;
        ImpulseListToEdit.Add(impulseVar);

    }

}
