using UnityEngine;

public class JellineHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [Range(0, 10)]
    [SerializeField] int maxHealth;

    //Add Player UI later

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
    }

    private int DamageAmount(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Check on UI later
            health = 0;
            Debug.Log("Jelline is dead");
            //Add Game Over UI later
            //Check on how many Lives left later
        }
        return health;
    }

    private int Heal(int heal)
    {
        Debug.Log("Jelline healed");
        health += heal;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        //Check on UI later

        return health;
    }
}
