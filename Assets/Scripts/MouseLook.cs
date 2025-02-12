using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField, Tooltip("Mouse sensitivity")] float mouseSensitivity = 100f;
   
    public Transform playerPrefab;
    private NoRbPlayerController controller;
    
    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = playerPrefab.GetComponent<NoRbPlayerController>();
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

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
}
