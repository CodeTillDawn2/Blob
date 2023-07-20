using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    protected Vector3 moveDir;
    protected Vector3 lastMovedVector;
    private bool IsMovingForward = false;
    private bool IsMovingBackward = false;
    private bool RotateLeft = false;
    private bool RotateRight = false;

    private bool TentacleDeployed = false;

    private void Awake()
    {
        lastMovedVector = new Vector3(1, 0f, 0f);

    }

    void Start()
    {


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
                PlayerController.me.transform.Rotate(0, -PlayerController.me.currentRotationSpeed * Time.deltaTime, 0);
            //}


        }
        else if (RotateRight && !RotateLeft)
        {
            //if (PlayerController.Player.detection.CanTurnRight())
            //{
                PlayerController.me.transform.Rotate(0, PlayerController.me.currentRotationSpeed * Time.deltaTime, 0);
            //}

        }



        if (IsMovingForward || IsMovingBackward)
        {
            if (IsMovingForward)
            {
 
                    moveDir = PlayerController.me.transform.forward;

                Vector3 MoveVector = new Vector3(moveDir.x, PlayerController.me.rb.velocity.y, moveDir.z);

                float CanMoveDistance = PlayerController.me.Brain.CanMoveInDirection(PlayerController.me.currentMoveSpeed * Time.deltaTime, PlayerController.me.transform.forward);
                if (CanMoveDistance > 0)
                {
                    PlayerController.me.transform.position = Vector3.MoveTowards(PlayerController.me.rb.position, PlayerController.me.rb.position + MoveVector, CanMoveDistance);
                }

            }
            else if (IsMovingBackward)
            {
      
                moveDir = -PlayerController.me.transform.forward;

                Vector3 MoveVector = new Vector3(moveDir.x, PlayerController.me.rb.velocity.y, moveDir.z);

                float CanMoveDistance = PlayerController.me.Brain.CanMoveInDirection(PlayerController.me.currentMoveSpeed * Time.deltaTime, -PlayerController.me.transform.forward);
                if (CanMoveDistance > 0)
                {
                    PlayerController.me.transform.position = Vector3.MoveTowards(PlayerController.me.rb.position, PlayerController.me.rb.position + MoveVector, CanMoveDistance);
                }
            }

            PlayerController.me.Animator.SetBool("Walking", true);
        }
        else
        {
            PlayerController.me.Animator.SetBool("Walking", false);
        }


        






    }
}
