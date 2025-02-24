using UnityEngine;
using System.Collections;
using TMPro;

public class MouseLook : MonoBehaviour
{
    //todo: player height seems to have been affected by the intialLocalPosition
    //todo: fix crouch

    [SerializeField, Tooltip("Mouse sensitivity")] float mouseSensitivity = 100f;
    [SerializeField, Tooltip("Head bob amount")] float bobAmount = 0f;
    [SerializeField, Tooltip("Head bob speed")] float bobSpeed = 0f;

    public Transform playerPrefab;
    private NoRbPlayerController controller;

    private Vector3 initialLocalPosition;
    float xRotation = 0f;  // Ensure this starts at 0 to look straight ahead
    private bool isSceneLoaded = false;  // Flag to check if the scene is loaded

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = playerPrefab.GetComponent<NoRbPlayerController>();

        initialLocalPosition = transform.localPosition;

        // Set initial camera rotation to look straight ahead
        xRotation = 0f; // Reset the xRotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Simulate a delay for the scene to load
        StartCoroutine(SceneLoadDelay());
    }

    void Update()
    {
        // Only process mouse input if the scene is loaded
        if (!isSceneLoaded)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;  // Update xRotation based on mouseY input
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Clamp xRotation to prevent over-rotation

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerPrefab.Rotate(Vector3.up * mouseX);


        Crouch();
        Headbob();
    }

    // Coroutine to simulate a delay for the scene to load
    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(1f);  // Adjust the delay duration as needed
        isSceneLoaded = true;  // Set the flag to true once the delay is over
    }

    void Crouch()
    {
        Vector3 targetPosition;
        if (controller.isCrouching)
        {
            targetPosition = new Vector3(initialLocalPosition.x, controller.crouchHeight, initialLocalPosition.z);
        }
        else
        {
            targetPosition = new Vector3(initialLocalPosition.x, controller.transform.position.y, initialLocalPosition.z);
        }

        transform.position = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
    }

    void Headbob()
    {
        //trying to adjust bobspeed for crouch

        float bobFactor;
        float currentBobSpeed = bobSpeed;
        if (controller.isMoving )
        {
            bobFactor = Mathf.Sin(Time.time *currentBobSpeed) * bobAmount;
            Vector3 bobOffset = new Vector3(0f, bobFactor, 0f);
            transform.localPosition = initialLocalPosition + bobOffset;
        }
        else if (controller.isMoving && controller.isCrouching)
        {
           
            bobFactor = Mathf.Sin(Time.time * currentBobSpeed * 0.5f) * bobAmount;
            Vector3 bobOffset = new Vector3(0f, bobFactor, 0f);
            transform.localPosition = initialLocalPosition + bobOffset;
        }
        else
        {
            // Reset to player's y position if not moving
            transform.localPosition = initialLocalPosition;
        }


    }
}
