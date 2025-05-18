using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class WheelchairController : MonoBehaviour
{
    public float moveSpeed = 0.43f;

    [SerializeField] private float turnSpeed = 2.5f;
    [SerializeField] private float slopeResistance = 25;
    [SerializeField] private float stickToGroundForce = 10;

    public bool interactuandoCanvas;

    public AudioClip wallCollisionSound;
    private CharacterController cc;

    private Vector3 velocity;

    private bool previouslyGrounded;
    private Vector3 initialPosition;
    private AudioSource audioSource;
    public int numRampas = 0;

    public GameObject hammer;
    public bool hasHammer = false;
    [SerializeField] public CloseMenu interactionCanvas;

    [SerializeField] private TMP_Text rampasText;

    // --- CONTROLES PERSONALIZADOS ---
    [SerializeField] private ControlsManager controlsManager;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        initialPosition = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found.");
        }

        if (hammer != null)
        {
            hammer.SetActive(false);
        }

        // Cargar ControlsManager si no está asignado
        if (controlsManager == null)
        {
            controlsManager = FindObjectOfType<ControlsManager>();
            if (controlsManager == null)
            {
                Debug.LogError("ControlsManager not found in the scene.");
            }
        }
    }

    void Update()
    {
        if (cc.isGrounded)
        {
            velocity.y = -stickToGroundForce;
            previouslyGrounded = true;
        }
        else
        {
            if (previouslyGrounded)
            {
                velocity.y = 0;
            }
            previouslyGrounded = false;
        }

        // --- CONTROLES PERSONALIZADOS PARA MOVIMIENTO Y GIRO ---
        float vertical = 0f;
        float horizontal = 0f;

        if (controlsManager != null)
        {
            if (Input.GetKey(controlsManager.GetKey("Adelante"))) vertical += 1f;
            if (Input.GetKey(controlsManager.GetKey("Atrás"))) vertical -= 1f;
            if (Input.GetKey(controlsManager.GetKey("GirarDerecha"))) horizontal += 1f;
            if (Input.GetKey(controlsManager.GetKey("GirarIzquierda"))) horizontal -= 1f;
        }
        else
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        transform.Rotate(0, horizontal * turnSpeed, 0);
        Vector3 desiredMove = transform.forward * vertical * moveSpeed;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, cc.radius, Vector3.down, out hitInfo, cc.height / 2f);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);

        float hitAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
        float slopeEffect = Mathf.Clamp01((hitAngle - slopeResistance) / cc.slopeLimit);
        Vector3 slideForce = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z) * slopeEffect;
        velocity += Physics.gravity * Time.deltaTime;

        cc.Move((velocity + desiredMove + slideForce) * Time.deltaTime);

        if (hasHammer && hammer != null)
        {
            hammer.SetActive(true);
        }
        else if (hammer != null)
        {
            hammer.SetActive(false);
        }

        if (rampasText != null) rampasText.text = numRampas.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (interactionCanvas != null && !interactuandoCanvas)
            {
                OpenInteractionCanvas();
            }
            else if (interactionCanvas != null && interactuandoCanvas)
            {
                CloseInteractionCanvas();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            previouslyGrounded = true;
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy! Resetting position.");
            transform.position = initialPosition;
        }

        if (hit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall collision");
            if (audioSource != null && wallCollisionSound != null)
            {
                Debug.Log("Playing sound");
                audioSource.PlayOneShot(wallCollisionSound);
            }
        }

        if (hit.gameObject.CompareTag("Hammer"))
        {
            hasHammer = true;
            Debug.Log("Hammer picked up");
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.CompareTag("Rampa"))
        {
            numRampas++;
            Destroy(hit.gameObject);
            Debug.Log("Rampa picked up. Total rampas: " + numRampas);
        }
        if (hit.gameObject.CompareTag("FueraMartillo"))
        {
            hasHammer = false;
            Destroy(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LevelLoader"))
        {
            other.gameObject.GetComponent<ChangeScene>().ChangeToScene();
        }

        if (other.gameObject.CompareTag("LevelCompletion"))
        {
            LevelCompletion levelCompletion = other.gameObject.GetComponent<LevelCompletion>();
            if (levelCompletion != null)
            {
                levelCompletion.MarkLevelAsCompleted();
            }
        }

        if (other.gameObject.CompareTag("RampaGenerator"))
        {
            RampaGenerator rampaGenerator = other.gameObject.GetComponent<RampaGenerator>();
            if (numRampas > 0)
            {
                numRampas--;
                Debug.Log("Rampa used. Remaining rampas: " + numRampas);
                if (rampaGenerator == null)
                {
                    Debug.LogWarning("RampaGenerator component not found.");
                    return;
                }
                rampaGenerator.GenerateRampa();
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("No rampas left to use.");
                if (rampaGenerator.IsDown())
                {
                    cc.enabled = false;
                    transform.position = initialPosition;
                    cc.enabled = true;
                }
            }
        }
    }

    public void OpenInteractionCanvas()
    {
        if (interactionCanvas != null)
        {
            interactuandoCanvas = true;
            interactionCanvas.OpenMenu();
        }
        else
        {
            Debug.LogWarning("Interaction canvas not assigned.");
        }
    }

    public void CloseInteractionCanvas()
    {
        if (interactionCanvas != null)
        {
            interactuandoCanvas = false;
            interactionCanvas.ExitMenu();
        }
        else
        {
            Debug.LogWarning("Interaction canvas not assigned.");
        }
    }
}