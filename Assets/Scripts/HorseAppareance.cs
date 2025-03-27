using UnityEngine;

public class HorseAppareance : MonoBehaviour
{
    public int requiredLevelIndex; // Nivel que debe completarse
    public bool enable = true;

    void OnEnable() // Se ejecuta cada vez que el objeto se activa
    {
        if (PlayerPrefs.GetInt("Level_" + requiredLevelIndex, 0) == 1)
        {
            gameObject.SetActive(enable);  // Aparece si el nivel está completado
        }
        else
        {
            gameObject.SetActive(!enable); // Se esconde si no está desbloqueado
        }
    }
}

