using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Visualizer")]
    [SerializeField] SpriteRenderer visualSprite;
    [SerializeField] Color defaultColor;
    [SerializeField] Color damageColor;

    //Work on Death Later
    [Header("Death")]
    [SerializeField] GameObject deathEffectPrefab;

    private void Awake()
    {
        currentHealth = maxHealth;

        if(visualSprite != null)
        {
            visualSprite.color = defaultColor;
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        visualSprite.color = defaultColor;
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

    private void Die()
    {
        if(deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
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
