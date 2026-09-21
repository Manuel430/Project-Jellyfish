using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Visualizer")]
    [SerializeField] SpriteRenderer visualSprite;
    [SerializeField] Color defaultColor;
    [SerializeField] Color damageColor;

    [Header("Death")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject victoryPrefab;

    //DeathAnimation

    private void Awake()
    {
        currentHealth = maxHealth;

        if(visualSprite != null)
        {
            visualSprite.color = defaultColor;
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashDamageColor());

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        //Debug
        if(victoryPrefab != null)
        {
            Instantiate(victoryPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    IEnumerator FlashDamageColor()
    {
        if(visualSprite != null)
        {
            visualSprite.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            visualSprite.color = defaultColor;
        }
    }
}
