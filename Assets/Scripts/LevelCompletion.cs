using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    public int levelIndex; // Índice del nivel que se está completando

    public void MarkLevelAsCompleted()
    {
        if (PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 0) // Solo guarda si no estaba completado
        {
            PlayerPrefs.SetInt("Level_" + levelIndex, 1);
            PlayerPrefs.Save();
            Debug.Log("Nivel " + levelIndex + " completado.");
        }
        else
        {
            Debug.Log("Nivel " + levelIndex + " ya estaba completado.");
        }
    }
}
