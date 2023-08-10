using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlobLocomotion : Locomotion
{
    public static Type[] _expectedStatsInterfaces = { typeof(IHaveMoveSpeed) };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;

    [Header("Stat Block")]
    [Serialize] public FloatVariable CurrentRotationSpeed;
    [Serialize] public FloatVariable CurrentMoveSpeed;
    [Serialize] public BlobConfiguration StartingStats;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public QuaternionVariable BodyRotation;
    /// <summary>
    /// Ground only, for climbing stairs
    /// </summary>
    [Serialize] public LayerMask GroundLayerMask;
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
        CurrentMoveSpeed.Value = StartingStats.MoveSpeed.Value;
        CurrentRotationSpeed.Value = StartingStats.RotateSpeed.Value;
    }

    void Update()
    {
        DetectSteps();
    }

    public void DetectSteps()
    {
        Vector3 TopRightPoint = transform.TransformPoint(new Vector3(BlobDims.Value.x / 2f, BlobDims.Value.y, BlobDims.Value.z / 2 + BlobDims.Value.z / 20));
        Vector3 TopLeftPoint = transform.TransformPoint(new Vector3(-BlobDims.Value.x / 2f, BlobDims.Value.y, BlobDims.Value.z / 2 + BlobDims.Value.z / 20));
        Vector3 BottomRightPoint = transform.TransformPoint(new Vector3(BlobDims.Value.x / 2f, 0, BlobDims.Value.z / 2f + BlobDims.Value.z / 20));
        Vector3 BottomLeftPoint = transform.TransformPoint(new Vector3(-BlobDims.Value.x / 2f, 0, BlobDims.Value.z / 2f + BlobDims.Value.z / 20));


        Debug.DrawLine(TopRightPoint
                , BottomRightPoint - new Vector3(0, BlobDims.Value.y * .5f, 0), Color.red);
        Debug.DrawLine(TopLeftPoint, BottomLeftPoint - new Vector3(0, BlobDims.Value.y * .5f, 0), Color.red);

        bool FoundLeft = false;
        bool FoundRight = false;
        //Left
        if (Physics.Raycast(TopLeftPoint, -transform.up, out RaycastHit hit, BlobDims.Value.y * 1.5f, GroundLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            FoundLeft = true;
        }
        //Right
        if (Physics.Raycast(TopRightPoint, -transform.up, out RaycastHit hit2, BlobDims.Value.y * 1.5f, GroundLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            FoundRight = true;
        }



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
