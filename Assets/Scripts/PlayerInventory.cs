using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    /*The 3 inventory type scripts depend on eachother InventoryBar, Pickups and PlayerInventory if something changes in 1 check the others*/


    [SerializeField, Tooltip("List of the items sprites in the players inventory")] private List<Sprite> itemSprites = new List<Sprite>();
    [SerializeField, Tooltip("List of keys in the players inventory")] List<string> keys = new List<string>();
    
    private InventoryBar inventoryBar;
    private Pickups pickup;
    void Start()
    {
        inventoryBar = FindFirstObjectByType<InventoryBar>();
        pickup = FindFirstObjectByType<Pickups>();
        
        //redDoorKey added for testing purposes
        keys.Add("redDoorKey");
        inventoryBar.UpdateInventoryUI(itemSprites, keys);
    }

    private void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            UseItem(0); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            UseItem(1); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        { 
            UseItem(2); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) 
        { 
            UseItem(3); 
        }
    }

    public void AddItem(Sprite itemSprite)
    {
        if (!itemSprites.Contains(itemSprite))
        {
            itemSprites.Add(itemSprite);
            inventoryBar.UpdateInventoryUI(itemSprites, keys);
        }
    }

    public void AddKey(string key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            inventoryBar.UpdateInventoryUI(itemSprites, keys);
        }

    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }

    public void RemoveKey(string keyID)
    {
        if (keys.Contains(keyID))
        {
            keys.Remove(keyID);
            inventoryBar.UpdateInventoryUI(itemSprites, keys);
        }
        
    }

    public void RemoveItem(Sprite itemSprite)
    {
        itemSprites.Remove(itemSprite);
        inventoryBar.UpdateInventoryUI(itemSprites, keys);
    }

    public void UseItem(int slot)
    {
        if (slot >= 0 && slot < itemSprites.Count)
        {
            Debug.Log("Using item in slot " + (slot + 1));

            // Item behaviours

            // Get the sprite in the selected slot
            Sprite itemSprite = itemSprites[slot];

            // Get the name of the sprite
            string itemName = itemSprite.name; 

            if (itemName == "Battery_placeholder_0")
            {
                pickup.ChargeBattery();
                RemoveItem(itemSprite); 
                inventoryBar.UpdateInventoryUI(itemSprites, keys);
            }
            else if (itemName == "Medkit_placeholder_0")
            {
                pickup.Heal();
                RemoveItem(itemSprite); 
                inventoryBar.UpdateInventoryUI(itemSprites, keys);
            }
        }
        else
        {
            Debug.Log("No item in this slot.");
        }
    }




}
