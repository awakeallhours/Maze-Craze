using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;

public class Pickups : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of health/battery to restore")] float restore = 10f;
    [SerializeField, Tooltip("Speed of item rotation")] float rotationSpeed = 10f;
    [SerializeField, Tooltip("")] public bool validPickup;
    //[SerializeField, Tooltip("Item ID if neccessary, currently used for keys")] public string itemID;

    private PlayerAttributes attributes;
    private PlayerInventory inv;
    private string item;
    void Start()
    {
        attributes = FindFirstObjectByType<PlayerAttributes>();
        inv = FindAnyObjectByType<PlayerInventory>();
        validPickup = false;
        item = gameObject.name;
    }

   
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void CheckItem()
    {
        if(gameObject.tag == "Medpack")
        {
            Heal();
        }

        if (gameObject.tag == "Battery")
        {
            ChargeBattery();
        }

        if (gameObject.tag == "Key")
        {
            Key();
        }
    }

    void Heal()
    {
        if(attributes.currentHealth < attributes.maxHealth)
        {
            validPickup = true;
            attributes.IncreaseHealth(restore);
            Destroy(gameObject);
        }
        else
        {
            validPickup = false;
        }
    }

    void ChargeBattery()
    {
        if (attributes.currentBattery < attributes.maxBattery)
        {
            validPickup = true;
            attributes.IncreaseBattery(restore);
            Destroy(gameObject);
        }
        else
        {
            validPickup = false;
        }
    }

    void Key()
    {
        validPickup = true;
        Destroy(gameObject);

        // if the key name is not null and the player does not already have it, add it to the player inventory 
        if (item != null && !inv.HasItem(item))
        {
            inv.AddItem(item);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            CheckItem();
        }
    }



}
