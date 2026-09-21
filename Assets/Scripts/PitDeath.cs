using UnityEngine;

public class PitDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellineHealth health = collision.GetComponent<JellineHealth>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
    }
}
