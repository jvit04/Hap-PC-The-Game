using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
