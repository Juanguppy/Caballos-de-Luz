using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquizoPlayerLogic : PlayerLogic
{
    public AudioClip squizoSound;
    public Canvas squizoCanvas;
    public AudioClip alucinacionSound;
    public float alucinacionTime = 35f;

    public float alucinacionTimer = 0f;
    private bool isSquizoCanvasActive = false;

    private bool estaBajoEfectos = false;
    public float efectosTimer = 0f; 
    public float efectosTime = 45f; // tiempo que dura el efecto de las pastillas

    [SerializeField] public Canvas efectosCanvas;
    [SerializeField] public TMPro.TextMeshProUGUI efectosText;

    private List<GameObject> fakeEnemies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    

    protected override void Start()
    {
        base.Start();
        squizoCanvas.gameObject.SetActive(false);
        fakeEnemies.AddRange(GameObject.FindGameObjectsWithTag("FakeEnemy"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        if (efectosCanvas != null) efectosCanvas.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        alucinacionTimer += Time.deltaTime;

        if (alucinacionTimer >= alucinacionTime && !isSquizoCanvasActive)
        {
            if(estaBajoEfectos) return;
            StartCoroutine(ActivateCanvas());
            alucinacionTimer = 0f;
            audioSource.PlayOneShot(alucinacionSound);
        }

        if (estaBajoEfectos)
        {
            efectosTimer += Time.deltaTime;
            float tiempoRestante = efectosTime - efectosTimer;
            if (efectosText != null) efectosText.text = "Efecto de pastillas: " + Mathf.CeilToInt(tiempoRestante).ToString() + "s";
            if (efectosTimer >= efectosTime)
            {
                PasarEfectoPastillas();
                efectosTimer = 0f;
            }
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

        if(collision.gameObject.CompareTag("DrugSealer")){
            this.TomarPastillas();
            Debug.Log("Tomar pastillas");
        }
    }

    protected void TomarPastillas(){
        estaBajoEfectos = true;
        if (efectosCanvas != null) efectosCanvas.gameObject.SetActive(true);
        foreach (GameObject enemy in fakeEnemies){
            if (enemy != null) enemy.SetActive(false);
        }
        foreach (GameObject enemy in enemies){
            if (enemy != null) enemy.SetActive(false);
        }
        return;
    }

    protected void PasarEfectoPastillas(){
        estaBajoEfectos = false;
        if (efectosCanvas != null) efectosCanvas.gameObject.SetActive(false);
        foreach (GameObject enemy in fakeEnemies){
            if (enemy != null) enemy.SetActive(true);
        }
        foreach (GameObject enemy in enemies){
            if (enemy != null) enemy.SetActive(true);
        }
        return;
    }
}
