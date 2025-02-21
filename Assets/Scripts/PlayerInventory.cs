using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    /*[SerializeField, Tooltip("List of the items in the players inventory")] List<string> inventory = new List<string>();*/
    [SerializeField, Tooltip("List of the items sprites in the players inventory")] private List<Sprite> itemSprites = new List<Sprite>();
    [SerializeField, Tooltip("List of keys in the players inventory")] List<string> keys = new List<string>();
    [SerializeField, Tooltip("Inventory bar script reference")]
    private InventoryBar inventoryBar;
    
    void Start()
    {
        inventoryBar = FindFirstObjectByType<InventoryBar>();

        //redDoorKey added for testing purposes
        keys.Add("redDoorKey");
        inventoryBar.UpdateInventoryUI(itemSprites, keys);
    }

    private void Update()
    {
        
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

    
}
