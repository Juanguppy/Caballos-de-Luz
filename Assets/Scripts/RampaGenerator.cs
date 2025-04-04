using UnityEngine;

public class RampaGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rampaObject;

    public bool isDown = false;

    public void Start(){
        rampaObject.SetActive(false);
    }

    public void GenerateRampa()
    {
        if (rampaObject != null)
        {
            rampaObject.SetActive(true);
            Debug.Log("Rampa activada.");
        }
        else
        {
            Debug.LogWarning("RampaObject no está asignado en el RampaGenerator.");
        }
    }

    public bool IsDown()
    {
        return this.isDown;
    }
}