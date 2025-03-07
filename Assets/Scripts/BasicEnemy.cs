using UnityEngine;

public class BasciEnemy : MonoBehaviour
{
    public float speed = 7.5f;
    private Animator animator; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null) // por defecto el pibe anda
        {
            animator.SetBool("walking", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // el enemigo tira recto y ya
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {   // si choca con algo que tenga tag == Wall, el tio gira 180 grados 
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.Rotate(0, 180, 0);
        }

        if( collision.gameObject.CompareTag("Player"))
        {
            // si choca con el player, el player vuelve a la posición inicial
            Debug.Log("PlayerManager: OnCollisionEnter: Enemy --- Player");
        }
    }
}