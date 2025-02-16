using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using TMPro;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField, Tooltip("Player max health")] public float maxHealth = 100;
    [SerializeField, Tooltip("Player max Stamina")] public float maxStamina = 100;
    [SerializeField, Tooltip("Player max battery")] public float maxBattery = 100;
    [SerializeField, Tooltip("Player current health")] public float currentHealth;
    [SerializeField, Tooltip("Player current stamina")] public float currentStamina;
    [SerializeField, Tooltip("Player max battery")] public float currentBattery;
    [SerializeField, Tooltip("Stamina drain rate")] float staminaDrain = 1;
    [SerializeField, Tooltip("Battery drain rate")] float batteryDrain = 1;
    [SerializeField, Tooltip("Stamina recovery delay")] float staminaDelay = 4f;

    [SerializeField, Tooltip("Health value")] TextMeshProUGUI healthValue;
    [SerializeField, Tooltip("Stamina value")] TextMeshProUGUI staminaValue;
    [SerializeField, Tooltip("Battery value")] TextMeshProUGUI batteryValue;
    


    NoRbPlayerController controller;
    Torch torch;

    private float currentDelay;
    public bool noStamina;
    public bool torchAllowed;
    

    
    void Start()
    {
        controller = GetComponent<NoRbPlayerController>();
        torch = GetComponentInChildren<Torch>();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentBattery = maxBattery;

        currentDelay = staminaDelay;
    }

   
    void Update()
    {
        Stamina();
        Torch();
    }

    void Stamina()
    {
        
        staminaValue.text = Mathf.RoundToInt(currentStamina).ToString();
        
        if(currentStamina > 0)
        {
            noStamina = false;
        }
       

        if (controller.isSprinting && currentStamina > 0)
        {
            currentStamina -= staminaDrain * Time.deltaTime;


        }
        else if (currentStamina < maxStamina && controller.isSprinting == false && !noStamina)
        {
            currentStamina += staminaDrain * Time.deltaTime;
        }

        if (currentStamina <= 0)
        {
            StaminaDelay();
        }
    }

    void Torch()
    {
        batteryValue.text = Mathf.RoundToInt(currentBattery).ToString();

        if (currentBattery > 0)
        {
            torchAllowed = true;
        }
        else
        {
            torchAllowed = false;
        }

        if (torch.isOn && currentBattery > 0)
        {
            currentBattery -= batteryDrain * Time.deltaTime;


        }
        //Add function to recover battery
        /*else if()
        {
            currentStamina += staminaDrain * Time.deltaTime;

        }*/
    }

    void StaminaDelay()
    {
        if (currentStamina <= 0)
        {
            currentDelay -= Time.deltaTime;
            noStamina = true;
        }

        if(currentDelay <= 0)
        {
            currentDelay = staminaDelay;
            noStamina = false;
        }
    }
    

}
