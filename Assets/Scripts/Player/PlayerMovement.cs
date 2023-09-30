using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControllerScript;



    internal bool isFacingRight;
    internal float moveSpeedStart;
    internal float moveSpeedBlocking;
    private Vector3 velocity = Vector3.zero;
    [Range(0, 2f)] private float movementSmoothing;

    //Jump
    internal bool isGrounded;
    public Transform groundCheck;
    private float groundRadius = 0.1f;
    public LayerMask whatIsGround;
    private bool jump = false;
    public Vector2 movementVector = Vector2.zero;


    private void FixedUpdate()
    {

        if (movementVector.x > 0)
        {
            movementVector.x = 1;
            movementVector.y = 1;

        }

        else if (movementVector.x < 0)
        {
            movementVector.x = -1;
            movementVector.y = -1;

        }
        else if (movementVector.x == 0)
        {
            movementVector.x = 0;
            movementVector.y = 0;
        }

        if (isGrounded && jump)
        {
            playerControllerScript.animator.SetTrigger("jump");
            playerControllerScript.rb.AddForce(new Vector2(0f, playerControllerScript.jumpForce));

        }


        Vector3 targetVelocity = new Vector2(playerControllerScript.movementSpeed * 5f * movementVector.x, playerControllerScript.rb.velocity.y);
        playerControllerScript.rb.velocity = Vector3.SmoothDamp(playerControllerScript.rb.velocity, targetVelocity, ref velocity, movementSmoothing);


    }

    private void Update()
    {
        Flip();
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
    }

    private void Start()
    {
        moveSpeedStart = playerControllerScript.movementSpeed;
        moveSpeedBlocking = playerControllerScript.movementSpeed / 4;       
    }

    //Salta
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.action.triggered;
    }

    //Se mueve
    public void OnMove(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
        playerControllerScript.animator.SetBool("move", true);


        //Si deja de moverse
        if (context.canceled)
        {
            playerControllerScript.animator.SetBool("move", false);

        }

        //Si está bloqueando
        if (playerControllerScript.weaponScript.blocking)
        {
            playerControllerScript.movementSpeed = moveSpeedBlocking;
        }
        //Si no está bloqueando ni atacando
        if (!playerControllerScript.weaponScript.blocking && !playerControllerScript.weaponScript.attacking)
        {
            playerControllerScript.movementSpeed = moveSpeedStart;

            if(movementVector.x == 0)
            {
                playerControllerScript.movementSpeed = 0;
            }
        }


    }
    //Se flipea xabal
    internal void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if (transform.position.x > playerControllerScript.collisionsScript.otherPlayer.transform.position.x)
        {
            rotation.y = 180f;
            isFacingRight = false;

        }
        else
        {
            rotation.y = 0f;

            isFacingRight = true;

        }

        transform.eulerAngles = rotation;
    }

    

}
