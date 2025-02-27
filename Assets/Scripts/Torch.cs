using Debug = UnityEngine.Debug;
using FMOD.Studio;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField, Tooltip("Torch intensity default value is 3")] float torchIntensity;
    [SerializeField, Tooltip("Torch works in conjunction with flickerspeed, higher values wont work unless flicker speed is adjusted as well")] float flickerTimer = 0.0f;
    [SerializeField, Tooltip("Torch how often the flicker updates, increase to slow down effect. It's too obvious higher than about 0.3 ")] float flickerSpeed = 0.1f;
    [SerializeField, Tooltip("Torch intensity divisor, increase for subtler effect, 165 is a decent number")] float divisor = 100f;
    private PlayerAttributes attributes;
    Light torch;
    public bool isOn = false;
    
    //FMOD audio
    private float isOnFMODParameter;
    private EventInstance playerTorchToggleEventInstance;


    void Start()
    {
        torch = GetComponent<Light>();
        torch.enabled = false;
        torch.intensity = torchIntensity;

        attributes = GetComponentInParent<PlayerAttributes>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || !attributes.torchAllowed)
        {
            ToggleTorch();
        }

        if(isOn)
        {
            TorchFlicker();
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
            TorchAudio();
        }
    }


    void TorchFlicker()
    {
        if (torch == null)
        {
            Debug.LogWarning("Torch Light component is not assigned.");
            return;
        }

        // Update the flicker timer
        flickerTimer += Time.deltaTime;

        // Only update the flicker effect at the specified flicker speed
        if (flickerTimer >= flickerSpeed)
        {
            // Scale torch intensity with battery level
            float scaledIntensity = torchIntensity * (attributes.currentBattery / divisor);

            // Calculate a very small flicker range for a subtle effect
            float flickerRange = Mathf.Lerp(0.9999f, 1.0001f, attributes.currentBattery / divisor);
            float flickerAmount = Random.Range(scaledIntensity * (1 - flickerRange), scaledIntensity * (1 + flickerRange));

            // Calculate dynamic minimum threshold based on battery level
            float minThreshold = Mathf.Lerp(0.1f, torchIntensity, attributes.currentBattery / 100f);
            torch.intensity = Mathf.Max(flickerAmount, minThreshold);
            Debug.Log($"Updated torch intensity: {torch.intensity} (Battery: {attributes.currentBattery})");

            // Reset the flicker timer
            flickerTimer = 0.0f;
        }
    }

    void TorchAudio()
    {
        isOnFMODParameter = isOn ? 1f : 0f;
        playerTorchToggleEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.playerTorchToggle);
        playerTorchToggleEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        playerTorchToggleEventInstance.setParameterByName("Player_Torch_Toggle.isOn", isOnFMODParameter);
        playerTorchToggleEventInstance.start();
        playerTorchToggleEventInstance.release();
    }
}
