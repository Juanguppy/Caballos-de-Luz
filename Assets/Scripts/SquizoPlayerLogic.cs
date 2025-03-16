using UnityEngine;

public class SquizoPlayerLogic : PlayerLogic
{
    protected override void OnCollisionEnter(Collision collision){
        base.OnCollisionEnter(collision);

        // se puede chocar con un muro que no ve, y lo hace visible
        if(collision.gameObject.CompareTag("SquizoWall")){
            Debug.Log("SquizoWall collision");
            audioSource.PlayOneShot(wallCollisionSound);
            MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            if(meshRenderer == null) return; 
            meshRenderer.enabled = true;
        }

        // un muro falso 
        if(
           collision.gameObject.CompareTag("FakeWall")  ||
           collision.gameObject.CompareTag("FakeHorse") || 
           collision.gameObject.CompareTag("FakeEnemy")
           )
        {
            // transform.position = initialPosition;
            Destroy(collision.gameObject);
        }
    }
}
