using UnityEngine;
using UnityEngine.Rendering;



public class NoRbPlayerController : MonoBehaviour
{
    //[SerializeField, Tooltip("Player prefab")] GameObject player;
    //[SerializeField, Tooltip("Player height")] float playerHeight;
    [SerializeField] float currentSpeed; 
    
    public float baseSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //playerHeight = player.transform.position.y;
        //float crouchHeight = playerHeight / 2;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if ((isGrounded && velocity.y < 0))
        {
            velocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
       
        currentSpeed = baseSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 2;
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            //todo crouch logic
        }
        
        
        //why is the player taking off?????????
        //its something to do with this velocity line
        
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); 
    }

}
