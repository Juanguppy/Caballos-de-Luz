using UnityEngine;

public class HelpWall : MonoBehaviour
{
    public AudioClip helpWallCollisionSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found.");
        }
    }

    public void PlaySound(){
        if (audioSource != null && helpWallCollisionSound != null)
        {
            audioSource.PlayOneShot(helpWallCollisionSound);
        }
    }
}
