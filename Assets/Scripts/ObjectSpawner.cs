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
        Destroy(itemInstance);
        Debug.Log("pickup destroyed");
        itemInstance = null;
        
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
        if (other.CompareTag("Player"))
        {
            Pickup();
            Debug.Log("player inside pickup zone");
        }
    }

    void RespawnTimer()
    {
        currentTimer -= Time.deltaTime;
        
        
    }
}
