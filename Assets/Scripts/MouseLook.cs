using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    [SerializeField, Tooltip("Mouse sensitivity")] float mouseSensitivity = 100f;

    public Transform playerPrefab;
    private NoRbPlayerController controller;

    float xRotation = 0f;  // Ensure this starts at 0 to look straight ahead
    private bool isSceneLoaded = false;  // Flag to check if the scene is loaded

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = playerPrefab.GetComponent<NoRbPlayerController>();

        // Set initial camera rotation to look straight ahead
        xRotation = 0f; // Reset the xRotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Simulate a delay for the scene to load (you can replace this with your actual loading logic)
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

        if (controller.isCrouching)
        {
            transform.position = new Vector3(transform.position.x, controller.crouchHeight, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, controller.transform.position.y, transform.position.z);
        }
    }

    // Coroutine to simulate a delay for the scene to load
    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(1f);  // Adjust the delay duration as needed
        isSceneLoaded = true;  // Set the flag to true once the delay is over
    }
}
