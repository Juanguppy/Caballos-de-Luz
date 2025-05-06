using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;

    private bool hasTeleported = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTeleported && other.CompareTag("Player"))
        {
            Debug.Log("Teleporting player");
            other.transform.position = teleportDestination.position;
            hasTeleported = true;

            // Puedes desactivar el objeto o solo el collider, según lo que prefieras:
            // gameObject.SetActive(false); // Desactiva todo el GameObject
            GetComponent<Collider>().enabled = false; // Solo desactiva el trigger
        }
    }
}
