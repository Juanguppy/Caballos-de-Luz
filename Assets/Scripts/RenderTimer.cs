using UnityEngine;

public class RenderTimer : MonoBehaviour
{
    public float intervalHidden = 5.0f;
    public float intervalShown = 0.5f;

    private bool isHidden = true;
    private float timer = 0.0f;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.enabled = false;
        }
        isHidden = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isHidden && timer >= intervalHidden)
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = true;
            }
            isHidden = false;
            timer = 0.0f;
        }
        else if (!isHidden && timer >= intervalShown)
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = false;
            }
            isHidden = true;
            timer = 0.0f;
        }
    }
}
