using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] GameObject victoryItem;

    [Header("Animation")]
    [SerializeField] Animator bossAnimator;

    [Header("Boss")]
    [SerializeField] MsPuffer msPuffer;

    [Header("UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;

    [Header("Audio")]
    [SerializeField] AudioSource musicManager;

    private void Awake()
    {
        currentHealth = maxHealth;

        if(visualSprite != null)
        {
            visualSprite.color = defaultColor;
        }

        UpdateUI();

        victoryItem.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashDamageColor());

        currentHealth -= damage;

        UpdateUI();

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        if (msPuffer != null)
        {
            msPuffer.StopMoving();
        }
        if (victoryItem != null)
        {
            victoryItem.SetActive(true);
        }

        musicManager.Stop();

        bossAnimator.SetTrigger("Dead");
    }

    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
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
