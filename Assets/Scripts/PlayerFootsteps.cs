using System.Collections;
using FMODUnity;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
[SerializeField, Tooltip("If empty, drag the game object associated with the player into this field")] private GameObject playerGameObject;
private NoRbPlayerController playerController;
private StudioEventEmitter playerFootstepsEventEmitter;
private StudioParameterTrigger terrainTypeParameterFMOD;
private float currentSpeedMultiplier;
[SerializeField, Tooltip("Used for visual purposes only. Please do not edit this in the inspector")] private float footstepsDelayTime = 0.333f;
private bool coroutineReset = true;
[SerializeField, Tooltip("Used to determine what game layers the footsteps terrain type raycast should look for")] private LayerMask targetLayers;
private string hitLayer;

//this variable is used to determine the terrain material that is underneath the player
//0 = unknown terrain, 1 = grass, 2 = stone
public int terrainType;

private float debugFMODParameter;

void Update()
{
    IsPlayerMovingAndGrounded();
    if (IsPlayerMovingAndGrounded() && coroutineReset)
    {
        coroutineReset = false;

        //determine a current speed multiplier from the player controller script to be used to alter footstep speed
        currentSpeedMultiplier = playerController.currentSpeed / playerController.baseSpeed;

        //get the reciprocal of the current speed multiplier so that as the multiplier increases the delay time gets smaller, 
        //divide this by the base speed multiplied by an arbitrary amount
        footstepsDelayTime = 1 / currentSpeedMultiplier / (playerController.baseSpeed * 0.66f);
        
        playerFootstepsEventEmitter.Play();
        StartCoroutine(FootstepsDelay());
    }
}

    void FixedUpdate()
    {
        if(Physics.Raycast(playerGameObject.transform.position, Vector3.down, out RaycastHit hitInfo, 100f, targetLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(playerController.gameObject.transform.position, playerController.gameObject.transform.TransformDirection(Vector3.down) * hitInfo.distance, Color.red);
            //Debug.Log(hitInfo.collider.gameObject.name + " was hit!");
            //Debug.Log(hitInfo.collider.gameObject.layer);

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
            
            Debug.DrawRay(playerController.gameObject.transform.position, playerController.gameObject.transform.TransformDirection(Vector3.down) * 100f, Color.green);
            //Debug.Log("No hit!");      
        }

        playerFootstepsEventEmitter.EventInstance.setParameterByName("terrainType", terrainType, false);
        playerFootstepsEventEmitter.EventInstance.getParameterByName("terrainType", out debugFMODParameter);
        //Debug.Log(debugFMODParameter);
    }

    void Start()
{
    playerController = playerGameObject.GetComponent<NoRbPlayerController>();
    playerFootstepsEventEmitter = playerGameObject.GetComponent<StudioEventEmitter>();
}

bool IsPlayerMovingAndGrounded()
{
    //check if the player is currently moving AND if they are currently grounded
    if (playerController.isMoving && playerController.isGrounded)
    {
        return true;
    }
    else
    {
        return false;
    }
}

IEnumerator FootstepsDelay()
{
    yield return new WaitForSecondsRealtime(footstepsDelayTime);
    coroutineReset = true;
}
}