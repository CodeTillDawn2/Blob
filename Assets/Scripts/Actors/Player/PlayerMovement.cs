using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static HelperClasses;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stat Block")]
    [Serialize] public FloatVariable CurrentRotationSpeed;
    [Serialize] public FloatVariable CurrentMoveSpeed;

    public GameObject Eye_FrontTopRight;
    public GameObject Eye_FrontMiddleRight;
    public GameObject Eye_LeftTopFront;
    public GameObject Eye_LeftMiddleFront;
    public GameObject Eye_FrontTopLeft;
    public GameObject Eye_FrontMiddleLeft;
    public GameObject Eye_RightTopFront;
    public GameObject Eye_RightMiddleFront;

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
        rb = GetComponent<Rigidbody>();


    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();

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






        if (RotateLeft && !RotateRight)
        {
            //if (PlayerController.Player.detection.CanTurnLeft())
            //{
            transform.Rotate(0, -CurrentRotationSpeed.Value * Time.deltaTime, 0);
            //}


        }
        else if (RotateRight && !RotateLeft)
        {
            //if (PlayerController.Player.detection.CanTurnRight())
            //{
            transform.Rotate(0, CurrentRotationSpeed.Value * Time.deltaTime, 0);
            //}

        }



        if (IsMovingForward || IsMovingBackward)
        {
            if (IsMovingForward)
            {

                moveDir = transform.forward;

                Vector3 MoveVector = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

                float CanMoveDistance = CanMoveInDirection(CurrentMoveSpeed.Value * Time.deltaTime, transform.forward);
                if (CanMoveDistance > 0)
                {
                    transform.position = Vector3.MoveTowards(rb.position, rb.position + MoveVector, CanMoveDistance);
                }

            }
            else if (IsMovingBackward)
            {

                moveDir = -transform.forward;

                Vector3 MoveVector = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

                float CanMoveDistance = CanMoveInDirection(CurrentMoveSpeed.Value * Time.deltaTime, -transform.forward);
                if (CanMoveDistance > 0)
                {
                    transform.position = Vector3.MoveTowards(rb.position, rb.position + MoveVector, CanMoveDistance);
                }
            }

            //PlayerManager.me.Animator.SetBool("Walking", true);
        }
        else
        {
            //PlayerManager.me.Animator.SetBool("Walking", false);
        }









    }

    public bool CanTurnRight()
    {

        foreach (GameObject eyeGO in new List<GameObject>() { Eye_FrontTopLeft, Eye_FrontMiddleLeft, Eye_RightTopFront, Eye_RightMiddleFront })
        {
            Eye eye = eyeGO.GetComponent<Eye>();
            if (eye != null)
            {
                if (eye.hit != null && eye.hitObject != null)
                {
                    Shortcuts.UnityLayers FoundLayer = (Shortcuts.UnityLayers)eye.hitObject.layer;
                    if (FoundLayer == Shortcuts.UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                    {
                        return false;
                    }
                }
            }

        }

        return true;

    }

    public bool CanTurnLeft()
    {

        foreach (GameObject eyeGO in new List<GameObject>() { Eye_FrontTopRight, Eye_FrontMiddleRight, Eye_LeftTopFront, Eye_LeftMiddleFront })
        {
            Eye eye = eyeGO.GetComponent<Eye>();
            if (eye != null)
            {
                if (eye.hit != null && eye.hitObject != null)
                {
                    Shortcuts.UnityLayers FoundLayer = (Shortcuts.UnityLayers)eye.hitObject.layer;
                    if (FoundLayer == Shortcuts.UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                    {
                        return false;
                    }
                }
            }


        }

        return true;

    }

    public float CanMoveInDirection(float distance, Vector3 castDirection)
    {

        RaycastHit hitResult;

        if (rb.SweepTest(castDirection, out hitResult, distance, QueryTriggerInteraction.Ignore))
        {

            if (hitResult.collider.gameObject.layer == (int)Shortcuts.UnityLayers.Ground)
            {
                return hitResult.distance;
            }




        }

        return distance;

    }
}
