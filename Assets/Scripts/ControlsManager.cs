using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keyMappings;
    [SerializeField] private string keysetPrefix = "Player_"; // Cambia a "Wheelchair_" para el otro set

    void Start()
    {
        keyMappings = new Dictionary<string, KeyCode>
        {
            { "Salto", KeyCode.Space },
            { "Sprint", KeyCode.LeftShift },
            { "Adelante", KeyCode.W },
            { "Atrás", KeyCode.S },
            { "Izquierda", KeyCode.A },
            { "Derecha", KeyCode.D }
        };
        if(keysetPrefix == "Wheelchair_")
        {
            keyMappings = new Dictionary<string, KeyCode>
            {
                { "Adelante", KeyCode.W },
                { "Atrás", KeyCode.S },
                { "GirarIzquierda", KeyCode.A },
                { "GirarDerecha", KeyCode.D }
            };
        }

        LoadKeys();
    }

    public void SetKey(string action, KeyCode newKey)
    {
        if (keyMappings.ContainsKey(action))
        {
            keyMappings[action] = newKey;
            PlayerPrefs.SetString(keysetPrefix + action, newKey.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning($"Action '{action}' not found in key mappings.");
        }
    }

    public KeyCode GetKey(string action)
    {
        if (keyMappings.ContainsKey(action))
        {
            return keyMappings[action];
        }

        Debug.LogWarning($"Action '{action}' not found in key mappings.");
        return KeyCode.None;
    }

    public void LoadKeys()
    {
        var keys = new List<string>(keyMappings.Keys);

        foreach (var action in keys)
        {
            if (PlayerPrefs.HasKey(keysetPrefix + action))
            {
                Debug.Log($"Loading key for action '{action}' from PlayerPrefs.");
                keyMappings[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keysetPrefix + action));
            }
        }
    }

    public Dictionary<string, KeyCode> GetAllKeyMappings()
    {
        if (keyMappings == null || keyMappings.Count == 0)
        {
            Debug.LogWarning("Key mappings are empty or null.");
            return new Dictionary<string, KeyCode>();
        }
        return keyMappings;
    }
}