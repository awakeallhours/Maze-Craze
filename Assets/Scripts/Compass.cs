using System;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField, Tooltip("Used for visual purposes only. Please do not edit this in the inspector")] public float compassHeadingNumber;
    [SerializeField, Tooltip("If empty, drag the game object associated with the player into this field")] private GameObject playerGameObject;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerGameObject == null)
        {
            Debug.LogError(GetType().Name + ".cs - No player gameObject assigned in the inspector!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        CompassHeading();
    }


    float CompassHeading()
    {
        compassHeadingNumber = (float)Math.Round(playerGameObject.transform.rotation.eulerAngles.y);

        if (compassHeadingNumber == 360f)
        {
            compassHeadingNumber = 0f;
            return compassHeadingNumber;
        }
        else
        {
            return compassHeadingNumber;
        }    
    }
}
