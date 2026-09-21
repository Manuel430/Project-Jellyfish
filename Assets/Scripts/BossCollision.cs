using UnityEngine;

public class BossCollision : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int bossDamageAmount;
    [SerializeField] int playerDamageAmount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JellineMovement playerMovement = collision.gameObject.GetComponent<JellineMovement>();

            if (playerMovement != null)
            {
                if (playerMovement.IsTwirling())
                {
                    Debug.Log("Player is twirling, so the enemy takes damage instead.");

                    BossHealth bossHealth = GetComponent<BossHealth>();
                    if (bossHealth != null)
                    {
                        Debug.Log("Enemy takes damage: " + bossDamageAmount);
                        bossHealth.TakeDamage(bossDamageAmount);
                    }
                }
                else
                {
                    Debug.Log("Player is not twirling, so the player takes damage.");

                    JellineHealth playerHealth = collision.gameObject.GetComponent<JellineHealth>();
                    if (playerHealth != null)
                    {
                        Debug.Log("Player takes damage: " + playerDamageAmount);
                        playerHealth.TakeDamage(playerDamageAmount);
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
