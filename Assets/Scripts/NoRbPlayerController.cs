using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NoRbPlayerController : MonoBehaviour
{
    //playerheight is set to controller height
    public float playerHeight;
    private float gravity = -9.81f;

    //player position logs
    private Vector3 playerPos;
    private Vector3 lastPlayerPos;

    [SerializeField, Tooltip("Player Character prefab")] GameObject playerPrefab;
    [SerializeField, Tooltip("Crouch height divisor, default is 2. Which sets crouch to half of the player height")] public float crouchHeight = 2f;
    [SerializeField, Tooltip("Player jump height")] float jumpHeight = 1f;
    [SerializeField, Tooltip("The base movement speed for the player")] public float baseSpeed = 5f;
    [SerializeField, Tooltip("Allows us to view the current speed, not neccessary but useful. Value should NOT be edited manually")] public float currentSpeed;
    
    
    //bools
    public bool isGrounded;
    public bool isJumping = false;
    public bool isCrouching = false;
    public bool isSprinting = false;
    public bool isMoving = false;
    public bool isSneaking = false;

    private CharacterController controller;
    private PlayerAttributes attributes;
    public Vector3 velocity;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        attributes = GetComponent<PlayerAttributes>();
        playerHeight = controller.height;
        crouchHeight = playerHeight / crouchHeight;

        
    }

    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //logging player position at start of loop
        lastPlayerPos = transform.position;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            //because velocity.y is contantly decreasing due to gravity, this set it to -1, if set to 0 isGrounded no longer works properly
            velocity.y = -1f;
            isJumping = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);
        
        UpdateSpeed();
        
        #region *** Jump ***

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;

        }

        #endregion


        

        //new
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
        {
            isCrouching = true;
            controller.height /= 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            isCrouching = false;
            controller.height *= 2;
        }

        if (!float.IsNaN(velocity.y) && !float.IsInfinity(velocity.y))
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        //
        //isMoving checks 

        //logging player position at end of loop
        playerPos = transform.position;

        //setting isMoving to true if position don't match
        isMoving = playerPos != lastPlayerPos;

        //sneaking check for isMoving
        isSneaking = isCrouching && isMoving;

        //edited here
        //sprinting check for isMoving
        if (currentSpeed > baseSpeed && isMoving)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        ////////////////////////////
    }


    //NEW ***********
    void UpdateSpeed()
    {
        currentSpeed = baseSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && !attributes.noStamina)
        {
            currentSpeed *= 2;
        }

        if (isCrouching)
        {
            currentSpeed /= 2;
        }

        isSprinting = currentSpeed > baseSpeed;
    }
   
    ////////////////////////
}
