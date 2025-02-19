using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of health to remove")] float damage = 10f;
    
    
    private PlayerAttributes attributes;
    
    void Start()
    {
        attributes = FindFirstObjectByType<PlayerAttributes>();
        
    }

    void Update()
    {
        
    }

    void DamagePlayer()
    {
        attributes.DecreaseHealth(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("damage are entered");
            DamagePlayer();
        }
    }

}
