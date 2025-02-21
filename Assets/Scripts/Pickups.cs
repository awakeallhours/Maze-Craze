using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;

public class Pickups : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of health/battery to restore")] float restore = 10f;
    [SerializeField, Tooltip("Speed of item rotation")] float rotationSpeed = 10f;
    [SerializeField, Tooltip("Sprite of the item")] Sprite itemSprite;
    [SerializeField, Tooltip("Specify if the item is a key")] public bool isKey;
    //[SerializeField, Tooltip("Item ID if neccessary, currently used for keys")] public string itemID;

    private PlayerAttributes attributes;
    private PlayerInventory inv;
    private string itemID;
    void Start()
    {
        attributes = FindFirstObjectByType<PlayerAttributes>();
        inv = FindAnyObjectByType<PlayerInventory>();
        itemID = gameObject.name;
    }

   
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void CheckItem()
    {
        if(gameObject.tag == "Medpack")
        {
            AddToInventory();
            //Heal();
        }

        if (gameObject.tag == "Battery")
        {
            AddToInventory();
            //ChargeBattery();
        }

        if (gameObject.tag == "Key")
        {
            AddToInventory();
            //AddKey();
        }
    }

    void Heal()
    {
        if(attributes.currentHealth < attributes.maxHealth)
        {
            
            attributes.IncreaseHealth(restore);
            Destroy(gameObject);
        }
       
    }

    void ChargeBattery()
    {
        if (attributes.currentBattery < attributes.maxBattery)
        {
           
            attributes.IncreaseBattery(restore);
            Destroy(gameObject);
        }
        
    }

    void AddKey()
    {
        // if the key name is not null and the player does not already have it, add it to the player inventory 
        if (itemID != null && !inv.HasKey(itemID))
        {
            inv.AddKey(itemID);
            Destroy(gameObject);
        }
    }

    void AddToInventory()
    {
        inv.AddItem(itemSprite);
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            CheckItem();
        }
    }



}
