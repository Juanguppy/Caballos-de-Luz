using UnityEngine;

public class DoorLight : MonoBehaviour
{
    private Light doorLight;
    private Renderer cylinderRenderer;
    private bool isBlinking = false;

    void Start()
    {
        doorLight = GetComponent<Light>();
        cylinderRenderer = GetComponent<Renderer>();

        if (doorLight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }

        if (cylinderRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }
    }

    void Update()
    {
        if (isBlinking)
        {
            BlinkAmber();
        }
    }

    public void SetRed()
    {
        Debug.Log("SetRed");
        if (doorLight != null)
        {
            doorLight.color = Color.red;
            isBlinking = false;
        }

        if (cylinderRenderer != null)
        {
            cylinderRenderer.material.color = Color.red;
        }
    }

    public void SetGreen()
    {
        if (doorLight != null)
        {
            doorLight.color = Color.green;
            isBlinking = false;
        }

        if (cylinderRenderer != null)
        {
            cylinderRenderer.material.color = Color.green;
        }
    }

    public void StartBlinkingAmber()
    {
        if (doorLight != null)
        {
            isBlinking = true;
        }
    }

    public void StopBlinking()
    {
        if (doorLight != null)
        {
            isBlinking = false;
        }
    }

    private void BlinkAmber()
    {
        if (doorLight != null)
        {
            Color amberColor = Color.Lerp(Color.yellow, Color.black, Mathf.PingPong(Time.time, 1));
            doorLight.color = amberColor;

            if (cylinderRenderer != null)
            {
                cylinderRenderer.material.color = amberColor;
            }
        }
    }
}