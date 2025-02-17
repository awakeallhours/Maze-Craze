using FMODUnity;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField, Tooltip("Torch intesity default value is 3")] float torchIntensity; 
    private PlayerAttributes attributes;
    Light torch;
    StudioEventEmitter playerTorchToggleEventEmitter;
    float isOnFMODParameter = 0f;
    public bool isOn = false;

    float debugFMODParameter;

    void Start()
    {
        torch = GetComponent<Light>();
        torch.enabled = false;
        torch.intensity = torchIntensity;

        attributes = GetComponentInParent<PlayerAttributes>();

        //FMOD
        playerTorchToggleEventEmitter = gameObject.GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || !attributes.torchAllowed)
        {
            ToggleTorch();
        }
       
        if(!attributes.torchAllowed)
        {
            isOn = false;
            torch.enabled = false;
        }
    }

    void ToggleTorch()
    {
        if (attributes.torchAllowed)
        {
            isOn = !isOn;
            torch.enabled = isOn;
        }

        if(isOn)
        {
            TorchFlicker();
            Debug.Log("Flickering");
        }
    }

    void TorchFlicker()
    {
        float flickerAmount = 0;

        if (attributes.currentBattery >= 40)
        {
            flickerAmount = Random.Range(-10f, 10f) * Time.deltaTime;
        
        }
        else if (attributes.currentBattery < 40)
        {
            flickerAmount = Random.Range(torchIntensity / 2, 1f);
        }

        torch.intensity = torchIntensity + flickerAmount;
        isOn = !isOn;
        torch.enabled = isOn;

        isOnFMODParameter = isOn ? 1f : 0f;

        Debug.Log("isOn: - " + isOn); 
        Debug.Log("pre FMOD - " + isOnFMODParameter);

        playerTorchToggleEventEmitter.EventInstance.setParameterByName("isOn", isOnFMODParameter, false);
        playerTorchToggleEventEmitter.Play();
        
        playerTorchToggleEventEmitter.EventInstance.getParameterByName("isOn", out debugFMODParameter);
        Debug.Log("post FMOD - " + debugFMODParameter);
    }
}
