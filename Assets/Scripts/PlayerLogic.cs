using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public Vector3 initialPosition;
    public AudioClip wallCollisionSound;
    [SerializeField] public CloseMenu interactionCanvas;

    protected Rigidbody rb;
    protected float xRotation = 0f;
    protected bool isGrounded;
    protected AudioSource audioSource;
    protected bool interactuando;
    public bool interactuandoCanvas;
    protected bool canInteract = true;
    protected float interactTimer = 0f;
    protected float interactDelay = 7f;

    /// Controles
    [SerializeField] private ControlsManager controlsManager;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evitar que el personaje se caiga al chocar con algo
        initialPosition = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found.");
        } 
        interactuando = false;
        if (controlsManager == null)
        {
            controlsManager = FindObjectOfType<ControlsManager>();
            if (controlsManager == null)
            {
                Debug.LogError("ControlsManager not found in the scene.");
            }
        }
    }

    protected virtual void Update()
    {
        if (interactuando) return;
        MovePlayer();
        RotateCamera();
        Jump();
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
        if (!canInteract){
            interactTimer += Time.deltaTime;
            if (interactTimer >= interactDelay)
            {
                canInteract = true;
                interactTimer = 0f;
            }
        }
    }

    protected virtual void MovePlayer()
    {
        float moveSpeedCurrent = Input.GetKey(controlsManager.GetKey("Sprint")) ? sprintSpeed : moveSpeed;

        float x = 0f;
        float z = 0f;

        if (Input.GetKey(controlsManager.GetKey("Adelante"))) z += 1f;
        if (Input.GetKey(controlsManager.GetKey("Atrás"))) z -= 1f;
        if (Input.GetKey(controlsManager.GetKey("Derecha"))) x += 1f;
        if (Input.GetKey(controlsManager.GetKey("Izquierda"))) x -= 1f;

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = new Vector3(move.x * moveSpeedCurrent, rb.linearVelocity.y, move.z * moveSpeedCurrent);

        rb.linearVelocity = velocity;
    }

    protected virtual void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    protected virtual void Jump()
    {
        if (Input.GetKeyDown(controlsManager.GetKey("Salto")) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy2"))
        {
            transform.position = initialPosition;
        }
        // hacemos sonido pa los ciegos 
        if (collision.gameObject.CompareTag("Wall"))
        {   
            Debug.Log("Wall collision");
            if (audioSource != null && wallCollisionSound != null)
            {
                Debug.Log("Playing sound");
                audioSource.PlayOneShot(wallCollisionSound);
            }
        }
        if (collision.gameObject.CompareTag("Final"))
        {
            SceneManager.LoadScene(19);
        }
    }

    protected virtual void OnColissionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LevelLoader"))
        {
            Debug.Log("LevelLoader");
            other.gameObject.GetComponent<ChangeScene>().ChangeToScene();
        }

        if (other.gameObject.CompareTag("HelpWall"))
        {   // TODO como hacer que solo pase si choca por delante del muro?
            HelpWall helpWall = other.gameObject.GetComponent<HelpWall>();
            if (helpWall != null)
            {
                helpWall.PlaySound();
            }
            else
            {
                Debug.LogWarning("HelpWall component not found on the collided object.");
            }
        }
        //  Marca el nivel como completado al tocar un objeto con el tag "LevelCompletion"
        if (other.gameObject.CompareTag("LevelCompletion"))
        {
            LevelCompletion levelCompletion = other.gameObject.GetComponent<LevelCompletion>();
            if (levelCompletion != null)
            {
                levelCompletion.MarkLevelAsCompleted();
            }
        }

        // ONLY FOR DEV, THEN DELETE THIS CODE!!!!!!!! (ITS FOR BETA TESTING)
        if(other.gameObject.CompareTag("BetaRemover"))
        {
            PlayerPrefs.DeleteAll(); PlayerPrefs.Save(); Debug.Log("All data has been deleted!!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    public void InteractuarCaballo(){
        if(interactuando || !canInteract) return;
        interactuando = true;
        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor
        Cursor.visible = true; // Hacer visible el cursor
    } 

    public void DeInteractuarCaballo(){
        interactuando = false;
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        Cursor.visible = false; // Hacer invisible el cursor
        canInteract = false;
    }

    public void OpenInteractionCanvas()
    {
        if (interactionCanvas != null)
        {
            interactuandoCanvas = true;
            interactionCanvas.OpenMenu();
            Time.timeScale = 0f; // Pausar el juego
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
            Time.timeScale = 1f; // Reanudar el juego
        }
        else
        {
            Debug.LogWarning("Interaction canvas not assigned.");
        }
    }

    public bool CanInteract(){
        return !interactuando && canInteract;
    }
}
