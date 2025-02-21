using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Object prefab to be spawned")] GameObject itemPrefab;
    [SerializeField, Tooltip("Time until item respawns if at all")] float respawnTimer = 3f;
    [SerializeField, Tooltip("Will this item respawn")] bool respawnable;
    [SerializeField, Tooltip("Item ID to be passed to item, currently used for setting a keys name. Syntax is DoorIDKey eg(redDoorKey)")] string itemID;

    private GameObject itemInstance;

   
    public float currentTimer;

    private void Start()
    {
        SpawnObject();
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
        if (pickup != null)
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

            //checks if the item is a key and assigns the key a name
            if (itemPrefab.name == "Key") 
            {
                itemInstance.name = itemID;
            }
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
