using Debug = UnityEngine.Debug;
using FMOD.Studio;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField, Tooltip("Torch intensity default value is 3")] float torchIntensity; 
    
    private PlayerAttributes attributes;
    Light torch;
    
    public bool isOn = false;
    
    //FMOD audio
    private float isOnFMODParameter;
    private EventInstance playerTorchToggleEventInstance;
    private GameObject torchGameObject;
    private bool allowNonRigidbodyVelocity = true; //FMOD will determine velocity data without the need for a rigidbody

    void Start()
    {
        torch = GetComponent<Light>();
        torch.enabled = false;
        torch.intensity = torchIntensity;

        attributes = GetComponentInParent<PlayerAttributes>();

        //FMOD audio
        torchGameObject = this.gameObject;
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
            //TorchAudio();
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
    }

    void TorchAudio()
    {
        isOnFMODParameter = isOn ? 1f : 0f;
        playerTorchToggleEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.playerTorchToggle);
        AudioManager.audioManagerInstance.AttachInstanceToGameObject(playerTorchToggleEventInstance, torchGameObject, allowNonRigidbodyVelocity);
        playerTorchToggleEventInstance.setParameterByName("isOn", isOnFMODParameter);
        playerTorchToggleEventInstance.start();
        playerTorchToggleEventInstance.release();
    }
}
