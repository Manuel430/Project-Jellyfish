using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [Header("Attack Types")]
    [SerializeField] int chooseAttack;

    [Header("Cooldown")]
    [SerializeField] float cooldownTime;
    float cooldownTimer;

    public void ResetTimer()
    {
        cooldownTimer = Random.Range(1, cooldownTimer);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
        Debug.Log("Choosing Attack");

        chooseAttack = Random.Range(0, chooseAttack);

        switch(chooseAttack)
        {
            case 0:
                Attack01();
                break;
            case 1:
                Attack02();
                break;
            default:
                NoAttack();
                break;
        }
    }

    public virtual void Attack01()
    {
        Debug.Log("I'm doing my first attack");
    }

    public virtual void Attack02()
    {
        Debug.Log("I'm doing my second attack");
    }

    private void NoAttack()
    {
        Debug.Log("Can't attack, wait another turn");
    }
}
