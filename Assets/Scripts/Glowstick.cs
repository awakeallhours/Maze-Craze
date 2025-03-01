using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Glowstick : MonoBehaviour
{
    [SerializeField, Tooltip("Throwable glowstick prefab")] GameObject throwable = null;
    [SerializeField, Tooltip("Glowstick colour")] private Color glowColour;
    [SerializeField, Tooltip("Glowstick light colour")] private Color lightColour;
    [SerializeField, Tooltip("Glowstick light object")] private Light glowLight;
    [SerializeField, Tooltip("Glowstick light intensity")] private float glowIntensity;
    [SerializeField, Tooltip("Illumination duration")] private float illuminationDuration = 10f;
    [SerializeField, Tooltip("Emission intensity multiplier")] private float emissionIntensity = 1.5f;

    //Allows us to change the material
    private Renderer glowRenderer;
    private Rigidbody rb;
    private NoRbPlayerController controller;

    public float timer;
    public bool isGlowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        glowRenderer = GetComponent<Renderer>();
        controller = FindFirstObjectByType<NoRbPlayerController>();
        glowLight.enabled = false;
        SetGlowState(false);
        rb.useGravity = false;
    }

    void Update()
    {
        
    }

    void SetGlowState(bool state)
    {
        glowLight.enabled = state;
        glowLight.color = lightColour;
        glowLight.intensity = state ? glowIntensity : 0;
        glowRenderer.material.color = glowColour;
        

        if (state)
        {
            glowRenderer.material.EnableKeyword("_EMISSION") ;
            glowRenderer.material.SetColor("_EmissionColor", glowColour * Mathf.LinearToGammaSpace(emissionIntensity));

        }
        else
        {
            glowRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    private IEnumerator UseGlowstick()
    {
        
        timer = illuminationDuration;
        SetGlowState(true);
        isGlowing = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        SetGlowState(false);
        isGlowing = false;
    }

    public void ThrowGlowstick()
    {
        //This version does not correctly work but it is the safe version before i change the script too much

        //Instantiate(throwable, controller.transform.position, Quaternion.identity);
        //StartCoroutine(UseGlowstick());
        //rb.useGravity = true;
        Debug.Log("GLOWSTICK SPAWNED");
    }
}
