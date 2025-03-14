using UnityEngine;

public class HammerLogic : MonoBehaviour
{
  private void OnCollisionEnter(Collision collision)
  {
      if (collision.gameObject.CompareTag("BreakableWall") && collision.gameObject.activeInHierarchy)
      {
          Debug.Log("Impacto con pared activa, destruyendo...");
          Destroy(collision.gameObject);
      }
  }
}
