using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public Vector3 initialPosition;
    public AudioClip wallCollisionSound;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;
    private AudioSource audioSource;
    
    void Start()
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
    }

    void Update()
    {
        MovePlayer();
        RotateCamera();
        Jump();
    }

    void MovePlayer()
    {
        float moveSpeedCurrent = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = new Vector3(move.x * moveSpeedCurrent, rb.linearVelocity.y, move.z * moveSpeedCurrent);
        
        rb.linearVelocity = velocity;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
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
    }

    private void OnClissionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LevelLoader"))
        {
            other.gameObject.GetComponent<ChangeScene>().ChangeToScene();
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
    }
}
