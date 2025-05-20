using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoPlayerObject; // El objeto que contiene el VideoPlayer (o el canvas/UI si es el caso)
    public Button skipButton; // Botón para saltar
    public string videoName = "empatia.mp4"; // Nombre del video a reproducir
    public bool changeScene = false; 
    public int sceneIndex = 0;

    private bool videoFinished = false;

    void Start()
    {
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None;

        Debug.Log(videoName);
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;

        videoPlayer.prepareCompleted += (vp) => vp.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();

        skipButton.onClick.AddListener(SkipVideo);
    }

    void Update()
    {
        if (!videoFinished && Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (!videoFinished)
        {
            videoFinished = true;
            EndVideo();
        }
    }

    void SkipVideo()
    {
        if (!videoFinished)
        {
            videoFinished = true;
            videoPlayer.Stop();
            EndVideo();
        }
    }

    void EndVideo()
    {
        if(changeScene)
        {
            SceneManager.LoadScene(sceneIndex);
            return;
        }
        videoPlayerObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
