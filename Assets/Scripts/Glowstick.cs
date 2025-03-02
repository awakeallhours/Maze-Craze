using UnityEngine;
using System.Collections;

public class Glowstick : MonoBehaviour
{
    [SerializeField, Tooltip("Glowstick base colour")] private Color glowColour;
    [SerializeField, Tooltip("Glowstick colour of light emitted")] private Color lightColour;
    [SerializeField, Tooltip("Glowstick light intensity")] private float glowIntensity;
    [SerializeField, Tooltip("Illumination duration")] private float illuminationDuration = 10f;
    [SerializeField, Tooltip("Emission intensity multiplier")] private float emissionIntensity = 1.5f;
    [SerializeField, Tooltip("Force of throw")] private float throwForce;
    [SerializeField, Tooltip("Reference to the light component")] private Light glowLight; // Public light component
    [SerializeField, Tooltip("Reference to the renderer component")] private Renderer glowRenderer; // Allows us to change the material

    private float forceMultiplier = 100;
    private Rigidbody rb;
    private float timer;
    public bool isGlowing = false;

    void Start()
    {
        Debug.Log("Glowstick Start method is running.");

        rb = GetComponent<Rigidbody>();

        glowLight.enabled = true;
        glowLight.intensity = glowIntensity; // Set the initial intensity
        glowLight.color = lightColour; // Set the initial color
    }

    void Update()
    {

    }

    void SetGlowState(bool state)
    {
        glowLight.enabled = state;
        glowRenderer.material.color = glowColour;
        
        if (state)
        {
            glowRenderer.material.EnableKeyword("_EMISSION");
            glowRenderer.material.SetColor("_EmissionColor", glowColour * Mathf.LinearToGammaSpace(emissionIntensity));
            Debug.Log("Glowstick emission enabled.");
        }
        else
        {
            glowRenderer.material.DisableKeyword("_EMISSION");
            glowRenderer.material.SetColor("_EmissionColor", glowColour / Mathf.LinearToGammaSpace(emissionIntensity));
            Debug.Log("Glowstick emission disabled.");
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
        StartCoroutine(UseGlowstick());
        if (rb != null)
        {
            rb.useGravity = true;
            // Calculating the force vector
            Vector3 forceVector = transform.forward * throwForce * forceMultiplier;
            Debug.Log("Applying force: " + forceVector);

            // Apply force
            rb.AddForce(forceVector, ForceMode.Impulse);
        }
    }
}
