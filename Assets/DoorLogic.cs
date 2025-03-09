using UnityEngine;
using System.Collections;

public class DoorLogic : MonoBehaviour
{
    public float moveDistance = 50f; // Distancia que se moverá el objeto
    public float moveDuration = 2f; // Duración de la transición en segundos
    public float interval = 10f; // Intervalo de tiempo entre movimientos en segundos

    private Vector3 targetPosition;
    private float moveTimer;
    private bool isMoving;

    void Start()
    {
        targetPosition = transform.position;
        moveTimer = interval;
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0 && !isMoving)
        {
            moveTimer = interval;
            targetPosition = transform.position + Vector3.left * moveDistance;
            StartCoroutine(MoveToPosition(targetPosition, moveDuration));
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}