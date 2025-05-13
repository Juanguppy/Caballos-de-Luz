using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionalStart : MonoBehaviour
{
    public int firstTimeSceneIndex = 1;
    public int otherTimeSceneIndex = 2;

    public void OnButtonPressed()
    {
        if (PlayerPrefs.GetInt("HasPressedButton", 0) == 0)
        {
            PlayerPrefs.SetInt("HasPressedButton", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(firstTimeSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(otherTimeSceneIndex);
        }
    }
}