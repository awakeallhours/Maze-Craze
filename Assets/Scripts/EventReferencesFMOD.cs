using Debug = UnityEngine.Debug;
using FMODUnity;
using UnityEngine;

public class EventReferencesFMOD : MonoBehaviour
{
    //allow the EventReferencesFMOD class to be accessed publically, but privately set
    public static EventReferencesFMOD eventReferencesFMODInstance {get; private set;}

    //add FMOD event references below - publically accessible, privately set
    [field: SerializeField] public EventReference playerFootsteps {get; private set;}
    [field: SerializeField] public EventReference playerTorchToggle {get; private set;}
    [field: SerializeField] public EventReference playerJump {get; private set;}
    [field: SerializeField] public EventReference playerLand {get; private set;}


    //ensure that only 1 EventReferencesFMOD instance is created
    void Awake()
    {
        if (eventReferencesFMODInstance != null)
        {
            Debug.LogError("More than one Event Reference FMOD instance found!");
        }
        else
        {
            eventReferencesFMODInstance = this;
        }
    }
}
