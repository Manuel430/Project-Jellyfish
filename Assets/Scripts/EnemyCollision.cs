using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int damageAmount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            JellineMovement playerMovement = collision.gameObject.GetComponent<JellineMovement>();

            if(playerMovement != null)
            {
                if(playerMovement.IsTwirling())
                {
                    Debug.Log("Player is twirling, so the enemy takes damage instead.");

                    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
                    if(enemyHealth != null)
                    {
                        Debug.Log("Enemy takes damage: " + damageAmount);
                        enemyHealth.TakeDamage(damageAmount);
                    }
                }
                else
                {
                    Debug.Log("Player is not twirling, so the player takes damage.");

                    JellineHealth playerHealth = collision.gameObject.GetComponent<JellineHealth>();
                    if(playerHealth != null)
                    {
                        Debug.Log("Player takes damage: " + damageAmount);
                        playerHealth.TakeDamage(damageAmount);
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

                if(collision.transform.position.y < transform.position.y)
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
