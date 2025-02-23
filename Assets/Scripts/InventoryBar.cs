using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InventoryBar : MonoBehaviour
{
    /*The 3 inventory type scripts depend on eachother InventoryBar, Pickups and PlayerInventory if something changes in 1 check the others*/

    [SerializeField, Tooltip("How long will the inventory ribbon be visible for")] float visibilityTime = 3f;
    [SerializeField, Tooltip("Panel game object to be attached")] GameObject panel;
    [SerializeField, Tooltip("Inventory slot image UI elements")] private List<Image> inventorySlotImages = new List<Image>();
    [SerializeField, Tooltip("Text input field for collected keys")] private TextMeshProUGUI keysText;
    [SerializeField, Tooltip("Text input field for collected keys")] private TextMeshProUGUI compassText;
    [SerializeField, Tooltip("Placeholder sprites for empty slots")] private Sprite emptySlotSprite;

    private Compass compass;
    private PlayerInventory inv;
    public bool showPanel = false;
    public float currentTimer;
   
    void Start()
    {
        panel.SetActive(false);
        currentTimer = visibilityTime;
        inv = FindFirstObjectByType<PlayerInventory>();
        compass = FindFirstObjectByType<Compass>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Showinventory();
        }

        if (showPanel)
        {
            Countdown();
        }

        if (currentTimer <= 0)
        {
            HideInventory();
        }

        compassText.text = compass.compassHeadingNumber.ToString();

    }

    public void UpdateInventoryUI(List<Sprite> itemSprites, List<string> keys)
    {
        //clear all slots first
        foreach(Image slotImage in inventorySlotImages)
        {
            slotImage.sprite = emptySlotSprite;
        }

        //populate slots with images
        for(int i = 0; i < itemSprites.Count && i < inventorySlotImages.Count; i++)
        {
            inventorySlotImages[i].sprite = itemSprites[i];
        }

        //update the keys text field
        {
            keysText.text = string.Join("\n", keys);
        }
    }



    void Showinventory()
    {
        showPanel = true;
        panel.SetActive(true);
    }

    void HideInventory()
    {
        showPanel = false;
        panel.SetActive(false);
        currentTimer = visibilityTime;
    }

    void Countdown()
    {
        currentTimer -= Time.deltaTime;
    }
}
