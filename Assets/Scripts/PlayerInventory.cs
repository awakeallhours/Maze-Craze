using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, Tooltip("")] List<string> inventory;
    
    
    void Start()
    {
        //redDoorKey added for testing purposes
        inventory.Add("redDoorKey");
    }

    public void AddItem(string item)
    {
        inventory.Add(item);
    }


    public bool HasItem(string itemID)
    {
        return inventory.Contains(itemID);
    }
}
