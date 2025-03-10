using UnityEngine;

public class BasciEnemy : MonoBehaviour
{
    public float speed = 7.5f;
    public float detectionRange = 10f; // Rango de detección del jugador
    public float soundInterval = 5f; // Intervalo de tiempo entre sonidos
    public AudioClip alertSound; // Sonido que se reproducirá
    private Animator animator;
    private AudioSource audioSource;
    private Transform playerTransform;
    private float soundTimer;
    private bool isPlayingSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator != null) // Por defecto el enemigo anda
        {
            animator.SetBool("walking", true);
        }

        soundTimer = 0;
        isPlayingSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckPlayerDistance();
    }

    void Move()
    {
        // El enemigo se mueve hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void CheckPlayerDistance()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRange)
            {
                soundTimer -= Time.deltaTime;

                if (soundTimer <= 0 && !isPlayingSound)
                {
                    PlayAlertSound();
                    soundTimer = soundInterval;
                }
            }
        }
    }

    void PlayAlertSound()
    {
        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
            isPlayingSound = true;
            Invoke("ResetSoundFlag", alertSound.length);
        }
    }

    void ResetSoundFlag()
    {
        isPlayingSound = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con algo que tenga tag == Wall, el enemigo gira 180 grados
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.Rotate(0, 180, 0);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Si choca con el jugador, el jugador vuelve a la posición inicial
            Debug.Log("PlayerManager: OnCollisionEnter: Enemy --- Player");
        }
    }
}