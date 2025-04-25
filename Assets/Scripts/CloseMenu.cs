using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseMenu : MonoBehaviour
{
    public void ExitMenu(){
        this.gameObject.SetActive(false);       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.gameObject.SetActive(true);       
    }

    public void ExitGame(){
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }
}
