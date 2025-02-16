using System.Collections;
using FMODUnity;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
[SerializeField, Tooltip("If empty, drag the game object associated with the player into this field")] private GameObject playerGameObject;
private NoRbPlayerController playerController;
private StudioEventEmitter playerFootstepsEventEmitter;
private float currentSpeedMultiplier;
[SerializeField, Tooltip("Used for visual purposes only. Please do not edit this in the inspector")] private float footstepsDelayTime = 0.333f;
private bool coroutineReset = true;

void Update()
{
    IsPlayerMovingAndGrounded();
    if (IsPlayerMovingAndGrounded() && coroutineReset)
    {
        coroutineReset = false;
        //determine a current speed multiplier from the player controller script to be used to alter footstep speed
        currentSpeedMultiplier = playerController.currentSpeed / playerController.baseSpeed;
        //get the reciprocal of the current speed multiplier so that as the multiplier increases the delay time gets smaller, distribute this between the player's 2 feet, then scale down by an arbitrary amount 
        footstepsDelayTime = 1 / currentSpeedMultiplier / 2 / 1.5f;
        playerFootstepsEventEmitter.Play();
        StartCoroutine(FootstepsDelay());
    }
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