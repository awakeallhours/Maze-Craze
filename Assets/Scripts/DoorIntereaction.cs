using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorIntereaction : MonoBehaviour
{
    [SerializeField,Tooltip("Angle door should open to")] float openingAngle = 90;
    [SerializeField,Tooltip("Door opening speed")] float openingSpeed = 1;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;

    bool isOpen;
    public bool canOpen;

    private void Start()
    {
        //sets door _closedRotation to initial spawn rotation 
        _closedRotation = transform.rotation;

        //sets door _openingRotation to new rotation
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openingAngle, 0));
        canOpen = false; 
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
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
                

            _currentCoroutine = StartCoroutine(ToggleDoor());

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<NoRbPlayerController>())
        {
            Debug.Log("Player entered");
            canOpen = true;
        }
       
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NoRbPlayerController>())
        {
            Debug.Log("Player exited");
            canOpen = false;
        }
            
    }
}
