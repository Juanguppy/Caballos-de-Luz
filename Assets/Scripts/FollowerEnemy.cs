using UnityEngine;

public class FollowerEnemy : BasciEnemy
{

    private Vector3 initialPosition; 
    private float returnThreshold = 0.5f;

    protected override void Start()
    {
        base.Start();
        animator.SetBool("idle", true);

        initialPosition = transform.position;
    }

    protected override void Move()
    {
        if (playerTransform != null)
        {
            // Calcular la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Si el jugador está dentro del rango de detección, perseguirlo
            if (distanceToPlayer <= detectionRange) {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                transform.position += directionToPlayer * speed * Time.deltaTime;
                animator.SetBool("idle", false);

                // Rotar hacia el jugador
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            } else if (Vector3.Distance(transform.position, initialPosition) > returnThreshold) {
                Vector3 directionToInitial = (initialPosition - transform.position).normalized;
                transform.position += directionToInitial * speed * Time.deltaTime;
                animator.SetBool("idle", false);

                Quaternion lookRotation = Quaternion.LookRotation(directionToInitial);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            } else {
                animator.SetBool("idle", true);
            }
        }
    }

    // para dibujar el rango de detección en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
