using System;
using System.Collections;

public class PigBrain : Brain
{

    public static Type[] _expectedStatsInterfaces = { typeof(IHaveMoveSpeed) };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;

    public GameObjectVariable target;



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Update()
    {
        base.Update();


    }


    protected override IEnumerator DoActions()
    {

        if (IsAlive.Value)
        {
            if (target.Value != null)
            {

                yield return StartCoroutine(MoveImpulses[0].Value.Go());


            }

        }
        else
        {

        }

        DoingActions = false;

    }





}
