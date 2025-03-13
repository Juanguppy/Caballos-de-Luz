using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WheelchairController player = collision.gameObject.GetComponent<WheelchairController>();
            if (player != null && player.hasHammer)
            {
                Destroy(gameObject);
                Debug.Log("Breakable wall destroyed by hammer.");
            }
        }
    }
}