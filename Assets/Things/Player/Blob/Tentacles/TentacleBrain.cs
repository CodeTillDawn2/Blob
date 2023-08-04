using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleBrain : MonoBehaviour//, IAttack, ISpawn, IDespawn, ISearch
{

    bool IsReady = false;

    private List<ImpulseVariable> SearchImpulses;

    private List<ImpulseVariable> AttackImpulses;

    private List<ImpulseVariable> SpawnImpulses;

    private List<ImpulseVariable> DespawnImpulses;

    private List<ImpulseVariable> MoveImpulses;



    //Passed from parent
    public BooleanVariable IsAlive;
    public GameObjectVariable target;

    private bool DoingAction = false;
    private bool DoingActions = false;

    protected void Awake()
    {
        SearchImpulses = new List<ImpulseVariable>();
        AttackImpulses = new List<ImpulseVariable>();
        SpawnImpulses = new List<ImpulseVariable>();
        DespawnImpulses = new List<ImpulseVariable>();
        MoveImpulses = new List<ImpulseVariable>();
    }

    protected void Start()
    {
        //StartCoroutine(SpawnImpulses[0].Value.Go());

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

    public void StartImpulse(IEnumerator impulse, bool waitForFinish)
    {
        if (waitForFinish)
        {
            StartCoroutine(DoingActionWrapper(impulse));
            return;
        }
        StartCoroutine(impulse);
    }

    private IEnumerator DoActions()
    {

        if (IsAlive.Value)
        {
            if (target.Value == null)
            {

                yield return StartCoroutine(SearchImpulses[0].Value.Go());
                if (target.Value != null)
                {
   
                    yield return StartCoroutine(MoveImpulses[0].Value.Go());
                }
                //else
                //{
                //    print("Brain: I have no target. Shrinking.");
                //    yield return StartCoroutine(DespawnImpulses[0].Value.Go());
                //}
            }
            //else
            //{
            //    print("Brain: I have a target and am alive. Attacking!");
            //    yield return StartCoroutine(AttackImpulses[0].Value.Go());
            //}
        }
        else
        {

        }

        //if (!IsAlive.Value)
        //{
        //    print("Brain: I am not alive. Despawn.");
        //    yield return StartCoroutine(DespawnImpulses[0].Value.Go());
        //}
        DoingActions = false;

    }



    public void BrainDead()
    {
        IsAlive.Value = false;
        StartCoroutine(DoingActionWrapper(DespawnImpulses[0].Value.Go()));
    }

    public Func<bool> WrapDelegates(params Func<bool>[] delegates)
    {
        Func<bool> returnDelegate = delegates[0];
        if (delegates.Count() == 0) return returnDelegate;
        for (int i = 1; i < delegates.Length; i++)
        {
            Func<bool> del = delegates[i];
            returnDelegate = () => returnDelegate() && del();
        }

        return returnDelegate;
    }

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

        ImpulseVariable impulseVar = Instantiate(SOLibrary.instance.EmptyImpulseVariable);
        impulseVar.Value = impulse;
        ImpulseListToEdit.Add(impulseVar);

    }


}
