using System;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField, Tooltip("Used for visual purposes only. Please do not edit this in the inspector")] public float compassHeadingNumber;
    [SerializeField, Tooltip("If empty, drag the game object associated with the player into this field")] private GameObject playerGameObject;
    [SerializeField, Tooltip("Needle of the compass")] private GameObject needle;
    void Start()
    {
        if (playerGameObject == null)
        {
            Debug.LogError(GetType().Name + ".cs - No player gameObject assigned in the inspector!");
        }
        if (needle == null)
        {
            Debug.LogError(GetType().Name + ".cs - No needle gameObject assigned in the inspector!");
        }
    }
    void Update()
    {
        UpdateCompassHeading();
    }
    void UpdateCompassHeading()
    {
        if (playerGameObject != null)
        {
            compassHeadingNumber = (float)Math.Round(playerGameObject.transform.rotation.eulerAngles.y % 360f);
            UpdateNeedleRotation();
        }
    }
    void UpdateNeedleRotation()
    {
        if (needle != null)
        {
            // Ensure the needle rotation only affects the z-axis
            needle.transform.localRotation = Quaternion.Euler(0, 0, compassHeadingNumber);
        }
    }
}