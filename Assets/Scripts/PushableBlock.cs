using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    [SerializeField, Tooltip("force exterted on box")] float pushForce = 5f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody !=null && other.CompareTag("Pushable"))
        {
            Vector3 forceDirection = transform.forward;
            other.attachedRigidbody.AddForce(forceDirection * pushForce, ForceMode.Force);
            Debug.Log("Pushing");
        }
       
    }
}
