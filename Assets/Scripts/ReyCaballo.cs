using UnityEngine;

public class ReyCaballo : MonoBehaviour
{
    public AudioClip audioClip;
    public float moveUpDistance = 100f;
    public float moveUpSpeed = 2f;

    private AudioSource audioSource;
    private bool hasCollided = false;
    private bool isMovingUp = false;
    private Vector3 targetPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(MoveUpAfterAudio());
        }
    }

    private System.Collections.IEnumerator MoveUpAfterAudio()
    {
        // Espera a que termine el audio
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Calcula la posición objetivo
        targetPosition = transform.position + Vector3.up * moveUpDistance;
        isMovingUp = true;

        // Espera 3 segundos antes de destruir
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void Update()
    {
        if (isMovingUp)
        {
            // Movimiento suave hacia arriba
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveUpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingUp = false;
            }
        }
    }
}
