using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keyMappings;

    void Start()
    {
        // Inicializar el diccionario con las teclas predeterminadas
        keyMappings = new Dictionary<string, KeyCode>
        {
            { "Salto", KeyCode.Space },
            { "Sprint", KeyCode.LeftShift },
            { "Adelante", KeyCode.W },
            { "Atrás", KeyCode.S },
            { "Izquierda", KeyCode.A },
            { "Derecha", KeyCode.D }
        };

        LoadKeys(); // Cargar teclas personalizadas si existen
    }

    public void SetKey(string action, KeyCode newKey)
    {
        if (keyMappings.ContainsKey(action))
        {
            keyMappings[action] = newKey;
            PlayerPrefs.SetString(action, newKey.ToString());
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
        // Crear una copia de las claves para evitar modificar el diccionario mientras se itera
        var keys = new List<string>(keyMappings.Keys);
    
        foreach (var action in keys)
        {
            if (PlayerPrefs.HasKey(action))
            {
                keyMappings[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(action));
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
