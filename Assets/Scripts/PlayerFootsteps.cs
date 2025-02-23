using Debug = UnityEngine.Debug;
using FMOD.Studio;
using System.Collections;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField, Tooltip("If empty, drag the game object associated with the player into this field")] private GameObject playerGameObject;
    private NoRbPlayerController playerController;
    private float currentSpeedMultiplier;
    [SerializeField, Tooltip("Used for visual purposes only. Please do not edit this in the inspector")] private float secondsBetweenFootsteps = 0.333f;
    private bool coroutineReset = true;
    [SerializeField, Tooltip("Used to determine what game layers the footsteps terrain type raycast should look for")] private LayerMask targetLayers;
    private string hitLayer;
    public bool hasJumped;

    //FMOD audio
    private EventInstance playerFootstepsEventInstance;
    private EventInstance playerJumpEventInstance;
    private EventInstance playerLandEventInstance;
    //this variable is used to determine the terrain material that is underneath the player
    //0 = unknown terrain, 1 = grass, 2 = stone
    public int terrainType = 0;
    public int isRunning = 0;


    void Update()
    {
        FootstepsAudio();
        PlayerJumpAudio();
        PlayerLandAudio();
    }


    void FixedUpdate()
    {
        if (Physics.Raycast(playerGameObject.transform.position, Vector3.down, out RaycastHit hitInfo, 100f, targetLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(playerController.gameObject.transform.position, playerController.gameObject.transform.TransformDirection(Vector3.down) * hitInfo.distance, Color.red);

            hitLayer = LayerMask.LayerToName(hitInfo.collider.gameObject.layer);
            if(hitLayer == "Grass")
            {
                terrainType = 1;
            }
            else if(hitLayer == "Stone")
            {
                terrainType = 2;
            }
            else
            {
                terrainType = 0;              
            }
        }
        else
        {
            hitLayer = "Unknown";
            terrainType = 0;     
            
            //Debug.DrawRay(playerController.gameObject.transform.position, playerController.gameObject.transform.TransformDirection(Vector3.down) * 100f, Color.green);   
        }

    }


    void Start()
    {
        if (playerGameObject == null)
        {
            Debug.LogError(GetType().Name + ".cs - No player gameObject assigned in the inspector!");
        }

        playerController = playerGameObject.GetComponent<NoRbPlayerController>();
        hasJumped = false;
    }


    IEnumerator FootstepsDelay()
    {
        yield return new WaitForSecondsRealtime(secondsBetweenFootsteps);
        coroutineReset = true;
    }


    void FootstepsAudio()
    {        
        if (playerController.isMoving && playerController.isGrounded && coroutineReset)
        {
            coroutineReset = false;

            // Convert bool to int for FMOD
            isRunning = playerController.isSprinting ? 1 : 0;

            //determine a current speed multiplier from the player controller script to be used to alter footstep speed
            currentSpeedMultiplier = playerController.currentSpeed / playerController.baseSpeed;

            //get the reciprocal of the current speed multiplier so that as the multiplier increases the delay time gets smaller, 
            //divide this by the base speed multiplied by an arbitrary amount
            secondsBetweenFootsteps = 1 / currentSpeedMultiplier / (playerController.baseSpeed * 0.5f);

            //FMOD audio
            playerFootstepsEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.playerFootsteps);
            playerFootstepsEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            playerFootstepsEventInstance.setParameterByName("Player_Footsteps.terrainType", terrainType);
            playerFootstepsEventInstance.setParameterByName("Player_Footsteps.isRunning", isRunning);      
            playerFootstepsEventInstance.start();
            playerFootstepsEventInstance.release();
            StartCoroutine(FootstepsDelay());
        }    
    }


    void PlayerJumpAudio()
    {
        if (playerController.isJumping && !hasJumped)
        {
            //player jump audio event
            playerJumpEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.playerJump);
            playerJumpEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            playerJumpEventInstance.setParameterByName("Player_Footsteps.terrainType", terrainType);
            playerJumpEventInstance.setParameterByName("Player_Footsteps.isRunning", isRunning);      
            playerJumpEventInstance.start();
            playerJumpEventInstance.release();

            hasJumped = true;
        }
    }


    void PlayerLandAudio()
    {
        if (playerController.isGrounded && hasJumped)
        {
            //player land audio event
            playerJumpEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.playerLand);
            playerJumpEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            playerJumpEventInstance.setParameterByName("Player_Footsteps.terrainType", terrainType);
            playerJumpEventInstance.setParameterByName("Player_Footsteps.isRunning", isRunning);      
            playerJumpEventInstance.start();
            playerJumpEventInstance.release();

            hasJumped = false;
        }
    }
}
