using UnityEngine;

public class BasicHorse : MonoBehaviour
{
    public float speed = 7.5f;
    private Animator animator;
    public bool isIdle = false;
    private float changeDirectionInterval = 10f; // cada 10 segundos el caballo cambia de dirección
    private float changeDirectionTimer;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null) // By default, the horse walks
        {
            animator.SetBool("walking", true);
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
    }
}

