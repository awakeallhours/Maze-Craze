using FMODUnity;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField, Tooltip("Torch intesity default value is 3")] float torchIntensity; 
    
    private PlayerAttributes attributes;
    Light torch;
    
    public bool isOn = false;
    
    //David additions
    StudioEventEmitter playerTorchToggleEventEmitter;
    
    float isOnFMODParameter = 0f;
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

        TorchFlicker();
       
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

        /*if (isOn)
        {
            TorchFlicker();
            Debug.Log("Torch on");
        }
        else if (!isOn)
        {
            Debug.Log("Torch off");
        }*/
    }

    void TorchFlicker()
    {
        float flickerAmount = 0;

        
        //torch.enabled = true;

        if (attributes.currentBattery >= 40)
        {
            flickerAmount = Random.Range(-10f, 10f) * Time.deltaTime;
        
        }
        else if (attributes.currentBattery < 40)
        {
            flickerAmount = Random.Range(torchIntensity / 2, 1f) * Time.deltaTime;
        }
        
        torch.intensity = torchIntensity + flickerAmount;


        /*
        isOnFMODParameter = isOn ? 1f : 0f;

        Debug.Log("isOn: - " + isOn); 
        Debug.Log("pre FMOD - " + isOnFMODParameter);

        playerTorchToggleEventEmitter.EventInstance.setParameterByName("isOn", isOnFMODParameter, false);
        playerTorchToggleEventEmitter.Play();
        
        playerTorchToggleEventEmitter.EventInstance.getParameterByName("isOn", out debugFMODParameter);
        Debug.Log("post FMOD - " + debugFMODParameter);
        */
    }
}
