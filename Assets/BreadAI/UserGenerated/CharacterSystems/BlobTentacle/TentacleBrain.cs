using System.Collections;

public class TentacleBrain : Brain
{


    public GameObjectVariable target;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {


    }

    protected override void Update()
    {
        base.Update();


    }


    protected override IEnumerator DoActions()
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
            }

        }
        else
        {

        }

        DoingActions = false;

    }






}
