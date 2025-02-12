using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    [SerializeField, Tooltip("force exterted on box")] float pushForce = 5f;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //allows objects tagged as pushable to be pushed 
        if (other.attachedRigidbody !=null && other.CompareTag("Pushable"))
        {
            //pushes the object in the players fowrward direction
            Vector3 forceDirection = transform.forward;
            other.attachedRigidbody.AddForce(forceDirection * pushForce, ForceMode.Force);
            
        }
       
    }
}
