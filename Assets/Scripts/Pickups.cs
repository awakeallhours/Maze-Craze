using UnityEngine;
using System;

public class Pickups : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of health/battery to restore")] float restore = 10f;
    [SerializeField, Tooltip("Speed of item rotation")] float rotationSpeed = 10f;
    [SerializeField, Tooltip("")] public bool validPickup;

    private PlayerAttributes attributes;
    void Start()
    {
        attributes = FindFirstObjectByType<PlayerAttributes>();
        validPickup = false;
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
            Debug.Log("Key collected");
            //need to make key logic
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
            CheckItem();
        }
    }



}
