using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Objeto a instanciar
    [SerializeField] private float spawnInterval = 2f; // Intervalo de tiempo entre spawns
    [SerializeField] private Transform spawnPoint; // Punto donde se instanciarán los objetos

    private float timer; // Temporizador para el spawn normal
    private float pauseTimer; // Temporizador para controlar los 30 segundos de actividad
    private bool isPaused = false; // Indica si el spawner está en pausa

    public bool isSquizo = false; 

    void Start()
    {
        timer = spawnInterval; // Inicializar el temporizador de spawn
        pauseTimer = 30f; // Cada 30 segundos se activa la pausa
    }

    void Update()
    {
        // Controlar el temporizador de pausa
        if (isPaused && !isSquizo)
        {
            pauseTimer -= Time.deltaTime;

            // Si han pasado 15 segundos de pausa, salir de la pausa
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                pauseTimer = 30f; // Reiniciar el temporizador para la próxima pausa
            }

            return; // No spawnear nada mientras está en pausa
        }

        // Reducir el temporizador de actividad
        pauseTimer -= Time.deltaTime;

        // Si han pasado 30 segundos de actividad, activar la pausa
        if (pauseTimer <= 15f && !isPaused)
        {
            isPaused = true;
            return; // No spawnear nada en este frame
        }

        // Controlar el temporizador de spawn
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnObject();
            timer = spawnInterval; // Reiniciar el temporizador de spawn
        }
        if (isSquizo){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player == null) return;
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn.");
        }
    }
}