using UnityEngine;

public class HorseAppareance : MonoBehaviour
{
    public int requiredLevelIndex; // Nivel que debe completarse

    void OnEnable() // Se ejecuta cada vez que el objeto se activa
    {
        if (PlayerPrefs.GetInt("Level_" + requiredLevelIndex, 0) == 1)
        {
            gameObject.SetActive(true);  // Aparece si el nivel está completado
        }
        else
        {
            gameObject.SetActive(false); // Se esconde si no está desbloqueado
        }
    }
}

