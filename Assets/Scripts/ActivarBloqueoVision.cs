using UnityEngine;

public class ActivarBloqueoVision : MonoBehaviour
{
    public GameObject bloqueoVision; // arrastra el objeto que bloquea la visión aquí

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bloqueoVision.SetActive(true);
        }
    }
}
