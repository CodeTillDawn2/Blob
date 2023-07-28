using Unity.VisualScripting;
using UnityEngine;





public class BlobBrain : MonoBehaviour
{



    [Serialize] public BooleanVariable PlayerIsAlive;
    [Serialize] public GameObjectVariable PlayerGameObject;




    //Unity Functions
    protected void Awake()
    {

        PlayerGameObject.Value = gameObject;
        PlayerIsAlive.Value = true;



    }

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
