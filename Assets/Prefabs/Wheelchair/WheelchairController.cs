using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class WheelchairController : MonoBehaviour
{
    public float moveSpeed = 0.43f;

    [SerializeField] private float turnSpeed = 2.5f;
    [SerializeField] private float slopeResistance = 25;
    [SerializeField] private float stickToGroundForce = 10;

    public AudioClip wallCollisionSound;
    private CharacterController cc;

    private Vector3 velocity;

    private bool previouslyGrounded;
    private Vector3 initialPosition;
    private AudioSource audioSource;

    // Variable para almacenar el martillo
    public GameObject hammer;
    public bool hasHammer = false;

    // Use this for initialization
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
            hammer.SetActive(false); // Asegúrate de que el martillo esté desactivado al inicio
        }
    }

    // Update is called once per frame
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

        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed, 0);

        Vector3 desiredMove = transform.forward * Input.GetAxis("Vertical") * moveSpeed;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, cc.radius, Vector3.down, out hitInfo, cc.height / 2f);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);

        // Slide down slopes
        float hitAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
        float slopeEffect = Mathf.Clamp01((hitAngle - slopeResistance) / cc.slopeLimit);
        Vector3 slideForce = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z) * slopeEffect;
        velocity += Physics.gravity * Time.deltaTime;

        cc.Move((velocity + desiredMove + slideForce) * Time.deltaTime);

        // Renderizar el martillo si ha sido recogido
        if (hasHammer && hammer != null)
        {
            hammer.SetActive(true);
            //hammer.transform.position = transform.position + transform.forward * 0.5f + transform.up * 0.5f;
            //hammer.transform.rotation = transform.rotation;
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
            // Recoger el martillo
            hasHammer = true;
            Debug.Log("Hammer picked up");
			Destroy(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LevelLoader"))
        {
            other.gameObject.GetComponent<ChangeScene>().ChangeToScene();
        }

        // Marca el nivel como completado al tocar un objeto con el tag "LevelCompletion"
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