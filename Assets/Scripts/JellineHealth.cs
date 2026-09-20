using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JellineHealth : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] JellineMovement playerMovement;

    [Header("Health")]
    [SerializeField] int health;
    [Range(0, 10)]
    [SerializeField] int maxHealth;

    [Header("Visualizer")]
    [SerializeField] SpriteRenderer visualSprite;
    [SerializeField] Color defaultColor;
    [SerializeField] Color damageColor;

    [Header("Animator")]
    [SerializeField] JellineAnimation playerAnim;

    [Header("UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;

    #region Public Methods
    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        DamageAmount(damage);
    }

    public void TakeHeal(int heal)
    {
        Heal(heal);
    }
    #endregion

    private void Awake()
    {
        health = maxHealth;

        UpdateUI();
    }

    private int DamageAmount(int damage)
    {
        health -= damage;
        UpdateUI();

        if (health <= 0)
        {
            //Check on UI later
            health = 0;
            Debug.Log("Jelline is dead");

            playerMovement.StopMoving();
            playerAnim.PlayAnimGameOver();

            //Add Game Over UI later
            //Check on how many Lives left later
        }
        else
        {
            playerAnim.PlayAnimHit();
            StartCoroutine(FlashDamageColor());
        }
            return health;
    }

    private int Heal(int heal)
    {
        Debug.Log("Jelline healed");
        health += heal;
        UpdateUI();

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        //Check on UI later

        return health;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
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
