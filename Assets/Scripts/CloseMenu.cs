using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseMenu : MonoBehaviour
{
    public void ExitMenu(){
        Debug.Log("Exit menu");
        this.gameObject.SetActive(false);       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerLogic player = FindObjectOfType<PlayerLogic>();
        if (player == null){
            WheelchairController wheelchair = FindObjectOfType<WheelchairController>();
            if (wheelchair != null){
                wheelchair.interactuandoCanvas = false; Time.timeScale = 1f; // Reanudar el juego
            } 
            return;
        }
        player.interactuandoCanvas = false;
        Time.timeScale = 1f; // Reanudar el juego

    }

    public void OpenMenu(){
        Debug.Log("Open Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.gameObject.SetActive(true);   
        Time.timeScale = 0f; // Pausar el juego 
    }

    public void ExitGame(){
        Time.timeScale = 1f; // Reanudar el juego
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void GoToMainMenu(){
        Debug.Log("Go to Main Menu");
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene(0);
    }
}
