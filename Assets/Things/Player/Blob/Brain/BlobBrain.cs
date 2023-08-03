using Unity.VisualScripting;
using UnityEngine;





public class BlobBrain : MonoBehaviour
{



    [Serialize] public BooleanVariable PlayerIsAlive;
    [Serialize] public GameObjectVariable PlayerGameObject;

    string GameObjectName = "";
    string PlayerObjectName = "";

    //Unity Functions
    protected void Awake()
    {
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



    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(10, 10, Screen.width / 10, Screen.height / 10), "Current: " + gameObject.name + " GO: " + GameObjectName + " PO: " + PlayerObjectName);
    //}

    protected void Start()
    {


    }


    protected void Update()
    {

    }

    protected void FixedUpdate()
    {




    }

    private void OnDestroy()
    {

    }












}
