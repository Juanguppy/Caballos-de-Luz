using UnityEngine;

public class FollowerEnemy : BasciEnemy
{

    private Vector3 initialPosition; 
    private float returnThreshold = 0.5f;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        animator.SetBool("idle", true);

        initialPosition = transform.position;
         rb = GetComponent<Rigidbody>();
    }

    protected override void Move()
    {
        if (playerTransform == null) return;

        Vector3 targetDirection = Vector3.zero;
        bool isMoving = false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            targetDirection = (playerTransform.position - transform.position).normalized;
            isMoving = true;
        }
        else if (Vector3.Distance(transform.position, initialPosition) > returnThreshold)
        {
            targetDirection = (initialPosition - transform.position).normalized;
            isMoving = true;
        }

        if (isMoving)
        {
            // Movimiento físico que respeta colisiones
            Vector3 newPosition = rb.position + targetDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            // Rotación suave
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.deltaTime * 5f);
            rb.MoveRotation(newRotation);


            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("idle", true);
        }
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.MovePosition(initialPosition);

        }
    }
    // para dibujar el rango de detección en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    
    private void FixedUpdate()
    {
        Move();
    }

}
