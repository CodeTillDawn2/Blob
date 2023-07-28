using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlobMovement : MonoBehaviour
{
    [Header("Stat Block")]
    [Serialize] public FloatVariable CurrentRotationSpeed;
    [Serialize] public FloatVariable CurrentMoveSpeed;
    [Serialize] public PlayerStatsBase StartingStats;
    [Serialize] public QuaternionVariable BodyRotation;
    [Serialize][Tooltip("Rigidbody to control")] public GameObjectVariable RigidbodyObject;

    private Rigidbody rb;

    protected Vector3 moveDir;
    protected Vector3 lastMovedVector;
    private bool IsMovingForward = false;
    private bool IsMovingBackward = false;
    private bool RotateLeft = false;
    private bool RotateRight = false;

    private void Awake()
    {
        lastMovedVector = new Vector3(1, 0f, 0f);

    }

    void Start()
    {
        rb = RigidbodyObject.Value.GetComponent<Rigidbody>();
        CurrentMoveSpeed.Value = StartingStats.MoveSpeed;
        CurrentRotationSpeed.Value = StartingStats.RotateSpeed;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
        rb.angularVelocity = Vector3.zero;
        BodyRotation.Value = rb.rotation;
        //rb.AddTorque(-rb.angularVelocity * .8f, ForceMode.VelocityChange);

    }

    void OnForward(InputValue value)
    {
        if (value.isPressed)
        {
            IsMovingForward = true;
        }
        else
        {
            IsMovingForward = false;
        }

    }


    void OnBackward(InputValue value)
    {
        if (value.isPressed)
        {
            IsMovingBackward = true;
        }
        else
        {
            IsMovingBackward = false;
        }


    }

    void OnTurnLeft(InputValue value)
    {
        if (value.isPressed)
        {
            RotateLeft = true;
        }
        else
        {
            RotateLeft = false;
        }
    }

    void OnTurnRight(InputValue value)
    {
        if (value.isPressed)
        {

            RotateRight = true;
        }
        else
        {
            RotateRight = false;
        }
    }


    void Move()
    {

        if (IsMovingForward || IsMovingBackward)
        {
            if (IsMovingForward)
            {



                Vector3 targetChange = transform.forward * CurrentMoveSpeed.Value;
                float TargetChangeMagnitude = targetChange.magnitude;

                if (TargetChangeMagnitude > CurrentMoveSpeed.Value)
                {
                    targetChange *= (CurrentMoveSpeed.Value / TargetChangeMagnitude);
                }

                rb.AddForce(targetChange - rb.velocity, ForceMode.VelocityChange);


            }
            else if (IsMovingBackward)
            {

                Vector3 targetChange = -transform.forward * CurrentMoveSpeed.Value;
                float TargetChangeMagnitude = targetChange.magnitude;

                if (TargetChangeMagnitude > CurrentMoveSpeed.Value)
                {
                    targetChange *= (CurrentMoveSpeed.Value / TargetChangeMagnitude);
                }

                rb.AddForce(targetChange - rb.velocity, ForceMode.VelocityChange);
            }

            //PlayerManager.me.Animator.SetBool("Walking", true);
        }
        else
        {
            //PlayerManager.me.Animator.SetBool("Walking", false);
        }

        Vector3 axis = new Vector3(0f, 1f, 0f);
        float angle = 0;

        if (RotateLeft && !RotateRight)
        {
            angle = -15f;
        }
        else if (RotateRight && !RotateLeft)
        {
            angle = 15f;
        }
        rb.AddTorque(axis.normalized * angle * CurrentRotationSpeed.Value, ForceMode.Acceleration);






    }

}
