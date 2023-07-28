using Unity.VisualScripting;
using UnityEngine;

public class BlobAnimation : MonoBehaviour
{

    [Serialize] public GameObject BoneToMove;

    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        //MoveSomeBones();

    }

    public void MoveSomeBones()
    {
        BoneToMove.transform.Rotate(gameObject.transform.right, 45f);
    }

}
