using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JellineHealth : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] JellineMovement playerMovement;
    [SerializeField] GameObject gameOverUI;

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

    [Header("Health UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;

    [Header("Lives")]
    [Range(0, 99)] [SerializeField] int currentLives;
    [SerializeField] int startingLives;

    [Header("Respawn")]
    [SerializeField] Transform respawnPoint;

    [Header("LivesUI")]
    [SerializeField] Sprite[] livesNumber;
    [SerializeField] GameObject livesNumberFront;
    [SerializeField] GameObject livesNumberBack;

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

    public void LosingLife()
    {
        LoseLife();
    }

    public void GivingLife()
    {
        AddLife();
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        respawnPoint = newPoint;
    }
    #endregion

    private void Awake()
    {
        health = maxHealth;

        UpdateUI();

        currentLives = startingLives;

        UpdateLivesUI();

        gameOverUI.SetActive(false);
    }

    private int DamageAmount(int damage)
    {
        health -= damage;
        UpdateUI();

        if (health <= 0)
        {
            health = 0;
            Debug.Log("Jelline is dead");

            playerMovement.StopMoving();
            playerAnim.PlayAnimGameOver();

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

    private void UpdateLivesUI()
    {
        if(currentLives < 10)
        {
            livesNumberFront.GetComponent<Image>().sprite = livesNumber[0];
            livesNumberBack.GetComponent<Image>().sprite = livesNumber[currentLives];
        }
        else
        {
            int firstDigit = currentLives / 10;
            int secondDigit = currentLives % 10;

            livesNumberFront.GetComponent<Image>().sprite = livesNumber[firstDigit];
            livesNumberBack.GetComponent<Image>().sprite = livesNumber[secondDigit];
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

    private void LoseLife()
    {
        currentLives--;

        if(currentLives < 0)
        {
            Debug.Log("GameOver");
            gameOverUI.SetActive(true);
        }
        else
        {
            UpdateLivesUI();

            playerMovement.TurnOnTransition();

            gameObject.transform.position = respawnPoint.position;
            Heal(maxHealth);
            UpdateUI();
            playerAnim.ResetGameOverTrigger();
            playerAnim.PlayAnimRespawn();

            playerMovement.StartMoving();
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void AddLife()
    {
        currentLives++;
        UpdateLivesUI();
    }
}
