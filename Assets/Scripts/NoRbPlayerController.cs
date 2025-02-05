using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;



public class NoRbPlayerController : MonoBehaviour
{
    
    [SerializeField, Tooltip("Player height")] float playerHeight;
    [SerializeField, Tooltip("Crouch height, default is half of player height")] float crouchHeight;
    [SerializeField] float currentSpeed;
   
    public float baseSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;
    public bool isSprinting = false;

    void Start()
    {
        // assigns the controller to the character controller, also uses the controller to set the height of the player for the crouch mechanic
        controller = GetComponent<CharacterController>();
        playerHeight = controller.height;
        crouchHeight = playerHeight / 2;
    }

    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //enures player is grounded
        isGrounded = controller.isGrounded;
        if ((isGrounded && velocity.y < 0))
        {
            velocity.y = -2f;
            
        }


        // movement variables
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        
        //Moves the player
        controller.Move(move * currentSpeed * Time.deltaTime);
        currentSpeed = baseSpeed;

        //sprint
        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting)
        {
            isSprinting = true;
           

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting)
        {
            isSprinting = false;
        }
        if (isSprinting)
        {
            currentSpeed *= 2;
        }

        
        
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }


        //crouch
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
        {
            controller.height = crouchHeight;
            isCrouching = true;
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            controller.height = playerHeight;
            isCrouching = false;

        }

        if (isCrouching)
        {
            currentSpeed /= 2;
        }
        

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
