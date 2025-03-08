using UnityEngine;

public class BasicHorse : MonoBehaviour
{
    public float speed = 7.5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null) // By default, the horse walks
        {
            animator.SetBool("walking", true);
        }
    }

    void Update()
    {
        // The horse moves straight ahead
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

