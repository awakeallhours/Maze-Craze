using FMOD;
using FMODUnity;
using UnityEngine;

public class Torch : MonoBehaviour
{
    Light torch;
    StudioEventEmitter playerTorchToggleEventEmitter;
    float isOnFMODParameter = 0f;
    public bool isOn = false;

    float debugFMODParameter;

    void Start()
    {
        torch = GetComponent<Light>();  
        torch.enabled = false;

        //FMOD
        playerTorchToggleEventEmitter = gameObject.GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleTorch();
        }
    }

    void ToggleTorch()
    {
        isOn = !isOn;
        torch.enabled = isOn;

        isOnFMODParameter = isOn ? 1f : 0f;

        UnityEngine.Debug.Log("isOn: - " + isOn); 
        UnityEngine.Debug.Log("pre FMOD - " + isOnFMODParameter);

        playerTorchToggleEventEmitter.EventInstance.setParameterByName("isOn", isOnFMODParameter, false);
        playerTorchToggleEventEmitter.Play();
        
        playerTorchToggleEventEmitter.EventInstance.getParameterByName("isOn", out debugFMODParameter);
        UnityEngine.Debug.Log("post FMOD - " + debugFMODParameter);
    }
}
