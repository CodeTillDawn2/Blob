using Unity.VisualScripting;
using UnityEngine;

public class BlobBody : MonoBehaviour
{
    [Header("Stat Block")]
    [Serialize] public GameObjectVariable BlobBodyObject;



    private void Awake()
    {


    }

    void Start()
    {
        BlobBodyObject.Value = gameObject;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {


    }


}
