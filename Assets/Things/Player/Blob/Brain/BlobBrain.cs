using System;
using System.Collections;
using Unity.VisualScripting;





public class BlobBrain : Brain
{


    [Serialize] public BooleanVariable PlayerIsAlive;
    [Serialize] public GameObjectVariable PlayerGameObject;

    string GameObjectName = "";
    string PlayerObjectName = "";

    //Unity Functions
    protected override void Awake()
    {
        base.Awake();
        MomentumSensor moSensor = GetComponent<MomentumSensor>();
        if (moSensor == null)
        {
            gameObject.AddComponent<MomentumSensor>();
        }

        PlayerGameObject.Value = gameObject;
        GameObjectName = gameObject.name;
        PlayerObjectName = PlayerGameObject.Value.name;
        PlayerIsAlive.Value = true;



    }


    protected override IEnumerator DoActions()
    {
        yield return 0;
    }

    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(10, 10, Screen.width / 10, Screen.height / 10), "Current: " + gameObject.name + " GO: " + GameObjectName + " PO: " + PlayerObjectName);
    //}

    protected override void Start()
    {
        base.Update();

    }


    protected override void Update()
    {
        base.Update();
    }



}
