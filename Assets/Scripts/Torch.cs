using UnityEngine;

public class Torch : MonoBehaviour
{
    Light torch;

    public bool isOn = false;
    void Start()
    {
        torch = GetComponent<Light>();  
        torch.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleTorch();
        }
    }

    void ToggleTorch()
    {
        
        isOn = !isOn;
        torch.enabled = isOn;
    }
}
