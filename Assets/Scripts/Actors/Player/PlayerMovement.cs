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
        //CorrectRoll();
    }

    private void CorrectRoll()
    {
        bool NeedsRotated = false;
        float oldx = PlayerController.Player.transform.rotation.eulerAngles.x;
        float oldz = PlayerController.Player.transform.rotation.eulerAngles.z;

        float newx = 0;
        float newy = 0;
        float newz = 0;

        if (oldx > 45 && oldx < 315)
        {
            if (oldx > (45 + 315) / 2) //Add
            {
                newx = (315 + 45) - oldx;
            }
            else //Subtract
            {
                newx = (45 - 45) - oldx;
            }
            NeedsRotated = true;
        }

        if (oldz > 45 && oldz < 315)
        {
            if (oldz > (45 + 315) / 2) //Add
            {
                newz = (315 + 45) - oldz;

            }
            else //Subtract
            {
                newz = (45 - 45) - oldz;
            }
            NeedsRotated = true;
        }
        if (NeedsRotated)
        {
            PlayerController.Player.transform.Rotate(new Vector3(newx, newy, newz) * Time.deltaTime);
        }

    }

    void OnTentacleForward(InputValue value)
    {
        if (value.isPressed)
        {

            PlayerController.Player.Animator.SetBool("TentacleForward", true);
            TentacleDeployed = true;
        }
    }

    void OnTentacleBackward(InputValue value)
    {
        if (value.isPressed)
        {
            PlayerController.Player.Tentacles.UseBackTentacle = true;
            PlayerController.Player.Animator.SetBool("TentacleBack", true);
            TentacleDeployed = true;
        }
    }

    void OnTentacleLeft(InputValue value)
    {
        if (value.isPressed)
        {

            PlayerController.Player.Animator.SetBool("TentacleLeft", true);
            TentacleDeployed = true;
        }
    }

    void OnTentacleRight(InputValue value)
    {
        if (value.isPressed)
        {

            PlayerController.Player.Animator.SetBool("TentacleRight", true);
            TentacleDeployed = true;
        }
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
            if (PlayerController.Player.detection.CanTurnLeft())
            {
                PlayerController.Player.transform.Rotate(0, -PlayerController.Player.currentRotationSpeed * Time.deltaTime, 0);
            }


        }
        else if (RotateRight && !RotateLeft)
        {
            if (PlayerController.Player.detection.CanTurnRight())
            {
                PlayerController.Player.transform.Rotate(0, PlayerController.Player.currentRotationSpeed * Time.deltaTime, 0);
            }

        }


        bool Moved = false;

        if (IsMovingForward || IsMovingBackward)
        {
            if (IsMovingForward && !IsMovingBackward)
            {
                if (PlayerController.Player.detection.CanMoveForward())
                {
                    moveDir = PlayerController.Player.transform.forward;
                    Moved = true;
                }
            }
            else if (IsMovingBackward && !IsMovingForward)
            {
                if (PlayerController.Player.detection.CanMoveBackwards())
                {
                    moveDir = -PlayerController.Player.transform.forward;
                    Moved = true;
                }
            }
            Vector3 MoveVector = new Vector3(moveDir.x, PlayerController.Player.rb.velocity.y, moveDir.z);
            PlayerController.Player.transform.position = Vector3.MoveTowards(PlayerController.Player.rb.position, PlayerController.Player.rb.position + MoveVector, PlayerController.Player.currentMoveSpeed * Time.deltaTime);

        }


        PlayerController.Player.Animator.SetBool("Walking", Moved);






    }
}
