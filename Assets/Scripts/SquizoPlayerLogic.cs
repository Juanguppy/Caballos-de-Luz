using UnityEngine;
using System.Collections;

public class SquizoPlayerLogic : PlayerLogic
{
    public AudioClip squizoSound;
    public Canvas squizoCanvas;
    public AudioClip alucinacionSound;
    public float alucinacionTime = 35f;

    public float alucinacionTimer = 0f;
    private bool isSquizoCanvasActive = false;

    protected override void Start()
    {
        base.Start();
        squizoCanvas.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        alucinacionTimer += Time.deltaTime;

        if (alucinacionTimer >= alucinacionTime && !isSquizoCanvasActive)
        {
            StartCoroutine(ActivateCanvas());
            alucinacionTimer = 0f;
            audioSource.PlayOneShot(alucinacionSound);
        }
    }

    private IEnumerator ActivateCanvas()
    {
        Debug.Log("Activating Canvas");
        isSquizoCanvasActive = true;
        squizoCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        squizoCanvas.gameObject.SetActive(false);
        isSquizoCanvasActive = false;
        Debug.Log("Deactivating Canvas");
    }

    protected override void OnCollisionEnter(Collision collision){
        base.OnCollisionEnter(collision);

        // se puede chocar con un muro que no ve, y lo hace visible
        if(collision.gameObject.CompareTag("SquizoWall")){
            Debug.Log("SquizoWall collision");
            audioSource.PlayOneShot(wallCollisionSound); audioSource.PlayOneShot(squizoSound);
            MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            if(meshRenderer == null) return; 
            meshRenderer.enabled = true;
        }

        // un muro falso 
        if(
           collision.gameObject.CompareTag("FakeWall")  ||
           collision.gameObject.CompareTag("FakeHorse") || 
           collision.gameObject.CompareTag("FakeEnemy")
           )
        {
            // transform.position = initialPosition;
            if(audioSource != null) audioSource.PlayOneShot(squizoSound);
            Destroy(collision.gameObject);
        }
    }
}
