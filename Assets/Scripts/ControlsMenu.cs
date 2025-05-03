using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private ControlsManager controlsManager; // Referencia al ControlsManager
    [SerializeField] private GameObject controlRowPrefab; // Prefab para cada fila
    [SerializeField] private Transform controlsContainer; // Contenedor para las filas

    private Dictionary<string, TMP_InputField> inputFields = new Dictionary<string, TMP_InputField>();

    void Start()
    {
        if (controlsManager == null)
        {
            Debug.LogError("ControlsManager reference not assigned.");
            return;
        }

        if (controlsManager == null)
        {
            Debug.LogWarning("ControlsManager reference is null. Attempting to find it in the scene.");
            controlsManager = FindObjectOfType<ControlsManager>();
            if (controlsManager == null)
            {
                Debug.LogError("ControlsManager not found in the scene.");
                return;
            } else {
                Debug.Log("ControlsManager reference found in the scene.");
            }
        } else {
            Debug.Log("ControlsManager reference assigned successfully.");
        }

        // Crear una fila para cada acción en los controles
        foreach (var action in controlsManager.GetAllKeyMappings())
        {
            CreateControlRow(action.Key, action.Value);
        }
    }

    private void CreateControlRow(string actionName, KeyCode currentKey)
    {
        // Instanciar el prefab de la fila
        GameObject row = Instantiate(controlRowPrefab, controlsContainer);
    
        // Configurar el texto con el nombre de la acción
        TMP_Text actionText = row.transform.Find("ActionText").GetComponent<TMP_Text>();
        actionText.text = actionName;
    
        // Configurar el texto con la tecla actual
        TMP_Text currentKeyText = row.transform.Find("CurrentKeyText").GetComponent<TMP_Text>();
        currentKeyText.text = currentKey.ToString();
    
        // Configurar el campo de entrada
        TMP_InputField inputField = row.transform.Find("KeyInput").GetComponent<TMP_InputField>();
        inputField.text = currentKey.ToString();
        inputFields[actionName] = inputField;
    
        // Añadir un evento para capturar la tecla presionada
        inputField.onSelect.RemoveAllListeners(); // Limpia cualquier evento anterior
        inputField.onSelect.AddListener((_) => StartKeyCapture(actionName, inputField, currentKeyText));
    }
    
    private void StartKeyCapture(string actionName, TMP_InputField inputField, TMP_Text currentKeyText)
    {
        StartCoroutine(CaptureKey(actionName, inputField, currentKeyText));
    }
    
    private IEnumerator CaptureKey(string actionName, TMP_InputField inputField, TMP_Text currentKeyText)
    {
        Debug.Log($"Press a key to assign to action '{actionName}'...");
    
        while (true)
        {
            // Esperar a que el usuario presione una tecla
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
    
            // Detectar la tecla presionada
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Ignorar entradas del ratón
                    if (keyCode.ToString().StartsWith("Mouse"))
                    {
                        Debug.LogWarning($"Mouse input '{keyCode}' ignored. Waiting for a valid key...");
                        yield return null; // Continuar esperando
                        break;
                    }
    
                    Debug.Log($"Key '{keyCode}' captured for action '{actionName}'.");
    
                    // Actualizar el campo de entrada y el texto de la tecla actual
                    inputField.text = keyCode.ToString();
                    currentKeyText.text = keyCode.ToString();
    
                    // Guardar la nueva tecla en el ControlsManager
                    controlsManager.SetKey(actionName, keyCode);
    
                    yield break; // Salir de la corutina después de capturar una tecla válida
                }
            }
        }
    }
}
