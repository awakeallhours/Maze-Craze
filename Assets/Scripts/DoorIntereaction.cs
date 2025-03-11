using Debug = UnityEngine.Debug;
using System.Collections;
using FMOD.Studio;
using UnityEngine;


public class DoorIntereaction : MonoBehaviour
{
    //DoorKeys are related to Player Inventory

    [SerializeField, Tooltip("Angle door should open to")] float openingAngle = 90;
    [SerializeField, Tooltip("Door opening speed")] float openingSpeed = 1;
    [SerializeField, Tooltip("Unique Door identifier for key system")] string doorID = "redDoor";
    [SerializeField, Tooltip("Is the door locked")] public bool isLocked;
    [SerializeField, Tooltip("Can the door ever be opened")] public bool inoperable = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;

    bool isOpen;
    public bool canOpen;
    private PlayerInventory inv;

    //FMOD
    private EventInstance environmentDoorUnlockEventInstance;
    

    private void Start()
    {
        //sets door _closedRotation to initial spawn rotation 
        _closedRotation = transform.rotation;

        //sets door _openingRotation to new rotation
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openingAngle, 0));
        canOpen = false;

        inv = FindFirstObjectByType<PlayerInventory>();
    }

    private void Update()
    {
        OperateDoor();
    }

    IEnumerator ToggleDoor()
    {
        //checks state of door and toggles between states
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
            isOpen = !isOpen;

        //operates door smoothly
        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openingSpeed);
                yield return null;
        }

        transform.rotation = targetRotation;
    }

    void OperateDoor()
    {
        if (canOpen && !isLocked && Input.GetKeyDown(KeyCode.E))
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
            _currentCoroutine = StartCoroutine(ToggleDoor());
        }

        //Checks the players inventory for a door key if the door is locked and a key is needed
        else if(isLocked && Input.GetKeyDown(KeyCode.E)&& canOpen)
        {
            if (inv.HasKey(doorID+"Key"))
            {
                UnlockDoor();
                Debug.Log(doorID + " Door Unlocked");
                inv.RemoveKey(doorID + "Key");
            }
            else if(!inv.HasKey(doorID+"Key"))
            {
                Debug.Log("This door is locked you need " + doorID + "key.");
            }
        }
    }

    public void LockDoor()
    {
        isLocked = true;
    }

    public void UnlockDoor()
    {
        isLocked = false;
        UnlockDoorAudio();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NoRbPlayerController>())
        {
            
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NoRbPlayerController>())
        {
            
            canOpen = false;
        }
    }

    void UnlockDoorAudio()
    {
        environmentDoorUnlockEventInstance = AudioManager.audioManagerInstance.CreateEventInstance(EventReferencesFMOD.eventReferencesFMODInstance.environmentDoorUnlock);
        environmentDoorUnlockEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        environmentDoorUnlockEventInstance.start();
        environmentDoorUnlockEventInstance.release();
    }
}
