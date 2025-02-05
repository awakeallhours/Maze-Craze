using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField, Tooltip("Mouse sensitivity")] float mouseSensitivity = 100f;
    public Transform playerPrefab;

    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerPrefab.Rotate(Vector3.up * mouseX);
    }
}
