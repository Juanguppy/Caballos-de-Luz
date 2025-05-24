using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;  // Asigna el AudioMixer en el Inspector
    public Slider volumeSlider;    // Asigna el Slider en el Inspector

    void Start()
    {
        // Obtener volumen guardado (opcional)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void OnVolumeSliderChanged()
    {
        // Llamar a SetVolume cuando el slider cambie
        SetVolume(volumeSlider.value);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Setting volume to: " + volume);
        // Convertir a dB (AudioMixer usa escala logarítmica)
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("MusicVolume", dB);

        // Guardar la preferencia (opcional)
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
