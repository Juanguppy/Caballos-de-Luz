using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BasicHorse : MonoBehaviour
{
    public float speed = 7.5f;
    private Animator animator;
    public bool isIdle = false;
    private float changeDirectionInterval = 10f; // cada 10 segundos el caballo cambia de dirección
    private float changeDirectionTimer;
    public string[] subtitles; // Subtítulos para el caballo
    public float[] subtitleTimings; // Tiempos para cada línea de subtítulos
    public string caballoId; // Por ejemplo: "EstrellaClara"


    [SerializeField] private GameObject interactionMenu; // Menú de interacción (Canvas)
    [SerializeField] private TextMeshProUGUI subtitlesText; // Campo para mostrar subtítulos
    [SerializeField] private AudioSource audioSource; // Fuente de audio
    [SerializeField] private AudioClip horseAudioClip; // Clip de audio del caballo
    [SerializeField] private TextMeshProUGUI horseNameText; // Texto para el nombre del caballo
    [SerializeField] private TextMeshProUGUI horseDescriptionText; // Texto para la descripción del caballo
    private PlayerLogic playerLogic;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null) // By default, the horse walks
        {
            animator.SetBool("walking", true);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerLogic = player.GetComponent<PlayerLogic>();
            
            if (interactionMenu != null) interactionMenu.SetActive(false); 
            CargarDialogoDesdeCSV();

        }
        if (isIdle) // If the horse is idle, it stops walking
        {
            animator.SetBool("Idle", true);
        }


        changeDirectionTimer = changeDirectionInterval;

    }

    void Update()
    {
        // The horse moves straight ahead but only if is not idle
        if (!isIdle)
            Move();
    }

    void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // cambiamos la dirección una vez expira el timer
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0){

            float randomYRotation = Random.Range(0f, 360f);
            transform.Rotate(0, randomYRotation, 0);

            changeDirectionTimer = changeDirectionInterval;
        }
    }

    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("Wall"))
        {
            //The horse turns around when hitting a wall
            transform.Rotate(0, 180, 0);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // The horse interacts with the player (could knock them back, etc.)
            Debug.Log("Neigh! The horse bumped into the player!");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if(isIdle) return; // el caballo del nivel no puede interactuar con el jugador
            // El caballo interactúa con el jugador
            Debug.Log("Neigh! The horse bumped into the player!");
            StopAndFacePlayer();
        }
    }

    void StopAndFacePlayer()
    {
        // Detener el movimiento del caballo
        isIdle = true;
        if (animator != null)
        {
            animator.SetBool("walking", false);
            animator.SetBool("Idle", true);
        }

        // Mostrar el menú de interacción
        if (interactionMenu != null)
        {
            interactionMenu.SetActive(true);
            playerLogic.InteractuarCaballo();
        }
    }

    public void CloseInteractionMenu()
    {
        // Cerrar el menú de interacción y permitir que el caballo se mueva nuevamente
        if (interactionMenu != null)
        {
            interactionMenu.SetActive(false);
            playerLogic.DeInteractuarCaballo();
        }

        isIdle = false;
        if (animator != null)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("walking", true);
        }
    }

    public void ObtenerInformacion(){
        if (audioSource != null && horseAudioClip != null)
        {
            StartCoroutine(PlayAudioWithSubtitles());
        }
    }

    private IEnumerator PlayAudioWithSubtitles()
    {
        // Subtítulos sincronizados con el audio
        if (subtitles == null || subtitles.Length == 0)
        {
            Debug.LogWarning("No subtitles provided for the horse audio.");
            yield break;
        }
        if (subtitlesText == null)
        {
            Debug.LogWarning("Subtitles text field is not assigned.");
            yield break;
        }

        // Reproducir el audio
        audioSource.clip = horseAudioClip;
        audioSource.Play();

        // Mostrar subtítulos sincronizados
        for (int i = 0; i < subtitles.Length; i++)
        {
            Debug.Log("Subtítulo: " + subtitles[i] + " (Tiempo: " + subtitleTimings[i] + ")");
            subtitlesText.text = subtitles[i];
            yield return new WaitForSeconds(subtitleTimings[i]);
        }

        // Limpiar subtítulos al finalizar
        yield return new WaitForSeconds(horseAudioClip.length - subtitleTimings[subtitleTimings.Length - 1]);
        subtitlesText.text = "";
    }

    void CargarDialogoDesdeCSV()
    {
        TextAsset archivo = Resources.Load<TextAsset>("DialogosCaballos" + caballoId); // sin extensión .csv

        if (archivo == null)
        {
            Debug.LogError("No se encontró el archivo CSV en Resources/DialogosCaballos" + caballoId + ".csv");
            return;
        }

        List<string> textos = new List<string>();
        List<float> tiempos = new List<float>();

        string[] lineas = archivo.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string linea in lineas)
        {
            // Esperado: CaballoId,Texto,Tiempo
            string[] partes = linea.Split(',');

            if (partes.Length < 3) continue;

            string id = partes[0].Trim();
            if (id != caballoId) continue;

            string texto = partes[1].Trim().Trim('"');
            if (float.TryParse(partes[2], out float tiempo))
            {
                textos.Add(texto);
                tiempos.Add(tiempo);
            }
        }

        subtitles = textos.ToArray();
        subtitleTimings = tiempos.ToArray();

        Debug.Log($"🎤 Subtítulos cargados para {caballoId}: {subtitles.Length} líneas.");
    }
}

