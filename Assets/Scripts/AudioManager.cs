using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //allow the AudioManager class to be accessed publically, but privately set
    public static AudioManager audioManagerInstance {get; private set;}
    private List<EventInstance> eventInstances;

    //ensure that only 1 AudioManager instance is created
    void Awake()
    {
        if (audioManagerInstance != null)
        {
            Debug.LogError("More than one Audio Manager instance found!");
        }
        else
        {
            audioManagerInstance = this;

            //create a list for event instances
            eventInstances = new List<EventInstance>();
        }
    }


    //this method is used to create an event instance within the
    //FMOD RuntimeManager using a passed event reference
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        //add the event instance to the event instance list
        eventInstances.Add(eventInstance);
        return eventInstance;
    }


    public void AttachInstanceToGameObject(EventInstance eventInstance, GameObject gameObject, bool allowNonRigidbodyVelocity)
    {
        RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, allowNonRigidbodyVelocity);
    }


    //create a method for stopping all current event instances and
    //release them from memory
    void StopAndReleaseAllEventInstances()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }


    //stop and release all event instances if this game object is destroyed
    void OnDestroy()
    {
        StopAndReleaseAllEventInstances();        
    }


    //establish an easy-to-use oneshot audio event method
    //3D positional data is updated every frame
    //parameters cannot be set
    //once event is complete it is automatically released from memory
    public void PlayOneShotAttached(EventReference eventFMOD, Vector3 worldPos)
    {
        audioManagerInstance.PlayOneShotAttached(eventFMOD, worldPos);
    }
}
