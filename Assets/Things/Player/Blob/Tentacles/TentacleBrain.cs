using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBrain : MonoBehaviour, IAttackThings, ISpawn, IDespawn, IRetarget
{

    bool IsReady = false;

    private ImpulseVariable retarget;

    public ImpulseVariable Retarget { get { return retarget; } }
    private ImpulseVariable attackThings;
    
    public ImpulseVariable AttackThings { get { return attackThings; } }
    private ImpulseVariable spawn;
    public ImpulseVariable Spawn { get { return spawn; } }
    private ImpulseVariable despawn;
    public ImpulseVariable Despawn { get { return despawn; } }


    //Passed from parent
    public BooleanVariable IsAlive;
    public GameObjectVariable target;

    private bool DoingAction = false;
    private bool DoingActions = false;

    protected void Awake()
    {
        retarget = Instantiate(SOLibrary.instance.EmptyImpulseVariable);
        attackThings = Instantiate(SOLibrary.instance.EmptyImpulseVariable);
        spawn = Instantiate(SOLibrary.instance.EmptyImpulseVariable);
        despawn = Instantiate(SOLibrary.instance.EmptyImpulseVariable);
        
    }

    protected void Start()
    {
        StartCoroutine(Spawn.Value.Go());
    }

    protected void Update()
    {
        if (!DoingActions)
        {
            DoingActions = true;
            StartCoroutine(DoActions());
        }
        
      
 
    }

    private IEnumerator DoingActionWrapper(IEnumerator enumerator)
    {
        DoingAction = true;
        StartCoroutine(enumerator);
        while (enumerator.MoveNext())
        {
            yield return null;
        }
        DoingAction = false;
    }

    private IEnumerator DoActions()
    {
        if (!IsAlive.Value) StartCoroutine(DoingActionWrapper(Despawn.Value.Go()));
        while (DoingAction) yield return null;
        if (IsAlive.Value)
        {
            if (target.Value == null) StartCoroutine(DoingActionWrapper(Retarget.Value.Go()));
            while (DoingAction) yield return null;
            if (target.Value == null) BrainDead();
            while (DoingAction) yield return null;
            if (IsAlive.Value)
            {
                if (target.Value != null) StartCoroutine(DoingActionWrapper(AttackThings.Value.Go()));
                while (DoingAction) yield return null;
            }
        }
        DoingActions = false;
    }
    /// <summary>
    /// If used for animations it will double speed
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public IEnumerator WaitUntilAfterCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
        while (enumerator.MoveNext())
        {
            yield return null;
        }
    }

    public void BrainDead()
    {
        IsAlive.Value = false;
        StartCoroutine(Despawn.Value.Go());
    }


    public void SetImpulse(ImpulseVariable impulse, params WrappedFunc[] funcs)
    {
        impulse.Value = new Impulse();
        foreach (WrappedFunc func in funcs)
        {
            ImpulseStep step = new ImpulseStep(func.func, func.time);
            impulse.Value.ImpulseSteps.Add(step);
        }
    }

    
}
