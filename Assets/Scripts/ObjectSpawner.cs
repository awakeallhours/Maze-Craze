using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Object prefab to be spawned")] GameObject itemPrefab;
    [SerializeField, Tooltip("Time until item respawns if at all")] float respawnTimer = 3f;
    [SerializeField, Tooltip("Will this item respawn")] bool respawnable;
    

    private GameObject itemInstance;

   
    public float currentTimer;

    

    private void Start()
    {
        if (itemInstance == null)
        {
            itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }

        currentTimer = respawnTimer;
    }

    private void Update()
    {
        if(itemInstance == null && respawnable)
        {
            RespawnTimer();
            
            if(currentTimer <= 0)
            {
                SpawnObject();
            }
        }
        
    }

    void Pickup()
    {
        //Gets pickups component from item, ?. is short hand for a null check and avoids null reference error
        Pickups pickup = itemInstance?.GetComponent<Pickups>();
        if (pickup != null && pickup.validPickup)
        {
            itemInstance = null;
        }

    }

    void SpawnObject()
    {
        if (itemInstance == null && respawnable)
        {
            itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            currentTimer = respawnTimer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& itemInstance != null)
        {
            Pickup();
        }
    }

    void RespawnTimer()
    {
        currentTimer -= Time.deltaTime;
    }
}
