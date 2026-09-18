using UnityEngine;

public class NeedleProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] float speed;
    [SerializeField] int damage;
    Rigidbody2D rb;

    [Header("Duration")]
    [SerializeField] float duration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            JellineMovement playerMovement = collision.gameObject.GetComponent<JellineMovement>();

            if (playerMovement != null)
            {
                if (playerMovement.IsTwirling())
                {
                    Debug.Log("Player is twirling, so the enemy takes damage instead.");

                    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        Debug.Log("Enemy takes damage: " + damage);
                        enemyHealth.TakeDamage(damage);
                    }
                }
                else
                {
                    Debug.Log("Player is not twirling, so the player takes damage.");

                    JellineHealth playerHealth = collision.gameObject.GetComponent<JellineHealth>();
                    if (playerHealth != null)
                    {
                        Debug.Log("Player takes damage: " + damage);
                        playerHealth.TakeDamage(damage);
                    }
                }

                playerMovement.SetKBTimer();
                if (collision.transform.position.x < transform.position.x)
                {
                    playerMovement.SetKnockbackFromRight(true);
                }
                else
                {
                    playerMovement.SetKnockbackFromRight(false);
                }

                if (collision.transform.position.y < transform.position.y)
                {
                    playerMovement.SetKnockbackFromTop(true);
                }
                else
                {
                    playerMovement.SetKnockbackFromTop(false);
                }
            }
        }
    }
}