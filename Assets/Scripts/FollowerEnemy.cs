using UnityEngine;

public class FollowerEnemy : BasciEnemy
{
    public float speed = 7.5f;
    public float detectionRange = 10f; 
    private Transform playerTransform;

    protected override void Move()
    {
        if (playerTransform != null)
        {
            // Calcular la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Si el jugador está dentro del rango de detección, perseguirlo
            if (distanceToPlayer <= detectionRange)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                transform.position += directionToPlayer * speed * Time.deltaTime;

                // Rotar hacia el jugador
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }
}
